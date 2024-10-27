//using StockAnalyzer.Processor;
//using System.Diagnostics;

//var watch = new Stopwatch();
//watch.Start();

//var result = string.Empty;

//var processorSlow = new Processor();

//processorSlow.Initialize();


//foreach (var stock in processorSlow.Stocks)
//{
//    var min = processorSlow.Min(stock.Key);
//    var max = processorSlow.Max(stock.Key);
//    var average = processorSlow.Average(stock.Key);

//    result += $"{stock.Key},{min},{max},{average}{Environment.NewLine}";
//}
//File.WriteAllText("Result_Slow.txt", $"{result}");

//Console.WriteLine($"Completed in {watch.ElapsedMilliseconds}ms");

//result = string.Empty;

//watch.Restart();

//var processorFast = new ProcessorFaster();

//processorFast.Initialize();

//foreach (var stock in processorFast.Stocks)
//{
//    var min = stock.Value.Min;
//    var max = stock.Value.Max;
//    var average = stock.Value.Average;

//    result += $"{stock.Key},{min},{max},{average}{Environment.NewLine}";
//}

//File.WriteAllText("Result_Fast.txt", $"{result}");

//Console.WriteLine($"Completed in {watch.ElapsedMilliseconds}ms");

//Console.ReadLine();