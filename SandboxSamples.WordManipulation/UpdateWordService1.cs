using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace SandboxSamples.WordManipulation;

public class UpdateWordService
{
    static void Main(string[] args)
    {
        string templatePath = @"C:\path\to\template.dotx";
        string docPath = @"C:\path\to\document.docx";

        using (WordprocessingDocument doc = WordprocessingDocument.Open(docPath, true))
        {
            doc.ChangeDocumentType(DocumentFormat.OpenXml.WordprocessingDocumentType.Document);

            MainDocumentPart mainPart = doc.MainDocumentPart;

            if (mainPart.DocumentSettingsPart == null)
            {
                mainPart.AddNewPart<DocumentSettingsPart>();
            }

            mainPart.DocumentSettingsPart.Settings = new Settings();
            mainPart.DocumentSettingsPart.Settings.AppendChild(new AttachedTemplate() 
            { 
                Id = "template", 
                //Template = templatePath 
            });

            StyleDefinitionsPart stylePart = mainPart.StyleDefinitionsPart;

            if (stylePart == null)
            {
                stylePart = mainPart.AddNewPart<StyleDefinitionsPart>();
                stylePart.Styles = new Styles();
                stylePart.Styles.Save();
            }

            stylePart.Styles.AppendChild(new Style()
            {
                Type = StyleValues.Paragraph,
                StyleId = "MyStyle",
                //Name = new StyleName() { Val = "My Style" },
                BasedOn = new BasedOn() { Val = "Normal" },
                NextParagraphStyle = new NextParagraphStyle() { Val = "Normal" },
                //ParagraphProperties = new ParagraphProperties(new Justification() { Val = JustificationValues.Center })
            });

            stylePart.Styles.Save();
        }

        Console.WriteLine("Template attached and styles applied successfully!");
    }
}