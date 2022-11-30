using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;


namespace WebApi.Jwt
{
    public class Auth:IJwtAuth
    {

        //private readonly string client_id = "admin";
        //private readonly string client_secret = "admin@123#";
        private readonly string key;


        private readonly List<UserCredential> lstMember = new List<UserCredential>()
        {
            new UserCredential{client_id="2609",client_secret="hmisUser@123#",grant_type="Normal"},
            new UserCredential {client_id="admin",client_secret="admin@@123#4",grant_type="supper"},
            new UserCredential{client_id="account",client_secret="admin@@123",grant_type="supper"}
        };

        public Auth(string key)
        {
            this.key = key;
        }
        public string Authentication(string username, string password)
        {

           // int count = lstMember.Count(item => item.client_id ==username && item.client_secret==password);


            //if (count==0)
            //{
            //    return null;
            //}



            DateTime TokenExpireTime = DateTime.Now.AddMinutes(60);

            //if ( username =="admin")
            //{
            //     TokenExpireTime = DateTime.Now.AddDays(4000);
            //}
            

            // 1. Create Security Token Handler
            var tokenHandler = new JwtSecurityTokenHandler();

            // 2. Create Private Key to Encrypted
            var tokenKey = Encoding.ASCII.GetBytes(key);

            

            //3. Create JETdescriptor
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(
                    new Claim[]
                    {
                        new Claim(ClaimTypes.Name, username)
                    }),
                
             // Expires = TokenExpireTime,
              SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };
            //4. Create Token
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // 5. Return Token from method
            return tokenHandler.WriteToken(token);
        }
    }
}
