using System;
using System.IO;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Stock.Core.Domain;
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

            //Blank user image
            var defaultImageUri = new Uri(@"pack://application:,,,/Stock;component/Themes/UserAcc.png");
            _defaultUserImage = new BitmapImage(defaultImageUri);
            UserImage = _defaultUserImage;

            var currentUser = ApplicationState.GetValue<UserAcc>("User");
            if (currentUser != null)
            {
                User = currentUser;
                LoadData();
            }
            else
            {
                showLoginWindow();
            }
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

        private readonly BitmapImage _defaultUserImage;

        private void LoadData()
        {
            if (!LoadSettings())
                return;
            LoadUserProfile();
        }

        private bool LoadSettings()
        {
            ApplicationState.LoadSettings();
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
            var settingsAppFolder = ApplicationState.GetValue<string>("SettingsAppFolder");
            var templatesFolderPath = ApplicationState.GetValue<string>("TemplatesFolderPath");
            var exportFolderPath = ApplicationState.GetValue<string>("ExportFolderPath");
            var stockUnitFilesFolder = ApplicationState.GetValue<string>("StockUnitFilesFolder");

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
    }
}
