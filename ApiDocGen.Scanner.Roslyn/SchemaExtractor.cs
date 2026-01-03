using ApiDocGen.Core.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ApiDocGen.Scanner.Roslyn;

internal static class SchemaExtractor
{
    public static TypeSchema? TryBuildSchema(string typeName, RepoTypeIndex idx)
    {
        var cleaned = CleanTypeName(typeName);

        if (!idx.ClassesByName.TryGetValue(cleaned, out var cls))
            return null;

        var props = cls.Members
            .OfType<PropertyDeclarationSyntax>()
            .Select(p =>
            {
                var propName = p.Identifier.Text;
                var propType = p.Type.ToString();

                var attrs = p.AttributeLists
                    .SelectMany(a => a.Attributes)
                    .Select(a => a.Name.ToString())
                    .ToList();

                var required = attrs.Any(a =>
                    a.EndsWith("Required", StringComparison.OrdinalIgnoreCase) ||
                    a.EndsWith("RequiredAttribute", StringComparison.OrdinalIgnoreCase));

                return new PropertySchema(
                    Name: propName,
                    Type: propType,
                    IsRequired: required,
                    Attributes: attrs
                );
            })
            .ToList();

        return new TypeSchema(cleaned, props);
    }

    private static string CleanTypeName(string t)
    {
        t = t.Trim();

        // remove nullable '?'
        if (t.EndsWith("?", StringComparison.Ordinal)) t = t.TrimEnd('?');

        // remove namespace Foo.Bar.Baz -> Baz
        var lastDot = t.LastIndexOf('.');
        if (lastDot >= 0) t = t[(lastDot + 1)..];

        // remove generic Response<T> -> Response
        var lt = t.IndexOf('<');
        if (lt >= 0) t = t[..lt];

        return t.Trim();
    }
}
