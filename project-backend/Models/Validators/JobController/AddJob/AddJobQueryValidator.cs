using FluentValidation;
using project_backend.Models.JobController.AddJob;
using System.Linq;

namespace project_backend.Models.Validators.JobController.AddJob
{
    public class AddJobQueryValidator : AbstractValidator<AddJobQueryObject>
    {
        public AddJobQueryValidator()
        {
            const string nameMissingError = "Name must not be empty";


            RuleFor(x => x.Name).NotEmpty().WithMessage(nameMissingError);

            RuleFor(x => x.Images)
                .Must(x => x.Count < 10)
                .Unless(x => x.Images == null)
                .WithMessage("At most 10 images can be uploaded for a single job");
        }
    }
}
