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
            if (ViewModel.SelectFolderAction == null)
                ViewModel.SelectFolderAction = OpenFolderDialog;
        }

        private void OpenFolderDialog()
        {
            var dialog = new FolderBrowserDialog();
            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.OK)
                ViewModel.SettingsAppFolderPath = dialog.SelectedPath;
        }
    }
}
