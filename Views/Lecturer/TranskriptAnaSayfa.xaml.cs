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
    using OKUL.Views.Akademisyen;

    namespace OKUL.Akademisyen
    {
        /// <summary>
        /// TranskriptAnaSayfa.xaml etkileşim mantığı
        /// </summary>
        public partial class TranskriptAnaSayfa : Window
        {
            public TranskriptAnaSayfa()
            {
                InitializeComponent();
            }

            private void Button_Click(object sender, RoutedEventArgs e)
            {
                var dersEkren = new DersProgramı();
                dersEkren.Show();
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
                // ListView'e satır ekle
                listViewSinavlar.Items.Add(new SinavItem { Id = 1, SinavAdi = "Vize Sınavı" , DersAdi = "Ders1"});
                listViewSinavlar.Items.Add(new SinavItem { Id = 2, SinavAdi = "Final Sınavı" , DersAdi = "Ders2"});
                listViewSinavlar.Items.Add(new SinavItem { Id = 3, SinavAdi = "Bütünleme" , DersAdi = "Ders3"});
            }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            var transkriptGörübtüle = new TranskriptGörüntüleme();
            transkriptGörübtüle.Show();
            this.Hide();
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            var transkriptHesapla = new TranskriptHesaplama();
            transkriptHesapla.Show();
            this.Hide();
        }
    }

    public class SinavItem
        {
            public int Id { get; set; }
            public string DersAdi { get; set; }

            public string SinavAdi { get; set; }
        }
    }
