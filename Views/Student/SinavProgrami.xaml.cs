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
using System.Windows.Shapes;

namespace gp_unisis.Views.Student
{
    /// <summary>
    /// SinavProgrami.xaml etkileşim mantığı
    /// </summary>
    public partial class SinavProgrami : UserControl
    {
        public ObservableCollection<SinavProgramiSatiri> Sinavlar { get; set; }

        public SinavProgrami()
        {
            Sinavlar = new ObservableCollection<SinavProgramiSatiri>
            {
                new SinavProgramiSatiri { ID=1, SinavTipi="Ara Sınav", SinavAdi="Matematik I", SinavSuresi="90 dk", SinavTarihi="25.06.2025", SinavSaati="10:00" },
                new SinavProgramiSatiri { ID=2, SinavTipi="Final", SinavAdi="Fizik II", SinavSuresi="120 dk", SinavTarihi="30.06.2025", SinavSaati="14:00" },
                new SinavProgramiSatiri { ID=3, SinavTipi="Ara Sınav", SinavAdi="Programlama", SinavSuresi="60 dk", SinavTarihi="27.06.2025", SinavSaati="13:00" },
                new SinavProgramiSatiri { ID=4, SinavTipi="Yerenlemeli Sınav", SinavAdi="Nasıl Sinemlenilir", SinavSuresi="5 dk", SinavTarihi="27.06.2025", SinavSaati="13:00" }

            };
            //this.DataContext = this;
            InitializeComponent();
        }
    }

    public class SinavProgramiSatiri
    {
        public int ID { get; set; }
        public string SinavTipi { get; set; }
        public string SinavAdi { get; set; }
        public string SinavSuresi { get; set; }
        public string SinavTarihi { get; set; }
        public string SinavSaati { get; set; }
    }
}
