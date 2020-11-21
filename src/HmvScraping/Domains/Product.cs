namespace HmvScraping.Domains
{
    using System;

    public class Product
    {
        public string Title { get; internal set; }

        public Artist Artist { get; internal set; }

        public StockKeepingUnit Sku { get; internal set; }

        public bool IsInStock { get; internal set; }

        public bool IsInPreOrder { get; internal set; }

        public Uri PageLink { get; internal set; }

        public override string ToString()
        {
            return $@"{{
  {Title}, 
  {Artist},
  {Sku}
}}";
        }
    }
}
