using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using IronPython.Hosting;

namespace Pysor
{
    public static class PythonConfigurator
    {
        public static void ConfigureContainer(IWindsorContainer container, string scriptPath)
        {
            var engine = Python.CreateEngine();
            var runtime = engine.Runtime;
            var scope = runtime.CreateScope();
            scope.SetVariable("__main__", "__main__");
           Func<string, Type, Type, IDictionary<object, object> , string> action =
                (name, service, impl, parameters) =>
                    {
                        var pairs = parameters.ToList();
                        var reg = Component
                            .For(service)
                            .ImplementedBy(impl)
                            .Named(name);
                        if (pairs.Count > 0)
                        {
                            var param = (from pair in pairs
                                         select Parameter
                                            .ForKey(pair.Key.ToString())
                                            .Eq(pair.Value.ToString())
                                        ).ToArray();
                            reg = reg.Parameters(param);
                        }
                        container.Register(reg);
                        return "${" + name + "}";
                    };

            //Inject this function into IronPython runtime
           scope.SetVariable("addComponent", action);
            var script = engine.CreateScriptSourceFromFile(scriptPath);
            var code = script.Compile();
            code.Execute(scope);
        }
    }
}
