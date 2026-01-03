using ApiDocGen.Core.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ApiDocGen.Scanner.Roslyn;

public static class RoslynControllerScanner
{
    public static List<ControllerSpec> ScanControllers(string repoRoot)
    {
        // Common pattern: <Project>/Controllers/*.cs
        var controllerFiles = Directory.EnumerateFiles(repoRoot, "*.cs", SearchOption.AllDirectories)
            .Where(p => p.EndsWith("Controller.cs", StringComparison.OrdinalIgnoreCase)
                        || p.Contains($"{Path.DirectorySeparatorChar}Controllers{Path.DirectorySeparatorChar}", StringComparison.OrdinalIgnoreCase))
            .ToList();

        var controllers = new List<ControllerSpec>();
        var idx = RepoTypeIndex.Build(repoRoot);

        foreach (var file in controllerFiles)
        {
            var code = File.ReadAllText(file);
            var tree = CSharpSyntaxTree.ParseText(code);
            var root = tree.GetCompilationUnitRoot();

            foreach (var classNode in root.DescendantNodes().OfType<ClassDeclarationSyntax>())
            {
                if (!LooksLikeController(classNode)) continue;

                var controllerName = classNode.Identifier.Text;
                var routePrefix = GetRouteTemplate(classNode.AttributeLists);

                var endpoints = new List<EndpointSpec>();

                foreach (var method in classNode.Members.OfType<MethodDeclarationSyntax>())
                {
                    var http = GetHttpMethod(method.AttributeLists);
                    if (http is null) continue;

                    var methodTemplate = GetHttpTemplate(method.AttributeLists); // e.g. "{id}"
                    var route = CombineRoutes(routePrefix, methodTemplate);

                    var parameters = new List<ParameterSpec>();
                    string? bodyType = null;

                    foreach (var p in method.ParameterList.Parameters)
                    {
                        var pName = p.Identifier.Text;
                        var pType = p.Type?.ToString() ?? "object";
                        var source = GetParamSource(p.AttributeLists);

                        if (source == "body") bodyType = pType;

                        parameters.Add(new ParameterSpec(
                            Name: pName,
                            Type: pType,
                            Source: source
                        ));
                    }

                    // Request body schema (FromBody)
                    var bodyParam = method.ParameterList.Parameters.FirstOrDefault(p =>
                        p.AttributeLists.SelectMany(a => a.Attributes)
                            .Any(a => a.Name.ToString().Contains("FromBody", StringComparison.OrdinalIgnoreCase)));

                    TypeSchema? requestSchema = null;
                    if (bodyParam?.Type is not null)
                    {
                        requestSchema = SchemaExtractor.TryBuildSchema(bodyParam.Type.ToString(), idx);
                    }

                    // Response schema (best-effort from return type)
                    var responseBodyType = ReturnTypeParser.TryExtractResponseBodyType(method.ReturnType.ToString());

                    TypeSchema? responseSchema = null;
                    if (!string.IsNullOrWhiteSpace(responseBodyType))
                    {
                        responseSchema = SchemaExtractor.TryBuildSchema(responseBodyType, idx);
                    }

                    var responses = new List<ResponseSpec>
                    {
                        new ResponseSpec(200, responseSchema)
                    };



                    endpoints.Add(new EndpointSpec(
                        HttpMethod: http,
                        Route: NormalizeRoute(route, controllerName),
                        Action: method.Identifier.Text,
                        Parameters: parameters,
                        RequestBodySchema: requestSchema,
                        Responses: responses
                    ));

                }

                if (endpoints.Count > 0)
                {
                    controllers.Add(new ControllerSpec(
                        Name: controllerName,
                        RoutePrefix: routePrefix,
                        Endpoints: endpoints
                            .OrderBy(e => e.Route)
                            .ThenBy(e => e.HttpMethod)
                            .ToList()
                    ));
                }
            }
        }

        // de-dupe by name (if partial classes etc.)
        return controllers
            .GroupBy(c => c.Name)
            .Select(g => new ControllerSpec(
                Name: g.Key,
                RoutePrefix: g.Select(x => x.RoutePrefix).FirstOrDefault(x => !string.IsNullOrWhiteSpace(x)),
                Endpoints: g.SelectMany(x => x.Endpoints).Distinct().ToList()
            ))
            .OrderBy(c => c.Name)
            .ToList();
    }

