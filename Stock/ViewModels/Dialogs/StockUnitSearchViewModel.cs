using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Windows.Input;
using Stock.Core.Domain;
using Stock.Core.Repository;
using Stock.UI.ViewModels.Base;

namespace Stock.UI.ViewModels.Dialogs
{
    public class StockUnitSearchViewModel : TableSearchViewModel<StockUnit>
    {
        public StockUnitSearchViewModel()
        {
            _card = new Card();
            InitViewModel();
        }

        public StockUnitSearchViewModel(Card card)
        {
            _card = card;
            InitViewModel();
        }

        public override string SearchString
        {
            get { return base.SearchString; }
            set
            {
                base.SearchString = value;
                IsSearched = value.Length > SearchStringMininumLength;
                RefreshMethod();
            }
        }

        public IList<StockUnit> Result { get; private set; }

        private ObservableCollection<object> _selectedItems;
        public ObservableCollection<object> SelectedItems
        {
            get { return _selectedItems; }
            set 
            { 
                _selectedItems = value;
                OnPropertyChanged("SelectedItems");
            }
        }

        public ICommand OkCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public Action CloseAction { get; set; }
        public Action GetSelectedItemsAction { get; set; }

        private Card _card;
        private StockUnitRepository _stockUnitRepository;

        private void InitViewModel()
        {
            _stockUnitRepository = new StockUnitRepository();
            Result = new List<StockUnit>();

            OkCommand = new RelayCommand(x => OkMethod());
            CancelCommand = new RelayCommand(x => CancelMethod());

            TableItemList = _card.IsNew
                ? new ObservableCollection<StockUnit>(_stockUnitRepository.GetAll(x => x.StockNumber, true, false))
                : new ObservableCollection<StockUnit>(_card.StockUnitList);

            RefreshMethod();
        }

        private void RefreshMethod()
        {
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
        }

        private bool FilterItems(object obj)
        {
            if (!(obj is StockUnit))
                return false;

            var filterString = SearchString;
            var right = (StockUnit)obj;

            if (StringContains(right.StockName, filterString))
                return true;
            return StringContains(right.StockNumber, filterString);
        }

        private void OkMethod()
        {
            GetSelectedItemsAction();
            if (SelectedItems.Count > 0)
            {
                foreach (var item in SelectedItems)
                {
                    var result = item as StockUnit;
                    Result.Add(result);
                }
            }
            CloseAction();
        }

        private void CancelMethod()
        {
            CloseAction();
        }
    }
}
