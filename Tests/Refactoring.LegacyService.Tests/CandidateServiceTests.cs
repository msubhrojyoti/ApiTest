using Moq;
using Refactoring.LegacyService.Exceptions;
using Refactoring.LegacyService.Interfaces;
using System;
using Xunit;

namespace Refactoring.LegacyService.Tests
{
    public class CandidateServiceTests : IDisposable
    {
        private readonly CandidateService _candidateService;
        private readonly Mock<ICandidateDataAccess> _dataAccessSrv = new Mock<ICandidateDataAccess>() { CallBase = true };
        private readonly Mock<IPositionRepository> _positionSrv = new Mock<IPositionRepository>() { CallBase = true };
        private readonly Mock<ICandidateCreditService> _creditSrv = new Mock<ICandidateCreditService>() { CallBase = true };

        public CandidateServiceTests()
        {
            _candidateService = new CandidateService(
                _dataAccessSrv.Object,
                _creditSrv.Object,
                _positionSrv.Object);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void TestAddCandidateWithInvalidCredit()
        {
            int credit = 400;
            
            //setups
            _positionSrv.Setup(x => x.GetById(
                It.IsAny<int>())).Returns(new Position() 
                {
                    Name = Position.PositionType_FeatureDeveloper,
                });

            _creditSrv.Setup(x => x.GetCredit(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<DateTime>())).Returns(credit);

            _dataAccessSrv.Setup(x => x.AddCandidate(
                It.IsAny<Candidate>())).Verifiable();

            var x = Assert.Throws<CreditCheckFailedException>(() =>
            {
                _candidateService.AddCandidate(
                    "Subhro",
                    "Mondal",
                    100,
                    DateTime.Now,
                    "subhro.mondal@company.com");
            });

            // validations
            _dataAccessSrv.Verify(x => x.AddCandidate(
                It.IsAny<Candidate>()), Times.Never);

            _positionSrv.Verify(x => x.GetById(
                It.IsAny<int>()), Times.AtMostOnce);

            Assert.Equal(CandidateService.CreditMessage, x.Message);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void TestAddCandidateWithValidCredit()
        {
            int credit = 500;

            _positionSrv.Setup(x => x.GetById(
                It.IsAny<int>())).Returns(new Position()
                {
                    Name = Position.PositionType_FeatureDeveloper,
                });

            _creditSrv.Setup(x => x.GetCredit(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<DateTime>())).Returns(credit);

            _dataAccessSrv.Setup(x => x.AddCandidate(
                It.IsAny<Candidate>())).Verifiable();
          
            _candidateService.AddCandidate(
                    "Subhro",
                    "Mondal",
                    100,
                    DateTime.Now,
                    "subhro.mondal@company.com");

            _dataAccessSrv.Verify(x => x.AddCandidate(
                It.IsAny<Candidate>()), Times.Once);

            _creditSrv.Verify(x => x.GetCredit(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<DateTime>()), Times.Once);

            _positionSrv.Verify(x => x.GetById(
                It.IsAny<int>()), Times.AtMostOnce);

        }

        [Fact]
        [Trait("Category", "Unit")]
        public void TestRequireCreditCheck()
        {
            Assert.True(_candidateService.RequireCreditCheck(Position.PositionType_FeatureDeveloper));
            Assert.True(_candidateService.RequireCreditCheck(Position.PositionType_FeatureDeveloper));
            Assert.False(_candidateService.RequireCreditCheck("some xyz"));
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void TestPerformCreditCheck()
        {
            _candidateService.PerformCreditCheck(500);
            _candidateService.PerformCreditCheck(600);
            Assert.Throws<CreditCheckFailedException>(() => _candidateService.PerformCreditCheck(100));
            Assert.Throws<CreditCheckFailedException>(() => _candidateService.PerformCreditCheck(-1));
        }

        public void Dispose()
        {
            _candidateService.Dispose();
        }
    }
}
