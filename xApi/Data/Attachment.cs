using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using xApi.Data.Helpers;
using xApi.Data.Json;
using xApi.Data.Json.Exceptions;

namespace xApi.Data
{
    [JsonObject]
    public class Attachment : JsonModel
    {
        public Attachment() { }
        public Attachment(JsonString jsonString) : this(jsonString.ToJToken(), ApiVersion.GetLatest()) { }
        public Attachment(JToken jobj, ApiVersion version)
        {
            GuardType(jobj, JTokenType.Object);

            var usageType = jobj["usageType"];
            if (usageType != null)
            {
                GuardType(usageType, JTokenType.String);
                UsageType = new Iri(usageType.Value<string>());
            }

            var display = jobj["display"];
            if (display != null)
            {
                Display = new LanguageMap(display, version);
            }

            var description = jobj["description"];
            if (description != null)
            {
                Description = new LanguageMap(description, version);
            }

            var contentType = jobj["contentType"];
            if (contentType != null)
            {
                GuardType(contentType, JTokenType.String);
                ContentType = contentType.Value<string>();
            }

            var length = jobj["length"];
            if (length != null)
            {
                GuardType(length, JTokenType.Integer);
                Length = length.Value<int>();
            }

            var sha2 = jobj["sha2"];
            if (sha2 != null)
            {
                GuardType(sha2, JTokenType.String);
                SHA2 = sha2.Value<string>();
            }

            var fileUrl = jobj["fileUrl"];
            if (fileUrl != null)
            {
                GuardType(fileUrl, JTokenType.String);
                try
                {
                    FileUrl = new Uri(fileUrl.Value<string>());
                }
                catch (UriFormatException ex)
                {
                    throw new JsonTokenModelException(fileUrl, ex);
                }
            }
        }

        /// <summary>
        /// Identifies the usage of this Attachment. 
        /// For example: one expected use case for Attachments is to include a "completion certificate". 
        /// An IRI corresponding to this usage MUST be coined, and used with completion certificate attachments.
        /// </summary>
        public Iri UsageType { get; set; }

        /// <summary>
        /// Display name (title) of this Attachment.
        /// </summary>
        public LanguageMap Display { get; set; }

        /// <summary>
        /// A description of the Attachment
        /// </summary>
        public LanguageMap Description { get; set; }

        /// <summary>
        /// The content type of the Attachment.
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// The length of the Attachment data in octets. (Content-Type)
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// The SHA-2 hash of the Attachment data. (X-Experience-API-Hash)
        /// This property is always required, even if fileURL is also specified.
        /// </summary>
        public string SHA2 { get; set; }

        /// <summary>
        /// An IRL at which the Attachment data can be retrieved, or from which it used to be retrievable.
        /// </summary>
        public Uri FileUrl { get; set; }

        /// <summary>
        /// The byte array of Attachment data. (Use SetPayload method)
        /// </summary>
        [JsonIgnore]
        public byte[] Payload { get; private set; }

        /// <summary>
        /// Sets the Attachment data payload.
        /// </summary>
        /// <param name="s">The string representing the payload.</param>
        public void SetPayload(string s)
        {
            SetPayload(System.Text.Encoding.UTF8.GetBytes(s));
        }

        /// <summary>
        /// Sets the Attachment data payload.
        /// </summary>
        /// <param name="bytes">The bytes representing the payload.</param>
        public void SetPayload(byte[] bytes)
        {
            Payload = bytes;

            if (!string.IsNullOrEmpty(SHA2))
            {
                SHA2 = SHAHelper.SHA2.ComputeHash(bytes);
            }

            if (Length <= 0)
            {
                Length = bytes.Length;
            }
        }

        public override JToken ToJToken(ApiVersion version, ResultFormat format)
        {
            var obj = new JObject();

            obj["usageType"] = UsageType.ToString();

            if (Display != null)
            {
                obj["dispay"] = Display.ToJToken(version, format);
            }

            if (Description != null)
            {
                obj["description"] = Description.ToJToken(version, format);
            }

            obj["contentType"] = ContentType;

            obj["length"] = Length;

            obj["sha2"] = SHA2;

            if (FileUrl != null)
            {
                obj["fileUrl"] = FileUrl.ToString();
            }

            return obj;
        }
    }
}