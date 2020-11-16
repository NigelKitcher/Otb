using Otb.NodeSequencer.Service;

namespace Otb.NodeSequencer.Spec
{
    internal class FakeLinkedNode : FakeNode, ILinkedNode
    {
        public string Dependency { get; }

        private FakeLinkedNode(string _) : base(_)
        {
        }

        public FakeLinkedNode(string name, string dependency) : base(name)
        {
            Dependency = dependency;
        }
    }
}
