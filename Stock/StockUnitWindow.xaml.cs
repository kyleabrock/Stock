using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using Microsoft.Windows.Controls;
using Stock.StockUnitDialogs;
using Core.Domain;
using Core.Repository;

namespace Stock
{
    /// <summary>
    /// Логика взаимодействия для ResourceFlowWindow.xaml
    /// </summary>
    public partial class StockUnitWindow
    {
        public StockUnitWindow()
        {
            InitializeComponent();
            _stockUnitRepository = new StockUnitRepository();
        }

        public void Refresh()
        {
            _fullList = _stockUnitRepository.GetAllFull(false);
            SetItemsToDataGrid(_fullList);
            
            if (OnRefresh != null)
                OnRefresh(this, new EventArgs());
        }
        
        public delegate void OnRefreshHandler(object sender, EventArgs e);
        public event OnRefreshHandler OnRefresh;

        private readonly StockUnitRepository _stockUnitRepository;
        private IList<StockUnit> _fullList;

        private void SetItemsToDataGrid(IList<StockUnit> items)
        {
            var lastSortDescriptions = new SortDescriptionCollection();
            foreach (var item in MainDataGrid.Items.SortDescriptions)
                lastSortDescriptions.Add(item);

            var lastSelectedItem = new StockUnit();
            if (MainDataGrid.SelectedItem != null)
                lastSelectedItem = (StockUnit) MainDataGrid.SelectedItem;

            MainDataGrid.ItemsSource = null;
            MainDataGrid.ItemsSource = items;

            //Sort DataGrid items
            var sortDescriptions = MainDataGrid.Items.SortDescriptions;
            sortDescriptions.Clear();
            foreach (var item in lastSortDescriptions)
            {
                sortDescriptions.Add(item);
                
                //Show sort arrow on the DataGrid
                //MainDataGrid.Columns[sortDirection.Key].SortDirection = sortDirection.Value;
            }

            Filter();
            //Select item
            if (!lastSelectedItem.IsNew)
            {
                MainDataGrid.SelectedItem = lastSelectedItem;
                MainDataGrid.UpdateLayout();
            }

            MainDataGrid.Items.Refresh();
        }
        
        private void Filter()
        {
            var itemSourceList = new CollectionViewSource { Source = _fullList };
            var itemList = itemSourceList.View;
            var filter = new Predicate<object>(StockUnitFilter);
            itemList.Filter = filter;
            
            MainDataGrid.ItemsSource = itemList;
        }

        private bool StockUnitFilter(object obj)
        {
            if (!(obj is StockUnit))
                return false;

            var filterString = SearchTb.Text;
            var right = (StockUnit)obj;

            if (StringContains(right.Status.StatusName, filterString))
                return true;
            if (StringContains(right.StockName, filterString))
                return true;
            if (StringContains(right.StockNumber, filterString))
                return true;
            if (StringContains(right.Card.CardNumber, filterString))
                return true;
            return StringContains(right.Comments, filterString);
        }

        private bool StringContains(string arg, string compareString)
        {
            var culture = CultureInfo.GetCultureInfo("ru-RU");
            return culture.CompareInfo.IndexOf(arg, compareString, CompareOptions.IgnoreCase) >= 0;
        }

        private void DeleteStockUnit(StockUnit item)
        {
            var repository = new StockUnitRepository();
            var stockUnit = repository.GetByIdFull(item.Id);
            if (stockUnit.UnitList != null)
            {
                var unitRepository = new UnitRepository();
                foreach (var unit in stockUnit.UnitList)
                    unitRepository.Delete(unit);
            }

            repository.Delete(item);
        }

        private void AddButton_OnClick(object sender, RoutedEventArgs e)
        {
            var addDialog = new StockUnitAddDialog { Owner = Window.GetWindow(this) };
            addDialog.Closed += (s, j) => Refresh();
            addDialog.ShowDialog();
        }

        private void EditButton_OnClick_OnClick(object sender, RoutedEventArgs e)
        {
            if (MainDataGrid.SelectedItems != null && MainDataGrid.SelectedItems.Count == 1)
            {
                var item = MainDataGrid.SelectedItem as StockUnit;
                if (item != null)
                {
                    var addDialog = new StockUnitAddDialog(item) { Owner = Window.GetWindow(this) };
                    addDialog.Closed += (s, j) => Refresh();
                    addDialog.ShowDialog();
                }
            }
        }

        private void DeleteButton_OnClick(object sender, RoutedEventArgs e)
        {
            var item = MainDataGrid.SelectedItem as StockUnit;
            if (item != null)
            {
                const string caption = "Удаление";
                const string text = "Вы действительно хотите удалить эту запись?\r\n" +
                                    "Все устройства будут удалены.";
                const MessageBoxButton buttons = MessageBoxButton.OKCancel;

                if (MessageBox.Show(text, caption, buttons) == MessageBoxResult.OK)
                {
                    DeleteStockUnit(item);
                    Refresh();
                }
            }
        }

        private void MainDataGrid_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var element = e.MouseDevice.DirectlyOver as FrameworkElement;
            if (element != null && element.Parent is DataGridCell)
            {
                var datagrid = sender as DataGrid;
                if (datagrid != null)
                    EditButton.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
            }
        }

        private void SearchButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(SearchTb.Text))
                Filter();
            else
            {
                Refresh();
            }
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            Refresh();
        }
        
        private void FilterButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (FilterRow.Height.IsAbsolute && FilterRow.Height.Value == 0)
                FilterRow.Height = new GridLength(64, GridUnitType.Pixel);
            else
            {
                FilterRow.Height = new GridLength(0, GridUnitType.Pixel);
            }
        }
    }
}
