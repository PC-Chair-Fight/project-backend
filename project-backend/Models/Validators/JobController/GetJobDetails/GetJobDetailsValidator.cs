using FluentValidation;
using project_backend.Models.JobController.GetJobDetails;

namespace project_backend.Models.Validators.JobController.GetJobDetails
{
    public class GetJobDetailsValidator : AbstractValidator<GetJobDetailsQueryObject>
    {
        public GetJobDetailsValidator()
        {
        }
    }
}
