using System;

namespace Cobalt.Sourcing
{
    public class SourceConnection<T>
    {
        private Func<T> _valueFunc;


        public SourceConnection(Func<T> valueFunc)
        {
            _valueFunc = valueFunc;
        }
    }
}