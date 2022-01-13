using FluentValidation;
using project_backend.Models.JobController.EditJob;

namespace project_backend.Models.Validators.JobController.EditJob
{
    public class EditJobQueryValidator : AbstractValidator<EditJobQueryObject>
    {
        public EditJobQueryValidator()
        {
            const string nameMissingError = "Name must not be empty";
            const string idMissingError = "Id must not be empty";

            RuleFor(x => x.Name).NotEmpty().WithMessage(nameMissingError);
            RuleFor(x => x.Id).NotEmpty().WithMessage(idMissingError);
        }
    }
}
