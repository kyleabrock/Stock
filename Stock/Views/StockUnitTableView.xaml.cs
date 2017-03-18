using System;
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
    public partial class StockUnitTableView : ITableView
    {
        public StockUnitTableView()
        {
            InitializeComponent();
            SetActions();
        }

        public void Refresh()
        {
            if (RefreshButton.Command.CanExecute(null))
                RefreshButton.Command.Execute(null);
        }

        private void SetActions()
        {
            if (ViewModel.AddAction == null)
                ViewModel.AddAction = DisplayAddDialog;
            if (ViewModel.EditAction == null)
                ViewModel.EditAction = DisplayEditDialog;
            if (ViewModel.CopyAction == null)
                ViewModel.CopyAction = DisplayCopyDialog;
            if (ViewModel.ShowCardAction == null)
                ViewModel.ShowCardAction = ShowCard;
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

        private void ShowCard(Card card)
        {
            var dialog = new CardAddView(card.Id) { Owner = Window.GetWindow(this) };
            dialog.Closed += (s, e) => dialog.Owner.Focus();
            dialog.Show();
        }

        private void DisplayCopyDialog(StockUnit stockUnit)
        {
            var dialog = new StockUnitAddView(stockUnit) { Owner = Window.GetWindow(this) };
            dialog.Closed += (s, e) => dialog.Owner.Focus();
            dialog.Show();
        }

        private void DisplayAddDialog()
        {
            var dialog = new StockUnitAddView { Owner = Window.GetWindow(this) };
            dialog.Closed += (s, e) => dialog.Owner.Focus();
            dialog.Show();
        }

        private void DisplayEditDialog()
        {
            var item = ViewModel.SelectedItem as StockUnit;
            if (item != null)
            {
                var dialog = new StockUnitAddView(item) { Owner = Window.GetWindow(this) };
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
        
        private void FilterButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (FilterGridColumn.Width.IsAbsolute && Math.Abs(FilterGridColumn.Width.Value) <= 0.0)
                FilterGridColumn.Width = new GridLength(1, GridUnitType.Auto);
            else
            {
                FilterGridColumn.Width = new GridLength(0, GridUnitType.Pixel);
            }
        }

        private void DataGrid_OnCopyingRowClipboardContent(object sender, DataGridRowClipboardEventArgs e)
        {
            //e.ClipboardRowContent.Clear();
            //var cells = DataGrid.SelectedCells;
        }
    }
}
