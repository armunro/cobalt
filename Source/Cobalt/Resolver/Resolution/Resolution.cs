using System;

namespace Cobalt.Resolver.Resolution
{
    public class Resolution<T>
    {
        private Func<T> _resolveFunc;


        public Resolution(Func<T> resolveFunc)
        {
            _resolveFunc = resolveFunc;
        }
    }
}