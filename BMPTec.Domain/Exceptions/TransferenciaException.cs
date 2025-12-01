using System;
using System.Collections.Generic;
using System.Text;

namespace BMPTec.Domain.Exceptions
{
    public class TransferenciaException : DomainException
    {
        public TransferenciaException(string message) : base(message)
        {
        }
    }
}
