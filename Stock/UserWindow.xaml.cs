using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using Stock.UserDialogs;
using Core.Domain;
using Core.Repository;
using Microsoft.Windows.Controls;

namespace Stock
{
    /// <summary>
    /// Логика взаимодействия для ResourceFlowWindow.xaml
    /// </summary>
    public partial class UserWindow
    {
        public UserWindow()
        {
            InitializeComponent();
            _repository = new Repository<UserAcc>();
        }

        public void Refresh()
        {
            _fullList = _repository.GetAll();
            SetItemsToDataGrid(_fullList);
        }

        private readonly Repository<UserAcc> _repository;
        private IList<UserAcc> _fullList;

        private void SetItemsToDataGrid(IList<UserAcc> items)
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

            var selectedItem = new UserAcc();
            if (MainDataGrid.SelectedItem != null)
                selectedItem = (UserAcc)MainDataGrid.SelectedItem;

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
            var filter = new Predicate<object>(UserFilter);
            itemList.Filter = filter;

            MainDataGrid.ItemsSource = itemList;
        }

        private bool UserFilter(object obj)
        {
            if (!(obj is UserAcc))
                return false;

            var filterString = SearchTb.Text;
            var right = (UserAcc)obj;

            if (StringContains(right.Name.DisplayName, filterString))
                return true;
            if (StringContains(right.Department, filterString))
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
            var addDialog = new UserAddView { Owner = Window.GetWindow(this) };
            addDialog.Closed += (s, j) => Refresh();
            addDialog.ShowDialog();
        }

        private void EditButton_OnClick_OnClick(object sender, RoutedEventArgs e)
        {
            if (MainDataGrid.SelectedItems != null && MainDataGrid.SelectedItems.Count == 1)
            {
                var item = MainDataGrid.SelectedItem as UserAcc;
                if (item != null)
                {
                    var addDialog = new UserAddView(item) { Owner = Window.GetWindow(this) };
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
            var item = MainDataGrid.SelectedItem as UserAcc;
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
    