using CAF.Core.Entities;
using CAF.Core.Enums;
using CAF.Core.Exception;
using CAF.Core.Repository;
using CAF.EFRepository.Repositories.Base;

using Microsoft.AspNetCore.Http;

using System;
using System.Collections.Generic;
using System.Text;

namespace CAF.EFRepository.Repositories
{
    public class ActionLogRepository : EFRepositoryBase<ActionLog, long>, IActionLogRepository
    {
        public ActionLogRepository(EFDatabaseContext dbContext, IHttpContextAccessor httpContextAccessor) : base(dbContext, httpContextAccessor)
        {
        }

        public void AddLog(ActionLogType type, string message, long userId)
        {
            Add(new ActionLog()
            {
                Date = CAF.Core.Helper.UtilitiesHelper.DateTimeNow(),
                EnumType = type,
                Message = message,
                UserId = userId
            });
        }

        public override ActionLog FindOrThrow(long id, string message = "")
        {
            var log = base.Find(id);

            if (log == null)
                throw new NotFoundExcepiton(string.IsNullOrEmpty(message) ? "Log bulunamadı." : message, Core.Enums.ErrorType.Warning);

            return log;
        }
    }
}
