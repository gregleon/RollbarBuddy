using Newtonsoft.Json;

namespace RollbarBuddy.Models.Rollbar
{
    public class ApiJobsGetResponse
    {
        [JsonProperty("err")]
        public long Err { get; set; }

        [JsonProperty("result")]
        public ApiJobsGetResponseResult Result { get; set; }
    }

    public class ApiJobsGetResponseResult
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("job_hash")]
        public string JobHash { get; set; }

        [JsonProperty("date_modified")]
        public long DateModified { get; set; }

        [JsonProperty("query_string")]
        public string QueryString { get; set; }

        [JsonProperty("date_created")]
        public long DateCreated { get; set; }

        [JsonProperty("project_id")]
        public long ProjectId { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("project_group_id")]
        public long ProjectGroupId { get; set; }

        public bool IsSuccess => Status == "success";
    }
}