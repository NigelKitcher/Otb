namespace Otb.NodeSequencer.Service
{
    public interface INode
    {
        string Name { get; }
        string Dependency { get; }
    }
}
