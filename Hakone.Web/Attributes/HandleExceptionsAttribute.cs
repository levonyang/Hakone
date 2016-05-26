using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using log4net;

namespace Hakone.Web.Attributes
{
    public class HandleExceptionsAttribute : HandleErrorAttribute
    {
        private readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public override void OnException(ExceptionContext filterContext)
        {
            var expception = filterContext.Exception;

            _log.Error(expception.ToString());
        }
    }
}