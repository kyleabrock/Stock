using System.Windows.Forms;

namespace Stock.UI.Views
{
    /// <summary>
    /// Логика взаимодействия для ResourceFlowWindow.xaml
    /// </summary>
    public partial class SettingsView
    {
        public SettingsView()
        {
            InitializeComponent();
            if (ViewModel.SelectFolderFunc == null)
                ViewModel.SelectFolderFunc = OpenFolderDialog;
            if (ViewModel.CloseAction == null)
                ViewModel.CloseAction = Close;
        }

        private string OpenFolderDialog()
        {
            var dialog = new FolderBrowserDialog();
            DialogResult result = dialog.ShowDialog();
            
            return result == System.Windows.Forms.DialogResult.OK ? dialog.SelectedPath : null;
        }
    }
}
