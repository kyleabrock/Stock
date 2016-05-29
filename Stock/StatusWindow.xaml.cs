using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using Stock.StatusDialogs;
using Core.Domain;
using Core.Repository;
using Microsoft.Windows.Controls;

namespace Stock
{
    /// <summary>
    /// Логика взаимодействия для ResourceFlowWindow.xaml
    /// </summary>
    public partial class StatusWindow
    {
        public StatusWindow()
        {
            InitializeComponent();
            _repository = new Repository<Status>();
        }

        public void Refresh()
        {
            _fullList = _repository.GetAll();
            SetItemsToDataGrid(_fullList);
        }

        private readonly Repository<Status> _repository;
        private IList<Status> _fullList;

        private void SetItemsToDataGrid(IList<Status> items)
        {
            var columnsSortDirections = new List<KeyValuePair<int, ListSortDirection>>();
            var columns = MainDataGrid.Columns;
            for (int i = 0; i < columns.Count; i++)
            {
                var listSortDirection = columns[i].SortDirection;
                if (listSortDirection != null)
                {
                    var sortDirection = (ListSortDirection)listSortDirection;
                    columnsSortDirections.Add(new KeyValuePair<int, ListSortDirection>(i, sortDirection));
                }
            }

            var selectedItem = new Status();
            if (MainDataGrid.SelectedItem != null)
                selectedItem = (Status)MainDataGrid.SelectedItem;

            MainDataGrid.ItemsSource = null;
            MainDataGrid.ItemsSource = items;

            MainDataGrid.Items.SortDescriptions.Clear();
            foreach (var sortDirection in columnsSortDirections)
            {
                MainDataGrid.Items.SortDescriptions.Add(new SortDescription(MainDataGrid.Columns[sortDirection.Key].SortMemberPath, sortDirection.Value));
                MainDataGrid.Columns[sortDirection.Key].SortDirection = sortDirection.Value;
            }

            if (!selectedItem.IsNew)
            {
                MainDataGrid.SelectedItem = selectedItem;
                MainDataGrid.UpdateLayout();
            }

            MainDataGrid.Items.Refresh();
        }

        private void Filter()
        {
            var itemSourceList = new CollectionViewSource { Source = _fullList };
            var itemList = itemSourceList.View;
            var filter = new Predicate<object>(StatusFilter);
            itemList.Filter = filter;

            MainDataGrid.ItemsSource = itemList;
        }

        private bool StatusFilter(object obj)
        {
            if (!(obj is Status))
                return false;

            var filterString = SearchTb.Text;
            var right = (Status)obj;

            if (StringContains(right.StatusName, filterString))
                return true;
            return StringContains(right.Comments, filterString);
        }

        private bool StringContains(string arg, string compareString)
        {
            var culture = CultureInfo.GetCultureInfo("ru-RU");
            return culture.CompareInfo.IndexOf(arg, compareString, CompareOptions.IgnoreCase) >= 0;
        }

        private void AddButton_OnClick(object sender, RoutedEventArgs e)
        {
            var addDialog = new StatusAddView { Owner = Window.GetWindow(this) };
            addDialog.Closed += (s, j) => Refresh();
            addDialog.ShowDialog();
        }

        private void EditButton_OnClick_OnClick(object sender, RoutedEventArgs e)
        {
            if (MainDataGrid.SelectedItems != null && MainDataGrid.SelectedItems.Count == 1)
            {
                var item = MainDataGrid.SelectedItem as Status;
                if (item != null)
                {
                    var addDialog = new StatusAddView(item) { Owner = Window.GetWindow(this) };
                    addDialog.Closed += (s, j) => Refresh();
                    addDialog.ShowDialog();
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

        private void DeleteButton_OnClick(object sender, RoutedEventArgs e)
        {
            var item = MainDataGrid.SelectedItem as Status;
            if (item != null)
            {
                const string caption = "Удаление";
                const string text = "Вы действительно хотите удалить эту запись?\r\n" +
                                    "Запись будет исключена из всех связанных записей.";
                const MessageBoxButton buttons = MessageBoxButton.OKCancel;

                if (MessageBox.Show(text, caption, buttons) == MessageBoxResult.OK)
                {
                    _repository.Delete(item);
                    Refresh();
                }
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
    }
}
    