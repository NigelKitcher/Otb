using Otb.NodeSequencer.Service;

namespace Otb.NodeSequencer.Spec
{
    internal class FakeNode : INode
    {
        public string Name { get; }

        public FakeNode(string name)
        {
            Name = name;
        }
    }
}
