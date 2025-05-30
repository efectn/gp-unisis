using gp_unisis.Views.Admin;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Controls;

namespace gp_unisis.Views
{
    /// <summary>
    /// LoginWindow.xaml etkileşim mantığı
    /// </summary>
    public partial class LoginWindow : UserControl
    {
        public LoginWindow()
        {
            InitializeComponent();
            //DataContext = new LoginViewModel();
        }
        /*private void GirisTuru_Click(object sender, RoutedEventArgs e)
        {
            if(sender==OgrenciButton)
            {
                IdariButton.IsChecked = false;
                OgretmenButton.IsChecked = false;
                AdminButton.IsChecked = false;

            }
            if(sender==IdariButton)
            {
                OgrenciButton.IsChecked = false;
                OgretmenButton.IsChecked = false;
                AdminButton.IsChecked= false;
            }
            if(sender==AdminButton)
            {
                IdariButton.IsChecked = false;
                OgrenciButton.IsChecked = false;
                OgretmenButton.IsChecked = false;

            }
            if(sender==OgretmenButton)
            {
                IdariButton.IsChecked = false;
               OgrenciButton.IsChecked=false;
                AdminButton.IsChecked = false;
            }

        }
        
        private void GirisYapTiklandi(object sender,RoutedEventArgs e)
        {
           

            if (OgrenciButton.IsChecked==true)
            {
               

                var ogrenciPencere = new OgrenciAnaSayfa();
                ogrenciPencere.Show();
                this.Close();
            }
            if(OgretmenButton.IsChecked==true)
            {
                var ogretmenPencere = new OgretmenAnaSayfa();
                ogretmenPencere.Show();
                this.Close();
            }
            if(AdminButton.IsChecked==true)
            {
                var adminSayfa = new AdminAnaSayfa();
                adminSayfa.Show();
                this.Close();
            }
            if(IdariButton.IsChecked==true)
            {
                var idariSayfa = new OgrenciIsleriAna();
                idariSayfa.Show();
                this.Close();
            }
        }*/
    }
}
