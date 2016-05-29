using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using Core.Domain;
using Core.Repository;
using Microsoft.Windows.Controls;
using Stock.CardDialogs;

namespace Stock
{
    /// <summary>
    /// Логика взаимодействия для ResourceFlowWindow.xaml
    /// </summary>
    public partial class CardWindow
    {
        public CardWindow()
        {
            InitializeComponent();
            _cardRepository = new CardRepository();
        }

        public void Refresh()
        {
            _fullList = _cardRepository.GetAllFull(false);
            SetItemsToDataGrid(_fullList);
            
            if (OnRefresh != null)
                OnRefresh(this, new EventArgs());
        }

        public int NotCompletedItems { get; set; }

        public delegate void OnRefreshHandler(object sender, EventArgs e);
        public event OnRefreshHandler OnRefresh;

        private readonly CardRepository _cardRepository;
        private IList<Card> _fullList;

        private void SetItemsToDataGrid(IList<Card> items)
        {
            var lastSortDescriptions = new SortDescriptionCollection();
            foreach (var item in MainDataGrid.Items.SortDescriptions)
                lastSortDescriptions.Add(item);

            var lastSelectedItem = new Card();
            if (MainDataGrid.SelectedItem != null)
                lastSelectedItem = (Card)MainDataGrid.SelectedItem;

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
            var filter = new Predicate<object>(CardFilter);
            itemList.Filter = filter;

            MainDataGrid.ItemsSource = itemList;
        }

        private bool CardFilter(object obj)
        {
            if (!(obj is Card))
                return false;

            var filterString = SearchTb.Text;
            var right = (Card) obj;

            if (StringContains(right.CardNumber, filterString))
                return true;
            if (StringContains(right.CardName, filterString))
                return true;
            if (StringContains(right.CreationDate.ToShortDateString(), filterString))
                return true;
            if (StringContains(right.Staff.Name.DisplayName, filterString))
                return true;
            return StringContains(right.Comments, filterString);
        }

        private bool StringContains(string arg, string compareString)
        {
            var culture = CultureInfo.GetCultureInfo("ru-RU");
            return culture.CompareInfo.IndexOf(arg, compareString, CompareOptions.IgnoreCase) >= 0;
        }

        private void DeleteCard(Card item)
        {
            var repository = new CardRepository();
            var card = repository.GetById(item.Id);
            var defaultCard = repository.GetDefaultCard();
            if (card.StockUnitList != null)
            {
                var stockUnitRepository = new StockUnitRepository();
                foreach (var stockUnit in card.StockUnitList)
                {
                    stockUnit.Card = defaultCard;
                    stockUnitRepository.Update(stockUnit);
                }
            }

            repository.Delete(item);
        }

        private void AddButton_OnClick(object sender, RoutedEventArgs e)
        {
            var addDialog = new CardAddView { Owner = Window.GetWindow(this) };
            addDialog.Closed += (s, j) => Refresh();
            addDialog.ShowDialog();
        }

        private void EditButton_OnClick_OnClick(object sender, RoutedEventArgs e)
        {
            if (MainDataGrid.SelectedItems != null && MainDataGrid.SelectedItems.Count == 1)
            {
                var item = MainDataGrid.SelectedItem as Card;
                if (item != null)
                {
                    var addDialog = new CardAddView(item) { Owner = Window.GetWindow(this) };
                    addDialog.Closed += (s, j) => Refresh();
                    addDialog.ShowDialog();
                }
            }
        }

        private void DeleteButton_OnClick(object sender, RoutedEventArgs e)
        {
            var item = MainDataGrid.SelectedItem as Card;
            if (item != null)
            {
                const string caption = "Удаление";
                const string text = "Вы действительно хотите удалить эту запись?\r\n" +
                                    "Запись будет исключена из всех связанных записей.\r\n" +
                                    "Устройства будут перемещены в карточку по-умолчанию.";
                const MessageBoxButton buttons = MessageBoxButton.OKCancel;

                if (MessageBox.Show(text, caption, buttons) == MessageBoxResult.OK)
                {
                    DeleteCard(item);
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
    }
}
