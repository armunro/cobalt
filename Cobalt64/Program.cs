using System.Threading.Tasks;
using Cobalt;
using Cobalt.Channel.Delimited.Input;
using Cobalt.Pipeline.Operation;

namespace Cobalt64
{
    internal static class Program
    {
        private static async Task Main(string[] args)
        {
            var pipe = Cblt.Pipeline
                .Channel<DelimitedFileDataChannel, DelimitedFileDataChannelOptions>(options =>
                    options.CsvFilePath = "icons.csv")
                .Stage(builder => builder.Operation(new CobaltOperation()));
            await pipe.ExecuteAsync();
        }
    }
}