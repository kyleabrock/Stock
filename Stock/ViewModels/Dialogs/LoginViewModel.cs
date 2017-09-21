using System;
using System.Windows.Controls;
using System.Windows.Input;
using Stock.Core;
using Stock.Core.Domain;
using Stock.Core.Repository;
using Stock.UI.Utils;
using Stock.UI.ViewModels.Base;

namespace Stock.UI.ViewModels.Dialogs
{
    public class LoginViewModel : ViewModelBase
    {
        public LoginViewModel()
        {
            Action<object> loginAction = LoginMethod;
            LoginCommand = new AsyncCommand(loginAction);
            LoginCommand.RunWorkerCompleted += (s, e) => { if (User != null) LoginAction(); };
            ChangeConnectionCommand = new RelayCommand(x => ChangeConnectionAction());

            RememberCreditentials = Properties.Settings.Default.RememberCreditentials;
            LdapAuth = Properties.Settings.Default.LdapAuth;
            LoginText = Properties.Settings.Default.UserId;
        }

        public LoginViewModel(Action<string> fillPass) : this()
        {
            var userPassword = Properties.Settings.Default.UserPassword;
            fillPass(userPassword);
        }

        private bool _rememberCreditentials;
        public bool RememberCreditentials
        {
            get { return _rememberCreditentials; }
            set 
            { 
                _rememberCreditentials = value;
                OnPropertyChanged("RememberCreditentials");
            }
        }

        private bool _ldapAuth;
        public bool LdapAuth
        {
            get { return _ldapAuth; }
            set 
            { 
                _ldapAuth = value;
                OnPropertyChanged("LdapAuth");
            }
        }

        private string _loginText = String.Empty;
        public string LoginText
        {
            get { return _loginText; }
            set 
            { 
                _loginText = value;
                OnPropertyChanged("LoginText");
            }
        }

        private bool _showError;
        public bool ShowError
        {
            get { return _showError; }
            set 
            { 
                _showError = value;
                OnPropertyChanged("ShowError");
            }
        }

        private string _errorMessage = String.Empty;
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set 
            { 
                _errorMessage = value;
                OnPropertyChanged("ErrorMessage");
            }
        }

        private bool _inProgress;
        public bool InProgress
        {
            get { return _inProgress; }
            set
            {
                _inProgress = value; 
                OnPropertyChanged("InProgress");
            }
        }

        public AsyncCommand LoginCommand { get; private set; }
        public ICommand ChangeConnectionCommand { get; private set; }
        public Action ChangeConnectionAction { get; set; }
        public Action LoginAction { get; set; }
        private UserAcc User { get; set; }

        private void LoginMethod(object parameter)
        {
            InProgress = true;
            if (!ConfigureDbConnection())
            {
                ShowError = true;
                InProgress = false;
                return;
            }

            var passwordBox = parameter as PasswordBox;
            if (passwordBox == null) return;

            var accountRepository = new AccountRepository();
            var account = accountRepository.GetByLogin(LoginText);
            if (!account.IsNew)
            {
                var generator = new HashGenerator();
                var passwordHash = generator.GenerateSaltedHash(passwordBox.Password, account.Salt);

                if (string.Equals(passwordHash, account.HashedPassword))
                {
                    var userAccRepository = new Repository<UserAcc>();
                    account.UserAcc = userAccRepository.GetById(account.Id);
                    User = account.UserAcc;
                    SaveSettings(account, passwordBox.Password);
                    InProgress = false;
                    return;
                }
            }
            
            ErrorMessage = "Ошибка. Неверный логин/пароль.";
            ShowError = true;
            InProgress = false;
        }

        private void SaveSettings(Account account, string password)
        {
            AppSettings.User = User;
            AppSettings.Account = account;

            if (RememberCreditentials)
            {
                Properties.Settings.Default.RememberCreditentials = RememberCreditentials;
                Properties.Settings.Default.LdapAuth = LdapAuth;
                Properties.Settings.Default.UserId = LoginText;
                Properties.Settings.Default.UserPassword = password;
            }
            else
            {
                Properties.Settings.Default.RememberCreditentials = false;
                Properties.Settings.Default.LdapAuth = false;
                Properties.Settings.Default.UserId = String.Empty;
                Properties.Settings.Default.UserPassword = String.Empty;
            }
            
            Properties.Settings.Default.Save();
        }

        private bool ConfigureDbConnection()
        {
            var dbDataSource = Properties.Settings.Default.DbDataSource;
            var dbInitialCatalog = Properties.Settings.Default.DbInitialCatalog;
            var dbUserId = Properties.Settings.Default.DbUserID;
            var dbPassword = Properties.Settings.Default.DbPassword;
            var integratedSecurity = Properties.Settings.Default.IntegratedSecurity;
            
            var result = NHibernateHelper.Configure(dbDataSource, dbInitialCatalog, dbUserId, 
                dbPassword, integratedSecurity);
            if (!result)
            {
                ErrorMessage = "Ошибка при конфигурировании подключения." +
                               "\r\nПодробные сведения об ошибке: " + NHibernateHelper.LastError;
                return false;
            }
            if (!NHibernateHelper.TestConnection())
            {
                ErrorMessage = "Ошибка при подключении к базе данных." +
                               "\r\nПодробные сведения об ошибке: " + NHibernateHelper.LastError;
                return false;
            }

            return true;
        }
    }
}
