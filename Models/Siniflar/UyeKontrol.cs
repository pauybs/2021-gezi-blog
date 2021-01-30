using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TezProje.Models.Siniflar
{
    public class UyeKontrol : ActionFilterAttribute, IActionFilter
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                if (!string.IsNullOrEmpty(HttpContext.Current.Session["UyeMail"].ToString()))
                {
                    base.OnActionExecuting(filterContext);
                }
                else
                {
                    HttpContext.Current.Response.Redirect("/UyeLogin/Index");
                }
            }
            catch (Exception)
            {
                HttpContext.Current.Response.Redirect("/UyeLogin/Index");
            }

        }
    }
}