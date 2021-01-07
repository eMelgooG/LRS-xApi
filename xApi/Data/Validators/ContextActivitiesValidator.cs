using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace xApi.Data.Validators
{
    public class ContextActivitiesValidator : AbstractValidator<ContextActivities>
    {
        public ContextActivitiesValidator()
        {
            RuleForEach(x => x.Category).SetValidator(new ActivityValidator())
                .When(x => x.Category != null);

            RuleForEach(x => x.Parent).SetValidator(new ActivityValidator())
                .When(x => x.Parent != null);

            RuleForEach(x => x.Grouping).SetValidator(new ActivityValidator())
                .When(x => x.Grouping != null);

            RuleForEach(x => x.Other).SetValidator(new ActivityValidator())
                .When(x => x.Other != null);
        }
    }
}