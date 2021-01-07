using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using xApi.Data.Helpers;

namespace xApi.Data.Validators
{
    public class ContextValidator : AbstractValidator<Context>
    {
        public ContextValidator()
        {
            RuleFor(x => x.Instructor).NotEmpty().DependentRules(() =>
            {
                RuleFor(x => x.Instructor).SetValidator(new AgentValidator())
                .When(x => x.Instructor.ObjectType == ObjectType.Agent);

                RuleFor(x => x.Instructor as Group).SetValidator(new GroupValidator())
                .When(x => x.Instructor.ObjectType == ObjectType.Group);
            })
                .When(x => x.Instructor != null);

            RuleFor(x => x.Team as Group).SetValidator(new GroupValidator())
                .When(x => x.Team != null);

            RuleFor(x => x.Statement).SetValidator(new StatementRefValidator())
                .When(x => x.Statement != null);

            RuleFor(x => x.Extensions).SetValidator(new ExtensionsValidator())
                .When(x => x.Extensions != null);

            RuleFor(x => x.ContextActivities).SetValidator(new ContextActivitiesValidator())
                .When(x => x.ContextActivities != null);

            RuleFor(x => x.Language).Must(x => CultureHelper.IsValidCultureName(x))
                .When(x => !string.IsNullOrEmpty(x.Language))
                .WithMessage("Invalid ");
        }
    }
}