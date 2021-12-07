using FluentValidation;
using project_backend.Models.JobController.GetJobBids;

namespace project_backend.Models.Validators.JobController.GetJobBids
{
    public class GetJobBidsQueryValidator : AbstractValidator<GetJobBidsQueryObject>
    {
        public GetJobBidsQueryValidator()
        {
            RuleFor(x => x.JobId).NotNull();
            RuleFor(x => x.Index).NotNull().GreaterThanOrEqualTo(0);
            RuleFor(x => x.Count).NotNull().GreaterThan(0);
        }
    }
}
