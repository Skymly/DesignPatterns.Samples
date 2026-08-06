using DesignPatterns.Behavioral;

namespace DesignPatterns.Samples.WorkGraph;

/// <summary>
/// Holder for the request-prep fork–join graph (ROADMAP F3 sample sketch).
/// </summary>
[WorkGraph<PrepContext>]
public static class RequestPrep;

[WorkStep(typeof(RequestPrep), Id = "auth")]
public sealed class AuthStep : IWorkStep<PrepContext>
{
    public async ValueTask ExecuteAsync(PrepContext context, CancellationToken cancellationToken = default)
    {
        context.Log("Auth start");
        await Task.Delay(40, cancellationToken);
        context.Token = "tok_sample";
        context.Log("Auth done");
    }
}

[WorkStep(typeof(RequestPrep), Id = "load-config")]
public sealed class LoadConfigStep : IWorkStep<PrepContext>
{
    public async ValueTask ExecuteAsync(PrepContext context, CancellationToken cancellationToken = default)
    {
        context.Log("LoadConfig start");
        await Task.Delay(40, cancellationToken);
        context.ConfigRole = "editor";
        context.Log("LoadConfig done");
    }
}

[WorkStep(typeof(RequestPrep), Id = "build-principal", DependsOn = ["auth", "load-config"])]
public sealed class BuildPrincipalStep : IWorkStep<PrepContext>
{
    public ValueTask ExecuteAsync(PrepContext context, CancellationToken cancellationToken = default)
    {
        context.Log("BuildPrincipal start");
        context.Principal = $"{context.ConfigRole}:{context.Token}";
        context.Log($"BuildPrincipal done → {context.Principal}");
        return default;
    }
}

[WorkStep(typeof(RequestPrep), Id = "authorize", DependsOn = ["build-principal"])]
public sealed class AuthorizeStep : IWorkStep<PrepContext>
{
    public ValueTask ExecuteAsync(PrepContext context, CancellationToken cancellationToken = default)
    {
        context.Log("Authorize start");
        context.Authorized = context.Principal is { Length: > 0 };
        context.Log($"Authorize done → authorized={context.Authorized}");
        return default;
    }
}
