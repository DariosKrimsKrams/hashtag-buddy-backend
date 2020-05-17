namespace Instaq.API.Extern.Tests.Services
{
    using System;
    using System.Runtime.CompilerServices;

    using FluentAssertions;
    using FluentAssertions.Execution;

    using Instaq.API.Extern.Models.Responses;
    using Instaq.API.Extern.Services;
    using Instaq.API.Extern.Services.Interfaces;
    using Instaq.Contract;
    using Instaq.Contract.Storage;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class EvaluationService_WhenGetSimilarHashtags
    {
        private IEvaluationService evaluationService;
        private IEvaluationStorage evaluationStorage;

        [SetUp]
        public void Setup()
        {
            this.evaluationStorage = Substitute.For<IEvaluationStorage>();
            var customerStorage = Substitute.For<ICustomerStorage>();
            var logHashtagSearchStorage = Substitute.For<ILogHashtagSearchStorage>();
            this.evaluationService = new EvaluationService(evaluationStorage, null, null, null, null, customerStorage, logHashtagSearchStorage);
        }

        [Test]
        public void ThenClass_ShouldImplementItsInterface()
        {
            // Assert
            typeof(EvaluationService).Should().Implement<IEvaluationService>();
        }

        [Test]
        public void ThenEmptyKeyword_ShouldThrowException()
        {
            // Arrange
            var customerId = "xyz";
            var keyword    = "";

            // Act
            Action action = () => this.evaluationService.GetSimilarHashtags(customerId, keyword);

            // Assert
            Assert.Throws<ArgumentException>(action.Invoke);
        }

        [Test]
        public void ThenUnknownKeyword_ShouldReturnEmptyHashtags()
        {
            // Arrange
            var customerId = "xyz";
            var keyword    = "unknown";
            
            // Act 
            var result = this.evaluationService.GetSimilarHashtags(customerId, keyword);

            // Assert
            using (new AssertionScope())
            {
                result.Should().BeOfType<SearchResponse>();
                result.LogId.Should().Be(0, because: "logging Logic not mocked yet");
                result.Hashtags.Should().NotBeNull()
                    .And.BeEmpty(because: "No known keyword has been passed");
            }

        }

    }
}