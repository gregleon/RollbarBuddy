using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RollbarBuddy.Models;
using RollbarBuddy.Services;
using RollbarBuddy.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RollbarBuddy
{
    public class RollbarFunctions
    {
        [FunctionName("NotifyNewRollbarErrors")]
        // every day at 8 AM
        public async Task Run([TimerTrigger("0 0 8 * * *"
#if DEBUG
            ,RunOnStartup = true
#endif
            )] TimerInfo myTimer, ILogger log)
        {
#if !DEBUG
           // Only notify on work days
            if (!DateTime.Now.IsWorkDay())
            {
                return;
            }
#endif

            var rollbarProject = Environment.GetEnvironmentVariable("ROLLBAR_PROJECT");
            var rollbarTeam = Environment.GetEnvironmentVariable("ROLLBAR_TEAM");
            var accessToken = Environment.GetEnvironmentVariable("ROLLBAR_ACCESS_TOKEN");

            var service = new RollbarService(rollbarTeam, rollbarProject, accessToken);

            // shows logs from last working day 8 AM
            var minimumFirstOccurrenceDate = DateTime.Now.PreviousBusinessDay().Date.AddHours(8);
            var errors = await service.RunRQLJob(service.BuildNewErrorsQuery(minimumFirstOccurrenceDate));

            // TODO: Create message card for every 10 errors
            var messageCard = new MessageCard()
            {
                ThemeColor = "ff0000",
                Title = "New Rollbar errors since " + minimumFirstOccurrenceDate.ToString("u"),
                Sections = errors.OrderByDescending(c => c.Occurrences).Select(c => new Section()
                {
                    ActivityTitle = $"# {c.Title}",
                    Facts = new List<Fact>()
                    {
                        new Fact()
                        {
                            Name = "Occurrences",
                            Value = c.Occurrences.ToString(),
                        },
                        new Fact()
                        {
                            Name = "Language",
                            Value = c.Language
                        }
                    },
                    PotentialAction = new List<PotentialAction>()
                    {
                        new PotentialAction()
                        {
                            Type = "OpenUri",
                            Name = "View in Rollbar",
                            Targets = new List<Target>()
                            {
                                new Target()
                                {
                                    Uri = c.Url
                                }
                            }
                        }
                    }
                }).ToList()
            };

            var json = JsonConvert.SerializeObject(messageCard);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var url = Environment.GetEnvironmentVariable("WEBHOOK_URL");

            using var client = new HttpClient();
            await client.PostAsync(url, data);
        }
    }
}