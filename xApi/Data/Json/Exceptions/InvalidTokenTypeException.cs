using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace xApi.Data.Json.Exceptions
{
    public class InvalidTokenTypeException : JsonTokenModelException
    {
        public JTokenType[] Types;

        public InvalidTokenTypeException(JToken token, JTokenType type)
            : base(token, FormatMessage(token, type))
        {
            Types = new[] { type };
        }

        public InvalidTokenTypeException(JToken token, JTokenType[] types)
            : base(token, FormatMessage(token, types))
        {
            Types = types;
        }

        public static string FormatMessage(JToken token, params JTokenType[] types)
        {
            string message = "Expected JSON ";
            if (types.Length > 1)
            {
                message += $"types '{string.Join(", ", types.Select(x => Enum.GetName(typeof(JTokenType), x)).ToArray())}'";
            }
            else
            {
                message += $"type '{Enum.GetName(typeof(JTokenType), types[0])}'";
            }

            if (token != null)
            {
                message += $", but received '{Enum.GetName(typeof(JTokenType), token.Type)}'.";
            }
            else
            {
                message += ", but received nothing.";
            }

            return message;
        }
    }
}