using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mime;
using System.Text.Json;

namespace ETicaretAPI.API.Extensions
{

    //bu middleware'in çalışabilmesi için uygulamamıza servis olarak kaydının yapılması gerek.
    public class GlobalExceptionHandlingMiddleware2 : IMiddleware
    {
		readonly ILogger _logger;

        public GlobalExceptionHandlingMiddleware2(ILogger logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
			try
			{
				await next(context);
			}
			catch (Exception e)
			{
                _logger.LogError(e, e.Message);
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                ProblemDetails problem = new()
                {
                    Status = context.Response.StatusCode,
                    Type = "Server error",
                    Title = "Internal Error",
                    Detail = e.Message
                };


                string json = JsonSerializer.Serialize(problem);

                await context.Response.WriteAsync(json);
                //"application/json"
                context.Response.ContentType = MediaTypeNames.Application.Json;

			}
        }
    }
}
