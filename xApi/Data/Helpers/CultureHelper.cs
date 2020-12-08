using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace xApi.Data.Helpers
{
    /// <summary>
    /// ISO 639 language code
    /// </summary>
    public class CultureHelper
    {
        private static List<string> _cultureNames;

        /// <summary>
        /// Contains a list of valid RFC5646 Language Tag's
        /// </summary>
        public static List<string> CultureNames
        {
            get
            {
                if (_cultureNames == null)
                {
                    _cultureNames = new List<string>() {
                        "und",
                        "ase",
                        "tlh",
                        "cmn",

                        // Language subtag plus Script subtag:
                        "zh-Hant", // (Chinese written using the Traditional Chinese script)
                        "zh-Hans", // (Chinese written using the Simplified Chinese script)
                        "sr-Cyrl", // (Serbian written using the Cyrillic script)
                        "sr-Latn", // (Serbian written using the Latin script)

                         // Extended language subtags and their primary language subtag counterparts:
                        "zh-cmn-Hans-CN", // (Chinese, Mandarin, Simplified script, as used in China)
                        "cmn-Hans-CN", // (Mandarin Chinese, Simplified script, as used in China)
                        "zh-yue-HK", // (Chinese, Cantonese, as used in Hong Kong SAR)
                        "yue-HK", // (Cantonese Chinese, as used in Hong Kong SAR)

                        // Language-Script-Region:
                        "zh-Hans-CN", // (Chinese written using the Simplified script as used in mainland China)
                        "sr-Latn-RS" // (Serbian written using the Latin script as used in Serbia)
                    };
                    _cultureNames.AddRange(CultureInfo.GetCultures(CultureTypes.AllCultures).Select(x => x.Name).ToList());
                }

                return _cultureNames;
            }
        }

        /// <summary>
        /// Validates the culture name according 
        /// </summary>
        /// <param name="cultureName"></param>
        /// <returns></returns>
        public static bool IsValidCultureName(string cultureName)
        {
            return CultureNames.Contains(cultureName);
        }
    }
}
