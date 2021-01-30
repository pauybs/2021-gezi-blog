using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TezProje.Models.Siniflar;

namespace TezProje.Controllers
{
    public class IletisimController : Controller
    {
        // GET: Iletisim
        TatilBlogEntities db = new TatilBlogEntities();
       
        [HttpGet]
        [Route("iletisim")]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("iletisim")]
        public ActionResult Index(İletisim p)
        {
            
            db.İletisim.Add(p);
            db.SaveChanges();
            TempData["iletisim"] = " ";
            return RedirectToAction("Index", "Iletisim");
        }

        public PartialViewResult Harita()
        {
            var a = db.Harita.Where(x=>x.HaritaID==1).ToList();
            return PartialView(a);
        }
    }
}