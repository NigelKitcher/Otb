using Otb.NodeSequencer.Service;

namespace Otb.NodeSequencer.Spec
{
    internal class FakeNode : INode
    {
        public string Name { get; }
        public string Dependency { get; }

        public FakeNode(string name, string dependency)
        {
            Name = name;
            Dependency = dependency;
        }

        public FakeNode(string name)
        {
            Name = name;
            Dependency = string.Empty;
        }

        public bool HasDependency => !string.IsNullOrEmpty(Dependency);
    }
}
