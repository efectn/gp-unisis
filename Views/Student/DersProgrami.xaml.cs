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

namespace OKUL.Views.OgrenciSayfalari
{
    public class Ders2
    {
        public string DersKodu { get; set; }
        public string DersAdi { get; set; }
        public string Gun { get; set; }
        public string Saat { get; set; }
    }

    /// <summary>
    /// DersProgrami.xaml etkileşim mantığı
    /// </summary>
    public partial class DersProgrami : Window
    {
        public string[] Gunler { get; } = { "Pazartesi", "Salı", "Çarşamba", "Perşembe", "Cuma" };
        public string[] Saatler { get; } = { "08:00", "09:00", "10:00", "11:00", "12:00", "13:00", "14:00", "15:00" };

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ScheduleGrid.RowDefinitions.Clear();
            ScheduleGrid.ColumnDefinitions.Clear();
            ScheduleGrid.Children.Clear();

            var gunler = Gunler;
            var saatler = Saatler;

            var Dersler = new ObservableCollection<Ders2>
            {
                new Ders2 { Gun = "Pazartesi", Saat = "09:00", DersAdi = "Matematik", DersKodu = "MAT101" },
                new Ders2 { Gun = "Salı", Saat = "10:00", DersAdi = "Fizik", DersKodu = "FIZ201" },
                new Ders2 { Gun = "Çarşamba", Saat = "11:00", DersAdi = "Kimya", DersKodu = "KIM102" },
                new Ders2 { Gun = "Cuma", Saat = "13:00", DersAdi = "Biyoloji", DersKodu = "BIO301" },
            };

            ScheduleGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            foreach (var gun in gunler)
                ScheduleGrid.ColumnDefinitions.Add(new ColumnDefinition());

            ScheduleGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            foreach (var saat in saatler)
                ScheduleGrid.RowDefinitions.Add(new RowDefinition());

            for (int i = 0; i < gunler.Length; i++)
            {
                var header = new TextBlock
                {
                    Text = gunler[i],
                    FontWeight = FontWeights.Bold,
                    Background = Brushes.LightGray,
                    Padding = new Thickness(5),
                    TextAlignment = TextAlignment.Center
                };
                Grid.SetRow(header, 0);
                Grid.SetColumn(header, i + 1);
                ScheduleGrid.Children.Add(header);
            }

            for (int i = 0; i < saatler.Length; i++)
            {
                var header = new TextBlock
                {
                    Text = saatler[i],
                    FontWeight = FontWeights.Bold,
                    Background = Brushes.LightGray,
                    Padding = new Thickness(5),
                    TextAlignment = TextAlignment.Center
                };
                Grid.SetRow(header, i + 1);
                Grid.SetColumn(header, 0);
                ScheduleGrid.Children.Add(header);
            }

            foreach (var ders in Dersler)
            {
                int col = Array.IndexOf(gunler, ders.Gun) + 1;
                int row = Array.IndexOf(saatler, ders.Saat) + 1;

                if (col > 0 && row > 0)
                {
                    var cell = new Border
                    {
                        BorderBrush = Brushes.Gray,
                        BorderThickness = new Thickness(0.5),
                        Background = Brushes.LightBlue,
                        Margin = new Thickness(1),
                        Child = new TextBlock
                        {
                            Text = $"{ders.DersAdi}\n({ders.DersKodu})",
                            TextWrapping = TextWrapping.Wrap,
                            TextAlignment = TextAlignment.Center,
                            Margin = new Thickness(5)
                        }
                    };
                    Grid.SetRow(cell, row);
                    Grid.SetColumn(cell, col);
                    ScheduleGrid.Children.Add(cell);
                }
            }
        }

        public DersProgrami()
        {
            InitializeComponent();
        }
    }
}
