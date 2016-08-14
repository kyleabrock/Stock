using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;
using Stock.Core.Domain;
using Stock.Core.Filter;
using Stock.Core.Finder;
using Stock.Core.Repository;
using Stock.UI.ViewModels.Base;

namespace Stock.UI.ViewModels
{
    public class UnitTableViewModel : TableSearchViewModel<Unit>
    {
        //TODO: IsSearched results
        public UnitTableViewModel()
        {
            InitViewModel();
        }

        public ICommand OpenCardCommand { get; set; }
        public ICommand OpenStockUnitCommand { get; set; }
        public Action OpenCardAction { get; set; }
        public Action OpenStockUnitAction { get; set; }

        private IEnumerable<UnitType> _filterUnitTypeList;
        public IEnumerable<UnitType> FilterUnitTypeList
        {
            get { return _filterUnitTypeList; }
            set { _filterUnitTypeList = value; OnPropertyChanged("FilterUnitTypeList"); }
        }

        private IEnumerable<Owner> _filterOwnerList;
        public IEnumerable<Owner> FilterOwnerList
        {
            get { return _filterOwnerList; }
            set { _filterOwnerList = value; OnPropertyChanged("FilterOwnerList"); }
        }

        private IEnumerable<string> _filterManufactureList;
        public IEnumerable<string> FilterManufactureList
        {
            get { return _filterManufactureList; }
            set { _filterManufactureList = value; OnPropertyChanged("FilterManufactureList"); }
        }

        private IEnumerable<string> _filterModelNameList;
        public IEnumerable<string> FilterModelNameList
        {
            get { return _filterModelNameList; }
            set { _filterModelNameList = value; OnPropertyChanged("FilterModelNameList"); }
        }

        private IFilterBase _complexFilter = new UnitFilter();
        public IFilterBase ComplexFilter
        {
            get { return _complexFilter; }
            set { _complexFilter = value; OnPropertyChanged("ComplexFilter"); }
        }

        public ICommand FilterCommand { get; set; }
        public ICommand ClearFilterCommand { get; set; }

        private bool ComplexFilterStatus { get; set; }

        private UnitRepository _unitRepository;
        private bool _initFilterStatus;

        private void InitViewModel()
        {
            _unitRepository = new UnitRepository();

            OpenCardCommand = new RelayCommand(x => OpenCardMethod());
            OpenStockUnitCommand = new RelayCommand(x => OpenStockUnitMethod());
            RefreshCommand = new AsyncCommand(x => RefreshMethod());
            FilterCommand = new RelayCommand(x => FilterMethod());
            ClearFilterCommand = new RelayCommand(x => CancelFilter());

            RefreshCommand.RunWorkerCompleted += RefreshCommand_RunWorkerCompleted;
        }

        private void InitFilter()
        {
            var unitTypeRepository = new UnitTypeRepository();
            FilterUnitTypeList = unitTypeRepository.GetAll(type => type.TypeName);
            FilterManufactureList = _unitRepository.GetManufactureList();
            FilterModelNameList = _unitRepository.GetModelList();

            var ownerRepository = new OwnerRepository();
            FilterOwnerList = ownerRepository.GetAll(owner => owner.Name.DisplayName);

            _initFilterStatus = true;
        }

        private void OpenCardMethod()
        {
            OpenCardAction();
        }

        private void OpenStockUnitMethod()
        {
            OpenStockUnitAction();
        }

        private void FilterMethod()
        {
            ComplexFilterStatus = true;
            if (RefreshCommand != null)
                RefreshCommand.Execute(null);
        }

        private void CancelFilter()
        {
            ComplexFilterStatus = false;
            ComplexFilter.ClearFilter();
            OnPropertyChanged("ComplexFilter");

            if (RefreshCommand != null)
                RefreshCommand.Execute(null);
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
                    ? _unitRepository.GetAllByComplexFilter(ComplexFilter)
                    : _unitRepository.GetAll(x => x.Id, false, true);
            }
            else
            {
                var finder = new UnitFinder();
                finder.CreateCriteria(SearchString);
                TableItemList = _unitRepository.Find(finder);
            }
        }

        private bool FilterItems(object obj)
        {
            if (!(obj is Unit))
                return false;

            var filterString = SearchString;
            var right = (Unit)obj;

            if (StringContains(right.UnitType.TypeName, filterString))
                return true;
            if (StringContains(right.Manufacture, filterString))
                return true;
            if (StringContains(right.ModelName, filterString))
                return true;
            if (StringContains(right.Serial, filterString))
                return true;
            return StringContains(right.Comments, filterString);
        }
    }
}
