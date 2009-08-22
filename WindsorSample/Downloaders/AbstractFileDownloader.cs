using System;

namespace WindsorSample.Downloaders
{
    public abstract class AbstractFileDownloader :IFileDownloader
    {
        const string HtmFormatString = "<html><title>{0}</title><body><body><h1>Test<h1></html>";
        public string Download(Uri file)
        {
            
            return String.Format(HtmFormatString, GetType().Name);
        }

        public abstract bool SupportsUriScheme(Uri file);
    }
}