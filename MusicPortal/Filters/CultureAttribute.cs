// MusicPortal/Filters/CultureAttribute.cs
using Microsoft.AspNetCore.Mvc.Filters;

namespace MusicPortal.Filters
{
    public class CultureAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // No-op: culture handled via middleware
        }
    }
}
