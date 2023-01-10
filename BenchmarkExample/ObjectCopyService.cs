using AutoFixture;
using AutoMapper;
using BenchmarkDotNet.Attributes;
using ObjectCopyExample.Model;
using ObjectCopyExample.ObjectCopy;
using Mapster;
using System.Diagnostics;

namespace ObjectCopyExample;

[MemoryDiagnoser]
[Orderer(BenchmarkDotNet.Order.SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
public class ObjectCopyService
{
    private readonly static Fixture Fixture = new();
    private readonly static HappyPerson PersonToCopy = Fixture.Create<HappyPerson>();

    // automapper
    private readonly static MapperConfiguration Config = new(cfg => {
        cfg.CreateMap<HappyPerson, Person>();
    });
    private static readonly IMapper mapper = Config.CreateMapper();

    //mapster - no config!!
    //ExpressMapper - to implement

    public Person ReflectionCopyPersonAndAddress(Person person)
    {
        return new Person().InjectFrom(person);
    }

    [Benchmark(Baseline = true)]
    public Person Reflection_ObjectCopy() => new Person().InjectFrom(PersonToCopy);

    [Benchmark]
    public Person AutoMapper_ObjectCopy() => mapper.Map<HappyPerson, Person>(PersonToCopy);

    [Benchmark]
    public Person Mapster_ObjectCopy() => PersonToCopy.Adapt<Person>();

    public static void CompareObjectCopyMethods()
    {
        var objectCopyService = new ObjectCopyService();

        var reflectPerson = objectCopyService.Reflection_ObjectCopy();
        var autoMapperPerson = objectCopyService.AutoMapper_ObjectCopy();
        var mapsterPerson = objectCopyService.Mapster_ObjectCopy();

        // Since HappyPerson and Person are both classes Equality operator "==" uses object.ReferenceEquals to determine equality
        var arrayIndex = 0;

        // Reflection
        if (PersonToCopy.Addresses == reflectPerson.Addresses)
            Console.WriteLine("REFLECTION - Same reference on heap");
       
        if (PersonToCopy.Addresses[arrayIndex] == reflectPerson.Addresses[arrayIndex])
            Console.WriteLine("REFLECTION - Same reference on heap");

        // AutoMapper
        if (PersonToCopy.Addresses == autoMapperPerson.Addresses)
            Console.WriteLine("AUTOMAPPER - Same reference on heap");

        if (PersonToCopy.Addresses[arrayIndex] == autoMapperPerson.Addresses[arrayIndex])
            Console.WriteLine("AUTOMAPPER - Same reference on heap");

        // Mapster
        if (PersonToCopy.Addresses == mapsterPerson.Addresses)
            Console.WriteLine("MAPSTER - Same reference on heap");

        if (PersonToCopy.Addresses[arrayIndex] == mapsterPerson.Addresses[arrayIndex])
            Console.WriteLine("MAPSTER - Same reference on heap");
        else
            Console.WriteLine("MAPSTER - NOT THE SAME reference on heap - OBJECT WAS COPIED");

        // Change an address in the source address list to see what happens to the target address list
        PersonToCopy.Addresses[arrayIndex].Line1 = "11";

        Console.WriteLine("reflection - address line 1 is affected {0}", reflectPerson.Addresses[arrayIndex].Line1); // affected!!
        Console.WriteLine("automapper - address line 1 is affected {0}", autoMapperPerson.Addresses[arrayIndex].Line1); // affected!!
        Console.WriteLine("mapster - address line 1 is not affected {0}", mapsterPerson.Addresses[arrayIndex].Line1); // not affected

        Console.ReadKey();
    }
}


