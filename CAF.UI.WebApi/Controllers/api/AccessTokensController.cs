using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using CAF.Core.Service;
using CAF.Core.Utilities;
using CAF.Core.ViewModel.AccessToken.Request;
using CAF.Core.ViewModel.AccessToken.Response;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace CAF.UI.WebApi.Controllers.api
{
    [Route(route)]
    [ApiController]
    [Authorize(Roles = Core.Constant.UserRoleConstant.Admin)] //Access Token tanımlamak ve diğer işlemleri sadece yönetici yapabilir
    public class AccessTokensController : BaseController
    {

        private readonly IAccessTokenService _accessTokenService;
        public AccessTokensController(IAccessTokenService accessTokenService, IConfiguration configuration, IAppSettings appSettings) : base(configuration, appSettings)
        {
            _accessTokenService = accessTokenService;
        }

        [HttpPost]
        public virtual ActionResult<AccessTokenVM> CreateAccessToken([FromBody] CreateAccessTokenRequest request)
        {
            return _accessTokenService.CreateAccessToken(request);
        }

        [HttpPost]
        [Route("{id:guid}")]
        public virtual ActionResult<string> Delete([FromRoute] Guid id)
        {
            return _accessTokenService.Delete(id);
        }

        [HttpPost]
        [Route("{id:guid}")]
        public virtual ActionResult<AccessTokenVM> Regenerate([FromRoute] Guid id)
        {
            return _accessTokenService.Regenerate(id);
        }
        [HttpPost]
        public virtual ActionResult<List<AccessTokenGridVM>> GetAccessTokenGrid([FromBody] AccessTokenGridRequest request)
        {
            return _accessTokenService.GetAccessTokenGrid(request);
        }

        [HttpPost]
        [Route("{id:guid}")]
        public virtual ActionResult<string> SetActive([FromRoute] Guid id, [FromQuery] bool active)
        {
            return _accessTokenService.SetActive(id, active);
        }

    }
}
