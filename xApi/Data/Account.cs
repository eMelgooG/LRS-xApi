using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace xApi.Data
{
    public class Account
    {
        public Uri HomePage { get; set; }

        /// <summary>
        /// The unique id or name used to log in to this account. This is based on FOAF's accountName.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets the url with username
        /// </summary>
        /// <returns>Url with username Eg. https://username@www.domain.com </returns>
        public Uri ToUri()
        {
            var uriBuilder = new UriBuilder(HomePage)
            {
                UserName = Name
            };
            return new Uri(uriBuilder.ToString());
        }
    }
}