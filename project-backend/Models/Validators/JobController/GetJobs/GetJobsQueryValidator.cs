using FluentValidation;
using project_backend.Models.JobController.GetJobs;
using System.Linq;

namespace project_backend.Models.Validators.JobController.GetJobs
{
    public class GetJobsQueryValidator : AbstractValidator<GetJobsQueryObject>
    {
        public GetJobsQueryValidator()
        {
            const string filtersErrorMessage = "FilterValues, ExactFilters, FilterFields must be all null or they must have the same length";

            // all filters are null or none is
            RuleFor(x => x.FilterValues).NotEmpty().Unless(x => x.FilterFields == null && x.ExactFilters == null).WithMessage(filtersErrorMessage);
            RuleFor(x => x.ExactFilters).NotEmpty().Unless(x => x.FilterFields == null && x.FilterValues == null).WithMessage(filtersErrorMessage);
            RuleFor(x => x.FilterFields).NotEmpty().Unless(x => x.ExactFilters == null && x.FilterValues == null).WithMessage(filtersErrorMessage);

            // the lengths of the filters are equal
            RuleFor(x => x.FilterValues.Length)
                .Equal(x => x.FilterFields.Length).WithMessage(filtersErrorMessage)
                .Equal(x => x.ExactFilters.Length).WithMessage(filtersErrorMessage)
                .When(x => x.FilterValues != null && x.FilterFields != null && x.ExactFilters != null);

            RuleForEach(x => x.FilterValues.Zip(x.FilterFields))
                .Must(x =>
                {
                    var field = x.Second;
                    var value = x.First;
                    switch (field)
                    {
                        case GetJobsQueryObject.FilterField.Id: // must be able to parse int
                            return int.TryParse(value, out int dummyInt);
                        case GetJobsQueryObject.FilterField.Done: // must be able to parse bool
                            return bool.TryParse(value, out bool dummyBool);
                    }
                    return true;
                })
                .WithMessage("Filter values must be string representations of values of their respective types");

            RuleFor(x => x.OlderThan).GreaterThanOrEqualTo(x => x.NewerThan).When(x => x.OlderThan != null && x.NewerThan != null);

            RuleFor(x => x.FilterFields)
                .Must(x => x.Distinct().Count() == x.Length)
                .WithMessage("FilterFields must be unique");

            RuleFor(x => x.Index).NotEmpty().GreaterThanOrEqualTo(0);
            RuleFor(x => x.Count).NotEmpty().GreaterThan(0);

            RuleFor(x => x.OrderBy).NotEmpty().When(x => x.OrderAscending != null)
                .WithMessage("If providing OrderAscending, OrderBy must also be given");
        }
    }
}
