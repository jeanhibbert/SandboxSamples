using BenchmarkDotNet.Attributes;

namespace SanboxSamples.Optimisation;

[MemoryDiagnoser]
[Orderer(BenchmarkDotNet.Order.SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
public class ListManipulatorService
{
    private static Random random = new Random();

    private static int n = 30;
    private static int[] array = Enumerable.Range(0, n).Select(_ => random.Next(0, 20)).ToArray();
    private static List<int> list = array.ToList();

    // never manipulate lists or array with span

    public void CanManipulateList()
    {
        //IterateArraySeq(array);
        //IterateListSeq(list);

        //IterateArraySeqWithLinq(array);
        //IterateListSeqWithLinq(list);

        //Span<int> span1 = array;
        //Span<int> span2 = CollectionsMarshal.AsSpan(list);
    }

    [Benchmark]
    public void IterateSpan()
    {
        Span<int> span1 = array;
        foreach (int i in span1) { }
    }
    
    [Benchmark]
    public void IterateArraySeq()
    {
        foreach (int i in array) { }
    }

    [Benchmark]
    public void IterateListSeq()
    {
        foreach (int i in list) { }
    }

    [Benchmark]
    public void IterateArraySeqWithLinq()
    {
        var output = array.Select(x => x);
    }

    [Benchmark]
    public void IterateListSeqWithLinq()
    {
        var output = list.Select(x => x);
    }
}
