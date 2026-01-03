using System.Diagnostics;

namespace ApiDocGen.Git;

public static class GitClient
{
    public static async Task CloneAsync(string repoUrl, string targetDir, CancellationToken ct = default)
    {
        Directory.CreateDirectory(targetDir);

        // git clone --depth 1 <repoUrl> <targetDir>
        await RunAsync("git", $"clone --depth 1 \"{repoUrl}\" \"{targetDir}\"", ct);
    }

    public static async Task<string> GetHeadCommitAsync(string repoDir, CancellationToken ct = default)
    {
        // git rev-parse HEAD
        var output = await RunCaptureAsync("git", "rev-parse HEAD", repoDir, ct);
        return output.Trim();
    }

    private static async Task RunAsync(string fileName, string args, CancellationToken ct)
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
            throw new Exception($"Command failed: {fileName} {args}\n{stdErr}\n{stdOut}");
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
