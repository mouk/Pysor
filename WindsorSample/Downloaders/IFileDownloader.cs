using System;

namespace WindsorSample
{
    public interface IFileDownloader
    {
        string Download(Uri file);
        bool SupportsUriScheme(Uri file);
    }
}