using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using xApi.ApiUtils.Client.Exceptions;
using xApi.ApiUtils.Client.Http;
using xApi.Data;
using xApi.Data.Exceptions;
using xApi.Data.Http;
using xApi.Data.Interfaces;
using xApi.Data.Json;

namespace xApi.ApiUtils.Client
{
    public class JsonModelReader
    {
        public readonly Stream Body;
        public readonly HttpContentHeaders Headers;
        public MediaTypeHeaderValue ContentType { get; }

        public JsonModelReader(HttpContentHeaders headers, Stream bodyStream)
        {
            Headers = headers;
            Body = bodyStream;
            ContentType = GetContentType();
        }

        /// <summary>
        /// Read the stream as TResult
        /// </summary>
        /// <exception cref="JsonModelReaderException"></exception>
        public async Task<TResult> ReadAs<TResult>()
            where TResult : IJsonModel, IAttachmentByHash, new()
        {
            if (IsApplicationJson(ContentType.MediaType))
            {
                return (TResult)ReadAs<TResult>(await ReadAsJson(Body));
            }
            else if (ContentType.MediaType == MediaTypes.Multipart.Mixed)
            {
                return await ReadAsMultipart<TResult>();
            }

            throw new JsonModelReaderException($"Content-Type header must be {MediaTypes.Application.Json} or {MediaTypes.Multipart.Mixed}");
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        /// <exception cref="JsonModelReaderException"></exception>
        private async Task<TResult> ReadAsMultipart<TResult>() where TResult : IJsonModel, IAttachmentByHash, new()
        {
            var result = new TResult();

            string boundary = GetBoundary();

            var multipartReader = new MultipartReader(boundary, Body);
            int sectionIndex = 0;
            while (true)
            {
                MultipartSection section = await multipartReader.ReadNextSectionAsync();
                if (section == null)
                {
                    break;
                }
                var sectionContentType = ParseContentType(section.ContentType);

                if (sectionIndex == 0)
                {
                    if (!IsApplicationJson(sectionContentType.MediaType))
                    {
                        throw new JsonModelReaderException($"First part must have a Content-Type header value of \"{MediaTypes.Application.Json}\".");
                    }

                    try
                    {
                        result = (TResult)ReadAs<TResult>(await ReadAsJson(section.Body));
                    }
                    catch (MultipartSectionException ex)
                    {
                        throw new JsonModelReaderException("", ex);
                    }
                }
                else
                {
                    MultipartAttachmentSection attachmentSection;
                    try
                    {
                        attachmentSection = new MultipartAttachmentSection(section);
                    }
                    catch (MultipartSectionException ex)
                    {
                        throw new JsonModelReaderException($"Invalid Multipart attachment section.", ex);
                    }
                    string hash = attachmentSection.XExperienceApiHash;

                    var attachment = result.GetAttachmentByHash(hash);
                    if (attachment != null)
                    {
                        attachment.SetPayload(await attachmentSection.ReadAsByteArrayAsync());
                    }
                    else
                    {
                        throw new JsonModelReaderException($"Header '{Data.Http.ExperienceApiHeaders.XExperienceApiHash}: {hash}' does not match any attachments.");
                    }
                }
                sectionIndex++;
            }

            return result;
        }

        private async Task<JsonString> ReadAsJson(Stream jsonStream)
        {
            using (StreamReader streamReader = new StreamReader(jsonStream, Encoding.UTF8))
            {
                return await streamReader.ReadToEndAsync();
            }
        }

        private IJsonModel ReadAs<TResult>(JsonString jsonString)
            where TResult : IJsonModel, IAttachmentByHash, new()
        {
            Type type = typeof(TResult);

            try
            {
                if (type == typeof(Statement))
                {
                    return new Statement(jsonString);
                }
                else if (type == typeof(StatementCollection))
                {
                    return new StatementCollection(jsonString);
                }
                else if (type == typeof(StatementsResult))
                {
                    return new StatementsResult(jsonString);
                }
            }
            catch (ExperienceDataException ex)
            {
                throw new JsonModelReaderException($"Error while trying to read json as {type.Name}", ex);
            }

            return null;
        }

        private MediaTypeHeaderValue GetContentType()
        {
            if (Headers.TryGetValues("Content-Type", out IEnumerable<string> values))
            {
                return ParseContentType(values.FirstOrDefault());
            }

            throw new JsonModelReaderException("Content-Type header was not found");
        }

        private static MediaTypeHeaderValue ParseContentType(string headerValue)
        {
            try
            {
                return MediaTypeHeaderValue.Parse(headerValue.ToString());
            }
            catch (FormatException ex)
            {
                throw new JsonModelReaderException(ex.Message, ex);
            }
        }

        private static bool IsApplicationJson(string contentType)
        {
            var parsedValue = ParseContentType(contentType);
            return parsedValue.MediaType == MediaTypes.Application.Json;
        }

        private string GetBoundary()
        {
            var boundary = ContentType.Parameters.FirstOrDefault(x => x.Name == "boundary");
            if (boundary == null || string.IsNullOrWhiteSpace(boundary.Value))
            {
                throw new JsonModelReaderException("Content-Type parameter boundary is null or empty.");
            }

            return boundary.Value;
        }
    }
}