using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using API.Services;
using API.Models;
using Domain;

namespace API.Controllers
{
    [ApiController]
    [Route("api/")]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// POST api/login
        /// </summary>
        /// <param name="req">AuthenticateRequest object containing email and password</param>
        /// <returns>A valid JWT</returns>
        [HttpPost("login")]
        public IActionResult Login(AuthenticateRequest req)
        {
            var response = _userService.Login(req);

            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });
            
            return Ok(response);
        }

        [HttpPost("users")]
        public IActionResult Register(User user)
        {
            try
            {
                var response = _userService.Register(user);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// GET api/auth
        /// </summary>
        /// <returns>The authorized user</returns>
        [Authorize]
        [HttpGet("auth")]
        public IActionResult Auth()
        {
            return Ok(HttpContext.Items["User"]);
        }

        /// <summary>
        /// GET api/users
        /// </summary>
        /// <returns>A list of all the users</returns>
        [Authorize]
        [HttpGet("users")]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            return Ok(users);
        }
        
        
    }
}