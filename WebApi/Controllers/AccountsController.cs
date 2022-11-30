using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Error;
using WebApi.Identity;
using WebApi.Jwt;

namespace WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    
    public class AccountsController : ControllerBase
    {

        private readonly IJwtAuth jwtAuth;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILog _logger;
        private readonly IConfiguration _configuration;



        //private readonly List<User> lstMember = new List<User>()
        //{
        //    new User{Id=1, Name="admin"},
        //    new User {Id=2, Name="Normal" },
        //    new User{Id=3, Name="pankaj"}
        //};

        public AccountsController(IJwtAuth jwtAuth , UserManager<IdentityUser> userManager,
                              SignInManager<IdentityUser> signInManager , ILog logger , IConfiguration iconfiguration)
        {
            this.jwtAuth = jwtAuth;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _configuration = iconfiguration;
        }
        // GET: api/<MembersController>
        //[HttpGet]
        //public IEnumerable<User> AllUsers()
        //{
        //    return lstMember;
        //}

        // GET api/<MembersController>/5
        //[HttpGet("{id}")]
        //public User UserByid(int id)
        //{
        //    return lstMember.Find(x => x.Id == id);
        //}

    
        [AllowAnonymous]
        // POST api/<MembersController>
        [HttpPost("authentication")]
        public async Task <IActionResult> Authentication([FromBody] UserCredential userCredential)
        {
            _logger.Information($"Login Successfully! user name: {userCredential.client_id}");

            var result = await _signInManager.PasswordSignInAsync(userCredential.client_id, userCredential.client_secret, false, false);

            if  (result.Succeeded)
            {
                _logger.Information($"Login Successfully! user name: {userCredential.client_id}");              
            }
            else
            {
                return Unauthorized();
            }


            var token = jwtAuth.Authentication(userCredential.client_id, userCredential.client_secret);
            if (token == null)
                return Unauthorized();

            ResponseToken responseToken = new ResponseToken();
            responseToken.access_token = token;
            responseToken.token_type = "Bearer";
            responseToken.expires_in = "3600";

            return Ok(responseToken);
        }



        //[AllowAnonymous]
        //// POST api/<MembersController>
        //[HttpPost("authentication2")]
        //public  IActionResult Authentication2([FromBody] UserCredential userCredential)
        //{
        //    try
        //    {

        //        _logger.Information($"Login Successfully! user name: {userCredential.client_id}");

        //        var result = _signInManager.PasswordSignInAsync(userCredential.client_id, userCredential.client_secret, false, false);

        //        //if (result.ConfigureAwait)
        //        //{
        //        //    _logger.Information($"Login Successfully! user name: {userCredential.client_id}");
        //        //}
        //        //else
        //        //{
        //        //    return Unauthorized();
        //        //}


        //        var token = jwtAuth.Authentication(userCredential.client_id, userCredential.client_secret);
        //        if (token == null)
        //            return Unauthorized();

        //        ResponseToken responseToken = new ResponseToken();
        //        responseToken.access_token = token;
        //        responseToken.token_type = "Bearer";
        //        responseToken.expires_in = "3600";

        //        return Ok(responseToken);


        //    }
        //    catch (Exception ex)
        //    {

        //        return Ok(ex.Message);
        //    }
            
        //  //  return Ok("Narendra Authentication");
        //}

        //[AllowAnonymous]
        //// POST api/<MembersController>
        //[HttpPost("authentication3")]
        //public IActionResult Authentication3([FromBody] UserCredential userCredential)
        //{
        //    try
        //    {

        //        //_logger.Information($"Login Successfully! user name: {userCredential.client_id}");

        //        //var result = _signInManager.PasswordSignInAsync(userCredential.client_id, userCredential.client_secret, false, false);

        //        ////if (result.ConfigureAwait)
        //        ////{
        //        ////    _logger.Information($"Login Successfully! user name: {userCredential.client_id}");
        //        ////}
        //        ////else
        //        ////{
        //        ////    return Unauthorized();
        //        ////}


        //        //var token = jwtAuth.Authentication(userCredential.client_id, userCredential.client_secret);
        //        //if (token == null)
        //        //    return Unauthorized();

        //        //ResponseToken responseToken = new ResponseToken();
        //        //responseToken.access_token = token;
        //        //responseToken.token_type = "Bearer";
        //        //responseToken.expires_in = "3600";

        //        return Ok("try");


        //    }
        //    catch (Exception ex)
        //    {

        //        return Ok(ex.Message);
        //    }

        //    //  return Ok("Narendra Authentication");
        //}


        [AllowAnonymous]
       // POST api/<MembersController>
        [HttpPost("register")]
        public async Task <IActionResult> Register([FromBody] User userModel,[FromHeader] string regcode)
        {


            //  if (ModelState.IsValid)
            // {

            if(regcode==null || regcode != _configuration.GetValue<string>("HospitalSetting:RegCode"))
            {
                return BadRequest();
            }




            var user = new IdentityUser
            {
                UserName = userModel.UserName,
                Email = userModel.Email,
                PhoneNumber=userModel.Password
                
            };

                var result = await _userManager.CreateAsync(user, userModel.Password);

                if (result.Succeeded)
                {
                    //  await _signInManager.SignInAsync(user, isPersistent: false);
                        _logger.Information($"User Created Successfully!,User Name:{user.UserName},Email:{user.Email}");
                         return Ok("User Created Successfully!");
              
                }

                foreach (var error in result.Errors)
                {
                     // ModelState.AddModelError("", error.Description);
                     _logger.Error(error.Description);
                }

               // ModelState.AddModelError(string.Empty, "Invalid Login Attempt");

    
           // }

            return BadRequest();


        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User userModel)
        {

            // var result = await _signInManager.PasswordSignInAsync(userModel.Email, userModel.Password, user.RememberMe, false);

            var result = await _signInManager.PasswordSignInAsync(userModel.UserName, userModel.Password, false, false);

                 if (result.Succeeded)
                {

                _logger.Information($"Login Successfully! user name ");
                    return Ok("Login Successfully !");

                }
               return BadRequest();
        }

    }
}
