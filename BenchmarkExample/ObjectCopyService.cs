using AutoFixture;
using AutoMapper;
using BenchmarkDotNet.Attributes;
using BenchmarkExample.Model;
using BenchmarkExample.ObjectCopy;
using Mapster;
using System.Diagnostics;

namespace BenchmarkExample;

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

        // Reflection
        if (object.ReferenceEquals(ObjectCopyService.PersonToCopy.Addresses, reflectPerson.Addresses))
        {
            Console.WriteLine("Same reference on heap");
        }

        if (object.ReferenceEquals(ObjectCopyService.PersonToCopy.Addresses[arrayIndex], reflectPerson.Addresses[arrayIndex]))
        {
            Console.WriteLine("Same reference on heap");
        }

        // AutoMapper
        if (object.ReferenceEquals(ObjectCopyService.PersonToCopy.Addresses, autoMapperPerson.Addresses))
        {
            Console.WriteLine("Same reference on heap");
        }

        if (object.ReferenceEquals(ObjectCopyService.PersonToCopy.Addresses[arrayIndex], autoMapperPerson.Addresses[arrayIndex]))
        {
            Console.WriteLine("Same reference on heap");
        }

        // Mapster
        if (object.ReferenceEquals(ObjectCopyService.PersonToCopy.Addresses, mapsterPerson.Addresses))
        {
            Console.WriteLine("Same reference on heap");
        }

        if (object.ReferenceEquals(ObjectCopyService.PersonToCopy.Addresses[arrayIndex], mapsterPerson.Addresses[arrayIndex]))
        {
            Console.WriteLine("Same reference on heap");
        }

        ObjectCopyService.PersonToCopy.Addresses[arrayIndex].Line1 = "11";

        Console.WriteLine(reflectPerson.Addresses[arrayIndex].Line1); // affected!!
        Console.WriteLine(autoMapperPerson.Addresses[arrayIndex].Line1); // affected!!
        Console.WriteLine(mapsterPerson.Addresses[arrayIndex].Line1); // not affected
    }
}
