using CAF.Core.Exception;

using Microsoft.AspNetCore.Mvc.Filters;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace CAF.UI.WebApi.Filters
{
    public class DBTransactionFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var transaction = (IDbTransaction)context.HttpContext.RequestServices.GetService(typeof(IDbTransaction));

            var connection = transaction.Connection;
            if (connection.State != ConnectionState.Open)
                throw new NotSupportedException("The provided connection was not open!");

            var executedContext = await next.Invoke();
            if (executedContext.Exception == null || executedContext.Exception is NotRollbackException)
            {
                transaction.Commit();
            }
            else
            {
                transaction.Rollback();
            }
        }
    }
}
