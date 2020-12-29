using Cobalt.Guidance.Visuals.Graphics;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cobalt.Web.Pages
{
    public class Registry : PageModel
    {
        public Resolver.Resolver<Graphic> GraphicResolver { get; }

        public Registry(Resolver.Resolver<Graphic> graphicResolver)
        {
            GraphicResolver = graphicResolver;
        }
        public void OnGet()
        {
            
        }
    }
}