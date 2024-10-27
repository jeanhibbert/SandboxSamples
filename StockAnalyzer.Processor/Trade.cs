namespace StockAnalyzer.Processor;

public record Trade(DateTime TradeDate, decimal Volume, decimal Change, decimal ChangePercent);