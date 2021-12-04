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
            const string orderByErrorMessage = "OrderBy and Ascending must both be null or they must have the same length";

            // all filters are null or none is
            RuleFor(x => x.FilterValues).NotEmpty().Unless(x => x.FilterFields == null && x.ExactFilters == null).WithMessage(filtersErrorMessage);
            RuleFor(x => x.ExactFilters).NotEmpty().Unless(x => x.FilterFields == null && x.FilterValues == null).WithMessage(filtersErrorMessage);
            RuleFor(x => x.FilterFields).NotEmpty().Unless(x => x.ExactFilters == null && x.FilterValues == null).WithMessage(filtersErrorMessage);

            // the lengths of the filters are equal
            RuleFor(x => x.FilterValues.Length)
                .Equal(x => x.FilterFields.Length).WithMessage(filtersErrorMessage)
                .Equal(x => x.ExactFilters.Length).WithMessage(filtersErrorMessage)
                .When(x => x.FilterValues != null && x.FilterFields != null && x.ExactFilters != null);

            // similar checks for OrderBy and Ascending
            RuleFor(x => x.OrderBy).NotEmpty().Unless(x => x.Ascending == null).WithMessage(orderByErrorMessage);
            RuleFor(x => x.Ascending).NotEmpty().Unless(x => x.OrderBy == null).WithMessage(orderByErrorMessage);
            RuleFor(x => x.OrderBy.Length).Equal(x => x.Ascending.Length)
                .Unless(x => x.OrderBy == null || x.Ascending == null)
                .WithMessage(orderByErrorMessage);

            RuleForEach(x => x.FilterValues.Zip(x.FilterFields))
                .Must(x =>
                {
                    var field = x.Second;
                    var value = x.First;
                    return field switch
                    {
                        // must be able to parse int
                        GetJobsQueryObject.FilterField.Id => int.TryParse(value, out int dummyInt),
                        // must be able to parse bool
                        GetJobsQueryObject.FilterField.Done => bool.TryParse(value, out bool dummyBool),
                        _ => true
                    };
                })
                .WithMessage("Filter values must be string representations of values of their respective types")
                .Unless(x => x.FilterFields == null || x.FilterValues == null)
                .OverridePropertyName("FilterValues")
                .OverridePropertyName("FilterFields");

            RuleFor(x => x.OlderThan).GreaterThanOrEqualTo(x => x.NewerThan).When(x => x.OlderThan != null && x.NewerThan != null);

            RuleFor(x => x.FilterFields)
                .Must(x => x.Distinct().Count() == x.Length)
                .Unless(x => x.FilterFields == null)
                .WithMessage("FilterFields must be unique");

            RuleFor(x => x.Index).NotEmpty().GreaterThanOrEqualTo(0);
            RuleFor(x => x.Count).NotEmpty().GreaterThan(0);
        }
    }
}
