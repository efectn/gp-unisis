using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using static OKUL.Views.OgrenciSayfalari.DersGruplari;

namespace OKUL.Views.OgrenciSayfalari
{
    public partial class DersGruplari : Window, INotifyPropertyChanged
    {
        public ObservableCollection<DersGrubu> DersGruplariListesi { get; set; }

        private int _toplamKredi;
        public int ToplamKredi
        {
            get => _toplamKredi;
            set { _toplamKredi = value; OnPropertyChanged(nameof(ToplamKredi)); }
        }

        private int _toplamDers;
        public int ToplamDers
        {
            get => _toplamDers;
            set { _toplamDers = value; OnPropertyChanged(nameof(ToplamDers)); }
        }

        private string _mezunOlabilirMi;
        public string MezunOlabilirMi
        {
            get => _mezunOlabilirMi;
            set { _mezunOlabilirMi = value; OnPropertyChanged(nameof(MezunOlabilirMi)); }
        }

        private string _aktifDonem;
        public string AktifDonem
        {
            get => _aktifDonem;
            set { _aktifDonem = value; OnPropertyChanged(nameof(AktifDonem)); }
        }

        public DersGruplari()
        {
            InitializeComponent();

            DersGruplariListesi = new ObservableCollection<DersGrubu>
            {
                new DersGrubu { GrupAdi = "Zorunlu Dersler", MinKredi = 20, MinDersSayisi = 5, Secmeli = false },
                new DersGrubu { GrupAdi = "Alan Seçmeli", MinKredi = 10, MinDersSayisi = 3, Secmeli = true },
                new DersGrubu { GrupAdi = "Üniversite Ortak", MinKredi = 6, MinDersSayisi = 2, Secmeli = false },
                new DersGrubu { GrupAdi = "Serbest Seçmeli", MinKredi = 4, MinDersSayisi = 1, Secmeli = true }
            };

            // Örnek aktif dönem
            AktifDonem = "2024 Bahar";

            HesaplaToplamlar();

            this.DataContext = this;
        }

        private void HesaplaToplamlar()
        {
            // Toplam kredi ve ders sayısı hesapla
            ToplamKredi = DersGruplariListesi.Sum(d => d.MinKredi);
            ToplamDers = DersGruplariListesi.Sum(d => d.MinDersSayisi);

            // Mezun olabilme durumu (örnek: toplam kredi >= 40 ise mezun olabilir)
            MezunOlabilirMi = ToplamKredi >= 40 ? "Evet" : "Hayır";
        }

        public class DersGrubu
        {
            public string GrupAdi { get; set; }
            public int MinKredi { get; set; }
            public int MinDersSayisi { get; set; }
            public bool Secmeli { get; set; }
        }

        private void Detaylar_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is DersGrubu grup)
            {
                MessageBox.Show($"'{grup.GrupAdi}' grubunun detay sayfası açılacak.", "Grup Detay", MessageBoxButton.OK, MessageBoxImage.Information);
                // Burada: yeni sayfa açılabilir, örneğin:
                // new GrupDetaySayfasi(grup).Show();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}