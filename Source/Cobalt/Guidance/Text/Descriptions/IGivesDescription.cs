using Cobalt.Pipeline.Stage;

namespace Cobalt.Guidance.Text.Descriptions
{
    public interface IGivesDescription<TDescribes>
    {
        StageDescription Describe();
    }
}