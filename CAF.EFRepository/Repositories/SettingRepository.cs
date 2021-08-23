using CAF.Core.Entities;
using CAF.Core.Exception;
using CAF.Core.Repository;
using CAF.EFRepository.Repositories.Base;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAF.EFRepository.Repositories
{
    public class SettingRepository : EFRepositoryInMemory<Setting>, ISettingRepository
    {
        public SettingRepository(EFDatabaseContext dbContext, IHttpContextAccessor httpContextAccessor, IMemoryCache memCache) : base(dbContext, httpContextAccessor, memCache)
        {
        }

        public List<Setting> GestByGroup(string groupkey)
        {
            return Where(x => x.GroupKey == groupkey).ToList();
        }

        public Setting GetByKey(string key)
        {
            return Get(x => x.Key == key)
                    ?? throw new BadRequestException($"{key} parametresi bulunamadı");
        }

        public void SaveByKey(string key, string value)
        {
            var setting = GetByKey(key);
            setting.Value = value;
            Update(setting);
        }
    }
}
