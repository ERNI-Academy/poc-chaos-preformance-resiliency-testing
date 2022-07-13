using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Microservices.GeneratingChaos.BuildingBlocks.Helpers
{
    /// <summary>
    /// Class HttpGlobalExceptionFilter.
    /// Implements the <see cref="Microsoft.AspNetCore.Mvc.Filters.IExceptionFilter" />
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Filters.IExceptionFilter" />
    public partial class HttpGlobalExceptionFilter : IExceptionFilter
    {
        /// <summary>
        /// The env
        /// </summary>
        private readonly IHostingEnvironment env;
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<HttpGlobalExceptionFilter> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpGlobalExceptionFilter"/> class.
        /// </summary>
        /// <param name="env">The env.</param>
        /// <param name="logger">The logger.</param>
        public HttpGlobalExceptionFilter(IHostingEnvironment env, ILogger<HttpGlobalExceptionFilter> logger)
        {
            this.env = env;
            this.logger = logger;
        }

        /// <summary>
        /// Called after an action has thrown an <see cref="T:System.Exception" />.
        /// </summary>
        /// <param name="context">The <see cref="T:Microsoft.AspNetCore.Mvc.Filters.ExceptionContext" />.</param>
        public void OnException(ExceptionContext context)
        {
            logger.LogError(new EventId(context.Exception.HResult),
                context.Exception,
                context.Exception.Message);

            var json = new JsonErrorResponse
            {
                Messages = new[] { env.IsDevelopment() ? context.Exception.Message : "An error occurred. Try it again." },
                DeveloperMessage = env.IsDevelopment() ? context.Exception : null
            };

            context.Result = new BadRequestObjectResult(json);
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.ExceptionHandled = true;
        }
        /// <summary>
        /// Class JsonErrorResponse.
        /// </summary>
        private class JsonErrorResponse
        {
            /// <summary>
            /// Gets or sets the messages.
            /// </summary>
            /// <value>The messages.</value>
            public string[] Messages { get; set; }

            /// <summary>
            /// Gets or sets the developer message.
            /// </summary>
            /// <value>The developer message.</value>
            public object DeveloperMessage { get; set; }
        }
    }
}
