using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace xApi.Data.Security.Cryptography.Exceptions
{
    public class JsonWebSignatureException : Exception
    {
        public JsonWebSignatureException(string message) : base(message)
        {
        }

        public JsonWebSignatureException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}