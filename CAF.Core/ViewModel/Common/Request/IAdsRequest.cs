using System;
using System.Collections.Generic;
using System.Text;

namespace CAF.Core.ViewModel.Common.Request
{
    public interface IAdsRequest
    {
        public string Ads_ref { get; set; }
        public string Ads_utm_source { get; set; }
        public string Ads_utm_medium { get; set; }
        public string Ads_utm_campaign { get; set; }
    }
}
