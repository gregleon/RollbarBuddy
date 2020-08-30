using System.Collections.Generic;
using Newtonsoft.Json;

namespace RollbarBuddy.Models.Rollbar
{
    public partial class ApiRqlResponse
    {
        [JsonProperty("err")]
        public long Err { get; set; }

        [JsonProperty("result")]
        public ApiRqlResponseResult Result { get; set; }
    }

    public partial class ApiRqlResponseResult
    {
        [JsonProperty("job_id")]
        public long JobId { get; set; }

        [JsonProperty("result")]
        public ApiRqlResponseInnerResult InnerResult { get; set; }
    }

    public partial class ApiRqlResponseInnerResult
    {
        [JsonProperty("isSimpleSelect")]
        public bool IsSimpleSelect { get; set; }

        [JsonProperty("errors")]
        public List<object> Errors { get; set; }

        [JsonProperty("warnings")]
        public List<string> Warnings { get; set; }

        [JsonProperty("executionTime")]
        public double ExecutionTime { get; set; }

        [JsonProperty("effectiveTimestamp")]
        public long EffectiveTimestamp { get; set; }

        [JsonProperty("rowcount")]
        public long Rowcount { get; set; }

        [JsonProperty("rows")]
        public List<List<string>> Rows { get; set; }

        [JsonProperty("selectionColumns")]
        public List<string> SelectionColumns { get; set; }

        [JsonProperty("projectIds")]
        public List<long> ProjectIds { get; set; }

        [JsonProperty("columns")]
        public List<string> Columns { get; set; }
    }
}