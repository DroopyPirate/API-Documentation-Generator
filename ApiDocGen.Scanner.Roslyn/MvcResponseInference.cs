using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ApiDocGen.Scanner.Roslyn;

internal static class MvcResponseInference
{
    /// <summary>
    /// Detects: return View(model) or return View()
    /// </summary>
    public static (string? ModelType, bool IsView) TryInferViewModel(MethodDeclarationSyntax method)
    {
        foreach (var ret in method.DescendantNodes().OfType<ReturnStatementSyntax>())
        {
            if (ret.Expression is not InvocationExpressionSyntax invoke)
                continue;

            // View(...)
            if (!invoke.Expression.ToString().EndsWith("View", StringComparison.OrdinalIgnoreCase))
                continue;

            // View(model)
            if (invoke.ArgumentList.Arguments.Count == 1)
            {
                var arg = invoke.ArgumentList.Arguments[0].Expression.ToString();

                // Common MVC pattern: View(model)
                if (arg == "model")
                {
                    // Signal: reuse request schema
                    return ("<REQUEST_MODEL>", true);
                }
            }

            // View() without model
            return (null, true);
        }

        return (null, false);
    }

    /// <summary>
    /// Detects: RedirectToAction / Redirect / RedirectToRoute
    /// </summary>
    public static bool HasRedirect(MethodDeclarationSyntax method)
    {
        return method.DescendantNodes()
            .OfType<ReturnStatementSyntax>()
            .Any(r =>
                r.Expression is InvocationExpressionSyntax inv &&
                inv.Expression.ToString().Contains("Redirect", StringComparison.OrdinalIgnoreCase));
    }
}
