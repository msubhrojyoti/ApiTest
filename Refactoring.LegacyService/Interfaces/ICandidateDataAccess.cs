using System;
using System.Collections.Generic;
using System.Text;

namespace Refactoring.LegacyService.Interfaces
{
    public interface ICandidateDataAccess
    {
        void AddCandidate(Candidate candidate);
    }
}