    private static bool LooksLikeController(ClassDeclarationSyntax cls)
    {
        var name = cls.Identifier.Text;
        if (name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase)) return true;

        // Also allow [ApiController]
        return cls.AttributeLists
            .SelectMany(a => a.Attributes)
            .Any(a => a.Name.ToString().EndsWith("ApiController", StringComparison.OrdinalIgnoreCase)
                   || a.Name.ToString().EndsWith("ApiControllerAttribute", StringComparison.OrdinalIgnoreCase));
    }

    private static string? GetRouteTemplate(SyntaxList<AttributeListSyntax> attrs)
    {
        // [Route("api/[controller]")]
        foreach (var a in attrs.SelectMany(x => x.Attributes))
        {
            var n = a.Name.ToString();
            if (n.EndsWith("Route", StringComparison.OrdinalIgnoreCase) || n.EndsWith("RouteAttribute", StringComparison.OrdinalIgnoreCase))
            {
                var arg = a.ArgumentList?.Arguments.FirstOrDefault()?.ToString();
                return StripQuotes(arg);
            }
        }
        return null;
    }

    private static string? GetHttpMethod(SyntaxList<AttributeListSyntax> attrs)
    {
        foreach (var a in attrs.SelectMany(x => x.Attributes))
        {
            var n = a.Name.ToString();
            if (EndsWithAny(n, "HttpGet", "HttpGetAttribute")) return "GET";
            if (EndsWithAny(n, "HttpPost", "HttpPostAttribute")) return "POST";
            if (EndsWithAny(n, "HttpPut", "HttpPutAttribute")) return "PUT";
            if (EndsWithAny(n, "HttpDelete", "HttpDeleteAttribute")) return "DELETE";
            if (EndsWithAny(n, "HttpPatch", "HttpPatchAttribute")) return "PATCH";
        }
        return null;
    }

    private static string? GetHttpTemplate(SyntaxList<AttributeListSyntax> attrs)
    {
        // [HttpGet("{id}")]
        foreach (var a in attrs.SelectMany(x => x.Attributes))
        {
            var n = a.Name.ToString();
            if (!n.StartsWith("Http", StringComparison.OrdinalIgnoreCase)) continue;

            var arg = a.ArgumentList?.Arguments.FirstOrDefault()?.ToString();
            return StripQuotes(arg);
        }
        return null;
    }

    private static string GetParamSource(SyntaxList<AttributeListSyntax> attrs)
    {
        var names = attrs.SelectMany(x => x.Attributes).Select(a => a.Name.ToString()).ToList();
        if (names.Any(n => EndsWithAny(n, "FromBody", "FromBodyAttribute"))) return "body";
        if (names.Any(n => EndsWithAny(n, "FromRoute", "FromRouteAttribute"))) return "route";
        if (names.Any(n => EndsWithAny(n, "FromQuery", "FromQueryAttribute"))) return "query";
        return "unknown";
    }

    private static string CombineRoutes(string? prefix, string? template)
    {
        prefix = (prefix ?? "").Trim();
        template = (template ?? "").Trim();

        if (string.IsNullOrWhiteSpace(prefix)) return template;
        if (string.IsNullOrWhiteSpace(template)) return prefix;

        return $"{prefix.TrimEnd('/')}/{template.TrimStart('/')}";
    }

    private static string NormalizeRoute(string? route, string controllerName)
    {
        var r = (route ?? "").Trim();
        if (string.IsNullOrWhiteSpace(r)) r = "/";

        // Replace [controller] token with controller name sans "Controller"
        var tokenValue = controllerName.EndsWith("Controller", StringComparison.OrdinalIgnoreCase)
            ? controllerName[..^"Controller".Length]
            : controllerName;

        r = r.Replace("[controller]", tokenValue, StringComparison.OrdinalIgnoreCase);

        if (!r.StartsWith("/")) r = "/" + r;
        while (r.Contains("//")) r = r.Replace("//", "/");

        return r;
    }

    private static bool EndsWithAny(string value, params string[] suffixes)
        => suffixes.Any(s => value.EndsWith(s, StringComparison.OrdinalIgnoreCase));

    private static string? StripQuotes(string? s)
    {
        if (string.IsNullOrWhiteSpace(s)) return null;
        s = s.Trim();
        if (s.StartsWith("\"") && s.EndsWith("\"") && s.Length >= 2)
            return s[1..^1];
        return s;
    }
}
