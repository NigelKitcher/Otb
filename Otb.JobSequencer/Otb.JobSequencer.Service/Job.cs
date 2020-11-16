using Otb.NodeSequencer.Service;

namespace Otb.JobSequencer.Service
{
    internal class Job : INode
    {
        public string Name { get; }
        public string Dependency { get; }

        public Job(string name, string dependency)
        {
            Name = name;
            Dependency = dependency;
        }

        public Job(string name)
        {
            Name = name;
            Dependency = string.Empty;
        }

        public bool HasDependency => !string.IsNullOrEmpty(Dependency);
    }
}
