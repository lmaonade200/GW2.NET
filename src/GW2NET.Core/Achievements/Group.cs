namespace GW2NET.Achievements
{
    using System;
    using System.Collections.Generic;

    public class Group
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Order { get; set; }

        public IEnumerable<int> CategorieIds { get; set; }

        public IEnumerable<Category> Categories { get; set; }

    }
}