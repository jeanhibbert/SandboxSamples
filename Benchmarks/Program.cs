using BenchmarkDotNet.Running;
using Benchmarks;

BenchmarkRunner.Run<ProcessorBenchmarks>();

BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run();