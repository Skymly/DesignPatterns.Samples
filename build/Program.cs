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
        ("DesignPatterns.Samples.Composite/DesignPatterns.Samples.Composite.csproj", true),
        ("DesignPatterns.Samples.Decorator/DesignPatterns.Samples.Decorator.csproj", true),
        ("DesignPatterns.Samples.EventAggregator/DesignPatterns.Samples.EventAggregator.csproj", true),
        ("DesignPatterns.Samples.Factory/DesignPatterns.Samples.Factory.csproj", true),
        ("DesignPatterns.Samples.GenerateSingleton/DesignPatterns.Samples.GenerateSingleton.csproj", true),
        ("DesignPatterns.Samples.RegisterFactory/DesignPatterns.Samples.RegisterFactory.csproj", true),
        ("DesignPatterns.Samples.DependencyInjection/DesignPatterns.Samples.DependencyInjection.csproj", true),
    ];

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
        });
}
