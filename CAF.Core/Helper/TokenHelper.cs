using CAF.Core.Constant;
using CAF.Core.Enums;

using Microsoft.IdentityModel.Tokens;

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace CAF.Core.Helper
{
    public static class TokenHelper
    {
        public static string CreateToken(Dictionary<string, string> values, string secret, DateTime expires)
        {
            var claims = values.Select(x => new Claim(x.Key, x.Value)).ToArray();

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expires,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        public static string CreateTokenBySystemUser(long userId, string secret)
        {
            Dictionary<string, string> tokenValues = new Dictionary<string, string>();
            tokenValues.Add(ClaimTypes.Name, userId.ToString());
            tokenValues.Add(ClaimTypes.Role, UserRole.SystemAdmin.ToString());
            tokenValues.Add(UserRoleConstant.LoginId, Guid.NewGuid().ToString());

            return CreateToken(tokenValues, secret, DateTime.MaxValue);
        }

        //public static string GetKeyName(ClaimsPrincipal claimUser, string keyname)
        //{

        //}
    }
}
