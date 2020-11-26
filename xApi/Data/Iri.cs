using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.ModelBinding;
using xApi.ApiUtils.Binders;
using xApi.Data.Exceptions;


namespace xApi.Data
{
    /// <summary>
    /// Internationalized Resource Identifiers, or IRIs, are unique identifiers which could also be resolvable.
    /// IRIs can contain some characters outside of the ASCII character set.
    /// </summary>
  [ModelBinder(typeof(IriModelBinder))]
    public class Iri
    {
        public string _iriString { get; set; }

        public Iri() { }

        public Iri(string iriString)
        {
            CreateThis(iriString);
        }

        private void CreateThis(string iriString)
        {
            try
            {
                var url = new Uri(iriString);
            }
            catch (UriFormatException)
            {
                throw new IriFormatException($"IRI '{iriString}' is not a well formatted IRI string.");
            }

            _iriString = iriString;
        }

        public override string ToString()
        {
            return _iriString;
        }

        public static Iri Parse(string iriString)
        {
            return new Iri(iriString);
        }

        public static bool TryParse(string iriString, out Iri iri)
        {
            iri = null;
            try
            {
                iri = Parse(iriString);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        /// <summary>
        /// Compute SHA1 hash converted to hex digits
        /// </summary>
        /// <returns></returns>
        public string ComputeHash()
        {
            return SHAHelper.SHA1.ComputeHash(_iriString.ToLowerInvariant());
        }

        public override bool Equals(object obj)
        {
            if (obj is Iri iri)
            {
                return iri != null &&
                   _iriString == iri._iriString;
            }
            else
            {
                return false;
            }
        }

        public static bool operator ==(Iri iri1, Iri iri2)
        {
            return EqualityComparer<Iri>.Default.Equals(iri1, iri2);
        }

        public static bool operator !=(Iri iri1, Iri iri2)
        {
            return !(iri1 == iri2);
        }

    }
}