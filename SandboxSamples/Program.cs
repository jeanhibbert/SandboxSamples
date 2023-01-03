using AutoFixture;
using BenchmarkDotNet.Running;
using ObjectCopyExample;
using ObjectCopyExample.Model;
using System.Diagnostics;
using System.Globalization;

BenchmarkRunner.Run<ObjectCopyService>();

//debug & compare object copy methods
//new ObjectCopyService().CompareObjectCopyMethods();


