namespace HmvScraping.Services
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
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

        public Stock GetStock(Product product)
        {
            var url = string.Format("https://www.hmv.co.jp/productitemstock/stock/{0}", HttpUtility.UrlEncode(product.Sku.Id));
            var doc = HttpUtils.GetDocument(new Uri(url));

            var table = doc.GetElementById("chiku-99");

            var storeStocks = new Collection<StoreStock>();
            foreach (var row in table.QuerySelectorAll("tr"))
            {
                var cells = row.GetElementsByTagName("td");
                var storeLink = cells[1].QuerySelector("a");
                if (storeLink == null)
                {
                    continue;
                }
                var stockIcon = cells[0].QuerySelector("img");
                var actionButton = cells[2].QuerySelector("button");
                storeStocks.Add(new StoreStock()
                {
                    Store = GetStore((IHtmlAnchorElement)storeLink),
                    StockLeft = GetStockLeft((IHtmlImageElement)stockIcon),
                    Action = GetAction((IHtmlButtonElement)actionButton)
                });
            }
            return new Stock
            {
                Product = product,
                StoreStocks = storeStocks
            };
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
            { StockLeft.NonStock.Mark, StockLeft.NonStock },
            { StockLeft.LowStock.Mark, StockLeft.LowStock },
            { StockLeft.InStock.Mark, StockLeft.InStock }
        };

        private StockLeft GetStockLeft(IHtmlImageElement element)
        {
            var mark = element.AlternativeText;
            if (!StockLeftMap.ContainsKey(mark))
            {
                return StockLeft.Unknown;
            }
            return StockLeftMap[mark];
        }

        private string GetAction(IHtmlButtonElement element)
        {
            if (element == null)
            {
                return null;
            }
            return element.InnerHtml.Trim();
        }
    }
}
