using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Stock.UI.Properties;

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
            BlockButtons(true);

            _settingsFolder = (string)Settings.Default["SettingsAppFolder"];
            _refreshPeriod = (int)Settings.Default["RefreshPeriod"];

            ApplicationState.SetValue("TemplatesFolderPath", Settings.Default["TemplatesFolderPath"]);
            ApplicationState.SetValue("ExportFolderPath", Settings.Default["ExportFolderPath"]);

            LoginWindow.LoggedIn += LoginWindow_LoggedIn;
            UserPageWindow.LoggedOut += UserPageWindow_LoggedOut;
        }

        private readonly string _settingsFolder;
        private readonly int _refreshPeriod;

        private void BlockButtons(bool block)
        {
            StockUnitTab.IsEnabled = !block;
            CardTab.IsEnabled = !block;
            DocumentTab.IsEnabled = !block;
            UnitTab.IsEnabled = !block;
            RepairTab.IsEnabled = !block;
            OwnerTab.IsEnabled = !block;
            StaffTab.IsEnabled = !block;
            StatusTab.IsEnabled = !block;
            LogTab.IsEnabled = !block;
        }

        private DispatcherTimer _timer;

        private void RefreshTab()
        {
            var item = MainTabControl.SelectedItem as TabItem;
            if (item != null)
            {
                if (item.Name == "StockUnitTab")
                    StockUnitWindow.Refresh();
                if (item.Name == "CardTab")
                    CardWindow.Refresh();
                if (item.Name == "DocumentTab")
                    DocumentWindow.Refresh();
                if (item.Name == "UnitTab")
                    UnitWindow.Refresh();
                if (item.Name == "RepairTab")
                    RepairWindow.Refresh();
                if (item.Name == "OwnerTab")
                    OwnerWindow.Refresh();
                if (item.Name == "StaffTab")
                    StaffWindow.Refresh();
                if (item.Name == "StatusTab")
                    StatusWindow.Refresh();
                if (item.Name == "LogTab")
                    LogWindow.Refresh();
            }
        }

        private void TimerStart()
        {
            _timer = new DispatcherTimer {Interval = new TimeSpan(0, 0, _refreshPeriod)};
            _timer.Tick += (s, j) => RefreshTab();
            _timer.Start();
        }

        private void LoginWindow_LoggedIn(object sender, EventArgs e)
        {
            if (!LoginWindow.LoginStatus) return;

            ApplicationState.SetValue("User", LoginWindow.LoggedInUser);
            LoginWindow.ClearValues();
            LoginWindow.Visibility = Visibility.Hidden;
            LoginWindow.IsEnabled = false;

            UserPageWindow.Visibility = Visibility.Visible;
            UserPageWindow.IsEnabled = true;

            var imagePath = _settingsFolder + LoginWindow.LoggedInUser.UserImagePath;
            if (File.Exists(imagePath))
                UserImage.Source = new BitmapImage(new Uri(_settingsFolder + LoginWindow.LoggedInUser.UserImagePath));

            UserNamePresenter.Content = LoginWindow.LoggedInUser.Name.DisplayName;

            BlockButtons(false);
            
            if (_timer != null)
                _timer.Stop();
            else
            {
                TimerStart();
            }
        }

        void UserPageWindow_LoggedOut(object sender, EventArgs e)
        {
            UserPageWindow.Visibility = Visibility.Hidden;
            UserPageWindow.IsEnabled = false;

            LoginWindow.IsEnabled = true;
            LoginWindow.LoginStatus = false;
            ApplicationState.SetValue("User", null);
            LoginWindow.Visibility = Visibility.Visible;

            UserImage.Source = (BitmapImage)FindResource("UserAccBitmapImage");
            UserNamePresenter.Content = "Гость";

            BlockButtons(true);
        }

        private void MainTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl)
                RefreshTab();
        }
    }
}
