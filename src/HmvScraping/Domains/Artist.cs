namespace HmvScraping.Domains
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class Artist
    {
        public string Id { get; }

        public string Name { get; }

        public Artist(string id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

        public override string ToString()
        {
            return $"{{ {Id}, {Name} }}";
        }
    }
}
