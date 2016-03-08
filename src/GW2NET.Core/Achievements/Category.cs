namespace GW2NET.Achievements
{
    using System;
    using System.Collections.Generic;

    using GW2NET.Files;

    public class Category
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Order { get; set; }

        public Uri IconUrl { get; set; }

        public Asset Icon { get; set; }

        public IEnumerable<int> AchievementIds { get; set; }

        public IEnumerable<Achievement> Achievements { get; set; }
    }
}