using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace xApi.ApiUtils
{
    public static class DbUtils
    {
        public static string GetConnectionString()
        {
            // Put the name the Sqlconnection from WebConfig..
            return ConfigurationManager.ConnectionStrings["default"].ConnectionString;
        }
    }
}