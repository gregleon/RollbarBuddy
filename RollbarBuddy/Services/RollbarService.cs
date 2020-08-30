using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RollbarBuddy.Models.Rollbar;
using RollbarBuddy.Utils;

namespace RollbarBuddy.Services
{
    public class RollbarService
    {
        private readonly string _team;
        private readonly string _project;
        private readonly string _accessToken;

        private readonly HttpClient _httpClient;

        public RollbarService(string team, string project, string access_token)
        {
            _team = team ?? throw new ArgumentNullException(nameof(team));
            _project = project ?? throw new ArgumentNullException(nameof(project));
            _accessToken = access_token ?? throw new ArgumentNullException(nameof(access_token));
            _httpClient = new HttpClient();
        }

        public string BuildNewErrorsQuery(DateTime timestampDateTime)
        {
            var timestamp = timestampDateTime.ToUnixTimestamp();

            // we only care about first_occurrence_timestamp, we search by timestamp also because it's indexed while the other date isn't
            return $@"SELECT item.title, count(*), item.counter, language
FROM item_occurrence
WHERE item.first_occurrence_timestamp > {timestamp}
AND timestamp > {timestamp}
AND item.environment = 'production'
AND item.status = 1
GROUP BY item.title, item.counter, language";
        }

        public async Task<List<RollbarItemDto>> RunRQLJob(string query)
        {
            var job = await PostRQLJob(query, _accessToken);

            await Task.Delay(1000);

            var isJobFinished = false;
            while (!isJobFinished)
            {
                var result = await GetRQLJob(job.Result.Id, _accessToken);
                isJobFinished = result.Result.IsSuccess;
                await Task.Delay(1000); 
            }

            var rqlJobResult = await GetRQLJobResult(job.Result.Id, _accessToken);

            var errors = new List<RollbarItemDto>();
            var columns = rqlJobResult.Result.InnerResult.Columns;
            foreach (var rows in rqlJobResult.Result.InnerResult.Rows)
            {
                var dataDict = columns.Zip(rows, (k, v) => new { k, v })
                    .ToDictionary(x => x.k, x => x.v);

                var errorDto = new RollbarItemDto()
                {
                    Title = dataDict["item.title"],
                    Occurrences = Convert.ToInt32(dataDict["count(*)"]),
                    Id = Convert.ToInt32(dataDict["item.counter"]),
                    Language = dataDict["language"],
                };

                errorDto.Url = new Uri($"https://rollbar.com/{_team}/{_project}/items/{errorDto.Id}/");
                errors.Add(errorDto);
            }

            return errors;
        }

        private async Task<ApiJobsGetResponse> GetRQLJob(int jobId, string token)
        {
            var url = $"https://api.rollbar.com/api/1/rql/job/{jobId}?access_token={token}";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ApiJobsGetResponse>(responseBody);
        }

        private async Task<ApiRqlResponse> GetRQLJobResult(int jobId, string token)
        {
            var url = $"https://api.rollbar.com/api/1/rql/job/{jobId}/result?access_token={token}";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ApiRqlResponse>(responseBody);
        }

        private async Task<ApiJobPostResponse> PostRQLJob(string query, string token)
        {
            var json = JsonConvert.SerializeObject(new ApiJobsPostRequest()
            {
                AccessToken = token,
                QueryString = query
            });

            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://api.rollbar.com/api/1/rql/jobs", data);
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ApiJobPostResponse>(responseBody);
        }
    }
}