using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Emlak_Projesi.ViewModel
{
    public class UrunModel
    {
        public int urunId { get; set; }
        public string urunAdi { get; set; }
        public int urunKatId { get; set; }
        public string urunKatAdi { get; set; }

        public decimal urunFiyat { get; set; }
    }
}