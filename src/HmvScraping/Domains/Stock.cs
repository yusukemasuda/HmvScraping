namespace HmvScraping.Domains
{
    using System.Collections.Generic;

    public class Stock
    {
        public Product Product { get; internal set; }

        public ICollection<StoreStock> StoreStocks { get; internal set; }
    }
}
