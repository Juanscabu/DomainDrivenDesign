﻿using EcommerceProject.Application.DTO;
using EcommerceProject.Application.Interface.Features;
using EcommerceProject.Service.WebApi.Helpers;
using EcommerceProject.Transversal.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EcommerceProject.Service.WebApi.Controllers.v2
{
    [Authorize]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("2.0")]
    public class UsersController : Controller
    {
        private readonly IUsersApplication _userApplication;
        private readonly AppSettings _appSettings;

        public UsersController(IUsersApplication userApplication, IOptions<AppSettings> appSettings)
        {
            _userApplication = userApplication;
            _appSettings = appSettings.Value;
        }

        [AllowAnonymous]
        [HttpPost("Authenticate")]
        public IActionResult Authenticate([FromBody] UserDto authDto)
        {
            var response = _userApplication.Authenticate(authDto.UserName, authDto.Password);
            if (response.IsSuccess)
            {
                if (response.Data != null)
                {
                    response.Data.Token = BuildToken(response);
                    return Ok(response);
                }
                else
                {
                    return NotFound(response.Message);
                }
            }
            return BadRequest(response);
        }

        private string BuildToken(Response<UserDto> userDto)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, userDto.Data.UserId.ToString())
                    }),
                Expires = DateTime.UtcNow.AddMinutes(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _appSettings.Issuer,
                Audience = _appSettings.Audience,
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            return tokenString;
        }

    }
}
