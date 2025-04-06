using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace OnlineShoppingApp.WebApi.Filters;

public class TimeControlFilter : ActionFilterAttribute
{
    public string StartTime { get; set; }
    public string EndTime { get; set; }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var now = DateTime.Now.TimeOfDay;

        StartTime = "20:00";
        EndTime = "23:59";

        // Block request if out of allowed time range
        if (now >= TimeSpan.Parse(StartTime) && now <= TimeSpan.Parse(EndTime))
        {
            base.OnActionExecuting(context);
        }
        else
        {
            context.Result = new ContentResult
            {
                Content = "Cannot use app in this period of time.",
                StatusCode = 403
            };
        }
    }
}