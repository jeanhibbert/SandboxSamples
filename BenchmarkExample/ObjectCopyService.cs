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
    public static readonly Fixture Fixture = new Fixture();
    public static HappyPerson PersonToCopy = new Fixture().Create<HappyPerson>();

    // automapper
    public static MapperConfiguration Config = new MapperConfiguration(cfg => {
        cfg.CreateMap<HappyPerson, Person>();
    });
    private static IMapper mapper = Config.CreateMapper();

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

    public void CompareObjectCopyMethods()
    {
        var arrayIndex = 0;

        var objectCopyService = new ObjectCopyService();

        var reflectPerson = objectCopyService.Reflection_ObjectCopy();
        var autoMapperPerson = objectCopyService.AutoMapper_ObjectCopy();
        var mapsterPerson = objectCopyService.Mapster_ObjectCopy();

        // Since HappyPerson and Person are both classes Equality operator "==" uses object.ReferenceEquals to determine equality

        // Reflection
        if (PersonToCopy.Addresses == reflectPerson.Addresses)
            Console.WriteLine("Same reference on heap");
        
        if (PersonToCopy.Addresses[arrayIndex] == reflectPerson.Addresses[arrayIndex])
            Console.WriteLine("Same reference on heap");

        // AutoMapper
        if (PersonToCopy.Addresses == autoMapperPerson.Addresses)
            Console.WriteLine("Same reference on heap");

        if (PersonToCopy.Addresses[arrayIndex] == autoMapperPerson.Addresses[arrayIndex])
            Console.WriteLine("Same reference on heap");

        // Mapster
        if (PersonToCopy.Addresses == mapsterPerson.Addresses)
            Console.WriteLine("Same reference on heap");

        if (PersonToCopy.Addresses[arrayIndex] == mapsterPerson.Addresses[arrayIndex])
            Console.WriteLine("Same reference on heap");
        else
            Console.WriteLine("NOT THE SAME reference on heap - OBJECT WAS COPIED");

        // Change an address in the source address list to see what happens to the target address list
        PersonToCopy.Addresses[arrayIndex].Line1 = "11";

        Console.WriteLine(reflectPerson.Addresses[arrayIndex].Line1); // affected!!
        Console.WriteLine(autoMapperPerson.Addresses[arrayIndex].Line1); // affected!!
        Console.WriteLine(mapsterPerson.Addresses[arrayIndex].Line1); // not affected
    }
}


