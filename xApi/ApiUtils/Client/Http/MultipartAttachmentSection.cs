using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using xApi.Data.Exceptions;
using xApi.Data.Http;

namespace xApi.ApiUtils.Client.Http
{
    public class MultipartAttachmentSection
    {
        private readonly MultipartSection section;
        public string XExperienceApiHash => GetExperienceApiHash();
        public string ContentTransferEncoding => GetContentTransferEncoding();

        private string GetContentTransferEncoding()
        {
            if (section.Headers.TryGetValue(ExperienceApiHeaders.ContentTransferEncoding, out StringValues cteValues))
            {
                return cteValues;
            }
            return null;
        }

        private string GetExperienceApiHash()
        {
            if (section.Headers.TryGetValue(ExperienceApiHeaders.XExperienceApiHash, out StringValues hashValues))
            {
                return hashValues;
            }
            return null;
        }

        public MultipartAttachmentSection(MultipartSection section)
        {
            this.section = section;


            if (!string.IsNullOrEmpty(ContentTransferEncoding))
            {
                if (ContentTransferEncoding != "binary")
                {
                    throw new MultipartSectionException($"MUST include a {ExperienceApiHeaders.ContentTransferEncoding} parameter with a value of binary in each part's header after the first (Statements) part.");
                }
            }
            else
            {
                throw new MultipartSectionException($"{ExperienceApiHeaders.ContentTransferEncoding}'' header is missing.");
            }

            if (string.IsNullOrEmpty(XExperienceApiHash))
            {
                // MUST include a Content-Transfer-Encoding parameter with a value of binary in each part's header after the first (Statements) part.
                throw new MultipartSectionException($"'{ExperienceApiHeaders.XExperienceApiHash}' is missing.");
            }
        }

        public async Task<byte[]> ReadAsByteArrayAsync()
        {
            using (StreamReader sr = new StreamReader(section.Body))
            {
                return Encoding.UTF8.GetBytes(await sr.ReadToEndAsync());
            }
        }
    }
}