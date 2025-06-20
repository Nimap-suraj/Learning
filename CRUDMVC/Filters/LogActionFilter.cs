using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDMVC.Filters
{
    public class LogActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var controller = context.RouteData.Values["controller"];
            var action = context.RouteData.Values["action"];
            var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            string logMessage = $"[{timestamp}] - Executing {controller}/{action}";

            // Save to a local file (App_Data/log.txt or root)
            File.AppendAllText("action_log.txt", logMessage + Environment.NewLine);

            base.OnActionExecuting(context);
        }
    }
}
