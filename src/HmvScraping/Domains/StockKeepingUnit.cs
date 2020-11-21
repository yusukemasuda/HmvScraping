namespace HmvScraping.Domains
{
    public class StockKeepingUnit
    {
        public string Id { get; }

        public string Title { get; }

        public StockKeepingUnit(string id, string title)
        {
            this.Id = id;
            this.Title = title;
        }

        public override string ToString()
        {
            return $"{{ {Id}, {Title} }}";
        }

    }
}
