using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using static OKUL.Views.OgrenciSayfalari.DersGruplari;

namespace OKUL.Views.OgrenciSayfalari
{
    public partial class DersGrubuAyrinti : Window
    {
        public ObservableCollection<Ders> Dersler { get; set; } = new ObservableCollection<Ders>();

        public string SecmeliText { get; set; } = "Belirsiz";
        public int DersSayisi => Dersler?.Count ?? 0;
        public int AlinanKredi => Dersler?.Sum(d => d.Kredi) ?? 0;
        public int AlinanDersSayisi => Dersler?.Count ?? 0;

        public DersGrubuAyrinti()
        {
            InitializeComponent();

            SecmeliText = "Evet";

            // Örnek veri - sen burada gerçek verileri çekebilirsin
            Dersler = new ObservableCollection<Ders>
            {
                new Ders { DersAdi = "Matematik I", Kredi = 4, Donem = "Güz 2024", Akademisyen = "Prof. Dr. Ayşe Yılmaz" },
                new Ders { DersAdi = "Fizik I", Kredi = 3, Donem = "Güz 2024", Akademisyen = "Doç. Dr. Mehmet Kara" },
                new Ders { DersAdi = "Programlama Temelleri", Kredi = 5, Donem = "Güz 2024", Akademisyen = "Dr. Canan Demir" },
            };

            this.DataContext = this;
        }

        public class Ders
        {
            public string DersAdi { get; set; }
            public int Kredi { get; set; }
            public string Donem { get; set; }
            public string Akademisyen { get; set; }
        }
    }
}
