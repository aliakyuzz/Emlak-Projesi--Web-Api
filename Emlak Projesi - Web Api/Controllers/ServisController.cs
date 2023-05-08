using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Emlak_Projesi.Models;
using Emlak_Projesi.ViewModel;

namespace Emlak_Projesi.Controllers
{
    public class ServisController : ApiController
    {
        DB01Entities db = new DB01Entities();
        SonucModel sonuc = new SonucModel();

        #region Kategori

        [HttpGet]
        [Route("api/kategoriliste")]

        public List<KategoriModel> KategoriListe()
        {

            List<KategoriModel> liste = db.Kategori.Select(x => new KategoriModel() 
            {
            katId=x.katId,
            katAdi=x.katAdi,
            katUrunSay=x.Urun.Count()
           
            
            }).ToList();

            return liste;
        }

        [HttpGet]
        [Route("api/kategoribyid/{katId}")]
        public KategoriModel KategoriById(int katId)
        {
            KategoriModel kayit = db.Kategori.Where(s => s.katId == katId).Select(x => new KategoriModel() 
            {
                katId = x.katId,
                katAdi = x.katAdi,
                katUrunSay = x.Urun.Count()

            }).FirstOrDefault();
            return kayit;

            }

        [HttpPost]
        [Route("api/kategoriekle")]
        public SonucModel KategoriEkle(KategoriModel model)
        {
            if (db.Kategori.Count(s=> s.katAdi == model.katAdi) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Girilen Kategori Adı Kayıtlıdır!";
                return sonuc;
            }

            Kategori yeni = new Kategori();
            yeni.katAdi = model.katAdi;
            db.Kategori.Add(yeni);
            db.SaveChanges();

            sonuc.islem = true;
            sonuc.mesaj = "Kategori Eklendi";
        
            return sonuc;
        }
        [HttpPut]
        [Route("api/kategoriduzenle")]
        public SonucModel KategoriDuzenle(KategoriModel model)

        {
            Kategori kayit = db.Kategori.Where(s => s.katId == model.katId).FirstOrDefault();
            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunamadı!";
                return sonuc;

            }
            kayit.katAdi = model.katAdi;
            db.SaveChanges();

            sonuc.islem = true;
            sonuc.mesaj = "Kategori Düzenlendi";
            return sonuc;
        }


        [HttpDelete]
        [Route("api/kategorisil/{katId}")]
        public SonucModel KategoriSil(int katId)
        {
            Kategori kayit = db.Kategori.Where(s => s.katId == katId).FirstOrDefault();
            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunamadı!";
                return sonuc;

            }


            if(db.Urun.Count(s => s.urunKatId == katId ) > 0)
            {

                sonuc.islem = false;
                sonuc.mesaj = "Üzerinde Ürün Kaydı Olan Kategori Silinemez!";
                return sonuc;
            }


            db.Kategori.Remove(kayit);
            db.SaveChanges();

            sonuc.islem = true;
            sonuc.mesaj = "Kategori Silindi";
            return sonuc;
        }




        #endregion

        #region Urun

        [HttpGet]
        [Route("api/urunliste")]
        public List<UrunModel> UrunListe()
        {

            List<UrunModel> liste = db.Urun.Select(x => new UrunModel()
            {
                urunId = x.urunId,
                urunAdi = x.urunAdi,
                urunKatId = x.urunKatId,
                urunKatAdi = x.Kategori.katAdi,
                urunFiyat = x.urunFiyat


            }).ToList();

            return liste;

        }

        [HttpGet]
        [Route("api/urunlistebykatid/{katId}")]

        public List<UrunModel> UrunListeByKatId(int katId)
        {
            List<UrunModel> liste = db.Urun.Where(s => s.urunKatId == katId).Select (x => new UrunModel()
            {
                urunId = x.urunId,
                urunAdi = x.urunAdi,
                urunKatId = x.urunKatId,
                urunKatAdi = x.Kategori.katAdi,
                urunFiyat = x.urunFiyat


            }).ToList();

            return liste;

        }


        [HttpGet]
        [Route("api/urunbyid/{urunId}")]

        public UrunModel UrunById(int urunId)
        {
            UrunModel kayit = db.Urun.Where(s => s.urunId == urunId).Select(x => new UrunModel()
            {
                urunId = x.urunId,
                urunAdi = x.urunAdi,
                urunKatId = x.urunKatId,
                urunKatAdi = x.Kategori.katAdi,
                urunFiyat = x.urunFiyat


            }).FirstOrDefault();



            return kayit;

        }


        [HttpPost]
        [Route("api/urunekle")]
        public SonucModel UrunEkle(UrunModel model)
        {
            if (db.Urun.Count(s => s.urunAdi == model.urunAdi && s.urunKatId == model.urunKatId) > 0)

            {
                sonuc.islem = false;
                sonuc.mesaj = "Girilen Ürün İlgili Kategorilerde Kayıtlıdır!";

            }

            Urun yeni = new Urun();
            yeni.urunAdi = model.urunAdi;
            yeni.urunFiyat = model.urunFiyat;
            yeni.urunKatId = model.urunKatId;

            db.Urun.Add(yeni);
            db.SaveChanges();

            sonuc.islem = true;
            sonuc.mesaj = "Ürün Eklendi";

            return sonuc;



        }

        [HttpPut]
        [Route("api/urunduzenle")]
        public SonucModel UrunDuzenle(UrunModel model)
        {
            Urun kayit = db.Urun.Where(s => s.urunId == model.urunId).FirstOrDefault();

            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunamadı!";
                return sonuc;

            }
            kayit.urunAdi = model.urunAdi;
            kayit.urunFiyat = model.urunFiyat;
            kayit.urunKatId = model.urunKatId;
            db.SaveChanges();

            sonuc.islem = true;
            sonuc.mesaj = "Ürün Düzenlendi";

            return sonuc;



        }

        [HttpDelete]
        [Route("api/urunsil/{urunId}")]
        public SonucModel UrunSil(int urunId)
        {
            Urun kayit = db.Urun.Where(s => s.urunId == urunId).FirstOrDefault();

            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunamadı!";
                return sonuc;

            }

            db.Urun.Remove(kayit);
            db.SaveChanges();

            sonuc.islem = true;
            sonuc.mesaj = "Ürün Silindi";

            return sonuc;


        }




        #endregion







    }
}
