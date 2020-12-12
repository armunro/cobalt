using System.Threading.Tasks;
using Cobalt.Pipeline.Channel.Local;
using Cobalt.Pipeline.Steps;
using Cobalt.Pipeline.Steps.Local;
using Cobalt.Unit;


namespace Cobalt.Console
{
    internal static class Program
    {
        public static async Task Main(string[] args)
        {
            var firstUnit = new CobaltUnit();
          


            var unitInput = new InMemUnitInput(firstUnit);
            var unitOutput = new InMemUnitOutput();
            
            foreach (var unit in unitInput.Units)
            {
                System.Console.WriteLine(unit.ToString());
            }

            var pipeline = Cblt.Pipeline
                .In(unitInput)
                .Stage("Filter-1", stage =>
                {
                    stage
                        .Step(new MyStep());
                })
                .Out(unitOutput);

            await pipeline.ExecuteAsync();

            foreach (var unit in unitOutput.Units)
            {
                System.Console.WriteLine(unit.ToString());
            }
        }
    }
}