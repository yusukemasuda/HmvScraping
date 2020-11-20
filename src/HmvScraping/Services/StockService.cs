namespace HmvScraping.Services
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Text.RegularExpressions;
    using System.Web;
    using AngleSharp.Html.Dom;
    using HmvScraping.Domains;
    using HmvScraping.Utils;

    internal class StockService : IStockService
    {
        private IHttpUtils HttpUtils { get; }

        internal StockService(IHttpUtils httpUtils)
        {
            this.HttpUtils = httpUtils;
        }

        public IEnumerable<StoreStock> GetStoreStocks(StockKeepingUnit sku)
        {
            var url = string.Format("https://www.hmv.co.jp/productitemstock/stock/{0}", HttpUtility.UrlEncode(sku.Id));
            var doc = HttpUtils.GetDocument(new Uri(url));

            var table = doc.GetElementById("chiku-99");
            foreach (var row in table.QuerySelectorAll("tr"))
            {
                var cells = row.GetElementsByTagName("td");
                var storeLink = cells[1].QuerySelector("a");
                if (storeLink == null)
                {
                    continue;
                }
                var stockIcon = cells[0].QuerySelector("img");
                yield return new StoreStock()
                {
                    Sku = sku,
                    Store = GetStore((IHtmlAnchorElement)storeLink),
                    StockLeft = GetStockLeft((IHtmlImageElement)stockIcon)
                };
            }
        }

        private Store GetStore(IHtmlAnchorElement element)
        {
            var regex = new Regex(@"^.*/(?<storeId>[A-Z0-9]+)/$");
            var match = regex.Match(element.Href);
            if (!match.Success)
            {
                throw new ArgumentException("Not a store link", nameof(element));
            }
            var store = new Store
            {
                Id = match.Groups["storeId"].Value,
                Name = WebUtility.HtmlDecode(element.InnerHtml.Trim())
            };
            return store;
        }

        private readonly IDictionary<string, StockLeft> StockLeftMap = new Dictionary<string, StockLeft>
        {
            { "×", StockLeft.NonStock },
            { "△", StockLeft.LowStock },
            { "○", StockLeft.InStock }
        };

        private StockLeft? GetStockLeft(IHtmlImageElement element)
        {
            return StockLeftMap[element.AlternativeText];
        }
    }
}
