using CAF.Core.Exception;

using Microsoft.AspNetCore.Mvc.Filters;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace CAF.UI.WebApi.Filters
{
    public class DBTransactionFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                var executedContext = await next.Invoke();
                if (executedContext.Exception == null || executedContext.Exception is NotRollbackException)
                {
                    scope.Complete();
                }
            }
        }
    }
}
