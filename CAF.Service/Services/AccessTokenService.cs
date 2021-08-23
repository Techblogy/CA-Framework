using CAF.Core.Constant;
using CAF.Core.Entities;
using CAF.Core.Enums;
using CAF.Core.Exception;
using CAF.Core.Repository;
using CAF.Core.Service;
using CAF.Core.Utilities;
using CAF.Core.ViewModel.AccessToken.Request;
using CAF.Core.ViewModel.AccessToken.Response;
using CAF.Service.Services.Base;

using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace CAF.Service.Services
{
    public class AccessTokenService : BaseService, IAccessTokenService
    {
        private readonly IAccessTokenRepository accessTokenRepository;
        private IAppSettings _appSettings;
        public AccessTokenService(IHttpContextAccessor httpContextAccessor, IAppSettings appSettings, IAccessTokenRepository accessTokenRepository, IServiceProvider serviceProvider) : base(httpContextAccessor, serviceProvider)
        {
            this.accessTokenRepository = accessTokenRepository;
            _appSettings = appSettings;
        }
        private string CreateToken(Dictionary<string, string> values, string secret, DateTime expireDate)
        {
            var claims = values.Select(x => new Claim(x.Key, x.Value)).ToArray();

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expireDate,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        private Dictionary<string, string> getTokenValues(User user)
        {
            Dictionary<string, string> tokenValues = new Dictionary<string, string>();
            tokenValues.Add(ClaimTypes.Name, user.Id.ToString());
            tokenValues.Add(ClaimTypes.Role, user.RoleType.ToString());
            //tokenValues.Add(UserRoleConstant.AgencyIdKey, user.AgencyId.Value.ToString());
            tokenValues.Add(UserRoleConstant.IsAccessToken, "true");

            return tokenValues;
        }
        public AccessTokenVM CreateAccessToken(CreateAccessTokenRequest request)
        {
            var userRepository = base.ResolveService<IUserRepository>();
            var user = userRepository.FindOrThrow(request.UserId);

            //if (user.UserType == UserType.QuitJob)
            //{
            //    throw new BadRequestException("İşten ayrılmış için access token eklenemez.");
            //}
            #region Agency
            //if (!user.AgencyId.HasValue)
            //{
            //    throw new BadRequestException("Acenteye bağlı olmayan kullanıcı için access token oluşturulamaz.");
            //}
            //var agency = base.RepositoryContainer.AgencyRepository.FindOrThrow(user.AgencyId.Value);
            #endregion

            Dictionary<string, string> tokenValues = getTokenValues(user);

            var accessTokenCode = CreateToken(tokenValues, _appSettings.Secret, request.ExpireDate);

            var accessToken = new AccessToken()
            {
                AccessTokenCode = accessTokenCode,
                //AgencyId = user.AgencyId.Value,
                ExpireDate = request.ExpireDate,
                UserId = request.UserId
            };
            var id = accessTokenRepository.Add(accessToken);
            var entity = accessTokenRepository.FindOrThrow(id);
            return new AccessTokenVM(entity);
        }

        public string Delete(Guid id)
        {
            var entity = accessTokenRepository.FindOrThrow(id);
            accessTokenRepository.Delete(entity);

            return $"Access Token silinmiştir. Artık kullanılamaz";
        }

        public List<AccessTokenGridVM> GetAccessTokenGrid(AccessTokenGridRequest request)
        {
            var entities = accessTokenRepository.All("User"/*, "Agency"*/).ToList();
            return entities.Select(x => new AccessTokenGridVM(x)).ToList();
        }

        public AccessTokenVM Regenerate(Guid id)
        {
            var entity = accessTokenRepository.FindOrThrow(id, "User", /*"Agency",*/ "UpdateUser", "CreateUser");
            Dictionary<string, string> tokenValues = getTokenValues(entity.User);

            entity.AccessTokenCode = CreateToken(tokenValues, _appSettings.Secret, entity.ExpireDate);
            accessTokenRepository.Update(entity);


            return new AccessTokenVM(entity);
        }

        public TokenValidateResponse TokenValidateControl(string token)
        {
            var userRepository = base.ResolveService<IUserRepository>();

            var accessToken = accessTokenRepository.Get(x => x.AccessTokenCode == token);
            if (accessToken == null) return new TokenValidateResponse(false, "Tanımsız Access Token");

            if (accessToken.DbState == DbState.Passive) return new TokenValidateResponse(false, "Access Token Pasiftir.");

            var user = userRepository.Get(x => x.Id == accessToken.UserId);
            if (user == null) return new TokenValidateResponse(false, "Access Token'a sahip kullanıcı bulunamadı");

            //if (user.UserType == UserType.QuitJob) return new TokenValidateResponse(false, $"Access Token tanımlanmış {user.FirstName} {user.LastName} kullanıcısı işten ayrılmıştır.");

            /*if (user.AgencyId != accessToken.AgencyId)
            {
                return new TokenValidateResponse(false, $"Access Token'a ait acenta numarası ile {user.FirstName} {user.LastName} kullanıcısına ait  Acenta numarası uyumsuz.");
            }

            var agency = base.RepositoryContainer.AgencyRepository.Get(x => x.Id == accessToken.AgencyId);
            if (agency == null)
                return new TokenValidateResponse(false, $"Access Token'a ait acenta numarası bulunamadı");
            */
            return new TokenValidateResponse(true, "Geçerli Access Token");
        }

        public string SetActive(Guid id, bool active)
        {
            var entity = accessTokenRepository.FindOrThrow(id);
            entity.DbState = active ? DbState.Active : DbState.Passive;

            accessTokenRepository.Update(entity);

            return $"Access token {(entity.DbState == DbState.Active ? "aktif" : "pasif")} edilmiştir.";
        }
    }
}
