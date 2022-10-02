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

        var reflectAutoRef = object.ReferenceEquals(reflectPerson.Addresses, autoMapperPerson.Addresses);
        var addressReflectAutoRef = object.ReferenceEquals(reflectPerson.Addresses[arrayIndex], autoMapperPerson.Addresses[arrayIndex]);

        var reflectMapsterREf = object.ReferenceEquals(reflectPerson.Addresses, mapsterPerson.Addresses);
        var addressreflectMapsterREf = object.ReferenceEquals(reflectPerson.Addresses[arrayIndex], mapsterPerson.Addresses[arrayIndex]);

        reflectPerson.Addresses[arrayIndex].Line1 = "11";

        Debug.Assert(reflectPerson.Addresses[arrayIndex].Line1 == autoMapperPerson.Addresses[arrayIndex].Line1);

        Console.WriteLine(autoMapperPerson.Addresses[arrayIndex].Line1);

        Debug.Assert(reflectPerson.Addresses[arrayIndex].Line1 == mapsterPerson.Addresses[arrayIndex].Line1);

        Console.WriteLine(mapsterPerson.Addresses[arrayIndex].Line1);
    }
}
