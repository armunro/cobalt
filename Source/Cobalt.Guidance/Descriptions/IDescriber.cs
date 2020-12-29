namespace Cobalt.Guidance.Descriptions
{
    public interface IDescriber<T>
    {
        Description<T> Describe();
    }
}