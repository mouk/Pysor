using System;
using System.IO;
using System.Net;
using WindsorSample.Downloaders;

namespace WindsorSample
{
    public class FtpFileDownloader :  AbstractFileDownloader 
    {
        public override bool SupportsUriScheme(Uri file)
        {
            return file.Scheme == "ftp";
        }
    }
}