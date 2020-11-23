using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using xApi.Data.Exceptions;

namespace xApi.Data
{
    public class Mbox
    {
        const string emailPattern =
                  @"([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})";
        private readonly string _mbox = null;
        public Mbox() { }
        public Mbox(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new MboxFormatException("value");
            }

            if (!value.StartsWith("mailto:"))
            {
                throw new MboxFormatException("Must start with 'mailto:'");
            }

            var email = value.Split(new char[] { ':' })[1];
            var match = Regex.Match(email, emailPattern);
            if (!match.Success)
            {
                throw new MboxFormatException($"'{email}' is not a valid e-mail.");
            }

            _mbox = value;
        }

    }
}