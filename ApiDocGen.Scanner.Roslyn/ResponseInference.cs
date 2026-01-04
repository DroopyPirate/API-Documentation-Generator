using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ApiDocGen.Scanner.Roslyn;

internal static class ResponseInference
{
    /// <summary>
    /// Tries to infer T from patterns like:
    ///   return Ok(new Response<T>(...));
    ///   return BadRequest(new Response<T>(...));
    ///   return NotFound(new Response<T>(...));
    /// </summary>
    public static string? TryInferOkResponseModelType(MethodDeclarationSyntax method)
    {
        foreach (var ret in method.DescendantNodes().OfType<ReturnStatementSyntax>())
        {
            if (ret.Expression is not InvocationExpressionSyntax invoke)
                continue;

            // Ok(...), BadRequest(...), NotFound(...), etc.
            var invokedName = invoke.Expression.ToString();
            if (!IsSupportedResult(invokedName))
                continue;

            if (invoke.ArgumentList.Arguments.Count == 0)
                continue;

            var firstArg = invoke.ArgumentList.Arguments[0].Expression;

            // new Response<T>(...)
            if (firstArg is ObjectCreationExpressionSyntax obj &&
                TryGetResponseGenericType(obj, out var t))
            {
                return t;
            }

            // Sometimes you might see: Ok(Response<T>.Create(...)) etc. (skip for MVP)
        }

        return null;
    }

    private static bool TryGetResponseGenericType(ObjectCreationExpressionSyntax obj, out string? t)
    {
        t = null;

        // If "Response<T>" is written as generic name directly
        if (obj.Type is GenericNameSyntax g &&
            g.Identifier.Text == "Response" &&
            g.TypeArgumentList.Arguments.Count == 1)
        {
            t = g.TypeArgumentList.Arguments[0].ToString();
            return true;
        }

        // If written as qualified generic: Namespace.Response<T>
        if (obj.Type is QualifiedNameSyntax q &&
            q.Right is GenericNameSyntax g2 &&
            g2.Identifier.Text == "Response" &&
            g2.TypeArgumentList.Arguments.Count == 1)
        {
            t = g2.TypeArgumentList.Arguments[0].ToString();
            return true;
        }

        return false;
    }

    private static bool IsSupportedResult(string invokedName)
    {
        // Covers Ok(...), ControllerBase.Ok(...), Results.Ok(...), etc.
        return invokedName.EndsWith("Ok", StringComparison.OrdinalIgnoreCase)
            || invokedName.EndsWith("BadRequest", StringComparison.OrdinalIgnoreCase)
            || invokedName.EndsWith("NotFound", StringComparison.OrdinalIgnoreCase)
            || invokedName.EndsWith("Unauthorized", StringComparison.OrdinalIgnoreCase)
            || invokedName.EndsWith("Forbid", StringComparison.OrdinalIgnoreCase);
    }
}
