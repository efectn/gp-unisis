using OKUL.Models;
using OKUL.ViewModels;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using OKUL.Akademisyen;

namespace OKUL.Views.Akademisyen
{
    public partial class DersProgramı : Window
    {
        private DersProgramiViewModel ViewModel => (DersProgramiViewModel)DataContext;

        public DersProgramı()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ScheduleGrid.RowDefinitions.Clear();
            ScheduleGrid.ColumnDefinitions.Clear();
            ScheduleGrid.Children.Clear();

            var gunler = ViewModel.Gunler;
            var saatler = ViewModel.Saatler;

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

            foreach (var ders in ViewModel.Dersler)
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var dersDüzenle = new DersDüzenleSil();
            dersDüzenle.Show();
            this.Hide();
        }
    }
}
