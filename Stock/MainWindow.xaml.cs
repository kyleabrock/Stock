using System;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Stock
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            TimerStart();
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
                if (item.Name == "OwnerTab")
                    OwnerWindow.Refresh();
                if (item.Name == "StaffTab")
                    StaffWindow.Refresh();
                if (item.Name == "UserTab")
                    UserWindow.Refresh();
                if (item.Name == "StatusTab")
                    StatusWindow.Refresh();
            }
        }

        private void TimerStart()
        {
            _timer = new DispatcherTimer {Interval = new TimeSpan(0, 1, 0)};
            _timer.Tick += (s, j) => RefreshTab();
            _timer.Start();
        }

        private void MainTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl)
            {
                RefreshTab();
            }
        }
    }
}
