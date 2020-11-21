namespace HmvScraping.Domains
{
    public class StoreStock
    {
        public Store Store { get; internal set; }

        public StockLeft StockLeft { get; internal set; }

        public string Action { get; internal set; }
    }
}
