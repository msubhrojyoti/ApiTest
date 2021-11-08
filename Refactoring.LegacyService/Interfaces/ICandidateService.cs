using System;

namespace Refactoring.LegacyService.Interfaces
{
    public interface ICandidateService
    {
        void AddCandidate(
            string firstname,
            string surame,
            int PositionId,
            DateTime DateOfBirth,
            string EmailAddress);

        bool RequireCreditCheck(string positionName);

        void PerformCreditCheck(int credit);
    }
}
