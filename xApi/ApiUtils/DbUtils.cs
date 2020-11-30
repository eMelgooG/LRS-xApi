using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
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
        /// <summary>
        /// Reads all available bytes from reader
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="ordinal"></param>
        /// <returns></returns>
        public static byte[] GetBytes(SqlDataReader reader, int ordinal)
        {
            byte[] result = new byte[0];

            if (!reader.IsDBNull(ordinal))
            {
                long size = reader.GetBytes(ordinal, 0, null, 0, 0); //get the length of data 
                result = new byte[size];
                reader.GetBytes(ordinal, 0, result, 0, result.Length);
                return result;
            }
            return result;
        }
    }
}