using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Stock.UI.Views;

namespace Stock.UI
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            
            var viewModel = new MainWindowViewModel(ShowDialogMessage, ShowLoginWindow, ShowSettingsDialog);
            DataContext = viewModel;
        }

        private bool ShowLoginWindow()
        {
            var dialog = new Views.Dialogs.LoginView();
            var showDialog = dialog.ShowDialog();
            
            return showDialog != null && (bool) showDialog;
        }

        private void ShowDialogMessage(string s)
        {
            MessageBox.Show(s);
        }

        private void ShowSettingsDialog()
        {
            var dialog = new Views.SettingsView();
            dialog.ShowDialog();
        }

        private DispatcherTimer _timer;

        private void RefreshTab()
        {
            var item = MainTabControl.SelectedItem as TabItem;
            //if (item != null)
            //{
            //    if (item.Name == "StockUnitTab")
            //        StockUnitWindow.Refresh();
            //    if (item.Name == "CardTab")
            //        CardWindow.Refresh();
            //    if (item.Name == "DocumentTab")
            //        DocumentWindow.Refresh();
            //    if (item.Name == "UnitTab")
            //        UnitWindow.Refresh();
            //    if (item.Name == "RepairTab")
            //        RepairWindow.Refresh();
            //    if (item.Name == "OwnerTab")
            //        OwnerWindow.Refresh();
            //    if (item.Name == "StaffTab")
            //        StaffWindow.Refresh();
            //    if (item.Name == "StatusTab")
            //        StatusWindow.Refresh();
            //    if (item.Name == "LogTab")
            //        LogWindow.Refresh();
            //}
        }

        //private void TimerStart()
        //{
        //    _timer = new DispatcherTimer {Interval = new TimeSpan(0, 0, _refreshPeriod)};
        //    _timer.Tick += (s, j) => RefreshTab();
        //    _timer.Start();
        //}

        //private void LoginWindow_LoggedIn(object sender, EventArgs e)
        //{
        //    if (!LoginWindow.LoginStatus) return;

        //    ApplicationState.SetValue("User", LoginWindow.LoggedInUser);
        //    LoginWindow.ClearValues();
        //    LoginWindow.Visibility = Visibility.Hidden;
        //    LoginWindow.IsEnabled = false;

        //    UserPageWindow.Visibility = Visibility.Visible;
        //    UserPageWindow.IsEnabled = true;

        //    var imagePath = _settingsFolder + LoginWindow.LoggedInUser.UserImagePath;
        //    if (File.Exists(imagePath))
        //        UserImage.Source = new BitmapImage(new Uri(_settingsFolder + LoginWindow.LoggedInUser.UserImagePath));

        //    UserNamePresenter.Content = LoginWindow.LoggedInUser.Name.DisplayName;
            
        //    if (_timer != null)
        //        _timer.Stop();
        //    else
        //    {
        //        TimerStart();
        //    }
        //}

        //void UserPageWindow_LoggedOut(object sender, EventArgs e)
        //{
        //    UserPageWindow.Visibility = Visibility.Hidden;
        //    UserPageWindow.IsEnabled = false;

        //    LoginWindow.IsEnabled = true;
        //    LoginWindow.LoginStatus = false;
        //    ApplicationState.SetValue("User", null);
        //    LoginWindow.Visibility = Visibility.Visible;

        //    UserImage.Source = (BitmapImage)FindResource("UserAccBitmapImage");
        //    UserNamePresenter.Content = "Гость";
        //}

        private void MainTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var tabControl = e.Source as TabControl;
            if (tabControl == null) return;

            var tabItem = tabControl.SelectedItem as TabItem;
            if (tabItem == null) return;

            var grid = tabItem.Content as Grid;
            if (grid == null) return;

            var children = grid.Children;
            foreach (var child in children)
            {
                var item = child as ITableView;
                if (item != null) item.Refresh();
            }
        }
    }
}
