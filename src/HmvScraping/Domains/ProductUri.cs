namespace HmvScraping.Domains
{
    using System;
    using System.Text.RegularExpressions;
    using System.Web;

    public class ProductUri
    {
        private Regex Pattern { get; } = new Regex(
            @"^https?://([a-z0-9]*\.)?hmv\.co\.jp/artist_.+_(?<artist>[0-9]+)/item_.+_(?<sku>[0-9]+)$",
            RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture);

        private Uri Uri { get; }

        public string ArtistId { get; }

        public string Sku { get; }

        internal ProductUri(Artist artist, StockKeepingUnit sku)
        {
            this.ArtistId = artist.Id;
            this.Sku = sku.Id;
            this.Uri = new Uri($"https://www.hmv.co.jp/artist_{ HttpUtility.UrlEncode(artist.Name.Replace(' ', '-')) }_{ HttpUtility.UrlEncode(artist.Id) }/item_{ HttpUtility.UrlEncode(sku.Title.Replace(' ', '-')) }_{ HttpUtility.UrlEncode(sku.Id) }");
        }

        public ProductUri(Uri uri)
        {
            this.Uri = uri;
            var match = Pattern.Match(uri.AbsoluteUri);
            if (!match.Success)
            {
                throw new ArgumentException($"Invalid product url: {uri.AbsoluteUri}", nameof(uri));
            }
            ArtistId = match.Groups["artist"].Value;
            Sku = match.Groups["sku"].Value;
        }

        public ProductUri(string uriString) : this(new Uri(uriString))
        {
        }


        public Uri ToUri()
        {
            return Uri;
        }
    }
}
