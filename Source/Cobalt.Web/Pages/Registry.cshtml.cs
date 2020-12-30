using Cobalt.BuiltIn.Sourcing.Graphics;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cobalt.Web.Pages
{
    public class Registry : PageModel
    {
        public GraphicSource GraphicSource { get; }

        public Registry(GraphicSource graphicSource)
        {
            GraphicSource = graphicSource;
        }
        public void OnGet()
        {
            
        }
    }
}