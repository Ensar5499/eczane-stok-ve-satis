using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    // İlaç sınıfı
    public class Ilac
    {
        public int Id { get; set; } // İlaç ID'si
        public string Ad { get; set; } // İlaç adı
        public string Firma { get; set; } // İlaç firmasının adı
        public int Stok { get; set; } // Mevcut stok miktarı
        public DateTime SonKullanmaTarihi { get; set; } // İlaç son kullanma tarihi
        public decimal Fiyat { get; set; } // İlaç fiyatı
    }

    // Satış sınıfı
    public class Satis
    {
        public int IlacId { get; set; } // İlaç ID'si
        public int Miktar { get; set; } // Satılan miktar
        public DateTime Tarih { get; set; } // Satış tarihi
    }

    // Eczane yönetim sistemi sınıfı
    public class EczaneYonetim
    {
        private List<Ilac> ilaclar = new List<Ilac>(); // İlaçlar listesi
        private List<Satis> satislar = new List<Satis>(); // Satışlar listesi
        private int sonrakiId = 1; // Yeni ilacın ID'si için sayaç

        // Yeni ilaç ekleme metodu
        public void IlacEkle(string ad, string firma, int stok, DateTime sonKullanmaTarihi, decimal fiyat)
        {
            Ilac yeniIlac = new Ilac
            {
                Id = sonrakiId++, // Yeni ilaç için ID atanır
                Ad = ad, // İlaç adı
                Firma = firma, // İlaç firmasının adı
                Stok = stok, // İlaç stoğu
                SonKullanmaTarihi = sonKullanmaTarihi, // İlaç son kullanma tarihi
                Fiyat = fiyat // İlaç fiyatı
            };
            ilaclar.Add(yeniIlac); // İlaç listeye eklenir
            Console.WriteLine($"İlaç eklendi: {ad} (Firma: {firma}, Fiyat: {fiyat:C})");
        }

        // İlaç güncelleme metodu
        public void IlacDuzenle(int id, string yeniAd, int yeniStok, decimal yeniFiyat)
        {
            var ilac = ilaclar.Find(i => i.Id == id); // ID'sine göre ilaç bulunur
            if (ilac != null)
            {
                ilac.Ad = yeniAd; // İlaç adı güncellenir
                ilac.Stok = yeniStok; // İlaç stoğu güncellenir
                ilac.Fiyat = yeniFiyat; // İlaç fiyatı güncellenir
                Console.WriteLine($"İlaç güncellendi: {ilac.Ad}");
            }
            else
            {
                Console.WriteLine("İlaç bulunamadı!"); // İlaç bulunamazsa hata mesajı
            }
        }                                                           

        // İlaç silme metodu
        public void IlacSil(int id)
        {
            var ilac = ilaclar.Find(i => i.Id == id); // ID'sine göre ilaç bulunur
            if (ilac != null)
            {
                ilaclar.Remove(ilac); // İlaç listeden silinir
                Console.WriteLine($"{ilac.Ad} eczane stoklarından silindi.");
            }
            else
            {
                Console.WriteLine("İlaç bulunamadı!"); // İlaç bulunamazsa hata mesajı
            }
        }

        // Stok güncelleme metodu
        public void StokGuncelle(int id, int miktar)
        {
            var ilac = ilaclar.Find(i => i.Id == id); // ID'sine göre ilaç bulunur
            if (ilac != null)
            {
                ilac.Stok += miktar; // Stok miktarı güncellenir
                Console.WriteLine($"İlaç stoğu güncellendi: {ilac.Ad} (Yeni Stok: {ilac.Stok})");
            }
            else
            {
                Console.WriteLine("İlaç bulunamadı!"); // İlaç bulunamazsa hata mesajı
            }
        }

        // İlaç satışı metodu
        public void IlacSat(int id, int miktar)
        {
            var ilac = ilaclar.Find(i => i.Id == id); // ID'sine göre ilaç bulunur
            if (ilac != null)
            {
                if (ilac.Stok >= miktar) // Yeterli stok varsa
                {
                    ilac.Stok -= miktar; // Stok azaltılır
                    decimal toplamFiyat = miktar * ilac.Fiyat; // Toplam satış tutarı hesaplanır
                    Console.WriteLine($"{ilac.Ad} satıldı. Fatura Tutarı: {toplamFiyat:C}");

                    // Satışı kaydet
                    satislar.Add(new Satis
                    {
                        IlacId = id, // Satılan ilacın ID'si
                        Miktar = miktar, // Satılan miktar
                        Tarih = DateTime.Now // Satış tarihi
                    });
                }
                else
                {
                    Console.WriteLine("Yetersiz stok!"); // Yetersiz stok durumu
                }
            }
            else
            {
                Console.WriteLine("İlaç bulunamadı!"); // İlaç bulunamazsa hata mesajı
            }
        }

        // İlaç listeleme metodu
        public void IlaclariListele()
        {
            Console.WriteLine("\nEczane Stokları:");
            foreach (var ilac in ilaclar)
            {
                Console.WriteLine($"ID: {ilac.Id}, Ad: {ilac.Ad}, Firma: {ilac.Firma}, Stok: {ilac.Stok}, SKT: {ilac.SonKullanmaTarihi:dd.MM.yyyy}, Fiyat: {ilac.Fiyat:C}");
            }
        }

        // Son kullanma tarihi kontrolü metodu
        public void SonKullanmaTarihiKontrol()
        {
            Console.WriteLine("\nSon Kullanma Tarihi Geçen İlaçlar:");
            foreach (var ilac in ilaclar)
            {
                if (ilac.SonKullanmaTarihi < DateTime.Now) // Son kullanma tarihi geçmiş ilaçlar
                {
                    Console.WriteLine($"ID: {ilac.Id}, Ad: {ilac.Ad}, SKT: {ilac.SonKullanmaTarihi:dd.MM.yyyy}");
                }
            }
        }

        // En çok satılan ilaç raporu metodu
        public void EnCokSatisYapilanIlac()
        {
            var enCokSatisYapilan = satislar
                .GroupBy(s => s.IlacId) // Satışlar ilaç ID'sine göre gruplanır
                .OrderByDescending(g => g.Sum(s => s.Miktar)) // En çok satılan ilaçlar
                .FirstOrDefault();

            if (enCokSatisYapilan != null)
            {
                var ilac = ilaclar.Find(i => i.Id == enCokSatisYapilan.Key); // En çok satılan ilaç bulunur
                Console.WriteLine($"En Çok Satılan İlaç: {ilac.Ad} ({enCokSatisYapilan.Sum(s => s.Miktar)} Adet)");
            }
            else
            {
                Console.WriteLine("Henüz satış yapılmadı."); // Satış yapılmadıysa bilgi verilir
            }
        }

        // En çok satılan gün raporu metodu
        public void EnCokSatisYapilanGun()
        {
            var enCokSatisYapilanGun = satislar
                .GroupBy(s => s.Tarih.Date) // Satışlar tarihine göre gruplanır
                .OrderByDescending(g => g.Count()) // En çok satış yapılan gün
                .FirstOrDefault();

            if (enCokSatisYapilanGun != null)
            {
                Console.WriteLine($"En Çok Satış Yapılan Gün: {enCokSatisYapilanGun.Key:dd.MM.yyyy} ({enCokSatisYapilanGun.Count()} Satış)");
            }
            else
            {
                Console.WriteLine("Henüz satış yapılmadı."); // Satış yapılmadıysa bilgi verilir
            }
        }

        // Gün sonu raporu metodu
        public void GunSonuRaporu()
        {
            decimal toplamSatisTutari = satislar
                .Where(s => s.Tarih.Date == DateTime.Now.Date) // Bugün yapılan satışlar
                .Sum(s => ilaclar.First(i => i.Id == s.IlacId).Fiyat * s.Miktar); // Toplam satış tutarı hesaplanır

            Console.WriteLine($"\nBugün Yapılan Satışların Toplam Tutarı: {toplamSatisTutari:C}");

            EnCokSatisYapilanIlac(); // En çok satılan ilaç raporu
            EnCokSatisYapilanGun(); // En çok satış yapılan gün raporu
        }
    }

    // Programın ana metodu
    static void Main(string[] args)
    {
        EczaneYonetim eczane = new EczaneYonetim(); // Eczane yönetim nesnesi oluşturulur

        // Kullanıcı işlemleri
        bool devam = true; // İşlem döngüsü için kontrol değişkeni
        while (devam)
        {
            // Kullanıcıya menü seçenekleri sunulur
            Console.WriteLine("\nEczane Yönetim Sistemi");
            Console.WriteLine("1. İlaçları Listele");
            Console.WriteLine("2. Yeni İlaç Ekle");
            Console.WriteLine("3. İlaç Güncelle");
            Console.WriteLine("4. İlaç Sil");
            Console.WriteLine("5. Stok Güncelle");
            Console.WriteLine("6. İlaç Sat");
            Console.WriteLine("7. Son Kullanma Tarihi Kontrolü");
            Console.WriteLine("8. En Çok Satılan İlaç Raporu");
            Console.WriteLine("9. En Çok Satılan Gün Raporu");
            Console.WriteLine("10. Gün Sonu Raporu");
            Console.WriteLine("11. Çıkış");
            Console.Write("Seçiminizi yapın: ");
            int secim = int.Parse(Console.ReadLine()); // Kullanıcıdan seçim alınır

            switch (secim)
            {
                case 1:
                    eczane.IlaclariListele(); // İlaçları listele
                    break;
                case 2:
                    Console.Write("İlaç Adı: ");
                    string ad = Console.ReadLine(); // İlaç adı girilir
                    Console.Write("Firma: ");
                    string firma = Console.ReadLine(); // Firma adı girilir
                    Console.Write("Stok: ");
                    int stok = int.Parse(Console.ReadLine()); // Stok miktarı girilir
                    Console.Write("Son Kullanma Tarihi (yyyy-MM-dd): ");
                    DateTime skt = DateTime.Parse(Console.ReadLine()); // Son kullanma tarihi girilir
                    Console.Write("Fiyat: ");
                    decimal fiyat = decimal.Parse(Console.ReadLine()); // İlaç fiyatı girilir
                    eczane.IlacEkle(ad, firma, stok, skt, fiyat); // Yeni ilaç eklenir
                    break;
                case 3:
                    Console.Write("Güncellemek istediğiniz ilacın ID'sini girin: ");
                    int idGuncelle = int.Parse(Console.ReadLine()); // Güncellenmek istenen ilacın ID'si alınır
                    Console.Write("Yeni Ad: ");
                    string yeniAd = Console.ReadLine(); // Yeni ad girilir
                    Console.Write("Yeni Stok: ");
                    int yeniStok = int.Parse(Console.ReadLine()); // Yeni stok miktarı girilir
                    Console.Write("Yeni Fiyat: ");
                    decimal yeniFiyat = decimal.Parse(Console.ReadLine()); // Yeni fiyat girilir
                    eczane.IlacDuzenle(idGuncelle, yeniAd, yeniStok, yeniFiyat); // İlaç güncellenir
                    break;
                case 4:
                    Console.Write("Silmek istediğiniz ilacın ID'sini girin: ");
                    int idSil = int.Parse(Console.ReadLine()); // Silinmek istenen ilacın ID'si alınır
                    eczane.IlacSil(idSil); // İlaç silinir
                    break;
                case 5:
                    Console.Write("Stok güncellemek istediğiniz ilacın ID'sini girin: ");
                    int idStok = int.Parse(Console.ReadLine()); // Stok güncellenecek ilacın ID'si alınır
                    Console.Write("Eklenecek miktar: ");
                    int miktar = int.Parse(Console.ReadLine()); // Eklenecek miktar girilir
                    eczane.StokGuncelle(idStok, miktar); // Stok güncellenir
                    break;
                case 6:
                    Console.Write("Satmak istediğiniz ilacın ID'sini girin: ");
                    int idSat = int.Parse(Console.ReadLine()); // Satılacak ilacın ID'si alınır
                    Console.Write("Satılacak miktar: ");
                    int miktarSat = int.Parse(Console.ReadLine()); // Satılacak miktar girilir
                    eczane.IlacSat(idSat, miktarSat); // İlaç satılır
                    break;
                case 7:
                    eczane.SonKullanmaTarihiKontrol(); // Son kullanma tarihi kontrol edilir
                    break;
                case 8:
                    eczane.EnCokSatisYapilanIlac(); // En çok satılan ilaç raporu görüntülenir
                    break;
                case 9:
                    eczane.EnCokSatisYapilanGun(); // En çok satılan gün raporu görüntülenir
                    break;
                case 10:
                    eczane.GunSonuRaporu(); // Gün sonu raporu görüntülenir
                    break;
                case 11:
                    devam = false; // Döngü sonlandırılır
                    Console.WriteLine("Çıkış yapılıyor...");
                    break;
                default:
                    Console.WriteLine("Geçersiz seçim!"); // Geçersiz seçim yapılırsa hata mesajı
                    break;
            }
        }
    }
}
