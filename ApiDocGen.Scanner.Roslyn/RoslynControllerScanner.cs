using ApiDocGen.Core.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ApiDocGen.Scanner.Roslyn;

public static class RoslynControllerScanner
{
    public static List<ControllerSpec> ScanControllers(string repoRoot)
    {
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

                var isApiController = classNode.AttributeLists
                    .SelectMany(a => a.Attributes)
                    .Any(a => a.Name.ToString().Contains("ApiController", StringComparison.OrdinalIgnoreCase));

                var endpoints = new List<EndpointSpec>();

                foreach (var method in classNode.Members.OfType<MethodDeclarationSyntax>())
                {
                    var http = GetHttpMethod(method.AttributeLists);
                    if (http is null) continue;

                    var endpointKind = isApiController ? "api" : "mvc";

                    var methodTemplate = GetHttpTemplate(method.AttributeLists);
                    var route = CombineRoutes(routePrefix, methodTemplate);

                    var parameters = new List<ParameterSpec>();

                    foreach (var p in method.ParameterList.Parameters)
                    {
                        parameters.Add(new ParameterSpec(
                            Name: p.Identifier.Text,
                            Type: p.Type?.ToString() ?? "object",
                            Source: GetParamSource(p.AttributeLists, endpointKind)
                        ));
                    }

                    // -------- REQUEST SCHEMA --------
                    TypeSchema? requestSchema = null;

                    // Explicit [FromBody] or [FromForm]
                    var explicitBody = method.ParameterList.Parameters.FirstOrDefault(p =>
                        p.AttributeLists.SelectMany(a => a.Attributes)
                            .Any(a => a.Name.ToString().Contains("FromBody", StringComparison.OrdinalIgnoreCase)
                                   || a.Name.ToString().Contains("FromForm", StringComparison.OrdinalIgnoreCase)));

                    if (explicitBody?.Type is not null)
                    {
                        requestSchema = SchemaExtractor.TryBuildSchema(explicitBody.Type.ToString(), idx);
                    }
                    else
                    {
                        // Default complex model
                        var complexParam = method.ParameterList.Parameters
                            .FirstOrDefault(p => p.Type is not null && !IsSimpleType(p.Type.ToString()));

                        if (complexParam?.Type is not null)
                        {
                            requestSchema = SchemaExtractor.TryBuildSchema(complexParam.Type.ToString(), idx);
                        }
                    }

                    // -------- RESPONSES --------
                    var responses = new List<ResponseSpec>();

                    if (endpointKind == "api")
                    {
                        // Ok(new Response<T>(...))
                        var t = ResponseInference.TryInferOkResponseModelType(method);

                        TypeSchema? jsonSchema = null;
                        if (!string.IsNullOrWhiteSpace(t))
                        {
                            jsonSchema = SchemaExtractor.TryBuildSchema(t!, idx);
                        }

                        responses.Add(new ResponseSpec(
                            StatusCode: 200,
                            Kind: "json",
                            JsonBodySchema: jsonSchema,
                            ViewModelSchema: null
                        ));
                    }
                    else // MVC
                    {
                        // View(model)
                        var (vmType, isView) = MvcResponseInference.TryInferViewModel(method);
                        if (isView)
                        {
                            responses.Add(new ResponseSpec(
                                StatusCode: 200,
                                Kind: "view",
                                JsonBodySchema: null,
                                ViewModelSchema: requestSchema
                            ));
                        }

                        // Redirect
                        if (MvcResponseInference.HasRedirect(method))
                        {
                            responses.Add(new ResponseSpec(
                                StatusCode: 302,
                                Kind: "redirect",
                                JsonBodySchema: null,
                                ViewModelSchema: null
                            ));
                        }

                        if (responses.Count == 0)
                        {
                            responses.Add(new ResponseSpec(
                                StatusCode: 200,
                                Kind: "unknown",
                                JsonBodySchema: null,
                                ViewModelSchema: null
                            ));
                        }
                    }

                    endpoints.Add(new EndpointSpec(
                        Kind: endpointKind,
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

    // ---------- HELPERS ----------

    private static bool LooksLikeController(ClassDeclarationSyntax cls)
    {
        var name = cls.Identifier.Text;
        if (name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase)) return true;

        return cls.AttributeLists
            .SelectMany(a => a.Attributes)
            .Any(a => a.Name.ToString().Contains("ApiController", StringComparison.OrdinalIgnoreCase));
    }

    private static string? GetRouteTemplate(SyntaxList<AttributeListSyntax> attrs)
    {
        foreach (var a in attrs.SelectMany(x => x.Attributes))
        {
            if (a.Name.ToString().Contains("Route", StringComparison.OrdinalIgnoreCase))
            {
                return StripQuotes(a.ArgumentList?.Arguments.FirstOrDefault()?.ToString());
            }
        }
        return null;
    }

    private static string? GetHttpMethod(SyntaxList<AttributeListSyntax> attrs)
    {
        foreach (var a in attrs.SelectMany(x => x.Attributes))
        {
            var n = a.Name.ToString();
            if (n.Contains("HttpGet")) return "GET";
            if (n.Contains("HttpPost")) return "POST";
            if (n.Contains("HttpPut")) return "PUT";
            if (n.Contains("HttpDelete")) return "DELETE";
            if (n.Contains("HttpPatch")) return "PATCH";
        }
        return null;
    }

    private static string? GetHttpTemplate(SyntaxList<AttributeListSyntax> attrs)
    {
        foreach (var a in attrs.SelectMany(x => x.Attributes))
        {
            if (!a.Name.ToString().StartsWith("Http", StringComparison.OrdinalIgnoreCase)) continue;
            return StripQuotes(a.ArgumentList?.Arguments.FirstOrDefault()?.ToString());
        }
        return null;
    }

    private static string GetParamSource(SyntaxList<AttributeListSyntax> attrs, string kind)
    {
        var names = attrs.SelectMany(x => x.Attributes).Select(a => a.Name.ToString()).ToList();

        if (names.Any(n => n.Contains("FromBody"))) return "body";
        if (names.Any(n => n.Contains("FromRoute"))) return "route";
        if (names.Any(n => n.Contains("FromQuery"))) return "query";
        if (names.Any(n => n.Contains("FromForm"))) return "form";

        return kind == "api" ? "body" : "form";
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

        var tokenValue = controllerName.EndsWith("Controller", StringComparison.OrdinalIgnoreCase)
            ? controllerName[..^"Controller".Length]
            : controllerName;

        r = r.Replace("[controller]", tokenValue, StringComparison.OrdinalIgnoreCase);

        if (!r.StartsWith("/")) r = "/" + r;
        while (r.Contains("//")) r = r.Replace("//", "/");

        return r;
    }

    private static bool IsSimpleType(string typeName)
    {
        typeName = typeName.Trim().TrimEnd('?');
        return typeName is "string" or "int" or "long" or "short" or "byte"
            or "bool" or "double" or "float" or "decimal"
            or "DateTime" or "Guid";
    }

    private static string? StripQuotes(string? s)
    {
        if (string.IsNullOrWhiteSpace(s)) return null;
        s = s.Trim();
        return s.StartsWith("\"") && s.EndsWith("\"") ? s[1..^1] : s;
    }
}
