namespace HmvScraping.Services
{
    using System;
    using System.Net;
    using AngleSharp;
    using AngleSharp.Html.Dom;
    using Newtonsoft.Json.Linq;
    using HmvScraping.Domains;
    using HmvScraping.Utils;

    internal class ProductService : IProductService
    {
        private IHttpUtils HttpUtils { get; }

        internal ProductService(IHttpUtils httpUtils)
        {
            this.HttpUtils = httpUtils;
        }

        public Product GetProduct(ProductUri uri)
        {
            var doc = HttpUtils.GetDocument(uri.ToUri());

            var stockStatusLabel = GetStockStatusLabel(uri.Sku);
            var product = new Product
            {
                Title = GetTitle((IHtmlDocument)doc),
                Artist = new Artist(uri.ArtistId, GetArtist((IHtmlDocument)doc)),
                Sku = new StockKeepingUnit(uri.Sku, GetTitle((IHtmlDocument)doc)),
                IsInStock = stockStatusLabel.Contains("在庫あり"),
                IsInPreOrder = stockStatusLabel.Contains("発売予定")
            };
            return product;
        }

        private string GetTitle(IHtmlDocument doc)
        {
            var div = doc.QuerySelector("div.singleMainInfo");
            var h1 = div.QuerySelector("h1.title");
            var title = default(string);
            if (h1.InnerHtml != null)
            {
                title = WebUtility.HtmlDecode(h1.InnerHtml);
            }
            return title.Trim();
        }

        private string GetArtist(IHtmlDocument doc)
        {
            var div = doc.QuerySelector("div.singleMainInfo");
            var span = div.QuerySelector("h2 > a > span.brand");
            var artist = default(string);
            if (span.InnerHtml != null)
            {
                artist = WebUtility.HtmlDecode(span.InnerHtml);
            }
            return artist.Trim();
        }

        private string GetStockStatusLabel(string sku)
        {
            var content = HttpUtils.GetContentString(new Uri($"https://www.hmv.co.jp/async/reloadPrductParts2/?sku={sku}&num=30"));
            var json = JObject.Parse(content);
            var renderText = json["renderText"].ToString();

            var context = BrowsingContext.New(Configuration.Default);
            var dom = context.OpenAsync(z => z.Content(renderText)).Result;

            var div = dom.GetElementById("productStockArea");
            var p = div.QuerySelector("p.title") ?? div.QuerySelector("p.date");
            var label = default(string);
            if (p.InnerHtml != null)
            {
                label = WebUtility.HtmlDecode(p.InnerHtml);
            }
            return label.Trim();
        }
    }
}
