using Newtonsoft.Json;
using System;

namespace project_backend.Models.JobController.GetJobs
{
    public class GetJobsQueryObject
    {
        public bool? ByCurrentUserOnly;

        public OrderField[] OrderBy { get; set; }
        public bool[] Ascending { get; set; }

        public FilterField[] FilterFields { get; set; }
        public string[] FilterValues { get; set; }
        public bool[] ExactFilters { get; set; }

        public int Index { get; set; }
        public int Count { get; set; }

        public DateTime? OlderThan { get; set; }
        public DateTime? NewerThan { get; set; }

        public override string ToString() => JsonConvert.SerializeObject(this);

        public enum OrderField
        {
            Id,
            Name,
            Description,
            PostDate
        }

        public enum FilterField
        {
            Id,
            Name,
            Description,
            Done
        }
    }
}
