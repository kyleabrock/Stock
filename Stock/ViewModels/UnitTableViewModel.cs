using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Stock.Core.Domain;
using Stock.Core.Filter;
using Stock.Core.Filter.FilterParams;
using Stock.Core.Repository;
using Stock.UI.ViewModels.Base;

namespace Stock.UI.ViewModels
{
    public class UnitTableViewModel : TableViewModel<Unit>
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

        private IEnumerable<Status> _filterStatusList;
        public IEnumerable<Status> FilterStatusList
        {
            get { return _filterStatusList; }
            set { _filterStatusList = value; OnPropertyChanged("FilterStatusList"); }
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

        private IFilterParams _complexFilterParams = new UnitFilterParams();
        public IFilterParams ComplexFilterParams
        {
            get { return _complexFilterParams; }
            set { _complexFilterParams = value; OnPropertyChanged("ComplexFilterParams"); }
        }

        public ICommand FilterCommand { get; set; }
        public ICommand ClearFilterCommand { get; set; }

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

        private void FillFilter()
        {
            var filterParams = ComplexFilterParams as UnitFilterParams;
            if (filterParams != null)
            {
                filterParams.UnitType = new List<UnitType>();
                filterParams.Status = new List<Status>();
                filterParams.Owner = new List<Owner>();
                filterParams.ModelName = new List<string>();
                filterParams.Manufacture = new List<string>();
                
                Filter = new UnitFilter(filterParams);
            }
        }

        private bool IsFilterInitialized { get; set; }

        private void InitViewModel()
        {
            Repository = new UnitRepository();
            ComplexFilterParams = new UnitFilterParams();

            OpenCardCommand = new RelayCommand(x => OpenCardMethod());
            OpenStockUnitCommand = new RelayCommand(x => OpenStockUnitMethod());
            FilterCommand = new RelayCommand(x => FilterMethod());
            ClearFilterCommand = new RelayCommand(x => ClearFilterMethod());
        }

        private void InitFilter()
        {
            Filter = new UnitFilter();
            ComplexFilterParams = new UnitFilterParams();

            var unitTypeRepository = new UnitTypeRepository();
            FilterUnitTypeList = unitTypeRepository.GetAll(type => type.TypeName);

            var repository = Repository as UnitRepository;
            if (repository != null)
            {
                FilterManufactureList = repository.GetManufactureList();
                FilterModelNameList = repository.GetModelList();
            }

            var ownerRepository = new OwnerRepository();
            FilterOwnerList = ownerRepository.GetAll(owner => owner.Name.DisplayName);

            var statusRepository = new StatusRepository();
            FilterStatusList = statusRepository.GetAll(x => x.StatusType);
        }

        private void FilterMethod()
        {
            var filter = ComplexFilterParams as UnitFilterParams;
            if (filter != null)
            {
                Filter = new UnitFilter(filter);
                IsSearched = true;
                if (RefreshCommand != null)
                    RefreshCommand.Execute(null);
            }
        }

        private void ClearFilterMethod()
        {
            Filter = new UnitFilter();
            ComplexFilterParams = new UnitFilterParams();
            InitFilter();

            if (string.IsNullOrEmpty(SearchString)) IsSearched = false;

            if (RefreshCommand != null)
                RefreshCommand.Execute(null);
        }

        private void OpenCardMethod()
        {
            OpenCardAction();
        }

        private void OpenStockUnitMethod()
        {
            OpenStockUnitAction();
        }
    }
}
