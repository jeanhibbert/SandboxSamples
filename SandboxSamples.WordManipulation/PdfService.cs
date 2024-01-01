using pdftron.PDF;
using pdftron.SDF;

namespace SandboxSamples.WordManipulation;
public class PdfService
{
    public void ConvertDocxToPdf(string inputDocxPath, string outputPdfPath)
    {
        using (PDFDoc pdfDoc = new PDFDoc())
        {
            // Convert DOCX to PDF using PDFTron Convert utility
            pdftron.PDF.Convert.OfficeToPDF(pdfDoc, inputDocxPath, new ConversionOptions ());

            // Save the PDF to the specified output path
            pdfDoc.Save(outputPdfPath, SDFDoc.SaveOptions.e_remove_unused);
        }
    }
}
