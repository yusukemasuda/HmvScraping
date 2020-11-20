namespace HmvScraping.Domains
{
    public class Product
    {
        public string Title { get; internal set; }

        public Artist Artist { get; internal set; }
        public StockKeepingUnit Sku { get; internal set; }

        public bool IsInStock { get; internal set; }

        public bool IsInPreOrder { get; internal set; }
    }
}
