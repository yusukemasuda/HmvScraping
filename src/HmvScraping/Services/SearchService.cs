namespace HmvScraping.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Web;
    using AngleSharp.Html.Dom;
    using HmvScraping.Domains;
    using HmvScraping.Utils;

    internal class SearchService : ISearchService
    {
        private IHttpUtils HttpUtils { get; }

        internal SearchService(IHttpUtils httpUtils)
        {
            this.HttpUtils = httpUtils;
        }

        public IEnumerable<SearchResult> Search(params string[] keywords)
        {
            var words = keywords.Select(s => s.Trim()).Aggregate((current, next) => current + ' ' + next);
            var url = string.Format("https://www.hmv.co.jp/search/keyword_{0}/target_ALL/type_sr/", HttpUtility.UrlEncode(words));

            var doc = HttpUtils.GetDocument(new Uri(url));
            var resultList = doc.GetElementsByClassName("resultList").FirstOrDefault();
            if (resultList == null)
            {
                yield break;
            }
            foreach (var resultItem in resultList?.Children)
            {
                var productUri = new ProductUri(GetProductPageLink((IHtmlListItemElement)resultItem));
                var result = new SearchResult
                {
                    ProductUri = productUri
                };
                yield return result;
            }
        }

        private string GetProductPageLink(IHtmlListItemElement element)
        {
            var a = GetTitleAnchorElement(element);
            return a.Href;
        }

        private IHtmlAnchorElement GetTitleAnchorElement(IHtmlListItemElement element)
        {
            var h3 = element?.QuerySelector("h3.title");
            var a = (IHtmlAnchorElement)h3?.GetElementsByTagName("a").FirstOrDefault();
            return a;
        }
    }
}
