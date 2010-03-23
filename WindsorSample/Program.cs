using System;
using System.Configuration;
using Castle.Windsor;
using Castle.Windsor.Configuration.Interpreters;

namespace WindsorSample
{
    public class Program
    {
        private static void Main()
        {
            //IWindsorContainer container = new WindsorContainer(new XmlInterpreter());

            //HtmlTitleRetriever retriever = container.Resolve<HtmlTitleRetriever>();

            //Console.WriteLine(retriever.GetTitle(new Uri(ConfigurationManager.AppSettings["fileUri"])));
            
            //container.Release(retriever);

            IWindsorContainer container = new WindsorContainer();
            Pysor.PythonConfigurator.ConfigureContainer(container, "Castle_Configuration.py");
            var ding = container.Resolve<ITitleScraper>();
        }
    }
}