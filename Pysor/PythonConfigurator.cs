using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Castle.Core.Configuration;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using IronPython.Hosting;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;

namespace Pysor
{
    public static class PythonConfigurator
    {
        public static void ConfigureContainer(IWindsorContainer container, string scriptPath)
        {
            var engine = Python.CreateEngine();
            var runtime = engine.Runtime;
            var scope = runtime.CreateScope();
            
            Func<string, Type, Type, IDictionary<object, object>, string> action =
                (name, service, impl, parameters) =>
                    {
                        var reg = Component
                            .For(service)
                            .ImplementedBy(impl)
                            .Named(name);

                        var pairs = parameters.ToList();
                        if (pairs.Count > 0)
                        {
                            var overrides = pairs.Where(p => p.Value.ToString().StartsWith("${")).ToList();
                            var rest = pairs.Except(overrides).ToList();

                           var param = GenerateDctionary(rest);
                                //.Select(pair => GetParameterFromPair(pair))
                                //.ToDictionary()
                                //.ToArray();
                            reg = reg.DependsOn( param);
                            reg.ServiceOverrides(GenerateDctionary(overrides));
                        }
                        container.Register(reg);
                        return "${" + name + "}";
                    };

            //Inject this function into IronPython runtime
            scope.SetVariable("addComponent", action);
            
            ConfigureEngineWithBaseFunctions(engine, scope);


            var script = engine.CreateScriptSourceFromFile(scriptPath);
            var code = script.Compile();
            code.Execute(scope);
        }

        private static IDictionary GenerateDctionary(List<KeyValuePair<object, object>> pairs)
        {
            var dic = new Dictionary<object, object>();

            foreach (var pair in pairs)
            {
                dic[pair.Key.ToString()] = pair.Value;
            }

            return dic;
        }

        private static void ConfigureEngineWithBaseFunctions(ScriptEngine engine, ScriptScope scope)
        {
            scope.SetVariable("__main__", "__main__");
            string builtinFunction = GetBuiltinFunctions();

            
            var script = engine.CreateScriptSourceFromString(builtinFunction,SourceCodeKind.File);
            var code = script.Compile();
            code.Execute(scope);
        }

        private static string GetBuiltinFunctions()
        {
            var assembly = typeof (PythonConfigurator).Assembly;
            using (var stream = assembly.GetManifestResourceStream("Pysor.Functions.py"))
            using(var reader = new StreamReader(stream))
                return reader.ReadToEnd();

        }

        private static Parameter GetParameterFromPair(KeyValuePair<object, object> pair)
        {
            return Parameter
                .ForKey(pair.Key.ToString())
                .Eq(ResolveValue(pair.Value));
        }

        private static string ResolveValue(object value)
        {
            return value.ToString();
        }
    }
}
