using System;
using System.Net;
using WindsorSample.Downloaders;

namespace WindsorSample
{
    public class HttpFileDownloader : AbstractFileDownloader 
    {
        

        public override bool SupportsUriScheme(Uri file)
        {
            return file.Scheme == "http";
        }
    }
}