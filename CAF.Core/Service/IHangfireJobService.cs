using CAF.Core.ViewModel.Common.Response;
using CAF.Core.ViewModel.User.Request;

using Hangfire;

using System;
using System.Collections.Generic;
using System.Text;

namespace CAF.Core.Service
{
    public interface IHangfireJobService
    {
        /// <summary>
        /// Mongo üzerindeki hata loglarını appconfig üzerinde belirtilen süreden fazla olanları temizler
        /// </summary>
        void ClearErrorLog();
        /// <summary>
        /// Mongo üzerindeki istek loglarını appconfig üzerinde belirtilen süreden fazla olanları temizler
        /// </summary>
        void ClearRequestLog();
    }
}
