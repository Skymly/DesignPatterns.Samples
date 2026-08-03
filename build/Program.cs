using System.Diagnostics;
using Nuke.Common;
using Nuke.Common.Execution;
using Nuke.Common.IO;
using Nuke.Common.Tools.DotNet;

using static Nuke.Common.Tools.DotNet.DotNetTasks;

[UnsetVisualStudioEnvironmentVariables]
sealed class Build : NukeBuild
{
    [Parameter("Build configuration (Debug/Release)")]
    readonly string Configuration = IsLocalBuild ? "Debug" : "Release";

    [Parameter("Use sibling DesignPatterns repo instead of NuGet packages")]
    readonly bool UseLocalDesignPatterns = true;

    AbsolutePath Root => RootDirectory;

    static readonly (string RelativePath, bool RunAfterBuild)[] SampleProjects =
    [
        ("DesignPatterns.Samples.Strategy/DesignPatterns.Samples.Strategy.csproj", true),
        ("DesignPatterns.Samples.Chain/DesignPatterns.Samples.Chain.csproj", true),
        ("DesignPatterns.Samples.CommandRouter/DesignPatterns.Samples.CommandRouter.csproj", true),
        ("DesignPatterns.Samples.Composite/DesignPatterns.Samples.Composite.csproj", true),
        ("DesignPatterns.Samples.Decorator/DesignPatterns.Samples.Decorator.csproj", true),
        ("DesignPatterns.Samples.EventAggregator/DesignPatterns.Samples.EventAggregator.csproj", true),
        ("DesignPatterns.Samples.Factory/DesignPatterns.Samples.Factory.csproj", true),
        ("DesignPatterns.Samples.GenerateSingleton/DesignPatterns.Samples.GenerateSingleton.csproj", true),
        ("DesignPatterns.Samples.RegisterFactory/DesignPatterns.Samples.RegisterFactory.csproj", true),
        ("DesignPatterns.Samples.DependencyInjection/DesignPatterns.Samples.DependencyInjection.csproj", true),
        ("DesignPatterns.Samples.State/DesignPatterns.Samples.State.csproj", true),
        ("DesignPatterns.Samples.StepBuilder/DesignPatterns.Samples.StepBuilder.csproj", true),
        ("DesignPatterns.Samples.HierarchicalState/DesignPatterns.Samples.HierarchicalState.csproj", true),
    ];

    AbsolutePath PluginAssembliesHostProject =>
        Root / "DesignPatterns.Samples.PluginAssemblies/Host/DesignPatterns.Samples.PluginAssemblies.Host.csproj";

    AbsolutePath PluginAssembliesInvalidKeyProject =>
        Root / "DesignPatterns.Samples.PluginAssemblies/Scenarios.InvalidKey/DesignPatterns.Samples.PluginAssemblies.Scenarios.InvalidKey.csproj";

    AbsolutePath PluginAssembliesDuplicateKeyProject =>
        Root / "DesignPatterns.Samples.PluginAssemblies/Scenarios.DuplicateKey/DesignPatterns.Samples.PluginAssemblies.Scenarios.DuplicateKey.csproj";

    AbsolutePath DesignPatternsAnalyzerTestsProject =>
        Root / "../DesignPatterns/tests/DesignPatterns.Analyzers.Tests/DesignPatterns.Analyzers.Tests.csproj";

    public static int Main() => Execute<Build>(x => x.Ci);

    Target Ci => _ => _
        .Executes(() =>
        {
            foreach ((string relativePath, bool runAfterBuild) in SampleProjects)
            {
                AbsolutePath projectFile = Root / relativePath;
                Assert.FileExists(projectFile, $"Sample project not found: {projectFile}");

                DotNetBuild(s => s
                    .SetProjectFile(projectFile)
                    .SetConfiguration(Configuration)
                    .SetProperty("UseLocalDesignPatterns", UseLocalDesignPatterns));

                if (runAfterBuild)
                {
                    DotNetRun(s => s
                        .SetProjectFile(projectFile)
                        .SetConfiguration(Configuration)
                        .EnableNoRestore()
                        .EnableNoBuild());
                }
            }

            RunPluginAssembliesScenarios();
        });

    void RunPluginAssembliesScenarios()
    {
        Assert.FileExists(PluginAssembliesHostProject, $"Sample project not found: {PluginAssembliesHostProject}");
        Assert.FileExists(PluginAssembliesInvalidKeyProject, $"Sample project not found: {PluginAssembliesInvalidKeyProject}");
        Assert.FileExists(PluginAssembliesDuplicateKeyProject, $"Sample project not found: {PluginAssembliesDuplicateKeyProject}");

        DotNetBuild(s => s
            .SetProjectFile(PluginAssembliesHostProject)
            .SetConfiguration(Configuration)
            .SetProperty("UseLocalDesignPatterns", UseLocalDesignPatterns));

        DotNetRun(s => s
            .SetProjectFile(PluginAssembliesHostProject)
            .SetConfiguration(Configuration)
            .EnableNoRestore()
            .EnableNoBuild());

        var missingProvider = StartDotNet(
            $"run --project \"{PluginAssembliesHostProject}\" -c {Configuration} --no-build -- s2",
            Root);
        Assert.True(
            missingProvider.ExitCode != 0,
            "Scenario S2 (missing provider) should fail when 'beta' is configured without Providers.Beta.");
        Assert.True(
            missingProvider.Output.Contains("beta", StringComparison.OrdinalIgnoreCase),
            "Scenario S2 output should mention the missing 'beta' provider key.");

        var invalidKeyBuild = StartDotNet(
            $"build \"{PluginAssembliesInvalidKeyProject}\" -c {Configuration}",
            Root);
        if (invalidKeyBuild.ExitCode != 0 && invalidKeyBuild.Output.Contains("DP025", StringComparison.Ordinal))
        {
            return;
        }

        Assert.FileExists(
            DesignPatternsAnalyzerTestsProject,
            $"DesignPatterns analyzer tests not found at {DesignPatternsAnalyzerTestsProject}. Clone the sibling DesignPatterns repo for S3 (DP025).");

        DotNetTest(s => s
            .SetProjectFile(DesignPatternsAnalyzerTestsProject)
            .SetConfiguration(Configuration)
            .SetFilter("FullyQualifiedName~UnknownRegistryKeyAnalyzerTests.ReportsDp025WhenStrategyRegistryKeyIsUnknown"));

        var duplicateKeyBuild = StartDotNet(
            $"build \"{PluginAssembliesDuplicateKeyProject}\" -c {Configuration}",
            Root);
        if (duplicateKeyBuild.ExitCode != 0 && duplicateKeyBuild.Output.Contains("DP033", StringComparison.Ordinal))
        {
            return;
        }

        DotNetTest(s => s
            .SetProjectFile(DesignPatternsAnalyzerTestsProject)
            .SetConfiguration(Configuration)
            .SetFilter("FullyQualifiedName~CrossAssemblyRegistryKeyAnalyzerTests.ReportsDp033WhenSameStrategyKeyExistsInTwoReferencedAssemblies"));
    }

    static (int ExitCode, string Output) StartDotNet(string arguments, AbsolutePath workingDirectory)
    {
        using var process = Process.Start(new ProcessStartInfo
        {
            FileName = "dotnet",
            Arguments = arguments,
            WorkingDirectory = workingDirectory,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
        }) ?? throw new InvalidOperationException("Failed to start dotnet.");

        var output = process.StandardOutput.ReadToEnd() + process.StandardError.ReadToEnd();
        process.WaitForExit();
        return (process.ExitCode, output);
    }
}
