using pdftron.PDF;
using pdftron.SDF;

namespace SandboxSamples.WordManipulation;
public class PdfService
{
    public void ConvertDocxToPdf(string inputDocxPath, string outputPdfPath)
    {
        using (PDFDoc pdfDoc = new PDFDoc())
        {
            pdftron.PDF.Convert.OfficeToPDF(pdfDoc, inputDocxPath, new ConversionOptions ());
            pdfDoc.Save(outputPdfPath, SDFDoc.SaveOptions.e_remove_unused);
        }
    }
}
