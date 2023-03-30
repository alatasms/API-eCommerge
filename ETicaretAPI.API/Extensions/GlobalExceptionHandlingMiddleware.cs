using System.Net;

namespace ETicaretAPI.API.Extensions
{
    //Service olarak tanıtılmasına gerek yoktur. çünkü ilgili RequestDelegate instancelarını biz manuel şeklinde DI'den çekiyoruz.
    public class GlobalExceptionHandlingMiddleware
    {
        readonly RequestDelegate _next;
        readonly ILogger _logger;

        public GlobalExceptionHandlingMiddleware(RequestDelegate next, ILogger logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
        }
    }
}
