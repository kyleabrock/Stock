using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using Stock.Core.Domain;
using Stock.Core.Filter;
using Stock.Core.Finder;
using Stock.Core.Repository;
using Stock.UI.ViewModels.Base;

namespace Stock.UI.ViewModels
{
    public class StockUnitTableViewModel : TableNavigationViewModel<StockUnit>
    {
        //TODO: IsSearched results
        public StockUnitTableViewModel()
        {
            InitViewModel();
        }

        public IEnumerable<Status> FilterStatusList 
        {
            get { return _filterStatusList; }
            set { _filterStatusList = value; OnPropertyChanged("FilterStatusList"); }
        }

        public IEnumerable<Owner> FilterOwnerList
        {
            get { return _filterOwnerList; }
            set { _filterOwnerList = value; OnPropertyChanged("FilterOwnerList"); }
        }

        public IEnumerable<Card> FilterCardList
        {
            get { return _filterCardList; }
            set { _filterCardList = value; OnPropertyChanged("FilterCardList"); }
        }

        private StockUnitRepository _stockUnitRepository;
        private IEnumerable<Status> _filterStatusList;
        private IEnumerable<Owner> _filterOwnerList;
        private IEnumerable<Card> _filterCardList;
        private bool _initFilterStatus = false;
        
        private void InitViewModel()
        {
            _stockUnitRepository = new StockUnitRepository();
            
            AddCommand = new RelayCommand(x => AddMethod());
            EditCommand = new RelayCommand(x => EditMethod());
            DeleteCommand = new RelayCommand(x => DeleteMethod());
            RefreshCommand = new AsyncCommand(x => RefreshMethod());
            RefreshCommand.RunWorkerCompleted += RefreshCommand_RunWorkerCompleted;
            
            ComplexFilter = new StockUnitFilter();
        }

        private void InitFilter()
        {
            var statusRepository = new StatusRepository();
            var ownerRepository = new OwnerRepository();
            var cardRepository = new CardRepository();

            FilterStatusList = statusRepository.GetAll(status => status.StatusType);
            FilterOwnerList = ownerRepository.GetAll(owner => owner.Name.DisplayName);
            FilterCardList = cardRepository.GetAll();

            _initFilterStatus = true;
        }

        private void RefreshCommand_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!_initFilterStatus)
                InitFilter();

            SaveTableSortOrder();
            TableItemListView = CollectionViewSource.GetDefaultView(TableItemList);
            LoadTableSortOrder();
        }

        private void RefreshMethod()
        {
            if (!IsSearched)
            {
                TableItemList = ComplexFilterStatus
                    ? _stockUnitRepository.GetAllByComplexFilter(ComplexFilter)
                    : _stockUnitRepository.GetAll(x => x.StockNumber, false, false);
            }
            else
            {
                var finder = new StockUnitFinder();
                finder.CreateCriteria(SearchString);
                TableItemList = _stockUnitRepository.Find(finder);
            }
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
            var item = SelectedItem as StockUnit;
            if (item != null)
            {
                const string caption = "Удаление";
                const string text = "Вы действительно хотите удалить эту запись?\r\n" +
                                    "Все устройства будут удалены.";
                const MessageBoxButton buttons = MessageBoxButton.OKCancel;

                if (MessageBox.Show(text, caption, buttons) == MessageBoxResult.OK)
                {
                    DeleteStockUnit(item);
                    if (RefreshCommand != null)
                        RefreshCommand.Execute(null);
                }
            }
        }

        private void DeleteStockUnit(StockUnit item)
        {
            var repository = new StockUnitRepository();
            var stockUnit = repository.GetById(item.Id, true);
            if (stockUnit.UnitList != null)
            {
                var unitRepository = new UnitRepository();
                foreach (var unit in stockUnit.UnitList)
                    unitRepository.Delete(unit);
            }

            repository.Delete(item);
        }
    }
}
