using CAF.Core.ViewModel.Common.Response;
using CAF.Core.ViewModel.Setting.Dto;

namespace CAF.Core.Service
{
    public interface ISettingService
    {
        GlobalSettingDto GetGlobalSetting();
        NotifyMessageResponse<string> SetGlobalSetting(GlobalSettingDto request);

        /// <summary>
        /// Otomatik hangfire joblarının oluşturulması
        /// </summary>
        void HangFireJonRegister();
    }
}
