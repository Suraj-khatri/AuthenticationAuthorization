using Serilog;

namespace AuthenticationAuthorization.API.Helpers
{
    public static class LoggerHelper
    {
        public static async Task LogRequestAsync(HttpContext context, string status, string requestBody = "", string responseBody = "", Exception? exception = null)
        {
            var routeValues = context.Request.RouteValues;
            var controllerName = routeValues["controller"]?.ToString() ?? "UnknownController";
            var actionName = routeValues["action"]?.ToString() ?? "UnknownAction";
            var userId = context.User.Identity?.IsAuthenticated == true ? context.User.Identity.Name : "Anonymous";
            var correlationId = context.TraceIdentifier;

            // Get client IP (use X-Forwarded-For header if present)
            string? ipAddress = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (!string.IsNullOrEmpty(ipAddress))
                ipAddress = ipAddress.Split(',').First().Trim();
            else
                ipAddress = context.Connection.RemoteIpAddress?.ToString() ?? "Unknown";

            var log = Log.ForContext("UserId", userId)
                         .ForContext("ControllerName", controllerName)
                         .ForContext("ActionName", actionName)
                         .ForContext("RequestBody", requestBody)
                         .ForContext("ResponseBody", responseBody)
                         .ForContext("CorrelationId", correlationId)
                         .ForContext("IpAddress", ipAddress);

            if (status == "Error")
                log.Error(exception, "Error in {ControllerName}/{ActionName} [CorrelationId: {CorrelationId}, IpAddress: {IpAddress}]", controllerName, actionName, correlationId, ipAddress);
            else
                log.Information("Success at {Controller}/{Action} [CorrelationId: {CorrelationId}, IpAddress: {IpAddress}]",
                    controllerName, actionName, correlationId, ipAddress);

            await Task.CompletedTask;
        }
    }
}

