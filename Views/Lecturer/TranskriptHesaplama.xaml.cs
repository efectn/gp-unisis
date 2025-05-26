using OKUL.Akademisyen;
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

namespace OKUL.Views.Akademisyen
{
    /// <summary>
    /// TranskriptHesaplama.xaml etkileşim mantığı
    /// </summary>
    public partial class TranskriptHesaplama : Window
    {
        public TranskriptHesaplama()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            listViewSinavlar.Items.Add(new NotItem
            {
                Id = 1,
                OgrenciAdi = "Ali Yılmaz",
                OgrenciNo = "20230001",
                VizeNotu = 65,
                FinalNotu = 75
            });

            listViewSinavlar.Items.Add(new NotItem
            {
                Id = 2,
                OgrenciAdi = "Ayşe Demir",
                OgrenciNo = "20230002",
                VizeNotu = 80,
                FinalNotu = 90
            });

            listViewSinavlar.Items.Add(new NotItem
            {
                Id = 3,
                OgrenciAdi = "Mehmet Can",
                OgrenciNo = "20230003",
                VizeNotu = 50,
                FinalNotu = 60
            });

            listViewSinavlar.Items.Add(new NotItem
            {
                Id = 4,
                OgrenciAdi = "Zeynep Aydın",
                OgrenciNo = "20230004",
                VizeNotu = 70,
                FinalNotu = 80
            });
        }


    }
    public class NotItem
    {
        public int Id { get; set; }
        public string OgrenciAdi { get; set; }
        public string OgrenciNo { get; set; }
        public int VizeNotu { get; set; }
        public int FinalNotu { get; set; }

    }
}
