using System;

namespace Cobalt.Core.Structures.FactMap.Node
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