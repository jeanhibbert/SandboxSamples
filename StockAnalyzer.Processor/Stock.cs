namespace StockAnalyzer.Processor;

public class Stock(string ticker)
{
    public IList<Trade> Trades { get; } = new List<Trade>();

    public decimal Min { get; set; } = decimal.MaxValue;
    public decimal Max { get; set; } = decimal.MinValue;
    public decimal Total { get; set; } = 0;
    public decimal Average => Total / Trades.Count;
}
