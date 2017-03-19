using System;
using System.Security.Cryptography;
using System.Windows.Controls;
using System.Windows.Input;
using Stock.Core;
using Stock.Core.Domain;
using Stock.Core.Repository;
using Stock.UI.ViewModels.Base;

namespace Stock.UI.ViewModels.Dialogs
{
    public class LoginViewModel : ViewModelBase
    {
        public LoginViewModel()
        {
            ApplicationState.LoadConnectionSettings();

            Action<object> loginAction = LoginMethod;
            LoginCommand = new AsyncCommand(loginAction);
            LoginCommand.RunWorkerCompleted += (s, e) => { if (User != null) LoginAction(); };
            ChangeConnectionCommand = new RelayCommand(x => ChangeConnectionAction());

            var rememberCreditentials = ApplicationState.GetValue<bool>("RememberCreditentials");
            var ldapAuth = ApplicationState.GetValue<bool>("LdapAuth");
            var userId = ApplicationState.GetValue<string>("UserId");

            RememberCreditentials = rememberCreditentials;
            LdapAuth = ldapAuth;
            LoginText = userId;
        }

        public LoginViewModel(Action<string> fillPass) : this()
        {
            var userPassword = ApplicationState.GetValue<string>("UserPassword");
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
        public Action<string> ShowInfoMessage { get; set; }
        private UserAcc User { get; set; }

        private void LoginMethod(object parameter)
        {
            InProgress = true;

            var passwordBox = parameter as PasswordBox;
            if (passwordBox == null) return;
            
            if (!CheckConnection())
            {
                ErrorMessage = "Ошибка подключения к БД.";
                ShowError = true;
                InProgress = false;
                
                return;
            }

            var accountRepository = new AccountRepository();
            var account = accountRepository.GetByLogin(LoginText);
            if (!account.IsNew)
            {
                byte[] hash = GenerateSaltedHash(GetBytes(passwordBox.Password), GetBytes(account.Salt));
                string hashStr = Convert.ToBase64String(hash);

                if (string.Equals(hashStr, account.HashedPassword))
                {
                    var userAccRepository = new Repository<UserAcc>();
                    User = userAccRepository.GetById(account.Id);
                    ApplicationState.SetValue("User", User);
                    InProgress = false;

                    if (RememberCreditentials)
                    {
                        ApplicationState.SetValue("RememberCreditentials", RememberCreditentials);
                        ApplicationState.SetValue("LdapAuth", LdapAuth);
                        ApplicationState.SetValue("UserId", LoginText);
                        ApplicationState.SetValue("UserPassword", passwordBox.Password);
                    }
                    else
                    {
                        ApplicationState.SetValue("RememberCreditentials", false);
                        ApplicationState.SetValue("LdapAuth", false);
                        ApplicationState.SetValue("UserId", String.Empty);
                        ApplicationState.SetValue("UserPassword", String.Empty);
                    }
                    ApplicationState.SaveUserCreditentials();

                    return;
                }
            }
            
            ErrorMessage = "Ошибка. Неверный логин/пароль.";
            ShowError = true;
            InProgress = false;
        }

        private bool CheckConnection()
        {
            var dbDataSource = ApplicationState.GetValue<string>("DbDataSource");
            var dbInitialCatalog = ApplicationState.GetValue<string>("DbInitialCatalog");
            var dbUserId = ApplicationState.GetValue<string>("DbUserId");
            var dbPassword = ApplicationState.GetValue<string>("DbPassword");
            var integratedSecurity = ApplicationState.GetValue<bool>("IntegratedSecurity");

            var result = NHibernateHelper.Configure(dbDataSource, dbInitialCatalog, dbUserId, dbPassword, integratedSecurity);
            if (!result)
                return false;
            
            return true;
        }

        static byte[] GenerateSaltedHash(byte[] plainText, byte[] salt)
        {
            HashAlgorithm algorithm = new SHA256Managed();

            var plainTextWithSaltBytes = new byte[plainText.Length + salt.Length];

            for (int i = 0; i < plainText.Length; i++)
                plainTextWithSaltBytes[i] = plainText[i];
            for (int i = 0; i < salt.Length; i++)
                plainTextWithSaltBytes[plainText.Length + i] = salt[i];

            return algorithm.ComputeHash(plainTextWithSaltBytes);
        }

        static byte[] GetBytes(string str)
        {
            var bytes = new byte[str.Length * sizeof(char)];
            Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        /*
        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var salt = new byte[32];
            RandomNumberGenerator rng = new RNGCryptoServiceProvider();
            rng.GetBytes(salt);

            string saltStr = Convert.ToBase64String(salt);
            byte[] hash = GenerateSaltedHash(GetBytes(PasswordBox.Password), GetBytes(saltStr));

            account.Salt = saltStr;
            account.HashedPassword = Convert.ToBase64String(hash);

            repository.Save(account);
            MessageBox.Show("Success!");
        }

        static string GetString(byte[] bytes)
        {
            var chars = new char[bytes.Length / sizeof(char)];
            Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }
        */
    }
}
