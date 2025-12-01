using System;
using System.Collections.Generic;
using System.Text;

namespace BMPTec.Domain.Exceptions
{
    public class ContaException : DomainException
    {
        public ContaException(string message) : base(message)
        {
        }
    }
}
