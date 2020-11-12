
using System;
using System.Collections.Generic;
using xApi.Data.Exceptions;

namespace xApi.Data.Helpers
{
    public sealed class ApiVersion
    {
        private readonly string _version;

        public static readonly ApiVersion V103 = new ApiVersion("1.0.3");
        public static readonly ApiVersion V102 = new ApiVersion("1.0.2");
        public static readonly ApiVersion V101 = new ApiVersion("1.0.1");
        public static readonly ApiVersion V100 = new ApiVersion("1.0.0");
        public static readonly ApiVersion V095 = new ApiVersion("0.95");
        public static readonly ApiVersion V090 = new ApiVersion("0.9");

        public static ApiVersion GetLatest()
        {
            return V103;
        }

        private static Dictionary<string, ApiVersion> KnownVersions;

        public static bool TryParse(string s, out ApiVersion version)
        {
            try
            {
                version = new ApiVersion(s);
                return true;
            }
            catch (Exception)
            {
                version = null;
                return false;
            }
        }

        private static Dictionary<string, ApiVersion> SupportedVersions;

        public static Dictionary<string, ApiVersion> GetKnownVersions()
        {
            if (KnownVersions != null)
            {
                return KnownVersions;
            }

            KnownVersions = new Dictionary<string, ApiVersion>
            {
                { "1.0.3", V103 },
                { "1.0.2", V102 },
                { "1.0.1", V101 },
                { "1.0.0", V100 },
                { "0.95", V095 },
                { "0.9", V090 }
            };

            return KnownVersions;
        }

        public static Dictionary<string, ApiVersion> GetSupported()
        {
            if (SupportedVersions != null)
            {
                return SupportedVersions;
            }

            SupportedVersions = new Dictionary<string, ApiVersion>
            {
                { "1.0.3", V103 },
                { "1.0.2", V102 },
                { "1.0.1", V101 },
                { "1.0.0", V100 }
            };

            return SupportedVersions;
        }

        public ApiVersion(string version)
        {
            if (string.IsNullOrWhiteSpace(version))
            {
                throw new VersionFormatException("Version is null or empty");
            }

            _version = version;

            if (version.StartsWith("1.0.") || version.Equals("1.0"))
            {
                // Accept version
            }

            else if (!GetKnownVersions().ContainsKey(version))
            {
                throw new VersionFormatException("Unrecognized version: " + version);
            }
        }

        public static implicit operator ApiVersion(string version)
        {
            return new ApiVersion(version);
        }

        public override string ToString()
        {
            return _version;
        }

        public override bool Equals(object obj)
        {
            var version = obj as ApiVersion;
            return version != null &&
                   _version == version._version;
        }

        public override int GetHashCode()
        {
            return -930009502 + EqualityComparer<string>.Default.GetHashCode(_version);
        }

        public static bool operator ==(ApiVersion version1, ApiVersion version2)
        {
            return EqualityComparer<ApiVersion>.Default.Equals(version1, version2);
        }

        public static bool operator !=(ApiVersion version1, ApiVersion version2)
        {
            return !(version1 == version2);
        }
    }
}
