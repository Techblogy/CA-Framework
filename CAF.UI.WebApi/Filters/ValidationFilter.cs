using CAF.Core.Exception;

using Microsoft.AspNetCore.Mvc.Filters;

using System;
using System.Collections.Generic;
using System.Linq;
namespace CAF.UI.WebApi.Filters
{
    public class ValidationFilter : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = new List<string>();
                if (Core.Utilities.AppStatic.IsDevelopment)
                {
                    var errorValues = context.ModelState.Values.SelectMany(x => x.Errors.Select(e => $"{e.ErrorMessage}()")).ToList();
                    foreach (var values in context.ModelState.Values)
                    {
                        var propertyName = Convert.ToString(Core.Helper.ReflectionHelper.GetPropValue(values, "Key"));
                        foreach (var item in values.Errors)
                        {
                            propertyName = string.IsNullOrEmpty(propertyName) ? "" : $" [{propertyName}] ";
                            errors.Add(item.ErrorMessage + propertyName);
                        }

                    }
                }
                else
                {
                    errors = context.ModelState.Values.SelectMany(x => x.Errors.Select(e => $"{e.ErrorMessage}()")).ToList();
                }

                var errorTitle = "Gerekli alanları doldurunuz";
                var error = string.Join(Environment.NewLine, errors);
                throw new BadRequestException(errorTitle, error, Core.Enums.ErrorType.Validation);
            }
            else
            {
                base.OnActionExecuting(context);
            }
        }
    }
}
