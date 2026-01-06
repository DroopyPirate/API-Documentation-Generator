using System.Text.Json;
using ApiDocGen.AI.Gemini;
using ApiDocGen.Core.AI;              // <- adjust if your PromptBuilder namespace differs
using ApiDocGen.Core.Models;
using ApiDocGen.Core.Serialization;
using ApiDocGen.Git;
using ApiDocGen.Scanner.Roslyn;

static string? Arg(string[] args, string name)
{
    var i = Array.FindIndex(args, a => string.Equals(a, name, StringComparison.OrdinalIgnoreCase));
    return (i >= 0 && i + 1 < args.Length) ? args[i + 1] : null;
}

static void PrintUsage()
{
    Console.WriteLine(
@"Usage:
  dotnet run -- --repo <github_url> --out <output_dir> [--gemini-model gemini-2.5-flash]

Environment:
  GEMINI_API_KEY must be set (recommended)

Optional (local testing only):
  --gemini-key <api_key>

Examples:
  $env:GEMINI_API_KEY=""YOUR_KEY""
  dotnet run -- --repo https://github.com/user/repo --out .\docs --gemini-model gemini-2.5-flash
");
}

var repoUrl = Arg(args, "--repo");
if (string.IsNullOrWhiteSpace(repoUrl))
{
    PrintUsage();
    return;
}

var outDir = Arg(args, "--out") ?? "./docs";
var geminiModel = Arg(args, "--gemini-model") ?? "gemini-2.5-flash";

// Prefer env var; allow CLI override for local testing
var geminiKey = Environment.GetEnvironmentVariable("GEMINI_API_KEY")
              ?? Arg(args, "--gemini-key");

if (string.IsNullOrWhiteSpace(geminiKey))
{
    Console.WriteLine("❌ Missing GEMINI_API_KEY. Set env var or pass --gemini-key for local testing.");
    return;
}

Directory.CreateDirectory(outDir);

// 1) Clone repo to temp
var tempRoot = Path.Combine(Path.GetTempPath(), "ApiDocGenFromSource", Guid.NewGuid().ToString("N"));
Console.WriteLine($"Cloning repo to: {tempRoot}");
await GitClient.CloneAsync(repoUrl, tempRoot);

// 2) Commit hash
var commit = await GitClient.GetHeadCommitAsync(tempRoot);

// 3) Scan controllers from source
Console.WriteLine("Scanning controllers from source...");
var controllers = RoslynControllerScanner.ScanControllers(tempRoot);

var spec = new ApiSpec(
    RepoUrl: repoUrl,
    RepoCommit: commit,
    Controllers: controllers
);

// 4) Write api.json
var jsonPath = Path.Combine(outDir, "api.json");
await File.WriteAllTextAsync(jsonPath, JsonSerializer.Serialize(spec, JsonDefaults.Options));
Console.WriteLine($"Wrote: {jsonPath}");

// 5) Ask Gemini for Markdown
Console.WriteLine($"Calling Gemini model '{geminiModel}'...");
var prompt = PromptBuilder.Build(spec); // keep same Build(...) signature you had

using var http = new HttpClient();
var gemini = new GeminiClient(http, geminiKey!, geminiModel);

var md = await gemini.GenerateTextAsync(prompt);

// (Optional) guardrail: Gemini sometimes returns ```markdown fences; remove if you want raw
md = StripWrappingMarkdownFence(md);

// 6) Write api.md
var mdPath = Path.Combine(outDir, "api.md");
await File.WriteAllTextAsync(mdPath, md);
Console.WriteLine($"Wrote: {mdPath}");

Console.WriteLine("✅ Done");

// ---------------- helpers ----------------
static string StripWrappingMarkdownFence(string s)
{
    if (string.IsNullOrWhiteSpace(s)) return s;

    var t = s.Trim();

    // Remove ```markdown ... ``` or ``` ... ```
    if (t.StartsWith("```", StringComparison.Ordinal))
    {
        var firstNewline = t.IndexOf('\n');
        if (firstNewline >= 0)
        {
            var afterHeader = t[(firstNewline + 1)..];
            var lastFence = afterHeader.LastIndexOf("```", StringComparison.Ordinal);
            if (lastFence >= 0)
            {
                return afterHeader[..lastFence].Trim();
            }
        }
    }

    return s;
}
