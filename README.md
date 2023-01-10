# SandboxSamples

This solution contains multiple projects used to benchmark open source frameworks

## Benchmarking the performance of the frameworks

1) ensure the in program.cs main method the following line is uncommented : BenchmarkRunner.Run<ObjectCopyService>();
2) navigate to the sandbox samples project folder, the perform a release build : dotnet build -c release
3) copy the location of the SandboxSamples.dll
4) run : dotnet ""C:\...\SandboxSamples\bin\release\net6.0\SandboxSamples.dll""

## Object Copy Example

Simple object copy Service that evaluates the performance of the following frameworks:

- Dot Net Reflection namespace
- AutoMapper
- Mapster

Additiional work was done to evaluate if the child reference properties were copied by reference or by value.

![ObjectCopy](https://user-images.githubusercontent.com/230106/210452703-0d142822-b6dd-4177-8d87-4872cb63ef3a.png)
