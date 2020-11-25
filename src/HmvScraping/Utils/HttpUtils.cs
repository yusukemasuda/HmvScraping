namespace HmvScraping.Utils
{
    using AngleSharp;
    using AngleSharp.Dom;
    using System;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;

    internal class HttpUtils : IHttpUtils
    {
        private readonly HttpClient httpClient;

        private readonly CookieContainer CookieStore;

        internal HttpUtils(IHttpClientFactory factory)
        {
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }
            httpClient = factory.CreateClient(nameof(HttpUtils));
            CookieStore = new CookieContainer();
        }

        public string DefaultUserAgent
        {
            set
            {
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", value);
            }
        }

        #region GetContentStringAsync
        public async Task<string> GetContentStringAsync(HttpRequestMessage request)
        {
            var response = await httpClient.SendAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            var task = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return task;
        }

        public string GetContentString(HttpRequestMessage request)
        {
            var task = GetContentStringAsync(request);
            return task.Result;
        }

        public async Task<string> GetContentStringAsync(HttpMethod method, Uri uri, HttpContent content)
        {
            using (var request = new HttpRequestMessage(method, uri))
            {
                if (content != null)
                {
                    request.Content = content;
                }
                return await GetContentStringAsync(request).ConfigureAwait(false);
            }
        }
        public string GetContentString(HttpMethod method, Uri uri, HttpContent content)
        {
            var task = GetContentStringAsync(method, uri, content);
            return task.Result;
        }

        public async Task<string> GetContentStringAsync(HttpMethod method, Uri uri)
        {
            return await this.GetContentStringAsync(method, uri, null).ConfigureAwait(false);
        }

        public string GetContentString(HttpMethod method, Uri uri)
        {
            var task = GetContentStringAsync(method, uri);
            return task.Result;
        }

        public async Task<string> GetContentStringAsync(Uri uri)
        {
            return await GetContentStringAsync(HttpMethod.Get, uri).ConfigureAwait(false);
        }

        public string GetContentString(Uri uri)
        {
            var task = GetContentStringAsync(uri);
            return task.Result;
        }

#endregion

        #region GetContentStreamAsync

        private async Task<Stream> GetContentStreamAsync(HttpRequestMessage request)
        {
            // TODO: response の Dispose 
            var response = await httpClient.SendAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            var task = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
            return task;
        }

        private Stream GetContentStream(HttpRequestMessage request)
        {
            var task = GetContentStreamAsync(request);
            return task.Result;
        }
        #endregion

        #region GetDocument

        public async Task<IDocument> GetDocumentAsync(HttpRequestMessage request)
        {
            // TODO: response の Dispose 
            using (var stream = GetContentStream(request))
            {
                var context = BrowsingContext.New(Configuration.Default);
                return await context.OpenAsync(r => r.Content(stream)).ConfigureAwait(false);
            }
        }

        public IDocument GetDocument(HttpRequestMessage request)
        {
            var task = GetDocumentAsync(request);
            return task.Result;
        }

        public async Task<IDocument> GetDocumentAsync(HttpMethod method, Uri uri)
        {
            using (var request = new HttpRequestMessage(method, uri))
            {
                return await GetDocumentAsync(request).ConfigureAwait(false);
            }
        }

        public IDocument GetDocument(HttpMethod method, Uri uri)
        {
            var task = GetDocumentAsync(method, uri);
            return task.Result;
        }

        public async Task<IDocument> GetDocumentAsync(Uri uri)
        {
            return await GetDocumentAsync(HttpMethod.Get, uri).ConfigureAwait(false);
        }

        public IDocument GetDocument(Uri uri)
        {
            var task = GetDocumentAsync(uri);
            return task.Result;
        }

        #endregion
    }
}
