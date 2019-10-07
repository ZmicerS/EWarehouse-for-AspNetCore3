using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace EWarehouse.Web.Services.Filters
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            if (actionContext.ModelState.IsValid == false)
            {
                actionContext.Result = new BadRequestObjectResult(
                                           actionContext.ModelState.Values
                                          .SelectMany(e => e.Errors)
                                          .Select(e => e.ErrorMessage));
            }
        }
    }

}
