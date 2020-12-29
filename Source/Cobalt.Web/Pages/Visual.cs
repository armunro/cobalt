using Cobalt.Guidance.Visuals;
using Microsoft.AspNetCore.Mvc;

namespace Cobalt.Web.Pages
{
    public class Visual : Controller
    {
        // GET
        public IActionResult Index([FromQuery] string type)
        {
            switch (type)
            {
                case "stage":
                    return Content(new StageVisual().Compile(), "image/svg+xml; charset=utf-8");
                    break;
                case "unit":
                    return Content(new UnitVisual().Compile(), "image/svg+xml; charset=utf-8");
                default:
                    return Content("NotFound");
            }
        }
    }
}