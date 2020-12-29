using Cobalt.BuiltIn.Stage;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cobalt.Web.Pages
{
    public class Pipe : PageModel
    {
        public Pipeline.CoPipe Pipeline { get; set; }

        public void OnGet()
        {
            Pipeline = Cb.CoPipe
                .Stage<LoadFileCoStage>(stg =>
                {
                    stg.FilePath = "sample.csv";
                    stg.ToFact = "Content";
                });

            Pipeline.In(Cb.Unit());
        }
    }
}