namespace Cobalt.Guidance.Visuals
{
    public interface IVisualizer<T>
    {
        Visual<T> Visualize(T source);
    }
}