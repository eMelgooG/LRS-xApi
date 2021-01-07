using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using xApi.Data.Security.Cryptography.Exceptions;

namespace xApi.Data.Security.Cryptography
{
    public class JsonWebSignature
    {
        public JObject ProtectedHeader { get; private set; }
        public string Payload { get; private set; }
        public string Signature { get; private set; }
        public ICollection<JsonWebSignatureException> Errors { get; private set; }

        public JsonWebSignature()
        {
            Errors = new HashSet<JsonWebSignatureException>();
        }

        public static JsonWebSignature Parse(string base64UrlEncoded)
        {
            var jws = new JsonWebSignature();
            try
            {
                var parts = base64UrlEncoded.Split('.');
                if (parts.Length != 3)
                {
                    throw new JsonWebSignatureException("JWS Compact must be made of 3 parts: JOSE header, JWS Payload and JWS Signature.");
                }

                jws.ProtectedHeader = ParseProctedHeader(parts[0]);
                jws.Payload = ParsePayload(parts[1]);
                jws.Signature = Encoding.ASCII.GetString(ParseSignature(parts[2]));
            }
            catch (FormatException ex)
            {
                jws.Errors.Add(new JsonWebSignatureException("Invalid format", ex));
            }
            catch (JsonWebSignatureException ex)
            {
                jws.Errors.Add(ex);
            }
            return jws;
        }

        public string[] GetParts()
        {
            string encodedProtectedHeader = Base64UrlEncode(Encoding.UTF8.GetBytes(ProtectedHeader.ToString()));
            string payload = Base64UrlEncode(Encoding.UTF8.GetBytes(Payload));
            string signature = Base64UrlEncode(Encoding.ASCII.GetBytes(Signature));
            return new string[] { encodedProtectedHeader, payload, signature };
        }

        public string SerializeCompact()
        {
            return string.Join(".", new string[]{
                    Base64UrlEncode(Encoding.UTF8.GetBytes(ProtectedHeader.ToString())),
                  Base64UrlEncode(Encoding.UTF8.GetBytes(Payload)),
                  Base64UrlEncode(Encoding.ASCII.GetBytes(Signature))
            });
        }

        private static string ParsePayload(string base64Url)
        {
            return Encoding.UTF8.GetString(Base64UrlDecode(base64Url));
        }

        private static JObject ParseProctedHeader(string protectedHeader)
        {
            byte[] decoded = Base64UrlDecode(protectedHeader);
            return JObject.Parse(Encoding.UTF8.GetString(decoded));
        }

        private static byte[] ParseSignature(string base64UrlEncodedSignature)
        {
            return Base64UrlDecode(base64UrlEncodedSignature);
        }

        private string GetSigningInput()
        {
            return Encoding.ASCII.GetBytes(
                Base64UrlEncode(Encoding.UTF8.GetBytes(ProtectedHeader.ToString())))
                + "."
                + Base64UrlEncode(Encoding.UTF8.GetBytes(Payload));
        }

