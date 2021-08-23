using System;
using System.Collections.Generic;
using System.Text;

namespace CAF.Core.Service
{
    public interface IMemoryCacheService
    {
        T GetObject<T>(string key);
        void SetObject<T>(string key,T data);
        void DeleteObject(string key);
    }
}
