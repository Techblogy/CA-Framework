using CAF.Core.Entities;
using CAF.Core.Exception;
using CAF.Core.Extensions;
using CAF.Core.Repository;
using CAF.Core.Service;
using CAF.Core.ViewModel.Common.Response;
using CAF.Core.ViewModel.Setting.Dto;
using CAF.Service.Services.Base;

using ExtensionCore;

using Hangfire;

using Microsoft.AspNetCore.Http;

using System;
using System.Collections.Generic;
using System.Linq;

namespace CAF.Service.Services
{
    public class SettingService : BaseService, ISettingService
    {
        private readonly ISettingRepository settingRepository;
        public SettingService(ISettingRepository _settingRepository, IHttpContextAccessor httpContextAccessor, IServiceProvider serviceProvider) : base(httpContextAccessor, serviceProvider)
        {
            settingRepository = _settingRepository;
        }

        #region GlobalSetting

        public GlobalSettingDto GetGlobalSetting()
        {
            var settings = settingRepository.GestByGroup(Core.Constant.SettingConstant.Group_GlobalSettingKey);

            var dto = new GlobalSettingDto();
            return dto;
        }

        public NotifyMessageResponse<string> SetGlobalSetting(GlobalSettingDto request)
        {
            settingRepository.SaveByKey(
                        Core.Constant.SettingConstant.GlobalSetting.RefreshDataHour,
                        request.RefreshDataHour.ToString()
                    );


            return new NotifyMessageResponse<string>(Core.Constant.SettingConstant.Group_GlobalSettingKey, "Ayarlar güncellendi");
        }

        #endregion



        public void HangFireJonRegister()
        {
            var setting = this.GetGlobalSetting();
            var timezoneInfo = CAF.Core.Helper.UtilitiesHelper.GetTimeZoneInfo();

            #region RefreshDataHour
            //var format = Core.Constant.HangfireConstant.RefreshDataFormatName;

            //if (setting.RefreshDataHour > -1 && setting.RefreshDataHour < 24)
            //    RecurringJob.AddOrUpdate<IHangfireJobService>(format, hangfireSer => hangfireSer.ExampleMethod(), Cron.Daily(setting.RefreshDataHour.Value, 0), timezoneInfo);
            //else
            //    RecurringJob.RemoveIfExists(format);
            #endregion

            #region İstek ve Hata loglarını temizlenmesi
            RecurringJob.AddOrUpdate<IHangfireJobService>(Core.Constant.HangfireConstant.DailyRequestLogClearJob, service => service.ClearRequestLog(), Cron.Daily(5, 0), timezoneInfo);

            RecurringJob.AddOrUpdate<IHangfireJobService>(Core.Constant.HangfireConstant.DailyErrorLogClearJob, service => service.ClearErrorLog(), Cron.Daily(5, 0), timezoneInfo);
            #endregion
        }
    }
}
