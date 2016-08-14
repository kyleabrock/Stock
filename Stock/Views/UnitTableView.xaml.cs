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
    public partial class UnitTableView
    {
        public UnitTableView()
        {
            InitializeComponent();
            if (ViewModel.OpenStockUnitAction == null)
                ViewModel.OpenStockUnitAction = OpenStockUnit;
            if (ViewModel.OpenCardAction == null)
                ViewModel.OpenCardAction = OpenCard;
        }

        public void Refresh()
        {
            if (RefreshButton.Command.CanExecute(null))
                RefreshButton.Command.Execute(null);
        }

        private void OpenStockUnit()
        {
            var item = ViewModel.SelectedItem as Unit;
            if (item == null) return;

            var stockUnit = item.StockUnit;
            if (stockUnit != null)
            {
                var dialog = new StockUnitAddView(stockUnit) {Owner = Window.GetWindow(this)};
                dialog.Closed += (s, e) => dialog.Owner.Focus();
                dialog.Show();
            }
        }

        private void OpenCard()
        {
            var item = ViewModel.SelectedItem as Unit;
            if (item == null) return;

            var stockUnit = item.StockUnit;
            if (stockUnit != null)
            {
                int cardId = stockUnit.Card.Id;
                var dialog = new CardAddView(cardId) { Owner = Window.GetWindow(this) };
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
                {
                    if (null != ToStockUnitButton.Command)
                        ToStockUnitButton.Command.Execute(null);
                }
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
    }
}
    