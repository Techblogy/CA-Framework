using CAF.Core.Entities;
using CAF.Core.Repository.Base;

using System;
using System.Collections.Generic;
using System.Text;

namespace CAF.Core.Repository
{
    public interface ISettingRepository : IRepository<Setting, Guid>
    {
        Setting GetByKey(string key);
        List<Setting> GestByGroup(string groupkey);
        void SaveByKey(string key, string value);
    }
}
