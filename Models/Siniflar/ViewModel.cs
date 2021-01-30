using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TezProje.Models.Siniflar;


namespace TezProje.Models.Siniflar
{
    public class ViewModel
    {
        public List<Blog>  blog { get; set; }
        public List<Uye> uye { get; set; }
        public List<Yorumlar> yorum { get; set; }
        public List<KullaniciYorum> kullaniciYorum { get; set; }
        public List<KullaniciYazi> kullaniciYazi { get; set; }
    }
}