using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace xApi.Data.Extensions
{
    public static class StringExtensions
    {
        public static string EnsureEndsWith(this string str, string endWith)
        {
            if (!str.EndsWith(endWith))
            {
                return str + endWith;
            }

            return str;
        }
    }
}