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


            InMemInputChannel inputChannel = new InMemInputChannel();
            inputChannel.AddUnits(first, second);


            var pipe = Cblt.Pipeline
                .Channel(inputChannel)
                .Stage(builder => builder
                    .Step(new FilterStep(unit => unit.PropertyA.StartsWith("A")))
                    .Step(new FilterStep(unit => unit.PropertyB.StartsWith("B")))
                );

            await pipe.ExecuteAsync();
        }
    }
}