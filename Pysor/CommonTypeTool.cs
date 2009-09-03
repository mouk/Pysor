using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pysor
{
    public class CommonTypeTool
    {
        public  Type GetCommonType(IList<object> list)
        {
            if (list.Count == 0)
                return typeof (object);
            Type ret = list[0].GetType();
            foreach (var o in list)
            {
                Type type = o.GetType();
                while (!ret.IsAssignableFrom(type))
                {
                    ret = ret.BaseType;
                }
            }
            return ret;
        }
    }
}
