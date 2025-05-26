using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OKUL.Views.Ogrenciİsleri
{
    public class Ogrenci
    {
        public int Id { get; set; }
        public string OgrenciNo { get; set; }
        public string AdSoyad { get; set; }
        public string Bolum { get; set; }
    }

    /// <summary>
    /// OgrenciListele.xaml etkileşim mantığı
    /// </summary>
    public partial class OgrenciListele : Window
    {
        public ObservableCollection<Ogrenci> Ogrenciler { get; set; }

        public OgrenciListele()
        {
            Ogrenciler = new ObservableCollection<Ogrenci>
        {
            new Ogrenci { Id = 1, OgrenciNo = "1001", AdSoyad = "Ahmet Yılmaz", Bolum = "Matematik" },
            new Ogrenci { Id = 2, OgrenciNo = "1002", AdSoyad = "Ayşe Demir", Bolum = "Fizik" },
            new Ogrenci { Id = 3, OgrenciNo = "1003", AdSoyad = "Mehmet Kaya", Bolum = "Kimya" },
        };

            this.DataContext = this;

            InitializeComponent();
        }
    }
}
