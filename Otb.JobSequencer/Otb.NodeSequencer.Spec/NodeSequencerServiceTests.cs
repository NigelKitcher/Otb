using Microsoft.VisualStudio.TestTools.UnitTesting;
using Otb.NodeSequencer.Service;
using Otb.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Otb.NodeSequencer.Spec
{
    [TestClass]
    public class NodeSequencerServiceTests
    {
        private INodeSequencerService<FakeJob> _sequencerService;

        [TestInitialize]
        public void Initialise()
        {
            _sequencerService = new NodeSequencerService<FakeJob>();
        }

        [TestMethod]
        public void Given_no_jobs_When_GetTopologicalOrdering_is_invoked_Then_returns_no_job()
        {
            // Arrange

            // Act
            var result = _sequencerService.GetTopologicalOrdering(new List<FakeJob>()).ToList();

            // Assert
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void Given_a_job_When_GetTopologicalOrdering_is_invoked_Then_returns_one_job()
        {
            // Arrange
            var jobs = new List<FakeJob>
            {
                new FakeJob("A")
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
            var jobs = new List<FakeJob>
            {
                new FakeJob("A"),
                new FakeJob("B"),
                new FakeJob("C")
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
            var jobs = new List<FakeJob>
            {
                new FakeJob("A"),
                new FakeJob("B", "C"),
                new FakeJob("C")
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
            var jobs = new List<FakeJob>
            {
                new FakeJob("A"),
                new FakeJob("B", "C"),
                new FakeJob("C", "F"),
                new FakeJob("D", "A"),
                new FakeJob("E", "B"),
                new FakeJob("F")
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

        [TestMethod, ExpectedExceptionWithMessage(typeof(ArgumentException), "FakeJobs can not have circular dependencies")]
        public void Given_three_jobs_with_a_self_dependency_When_GetTopologicalOrdering_is_invoked_Then_exception_thrown()
        {
            // Arrange
            var jobs = new List<FakeJob>
            {
                new FakeJob("A"),
                new FakeJob("B"),
                new FakeJob("C", "C")
            };

            // Act
            var result = _sequencerService.GetTopologicalOrdering(jobs).ToList();

            // Assert
        }

        [TestMethod, ExpectedExceptionWithMessage(typeof(ArgumentException), "FakeJobs can not have circular dependencies")]
        public void Given_a_list_of_jobs_that_have_circular_dependency_as_per_sixth_case_in_requirements_When_GetTopologicalOrdering_is_invoked_Then_Exception_thrown()
        {
            // Arrange
            var jobs = new List<FakeJob>
            {
                new FakeJob("A"),
                new FakeJob("B", "C"),
                new FakeJob("C", "F"),
                new FakeJob("D", "A"),
                new FakeJob("E"),
                new FakeJob("F", "B")
            };

            // Act
            var result = _sequencerService.GetTopologicalOrdering(jobs).ToList();

            // Assert
        }

        [TestMethod]
        public void Given_a_job_with_a_backwards_dependency_When_GetTopologicalOrdering_is_invoked_Then_two_jobs_GetTopologicalOrdering_is_invoked_with_correct_order()
        {
            // Arrange
            var jobs = new List<FakeJob>
            {
                new FakeJob("A"),
                new FakeJob("B", "A"),
            };

            // Act
            var result = _sequencerService.GetTopologicalOrdering(jobs).ToList();

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.That.IsOrdered(new Tuple<string, string>("A", "B"), result);
        }

        [TestMethod]
        public void Given_a_job_with_a_forwards_dependency_When_GetTopologicalOrdering_is_invoked_Then_two_jobs_GetTopologicalOrdering_is_invoked_with_correct_order()
        {
            // Arrange
            var jobs = new List<FakeJob>
            {
                new FakeJob("A", "B"),
                new FakeJob("B"),
            };

            // Act
            var result = _sequencerService.GetTopologicalOrdering(jobs).ToList();

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.That.IsOrdered(new Tuple<string, string>("B", "A"), result);
        }

        [TestMethod]
        public void Given_a_job_with_a_forwards_dependency_to_a_job_that_has_a_backwards_dependency_When_GetTopologicalOrdering_is_invoked_Then_three_jobs_GetTopologicalOrdering_is_invoked_with_correct_order()
        {
            // Arrange
            var jobs = new List<FakeJob>
            {
                new FakeJob("A", "C"),
                new FakeJob("B"),
                new FakeJob("C", "B")
            };

            // Act
            var result = _sequencerService.GetTopologicalOrdering(jobs).ToList();

            // Assert
            Assert.AreEqual(3, result.Count);
            Assert.That.IsOrdered(new Tuple<string, string>("B", "C"), result);
            Assert.That.IsOrdered(new Tuple<string, string>("C", "A"), result);
        }

        [TestMethod, ExpectedExceptionWithMessage(typeof(ArgumentException), "FakeJobs can not have circular dependencies")]
        public void Given_a_job_with_a_self_dependency_When_GetTopologicalOrdering_is_invoked_Then_exception_thrown()
        {
            // Arrange
            var jobs = new List<FakeJob>
            {
                new FakeJob("A", "A")
            };

            // Act
            var result = _sequencerService.GetTopologicalOrdering(jobs).ToList();

            // Assert
        }


        [TestMethod, ExpectedExceptionWithMessage(typeof(ArgumentException), "FakeJobs can not have circular dependencies")]
        public void Given_two_jobs_with_a_circular_dependency_When_GetTopologicalOrdering_is_invoked_Then_exception_thrown()
        {
            // Arrange
            var jobs = new List<FakeJob>
            {
                new FakeJob("A", "B"),
                new FakeJob("B", "A")
            };

            // Act
            var result = _sequencerService.GetTopologicalOrdering(jobs).ToList();

            // Assert
        }
    }
}
