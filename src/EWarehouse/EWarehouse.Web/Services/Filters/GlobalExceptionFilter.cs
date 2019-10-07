using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;

namespace EWarehouse.Web.Services.Filters
{
    public class ErrorResponse
    {
        public string Message { get; set; }
        public string StackTrace { get; set; }
    }

    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILogger _logger;

        public GlobalExceptionFilter(ILoggerFactory loggerFactory)
        {

            if (loggerFactory == null)
            {
                throw new ArgumentNullException(nameof(loggerFactory));
            }

            _logger = loggerFactory.CreateLogger("Global Exception Filter");
        }

        public void OnException(ExceptionContext context)
        {
            _logger.LogError(context.Exception.Message, context.Exception.StackTrace);
           var response = new ErrorResponse()
            {
                Message = context.Exception.Message,
                StackTrace = context.Exception.StackTrace
            };

            context.Result = new ObjectResult(response)
            {
                StatusCode = 500,
                DeclaredType = typeof(ErrorResponse)
            };

            _logger.LogError("GlobalExceptionFilter", context.Exception);
        }
    }

}
