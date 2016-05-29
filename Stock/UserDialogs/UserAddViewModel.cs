using System;
using System.Windows.Input;
using Core.Domain;
using Core.Repository;

namespace Stock.UserDialogs
{
    public class UserAddViewModel : ViewModelBase
    {
        public UserAddViewModel()
        {
            InitViewModel(new UserAcc());
        }

        public UserAddViewModel(UserAcc arg)
        {
            InitViewModel(arg);
        }

        private UserAcc _userAcc;
        public UserAcc UserAcc
        {
            get
            {
                return _userAcc;
            }
            set
            {
                _userAcc = value;
                OnPropertyChanged("UserAcc");
            }
        }

        public ICommand SaveCommand { get; set; }
        public ICommand CloseCommand { get; set; }
        public Action CloseAction { get; set; }

        private Repository<UserAcc> _userAccRepository;

        private void InitViewModel(UserAcc status)
        {
            _userAccRepository = new Repository<UserAcc>();
            UserAcc = status;

            SaveCommand = new RelayCommand(x => SaveMethod());
            CloseCommand = new RelayCommand(x => CloseMethod());
        }

        private void SaveMethod()
        {
            _userAccRepository.Save(UserAcc);
            CloseAction();
        }

        private void CloseMethod()
        {
            CloseAction();
        }
    }
}
