using System;
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
        }

        private string OpenFolderDialog()
        {
            var dialog = new FolderBrowserDialog();
            DialogResult result = dialog.ShowDialog();
            
            return String.Empty;
            //return result == DialogResult.OK ? dialog.SelectedPath : null;
        }
    }
}
