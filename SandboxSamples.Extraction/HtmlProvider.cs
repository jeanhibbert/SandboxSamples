using HtmlAgilityPack;


namespace SandboxSamples.Extraction;

public class HtmlProvider
{
    public async Task<string> GetDataAsync(string url)
    {

        string tableHtml = await GetDataTableHtml(url);

        return tableHtml;
    }

    public async Task<string> GetDataTableHtml(string url)
    {
        using (HttpClient client = new HttpClient())
        {
            try
            {
                // Make the GET request
                HttpResponseMessage response = await client.GetAsync(url);

                // Check if the request was successful
                response.EnsureSuccessStatusCode();

                // Read and parse HTML content
                string htmlContent = await response.Content.ReadAsStringAsync();

                // Use HtmlAgilityPack to extract the data table
                HtmlDocument htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(htmlContent);

                // Modify the XPath according to the structure of the target webpage
                HtmlNode tableNode = htmlDocument.DocumentNode.SelectSingleNode("//table[@class='your-table-class']");

                // Check if the tableNode is found
                if (tableNode != null)
                {
                    return tableNode.OuterHtml;
                }
                else
                {
                    Console.WriteLine("Table not found on the webpage.");
                    return null;
                }
            }
            catch (HttpRequestException ex)
            {
                // Handle exceptions
                Console.WriteLine($"An error occurred: {ex.Message}");
                return null;
            }
        }
    }
}


