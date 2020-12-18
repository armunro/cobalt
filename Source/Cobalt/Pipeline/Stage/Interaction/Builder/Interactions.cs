using Cobalt.Interaction.Unit;

namespace Cobalt.Interaction.Builder
{
    public class Interactions
    {
        public static UnitInteractions Unit => new UnitInteractions();
    }

    public class UnitInteractions
    {
        public static UnitFactInteractions Facts => new UnitFactInteractions();
    }

    public class UnitFactInteractions
    {
        public static CreateFactInteraction Create(string factName, object stringValue) =>
            new CreateFactInteraction(factName, factName);
    }
}