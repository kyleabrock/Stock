using Stock.UI.ViewModels.Dialogs;

namespace Stock.UI.Views.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для LoginView.xaml
    /// </summary>
    public partial class LoginView
    {
        public LoginView()
        {
            InitializeComponent();
            ViewModel = new LoginViewModel(s => { LoginPasswordBox.Password = s; });
            DataContext = ViewModel;

            if (ViewModel.ChangeConnectionAction == null)
                ViewModel.ChangeConnectionAction = ChangeConnectionAction;
            if (ViewModel.LoginAction == null)
                ViewModel.LoginAction = SussessLogin;
        }

        private void ChangeConnectionAction()
        {
            var connection = new ConnectionView {Owner = this};
            connection.Show();
        }

        private void SussessLogin()
        {
            DialogResult = true;
            Close();
        }
    }
}