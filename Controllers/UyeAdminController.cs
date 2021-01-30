using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TezProje.Models.Siniflar;
using PagedList;
using PagedList.Mvc;

namespace TezProje.Controllers
{
    [UyeKontrol]

    public class UyeAdminController : Controller
    {
        // GET: UyeAdmin
        TatilBlogEntities db = new TatilBlogEntities();
        [Authorize(Roles ="A")]
        public ActionResult Index()
        {
            var mail = (string)Session["UyeMail"];
            var id = db.Uye.Where(x => x.UyeMail == mail).Select(x => x.UyeID).FirstOrDefault();
            //var listele = db.Uye.Where(x => x.UyeMail == mail).ToList();
            var UyeAd = db.Uye.Where(x => x.UyeMail == mail).Select(x => x.UyeAd).FirstOrDefault();
            ViewBag.ad = UyeAd;

            ViewBag.mail = mail;

            var hakkimda= db.Uye.Where(x => x.UyeMail == mail).Select(x => x.UyeHakkimda).FirstOrDefault();
            ViewBag.hakkimda = hakkimda;

            var tarih = db.Uye.Where(x => x.UyeMail == mail).Select(x => x.UyeTarih).FirstOrDefault();
            ViewBag.tarih = tarih;

            var foto = db.Uye.Where(x => x.UyeMail == mail).Select(x => x.UyeFoto).FirstOrDefault();
            ViewBag.foto = foto;

            var toplamIcerik = db.KullaniciYazi.Where(x => x.Uye == id).Count();
            ViewBag.toplamIcerik = toplamIcerik;

            var sehir = db.Uye.Where(x => x.UyeMail == mail).Select(x => x.UyeSehir).FirstOrDefault();
            ViewBag.sehir = sehir;
            return View();
        }

        public ActionResult UyeYaziListele()
        {
            var listele = db.KullaniciYazi.ToList();
            return View(listele);
        }

        [HttpGet]
        public ActionResult UyeYeniYazi()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UyeYeniYazi(KullaniciYazi p)
        {
            if (p.YaziAciklama == null)
            {
                ViewBag.mesaj = "Lütfen boş bırakmayınız.";
                return View("UyeYeniYazi", p);
            }
            else
            {

                string metin = System.Text.RegularExpressions.Regex.Replace(p.YaziAciklama, @"<(.|\n)*?>", string.Empty);
                if (metin == "")
                {
                    ViewBag.mesaj = "Lütfen boş bırakmayınız.";
                    return View("UyeYeniYazi", p);
                }
                else if (metin.Length > 0 && metin.Length <= 80)
                {
                    ViewBag.mesaj = "Lütfen 80 karakterden uzun yazınız.";
                    return View("UyeYeniYazi", p);
                }
                else
                {
                    var mail = (string)Session["UyeMail"];
                    var id = db.Uye.Where(x => x.UyeMail == mail).Select(x => x.UyeID).FirstOrDefault();

                    p.Uye = id;
                    p.YaziTarih = DateTime.Now;
                    p.YaziDurum = true;
                    /*var ekle =*/
                    db.KullaniciYazi.Add(p);

                    db.SaveChanges();

                    return RedirectToAction("Index", "UyeAdmin");
                }
            }
            //p.YaziDurum = false;
            //db.KullaniciYazi.Add(p);
            //db.SaveChanges();
            //return RedirectToAction("Index","UyeAdmin");
        }

        public ActionResult UyeYaziSil(int id)
        {
            var bul = db.KullaniciYazi.Find(id);
            bul.YaziDurum = false;
            db.SaveChanges();
            TempData["yazisil"] = " ";
            return RedirectToAction("Index", "UyeAdmin");
        }


        public ActionResult UyeYaziGetir(int id)
        {
            var bul = db.KullaniciYazi.Find(id);

            return View("UyeYaziGetir",bul);
        }

