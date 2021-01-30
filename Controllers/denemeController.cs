using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TezProje.Models.Siniflar;
using PagedList;
using PagedList.Mvc;

namespace TezProje.Controllers
{
    public class denemeController : Controller
    {
        // GET: deneme

        TatilBlogEntities db = new TatilBlogEntities();
        [Route("uye-yazilari")]
        public ActionResult Index(int sayfa=1)
        {
            var listele = db.KullaniciYazi.Where(x=>x.Uye1.UyeDurum==true && x.YaziDurum==true).ToList().ToPagedList(sayfa, 10);
            return View(listele);
        }

        [Route("uye-yazilari/{ad}/{id}")]
        public ActionResult DenemeDetay(int id)
        {
            var detayListele = db.KullaniciYazi.Where(x => x.YaziID == id).ToList();
            ViewBag.yorumSayi = db.KullaniciYorum.Where(x => x.KullaniciYaziID == id && x.Durum == true).Count();
            return View(detayListele);
        }
        
        public PartialViewResult SonYazilar()
        {
            var getir = db.KullaniciYazi.OrderByDescending(x => x.YaziID).Where(x=>x.YaziDurum==true).Take(4).ToList();
            return PartialView(getir);
        }

        public PartialViewResult YorumListele(int id)
        {
            var listele = db.KullaniciYorum.Where(x => x.KullaniciYaziID == id && x.Durum == true).ToList();
            return PartialView(listele);
        }

      
        [HttpGet]
        public PartialViewResult DenemeYorum(int id)
        {
            ViewBag.id = id;
            return PartialView();
        }

        [HttpPost]
        public PartialViewResult DenemeYorum(KullaniciYorum p)
        {

            p.Durum = false;
            p.YorumTarih = DateTime.Now;
            db.KullaniciYorum.Add(p);
            db.SaveChanges();
            TempData["denemeyorum"] = " ";
            return PartialView();
        }

        public PartialViewResult SideBar()
        {

            var a = db.Blog.OrderBy(u => Guid.NewGuid()).Where(x=>x.BlogResim!=null).Take(3).ToList();
            return PartialView(a);
        }

        public PartialViewResult Galeri()
        {
            var listele = db.Harita.Where(x => x.HaritaID >= 2).ToList();
            return PartialView(listele);
        }

        public ActionResult Yazar(int id, int sayfa = 1)
        {
            var listele = db.KullaniciYazi.Where(x => x.Uye == id).ToList().ToPagedList(sayfa, 5);
            ViewBag.yazar = db.KullaniciYazi.Where(x => x.Uye == id).Select(x => x.Uye1.UyeKullaniciAdi).FirstOrDefault();
            return View(listele);
        }


    }
}