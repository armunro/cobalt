using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cobalt.Pipeline;

namespace Cobalt
{
    public class Cb
    {
        public static Pipe Pipe => new Pipe();
        public static Unit.Unit Unit() => new Unit.Unit();
        public static Unit.Unit Unit(dynamic source) => Cobalt.Unit.Unit.Make(AsDictionary(source));

      
        
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