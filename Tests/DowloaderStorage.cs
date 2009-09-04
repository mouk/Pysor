using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindsorSample;

namespace Tests
{
    public class DowloaderStorage 
    {
        private IFileDownloader _dowloader;

        public DowloaderStorage(IFileDownloader dowloaders)
        {
            _dowloader = dowloaders;
        }

        public IFileDownloader Dowloader
        {
            get { return _dowloader; }
        }
    }
}
