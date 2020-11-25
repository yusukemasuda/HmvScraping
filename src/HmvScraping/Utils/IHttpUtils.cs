namespace HmvScraping.Utils
{
    using System;
    using System.IO;
    using System.Net.Http;
    using System.Threading.Tasks;
    using AngleSharp.Dom;

    internal interface IHttpUtils
    {
        string DefaultUserAgent { set; }

        Task<string> GetContentStringAsync(HttpRequestMessage request);

        string GetContentString(HttpRequestMessage request);

        Task<string> GetContentStringAsync(HttpMethod method, Uri uri, HttpContent content);

        string GetContentString(HttpMethod method, Uri uri, HttpContent content);

        Task<string> GetContentStringAsync(HttpMethod method, Uri uri);

        string GetContentString(HttpMethod method, Uri uri);

        Task<string> GetContentStringAsync(Uri uri);

        string GetContentString(Uri uri);

        Task<IDocument> GetDocumentAsync(HttpRequestMessage request);

        IDocument GetDocument(HttpRequestMessage request);

        Task<IDocument> GetDocumentAsync(HttpMethod method, Uri uri);

        IDocument GetDocument(HttpMethod method, Uri uri);

        Task<IDocument> GetDocumentAsync(Uri uri);

        IDocument GetDocument(Uri uri);
    }
}
