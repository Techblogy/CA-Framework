
using CAF.Core.Service;
using CAF.Core.Utilities;
using CAF.Core.ViewModel.Common.Response;
using CAF.Core.ViewModel.Setting.Dto;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace CAF.UI.WebApi.Controllers.api
{
    [Route(route)]
    [ApiController]
    public class SettingsController : BaseController
    {
        private readonly ISettingService _settingService;

        public SettingsController(ISettingService settingService, IConfiguration configuration, IAppSettings appSettings) : base(configuration, appSettings)
        {
            _settingService = settingService;
        }

        #region Global Setting
        [HttpPost]
        public virtual ActionResult<GlobalSettingDto> GetGlobalSetting()
        {
            return _settingService.GetGlobalSetting();
        }

        [HttpPost]
        public virtual ActionResult<NotifyMessageResponse<string>> SetGlobalSetting(GlobalSettingDto request)
        {
            return _settingService.SetGlobalSetting(request);
        }
        #endregion

    }
}
