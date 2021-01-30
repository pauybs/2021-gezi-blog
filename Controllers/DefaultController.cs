using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TezProje.Models.Siniflar;
using PagedList;
using PagedList.Mvc;
using System.Xml;
using System.Text;

namespace TezProje.Controllers
{
    
    public class DefaultController : Controller
    {
        // GET: Default
        
        TatilBlogEntities db = new TatilBlogEntities();

        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult Slider()
        {
            var blog = db.Blog.Where(x=>x.Durum==true).Take(10).ToList();
            return PartialView(blog);
        }

        public PartialViewResult RndYazi()
        {
            var blog = db.Blog.OrderBy(x => Guid.NewGuid()).Where(x=>x.Durum==true && x.BlogResim!=null).Take(1).ToList();
            return PartialView(blog);
        }


        public PartialViewResult Bilgilendirme()
        {
            var icerik = db.Bilgilendirme.ToList();
            return PartialView(icerik);
        }

        public PartialViewResult BilgilendirmeBaslik()
        {
            var baslik = db.Basliklar.Where(x => x.BaslikID == 1).ToList();
            return PartialView(baslik);
        }

        public PartialViewResult UyeSonYazi()
        {
            var listele = db.KullaniciYazi.Take(6).ToList();
            return PartialView(listele);
        }
        public PartialViewResult UyeSonYaziBaslik()
        {
            var baslik = db.Basliklar.Where(x => x.BaslikID == 2).ToList();
            return PartialView(baslik);
        }

        public PartialViewResult Galeri()
        {
            var getir = db.Harita.Where(x => x.HaritaID >= 2).ToList();
            return PartialView(getir);
        }

        
        public ActionResult Arama(string p)
        {
            ViewModel vm = new ViewModel();
            var bul = from x in db.Blog where x.Durum==true select x;
            var bul2 = from x in db.KullaniciYazi where x.YaziDurum == true select x;

            
            if (!string.IsNullOrEmpty(p) && p!=null)
            {
                if (bul.Where(x => x.Baslik.Contains(p)) !=null || bul2.Where(x => x.YaziBaslik.Contains(p)) !=null)
                {
                    vm.blog = bul.Where(x => x.Baslik.Contains(p)).ToList();
                    vm.kullaniciYazi = bul2.Where(x => x.YaziBaslik.Contains(p)).ToList();
                   

                    

                    if (vm.blog.Count!=0 || vm.kullaniciYazi.Count!=0)
                    {
                        return View(vm);
                    }
                    else
                    {
                        ViewBag.mesaj = "Aradığınız bulunamamıştır!";
                        return View();
                    }
                    
                    
                }
                else
                {
                    ViewBag.mesaj = "Aradığınız bulunamamıştır!";
                    return View();
                }
                
                
            }
            
            //else if (string.IsNullOrEmpty(p))
            //{
            //    return RedirectToAction("Index", "Default");
            //}
            else
            {
                ViewBag.mesaj = "Aradığınız bulunamamıştır!";
                return View();

            }
        }

        public PartialViewResult Email()
        {
            return PartialView();
        }

        public ActionResult EmailEkle(Bulten p)
        {
            var kontrol = db.Bulten.Where(x => x.Email.Contains(p.Email)).FirstOrDefault();
            if (kontrol!=null)
            {
                TempData["kayitli"] = " ";
                return RedirectToAction("Index", "Default");
            }
            else
            {
                db.Bulten.Add(p);
                db.SaveChanges();
                TempData["bulten"] = " ";
                return RedirectToAction("Index", "Default");
            }
            
        }


        [Route("sitemap.xml")]
        public ActionResult SitemapXML()
        {
            TatilBlogEntities db = new TatilBlogEntities();
            var siteurl = Request.Url.GetLeftPart(UriPartial.Authority);
            Response.Clear();
            Response.ContentType = "text/xml";

            XmlTextWriter xr = new XmlTextWriter(Response.OutputStream, Encoding.UTF8);
            xr.WriteStartDocument();
            xr.WriteStartElement("uriset");
            xr.WriteAttributeString("xmlns", "http://www.sitemaps.org/schemas/sitemap/0.9");
            xr.WriteAttributeString("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
            xr.WriteAttributeString("xsi:schemaLocation", "http://www.sitemaps.org/schemas/sitemap/0.9 http://www.sitemaps.org/schemas/sitemap/0.9/siteindex.xsd");

            xr.WriteStartElement("url");
            xr.WriteElementString("loc", siteurl + "/");
            xr.WriteEndElement();


            xr.WriteStartElement("url");
            xr.WriteElementString("loc", siteurl + Url.Action("Index", "Hakkimda"));
            xr.WriteEndElement();

            xr.WriteStartElement("url");
            xr.WriteElementString("loc", siteurl + Url.Action("Index", "Blog"));
            xr.WriteEndElement();

            xr.WriteStartElement("url");
            xr.WriteElementString("loc", siteurl + Url.Action("Index", "Deneme"));
            xr.WriteEndElement();

            foreach (var x in db.Blog)
            {
                xr.WriteStartElement("url");
                xr.WriteElementString("loc", siteurl + "/" + "blog" + "/" + Url.FriendlyUrl(x.Baslik) + "/" + x.BlogID);
                xr.WriteEndElement();
            }


            foreach (var x in db.KullaniciYazi)
            {
                xr.WriteStartElement("url");
                xr.WriteElementString("loc", siteurl + "/" + "uye-yazilari" + "/" + Url.FriendlyUrl(x.YaziBaslik) + "/" + x.YaziID);
                xr.WriteEndElement();
            }

            xr.WriteEndDocument();

            xr.Flush();
            xr.Close();
            Response.End();

            return View();
        }


    }
}