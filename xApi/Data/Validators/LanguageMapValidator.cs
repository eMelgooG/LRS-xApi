using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using xApi.Data.Helpers;

namespace xApi.Data.Validators
{
    public class LanguageMapValidator : AbstractValidator<LanguageMap>
    {
        public LanguageMapValidator()
        {
            RuleForEach(x => x).Must(x => CultureHelper.IsValidCultureName(x.Key))
                .WithMessage("Invalid culture name.");
        }
    }
}