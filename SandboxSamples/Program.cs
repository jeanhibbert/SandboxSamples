using BenchmarkDotNet.Running;
using ObjectCopyExample;
using SanboxSamples.Optimisation;

// Uncomment if you would like to benchmark the mapping strategies
//BenchmarkRunner.Run<ObjectCopyService>();

//debug & compare object copy methods
//ObjectCopyService.CompareObjectCopyMethods();

BenchmarkRunner.Run<ListManipulatorService>();


