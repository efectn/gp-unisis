using OKUL.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace OKUL.ViewModels
{
    public class SinavProgramiViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<SinavProgramiSatiri> _sinavProgramiListesi;
        public ObservableCollection<SinavProgramiSatiri> SinavProgramiListesi
        {
            get => _sinavProgramiListesi;
            set
            {
                _sinavProgramiListesi = value;
                OnPropertyChanged();
            }
        }

        public ICommand DuzenleCommand { get; }
        public ICommand SilCommand { get; }
        public ICommand NotGirisCommand { get; }

        public SinavProgramiViewModel()
        {
            SinavProgramiListesi = new ObservableCollection<SinavProgramiSatiri>
            {
                new SinavProgramiSatiri { ID=1, SinavTipi="Ara Sınav", SinavAdi="Matematik I", SinavSuresi="90 dk", SinavTarihi="25.06.2025", SinavSaati="10:00" },
                new SinavProgramiSatiri { ID=2, SinavTipi="Final", SinavAdi="Fizik II", SinavSuresi="120 dk", SinavTarihi="30.06.2025", SinavSaati="14:00" },
                new SinavProgramiSatiri { ID=3, SinavTipi="Ara Sınav", SinavAdi="Programlama", SinavSuresi="60 dk", SinavTarihi="27.06.2025", SinavSaati="13:00" },
                new SinavProgramiSatiri { ID=4, SinavTipi="Yerenlemeli Sınav", SinavAdi="Nasıl Sinemlenilir", SinavSuresi="5 dk", SinavTarihi="27.06.2025", SinavSaati="13:00" }

            };

            DuzenleCommand = new RelayCommand(param => Duzenle(param));
            SilCommand = new RelayCommand(param => Sil(param));
            NotGirisCommand = new RelayCommand(param => NotGiris(param));
        }

        private void Duzenle(object param)
        {
            if (param is SinavProgramiSatiri satir)
                MessageBox.Show($"Düzenle: {satir.SinavAdi}");
        }

        private void Sil(object param)
        {
            if (param is SinavProgramiSatiri satir)
            {
                if (MessageBox.Show($"{satir.SinavAdi} silinsin mi?", "Silme Onayı", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    SinavProgramiListesi.Remove(satir);
            }
        }

        private void NotGiris(object param)
        {
            if (param is SinavProgramiSatiri satir)
                MessageBox.Show($"Not Girişi: {satir.SinavAdi}");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly System.Action<object> execute;
        private readonly System.Func<object, bool> canExecute;

        public RelayCommand(System.Action<object> execute, System.Func<object, bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public event System.EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public bool CanExecute(object parameter) => canExecute == null || canExecute(parameter);

        public void Execute(object parameter) => execute(parameter);
    }
}
