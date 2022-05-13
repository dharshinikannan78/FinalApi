using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RegistrationApllication.Data;
using RegistrationApllication.Modal;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace RegistrationApllication.Controllers
{
    [EnableCors("AllowOrigin")]
    [Route("api/[controller]")]
    [ApiController]

    public class UserController : ControllerBase
    {
        private const string SECRET_KEY = "DDFslkgdkgdlmlgkhlkghSDSDkdghjhgkhkglkasjdklajsfkljdsklgjsrjtoriupoeropterp";
        public static readonly SymmetricSecurityKey SIGNING_KEY = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SECRET_KEY));

        public readonly UserDbContext dataModel;
        public UserController(UserDbContext userData)
        {
            dataModel = userData;

        }

        [HttpPost("Login")]
        
        public IActionResult Login([FromBody] UserModelClass userObj)
        {
            
            if (userObj == null)
            {
                return BadRequest();
            }
            else
            {
                var user = dataModel.AdminLogin.Where(q =>
                q.Username == userObj.Username
                && q.Password == userObj.Password).FirstOrDefault();

                if (user != null)
                {
                    var credentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(SIGNING_KEY, SecurityAlgorithms.HmacSha256);
                    var header = new JwtHeader(credentials);
                    DateTime Exp = DateTime.UtcNow.AddDays(60);
                    int ts = (int)(Exp - new DateTime(1970, 1, 1)).TotalSeconds;
                    var payload = new JwtPayload()
            {
                {"sub", "testsubject" },
                {"Name", user.Username },
                {"email", user.Password },
                {"exp" , ts },
                {"iss" , "https://localhost:44310" },
                {"aud" , "https://localhost:44310" }

            };
                    var secToken = new JwtSecurityToken(header, payload);
                    var handler = new JwtSecurityTokenHandler();
                    var adminUserName = user.Username;
                    var adminUserPassword = user.Password;
                    var token = handler.WriteToken(secToken).ToString();
                    Console.WriteLine(token);
                    var finalToken = savetoDb(token , adminUserName, adminUserPassword);
                    return Ok(finalToken);
                    



                }
                else
                {
                    return NotFound(new
                    {
                        StatusCode = 404,
                        Message = "Unauthorized"
                    });
                }
                

            }


        }
        private TokenRequest savetoDb(string token , string username , string password)
        {
            var dataObj = new TokenRequest();
            {
                dataObj.Token = token;
                dataObj.AdminUserName = username;
                dataObj.AdminPassword = password;
            }
            dataModel.TokenDetails.Add(dataObj);
            dataModel.SaveChanges();
            return dataObj;
        } 


    }
}
