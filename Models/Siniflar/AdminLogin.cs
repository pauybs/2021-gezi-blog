using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TezProje.Models.Siniflar
{
    public class AdminLogin
    {
        TatilBlogEntities db = new TatilBlogEntities();

        public AdminLogin()
        {
            db = new TatilBlogEntities();
        }

        public bool IsLoginSuccess(Admin p)
        {
            var bilgiler = db.Admin.FirstOrDefault(x => x.Mail == p.Mail && x.Sifre == p.Sifre);
            if (bilgiler != null)
            {
                db.Entry(bilgiler).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                HttpContext.Current.Session.Add("Admin", bilgiler.Mail.ToString());
                return true;
            }
            return false;
        }
    }
}