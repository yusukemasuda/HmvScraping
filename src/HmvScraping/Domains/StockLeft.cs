namespace HmvScraping.Domains
{
    public sealed class StockLeft
    {
        public static readonly StockLeft NonStock = new StockLeft("×", "在庫なし");
        public static readonly StockLeft LowStock = new StockLeft("△", "残り僅か");
        public static readonly StockLeft InStock = new StockLeft("○", "在庫あり");
        public static readonly StockLeft Unknown = new StockLeft("？", "不明");

        public string Mark { get; }

        public string Text { get; }

        private StockLeft(string mark, string text)
        {
            this.Mark = mark;
            this.Text = text;
        }

        public override string ToString()
        {
            return $"{{ {Mark}, {Text} }}";
        }
    }
}
