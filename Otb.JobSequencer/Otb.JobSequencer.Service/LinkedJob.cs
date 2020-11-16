using Otb.NodeSequencer.Service;

namespace Otb.JobSequencer.Service
{
    internal class LinkedJob : Job, ILinkedNode
    {
        public string Dependency { get; }

        public LinkedJob(string name, string dependency) : base(name)
        {
            Dependency = dependency;
        }

        public LinkedJob(string name) : base(name)
        {
        }
    }
}
