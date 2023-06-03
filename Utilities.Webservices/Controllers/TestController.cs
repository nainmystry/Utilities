using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Utilities.Webservices.Controllers
{
    //[ApiVersion("1.0")]
    //[Route("api/v{version:apiVersion}/[controller]")]
    //[ApiController]
    public class TestController : ControllerBase
    {

        [Route("/")]
        [MapToApiVersion("1.0")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            IActionResult httpResponse = new ContentResult 
            { 
            StatusCode = 200,
            Content = "This is Utilities Web Service."
            };

            return httpResponse;
        }

        [Route("/v{version:apiVersion}/Test")]
        [Route("/Test")]
        [MapToApiVersion("1.0")]
        [MapToApiVersion("2.0")]
        [HttpGet]
        public async Task<IActionResult> GetV()
        {
            var reqVersion = !string.IsNullOrWhiteSpace(HttpContext.GetRequestedApiVersion()?.ToString()) ? HttpContext.GetRequestedApiVersion().ToString() : string.Empty;
            IActionResult httpResponse;
            if (reqVersion.Equals("1"))
            {
                httpResponse = new ContentResult
                {
                    StatusCode = 200,
                    Content = "This is Utilities Web Service from version 1."
                };
            }
            else if (reqVersion.Equals("2"))
            {
                httpResponse = new ContentResult
                {
                    StatusCode = 200,
                    Content = "This is Utilities Web Service from version 2."
                };
            }
            else
            {
                httpResponse = new ContentResult
                {
                    StatusCode = 200,
                    Content = "This is Utilities Web Service"
                };
            }


            return Ok("This is returned data.");
            //return httpResponse;
        }
    }
}