        public ActionResult UyeYaziGuncelle(KullaniciYazi p)
        {
            var eski = db.KullaniciYazi.Find(p.YaziID);
            eski.YaziBaslik = p.YaziBaslik;
            eski.YaziAciklama = p.YaziAciklama;
            eski.YaziResim = p.YaziResim;
            //eski.YaziTarih = p.YaziTarih;
            //eski.Uye = p.Uye;
            db.SaveChanges();
            TempData["yaziguncelle"] = " ";
            return RedirectToAction("Index", "UyeAdmin");
        }

        public PartialViewResult uyeYaziPartial(int sayfa=1)
        {
            var mail = (string)Session["UyeMail"];
            var id = db.Uye.Where(x => x.UyeMail == mail).Select(x => x.UyeID).FirstOrDefault();

            var listele = db.KullaniciYazi.Where(x => x.Uye == id && x.YaziDurum == true).ToList().ToPagedList(sayfa,5);
            return PartialView(listele);
        }
        //public PartialViewResult uyeYeniYaziPartial()
        //{
            
        //    return PartialView();
        //}
        

        //public ActionResult UyeYaziEkle(KullaniciYazi p)
        //{
           


        //    if (p.YaziAciklama == null)
        //    {
        //        ViewBag.mesaj = "Lütfen boş bırakmayınız.";
        //        return View("Index", p);
        //    }
        //    else
        //    {

        //        string metin = System.Text.RegularExpressions.Regex.Replace(p.YaziAciklama, @"<(.|\n)*?>", string.Empty);
        //        if (metin == "")
        //        {
        //            ViewBag.mesaj = "Lütfen boş bırakmayınız.";
        //            return View("Index", p);
        //        }
        //        else if (metin.Length > 0 && metin.Length <= 80)
        //        {
        //            ViewBag.mesaj = "Lütfen 80 karakterden uzun yazınız.";
        //            return View("Index", p);
        //        }
        //        else
        //        {
        //            var mail = (string)Session["UyeMail"];
        //            var id = db.Uye.Where(x => x.UyeMail == mail).Select(x => x.UyeID).FirstOrDefault();

        //            p.Uye = id;
        //            p.YaziTarih = DateTime.Now;
        //            /*var ekle =*/ db.KullaniciYazi.Add(p);

        //            db.SaveChanges();

        //            return RedirectToAction("Index", "UyeAdmin");
        //        }
        //    }
        //}

        public PartialViewResult UyeAyarPartial()
        {
            var mail = (string)Session["UyeMail"];
            var id = db.Uye.Where(x => x.UyeMail == mail).Select(x => x.UyeID).FirstOrDefault();

            var bul = db.Uye.Find(id);
            
            return PartialView("UyeAyarPartial", bul);
        }

        public ActionResult UyeBilgiGuncelle(Uye p)
        {


            var eski = db.Uye.Find(p.UyeID);

            if (Request.Files.Count >= 1 && p.UyeFoto!=null)
            {
              
                string dosyaAdi = Path.GetFileName(Request.Files[0].FileName);
                string uzanti = Path.GetExtension(Request.Files[0].FileName);
                string yol = "~/uyeImages/" + dosyaAdi + uzanti;
                Request.Files[0].SaveAs(Server.MapPath(yol));
                p.UyeFoto = "/uyeImages/" + dosyaAdi + uzanti;

                eski.UyeFoto = p.UyeFoto;
            }

            eski.UyeAd = p.UyeAd;
            eski.UyeKullaniciAdi = p.UyeKullaniciAdi;
            eski.UyeMail = p.UyeMail;
            eski.UyeSifre = p.UyeSifre;
            eski.UyeSehir = p.UyeSehir;
            eski.UyeHakkimda = p.UyeHakkimda;

            db.SaveChanges();
            TempData["ayar"] = " ";
            return RedirectToAction("Index", "UyeAdmin");
        }
    }
}