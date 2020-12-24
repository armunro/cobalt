using Cobalt.Pipeline.Stage;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cobalt.Web.Pages
{
    public class Pipe : PageModel
    {
        public Pipeline.Pipe Pipeline { get; set; }

        public void OnGet()
        {
            Pipeline = Cb.Pipe
                .Stage<LoadFileStage>(stg =>
                {
                    stg.FilePath = "first.csv";
                    stg.ToFact = "Content";
                })
                .Stage<ParseCsvStage>(stg =>
                {
                    stg.FromFact = "Content";
                    stg.ToFact = "Content";
                });

            Pipeline.In(Cb.Unit());
        }
    }
}