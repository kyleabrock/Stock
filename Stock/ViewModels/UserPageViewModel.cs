using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Stock.Core.Domain;
using Stock.Core.Repository;
using Stock.UI.ViewModels.Base;

namespace Stock.UI.ViewModels
{
    public class UserPageViewModel : ViewModelBase
    {
        public UserPageViewModel()
        {
            _settingsFolder = (string)Properties.Settings.Default["SettingsAppFolder"];

            SetUserCommand = new RelayCommand(x => SetUserMethod());
            LogoutCommand = new RelayCommand(x => LogoutMethod());
            
            if (SetUserCommand != null)
                SetUserCommand.Execute(null);
        }

        public ICommand SetUserCommand { get; set; }
        public ICommand LogoutCommand { get; set; }
        public Action Logout { get; set; }

        private string _settingsFolder;

        private UserAcc _user;
        public UserAcc User
        {
            get { return _user; }
            set { _user = value; OnPropertyChanged("User"); }
        }

        private ImageSource _userImageSource;
        public ImageSource UserImageSource
        {
            get { return _userImageSource; }
            set { _userImageSource = value; OnPropertyChanged("UserImageSource"); }
        }

        private ObservableCollection<Log> _userLogList;
        public ObservableCollection<Log> UserLogList
        {
            get { return _userLogList; }
            set { _userLogList = value; OnPropertyChanged("UserLogList"); }
        }

        private void SetUserMethod()
        {
            var user = ApplicationState.GetValue<UserAcc>("User");
            if (user != null)
            {
                User = user;

                var logRepository = new LogRepository();
                UserLogList = new ObservableCollection<Log>(logRepository.GetAllByUserId(User.Id, 0));

                var imagePath = _settingsFolder + user.UserImagePath;
                if (File.Exists(imagePath))
                    UserImageSource = new BitmapImage(new Uri(imagePath));
            }
            else
            {
                _user = new UserAcc();
                UserLogList = null;
                UserImageSource = (BitmapImage) Application.Current.Resources["UserAccBitmapImage"];
            }
        }

        private void LogoutMethod()
        {
            Logout();
        }
    }
}
