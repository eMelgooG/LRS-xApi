using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace xApi.Data.Validators
{
    public class VerbValidator : AbstractValidator<Verb>
    {
        public VerbValidator()
        {
            RuleFor(x => x.Id).NotNull();
            RuleFor(x => x.Display).SetValidator(new LanguageMapValidator()).When(verb => verb.Display != null);
        }
    }
}