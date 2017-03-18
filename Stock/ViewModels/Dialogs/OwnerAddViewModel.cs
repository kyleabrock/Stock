using System;
using System.Windows.Input;
using Stock.Core.Domain;
using Stock.Core.Factory;
using Stock.Core.Repository;
using Stock.UI.ViewModels.Base;

namespace Stock.UI.ViewModels.Dialogs
{
    public class OwnerAddViewModel : ViewModelBase
    {
        public OwnerAddViewModel()
        {
            InitViewModel(new Owner());
        }

        public OwnerAddViewModel(Owner arg)
        {
            InitViewModel(arg);
        }

        private Owner _owner;
        public Owner Owner
        {
            get { return _owner; }
            set 
            {
                _owner = value;
                OnPropertyChanged("Owner");
            }
        }

        public ICommand SaveCommand { get; set; }
        public ICommand CloseCommand { get; set; }
        public Action CloseAction { get; set; }

        private OwnerRepository _ownerRepository;

        private void InitViewModel(Owner status)
        {
            _ownerRepository = new OwnerRepository();
            Owner = status;

            SaveCommand = new RelayCommand(x => SaveMethod());
            CloseCommand = new RelayCommand(x => CloseMethod());
        }

        private void SaveMethod()
        {
            _ownerRepository.Save(Owner);

            var user = ApplicationState.GetValue<UserAcc>("User");
            ILogFactory logFactory = new LogFactory();
            var logEntity = logFactory.CreateMessage(user, Owner);
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
