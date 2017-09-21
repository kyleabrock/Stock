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

            var dbDataSource = Properties.Settings.Default.DbDataSource;
            var dbInitialCatalog = Properties.Settings.Default.DbInitialCatalog;
            var dbUserId = Properties.Settings.Default.UserId;
            var integratedSecurity = Properties.Settings.Default.IntegratedSecurity;

            if (!string.IsNullOrEmpty(dbDataSource)) DbDataSource = dbDataSource;
            if (!string.IsNullOrEmpty(dbInitialCatalog)) DbInitialCatalog = dbInitialCatalog;
            if (!string.IsNullOrEmpty(dbUserId)) DbUserId = dbUserId;
            IntergatedSecurity = integratedSecurity;
        }

        public ConnectionViewModel(Action<string> fillPass) : this()
        {
            var dbPassword = Properties.Settings.Default.DbPassword;
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

        private bool _intergatedSecurity;
        public bool IntergatedSecurity
        {
            get { return _intergatedSecurity; }
            set 
            { 
                _intergatedSecurity = value;
                OnPropertyChanged("IntergatedSecurity");
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

            Properties.Settings.Default.DbDataSource = DbDataSource;
            Properties.Settings.Default.DbInitialCatalog = DbInitialCatalog;
            if (IntergatedSecurity)
            {
                Properties.Settings.Default.DbUserID = String.Empty;
                Properties.Settings.Default.DbPassword = String.Empty;
            }
            else
            {
                Properties.Settings.Default.DbUserID = DbUserId;

                var password = passwordBox.Password;
                Properties.Settings.Default.DbPassword = password;
            }

            var configureResults = NHibernateHelper.Configure(DbDataSource, DbInitialCatalog, DbUserId, passwordBox.Password, IntergatedSecurity);
            if (!configureResults)
                ShowInfoMessage("Ошибка подключения к базе данных.\r\nПодробные сведения об ошибке:\r\n" + NHibernateHelper.LastError);
            else
            {
                Properties.Settings.Default.Save();
                CancelCommand.Execute(null);
            }
        }

        private void CheckConnectionMethod(object parameter)
        {
            var passwordBox = parameter as PasswordBox;
            if (passwordBox == null) return;
            
            var configureResults = NHibernateHelper.Configure(DbDataSource, DbInitialCatalog, DbUserId, passwordBox.Password, IntergatedSecurity);
            if (!configureResults)
                ShowInfoMessage("Ошибка подключения к базе данных.\r\nПодробные сведения об ошибке:\r\n" + NHibernateHelper.LastError);
            else
            {
                ShowInfoMessage("Успешное подключение к базе данных");
            }
        }
    }
}
