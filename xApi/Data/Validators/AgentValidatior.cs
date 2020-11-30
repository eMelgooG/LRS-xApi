using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace xApi.Data.Validators
{
    public class AgentValidator : AbstractValidator<Agent>
    {
        public AgentValidator()
        {
            RuleFor(x => x.ObjectType).Equal(ObjectType.Agent);

            RuleFor(x => x).Must(MustHaveIdentifier)
                .WithMessage("An Agent MUST be identified by one (1) of the four types of Inverse Functional Identifiers");

            RuleFor(x => x).Must(MustHaveSingleIdentifier)
                .WithMessage("An Agent MUST NOT include more than one (1) Inverse Functional Identifier;");

            RuleFor(x => x.Account).SetValidator(new AccountValidator()).When(x => x.Account != null);
        }

        private bool MustHaveSingleIdentifier(Agent arg)
        {
            int count = 0;

            if (arg.Mbox != null)
            {
                count++;
            }

            if (!string.IsNullOrEmpty(arg.Mbox_SHA1SUM))
            {
                count++;
            }

            if (arg.OpenId != null)
            {
                count++;
            }

            if (arg.Account != null)
            {
                count++;
            }

            return count == 1;
        }

        private bool MustHaveIdentifier(Agent arg)
        {
            if (arg.Mbox != null)
            {
                return true;
            }

            if (!string.IsNullOrEmpty(arg.Mbox_SHA1SUM))
            {
                return true;
            }

            if (arg.OpenId != null)
            {
                return true;
            }

            if (arg.Account != null)
            {
                if (!string.IsNullOrEmpty(arg.Account.Name) && arg.Account.HomePage != null)
                {
                    return true;
                }
            }

            return false;
        }
    }
}