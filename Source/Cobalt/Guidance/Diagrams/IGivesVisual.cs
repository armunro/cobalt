namespace Cobalt.Guidance.Diagrams
{
    public interface IGivesVisual<TVisualizes>
    {
        Visual<TVisualizes> Visualize();
    }
}