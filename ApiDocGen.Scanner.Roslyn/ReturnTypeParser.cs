namespace ApiDocGen.Scanner.Roslyn;

internal static class ReturnTypeParser
{
    public static string? TryExtractResponseBodyType(string returnTypeText)
    {
        var t = returnTypeText.Replace(" ", "");

        // unwrap Task<>
        t = UnwrapOuter(t, "Task<");

        // unwrap ActionResult<>
        t = UnwrapOuter(t, "ActionResult<");

        // unwrap Response<>
        var inner = UnwrapOuter(t, "Response<");
        if (inner != t) t = inner;

        // if still generic, take innermost
        var lastLt = t.LastIndexOf('<');
        var lastGt = t.LastIndexOf('>');
        if (lastLt >= 0 && lastGt > lastLt)
            t = t.Substring(lastLt + 1, lastGt - lastLt - 1);

        return string.IsNullOrWhiteSpace(t) ? null : t;
    }

    private static string UnwrapOuter(string text, string outerPrefix)
    {
        if (!text.StartsWith(outerPrefix, StringComparison.Ordinal)) return text;
        if (!text.EndsWith(">")) return text;

        return text.Substring(outerPrefix.Length, text.Length - outerPrefix.Length - 1);
    }
}
