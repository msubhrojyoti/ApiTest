namespace Refactoring.LegacyService
{
    using Refactoring.LegacyService.Exceptions;
    using Refactoring.LegacyService.Factory;
    using Refactoring.LegacyService.Interfaces;
    using System;
    using System.Linq;

    public class CandidateService : IDisposable, ICandidateService
    {
        private readonly ICandidateDataAccess _dataAccess;
        private readonly ICandidateCreditService _creditService;
        private readonly IPositionRepository _positionService;
        public const string CreditMessage = "Credit must be greater than or equal to 500";

        /// <summary>
        /// Depdency injection for custom usage. E.g. Testing out this class with Mocked instances.
        /// </summary>
        /// <param name="dataAccess"></param>
        /// <param name="positionAccess"></param>
        /// <param name="creditService"></param>
        public CandidateService(
            ICandidateDataAccess dataAccess,
            ICandidateCreditService creditService,
            IPositionRepository positionService)
        {
            _dataAccess = dataAccess;
            _creditService = creditService;
            _positionService = positionService;
        }

        public void AddCandidate(
            string firstname,
            string surname,
            int positionId,
            DateTime dateOfBirth,
            string emailAddress)
        {
            var position = _positionService.GetById(positionId);
            int credit = 0;

            bool creditCheck;
            if (creditCheck = RequireCreditCheck(position.Name))
            {
                credit = _creditService.GetCredit(firstname, surname, dateOfBirth);
                PerformCreditCheck(credit);
            }

            _dataAccess.AddCandidate(
                CandidateFactory.CreateCandiate(
                    firstname,
                    surname,
                    position,
                    emailAddress,
                    creditCheck,
                    credit,
                    dateOfBirth));
        }
        
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Dispose the object.
        /// </summary>
        /// <param name="disposing"></param>
        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                //close the service clients
            }
        }

        public bool RequireCreditCheck(string positionName)
        {
            string[] supportedPositions = new string[] 
            {
                Position.PositionType_SecuritySpecialist,
                Position.PositionType_FeatureDeveloper,
            };

            return supportedPositions.Contains(positionName, StringComparer.Ordinal);
        }

        public void PerformCreditCheck(int credit)
        {
            if (credit < 500)
            {
                throw new CreditCheckFailedException(CreditMessage);
            }
        }
    }
}
