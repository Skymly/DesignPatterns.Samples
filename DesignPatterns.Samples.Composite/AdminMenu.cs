using DesignPatterns.Structural;

namespace Composite.Sample;

[CompositePart<IMenuNode>("admin", Order = 5)]
public sealed class AdminMenu : IMenuNode, ICompositeBuildable<IMenuNode>
{
    private IReadOnlyList<IMenuNode> _children = Array.Empty<IMenuNode>();

    public string Title => "Admin";

    public IReadOnlyList<IMenuNode> Children => _children;

    public void SetChildren(IReadOnlyList<IMenuNode> children) =>
        _children = children.ToList();
}
