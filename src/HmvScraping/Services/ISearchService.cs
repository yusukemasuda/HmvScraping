namespace HmvScraping.Services
{
    using System;
    using System.Collections.Generic;
    using HmvScraping.Domains;

    public interface ISearchService
    {
        IEnumerable<SearchResult> Search(params string[] keywords);
    }
}
