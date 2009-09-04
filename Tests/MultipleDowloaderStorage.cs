using WindsorSample;

namespace Tests
{
    public class MultipleDowloaderStorage 
    {
        private IFileDownloader[] _dowloaders;

        public MultipleDowloaderStorage(IFileDownloader[] dowloaders)
        {
            _dowloaders = dowloaders;
        }

        public IFileDownloader[] Dowloader
        {
            get { return _dowloaders; }
        }
    }
}