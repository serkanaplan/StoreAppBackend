using Microsoft.AspNetCore.Mvc.Filters;
using Store.Entities;
using Store.Entities.LogModels;
using Store.Service.Contracts;

namespace Store.API;

public class LogFilterAttribute :ActionFilterAttribute
{
        private readonly ILoggerService _logger;

        public LogFilterAttribute(ILoggerService logger)
        {
            _logger = logger;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInfo(Log("OnActionExecuting", context.RouteData));
        }

        private string Log(string modelName, RouteData routeData)
        {
            var logDetails = new LogDetail()
            {
                ModelModel = modelName,
                Controller = routeData.Values["controller"],
                Action = routeData.Values["action"]
            };

            if (routeData.Values.Count >= 3)
                logDetails.Id = routeData.Values["Id"];
            
            return logDetails.ToString();
        }
}
