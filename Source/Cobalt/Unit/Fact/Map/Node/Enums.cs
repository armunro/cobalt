using System;

namespace Cobalt.Unit.Fact.Map.Node
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
        Differ
    }
    
    public enum FactNodeType
    {
        Nil,
        Value,
        Collision, 
        Reference
    }
}