using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Repository;

namespace WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private IConfiguration _configuration;
        private readonly ITest _test;

        public TestController(IConfiguration configuration,ITest test)
        {
            _configuration = configuration;
            _test = test;
        }


        [HttpGet]
        public IActionResult   GetAllPatient([FromHeader] string fromDate, [FromHeader] string toDate)
        {
            DateTime mfromDate = DateTime.Now;
            DateTime mtoDate = DateTime.Now;



            mfromDate = DateTime.ParseExact(fromDate, "dd/MM/yyyy HH:mm:ss",
                                             System.Globalization.CultureInfo.InvariantCulture);
            mtoDate = DateTime.ParseExact(toDate, "dd/MM/yyyy HH:mm:ss",
                                         System.Globalization.CultureInfo.InvariantCulture);

            var plist =   _test.GetAllPatientAsync(mfromDate, mtoDate);

            return Ok(plist);
        }
    }
}
