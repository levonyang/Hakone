using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Hakone.Web
{
    public class CatchModelStateErrorsActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var result = new StringBuilder();
            var allErrors = filterContext.Controller.ViewData.ModelState.Values.SelectMany(v => v.Errors);
            foreach (var error in allErrors)
            {
                result.AppendFormat("<p>{0}</p>", error.ErrorMessage);
            }

            if (result.ToString().Trim().Length > 0) filterContext.Controller.ViewBag.ErrorMsg = result.ToString();

            base.OnActionExecuted(filterContext);
        }
    }
}