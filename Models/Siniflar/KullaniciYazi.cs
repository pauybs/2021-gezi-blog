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
    using System.Web.Mvc;

    public partial class KullaniciYazi
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public KullaniciYazi()
        {
            this.KullaniciYorum = new HashSet<KullaniciYorum>();
        }
    
        public int YaziID { get; set; }
        public string YaziBaslik { get; set; }
        [AllowHtml]
        public string YaziAciklama { get; set; }
        public string YaziResim { get; set; }
        public Nullable<int> Uye { get; set; }
        public string YaziResimAltTag { get; set; }
        public string YaziTitleTag { get; set; }
        public string YaziDescriptonTag { get; set; }
        public Nullable<System.DateTime> YaziTarih { get; set; }
        public Nullable<bool> YaziDurum { get; set; }
    
        public virtual Uye Uye1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<KullaniciYorum> KullaniciYorum { get; set; }
    }
}
