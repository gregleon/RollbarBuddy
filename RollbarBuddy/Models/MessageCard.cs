using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace RollbarBuddy.Models
{
    public class MessageCard
    {
        [JsonProperty("@type")] public const string Type = "MessageCard";

        [JsonProperty("title")] public string Title { get; set; }

        [JsonProperty("@context")] public readonly Uri Context = new Uri("https://schema.org/extensions");

        [JsonProperty("summary")] public string Summary => Title;

        [JsonProperty("themeColor")] public string ThemeColor { get; set; }

        [JsonProperty("sections")] public List<Section> Sections { get; set; }
    }

    public class Section
    {
        [JsonProperty("activityImage")] public Uri ActivityImage { get; set; }

        [JsonProperty("activityTitle")] public string ActivityTitle { get; set; }

        [JsonProperty("activitySubtitle")] public string ActivitySubtitle { get; set; }

        [JsonProperty("facts")] public List<Fact> Facts { get; set; }

        [JsonProperty("text")] public string Text { get; set; }

        [JsonProperty("potentialAction")] public List<PotentialAction> PotentialAction { get; set; }

        [JsonProperty("startGroup", NullValueHandling = NullValueHandling.Ignore)]
        public bool? StartGroup { get; set; } = true;
    }

    public class Fact
    {
        [JsonProperty("name")] public string Name { get; set; }

        [JsonProperty("value")] public string Value { get; set; }
    }

    public class Target
    {
        [JsonProperty("os")] public string OS { get; set; } = "default";

        [JsonProperty("uri")] public Uri Uri { get; set; }
    }

    public class PotentialAction
    {
        [JsonProperty("@type")] public string Type { get; set; }

        [JsonProperty("name")] public string Name { get; set; }

        [JsonProperty("targets")] public List<Target> Targets { get; set; }
    }
}