using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using Cobalt.Pipeline;
using Cobalt.Unit;

namespace Cobalt
{
    public class Cb
    {
        public static CobaltPipeline Pipe => new CobaltPipeline();
        public static CobaltUnit Unit() => new CobaltUnit();
        public static CobaltUnit Unit(dynamic source) => CobaltUnit.Make(AsDictionary(source));

      
        
        private static IDictionary<string, object> AsDictionary(object source)
        {
            BindingFlags flags = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance;
            
            return source.GetType().GetProperties(flags).ToDictionary
            (
                propInfo => propInfo.Name,
                propInfo => propInfo.GetValue(source, null)
            );

        }
    }

}