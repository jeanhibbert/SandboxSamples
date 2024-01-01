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
}
