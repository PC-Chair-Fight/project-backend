using FluentValidation;
using project_backend.Models.JobController.AddJob;


namespace project_backend.Models.Validators.JobController.AddJob
{
    public class AddJobQueryValidator : AbstractValidator<AddJobQueryObject>
    {
        public AddJobQueryValidator()
        {
            const string nameMissingError = "Name must not be empty";

            RuleFor(x => x.Name).NotEmpty().WithMessage(nameMissingError);
        }
    }
}
