namespace HmvScraping.Domains
{
    public class StoreStock
    {
        public StockKeepingUnit Sku { get; internal set; }

        public Store Store { get; internal set; }

        public StockLeft? StockLeft { get; internal set; }
    }
}
