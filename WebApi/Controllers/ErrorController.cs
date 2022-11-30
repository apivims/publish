using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Error;

namespace WebApi.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class ErrorController : ControllerBase
    {
        private IConfiguration _configuration;
         private ILog _logger;
        public ErrorController(IConfiguration configuration ,ILog logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        [HttpGet]
        [Route("/error-production")]
        public IActionResult ErrorLocalDevelopment(
        [FromServices] IWebHostEnvironment webHostEnvironment)
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();

            _logger.Error(context.Error.Message);
            //Error code
           

            return Problem(
                detail: context.Error.StackTrace,
                title: context.Error.Message);
        }

    }
}
