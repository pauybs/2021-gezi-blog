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
    [AdminKontrol]
    public class AdminController : Controller
    {
        // GET: Admin
        TatilBlogEntities db = new TatilBlogEntities();

        public ActionResult Index()
        {
            var toplamIcerik = db.Blog.Count();
            ViewBag.toplamIcerik = toplamIcerik;

            var blogYorum = db.Yorumlar.Count();
            ViewBag.toplamYorum = blogYorum;

            var uyeSayi = db.Uye.Count();
            ViewBag.uye = uyeSayi;

            var enCokYazi = db.KullaniciYazi.OrderBy(x => x.Uye).GroupBy(x => x.Uye).Select(x => new { Toplam = x.Key }).Select(x => x.Toplam).FirstOrDefault();
            var uyeBul = db.Uye.Where(x => x.UyeID == enCokYazi).Select(x => x.UyeKullaniciAdi).FirstOrDefault();
            ViewBag.enCokYazi = uyeBul;

            var toplamUyeIcerik = db.KullaniciYazi.Count();
            ViewBag.kullaniciYazi = toplamUyeIcerik;

            var kulliciYorum = db.KullaniciYorum.Count();
            ViewBag.kullaniciYorum = kulliciYorum;

            //var sonYazilar = (from x in db.Blog
            //                  select new
            //                  {
            //                      x.Baslik,
            //                      x.BlogID,
            //                      x.Admin.KullaniciAdi,
            //                      x.Tarih
            //                  }).ToList();

            var sonYazilar = db.Blog.OrderByDescending(x => x.BlogID).Take(6).ToList();
            var yorumlar = db.Yorumlar.OrderByDescending(x => x.YorumID).Take(6).ToList();
            var kullaniciyorum = db.KullaniciYorum.OrderByDescending(x => x.KullaniciYorumID).Take(6).ToList();
            var kullaniciyazi = db.KullaniciYazi.OrderByDescending(x => x.YaziID).Take(6).ToList();
            var mail = (string)Session["Admin"];
            TempData["Admin"] = db.Admin.Where(x => x.Mail == mail).Select(x => x.KullaniciAdi).FirstOrDefault();
            ViewModel mv = new ViewModel();

            mv.blog = sonYazilar;
            mv.yorum = yorumlar;
            mv.kullaniciYorum = kullaniciyorum;
            mv.kullaniciYazi = kullaniciyazi;

           

            return View(mv);
        }

        //Blog Alanı
        public ActionResult BlogYazi()
        {
            
            var listele = db.Blog.Where(x=>x.Durum==true).ToList();
            return View(listele);
        }

        [HttpGet]
        public ActionResult YeniYazi()
        {
            var mail = (string)Session["Admin"];
            TempData["ID"] = db.Admin.Where(x => x.Mail == mail).Select(x => x.AdminID).FirstOrDefault();
            return View();
        }

        [HttpPost]
        public ActionResult YeniYazi(Blog p)
        {
            if (p.Aciklama == null)
            {
                ViewBag.mesaj = "Lütfen boş bırakmayınız.";
                return View("YeniYazi", p);
            }
            else
            {

                string metin = System.Text.RegularExpressions.Regex.Replace(p.Aciklama, @"<(.|\n)*?>", string.Empty);
                if (metin == "")
                {
                    ViewBag.mesaj = "Lütfen boş bırakmayınız.";
                    return View("YeniYazi", p);
                }
                else if (metin.Length > 0 && metin.Length <= 200)
                {
                    ViewBag.mesaj = "Lütfen 200 karakterden uzun yazınız.";
                    return View("YeniYazi", p);
                }
                else
                {
                    p.Durum = true;
                    db.Blog.Add(p);
                    db.SaveChanges();
                    TempData["a"] = " oldu";
                    return RedirectToAction("BlogYazi", "Admin");
                }
            }

        }

        public ActionResult BlogSil(int id)
        {
            var sil = db.Blog.Find(id);
            sil.Durum = false;
            db.SaveChanges();
            return RedirectToAction("BlogYazi", "Admin");
        }

        public ActionResult BlogSilAdmin(int id)
        {
            var sil = db.Blog.Find(id);
            db.Blog.Remove(sil);
            db.SaveChanges();
            return RedirectToAction("Index", "Admin");
        }

        public ActionResult BlogGetir(int id)
        {
            var getir = db.Blog.Find(id);
            return View("BlogGetir", getir);
        }

        public ActionResult BlogGuncelle(Blog p)
        {

            if (p.Aciklama == null)
            {
                ViewBag.mesaj = "Lütfen boş bırakmayınız.";
                return View("YeniYazi", p);
            }
            else
            {

                string metin = System.Text.RegularExpressions.Regex.Replace(p.Aciklama, @"<(.|\n)*?>", string.Empty);
                if (metin == "")
                {
                    ViewBag.mesaj = "Lütfen boş bırakmayınız.";
                    return View("BlogGetir", p);
                }
                else if (metin.Length > 0 && metin.Length <= 200)
                {
                    ViewBag.mesaj = "Lütfen 200 karakterden uzun yazınız.";
                    return View("BlogGetir", p);
                }
                else
                {
                    var yn = db.Blog.Find(p.BlogID);
                    yn.Baslik = p.Baslik;
                    yn.Aciklama = p.Aciklama;
                    yn.BlogResim = p.BlogResim;
                    yn.Tarih = p.Tarih;

                    yn.TitleTag = p.TitleTag;
                    yn.DescriptonTag = p.DescriptonTag;
                    yn.ResimAltTag = p.ResimAltTag;
                    yn.YazarID = p.YazarID;

                    db.SaveChanges();
                    TempData["guncelle"] = " ";
                    return RedirectToAction("BlogYazi", "Admin");
                }
            }

            
        }
        //Blog Alanı

        public ActionResult HakkimdaListele()
        {
            var a = db.Hakkimda.ToList();
            return View(a);
        }
        public ActionResult HakkimizdaGetir(int id)
        {
            var getir = db.Hakkimda.Find(id);
            return View("HakkimizdaGetir", getir);
        }

        public PartialViewResult HakkimdaPartial()
        {
            return PartialView(db.Hakkimda.ToList());
        }

        public ActionResult HakkimdaGuncelle(Hakkimda p)
        {

            if (p.Aciklama == null)
            {
                ViewBag.mesaj = "Lütfen boş bırakmayınız.";
                return View("HakkimizdaGetir", p);
            }
            else
            {

                string metin = System.Text.RegularExpressions.Regex.Replace(p.Aciklama, @"<(.|\n)*?>", string.Empty);
                if (metin == "")
                {
                    ViewBag.mesaj = "Lütfen boş bırakmayınız.";
                    return View("HakkimizdaGetir", p);
                }
                else if (metin.Length > 0 && metin.Length <= 200)
                {
                    ViewBag.mesaj = "Lütfen 200 karakterden uzun yazınız.";
                    return View("HakkimizdaGetir", p);
                }
                else
                {
                    var yn = db.Hakkimda.Find(p.HakkimizdaID);
                    yn.FotoUrl = p.FotoUrl;
                    yn.Aciklama = p.Aciklama;
                    yn.YazarID = p.YazarID;
                    db.SaveChanges();
                    TempData["bizkimiz"] = " ";
                    return RedirectToAction("HakkimdaListele", "Admin");
                }
            }
            
        }

        public ActionResult Mesajlar(string p,int sayfa=1)
        {
            var listele = db.İletisim.Where(x => x.İletisimID != 6 && x.İletisimID != 7).ToList();
            ViewBag.mesajSayi = db.İletisim.Where(x => x.İletisimID != 6 && x.İletisimID != 7).Count();
            var bul = from x in db.İletisim where x.İletisimID != 6 && x.İletisimID != 7 select x;
            if (!string.IsNullOrEmpty(p))
            {
                bul = bul.Where(x => x.Mail.Contains(p));
                return View(bul.ToList().ToPagedList(sayfa,5));
                //bul = bul.Where(x=> x.Mail.Contains(p));

            }
            else
            {
                return View(listele.ToPagedList(sayfa,5));

            }



        }
    
        public ActionResult MesajDetay(int? id)
        {
            if (id==null)
            {
                return RedirectToAction("Mesajlar", "Admin");
            }
            var listele = db.İletisim.Where(x => x.İletisimID != 6 && x.İletisimID != 7).ToList();
            ViewBag.mesajSayi = db.İletisim.Where(x => x.İletisimID != 6 && x.İletisimID != 7).Count();
            var bul = db.İletisim.Where(x => x.İletisimID == id).ToList();
            return View(bul);
        }

    

        [HttpGet]
        public ActionResult Iletisim()
        {
            
            var a = db.Harita.FirstOrDefault(x=>x.HaritaID==1);
            if (a.Frame==null)
            {
                ViewBag.mesaj = "Harita alanınız boş";
            }
            return View(a);
        }

        [HttpPost]
        public ActionResult Iletisim(Harita p)
        {
            var eski = db.Harita.Find(p.HaritaID);
            eski.Frame = p.Frame;
            db.SaveChanges();
            TempData["harita"] = " ";
            return RedirectToAction("Iletisim", "Admin");
        }

        public ActionResult Bilgilendirme()
        {
            return View();
        }

        public PartialViewResult BilgilendirmeAdmin()
        {
            var icerik = db.Bilgilendirme.ToList();
            return PartialView(icerik);
        }

        public PartialViewResult BilgilendirmeBaslikAdmin()
        {
            var baslik = db.Basliklar.Where(x => x.BaslikID == 1).ToList();
            return PartialView(baslik);
        }

        public ActionResult BilgiGetir(int id)
        {
            var a = db.Bilgilendirme.Find(id);
            return View("BilgiGetir", a);
        }
        public ActionResult BilgiGuncelle(Bilgilendirme p)
        {
            var eski = db.Bilgilendirme.Find(p.BilgiID);
            eski.Baslik = p.Baslik;
            eski.Aciklama = p.Aciklama;
            eski.FotoUrl = p.FotoUrl;
            db.SaveChanges();
            TempData["bilgiicerik"] = " ";
            return RedirectToAction("Bilgilendirme", "Admin");
        }

        public ActionResult BaslikGetir(int id)
        {
            var a = db.Basliklar.Find(id);
            return View("BaslikGetir", a);
        }

        public ActionResult BaslikGuncelle(Basliklar p)
        {
            var eski = db.Basliklar.Find(p.BaslikID);
            eski.AnaBaslik = p.AnaBaslik;
            eski.AnaAciklama = p.AnaAciklama;
            db.SaveChanges();
            TempData["baslik"] = " ";
            return RedirectToAction("Bilgilendirme", "Admin");
        }


        public ActionResult UyeSonYazi()
        {
            return View(db.Basliklar.Where(x => x.BaslikID == 2).ToList());
        }


        public ActionResult UyeBaslikGetir(int id)
        {
            var bul = db.Basliklar.Find(id);
            return View("UyeBaslikGetir", bul);
        }

        public ActionResult UyeSonYaziGuncelle(Basliklar p)
        {
            var eski = db.Basliklar.Find(p.BaslikID);
            eski.AnaBaslik = p.AnaBaslik;
            eski.AnaAciklama = p.AnaAciklama;
            db.SaveChanges();
            TempData["uyebaslik"] = " ";
            return RedirectToAction("UyeSonYazi", "Admin");
        }

        public ActionResult UyeListesi()
        {
            var uyeler = db.Uye.ToList();
            return View(uyeler);
        }
        public ActionResult UyeDetay(int id)
        {
            var uyeler = db.KullaniciYazi.Where(x => x.Uye == id).ToList(); ;
            ViewBag.uye = db.Uye.Where(x => x.UyeID == id).Select(x => x.UyeKullaniciAdi).FirstOrDefault();
            return View(uyeler);
        }

        public ActionResult UyePaylasimlar()
        {
            var listele = db.KullaniciYazi.ToList();
            return View(listele);
        }

        public ActionResult UyePaylasimOnayla(int id)
        {
            var bul = db.KullaniciYazi.Find(id);
            if (bul.YaziDurum == true)
            {
                bul.YaziDurum = false;
            }
            else
            {
                bul.YaziDurum = true;
            }

            db.SaveChanges();
            return RedirectToAction("UyePaylasimlar", "Admin");
        }

        public ActionResult UyePaylasimOnaylaAdmin(int id)
        {
            var bul = db.KullaniciYazi.Find(id);
            if (bul.YaziDurum == true)
            {
                bul.YaziDurum = false;
            }
            else
            {
                bul.YaziDurum = true;
            }

            db.SaveChanges();
            return RedirectToAction("Index", "Admin");
        }

        public ActionResult UyeBanla(int id)
        {
            var bul = db.Uye.Find(id);
            if (bul.UyeDurum == true)
            {
                bul.UyeDurum = false;
            }
            else
            {
                bul.UyeDurum = true;
            }

            db.SaveChanges();
            return RedirectToAction("UyeListesi", "Admin");
        }



        public ActionResult BanliUye()
        {
            var banli = db.Uye.Where(x => x.UyeDurum == false).ToList();
            return View(banli);
        }

        public ActionResult BlogYorumlar()
        {
            var listele = db.Yorumlar.ToList();
            return View(listele);
        }

        public ActionResult BlogYorumOnayla(int id)
        {
            var bul = db.Yorumlar.Find(id);
            if (bul.Durum == true)
            {
                bul.Durum = false;
            }
            else
            {
                bul.Durum = true;
            }

            db.SaveChanges();
            return RedirectToAction("BlogYorumlar", "Admin");
        }
        public ActionResult BlogYorumOnaylaIndex(int id)
        {
            var bul = db.Yorumlar.Find(id);
            if (bul.Durum == true)
            {
                bul.Durum = false;
            }
            else
            {
                bul.Durum = true;
            }

            db.SaveChanges();
            return RedirectToAction("Index", "Admin");
        }

        public ActionResult KullaniciYorumlar()
        {
            var listele = db.KullaniciYorum.ToList();
            return View(listele);
        }

        public ActionResult KullaniciYorumOnayla(int id)
        {
            var bul = db.KullaniciYorum.Find(id);
            if (bul.Durum == true)
            {
                bul.Durum = false;
            }
            else
            {
                bul.Durum = true;
            }

            db.SaveChanges();
            return RedirectToAction("KullaniciYorumlar", "Admin");
        }

        public ActionResult KullaniciYorumOnaylaAdmin(int id)
        {
            var bul = db.KullaniciYorum.Find(id);
            if (bul.Durum == true)
            {
                bul.Durum = false;
            }
            else
            {
                bul.Durum = true;
            }

            db.SaveChanges();
            return RedirectToAction("Index", "Admin");
        }

        public ActionResult Galeri()
        {
            var listele = db.Harita.Where(x => x.HaritaID >= 2).ToList();
            return View(listele);
        }

        public ActionResult GaleriGetir(int id)
        {
            var bul = db.Harita.Find(id);

            return View("GaleriGetir", bul);
        }

        public ActionResult GaleriGuncelle(Harita p)
        {
            var eski = db.Harita.Find(p.HaritaID);

            if (Request.Files.Count >= 1 && p.Frame != null)
            {
                string dosyaAdi = Path.GetFileName(Request.Files[0].FileName);
                string uzanti = Path.GetExtension(Request.Files[0].FileName);
                string yol = "~/galeri/" + dosyaAdi + uzanti;
                Request.Files[0].SaveAs(Server.MapPath(yol));
                p.Frame = "/galeri/" + dosyaAdi + uzanti;

                eski.Frame = p.Frame;
                TempData["galeri"] = " ";
            }


            db.SaveChanges();

            return RedirectToAction("Galeri", "Admin");
        }

        public ActionResult SideBarHakkimda()
        {
            var goster = db.İletisim.Where(x => x.İletisimID == 6).ToList();
            return View(goster);
        }

        public ActionResult SideBarGetir(int id)
        {
            var bul = db.İletisim.Find(id);
            return View("SideBarGetir", bul);
        }

        public ActionResult SideBarGuncelle(İletisim p)
        {
            var eski = db.İletisim.Find(p.İletisimID);
            eski.AdSoyad = p.AdSoyad;
            eski.Konu = p.Konu;

            if (Request.Files.Count >= 1 && p.Mesaj != null)
            {
                string dosyaAdi = Path.GetFileName(Request.Files[0].FileName);
                string uzanti = Path.GetExtension(Request.Files[0].FileName);
                string yol = "~/profilfoto/" + dosyaAdi + uzanti;
                Request.Files[0].SaveAs(Server.MapPath(yol));
                p.Mesaj = "/profilfoto/" + dosyaAdi + uzanti;

                eski.Mesaj = p.Mesaj;
            }
            TempData["sidebar"] = " ";
            db.SaveChanges();

            return RedirectToAction("SideBarHakkimda", "Admin");
        }

        public ActionResult SosyalMedya()
        {
            var bul = db.İletisim.Where(x => x.İletisimID == 7).FirstOrDefault();
            return View("SosyalMedya", bul);
        }

        public ActionResult SosyalMedyaGuncelle(İletisim p)
        {
            var eski = db.İletisim.Find(p.İletisimID);
            eski.AdSoyad = p.AdSoyad;
            eski.Mail = p.Mail;
            eski.Konu = p.Konu;
            db.SaveChanges();
            return RedirectToAction("SosyalMedya", "Admin");
        }




    }
}