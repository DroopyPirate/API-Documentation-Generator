namespace ApiDocGen.Core.Models;

public sealed record ApiSpec(
    string RepoUrl,
    string RepoCommit,
    List<ControllerSpec> Controllers
);

public sealed record ControllerSpec(
    string Name,
    string? RoutePrefix,
    List<EndpointSpec> Endpoints
);

public sealed record EndpointSpec(
    string Kind,                // "api" or "mvc"
    string HttpMethod,
    string Route,
    string Action,
    List<ParameterSpec> Parameters,
    TypeSchema? RequestBodySchema,
    List<ResponseSpec> Responses
);


public sealed record ParameterSpec(
    string Name,
    string Type,
    string Source // route | query | body | unknown
);

public sealed record TypeSchema(
    string Name, 
    List<PropertySchema> Properties
);

public sealed record PropertySchema(
    string Name, 
    string Type, 
    bool IsRequired, 
    List<string> Attributes
);

public sealed record ResponseSpec(
    int StatusCode,
    string Kind,                // "json" | "view" | "redirect" | "unknown"
    TypeSchema? JsonBodySchema,
    TypeSchema? ViewModelSchema
);
