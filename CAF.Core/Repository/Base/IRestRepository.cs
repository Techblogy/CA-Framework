using CAF.Core.ViewModel.Common.Response;

using RestSharp;

using System;
using System.Collections.Generic;
using System.Text;

namespace CAF.Core.Repository.Base
{
    public interface IRestRepository
    {
        public T Get<T>(string url);
        public string Get(string url);
        public IRestResponse<T> GetWithFullResponse<T>(string url);
        public ResponseWithHeader<T> GetWithHeader<T>(string url, params string[] headerKeys);
        public T GetRaw<T>(string url);
    }
}
