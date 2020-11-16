using Otb.JobSequencer.Contracts;

namespace Otb.JobSequencer.Service
{
    public class Job : IJob
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
