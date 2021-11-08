using System;
using System.Collections.Generic;
using System.Text;

namespace Refactoring.LegacyService.Exceptions
{
    public class CandidateAddException: Exception
    {
        public CandidateAddException()
        {

        }
        public CandidateAddException(string message)
        : base(message)
        {
        }

        public CandidateAddException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
