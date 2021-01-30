using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TezProje.Models.Siniflar
{
    public class AdminKontrol: ActionFilterAttribute, IActionFilter
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                if (!string.IsNullOrEmpty(HttpContext.Current.Session["Admin"].ToString()))
                {
                    base.OnActionExecuting(filterContext);
                }
                else
                {
                    HttpContext.Current.Response.Redirect("/Login/Index");
                }
            }
            catch (Exception)
            {
                HttpContext.Current.Response.Redirect("/Login/Index");
            }

        }
    }
}