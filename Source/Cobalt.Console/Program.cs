using System.Threading.Tasks;
using Cobalt.Pipeline.Channel.Local;
using Cobalt.Pipeline.Operation;
using Cobalt.Unit;

namespace Cobalt.Console
{
    internal static class Program
    {
        public static async Task Main(string[] args)
        {
            // CobaltUnits are dynamic objects with extra features
            dynamic first = new CobaltUnit();
            first.PropertyA = "Alphabet";
            first.PropertyB = "Baseball";

            dynamic second = new CobaltUnit();
            second.PropertyA = "Apple";
            second.PropertyB = "Dog";
            second.PropertyC = "Cat";

            InMemInputChannel inputChannel = new InMemInputChannel(first, second);
      
            var pipeline = Cblt.Pipeline
                .In(inputChannel)
                .Stage("Filter-1",
                    stage =>
                    {
                        stage.Step(new FilterStep(unit =>
                        {
                            return unit.PropertyA.StartsWith("A") || unit.PropertyA.StartsWith("B");
                        }));
                    });

            await pipeline.ExecuteAsync();
        }
    }
}