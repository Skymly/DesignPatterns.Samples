using DesignPatterns.Structural;

namespace Composite.Sample;

[CompositePart<IMenuNode>("admin-users", ParentKey = "admin", Order = 0)]
public sealed class AdminUsersMenu : IMenuNode, ICompositeBuildable<IMenuNode>
{
    private IReadOnlyList<IMenuNode> _children = Array.Empty<IMenuNode>();

    public string Title => "Users";

    public IReadOnlyList<IMenuNode> Children => _children;

    public void SetChildren(IReadOnlyList<IMenuNode> children) =>
        _children = children.ToList();
}
