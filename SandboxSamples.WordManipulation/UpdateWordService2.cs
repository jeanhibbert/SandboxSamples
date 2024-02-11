using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace SandboxSamples.WordManipulation;
public class UpdateWordService2
{
    public void ApplyStyles(string templatePath, string existingDocumentPath)
    {
        using (var templateDoc = WordprocessingDocument.Open(templatePath, false))
        using (var existingDoc = WordprocessingDocument.Open(existingDocumentPath, true))
        {
            var mainPart = existingDoc.MainDocumentPart;
            var body = mainPart.Document.Body;
            var styles = templateDoc.MainDocumentPart.StyleDefinitionsPart.Styles;

            // Clone the styles from the template to the existing document
            var styleDefinitionsPart = mainPart.StyleDefinitionsPart ?? mainPart.AddNewPart<StyleDefinitionsPart>();
            styleDefinitionsPart.Styles = (Styles)styles.CloneNode(true);

            body.AppendChild(new Paragraph(new Run(new Text("Hello, World!"))));

            mainPart.Document.Save();
        }
    }

    public void ApplyStylesFromTemplate(string templatePath, string outputPath)
    {
        using (WordprocessingDocument templateDoc = WordprocessingDocument.Open(templatePath, false))
        using (WordprocessingDocument outputDoc = WordprocessingDocument.Create(outputPath, WordprocessingDocumentType.Document))
        {
            // Clone the styles part from the template to the output document
            outputDoc.AddMainDocumentPart().AddPart(templateDoc.MainDocumentPart.StyleDefinitionsPart);

            // Clone the body of the template to the output document
            outputDoc.MainDocumentPart.Document = (Document)templateDoc.MainDocumentPart.Document.CloneNode(true);

            // Save changes
            outputDoc.Save();
        }
    }
}
