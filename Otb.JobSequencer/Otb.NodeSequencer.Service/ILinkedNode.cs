namespace Otb.NodeSequencer.Service
{
    public interface ILinkedNode : INode
    {
        string Dependency { get; }
    }
}
