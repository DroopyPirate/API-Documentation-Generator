using System.Text.Json;
using ApiDocGen.Core.Models;
using ApiDocGen.Core.Serialization;

namespace ApiDocGen.AI.Ollama;

public static class PromptBuilder
{
    public static string Build(ApiSpec spec)
    {
        var specJson = JsonSerializer.Serialize(spec, JsonDefaults.Options);

        return $"""
You are a senior backend engineer writing API documentation for developers who will CALL the API.

IMPORTANT ROLE RULES:
- Your audience is API consumers, NOT developers reading source code.
- Do NOT explain JSON schemas.
- Do NOT show C#, JavaScript, or parsing examples.
- Do NOT mention controllers, classes, models, or internal implementation.
- Do NOT describe the structure of the JSON spec.
- Write documentation as if this is a public REST API.

STRICT RULES:
- Use ONLY the information provided in the JSON spec.
- Do NOT invent endpoints, routes, parameters, or responses.
- If something is missing, say "Not specified".
- Output MUST be valid Markdown.
- Be concise and professional.

DOCUMENT FORMAT (follow exactly):
1. Title
2. Short overview
3. For each API group:
   - Section header with the group name
   - For each endpoint:
     - HTTP METHOD + ROUTE
     - Short description (1–2 lines, conservative)
     - Parameters:
       - Route parameters
       - Query parameters
       - Request body (if present)
     - Example curl command
     - Response notes

JSON SPEC (SOURCE OF TRUTH — DO NOT EXPLAIN IT):
```json
{specJson}
""";
    }
}