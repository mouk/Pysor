using System;

namespace WindsorSample
{
    public class HtmlTitleRetriever
    {
        public IFileDownloader Downloader { get; private set; }

        public ITitleScraper Scraper { get; private set; }

        public string AdditionalMessage { get; set; }

        public HtmlTitleRetriever(IFileDownloader downloader, ITitleScraper scraper)
        {
            AdditionalMessage = "";
            Downloader = downloader;
            Scraper = scraper;
        }


        public string GetTitle(Uri file)
        {
            if (Downloader.SupportsUriScheme(file))
                return string.Concat(AdditionalMessage,
                                     Scraper.Scrape(Downloader.Download(file)));

            return string.Empty;
        }
    }
}