using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TezProje.Models.Siniflar;

namespace TezProje.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        TatilBlogEntities db = new TatilBlogEntities();
        public ActionResult Index()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult Index(Admin p)
        {
            if (new AdminLogin().IsLoginSuccess(p))
            {
                //FormsAuthentication.SetAuthCookie(bilgiler.Mail, p.BeniHatirla);
                //Session["UyeMail"] = bilgiler.Mail.ToString();
                
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                return View();
            }

            //var bilgiler = db.Admin.FirstOrDefault(x => x.Mail == p.Mail && x.Sifre == p.Sifre);
            //if (bilgiler != null)
            //{
            //    FormsAuthentication.SetAuthCookie(bilgiler.Mail, p.BeniHatirla);
            //    Session["UyeMail"] = bilgiler.Mail.ToString();
            //    return RedirectToAction("Index", "Admin");
            //}
            //else
            //{
            //    return View();
            //}
        }

        public PartialViewResult SifremiUnuttum()
        {
            return PartialView();
        }
        

        public ActionResult SifreHatirlat(Admin p)
        {
            var bilgiler = db.Admin.FirstOrDefault(x => x.Mail == p.Mail);

            if (bilgiler != null)
            {
                var sifre = db.Admin.Where(x => x.Mail == p.Mail).Select(x => x.Sifre).FirstOrDefault();
                MailMessage mailim = new MailMessage();
                mailim.To.Add(p.Mail);
                mailim.From = new MailAddress("hakanunlusoy41@gmail.com");
                mailim.Subject = "Şifreniz aşağıdadııır.";
                mailim.Body = "Şifreniz : " + sifre;
                mailim.IsBodyHtml = true;


                SmtpClient smtp = new SmtpClient();
                smtp.Credentials = new NetworkCredential("hakanunlusoy41@gmail.com", "36424690756Aa");
                smtp.Port = 587;
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;

                try
                {
                    smtp.Send(mailim);
                    TempData["Message"] = "İsteğiniz alınmıştır mail adresini kontrol et";
                }
                catch (Exception ex)
                {

                    TempData["Message"] = "mesaj gönderilemedi " + ex.Message;
                }
            }
            else
            {
                TempData["Message"] = "Mail Bulunamadı";
            }
            return RedirectToAction("Index","Login");
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Index", "Login");
        }


    }
}