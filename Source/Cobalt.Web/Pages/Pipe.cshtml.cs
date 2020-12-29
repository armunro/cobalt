using Cobalt.BuiltIn.Stage;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cobalt.Web.Pages
{
    public class Pipe : PageModel
    {
        public CoPipe Pipeline { get; set; }

        public void OnGet()
        {
            Pipeline = new CoPipe()
                .Stage<LoadFileCoStage>(stg =>
                {
                    stg.FilePath = "sample.csv";
                    stg.ToFact = "Content";
                });

            Pipeline.In(CoUnit.Make());
        }
    }
}