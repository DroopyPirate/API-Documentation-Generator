# API Documentation Generator

A CLI tool that generates consumer-focused API documentation (Markdown) from a .NET codebase.

It:
- Scans an ASP.NET repository (controllers/actions) using Roslyn.
- Produces a machine-readable spec (`api.json`).
- Uses a language model (Google Gemini client included) to render concise, professional Markdown docs (`api.md`) from the spec.
- Targets external API consumers (not internal developer docs).

Status
- Working CLI + scanner + Gemini client implementation.
- Example output available in `docs/`.

Quick links
- CLI orchestration: ApiDocGen.Cli/Program.cs
- Spec model & prompt rules: ApiDocGen.Core/Models/ApiSpec.cs and ApiDocGen.Core/AI/PromptBuilder.cs
- Gemini client: ApiDocGen.AI.Gemini/GeminiClient.cs
- Example generated docs: docs/api_mvc.md

Requirements
- .NET SDK (recommended: .NET 9; .NET 8 may also work depending on your environment)
- Internet access for cloning public repos and calling Gemini
- A Gemini API key (required) — GEMINI_API_KEY
- Optional: GitHub personal access token (GITHUB_TOKEN) to scan private repos

Install / Build
1. Clone this repository:
   ```bash
   git clone https://github.com/DroopyPirate/API-Documentation-Generator.git
   cd API-Documentation-Generator
   ```
2. Build the solution:
   ```bash
   dotnet build
   ```

Usage (CLI)
- Basic usage:
  ```bash
  # Set GEMINI_API_KEY in environment (recommended)
  export GEMINI_API_KEY="your_gemini_api_key"      # macOS / Linux
  $env:GEMINI_API_KEY="your_gemini_api_key"       # PowerShell (Windows)

  # Generate docs for a public repository
  dotnet run --project ApiDocGen.Cli -- --repo https://github.com/user/repo --out ./docs
  ```

- Example with explicit keys (local testing only):
  ```bash
  dotnet run --project ApiDocGen.Cli -- --repo https://github.com/user/repo --out ./docs --gemini-key "KEY" --github-token "PAT"
  ```

CLI options
- --repo <github_url> (required) — HTTPS URL of the GitHub repository to scan.
- --out <output_dir> — Directory to write `api.json` and `api.md` (default: `./docs`).
- --gemini-model <model> — Gemini model to call (default set in code: `gemini-2.5-flash`).
- --gemini-key <key> — (optional) Gemini API key override for local testing.
- --github-token <token> — (optional) GitHub PAT to access private repositories.

Environment variables
- GEMINI_API_KEY — preferred way to provide the Gemini API key (required unless using `--gemini-key`).
- GITHUB_TOKEN — optional; required for private repos if not passing `--github-token`.

What the tool writes
- api.json — the serialized ApiSpec (machine-readable source-of-truth).
- api.md — the LLM-generated Markdown API documentation.

How it works (high-level)
1. Clone the target repository into a temporary folder.
2. Read the HEAD commit to record provenance.
3. Use RoslynControllerScanner to find controllers and their actions.
4. Build an ApiSpec model of controllers, endpoints, parameters, request/response schemas.
5. Write `api.json` to the output directory.
6. Build a strict prompt that serializes the spec and instructs the LLM how to render docs.
7. Call Gemini to generate Markdown and write `api.md`.

Prompt & generation rules
- The PromptBuilder enforces strict rules for the LLM:
  - Audience: API consumers (people who call the API).
  - Do NOT include internal implementation details (controllers, classes, languages).
  - Use ONLY information present in the JSON spec; do not invent missing data.
  - If data is missing, the text should say "Not specified".
  - Output MUST be valid Markdown.
- Resulting Markdown is intentionally concise and consumer-focused.

Output example
- See `docs/api_mvc.md` for a real example produced by the tool. It contains endpoint sections with:
  - HTTP method + route
  - Short description
  - Route / query parameters
  - Request body (if present)
  - Example curl command
  - Response notes

Extensibility ideas
- Add additional LLM backends (OpenAI, Azure, local LLM) by implementing a client similar to GeminiClient and wiring it into the CLI.
- Expand Roslyn scanning patterns to support more frameworks, minimal APIs, or attribute conventions.
- Add unit and integration tests for scanning, spec generation, and prompt/response handling.
- Add a validation pass for generated Markdown (linting, guardrails).

CI integration (suggestion)
- A CI workflow can run the CLI and commit generated docs to a branch or a `gh-pages` site. Example steps:
  - Checkout code
  - Setup .NET SDK
  - Configure GEMINI_API_KEY, GITHUB_TOKEN as secrets (be very careful with secret usage)
  - dotnet run --project ApiDocGen.Cli -- --repo https://github.com/owner/repo --out ./docs
  - Commit and push `docs/` (or create a PR)

Troubleshooting
- "Missing GEMINI_API_KEY" — set the environment variable or pass `--gemini-key`.
- "Unable to clone private repo" — provide GITHUB_TOKEN environment variable or `--github-token`.
- LLM output not as expected — confirm the scanned `api.json` contains the required data. The prompt forces the LLM to only use that spec; if data is missing, the LLM will say "Not specified".
- .NET SDK incompatibilities — ensure compatible SDK installed (recommended .NET 9).

Security & privacy notes
- The tool sends the serialized ApiSpec to the configured LLM. Do not send private or sensitive API schemas to external LLMs unless you accept the data-sharing implications.
- Prefer using private LLMs or on-premise options if needed and extend the code to support them.

Repository layout (important files)
- ApiDocGen.Cli/ — CLI entry point (Program.cs)
- ApiDocGen.Core/ — spec models, prompt builder, serialization
- ApiDocGen.Scanner.Roslyn/ — Roslyn-based controller/action scanner
- ApiDocGen.Git/ — Git utilities (clone, read commit)
- ApiDocGen.AI.Gemini/ — Gemini client implementation
- docs/ — example output (`api_mvc.md`)
