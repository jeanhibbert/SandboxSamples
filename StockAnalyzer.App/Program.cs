using StockAnalyzer.Processor;
using System.Diagnostics;

var processor = new ProcessorFaster();
var watch = new Stopwatch();

while (true)
{
    Console.WriteLine("1 - Run processor");
    Console.WriteLine("0 - Exit");

    Console.Write("What do you want to do: ");
    var action = Console.ReadLine()?.Trim();

    if (action is not "1" or null)
    {
        Console.WriteLine("Exiting");
        break;
    }

    watch.Restart();

    RunProcessor();

    Console.WriteLine($"Elapsed time: {watch.ElapsedMilliseconds}ms{Environment.NewLine}");
}

void Deadlock()
{
    Task t2 = null!;

    // DO NOT DO THIS.....
    var t1 = Task.Run(RunProcessor).ContinueWith(t =>
    {
        // NEVER use Wait!
        t2.Wait();
    });

    t2 = Task.Run(() =>
    {
        t1.Wait();
    });
}

void RunProcessor()
{
    processor = new ProcessorFaster();

    var result = string.Empty;

    Console.WriteLine("Starting..");

    processor.Initialize();

    foreach (var stock in processor.Stocks)
    {
        var min = processor.Min(stock.Key);
        var max = processor.Max(stock.Key);
        var average = processor.Average(stock.Key);

        result += $"{stock.Key},{min},{max},{average}{Environment.NewLine}";
    }
    File.WriteAllText("Result.txt", $"{result}");
}