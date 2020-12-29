using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cobalt.Pipeline;
using Cobalt.Unit;

namespace Cobalt
{
    public class Cb
    {
        public static CoPipe CoPipe => new CoPipe();
        public static CoUnit Unit() => new CoUnit();
        public static CoUnit Unit(dynamic source) => CoUnit.Make(AsDictionary(source));

      
        
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