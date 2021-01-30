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
    public class BlogController : Controller
    {
        // GET: Blog

        TatilBlogEntities db = new TatilBlogEntities();


        [Route("blog")]
        public ActionResult Index(int sayfa=1)
        {
            var listele = db.Blog.Where(x=>x.Durum==true).ToList().ToPagedList(sayfa,5);
            return View(listele);
        }

        [Route("blog/{baslik}/{id}")]
        public ActionResult BlogDetay(int id)
        {
            var bul = db.Blog.Where(x => x.BlogID == id).ToList(); ;
            
            return View(bul);
        }

        public PartialViewResult YorumListele(int id)
        {
            var listele = db.Yorumlar.Where(x => x.BlogID == id && x.Durum==true).ToList();
            return PartialView(listele);
        }

        [HttpGet]
        public PartialViewResult Yorum(int id)
        {
            ViewBag.id = id;
            return PartialView();
        }

        [HttpPost]
        public PartialViewResult Yorum(Yorumlar p)
        {
            p.Durum = false;
            p.YorumTarih = DateTime.Now;
            db.Yorumlar.Add(p);
            db.SaveChanges();
            TempData["yorumblog"] = " ";
            return PartialView();
        }



        //[OutputCache(Duration =20)]
        public PartialViewResult SideBar()
        {
           
            var a = db.Blog.OrderBy(u => Guid.NewGuid()).Where(x=>x.Durum==true && x.BlogResim!=null).Take(3).ToList();
            return PartialView(a);
        }

        public PartialViewResult Galeri()
        {
            var listele = db.Harita.Where(x => x.HaritaID >= 2).ToList();
            return PartialView(listele);
        }

        public PartialViewResult SideBarHakkimda()
        {
            var goster = db.İletisim.Where(x => x.İletisimID == 6).ToList();
            return PartialView(goster);
        }

        public PartialViewResult Email()
        {
            return PartialView();
        }

        public PartialViewResult SosyalMedya()
        {
            var goster = db.İletisim.Where(x => x.İletisimID == 7).ToList();
            return PartialView(goster);
        }
        public ActionResult Yazar(int id,int sayfa=1)
        {
            var listele = db.Blog.Where(x => x.YazarID == id).ToList().ToPagedList(sayfa, 5);
            ViewBag.yazar = db.Blog.Where(x => x.YazarID == id).Select(x => x.Admin.KullaniciAdi).FirstOrDefault();
            return View(listele);
        }
    }
}

