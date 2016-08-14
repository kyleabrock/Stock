using System;
using System.Windows;

namespace Stock.UI.Views
{
    /// <summary>
    /// Логика взаимодействия для ResourceFlowWindow.xaml
    /// </summary>
    public partial class UserPageView
    {
        public UserPageView()
        {
            InitializeComponent();
            if (ViewModel.Logout == null)
                ViewModel.Logout = OnRaiseLoggedOutEvent;
        }

        public event EventHandler LoggedOut;

        protected virtual void OnRaiseLoggedOutEvent()
        {
            EventHandler handler = LoggedOut;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        private void UserPageView_OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (ViewModel.SetUserCommand != null)
                ViewModel.SetUserCommand.Execute(null);
        }
    }
}
