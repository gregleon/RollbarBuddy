using Newtonsoft.Json;

namespace RollbarBuddy.Models.Rollbar
{
    public class ApiJobPostResponse
    {
        [JsonProperty("result")]
        public ApiPostJobResponseResult Result { get; set; }

        [JsonProperty("err")]
        public long Err { get; set; }
    }

    public class ApiPostJobResponseResult
    {
        [JsonProperty("date_created")]
        public long DateCreated { get; set; }

        [JsonProperty("date_modified")]
        public long DateModified { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("job_hash")]
        public string JobHash { get; set; }

        [JsonProperty("project_group_id")]
        public long ProjectGroupId { get; set; }

        [JsonProperty("project_id")]
        public long ProjectId { get; set; }

        [JsonProperty("query_string")]
        public string QueryString { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }
}