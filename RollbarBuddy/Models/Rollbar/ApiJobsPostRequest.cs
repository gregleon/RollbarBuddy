using Newtonsoft.Json;

namespace RollbarBuddy.Models.Rollbar
{
    public class ApiJobsPostRequest
    {
        [JsonProperty("query_string")]
        public string QueryString { get; set; }

        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
    }
}
