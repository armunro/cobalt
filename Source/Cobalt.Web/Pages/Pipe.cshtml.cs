using Cobalt.BuiltIn.Stage;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cobalt.Web.Pages
{
    public class Pipe : PageModel
    {
        public Cobalt.Pipe Pipeline { get; set; }

        public void OnGet()
        {
            Pipeline = new Cobalt.Pipe()
                .Stage<LoadFileStage>(stg =>
                {
                    stg.FilePath = "sample.csv";
                    stg.ToFact = "Content";
                });

            Pipeline.In(Unit.Make());
        }
    }
}