using System;
using System.Windows.Input;
using Stock.Core;
using Stock.Core.Domain;
using Stock.Core.Repository;
using Stock.UI.ViewModels.Base;

namespace Stock.UI.ViewModels.Dialogs
{
    public class StatusAddViewModel : ViewModelBase
    {
        public StatusAddViewModel()
        {
            InitViewModel(new Status());
        }

        public StatusAddViewModel(Status arg)
        {
            InitViewModel(arg);
        }

        private Status _status;
        public Status Status
        {
            get { return _status; }
            set
            {
                _status = value;
                OnPropertyChanged("Status");
            }
        }

        public int StatusType
        {
            get { return (int) Status.StatusType; }
            set
            {
                Status.StatusType = (StatusTypes) Enum.Parse(typeof(StatusTypes), value.ToString());
                OnPropertyChanged("StatusType");
            }
        }

        public ICommand SaveCommand { get; set; }
        public ICommand CloseCommand { get; set; }
        public Action CloseAction { get; set; }

        private StatusRepository _statusRepository;

        private void InitViewModel(Status status)
        {
            _statusRepository = new StatusRepository();
            Status = status;

            SaveCommand = new RelayCommand(x => SaveMethod());
            CloseCommand = new RelayCommand(x => CloseMethod());
        }

        private void SaveMethod()
        {
            _statusRepository.Save(Status);
            CloseAction();
        }

        private void CloseMethod()
        {
            CloseAction();
        }
    }
}
