using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace xApi.Data.Validators
{
 public class SubStatementValidator : AbstractValidator<SubStatement>
    {
        public SubStatementValidator()
        {
            Include(new StatementBaseValidator());

            RuleFor(x => x.Object).NotEmpty().DependentRules(() =>
            {
                RuleFor(x => x.Object.ObjectType).NotEqual(ObjectType.SubStatement)
                .WithMessage("A SubStatement MUST NOT contain a SubStatement of its own, i.e., cannot be nested.")
                .When(x => x.Object.ObjectType == ObjectType.SubStatement);
            });
        }
    }
}