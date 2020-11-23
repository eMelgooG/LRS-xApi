using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace xApi.Data
{
    public class ActivityDefinition
    {
        public Uri Id { get; set; }

        public ActivityDefinition Definition { get; set; }
        public LanguageMap Name { get; set; }
        public LanguageMap Description { get; set; }
        public virtual Uri Type { get; set; }

        /// <summary>
        /// Resolves to a document with human-readable information about the Activity, which could include a way to launch the activity.
        /// </summary>
        public Uri MoreInfo { get; set; }
        public ExtensionsDictionary Extensions { get; set; }
    }
}