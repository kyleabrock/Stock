using System;
using System.Windows.Controls;
using System.Windows.Input;
using Stock.Core;
using Stock.UI.ViewModels.Base;

namespace Stock.UI.ViewModels.Dialogs
{
    public class ConnectionViewModel : ViewModelBase
    {
        public ConnectionViewModel()
        {
            CheckConnectionCommand = new RelayCommand(CheckConnectionMethod);
            SaveCommand = new RelayCommand(SaveMethod);
            CancelCommand = new RelayCommand(x => CloseAction());

            var dbDataSource = ApplicationState.GetValue<string>("DbDataSource");
            var dbInitialCatalog = ApplicationState.GetValue<string>("DbInitialCatalog");
            var dbUserId = ApplicationState.GetValue<string>("DbUserId");
            var integratedSecurity = ApplicationState.GetValue<bool>("IntegratedSecurity");

            if (!string.IsNullOrEmpty(dbDataSource)) DbDataSource = dbDataSource;
            if (!string.IsNullOrEmpty(dbInitialCatalog)) DbInitialCatalog = dbInitialCatalog;
            if (!string.IsNullOrEmpty(dbUserId)) DbUserId = dbUserId;
            IntegratedSecurity = integratedSecurity;
        }

        public ConnectionViewModel(Action<string> fillPass) : this()
        {
            var dbPassword = ApplicationState.GetValue<string>("DbPassword");
            fillPass(dbPassword);
        }

        private string _dbDataSource = String.Empty;
        public string DbDataSource 
        {
            get { return _dbDataSource; }
            set
            {
                _dbDataSource = value;
                OnPropertyChanged("DbDataSource");
            }
        }

        private string _dbInitialCatalog = String.Empty;
        public string DbInitialCatalog 
        {
            get { return _dbInitialCatalog; }
            set
            {
                _dbInitialCatalog = value;
                OnPropertyChanged("DbInitialCatalog");
            }
        }

        private string _dbUserId = String.Empty;
        public string DbUserId
        {
            get { return _dbUserId; }
            set
            {
                _dbUserId = value;
                OnPropertyChanged("DbUserId");
            }
        }

        private bool _integratedSecurity;
        public bool IntegratedSecurity
        {
            get { return _integratedSecurity; }
            set 
            { 
                _integratedSecurity = value;
                OnPropertyChanged("IntegratedSecurity");
            }
        }

        public ICommand CheckConnectionCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public Action CloseAction { get; set; }
        public Action<string> ShowInfoMessage { get; set; }

        private void SaveMethod(object parameter)
        {
            var passwordBox = parameter as PasswordBox;
            if (passwordBox == null) return;
            
            ApplicationState.SetValue("DbDataSource", DbDataSource);
            ApplicationState.SetValue("DbInitialCatalog", DbInitialCatalog);
            if (IntegratedSecurity)
            {
                ApplicationState.SetValue("DbUserId", String.Empty);
                ApplicationState.SetValue("DbPassword", String.Empty);
                ApplicationState.SetValue("IntegratedSecurity", IntegratedSecurity);
            }
            else
            {
                ApplicationState.SetValue("DbUserId", DbUserId);

                var password = passwordBox.Password;
                ApplicationState.SetValue("DbPassword", password);
            }

            var configureResults = NHibernateHelper.Configure(DbDataSource, DbInitialCatalog, DbUserId, passwordBox.Password, IntegratedSecurity);
            if (!configureResults)
                ShowInfoMessage("Ошибка подключения к базе данных.\r\nПодробные сведения об ошибке:\r\n" + NHibernateHelper.LastError);
            else
            {
                ApplicationState.SaveConnectionSettings();
                CancelCommand.Execute(null);
            }
        }

        private void CheckConnectionMethod(object parameter)
        {
            var passwordBox = parameter as PasswordBox;
            if (passwordBox == null) return;
            
            var configureResults = NHibernateHelper.Configure(DbDataSource, DbInitialCatalog, DbUserId, passwordBox.Password, IntegratedSecurity);
            if (!configureResults)
                ShowInfoMessage("Ошибка подключения к базе данных.\r\nПодробные сведения об ошибке:\r\n" + NHibernateHelper.LastError);
            else
            {
                ShowInfoMessage("Успешное подключение к базе данных");
            }
        }
    }
}
