using System;
using System.Windows.Input;
using Stock.Core.Domain;
using Stock.Core.Factory;
using Stock.Core.Repository;
using Stock.UI.ViewModels.Base;

namespace Stock.UI.ViewModels.Dialogs
{
    public class StaffAddViewModel : ViewModelBase
    {
        public StaffAddViewModel()
        {
            InitViewModel(new Staff());
        }

        public StaffAddViewModel(Staff arg)
        {
            InitViewModel(arg);
        }

        private Staff _staff;
        public Staff Staff
        {
            get { return _staff; }
            set
            {
                _staff = value;
                OnPropertyChanged("Staff");
            }
        }

        public ICommand SaveCommand { get; set; }
        public ICommand CloseCommand { get; set; }
        public Action CloseAction { get; set; }

        private StaffRepository _staffRepository;

        private void InitViewModel(Staff status)
        {
            _staffRepository = new StaffRepository();
            Staff = status;

            SaveCommand = new RelayCommand(x => SaveMethod());
            CloseCommand = new RelayCommand(x => CloseMethod());
        }

        private void SaveMethod()
        {
            _staffRepository.Save(Staff);

            var user = ApplicationState.GetValue<UserAcc>("User");
            ILogFactory logFactory = new LogFactory();
            var logEntity = logFactory.CreateLogMessage(user, Staff);
            var repository = new Repository<Log>();
            repository.Save(logEntity);

            CloseAction();
        }

        private void CloseMethod()
        {
            CloseAction();
        }
    }
}
