namespace Stock.UI.Views
{
    /// <summary>
    /// Логика взаимодействия для ResourceFlowWindow.xaml
    /// </summary>
    public partial class LogTableView
    {
        public LogTableView()
        {
            InitializeComponent();
        }

        public void Refresh()
        {
            if (RefreshButton.Command.CanExecute(null))
                RefreshButton.Command.Execute(null);
        }
    }
}
