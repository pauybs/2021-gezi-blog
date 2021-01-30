using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TezProje.Models.Siniflar
{
    public class UyeLogin
    {
        TatilBlogEntities db = new TatilBlogEntities();

        public UyeLogin()
        {
            db = new TatilBlogEntities();
        }

        public bool IsLoginSuccess(Uye p)
        {
            var bilgiler = db.Uye.FirstOrDefault(x => x.UyeMail == p.UyeMail && x.UyeSifre == p.UyeSifre);
            if (bilgiler != null)
            {
                db.Entry(bilgiler).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                HttpContext.Current.Session.Add("UyeMail", bilgiler.UyeMail.ToString());
                return true;
            }
            return false;
        }
    }
}