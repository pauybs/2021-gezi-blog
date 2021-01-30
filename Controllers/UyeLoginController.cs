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
    public class UyeLoginController : Controller
    {
        // GET: UyeLogin
        TatilBlogEntities db = new TatilBlogEntities();
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(Uye p)
        {
            if (new UyeLogin().IsLoginSuccess(p))
            {

                FormsAuthentication.SetAuthCookie(p.UyeMail, false);
                Session["UyeMail"] = p.UyeMail.ToString();
                return RedirectToAction("Index", "UyeAdmin");
            }
            else
            {
                ViewBag.mesaj = "Şifre veya mail hatalı. Lütfen tekrar deneyiniz.";
                return View();
            }
            //var bilgiler = db.Uye.FirstOrDefault(x => x.UyeMail == p.UyeMail && x.UyeSifre == p.UyeSifre);
            //if (bilgiler!=null)
            //{
            //    FormsAuthentication.SetAuthCookie(bilgiler.UyeMail,false);
            //    Session["UyeMail"] = bilgiler.UyeMail.ToString();
            //    return RedirectToAction("Index", "UyeAdmin");
            //}
            //else
            //{
            //    ViewBag.mesaj = "Şifre veya mail hatalı. Lütfen tekrar deneyiniz.";
            //    return View();
            //}

        }

        [HttpGet]
        public ActionResult UyeKayit()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UyeKayit(Uye p,string Sifre)
        {
            var mail = db.Uye.Where(x=>x.UyeMail.Contains(p.UyeMail)).FirstOrDefault();
            var name = db.Uye.Where(x=>x.UyeKullaniciAdi.Contains(p.UyeKullaniciAdi)).FirstOrDefault();
            if (p.UyeSifre!=Sifre)
            {
                ViewBag.mesaj = "Şifreniz eşleşmiyor!";
                return View(p);
            }
            else if (mail!=null)
            {
                ViewBag.mesaj = "Email sistemde kayıtlı!";
                return View(p);
            }
            else if (name!=null)
            {
                ViewBag.mesaj = "Kullanıcı adı kullanımda! Lütfen başka bir kullanıcı adı seçiniz.";
                return View(p);
            }
            else
            {

                p.UyeDurum = true;
                p.Yetki = "A";
                p.UyeTarih = DateTime.Now;
                db.Uye.Add(p);
                db.SaveChanges();
                TempData["ok"] = "Üyeliğiniz başarıyla oluşturulmuştur";
                return RedirectToAction("Index","UyeLogin");
                
            }
            
        }

        public PartialViewResult SifremiUnuttum()
        {
            return PartialView();
        }

        public ActionResult SifreHatirlat(Uye p)
        {
            var bilgiler = db.Uye.FirstOrDefault(x => x.UyeMail == p.UyeMail);

            if (bilgiler != null)
            {
                var sifre = db.Uye.Where(x => x.UyeMail == p.UyeMail).Select(x => x.UyeSifre).FirstOrDefault();
                MailMessage mailim = new MailMessage();
                mailim.To.Add(p.UyeMail);
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
                    TempData["uyesifre"] = "İsteğiniz alınmıştır. Lüten mail adresinizi kontrol ediniz.";
                }
                catch (Exception ex)
                {

                    TempData["uyesifre"] = "Mesaj gönderilemedi. " + ex.Message;
                }
            }
            else
            {
                TempData["uyesifre"] = "Mail bulunamadı!";
            }
            return RedirectToAction("Index", "UyeLogin");
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Index","Default");
        }
    }
}