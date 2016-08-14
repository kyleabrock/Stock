using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Stock.UI.ViewModels.Base;

namespace Stock.UI.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        public SettingsViewModel()
        {
            SettingsAppFolderPath = (string)Properties.Settings.Default["SettingsAppFolder"];
            RefreshPeriod = Properties.Settings.Default["RefreshPeriod"].ToString();

            SaveCommand = new RelayCommand(x => SaveMethod());
            SelectFolderCommand = new RelayCommand(x => SelectFolderMethod());
        }

        private string _settingsAppFolderPath;
        public string SettingsAppFolderPath
        {
            get { return _settingsAppFolderPath; }
            set { _settingsAppFolderPath = value; OnPropertyChanged("SettingsAppFolderPath"); }
        }

        private string _refreshPeriod;
        public string RefreshPeriod
        {
            get { return _refreshPeriod; }
            set { _refreshPeriod = value; OnPropertyChanged("RefreshPeriod"); }
        }

        public ICommand SaveCommand { get; set; }
        public ICommand SelectFolderCommand { get; set; }
        public Action SelectFolderAction { get; set; }

        private void SaveMethod()
        {
            if (CheckValues())
            {
                Properties.Settings.Default["SettingsAppFolder"] = SettingsAppFolderPath;

                int refreshPeriod = int.Parse(RefreshPeriod);
                Properties.Settings.Default["RefreshPeriod"] = refreshPeriod;
                
                Properties.Settings.Default.Save();
            }
        }

        private bool CheckValues()
        {
            int result;
            if (!int.TryParse(RefreshPeriod, out result))
            {
                MessageBox.Show("Значение поля период обновление должно быть целым числом!");
                return false;
            }
            if (result <= 5)
            {
                MessageBox.Show("Значение поля период обновление должно быть не менее 5 сек.!");
                return false;
            }
            if (!Directory.Exists(SettingsAppFolderPath))
            {
                MessageBox.Show("Директория указанная в поле ресурсы приложения не существует!");
                return false;
            }
            return true;
        }

        private void SelectFolderMethod()
        {
            SelectFolderAction();
        }
    }
}
