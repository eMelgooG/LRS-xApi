using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using xApi.Data.Interfaces;

namespace xApi.Data.Validators
{
    public class StatementBaseValidator : AbstractValidator<IStatementBase>
    {
        public StatementBaseValidator()
        {
            RuleFor(x => x.Actor).NotEmpty().DependentRules(() =>
            {
                RuleFor(x => x.Actor)
                    .SetValidator(new AgentValidator())
                    .When(x => x.Actor.ObjectType == ObjectType.Agent);

                RuleFor(x => x.Actor as Group)
                    .SetValidator(new GroupValidator())
                    .When(x => x.Actor.ObjectType == ObjectType.Group);
            });

            RuleFor(x => x.Verb).NotEmpty().SetValidator(new VerbValidator());

            RuleFor(x => x.Object).NotEmpty().DependentRules(() =>
            {
                RuleFor(x => x.Object as Agent)
                    .SetValidator(new AgentValidator())
                    .When(x => x.Object.ObjectType == ObjectType.Agent);

                RuleFor(x => x.Object as Group)
                    .SetValidator(new GroupValidator())
                    .When(x => x.Object.ObjectType == ObjectType.Group);

                RuleFor(x => x.Object as Activity)
                    .SetValidator(new ActivityValidator())
                    .When(x => x.Object.ObjectType == ObjectType.Activity);

                RuleFor(stmt => stmt.Object as StatementRef)
                    .SetValidator(new StatementRefValidator())
                    .When(x => x.Object.ObjectType == ObjectType.StatementRef);
            });

            RuleFor(x => x.Result).SetValidator(new ResultValidator()).When(stmt => stmt.Result != null);
            RuleFor(x => x.Context).SetValidator(new ContextValidator()).When(stmt => stmt.Context != null);
            RuleForEach(x => x.Attachments).SetValidator(new AttachmentValidator()).When(stmt => stmt.Attachments != null);

            RuleFor(x => x).Must(statement =>
            {
                return string.IsNullOrWhiteSpace(statement.Context?.Revision) ||
                    statement.Object?.ObjectType == ObjectType.Activity;
            }).WithMessage("A Statement cannot contain both a 'revision' property in its 'context' property and have the value of the 'object' property's 'objectType' be anything but 'Activity'");

            RuleFor(x => x).Must(statement =>
            {
                return string.IsNullOrWhiteSpace(statement.Context?.Platform) ||
                    statement.Object?.ObjectType == ObjectType.Activity;
            }).WithMessage("A Statement cannot contain both a 'platform' property in its 'context' property and have the value of the 'object' property's 'objectType' be anything but 'Activity'");
        }
    }
}