using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace gp_unisis.Views.Student
{
    public partial class Transkript : UserControl
    {
        public Transkript()
        {
            InitializeComponent();
            YükleVeriler();
        }

        public class TranskriptDers
        {
            public string DersAdi { get; set; }
            public int Kredi { get; set; }
            public string Akademisyen { get; set; }
            public double HamNot { get; set; }
            public double TSkor { get; set; }
            public string HarfNotu { get; set; }
        }

        private void YükleVeriler()
        {
            // Örnek dönemler
            /*donemComboBox.ItemsSource = new List<string> { "2023 Güz", "2024 Bahar" };
            donemComboBox.SelectedIndex = 0;

            // Örnek transkript verisi
            var dersler = new List<TranskriptDers>
            {
                new TranskriptDers { DersAdi = "Matematik", Kredi = 4, Akademisyen = "Dr. Ali", HamNot = 85, TSkor = 87, HarfNotu = "BA" },
                new TranskriptDers { DersAdi = "Fizik", Kredi = 3, Akademisyen = "Dr. Ayşe", HamNot = 78, TSkor = 80, HarfNotu = "BB" },
                new TranskriptDers { DersAdi = "Programlama", Kredi = 5, Akademisyen = "Dr. Mehmet", HamNot = 92, TSkor = 90, HarfNotu = "AA" }
            };

            transkriptListView.ItemsSource = dersler;

            // Ortalama hesapla
            double toplamNot = 0;
            double toplamKredi = 0;

            foreach (var ders in dersler)
            {
                toplamNot += ders.TSkor * ders.Kredi;
                toplamKredi += ders.Kredi;
            }

            double ano = toplamKredi > 0 ? toplamNot / toplamKredi : 0;
            double gano = ano; // Bu örnekte GANO ile ANO aynı

            anoTextBlock.Text = $"Dönem ANO: {ano:F2}";
            ganoTextBlock.Text = $"Genel GANO: {gano:F2}";*/
        }

        private void Analiz_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is TranskriptDers ders)
            {
                MessageBox.Show($"'{ders.DersAdi}' dersi için analiz gösterilecek.", "Analiz", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
