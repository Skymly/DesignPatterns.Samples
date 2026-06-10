using Composite.Sample;
using DesignPatterns.Structural;

Console.WriteLine("=== Catalog: MenuNodeCompositeCatalog.BuildForest() ===");
var forest = MenuNodeCompositeCatalog.BuildForest();

Console.WriteLine($"Forest roots: {forest.Count} ({string.Join(", ", forest.Select(static r => r.Title))})");
Console.WriteLine();

CompositeTraverser.TraverseForest(
    forest,
    (node, depth, siblingIndex) =>
    {
        var suffix = depth == 0 ? $" (forest root #{siblingIndex})" : string.Empty;
        Console.WriteLine($"{new string(' ', depth * 2)}{node.Title}{suffix}");
    });

Console.WriteLine();
Console.WriteLine($"Root key constants: {MenuNodeCompositeKeys.Root}, {MenuNodeCompositeKeys.Admin}");

try
{
    MenuNodeCompositeCatalog.BuildRoot();
    Console.WriteLine();
    Console.WriteLine("BuildRoot() unexpectedly succeeded on a multi-root catalog.");
}
catch (CompositeAssemblyException ex)
{
    Console.WriteLine();
    Console.WriteLine($"BuildRoot() on multi-root catalog: {ex.Message}");
}

Console.WriteLine();
Console.WriteLine("=== Manual: CompositeTreeBuilder<IMenuNode>() ===");
var manualRoot = new ManualMenuBranch("Home (manual)");
var manualTree = new CompositeTreeBuilder<IMenuNode>()
    .Branch(manualRoot, branch => branch
        .Leaf(new ManualMenuLeaf("Profile"))
        .Branch(new ManualMenuBranch("Settings"), settings => settings
            .Leaf(new ManualMenuLeaf("Account"))
            .Leaf(new ManualMenuLeaf("Privacy"))))
    .Build();

CompositeTraverser.Traverse(
    manualTree,
    (node, depth, _) => Console.WriteLine($"{new string(' ', depth * 2)}{node.Title}"));
