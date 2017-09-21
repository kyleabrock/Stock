using System.Windows;
using System.Windows.Input;
using Microsoft.Windows.Controls;
using Stock.Core.Domain;
using Stock.UI.Views.Dialogs;

namespace Stock.UI.Views
{
    /// <summary>
    /// Логика взаимодействия для ResourceFlowWindow.xaml
    /// </summary>
    public partial class StatusTableView : ITableView
    {
        public StatusTableView()
        {
            InitializeComponent();
            SetActions();
        }

        public void Refresh()
        {
            if (RefreshButton.Command.CanExecute(null))
                RefreshButton.Command.Execute(null);

            if (!_settingsLoaded)
                LoadSettings();
            else
            {
                SaveSettings();
            }
        }

        private bool _settingsLoaded = false;

        private void SetActions()
        {
            if (ViewModel.AddAction == null)
                ViewModel.AddAction = DisplayAddDialog;
            if (ViewModel.EditAction == null)
                ViewModel.EditAction = DisplayEditDialog;
            if (ViewModel.ShowInfoMessage == null)
                ViewModel.ShowInfoMessage = (text, caption) => MessageBox.Show(text, caption);
            if (ViewModel.ShowDialogMessage == null)
            {
                const MessageBoxButton buttons = MessageBoxButton.OKCancel;
                const MessageBoxResult result = MessageBoxResult.OK;
                ViewModel.ShowDialogMessage =
                    (text, caption) => MessageBox.Show(text, caption, buttons) == result;
            }
        }

        private void DisplayAddDialog()
        {
            var dialog = new StatusAddView { Owner = Window.GetWindow(this) };
            dialog.Closed += (s, e) => dialog.Owner.Focus();
            dialog.Show();
        }

        private void DisplayEditDialog()
        {
            var item = ViewModel.SelectedItem as Status;
            if (item != null)
            {
                var dialog = new StatusAddView(item) { Owner = Window.GetWindow(this) };
                dialog.Closed += (s, e) => dialog.Owner.Focus();
                dialog.Show();
            }
        }

        private void MainDataGrid_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var element = e.MouseDevice.DirectlyOver as FrameworkElement;
            if (element != null && element.Parent is DataGridCell)
            {
                var datagrid = sender as DataGrid;
                if (datagrid != null)
                    DisplayEditDialog();
            }
        }

        private void LoadSettings()
        {
            for (int i = 0; i < DataGrid.Columns.Count; i++)
            {
                var columnWidth = string.Format("StatusTableColumn{0}Width", i);
                DataGrid.Columns[i].Width =
                    new DataGridLength(AppSettings.GetAsDouble(columnWidth));

                var columnDisplayIndex = string.Format("StatusTableColumn{0}DisplayIndex", i);
                DataGrid.Columns[i].DisplayIndex = AppSettings.GetAsInt(columnDisplayIndex);
            }

            _settingsLoaded = true;
        }

        private void SaveSettings()
        {
            for (int i = 0; i < DataGrid.Columns.Count; i++)
            {
                var columnWidth = string.Format("StatusTableColumn{0}Width", i);
                AppSettings.SetValue(columnWidth, DataGrid.Columns[i].Width.Value);

                var columnDisplayIndex = string.Format("StatusTableColumn{0}DisplayIndex", i);
                if (DataGrid.Columns[i].DisplayIndex != -1)
                    AppSettings.SetValue(columnDisplayIndex, DataGrid.Columns[i].DisplayIndex);
            }

            AppSettings.Save();
        }

        private void StatusTableView_OnLoaded(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this);
            window.Closing += (s, j) => SaveSettings();
        }
    }
}
    