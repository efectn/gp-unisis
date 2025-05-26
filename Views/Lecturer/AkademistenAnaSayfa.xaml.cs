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
using OKUL.Akademisyen;

namespace OKUL.Views.Akademisyen
{
    /// <summary>
    /// AkademistenAnaSayfa.xaml etkileşim mantığı
    /// </summary>
    public partial class AkademistenAnaSayfa : Window
    {
        public AkademistenAnaSayfa()
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
    }

    
}
