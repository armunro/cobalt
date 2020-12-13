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
            var unitsIn = new InMemUnitInput(Cb.Make.Unit.FromObject(new
            {
                Me = new
                {
                    FirstName = "Andrew",
                    LastName = "Munro"
                }
            }));
            var unitsOut = new InMemUnitOutput();

            System.Console.WriteLine("-- INPUTS");
            unitsIn.Units.ForEach(x => System.Console.WriteLine(x.ToString()));

            var pipeline = Cb.Pipeline
                .In(unitsIn)
                .Stage("Filter-1", stage => { stage.Step(new MyStep()); })
                .Out(unitsOut);

            await pipeline.ExecuteAsync();

            System.Console.WriteLine("-- OUTPUTS");
            unitsOut.Units.ForEach(x => System.Console.WriteLine(x.ToString()));
        }
    }
}