        public void Verify()
        {
            try
            {
                var validationResults = new List<IComputationError>();
                var parts = GetParts();
                foreach (var prop in ProtectedHeader.Properties())
                {
                    switch (prop.Name)
                    {
                        case "alg":
                            ALGVerifySignature(prop.Value<string>(), parts);
                            break;
                        default:
                            throw new JsonWebSignatureException($"\"{prop.Name}\" Header Parameter has not been implemented.");
                    }
                }

                throw new JsonWebSignatureException("The JWS signature MUST use an algorithm of \"RS256\", \"RS384\", or \"RS512\".");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void ALGVerifySignature(string algValue, string[] parts)
        {
            switch (algValue)
            {
                case "RS256":
                    RS256VerifySignature(parts);
                    break;
                //case "RS384": return RS384(payload, signature);
                //case "RS512": return RS512(payload, signature);
                default:
                    {
                        Errors.Add(new JsonWebSignatureException($"Invalid ALG value: '{algValue}'"));
                        break;
                    }
            }
        }

        public bool RS256VerifySignature(string[] parts)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            //rsa.ImportParameters(
            //    new RSAParameters()
            //    {
            //        Modulus = Base64UrlDecode("w7Zdfmece8iaB0kiTY8pCtiBtzbptJmP28nSWwtdjRu0f2GFpajvWE4VhfJAjEsOcwYzay7XGN0b-X84BfC8hmCTOj2b2eHT7NsZegFPKRUQzJ9wW8ipn_aDJWMGDuB1XyqT1E7DYqjUCEOD1b4FLpy_xPn6oV_TYOfQ9fZdbE5HGxJUzekuGcOKqOQ8M7wfYHhHHLxGpQVgL0apWuP2gDDOdTtpuld4D2LK1MZK99s9gaSjRHE8JDb1Z4IGhEcEyzkxswVdPndUWzfvWBBWXWxtSUvQGBRkuy1BHOa4sP6FKjWEeeF7gm7UMs2Nm2QUgNZw6xvEDGaLk4KASdIxRQ"),
            //        Exponent = Base64UrlDecode("AQAB")
            //    }
            //);

            SHA256 sha256 = SHA256.Create();
            byte[] hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(parts[1] + '.' + parts[2]));

            RSAPKCS1SignatureDeformatter rsaDeformatter = new RSAPKCS1SignatureDeformatter(rsa);
            rsaDeformatter.SetHashAlgorithm("SHA256");

            byte[] signature = Base64UrlDecode(parts[2]);

            if (rsaDeformatter.VerifySignature(hash, signature))
            {
                return true;
            }
            return false;
        }

        public static string SignData(byte[] data, RSAParameters privateKey)
        {
            //// The array to store the signed message in bytes
            byte[] signedBytes;
            using (var rsa = new RSACryptoServiceProvider())
            {
                //// Write the message to a byte array using UTF8 as the encoding.
                try
                {
                    //// Import the private key used for signing the message
                    rsa.ImportParameters(privateKey);

                    //// Sign the data, using SHA512 as the hashing algorithm
                    signedBytes = rsa.SignData(data, CryptoConfig.MapNameToOID("SHA512"));
                }
                catch (CryptographicException e)
                {
                    Console.WriteLine(e.Message);
                    return null;
                }
                finally
                {
                    //// Set the keycontainer to be cleared when rsa is garbage collected.
                    rsa.PersistKeyInCsp = false;
                }
            }
            //// Convert the a base64 string before returning
            return Convert.ToBase64String(signedBytes);
        }

        public static bool VerifyData(byte[] bytesToVerify, byte[] signedBytes, RSAParameters publicKey)
        {
            bool success = false;
            using (var rsa = new RSACryptoServiceProvider())
            {
                try
                {
                    rsa.ImportParameters(publicKey);

                    SHA512Managed Hash = new SHA512Managed();

                    byte[] hashedData = Hash.ComputeHash(signedBytes);

                    success = rsa.VerifyData(bytesToVerify, CryptoConfig.MapNameToOID("SHA512"), signedBytes);
                }
                catch (CryptographicException e)
                {
                    Console.WriteLine(e.Message);
                }
                finally
                {
                    rsa.PersistKeyInCsp = false;
                }
            }
            return success;
        }

        public static byte[] Base64UrlDecode(string base64Url)
        {
            string unpadded = base64Url.TrimEnd('=');
            string base64 = unpadded.Replace("_", "/")
                                .Replace("-", "+");

            string padded = base64.PadRight((int)Math.Ceiling((double)base64.Length / 4) * 4, '=');

            return Convert.FromBase64String(padded);
        }

        public static string Base64UrlEncode(byte[] bytes)
        {
            string base64 = Convert.ToBase64String(bytes);
            string base64Url = base64.Replace("/", "_")
                                  .Replace("+", "-");

            string padded = base64Url.Length % 4 == 0
               ? base64Url : base64Url + "====".Substring(base64Url.Length % 4);

            return padded;
        }
    }
}