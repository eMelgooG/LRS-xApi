using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace xApi.Data.Validators
{
    public class StatementRefValidator : AbstractValidator<StatementRef>
    {
        public StatementRefValidator()
        {
            RuleFor(x => x.ObjectType).Equal(ObjectType.StatementRef);
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}