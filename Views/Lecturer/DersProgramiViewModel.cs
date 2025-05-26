using CommunityToolkit.Mvvm.ComponentModel;
using OKUL.Models;
using System.Collections.ObjectModel;

namespace OKUL.ViewModels
{
    public partial class DersProgramiViewModel : ObservableObject
    {
        [ObservableProperty]
        public ObservableCollection<Ders> Dersler;

        public string[] Gunler { get; } = { "Pazartesi", "Salı", "Çarşamba", "Perşembe", "Cuma" };
        public string[] Saatler { get; } = { "08:00", "09:00", "10:00", "11:00", "12:00", "13:00", "14:00", "15:00" };

        public DersProgramiViewModel()
        {
            Dersler = new ObservableCollection<Ders>
            {
                new Ders { Gun = "Pazartesi", Saat = "09:00", DersAdi = "Matematik", DersKodu = "MAT101" },
                new Ders { Gun = "Salı", Saat = "10:00", DersAdi = "Fizik", DersKodu = "FIZ201" },
                new Ders { Gun = "Çarşamba", Saat = "11:00", DersAdi = "Kimya", DersKodu = "KIM102" },
                new Ders { Gun = "Cuma", Saat = "13:00", DersAdi = "Biyoloji", DersKodu = "BIO301" },
            };
        }
    }
}