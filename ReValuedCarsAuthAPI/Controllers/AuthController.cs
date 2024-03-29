﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReValuedCarsAuthAPI.Models;
using ReValuedCarsAuthAPI.Services;


namespace ReValuedCarsAuthAPI.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IAuthManager iauthManager;

        public AuthController(IAuthManager iManager)
        {
            this.iauthManager = iManager;
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<dynamic>> Register([FromBody]User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await iauthManager.AddUserAsync(user);
            return Created("", result);
        }

        [HttpPost("token")]
        public ActionResult<JClient> Token([FromBody] LoginModel login)
        {
            var token = iauthManager.AuthUsers(login);
            if (string.IsNullOrEmpty(token.Token))
            {
                return Unauthorized("Invalid userid and password");
            }
            return Ok(token);
        }
    }
}
