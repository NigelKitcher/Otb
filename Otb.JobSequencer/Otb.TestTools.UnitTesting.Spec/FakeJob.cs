using Otb.NodeSequencer.Service;

namespace Otb.TestTools.UnitTesting.Spec
{
    public class FakeJob : INode
    {
        public string Name { get; }
        public string Dependency { get; }

        public FakeJob(string name, string dependency)
        {
            Name = name;
            Dependency = dependency;
        }

        public FakeJob(string name)
        {
            Name = name;
            Dependency = string.Empty;
        }

        public bool HasDependency => !string.IsNullOrEmpty(Dependency);
    }
}
