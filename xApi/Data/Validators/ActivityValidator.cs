using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace xApi.Data.Validators
{
    public class ActivityValidator : AbstractValidator<Activity>
    {
        public ActivityValidator()
        {
            RuleFor(x => x.Id).NotEmpty();

            RuleFor(x => x.Definition).SetValidator(new ActivityDefinitionValidator())
                .When(x => x.Definition != null);

            RuleFor(x => x.Definition as InteractionTypes.InteractionActivityDefinitionBase).NotEmpty()
                .DependentRules(() =>
                {
                    RuleFor(x => x.Definition as InteractionTypes.Choice).NotEmpty()
                        .When(x => x.Definition is InteractionTypes.Choice);

                    RuleFor(x => x.Definition as InteractionTypes.FillIn).NotEmpty()
                        .When(x => x.Definition is InteractionTypes.FillIn);

                    RuleFor(x => x.Definition as InteractionTypes.Likert).NotEmpty()
                        .When(x => x.Definition is InteractionTypes.Likert);

                    RuleFor(x => x.Definition as InteractionTypes.LongFillIn).NotEmpty()
                        .When(x => x.Definition is InteractionTypes.LongFillIn);

                    RuleFor(x => x.Definition as InteractionTypes.Matching).NotEmpty()
                        .When(x => x.Definition is InteractionTypes.Matching);

                    RuleFor(x => x.Definition as InteractionTypes.Numeric).NotEmpty()
                        .When(x => x.Definition is InteractionTypes.Numeric);

                    RuleFor(x => x.Definition as InteractionTypes.Other).NotEmpty()
                        .When(x => x.Definition is InteractionTypes.Other);

                    RuleFor(x => x.Definition as InteractionTypes.Performance).NotEmpty()
                        .When(x => x.Definition is InteractionTypes.Performance);

                    RuleFor(x => x.Definition as InteractionTypes.Sequencing).NotEmpty()
                        .When(x => x.Definition is InteractionTypes.Sequencing);

                    RuleFor(x => x.Definition as InteractionTypes.TrueFalse).NotEmpty()
                        .When(x => x.Definition is InteractionTypes.TrueFalse);
                })
                .When(x => x.Definition is InteractionTypes.InteractionActivityDefinitionBase);
        }
    }
}