using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace xApi.Data.Validators
{
    public class ScoreValidator : AbstractValidator<Score>
    {
        public ScoreValidator()
        {
            RuleFor(x => x.Raw).LessThanOrEqualTo(x => x.Max)
                .Unless(x => x.Raw == null || x.Max == null);

            RuleFor(x => x.Raw).GreaterThanOrEqualTo(x => x.Min)
                .Unless(x => x.Raw == null || x.Max == null);

            RuleFor(x => x.Max).GreaterThan(x => x.Min)
                .Unless(x => x.Max == null || x.Min == null);

            RuleFor(x => x.Min).LessThan(x => x.Max)
                .Unless(x => x.Min == null || x.Max == null);

            RuleFor(x => x.Scaled).GreaterThanOrEqualTo(-1)
                .LessThanOrEqualTo(1)
                .Unless(x => x.Scaled == null);
        }
    }
}