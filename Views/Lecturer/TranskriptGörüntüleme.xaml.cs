using OKUL.Views.Akademisyen;
using System;
using System.Windows;

namespace OKUL.Akademisyen
{
    public partial class TranskriptGörüntüleme : Window
    {
        public TranskriptGörüntüleme()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var dersEkran = new DersProgramı();
            dersEkran.Show();
            this.Hide();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var sınavEkran = new SınavProgramı();
            sınavEkran.Show();
            this.Hide();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var dersEklemeEkran = new ProgramaDersEkleme();
            dersEklemeEkran.Show();
            this.Hide();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            var sınavProgram = new SınavProgramı();
            sınavProgram.Show();
            this.Hide();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            var notEkran = new NotGirişi();
            notEkran.Show();
            this.Hide();
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            var transkriptHesapla = new TranskriptAnaSayfa();
            transkriptHesapla.Show();
            this.Hide();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            listViewSinavlar.Items.Add(new NotItem { Id = 1, OgrenciAdi = "Ali Yılmaz", OgrenciNo = "20230001", SinavAdi = "Matematik - Vize", HarfNotu = "BB" });
            listViewSinavlar.Items.Add(new NotItem { Id = 2, OgrenciAdi = "Ayşe Demir", OgrenciNo = "20230002", SinavAdi = "Fizik - Final", HarfNotu = "AA" });
            listViewSinavlar.Items.Add(new NotItem { Id = 3, OgrenciAdi = "Mehmet Can", OgrenciNo = "20230003", SinavAdi = "Kimya - Bütünleme", HarfNotu = "CC" });
            listViewSinavlar.Items.Add(new NotItem { Id = 4, OgrenciAdi = "Zeynep Aydın", OgrenciNo = "20230004", SinavAdi = "Biyoloji - Vize", HarfNotu = "BA" });
        }
    }

    public class NotItem
    {
        public int Id { get; set; }
        public string OgrenciAdi { get; set; }
        public string OgrenciNo { get; set; }
        public string SinavAdi { get; set; }
        public string HarfNotu { get; set; }
    }
}