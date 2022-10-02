using AutoFixture;
using BenchmarkDotNet.Running;
using BenchmarkExample;
using BenchmarkExample.Model;
using System.Diagnostics;
using System.Globalization;

//BenchmarkRunner.Run<ObjectCopyService>();

//debug & compare object copy methods
new ObjectCopyService().CompareObjectCopyMethods();


