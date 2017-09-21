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
            SetUserCommand = new RelayCommand(x => SetUserMethod());
            OkCommand = new RelayCommand(x => OkMethod());
            CancelCommand = new RelayCommand(x => CloseAction());

            User = AppSettings.User;
            Account = AppSettings.Account;

            SetUserCommand.Execute(null);
        }

        private void OkMethod()
        {
            var userRepository = new Repository<UserAcc>();
            userRepository.Save(User);
            CloseAction();
        }

        public ICommand SetUserCommand { get; set; }
        public ICommand OkCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public Action CloseAction { get; set; }

        private UserAcc _user;
        public UserAcc User
        {
            get { return _user; }
            set { _user = value; OnPropertyChanged("User"); }
        }

        private Account _account;
        public Account Account
        {
            get { return _account; }
            set { _account = value; OnPropertyChanged("Account"); }
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
            var user = AppSettings.User;
            var settingsFolder = AppSettings.GetValue<string>("SettingsAppFolder");
            if (user != null)
            {
                User = user;

                var logRepository = new LogRepository();
                UserLogList = new ObservableCollection<Log>(logRepository.GetAllByUserId(User.Id, 0));

                var imagePath = settingsFolder + user.UserImagePath;
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
    }
}
