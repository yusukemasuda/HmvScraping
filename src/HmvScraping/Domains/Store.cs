namespace HmvScraping.Domains
{
    public class Store
    {
        public string Id { get; internal set; }

        public string Name { get; internal set; }

        public override string ToString()
        {
            return $"{{ {Id}, {Name} }}";
        }
    }
}
