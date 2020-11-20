namespace HmvScraping.Services
{
    using System.Collections.Generic;
    using HmvScraping.Domains;

    public interface IStockService
    {
        IEnumerable<StoreStock> GetStoreStocks(StockKeepingUnit sku);
    }
}
