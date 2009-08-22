namespace WindsorSample
{
    public class StringParsingTitleScraper : ITitleScraper
    {
        public string Scrape(string fileContents)
        {
            string title = string.Empty;
            int openingTagIndex = fileContents.IndexOf("<title>");
            int closingTagIndex = fileContents.IndexOf("</title>");

            if(openingTagIndex != -1 && closingTagIndex != -1)
                title = fileContents.Substring(openingTagIndex, closingTagIndex - openingTagIndex).Substring(7);

            return title;
        }
    }
}