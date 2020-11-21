namespace HmvScraping.Services
{
    using HmvScraping.Domains;

    public interface IStockService
    {
        Stock GetStock(Product product);
    }
}
