using System.Text.Json;
using ApiDocGen.AI.Ollama;
using ApiDocGen.Core.Models;
using ApiDocGen.Core.Serialization;
using ApiDocGen.Git;
using ApiDocGen.Scanner.Roslyn;

static string? Arg(string[] args, string name)
{
    var i = Array.FindIndex(args, a => string.Equals(a, name, StringComparison.OrdinalIgnoreCase));
    return (i >= 0 && i + 1 < args.Length) ? args[i + 1] : null;
}

var repoUrl = Arg(args, "--repo");
if (string.IsNullOrWhiteSpace(repoUrl))
{
    Console.WriteLine("Usage: dotnet run -- --repo <github_url> --out <output_dir> [--model llama3.1:8b] [--ollama http://localhost:11434]");
    return;
}

var outDir = Arg(args, "--out") ?? "./docs";
var model = Arg(args, "--model") ?? "llama3.1:8b";
var ollamaUrl = Arg(args, "--ollama") ?? "http://localhost:11434";

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

// 5) Ask Ollama for Markdown
Console.WriteLine($"Calling Ollama ({ollamaUrl}) model '{model}'...");
var prompt = PromptBuilder.Build(spec);

var client = new OllamaClient(new HttpClient(), ollamaUrl);
var md = await client.GenerateAsync(model, prompt);

// 6) Write api.md
var mdPath = Path.Combine(outDir, "api.md");
await File.WriteAllTextAsync(mdPath, md);
Console.WriteLine($"Wrote: {mdPath}");

Console.WriteLine("✅ Done");
