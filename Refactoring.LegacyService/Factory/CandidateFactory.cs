using System;
using System.Collections.Generic;
using System.Text;

namespace Refactoring.LegacyService.Factory
{
    public static class CandidateFactory
    {
        public static Candidate CreateCandiate(
            string firstname,
            string surname,
            Position position,
            string emailAddress,
            bool requireCreditCheck,
            int credit,
            DateTime dateOfBirth)
        {
            return new Candidate()
            {
                Firstname = firstname,
                Surname = surname,
                Position = position,
                Credit = credit,
                DateOfBirth = dateOfBirth,
                EmailAddress = emailAddress,
                RequireCreditCheck = requireCreditCheck
            };
        }
    }
}
