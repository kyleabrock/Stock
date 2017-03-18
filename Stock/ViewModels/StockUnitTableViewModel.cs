using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Stock.Core.Domain;
using Stock.Core.Filter;
using Stock.Core.Filter.FilterParams;
using Stock.Core.Repository;
using Stock.UI.ViewModels.Base;

namespace Stock.UI.ViewModels
{
    public class StockUnitTableViewModel : TableNavigationViewModel<StockUnit>
    {
        public StockUnitTableViewModel()
        {
            InitViewModel();
        }

        #region filterParams

        private ObservableCollection<CheckBoxItem<Owner>> _filterOwnerCheckList;
        public ObservableCollection<CheckBoxItem<Owner>> FilterOwnerCheckList
        {
            get { return _filterOwnerCheckList; }
            set { _filterOwnerCheckList = value; OnPropertyChanged("FilterOwnerCheckList"); }
        }

        private ObservableCollection<CheckBoxItem<Status>> _filterStatusCheckList;
        public ObservableCollection<CheckBoxItem<Status>> FilterStatusCheckList
        {
            get { return _filterStatusCheckList; }
            set { _filterStatusCheckList = value; OnPropertyChanged("FilterStatusCheckList"); }
        }

        private ObservableCollection<CheckBoxItem<Card>> _filterCardCheckList;
        public ObservableCollection<CheckBoxItem<Card>> FilterCardCheckList
        {
            get { return _filterCardCheckList; }
            set { _filterCardCheckList = value; OnPropertyChanged("FilterCardCheckList"); }
        }

        private int _filterOwnerCount;
        public int FilterOwnerCount
        {
            get { return _filterOwnerCount; }
            set { _filterOwnerCount = value; OnPropertyChanged("FilterOwnerCount"); }
        }

        private int _filterStatusCount;
        public int FilterStatusCount
        {
            get { return _filterStatusCount; }
            set { _filterStatusCount = value; OnPropertyChanged("FilterStatusCount"); }
        }

        private int _filterCardCount;
        public int FilterCardCount
        {
            get { return _filterCardCount; }
            set { _filterCardCount = value; OnPropertyChanged("FilterCardCount"); }
        }

        #endregion

        public ICommand CopyCommand { get; set; }
        public ICommand ShowCardCommand { get; set; }
        public Action<StockUnit> CopyAction { get; set; }
        public Action<Card> ShowCardAction { get; set; }

        protected override void RefreshMethod()
        {
            if (!IsFilterInitialized)
            {
                InitFilter();
                IsFilterInitialized = true;
            }

            FillFilter();
            base.RefreshMethod();
        }

        private bool IsFilterInitialized { get; set; }

        private void FillFilter()
        {
            var owners = (from item in FilterOwnerCheckList where item.IsChecked select item.Item).ToList();
            var cards = (from item in FilterCardCheckList where item.IsChecked select item.Item).ToList();
            var status = (from item in FilterStatusCheckList where item.IsChecked select item.Item).ToList();
            FilterOwnerCount = owners.Count;
            FilterCardCount = cards.Count;
            FilterStatusCount = status.Count;

            var filterParams = ComplexFilterParams as StockUnitFilterParams;
            if (filterParams != null)
            {
                filterParams.Owner = owners;
                filterParams.Status = status;
                filterParams.Card = cards;

                Filter = new StockUnitFilter(filterParams);
            }
        }
        
        private void InitViewModel()
        {
            Repository = new StockUnitRepository();
            
            AddCommand = new RelayCommand(x => AddAction());
            CopyCommand = new RelayCommand(x => CopyMethod());
            EditCommand = new RelayCommand(x => EditAction());
            DeleteCommand = new RelayCommand(x => DeleteMethod());
            ShowCardCommand = new RelayCommand(x => ShowCardMethod());
            
            ClearFilterCommand = new RelayCommand(x => ClearFilterMethod());
        }

