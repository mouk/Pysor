﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Castle.Core;
using Castle.Core.Configuration;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using IronPython.Hosting;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;

namespace Pysor
{
    public static class PythonConfigurator
    {
        private delegate object InjectionDelegate(
            string name, 
            Type service, 
            Type implementation,
            IDictionary<object, object> parameters,
            string lifestyle);

        public static void ConfigureContainer(IWindsorContainer container, string scriptPath)
        {
            var engine = Python.CreateEngine();
            var runtime = engine.Runtime;
            //runtime.LoadAssembly(typeof(Castle.MicroKernel.Resolvers.SpecializedResolvers.ArrayResolver).Assembly);
            var scope = runtime.CreateScope();

            InjectionDelegate action =
                (name, service, impl, parameters, lifestyle) =>
                    {
                        var reg = Component
                            .For(service)
                            .ImplementedBy(impl)
                            .Named(name);

                        if(!string.IsNullOrEmpty(lifestyle))
                        {
                            var lifestyleType =
                               (LifestyleType)Enum.Parse(typeof(LifestyleType), lifestyle,true);
                            reg.LifeStyle.Is(lifestyleType);
                        }
                        

                        var pairs = parameters.ToList();
                        if (pairs.Count > 0)
                        {
                            var overrides = pairs.Where(IsLookup).ToList();
                            var rest = pairs.Except(overrides).ToList();

                           var param = GenerateDctionary(rest);
                            
                            reg = reg.DependsOn(param);


                            //reg.ServiceOverrides(GenerateDctionary(overrides));
                            if (overrides.Count != 0)
                                foreach (var serviceOverride in GenerateOverrides(overrides))
                                    reg.ServiceOverrides(serviceOverride);
                        }
                        container.Register(reg);
                        return new LookUp(name);
                        
                    };

            //Inject this function into IronPython runtime
            scope.SetVariable("addComponent", action);
            scope.SetVariable("kernel", container.Kernel);
            
            
            ConfigureEngineWithBaseFunctions(engine, scope);


            var script = engine.CreateScriptSourceFromFile(scriptPath);
            var code = script.Compile();
            code.Execute(scope);
        }

        private static bool IsLookup(KeyValuePair<object, object> p)
        {
            var value = p.Value;

            bool lookup = value is LookUp;
            if (lookup)
                return true;


            var list = value as IList;

            return  list != null && list[0] is LookUp;
        }


        private static IDictionary GenerateDctionary(List<KeyValuePair<object, object>> pairs)
        {
            var dic = new Dictionary<object, object>();

            foreach (var pair in pairs)
            {
                dic[pair.Key.ToString()] = ResolveValue(pair.Value);
            }
            
            return dic;
        }

        private static IEnumerable<ServiceOverride> GenerateOverrides(List<KeyValuePair<object, object>> pairs)
        {
            var dic = new Dictionary<object, object>();

            foreach (var pair in pairs)
            {
                var ding = ServiceOverride.ForKey(pair.Key.ToString());
                var val = pair.Value;
                if (val is string)
                    yield return ding.Eq((string)val);
                else if (val is LookUp)
                    yield return ding.Eq((val as LookUp).Key);
                else if (val is IList)
                {
                    var objectArray = (IList)val;
                    //TODO what if objects are neither string nor LookUp ?
                    var stringArray = new List<object>(objectArray.Cast<object>()).
                        Select(obj => obj.ToString()).
                        ToArray();

                    yield return ding.Eq(stringArray);
                }
                else
                    throw new ApplicationException("Not handeled case");
            }
        }

       

        private static object ResolveValue(object value)
        {
            if (value is LookUp)
            {
                return ((LookUp) value).Key;
            }
            var list = value as IEnumerable<object>;
            if (list != null)
            {
                var tempList = list
                    .Select(obj => ResolveValue(obj))
                    .ToList();

                var typeTool = new CommonTypeTool();
                var type = typeTool.GetCommonType(tempList);

                var arr = Array.CreateInstance(type, tempList.Count);

                tempList.ToArray().CopyTo(arr,0);

                return arr;
            }
            return value; //.ToString();
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

        
        private class LookUp
        {
            public string Key { get; set; }

            public LookUp(string key)
            {
                Key = key; //"${" + key + "}";
            }

            public override string ToString()
            {
                return Key;
            }
        }
    }


}
