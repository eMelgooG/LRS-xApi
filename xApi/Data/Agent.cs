using FluentValidation.Attributes;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.ModelBinding;
using xApi.ApiUtils.Binders;
using xApi.Data.Exceptions;
using xApi.Data.Helpers;
using xApi.Data.Interfaces;
using xApi.Data.Json;
using xApi.Data.Json.Exceptions;
using xApi.Data.Validators;

namespace xApi.Data
{
    /// <summary>
    /// An Agent (an individual) is a persona or system.
    /// </summary>
    [ModelBinder(typeof(AgentModelBinder))]
    [Validator(typeof(AgentValidator))]
    public class Agent : StatementObjectBase, IInverseFunctionalIdentifiers, IAgent, IStatementObject
    {
        /// <summary>
        /// Agent. This property is optional except when the Agent is used as a Statement's object.
        /// </summary>
        public new ObjectType ObjectType { get { return OBJECT_TYPE; } }
        protected override ObjectType OBJECT_TYPE => ObjectType.Agent;
        public string Name { get; set; }
        public Mbox Mbox { get; set; }
        public string Mbox_SHA1SUM { get; set; }
        public Iri OpenId { get; set; }

        public Account Account { get; set; }

        public Agent() : base() { }
        public Agent(JsonString jsonString) : this(jsonString.ToJToken(), ApiVersion.GetLatest()) { }
        public Agent(JToken jobj, ApiVersion version) : base(jobj, version)
        {
            GuardType(jobj, JTokenType.Object);

            var objectType = jobj["objectType"];
            if (objectType != null)
            {
                GuardType(objectType, JTokenType.String);
                ParseObjectType(objectType, OBJECT_TYPE);
            }

            var name = jobj["name"];
            if (name != null)
            {
                GuardType(name, JTokenType.String);
                Name = jobj.Value<string>("name");
            }

            var mbox = jobj["mbox"];
            if (mbox != null)
            {
                GuardType(mbox, JTokenType.String);
                try
                {
                    Mbox = new Mbox(mbox.Value<string>());
                }
                catch (MboxFormatException ex)
                {
                    throw new JsonTokenModelException(mbox, ex);
                }
            }

            var mbox_sha1sum = jobj["mbox_sha1sum"];
            if (mbox_sha1sum != null)
            {
                GuardType(mbox_sha1sum, JTokenType.String);
                Mbox_SHA1SUM = mbox_sha1sum.Value<string>();
            }

            var openid = jobj["openid"];
            if (openid != null)
            {
                GuardType(openid, JTokenType.String);
                try
                {
                    OpenId = new Iri(openid.Value<string>());
                }
                catch (IriFormatException ex)
                {
                    throw new JsonTokenModelException(openid, ex);
                }
            }

            var account = jobj["account"];
            if (account != null)
            {
                GuardType(account, JTokenType.Object);
                Account = new Account(account, version);
            }
        }

        public override JToken ToJToken(ApiVersion version, ResultFormat format)
        {
            var jobj = base.ToJToken(version, format);

            if (Name != null && format != ResultFormat.Ids)
            {
                jobj["name"] = Name;
            }

            if (Mbox != null)
            {
                jobj["mbox"] = Mbox.ToString();
            }

            if (Mbox_SHA1SUM != null)
            {
                jobj["mbox_sha1sum"] = Mbox_SHA1SUM;
            }

            if (OpenId != null)
            {
                jobj["openid"] = OpenId.ToString();
            }

            if (Account != null)
            {
                jobj["account"] = Account.ToJToken(version, format);
            }

            return jobj;
        }

        public bool IsAnonymous()
        {
            return (Mbox == null
                && string.IsNullOrEmpty(Mbox_SHA1SUM)
                && Account == null
                && OpenId == null);
        }
        public override bool Equals(object obj)
        {
            var agent = obj as Agent;
            return agent != null &&
                   base.Equals(obj) &&
                   ObjectType == agent.ObjectType &&
                   Name == agent.Name &&
                   EqualityComparer<Mbox>.Default.Equals(Mbox, agent.Mbox) &&
                   Mbox_SHA1SUM == agent.Mbox_SHA1SUM &&
                   EqualityComparer<Iri>.Default.Equals(OpenId, agent.OpenId) &&
                   EqualityComparer<Account>.Default.Equals(Account, agent.Account);
        }
        public override int GetHashCode()
        {
            var hashCode = -790879124;
            hashCode = hashCode * -1521134295 + ObjectType.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<Mbox>.Default.GetHashCode(Mbox);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Mbox_SHA1SUM);
            hashCode = hashCode * -1521134295 + EqualityComparer<Iri>.Default.GetHashCode(OpenId);
            hashCode = hashCode * -1521134295 + EqualityComparer<Account>.Default.GetHashCode(Account);
            return hashCode;
        }
        public static bool operator ==(Agent agent1, Agent agent2)
        {
            return EqualityComparer<Agent>.Default.Equals(agent1, agent2);
        }

        public static bool operator !=(Agent agent1, Agent agent2)
        {
            return !(agent1 == agent2);
        }
    }

}