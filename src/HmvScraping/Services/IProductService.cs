namespace HmvScraping.Services
{
    using HmvScraping.Domains;

    public interface IProductService
    {
        Product GetProduct(ProductUri uri);
    }
}
