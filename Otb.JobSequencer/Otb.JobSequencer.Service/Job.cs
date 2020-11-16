using Otb.NodeSequencer.Service;

namespace Otb.JobSequencer.Service
{
    internal class Job : INode
    {
        public string Name { get; }

        public Job(string name)
        {
            Name = name;
        }
    }
}