        private void ShowCardMethod()
        {
            var item = SelectedItem as StockUnit;
            if (item != null)
            {
                var card = item.Card;
                if (card != null)
                    ShowCardAction(card);
            }
        }

        private void InitFilter()
        {
            ComplexFilterParams = new StockUnitFilterParams();
            var statusRepository = new StatusRepository();
            var ownerRepository = new OwnerRepository();
            var cardRepository = new CardRepository();

            var filterStatusList = statusRepository.GetAll(status => status.StatusType);
            var filterOwnerList = ownerRepository.GetAll(owner => owner.Name.DisplayName);
            var filterCardList = cardRepository.GetAll(card => card.CardName);

            _filterOwnerCheckList = new ObservableCollection<CheckBoxItem<Owner>>();
            foreach (var owner in filterOwnerList)
                FilterOwnerCheckList.Add(new CheckBoxItem<Owner> {Item = owner});
            OnPropertyChanged("FilterOwnerCheckList");

            _filterStatusCheckList = new ObservableCollection<CheckBoxItem<Status>>();
            foreach (var status in filterStatusList)
                FilterStatusCheckList.Add(new CheckBoxItem<Status> { Item = status });
            OnPropertyChanged("FilterStatusCheckList");

            _filterCardCheckList = new ObservableCollection<CheckBoxItem<Card>>();
            foreach (var card in filterCardList)
                FilterCardCheckList.Add(new CheckBoxItem<Card> { Item = card });
            OnPropertyChanged("FilterCardCheckList");
        }

        private void ClearFilterMethod()
        {
            Filter = new StockUnitFilter();
            ComplexFilterParams = new StockUnitFilterParams();
            InitFilter();

            if (string.IsNullOrEmpty(SearchString)) IsSearched = false;
            
            if (RefreshCommand != null)
                RefreshCommand.Execute(null);
        }

        private void CopyMethod()
        {
            var item = SelectedItem as StockUnit;
            if (item != null)
            {
                var repository = new StockUnitRepository();
                var stockUnit = repository.GetById(item.Id, true);

                var result = new StockUnit
                    {
                        StockNumber = item.StockNumber,
                        StockName = item.StockName,
                        CreationDate = item.CreationDate,
                        Comments = item.Comments
                    };

                var units = stockUnit.UnitList;
                var resultUnits = new List<Unit>();
                foreach (var unit in units)
                {
                    var resultUnit = new Unit
                        {
                            StockUnit = result,
                            UnitType = unit.UnitType,
                            Manufacture = unit.Manufacture,
                            ModelName = unit.ModelName,
                            Comments = unit.Comments
                        };
                    
                    resultUnits.Add(resultUnit);
                }
                
                result.UnitList = resultUnits;
                CopyAction(result);
            }
        }

        private void DeleteMethod()
        {
            var item = SelectedItem as StockUnit;
            if (item != null)
            {
                const string caption = "Удаление";
                const string text = "Вы действительно хотите удалить эту запись?\r\n" +
                                    "Все устройства будут удалены.";
                
                if (ShowDialogMessage(text, caption))
                {
                    if (DeleteStockUnit(item))
                    {
                        if (RefreshCommand != null)
                            RefreshCommand.Execute(null);
                    }
                }
            }
        }

        private bool DeleteStockUnit(StockUnit item)
        {
            var stockUnit = Repository.GetById(item.Id);
            if (stockUnit.UnitList != null)
            {
                IRepository<Unit> unitRepository = new Repository<Unit>();
                foreach (var unit in stockUnit.UnitList)
                {
                    try
                    {
                        unitRepository.Delete(unit);
                    }
                    catch (Exception ex)
                    {
                        ShowDialogMessage(ex.Message, "Ошибка");
                    }
                }
            }

            try
            {
                Repository.Delete(item);
            }
            catch (Exception ex)
            {
                ShowDialogMessage(ex.Message, "Ошибка");
                return false;
            }

            return true;
        }
    }
}
