using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace OKUL.Views.OgrenciSayfalari
{
    public class Ders
    {
        public bool IsSelected { get; set; }
        public string DersAdi { get; set; }
        public string Akademisyen { get; set; }
        public int Kredi { get; set; }
        public string Donem { get; set; }
        public int Kontenjan { get; set; }
    }

    /// <summary>
    /// DersSecimiListele.xaml etkileşim mantığı
    /// </summary>
    public partial class DersSecimi : Window
    {
        public DersSecimi()
        {
            InitializeComponent();
            var dersListesi = new List<Ders>
            {
                new Ders { IsSelected = false, DersAdi = "Matematik", Akademisyen = "Dr. Ahmet", Kredi = 5, Donem = "Güz", Kontenjan = 40 },
                new Ders { IsSelected = true, DersAdi = "Fizik", Akademisyen = "Dr. Elif", Kredi = 4, Donem = "Güz", Kontenjan = 35 },
                new Ders { IsSelected = false, DersAdi = "Kimya", Akademisyen = "Dr. Can", Kredi = 3, Donem = "Bahar", Kontenjan = 50 }
            };

            dersListView.ItemsSource = dersListesi;
        }
    }
}
