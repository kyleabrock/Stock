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
            RefreshPeriod = Properties.Settings.Default["RefreshPeriod"].ToString();
            SettingsAppFolderPath = Properties.Settings.Default["SettingsAppFolder"].ToString();
            TemplatesFolderPath = Properties.Settings.Default["TemplatesFolderPath"].ToString();
            ExportFolderPath = Properties.Settings.Default["ExportFolderPath"].ToString();
            StockUnitFilesFolder = Properties.Settings.Default["StockUnitFilesFolder"].ToString();

            SaveCommand = new RelayCommand(x => SaveMethod());
            SettingsAppFolderCommand = new RelayCommand(x => { SettingsAppFolderPath = SelectFolderFunc() ?? SettingsAppFolderPath; });
            TemplatesFolderPathCommand = new RelayCommand(x => { TemplatesFolderPath = SelectFolderFunc() ?? TemplatesFolderPath; });
            ExportFolderPathCommand = new RelayCommand(x => { ExportFolderPath = SelectFolderFunc() ?? ExportFolderPath; });
            StockUnitFilesFolderCommand = new RelayCommand(x => { StockUnitFilesFolder = SelectFolderFunc() ?? StockUnitFilesFolder; });
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

        private string _templatesFolderPath;
        public string TemplatesFolderPath
        {
            get { return _templatesFolderPath; }
            set { _templatesFolderPath = value; OnPropertyChanged("TemplatesFolderPath"); }
        }

        private string _exportFolderPath;
        public string ExportFolderPath
        {
            get { return _exportFolderPath; }
            set { _exportFolderPath = value; OnPropertyChanged("ExportFolderPath"); }
        }

        private string _stockUnitFilesFolder;
        public string StockUnitFilesFolder
        {
            get { return _stockUnitFilesFolder; }
            set { _stockUnitFilesFolder = value; OnPropertyChanged("StockUnitFilesFolder"); }
        }

        public ICommand SaveCommand { get; set; }
        public ICommand SettingsAppFolderCommand { get; set; }
        public ICommand TemplatesFolderPathCommand { get; set; }
        public ICommand ExportFolderPathCommand { get; set; }
        public ICommand StockUnitFilesFolderCommand { get; set; }
        public Func<string> SelectFolderFunc { get; set; }

        private void SaveMethod()
        {
            if (CheckValues())
            {
                int refreshPeriod = int.Parse(RefreshPeriod);
                Properties.Settings.Default["RefreshPeriod"] = refreshPeriod;

                Properties.Settings.Default["SettingsAppFolder"] = SettingsAppFolderPath;
                Properties.Settings.Default["TemplatesFolderPath"] = TemplatesFolderPath;
                Properties.Settings.Default["ExportFolderPath"] = ExportFolderPath;
                Properties.Settings.Default["StockUnitFilesFolder"] = StockUnitFilesFolder;
                
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
    }
}
