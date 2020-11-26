using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using xApi.Data.Extensions;

namespace xApi.Data.Json.Exceptions
{
    public class JsonTokenModelException : JsonModelException
    {
        public JsonTokenModelException(JToken token, string message)
          : base(FormatMessage(message, token))
        {
            Token = token;
        }

        public JsonTokenModelException(JToken token, Exception innerException)
            : base(FormatMessage(innerException.Message, token), innerException)
        {
            Token = token;
        }

        private static string FormatMessage(string message, JToken token)
        {
            message = message.TrimEnd().EnsureEndsWith(".");
            if (token != null)
            {
                message += $" Path: '{token.Path}'";
            }
            return message;
        }

        public JToken Token { get; }
    
}
}