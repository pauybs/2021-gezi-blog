//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TezProje.Models.Siniflar
{
    using System;
    using System.Collections.Generic;
    
    public partial class Uye
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Uye()
        {
            this.KullaniciYazi = new HashSet<KullaniciYazi>();
        }
    
        public int UyeID { get; set; }
        public string UyeKullaniciAdi { get; set; }
        public string UyeMail { get; set; }
        public string UyeSifre { get; set; }
        public Nullable<bool> UyeDurum { get; set; }
        public string UyeAd { get; set; }
        public Nullable<System.DateTime> UyeTarih { get; set; }
        public string UyeHakkimda { get; set; }
        public string UyeFoto { get; set; }
        public string UyeSehir { get; set; }
        public string Yetki { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<KullaniciYazi> KullaniciYazi { get; set; }
    }
}
