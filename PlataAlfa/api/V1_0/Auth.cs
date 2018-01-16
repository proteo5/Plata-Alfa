using PlataAlfa.core;
using PlataAlfa.data.V1_0.Admin;
using System.IdentityModel.Tokens;
using System.Security.Claims;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace PlataAlfa.api.V1_0
{
    public class Auth : Entity
    {
        public Envelope<dynamic> Login(dynamic data, UsersDS usersDS)
        {
            string user = data.user;
            string password = data.password;
            var dataSet = usersDS.GetByUser(new { user = user.ToLower() });
            if (dataSet.Result == "notSuccess")
            {
                return new Envelope<dynamic>() { Result = "notSuccess", Message = "User o Password not found" };
            }
            else if (dataSet.Data.password != password)
            {
                return new Envelope<dynamic>() { Result = "notSuccess", Message = "User o Password not found" };
            }
            else
            {
                var plainTextSecurityKey = "abcdefghijklmnopqrstuvwyxz01234567980";
                var signingKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(plainTextSecurityKey));
                var signingCredentials = new SigningCredentials(signingKey,
                    SecurityAlgorithms.HmacSha256Signature);

                var x =  new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier, user),
                    new Claim(ClaimTypes.Name, dataSet.Data.name ),
                    new Claim(ClaimTypes.Surname, dataSet.Data.lastname ),
                    //new Claim("SecurityStamp",Guid.NewGuid().ToString())
                };

                var claimsIdentity = new ClaimsIdentity(x, "Custom");

                var securityTokenDescriptor = new SecurityTokenDescriptor()
                {
                    Audience = "http://localhost:61101",
                    Issuer = "http://localhost:61101",
                    Subject = claimsIdentity,
                    Expires = DateTime.Now.AddHours(12),
                    SigningCredentials = signingCredentials,
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var plainToken = tokenHandler.CreateToken(securityTokenDescriptor);
                var signedAndEncodedToken = tokenHandler.WriteToken(plainToken);


                return new Envelope<dynamic>() { Result = "ok", Data = signedAndEncodedToken };
            }

        }


    }
}
