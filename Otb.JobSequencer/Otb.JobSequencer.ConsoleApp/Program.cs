using System;
using Otb.JobSequencer.Service;

namespace Otb.JobSequencer.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var jobRequestParser = new JobRequestParser();
            var sequencerService = new SequencerService();

            var input = "A => B" + Environment.NewLine + "B =>";

            var jobs = jobRequestParser.GetJobs(input);

            var sequencedJobs = sequencerService.GetTopologicalOrdering(jobs);

            Console.WriteLine("Input Jobs:");
            Console.WriteLine(input);

            Console.WriteLine("Sequenced Jobs:");
            foreach (var job in sequencedJobs)
            {
                Console.Write(job.Name);
            }

            Console.WriteLine("");
            Console.WriteLine("Completed.");
        }
    }
}
