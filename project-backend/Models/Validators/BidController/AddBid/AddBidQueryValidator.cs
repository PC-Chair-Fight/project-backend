using FluentValidation;
using project_backend.Models.BidController.AddBid;

namespace project_backend.Models.Validators.BidController.AddBid
{
    public class AddBidQueryValidator : AbstractValidator<AddBidQueryObject>
    {
        public AddBidQueryValidator()
        {
            const string negativeSumError = "Sum must be positive";
            const string sumMissingError = "Sum must not be null";
            const string jobIdMissingError = "Job id must not be null";

            RuleFor(x => x.Sum).NotNull().WithMessage(sumMissingError).GreaterThanOrEqualTo(0).WithMessage(negativeSumError);
            RuleFor(x => x.JobId).NotNull().WithMessage(jobIdMissingError);
        }
    }
}
