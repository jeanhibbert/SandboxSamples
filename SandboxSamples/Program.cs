using BenchmarkDotNet.Running;
using ObjectCopyExample;
using SanboxSamples.Optimisation;
using SandboxSamples.WordManipulation;
using System.Reflection;

// Uncomment if you would like to benchmark the mapping strategies
//BenchmarkRunner.Run<ObjectCopyService>();

//debug & compare object copy methods
//ObjectCopyService.CompareObjectCopyMethods();

//BenchmarkRunner.Run<ListManipulatorService>();

var updateWordService = new UpdateWordService2();

string docFileName = "DocumentToConvert.docx";
string resourceFileName = "Some Template.dotx";
string resourceFolderPath = "Resources"; // Replace with your actual folder name

string basePath = GetBasePath();
string templatePath = Path.Combine(basePath, resourceFolderPath, resourceFileName);
string documentPath = Path.Combine(basePath, resourceFolderPath, docFileName);

updateWordService.ApplyStyles(templatePath, documentPath);

var pdfService = new PdfService();

pdfService.ConvertDocxToPdf(documentPath, basePath + @"\output.pdf");

Console.ReadKey();

static string GetBasePath()
{
    var assembly = Assembly.GetExecutingAssembly();
    var assemblyLocation = assembly.Location;
    var assemblyDirectory = Path.GetDirectoryName(assemblyLocation);
    return assemblyDirectory;
}