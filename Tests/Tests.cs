using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.Windsor;
using NUnit.Framework;
using WindsorSample;


namespace Tests
{
    [TestFixture]
    public class Tests
    {
        private IWindsorContainer container;
        [SetUp]
        public void SetUp()
        {
             container = new WindsorContainer();
            Pysor.PythonConfigurator.ConfigureContainer(container, "Castle.py");
        }
        [TearDown]
        public void TearDown()
        {
            container.Dispose();
        }

        [Test]
        public void CanConfigureSimpleInterface()
        {
            var obj = container.Resolve<ITitleScraper>();
            Assert.IsNotNull(obj);
            var obj2 = container.Resolve<IFileDownloader>();
            Assert.IsNotNull(obj2);
            
        }
        [Test]
        public void CanConfigureCompositeService()
        {
            var obj = container.Resolve<HtmlTitleRetriever>();
            Assert.IsNotNull(obj);
        }


        [Test]
        public void CanProvideOptionalParameters()
        {
            var obj = container.Resolve<HtmlTitleRetriever>("retriverWithParam");
            Assert.AreNotEqual("", obj.AdditionalMessage);
        }
        
        
        [Test, Ignore]
        public void CanProvideParameterArrays()
        {
            var messageStorage = container.Resolve<MessageStorage>();
            Assert.IsNotNull(messageStorage.Messages);
            Assert.IsNotEmpty(messageStorage.Messages);
        }

        [Test]
        public void CanProvideSpecificImplimentationParameters()
        {
            var obj = container.Resolve<HtmlTitleRetriever>("ftpRetriver");
            Assert.IsInstanceOf<FtpFileDownloader>(obj.Downloader);
        }

    }
}
