namespace HmvScraping.Domains
{
    using HmvScraping.Domains;

    public class SearchResult
    {
        public StockKeepingUnit Sku { get; internal set; }

        public Artist Artist { get; internal set; }

        public ProductUri ProductUri { get; internal set; }
    }
}
