using System.Windows;
using Stock.UI.ViewModels.Dialogs;

namespace Stock.UI.Views.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для ConnectionView.xaml
    /// </summary>
    public partial class ConnectionView
    {
        public ConnectionView()
        {
            InitializeComponent();
            ViewModel = new ConnectionViewModel(s => { PasswordBox.Password = s; });
            DataContext = ViewModel;

            if (ViewModel.CloseAction == null)
                ViewModel.CloseAction = Close;
            if (ViewModel.ShowInfoMessage == null)
                ViewModel.ShowInfoMessage = s => MessageBox.Show(s);
        }
    }
}
