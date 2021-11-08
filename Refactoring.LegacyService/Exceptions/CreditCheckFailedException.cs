using System;
using System.Collections.Generic;
using System.Text;

namespace Refactoring.LegacyService.Exceptions
{
    public class CreditCheckFailedException : Exception
    {
        public CreditCheckFailedException()
        {

        }

        public CreditCheckFailedException(string message)
        : base(message)
        {
        }

        public CreditCheckFailedException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
