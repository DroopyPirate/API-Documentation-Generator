using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ApiDocGen.Scanner.Roslyn;

internal sealed class RepoTypeIndex
{
    public Dictionary<string, ClassDeclarationSyntax> ClassesByName { get; } = new(StringComparer.Ordinal);

    public static RepoTypeIndex Build(string repoRoot)
    {
        var idx = new RepoTypeIndex();

        var csFiles = Directory.EnumerateFiles(repoRoot, "*.cs", SearchOption.AllDirectories);

        foreach (var file in csFiles)
        {
            var code = File.ReadAllText(file);
            var tree = CSharpSyntaxTree.ParseText(code);
            var root = tree.GetCompilationUnitRoot();

            foreach (var cls in root.DescendantNodes().OfType<ClassDeclarationSyntax>())
            {
                var name = cls.Identifier.Text;
                if (!idx.ClassesByName.ContainsKey(name))
                    idx.ClassesByName[name] = cls;
            }
        }

        return idx;
    }
}
