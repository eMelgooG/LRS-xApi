using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using xApi.Data;
using xApi.Data.Helpers;

namespace xApi.Data
{

    /// <summary>
    /// A Singleton class to provide for information for About Controller
    /// </summary>
    public sealed class About
    {
        private static About instance = null;
        private static readonly object padlock = new object();
        public IEnumerable<string> Version { get; set; }
        public ExtensionsDictionary Extension { get; set; }

        About()
        {
            Version = ApiVersion.GetSupported().Select(x => x.Key);
            Extension = new ExtensionsDictionary();
        }

        public static About Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new About();
                    }
                    return instance;
                }

            }
        }
    }
}
