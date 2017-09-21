using System;
using System.IO;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Stock.Core.Domain;
using Stock.Core.Repository;
using Stock.UI.ViewModels.Base;

namespace Stock.UI
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel(Action<string> showDialogMessage, Func<bool> showLoginWindow, Action settingsWindow)
        {
            ShowDialogMessage = showDialogMessage;
            ShowLoginWindow = showLoginWindow;
            ShowSettingsWindow = settingsWindow;

            SettingsCommand = new RelayCommand(x => ShowSettingsWindow());
            AccountCommand = new RelayCommand(x => ShowAccountAction());
            ManualCommand = new RelayCommand(x => ManualMethod());
            ExitCommand = new RelayCommand(x => ExitMethod());

            var defaultImageUri = new Uri(@"pack://application:,,,/Stock;component/Themes/UserAcc.png");
            _defaultUserImage = new BitmapImage(defaultImageUri);

            LoadUser();
        }

        private UserAcc _user = UserAcc.Guest();
        public UserAcc User
        {
            get { return _user; }
            set { _user = value; OnPropertyChanged("User"); }
        }
        
        private BitmapImage _userImage = new BitmapImage();
        public BitmapImage UserImage
        {
            get { return _userImage; }
            set { _userImage = value; OnPropertyChanged("UserImage"); }
        }

        public Action<string> ShowDialogMessage { get; set; }
        public Func<bool> ShowLoginWindow { get; set; }
        public ICommand SettingsCommand { get; set; }
        public Action ShowSettingsWindow { get; set; }
        public ICommand AccountCommand { get; set; }
        public Action ShowAccountAction { get; set; }
        public ICommand ManualCommand { get; set; }
        public ICommand ExitCommand { get; set; }

        private readonly BitmapImage _defaultUserImage;

        private void LoadUser()
        {
            if (!LoadSettings()) return;

            //Blank user image
            UserImage = _defaultUserImage;

            var currentUser = AppSettings.User;
            if (currentUser == null)
                ShowLoginWindow();
            else
            {
                User = currentUser;
                LoadUserProfile();
            }
        }

        private bool LoadSettings()
        {
            AppSettings.Load();
            return ValidateSettings();
        }

        private void LoadUserProfile()
        {
            var userImage = User.UserImagePath;
            if (!Path.IsPathRooted(userImage))
                userImage = Path.GetFullPath(userImage);

            if (!File.Exists(userImage))
                ShowDialogMessage("Файл с изображением пользователя не найден.\r\nПуть к файлу: " + userImage);
            else
            {
                UserImage = new BitmapImage(new Uri(userImage));
            }
        }

        private bool ValidateSettings()
        {
            var settingsAppFolder = Properties.Settings.Default.SettingsAppFolder;
            var templatesFolderPath = Properties.Settings.Default.TemplatesFolderPath;
            var exportFolderPath = Properties.Settings.Default.ExportFolderPath;
            var stockUnitFilesFolder = Properties.Settings.Default.StockUnitFilesFolder;

            if (!Directory.Exists(settingsAppFolder))
            {
                ShowDialogMessage("Директория " + settingsAppFolder + " не существует. Проверьте настройки");
                return false;
            }
            if (!Directory.Exists(templatesFolderPath))
            {
                ShowDialogMessage("Директория " + templatesFolderPath + " не существует. Проверьте настройки");
                return false;
            }
            if (!Directory.Exists(exportFolderPath))
            {
                ShowDialogMessage("Директория " + exportFolderPath + " не существует. Проверьте настройки");
                return false;
            }
            if (!Directory.Exists(stockUnitFilesFolder))
            {
                ShowDialogMessage("Директория " + stockUnitFilesFolder + " не существует. Проверьте настройки");
                return false;
            }
            return true;
        }

        private void ManualMethod()
        {
            const string manualPath = ".\\Manual.pdf";
            if (!File.Exists(manualPath))
                ShowDialogMessage("Файл Manual.pdf не найден");
            else
            {
                System.Diagnostics.Process.Start(".\\Manual.pdf");
            }
        }

        private void ExitMethod()
        {
            ShowLoginWindow();
            LoadUser();
        }
    }
}
