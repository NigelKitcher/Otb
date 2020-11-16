using Microsoft.VisualStudio.TestTools.UnitTesting;
using Otb.JobSequencer.Service;
using Otb.TestTools.UnitTesting;
using System;
using System.Linq;
using Otb.NodeSequencer.Service;

namespace Otb.JobSequencer.Spec
{
    [TestClass]
    public class JobRequestParserTests
    {
        private JobRequestParser _jobParser;

        [TestInitialize]
        public void Initialise()
        {
            _jobParser = new JobRequestParser();
        }

        [TestMethod]
        public void Given_an_empty_string_When_GetJobs_is_invoked_Then_empty_collection_returned()
        {
            // Arrange
            var jobRequest = string.Empty;

            // Act
            var result = _jobParser.GetJobs(jobRequest);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod, ExpectedExceptionWithMessage(typeof(ArgumentException), "Invalid line length")]
        public void Given_a_string_missing_the_maps_to_characters_When_GetJobs_is_invoked_Then_Exception_thrown()
        {
            // Arrange
            var jobRequest = "A";

            // Act
            var result = _jobParser.GetJobs(jobRequest);

            // Assert
        }

        [TestMethod]
        public void Given_a_string_has_the_maps_to_characters_When_GetJobs_is_invoked_Then_job_with_no_dependency_returned()
        {
            // Arrange
            var jobRequest = "A =>";

            // Act
            var result = _jobParser.GetJobs(jobRequest);

            // Assert
            var results = result.ToList();
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("A", results[0].Name);
        }

        [TestMethod, ExpectedExceptionWithMessage(typeof(ArgumentException), "Missing definition for job B")]
        public void Given_input_with_dependency_and_dependency_is_missing_When_GetJobs_is_invoked_Then_Exception_Thrown()
        {
            // Arrange
            var jobRequest = "A => B";

            // Act
            var result = _jobParser.GetJobs(jobRequest);

            // Assert
        }

        [TestMethod, ExpectedExceptionWithMessage(typeof(ArgumentException), "Jobs can not depend on themselves")]
        public void Given_input_with_dependency_and_dependency_is_itself_When_GetJobs_is_invoked_Then_Exception_Thrown()
        {
            // Arrange
            var jobRequest = "A => A";

            // Act
            var result = _jobParser.GetJobs(jobRequest);

            // Assert
        }

        [TestMethod]
        public void Given_a_string_has_the_maps_to_characters_and_dependency_When_GetJobs_is_invoked_Then_jobs_returned()
        {
            // Arrange
            var jobRequest = "A => B" + Environment.NewLine + "B =>";

            // Act
            var result = _jobParser.GetJobs(jobRequest);

            // Assert
            var results = result.ToList();
            Assert.AreEqual(2, results.Count);
            Assert.AreEqual("A", results[0].Name);
            Assert.AreEqual("B", ((ILinkedNode)results[0]).Dependency);
            Assert.AreEqual("B", results[1].Name);
        }

        [TestMethod, ExpectedExceptionWithMessage(typeof(ArgumentException), "Job name is not an alpha character")]
        public void Given_a_string_is_not_alphabetic_When_GetJobs_is_invoked_Then_Exception_Thrown()
        {
            // Arrange
            var jobRequest = "! =>";

            // Act
            var result = _jobParser.GetJobs(jobRequest);

            // Assert
        }

        [TestMethod, ExpectedExceptionWithMessage(typeof(ArgumentException), "Job name is not an alpha character")]
        public void Given_dependency_name_is_not_alphabetic_When_GetJobs_is_invoked_Then_Exception_Thrown()
        {
            // Arrange
            var jobRequest = "A => !";

            // Act
            var result = _jobParser.GetJobs(jobRequest);

            // Assert
        }

        [TestMethod, ExpectedExceptionWithMessage(typeof(ArgumentException), "Invalid line length")]
        public void Given_a_string_has_incomplete_maps_to_characters_When_GetJobs_is_invoked_Then_Exception_thrown()
        {
            // Arrange
            var jobRequest = "A =";

            // Act
            var result = _jobParser.GetJobs(jobRequest);

            // Assert
        }

        [TestMethod, ExpectedExceptionWithMessage(typeof(ArgumentException), "Missing pointer")]
        public void Given_a_string_has_invalid_maps_to_characters_When_GetJobs_is_invoked_Then_Exception_thrown()
        {
            // Arrange
            var jobRequest = "A ==";

            // Act
            var result = _jobParser.GetJobs(jobRequest);

            // Assert
        }

        [TestMethod, ExpectedExceptionWithMessage(typeof(ArgumentException), "Missing space")]
        public void Given_a_string_has_does_not_have_space_after_job_name_When_GetJobs_is_invoked_Then_Exception_thrown()
        {
            // Arrange
            var jobRequest = "A=>B";

            // Act
            var result = _jobParser.GetJobs(jobRequest);

            // Assert
        }

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public void Given_null_When_GetJobs_is_invoked_Then_Argument_Null_Exception_Thrown()
        {
            // Arrange
            const string jobRequest = null;

            // Act
            var result = _jobParser.GetJobs(jobRequest);

            // Assert
        }
    }
}
