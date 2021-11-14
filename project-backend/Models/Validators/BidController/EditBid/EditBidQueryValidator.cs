using FluentValidation;
using project_backend.Models.BidController.EditBid;

namespace project_backend.Models.Validators.BidController.EditJob
{
    public class EditBidQueryValidator : AbstractValidator<EditBidQueryObject>
    {
        public EditBidQueryValidator()
        {
            const string negativeSumError = "Sum must be positive";
            const string sumMissingError = "Sum must not be null";
            const string bidIdMissingError = "Bid id must not be null";

            RuleFor(x => x.Sum).NotNull().WithMessage(sumMissingError).GreaterThanOrEqualTo(0).WithMessage(negativeSumError);
            RuleFor(x => x.BidId).NotNull().WithMessage(bidIdMissingError);
        }
    }
}
