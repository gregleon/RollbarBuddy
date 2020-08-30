using System;

namespace RollbarBuddy.Models.Rollbar
{
    public class RollbarItemDto
    {
        public string Title { get; set; }

        public int Id { get; set; }

        public int Occurrences { get; set; }

        public string Language { get; set; }

        public Uri Url { get; set; }
    }
}