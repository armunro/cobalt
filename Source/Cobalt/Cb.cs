using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cobalt.Pipeline;
using Cobalt.Unit;

namespace Cobalt
{
    public class Cb
    {
        public static CobaltPipeline Pipeline => new CobaltPipeline();

        public static Maker Make => new Maker();
    }

    public class Maker
    {
        public UnitMaker Unit => new UnitMaker();
    }

    public class UnitMaker
    {
        public CobaltUnit FromObject(object source,
            BindingFlags bindingAttr = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
        {
            var objects = AsDictionary(source);
            return CobaltUnit.Make(objects);
        }
        
        public static IDictionary<string, object> AsDictionary(object source, BindingFlags bindingAttr = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
        {
            return source.GetType().GetProperties(bindingAttr).ToDictionary
            (
                propInfo => propInfo.Name,
                propInfo => propInfo.GetValue(source, null)
            );

        }
    }
}