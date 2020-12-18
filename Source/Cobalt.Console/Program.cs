using System.Dynamic;
using System.IO;
using System.Net;
using System.Runtime.Remoting.Channels;
using System.Threading.Tasks;
using Cobalt.Pipeline.Stage;
using Microsoft.SqlServer.Server;


namespace Cobalt.Console
{
    internal static class Program
    {
        public static async Task Main(string[] args)
        {
            //A unit created from a dynamic builder works as well
            var unit = Cb.Unit();


            var pipeline = Cb.Pipe
                .Stage<LoadFileStage>(stg =>
                {
                    stg.FilePath = "my.csv";
                    stg.TargetFact = "Content";
                })
                .Stage<ParseCsvStage>(stg => stg.Delimiter = ",");
        }
    }
}