using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using Otb.JobSequencer.Contracts;
using Otb.JobSequencer.Service;
using Otb.TestTools.UnitTesting;

namespace Otb.JobSequencer.Spec
{
    [TestClass]
    public class SequencerServiceTests
    {

        private ISequencerService _sequencerService;

        [TestInitialize]
        public void Initialise()
        {
            _sequencerService = new SequencerService();
        }

        [TestMethod]
        public void Given_no_jobs_When_GetTopologicalOrdering_is_invoked_Then_returns_no_job()
        {
            // Arrange

            // Act
            var result = _sequencerService.GetTopologicalOrdering(new List<Job>()).ToList();

            // Assert
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void Given_a_job_When_GetTopologicalOrdering_is_invoked_Then_returns_one_job()
        {
            // Arrange
            var jobs = new List<Job>
            {
                new Job("A")
            };

            // Act
            var result = _sequencerService.GetTopologicalOrdering(jobs).ToList();

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("A", result[0].Name);
        }

        [TestMethod]
        public void Given_three_jobs_When_GetTopologicalOrdering_is_invoked_Then_returns_the_three_jobs_in_no_specific_order()
        {
            // Arrange
            var jobs = new List<Job>
            {
                new Job("A"),
                new Job("B"),
                new Job("C")
            };

            // Act
            var result = _sequencerService.GetTopologicalOrdering(jobs).ToList();

            // Assert
            Assert.AreEqual(3, result.Count);
            Assert.IsNotNull(result.Single(x => x.Name == "A"));
            Assert.IsNotNull(result.Single(x => x.Name == "B"));
            Assert.IsNotNull(result.Single(x => x.Name == "C"));
        }

        [TestMethod]
        public void Given_three_jobs_where_one_has_a_dependency_and_one_does_not_When_GetTopologicalOrdering_is_invoked_Then_returns_the_three_jobs_and_dependency_in_correct_order()
        {
            // Arrange
            var jobs = new List<Job>
            {
                new Job("A"),
                new Job("B", "C"),
                new Job("C")
            };

            // Act
            var result = _sequencerService.GetTopologicalOrdering(jobs).ToList();

            // Assert
            Assert.AreEqual(3, result.Count);
            Assert.IsNotNull(result.Single(x => x.Name == "A"));
            Assert.That.IsOrdered(new Tuple<string, string>("C", "B"), result);
        }

        [TestMethod]
        public void Given_jobs_with_multiple_references_as_per_fourth_test_case_from_requirements()
        {
            // Arrange
            var jobs = new List<Job>
            {
                new Job("A"),
                new Job("B", "C"),
                new Job("C", "F"),
                new Job("D", "A"),
                new Job("E", "B"),
                new Job("F")
            };

            // Act
            var result = _sequencerService.GetTopologicalOrdering(jobs).ToList();

            // Assert
            Assert.AreEqual(6, result.Count);
            Assert.IsNotNull(result.Single(x => x.Name == "A"));
            Assert.IsNotNull(result.Single(x => x.Name == "B"));
            Assert.IsNotNull(result.Single(x => x.Name == "C"));
            Assert.IsNotNull(result.Single(x => x.Name == "D"));
            Assert.IsNotNull(result.Single(x => x.Name == "E"));
            Assert.IsNotNull(result.Single(x => x.Name == "F"));
            Assert.That.IsOrdered(new Tuple<string, string>("F", "C"), result);
            Assert.That.IsOrdered(new Tuple<string, string>("C", "B"), result);
            Assert.That.IsOrdered(new Tuple<string, string>("B", "E"), result);
            Assert.That.IsOrdered(new Tuple<string, string>("A", "D"), result);
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void Given_three_jobs_with_a_self_dependency_When_GetTopologicalOrdering_is_invoked_Then_exception_thrown()
        {
            // Arrange
            var jobs = new List<Job>
            {
                new Job("A"),
                new Job("B"),
                new Job("C", "C")
            };

            // Act
            var result = _sequencerService.GetTopologicalOrdering(jobs).ToList();

            // Assert
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void Given_a_job_with_dependent_job_that_has_a_dependent_job_When_GetTopologicalOrdering_is_invoked_Then_three_jobs_returned_in_correct_order()
        {
            // Arrange
            var jobs = new List<Job>
            {
                new Job("A"),
                new Job("B", "C"),
                new Job("C", "F"),
                new Job("D", "A"),
                new Job("E"),
                new Job("F", "B")
            };

            // Act
            var result = _sequencerService.GetTopologicalOrdering(jobs).ToList();

            // Assert
        }
    }
}
