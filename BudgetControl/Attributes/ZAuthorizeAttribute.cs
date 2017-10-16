using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BudgetControl.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class ZAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            //switch (Roles)
            //{

            //}
            var isAuthorized = base.AuthorizeCore(httpContext);

            return true;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {

            //base.HandleUnauthorizedRequest(filterContext);
            filterContext.Result = new RedirectToRouteResult(
                new RouteValueDictionary(
                    new
                    {
                        controller = "Users",
                        action = "Unauthorized"
                    })
                );
        }
    }
}