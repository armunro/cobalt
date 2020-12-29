using Cobalt.BuiltIn.Stages.Files;
using Cobalt.Core;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cobalt.Web.Pages
{
    public class Pipe : PageModel
    {
        public Core.Pipe Pipeline { get; set; }

        public void OnGet()
        {
            Pipeline = new Core.Pipe()
                .Stage<LoadFileStage>(stg =>
                {
                    stg.FilePath = "sample.csv";
                    stg.ToFact = "Content";
                });

            Pipeline.In(Unit.Make());
        }
    }
}