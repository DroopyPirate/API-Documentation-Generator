using System.Text.Json;

namespace ApiDocGen.Core.Serialization;

public static class JsonDefaults
{
    public static readonly JsonSerializerOptions Options = new()
    {
        WriteIndented = true
    };
}
