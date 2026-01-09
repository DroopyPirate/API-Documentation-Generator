using System.Diagnostics;

namespace ApiDocGen.Git;

public static class GitClient
{
    // Backward-compatible overload (public repos)
    public static Task CloneAsync(string repoUrl, string targetDir, CancellationToken ct = default)
        => CloneAsync(repoUrl, targetDir, githubToken: null, ct);

    // New overload: supports private repos via GitHub token (PAT)
    public static async Task CloneAsync(string repoUrl, string targetDir, string? githubToken, CancellationToken ct = default)
    {
        Directory.CreateDirectory(targetDir);

        var cloneUrl = repoUrl;

        // If token provided, inject into HTTPS GitHub URL (for private repo access)
        if (!string.IsNullOrWhiteSpace(githubToken))
        {
            cloneUrl = TryInjectGithubToken(repoUrl, githubToken) ?? repoUrl;
        }

        // git clone --depth 1 <repoUrl> <targetDir>
        await RunAsync("git", $"clone --depth 1 \"{cloneUrl}\" \"{targetDir}\"", ct, redactArgs: true);
    }

    public static async Task<string> GetHeadCommitAsync(string repoDir, CancellationToken ct = default)
    {
        // git rev-parse HEAD
        var output = await RunCaptureAsync("git", "rev-parse HEAD", repoDir, ct);
        return output.Trim();
    }

    private static string? TryInjectGithubToken(string repoUrl, string token)
    {
        // Only handle https://github.com/... URLs
        if (!Uri.TryCreate(repoUrl, UriKind.Absolute, out var uri))
            return null;

        if (!uri.Scheme.Equals("https", StringComparison.OrdinalIgnoreCase))
            return null;

        // Support github.com and www.github.com
        var host = uri.Host;
        if (!host.Equals("github.com", StringComparison.OrdinalIgnoreCase) &&
            !host.Equals("www.github.com", StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        // Inject token as userinfo: https://<token>@github.com/owner/repo.git
        // Important: do NOT URL-encode token here; git handles raw PATs fine.
        return $"https://{token}@{uri.Host}{uri.PathAndQuery}";
    }

    private static async Task RunAsync(string fileName, string args, CancellationToken ct, bool redactArgs)
    {
        var psi = new ProcessStartInfo(fileName, args)
        {
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false
        };

        using var p = Process.Start(psi) ?? throw new InvalidOperationException("Failed to start process");
        var stdOut = await p.StandardOutput.ReadToEndAsync(ct);
        var stdErr = await p.StandardError.ReadToEndAsync(ct);

        await p.WaitForExitAsync(ct);
        if (p.ExitCode != 0)
        {
            // If args might contain token, don't print them
            var safeArgs = redactArgs ? "[REDACTED]" : args;
            throw new Exception($"Command failed: {fileName} {safeArgs}\n{stdErr}\n{stdOut}");
        }
    }

    private static async Task<string> RunCaptureAsync(string fileName, string args, string workDir, CancellationToken ct)
    {
        var psi = new ProcessStartInfo(fileName, args)
        {
            WorkingDirectory = workDir,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false
        };

        using var p = Process.Start(psi) ?? throw new InvalidOperationException("Failed to start process");
        var stdOut = await p.StandardOutput.ReadToEndAsync(ct);
        var stdErr = await p.StandardError.ReadToEndAsync(ct);

        await p.WaitForExitAsync(ct);
        if (p.ExitCode != 0)
            throw new Exception($"Command failed: {fileName} {args}\n{stdErr}\n{stdOut}");

        return stdOut;
    }
}
