using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
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
            container.Kernel.Resolver.AddSubResolver(new ArrayResolver(container.Kernel));
            Pysor.PythonConfigurator.ConfigureContainer(container, "Castle_Configuration.py");
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


        [Test]
        public void CanProvideParameterStringArrays()
        {
            var messageStorage = container.Resolve<MessageStorage>();
            Assert.IsNotNull(messageStorage.Messages);
            Assert.AreEqual(2, messageStorage.Messages.Length);
        }

        [Test]
        public void CanProvideParameterLookupArrays()
        {
            var messageStorage = container.Resolve<MultipleDowloaderStorage>();
            Assert.IsNotNull(messageStorage.Dowloader);
        }
        [Test]
        public void CanProvideParameterSingleLookup()
        {
            var messageStorage = container.Resolve<DowloaderStorage>();
            Assert.IsNotNull(messageStorage.Dowloader);
        }

        [Test]
        public void CanProvideSpecificImplimentationParameters()
        {
            var obj = container.Resolve<HtmlTitleRetriever>("ftpRetriver");
            Assert.IsInstanceOf<FtpFileDownloader>(obj.Downloader);
        }

        [Test]
        public void CanSetLifeStyle()
        {
            var obj = container.Resolve<object>("transient");
            var secondObj = container.Resolve<object>("transient");

            Assert.AreNotEqual(obj, secondObj);
        }

        [Test]
        public void CanSetIntegerProperty()
        {
            var simpleClass = container.Resolve<SimpleClass>();
            Assert.AreEqual(10,simpleClass.IntegerValue);
        }

        [Test]
        public void CanSetBooleanProperty()
        {
            var simpleClass = container.Resolve<SimpleClass>();
            Assert.AreEqual(true,simpleClass.BooleanValue);
        }

    }
}
