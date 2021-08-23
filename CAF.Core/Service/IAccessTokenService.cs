using CAF.Core.ViewModel.AccessToken.Request;
using CAF.Core.ViewModel.AccessToken.Response;

using System;
using System.Collections.Generic;
using System.Text;

namespace CAF.Core.Service
{
    public interface IAccessTokenService
    {
        AccessTokenVM CreateAccessToken(CreateAccessTokenRequest request);
        string Delete(Guid id);
        string SetActive(Guid id, bool active);
        AccessTokenVM Regenerate(Guid id);
        List<AccessTokenGridVM> GetAccessTokenGrid(AccessTokenGridRequest request);
        TokenValidateResponse TokenValidateControl(string token);
    }
}
