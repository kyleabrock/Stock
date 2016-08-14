using System;
using System.Security.Cryptography;
using System.Windows;
using Stock.Core.Domain;
using Stock.Core.Repository;

namespace Stock.UI.Views
{
    /// <summary>
    /// Логика взаимодействия для ResourceFlowWindow.xaml
    /// </summary>
    public partial class LoginView
    {
        public LoginView()
        {
            InitializeComponent();
            LoginTextBox.Focus();
        }

        public bool LoginStatus { get; set; }
        public UserAcc LoggedInUser { get; set; }

        public void ClearValues()
        {
            LoginTextBox.Clear();
            PasswordBox.Clear();
        }

        public event EventHandler LoggedIn;

        protected virtual void OnRaiseLoggedInEvent()
        {
            EventHandler handler = LoggedIn;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        private bool CheckConnection()
        {
            if (!Core.NHibernateHelper.TestConnection())
            {
                string errorText = "Ошибка при подключении к базе данных.\r\nПодробные сведения об ошибке: ";
                errorText += Core.NHibernateHelper.LastError;

                MessageBox.Show(Application.Current.MainWindow, errorText, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            if (!CheckConnection()) return;

            var accountRepository = new AccountRepository();
            var account = accountRepository.GetByLogin(LoginTextBox.Text);

            //var salt = new byte[32];
            //RandomNumberGenerator rng = new RNGCryptoServiceProvider();
            //rng.GetBytes(salt);

            //string saltStr = Convert.ToBase64String(salt);
            //byte[] hash = GenerateSaltedHash(GetBytes(PasswordBox.Password), GetBytes(saltStr));

            //account.Salt = saltStr;
            //account.HashedPassword = Convert.ToBase64String(hash);

            //repository.Save(account);
            //MessageBox.Show("Success!");

            if (!account.IsNew)
            {
                byte[] hash = GenerateSaltedHash(GetBytes(PasswordBox.Password), GetBytes(account.Salt));
                string hashStr = Convert.ToBase64String(hash);

                if (string.Equals(hashStr, account.HashedPassword))
                {
                    LoginStatus = true;
                    var userAccRepository = new Repository<UserAcc>();
                    LoggedInUser = userAccRepository.GetById(account.Id);
                }
                else
                {
                    MessageBox.Show("Неверный логин или пароль!");
                }

                OnRaiseLoggedInEvent();
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль!");
            }
        }

        static byte[] GenerateSaltedHash(byte[] plainText, byte[] salt)
        {
            HashAlgorithm algorithm = new SHA256Managed();

            var plainTextWithSaltBytes = new byte[plainText.Length + salt.Length];

            for (int i = 0; i < plainText.Length; i++)
            {
                plainTextWithSaltBytes[i] = plainText[i];
            }
            for (int i = 0; i < salt.Length; i++)
            {
                plainTextWithSaltBytes[plainText.Length + i] = salt[i];
            }

            return algorithm.ComputeHash(plainTextWithSaltBytes);
        }

        static byte[] GetBytes(string str)
        {
            var bytes = new byte[str.Length * sizeof(char)];
            Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        static string GetString(byte[] bytes)
        {
            var chars = new char[bytes.Length / sizeof(char)];
            Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }
    }
}
