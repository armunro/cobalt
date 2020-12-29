using System;

namespace Cobalt.Collections.Map.Node
{
    [Flags]
    public enum FactNodeChangeState
    {
        NewItem = 1,
        ChangedItem = 2
    }
    
    public enum FactNodeHashRelation
    {
        Equal, 
        Collide, 
        Different
    }
    
    public enum FactNodeType
    {
        Nil,
        Value,
        Collision, 
        Reference
    }
}