using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TezProje.Models.Siniflar;

namespace TezProje.Controllers
{
    public class HakkimdaController : Controller
    {
        // GET: Hakkimda
        TatilBlogEntities db = new TatilBlogEntities();

        [Route("hakkimizda")]
        public ActionResult Index()
        {
            var listele = db.Hakkimda.ToList();
            return View(listele);
        }


    }
}