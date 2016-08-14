using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using Stock.Core.Domain;
using Stock.Core.Filter;
using Stock.Core.Repository;
using Stock.UI.ViewModels.Base;

namespace Stock.UI.ViewModels
{
    public class CardTableViewModel : TableNavigationViewModel<Card>
    {
        //TODO: IsSearched results
        public CardTableViewModel()
        {
            InitViewModel();
        }

        private IEnumerable<Staff> _filterStaffList;
        public IEnumerable<Staff> FilterStaffList
        {
            get { return _filterStaffList; }
            set { _filterStaffList = value; OnPropertyChanged("FilterStaffList"); }
        }

        private IEnumerable<string> _filterDepartmentList;
        public IEnumerable<string> FilterDepartmentList
        {
            get { return _filterDepartmentList; }
            set { _filterDepartmentList = value; OnPropertyChanged("FilterDepartmentList"); }
        }

        private CardRepository _cardRepository;
        private bool _initFilterStatus = false;

        private void InitViewModel()
        {
            _cardRepository = new CardRepository();

            AddCommand = new RelayCommand(x => AddMethod());
            EditCommand = new RelayCommand(x => EditMethod());
            DeleteCommand = new RelayCommand(x => DeleteMethod());
            RefreshCommand = new AsyncCommand(x => RefreshMethod());
            RefreshCommand.RunWorkerCompleted += RefreshCommand_RunWorkerCompleted;
            
            ComplexFilter = new CardFilter();
        }

        private void InitFilter()
        {
            var staffRepository = new StaffRepository();
            FilterDepartmentList = staffRepository.GetDepartments();
            FilterStaffList = staffRepository.GetAll(staff => staff.Name.DisplayName);

            _initFilterStatus = true;
        }

        private void RefreshCommand_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!_initFilterStatus)
                InitFilter();

            SaveTableSortOrder();

            if (!IsSearched)
                TableItemListView = CollectionViewSource.GetDefaultView(TableItemList);
            else
            {
                //Get all units
                var itemList = CollectionViewSource.GetDefaultView(TableItemList);
                var filter = new Predicate<object>(FilterItems);
                itemList.Filter = filter;
                TableItemListView = itemList;
            }

            LoadTableSortOrder();
        }

        private void RefreshMethod()
        {
            TableItemList = ComplexFilterStatus ? 
                _cardRepository.GetAllByComplexFilter(ComplexFilter) 
                : _cardRepository.GetAll(x=> x.CardNumber, false, false);
        }

        private bool FilterItems(object obj)
        {
            if (!(obj is Card))
                return false;

            var filterString = SearchString;
            var right = (Card)obj;

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

        private void AddMethod()
        {
            AddAction();
        }

        private void EditMethod()
        {
            EditAction();
        }

        private void DeleteMethod()
        {
            var item = SelectedItem as Card;
            if (item != null)
            {
                const string caption = "Удаление";
                const string text = "Вы действительно хотите удалить эту запись?\r\n" +
                                    "Все устройства будут удалены.";
                const MessageBoxButton buttons = MessageBoxButton.OKCancel;

                if (MessageBox.Show(text, caption, buttons) == MessageBoxResult.OK)
                {
                    DeleteCard(item);
                    if (RefreshCommand != null)
                        RefreshCommand.Execute(null);
                }
            }
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
    }
}
