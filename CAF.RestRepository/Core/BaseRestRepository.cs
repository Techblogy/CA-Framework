using System;
using System.Collections.Generic;

using CAF.Core.Helper;
using CAF.Core.Repository.Base;
using RestSharp;
using RestSharp.Authenticators;

using Microsoft.Extensions.DependencyInjection;
using CAF.Core.ViewModel.Common.Response;
using System.Linq;

namespace CAF.RestRepository.Core
{
    public abstract class BaseRestRepository : IRestRepository
    {
        private readonly string serviceAddress;
        private readonly RestClient client;
        //private IHttpContextAccessor HttpContextAccessor { get; }
        private IServiceProvider ServiceProvider { get; }
        public BaseRestRepository(string serviceAddress/*, IHttpContextAccessor httpContextAccessor*/, IServiceProvider serviceProvider)
        {
            this.serviceAddress = serviceAddress;

            client = new RestClient(this.serviceAddress);
            client.AddDefaultHeader("Accept-Language", "tr-TR");
            //HttpContextAccessor = httpContextAccessor;
            ServiceProvider = serviceProvider;
        }
        protected void SetBasicAuthentication(string username, string password)
        {
            this.client.Authenticator = new HttpBasicAuthenticator(username, password);
        }
        protected void AddDefaultHeader(string name, string value)
        {
            this.client.AddDefaultHeader(name, value);
        }

        public T Get<T>(string url)
        {
            IRestResponse<T> response = GetWithFullResponse<T>(url);

            return response.Data;
        }
        public ResponseWithHeader<T> GetWithHeader<T>(string url, params string[] headerKeys)
        {
            RestRequest request = new RestRequest(url, Method.GET);
            // act
            IRestResponse<T> response = client.Execute<T>(request);
            var headers = new Dictionary<string, string>();
            if (headerKeys != null && headerKeys.Any())
                headers = response.Headers.Where(x => headerKeys.Contains(x.Name) && !string.IsNullOrEmpty(Convert.ToString(x.Value))).ToDictionary(k => k.Name, v => Convert.ToString(v.Value));
            return new ResponseWithHeader<T>()
            {
                Data = response.Data,
                Headers = headers
            };
        }
        private IRestResponse<T> getRaw<T>(string url)
        {
            RestRequest request = new RestRequest(new Uri(url), Method.GET);
            // act
            IRestResponse<T> response = client.Execute<T>(request);
            if ((int)response.StatusCode > 299 || (int)response.StatusCode < 200)
            {
                var detail = $@" 
                        Request Url : {url}
                        Http MEthod : Method.GET
                        Error Response : {response.Content}
                    ";

                ResolveRepository<ILogger>().Error("Indicata", response.StatusCode.ToString(), detail);
            }
            return response;
        }
        public T GetRaw<T>(string url)
        {
            return getRaw<T>(url).Data;
        }
        protected TRepository ResolveRepository<TRepository>()
        {
            return ServiceProvider.GetService<TRepository>();
            //return HttpContextAccessor.HttpContext.RequestServices.GetService<TRepository>();
        }

        public IRestResponse<T> GetWithFullResponse<T>(string url)
        {
            RestRequest request = new RestRequest(url, Method.GET);
            // act
            IRestResponse<T> response = client.Execute<T>(request);

            return response;
        }

        public string Get(string url)
        {
            RestRequest request = new RestRequest(url, Method.GET);
            
            IRestResponse response = client.Execute(request);
            
            return response.Content;
        }
    }
}
