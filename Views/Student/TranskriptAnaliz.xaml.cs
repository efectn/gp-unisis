using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace OKUL.Views.OgrenciSayfalari
{
    public partial class TranskriptAnaliz : Window
    {
        public class HarfNotuAnaliz
        {
            public string Harf { get; set; }
            public string Aralik { get; set; }
            public int KisiSayisi { get; set; }
        }

        public TranskriptAnaliz()
        {
            InitializeComponent();

            // Örnek veri
            var notlar = new List<double> { 96, 91, 85, 82, 78, 76, 69, 63, 59, 52, 45, 30 };
            bool bagilMi = false;

            // Not türü
            notTuruTextBlock.Text = $"Not Türü: {(bagilMi ? "Bağıl" : "Ham")}";

            // Harf aralıkları
            var harfAraliklari = new List<(string Harf, double Min, double Max)>
            {
                ("AA", 90, 100),
                ("BA", 85, 89.99),
                ("BB", 80, 84.99),
                ("CB", 75, 79.99),
                ("CC", 65, 74.99),
                ("DC", 60, 64.99),
                ("DD", 50, 59.99),
                ("FD", 40, 49.99),
                ("FF", 0, 39.99)
            };

            // Analiz listesi
            var analizSonuclari = new List<HarfNotuAnaliz>();
            foreach (var aralik in harfAraliklari)
            {
                int sayi = notlar.Count(n => n >= aralik.Min && n <= aralik.Max);
                analizSonuclari.Add(new HarfNotuAnaliz
                {
                    Harf = aralik.Harf,
                    Aralik = $"{aralik.Min}-{aralik.Max}",
                    KisiSayisi = sayi
                });
            }

            harfNotuListView.ItemsSource = analizSonuclari;

            // Ortalama ve standart sapma
            double ort = notlar.Average();
            double sapma = Math.Sqrt(notlar.Select(n => Math.Pow(n - ort, 2)).Average());

            ortalamaTextBlock.Text = $"Ortalama: {ort:F2}";
            stdSapmaTextBlock.Text = $"Standart Sapma: {sapma:F2}";
        }
    }
}
