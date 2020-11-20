namespace HmvScraping.Console
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using HmvScraping;
    using HmvScraping.Domains;
    using HmvScraping.Services;

    public class Application : IApplication
    {
        private ISearchService SearchService { get; }

        private IProductService ProductService { get; }

        private IStockService StockService { get; }

        private IUserNotification UserNotification { get; }

        public Application(ISearchService search, IProductService product, IStockService stock, IUserNotification notification)
        {
            this.SearchService = search;
            this.ProductService = product;
            this.StockService = stock;
            this.UserNotification = notification;
        }

        public void Run(Options options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (options.Keyword == null && options.File == null)
            {
                throw new ArgumentNullException("Keyword or File");
            }
            if (options.File != null && !File.Exists(options.File))
            {
                throw new ArgumentException($"File does not exist: { options.File }");
            }
            var enumerable = (new string[] { options.Keyword }).AsEnumerable();
            if (options != null && File.Exists(options.File))
            {
                enumerable = enumerable.Concat(File.ReadAllLines(options.File));
            }
            var keywords = enumerable.Where(s => !string.IsNullOrEmpty(s)).ToArray();

            this.Run(options.Directory, keywords);
        }

        public void Run(string outputDirectory, params string[] keywords)
        {
            this.Run(new DirectoryInfo(outputDirectory), keywords);
        }

        public void Run(DirectoryInfo outputDirectory, params string[] keywords)
        {
            if (outputDirectory == null)
            {
                throw new ArgumentNullException("Output directory should be set.", nameof(outputDirectory));
            }
            if (keywords == null || keywords.Length == 0)
            {
                throw new ArgumentNullException("Keywords should be passed at least one", nameof(keywords));
            }

            foreach (var keyword in keywords)
            {
                UserNotification.Put($"Keyword: { keyword }");
                var results = SearchService.Search(keyword);
                var first = results.FirstOrDefault();
                if (first == null)
                {
                    PutNoneFile(keyword, outputDirectory);
                    continue;
                }
                UserNotification.Put($"Item found for Keyword: { keyword }");
                var product = ProductService.GetProduct(first.ProductUri);
                if (product == null || !(product.IsInStock || product.IsInPreOrder))
                {
                    PutNoneFile(keyword, outputDirectory);
                    continue;
                }
                var stocks = StockService.GetStoreStocks(first.Sku);
                PutResultFile(keyword, stocks, outputDirectory);
            }
        }

        private void PutResultFile(string keyword, IEnumerable<StoreStock> storeStocks, DirectoryInfo directory)
        {
            if (!directory.Exists)
            {
                directory.Create();
            }

            var name = DateTime.Now.ToString("yyyyMMdd");
            var output = new FileInfo(Path.Combine(directory.FullName, $"{ keyword }_{ name }.csv"));
            if (output.Exists)
            {
                output.Delete();
            }
            using (var writer = new StreamWriter(output.FullName))
            {
                foreach (var storeStock in storeStocks)
                {
                    writer.WriteLine(storeStock.Store.Name);
                }
            }
            UserNotification.Put($"Output: { output.FullName }");
        }

        private void PutNoneFile(string keyword, DirectoryInfo directory)
        {
            if (!directory.Exists)
            {
                directory.Create();
            }
            var output = new FileInfo(Path.Combine(directory.FullName, $"{ keyword }_なし.csv"));
            output.Create();
            UserNotification.Put($"Output: { output.FullName }");
        }
    }
}
