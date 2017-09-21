using System;
using System.Collections.ObjectModel;
using System.Linq;
using Stock.Core.Domain;
using Stock.Core.Filter;
using Stock.Core.Filter.FilterParams;
using Stock.Core.Repository;
using Stock.UI.ViewModels.Base;

namespace Stock.UI.ViewModels
{
    public class CardTableViewModel : TableNavigationViewModel<Card>
    {
        public CardTableViewModel()
        {
            InitViewModel();
        }

        public Action LoadSettingsAction { get; set; }

        private ObservableCollection<CheckBoxItem<Staff>> _filterStaffCheckList;
        public ObservableCollection<CheckBoxItem<Staff>> FilterStaffCheckList
        {
            get { return _filterStaffCheckList; }
            set { _filterStaffCheckList = value; OnPropertyChanged("FilterStaffCheckList"); }
        }

        private ObservableCollection<CheckBoxItem<string>> _filterDepartmentCheckList;
        public ObservableCollection<CheckBoxItem<string>> FilterDepartmentCheckList
        {
            get { return _filterDepartmentCheckList; }
            set { _filterDepartmentCheckList = value; OnPropertyChanged("FilterDepartmentCheckList"); }
        }

        private int _filterStaffCount;
        public int FilterStaffCount
        {
            get { return _filterStaffCount; }
            set { _filterStaffCount = value; OnPropertyChanged("FilterStaffCount"); }
        }

        private int _filterDepartmentCount;
        public int FilterDepartmentCount
        {
            get { return _filterDepartmentCount; }
            set { _filterDepartmentCount = value; OnPropertyChanged("FilterDepartmentCount"); }
        }

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
            var staff = (from item in FilterStaffCheckList where item.IsChecked select item.Item).ToList();
            var departments = (from item in FilterDepartmentCheckList where item.IsChecked select item.Item).ToList();

            FilterStaffCount = staff.Count;
            FilterDepartmentCount = departments.Count;

            var filterParams = ComplexFilterParams as CardFilterParams;
            if (filterParams != null)
            {
                filterParams.Staff = staff;
                filterParams.Department = departments;

                Filter = new CardFilter(filterParams);
            }
        }

        private void InitViewModel()
        {
            Repository = new CardRepository();
            
            AddCommand = new RelayCommand(x => AddMethod());
            EditCommand = new RelayCommand(x => EditMethod());
            DeleteCommand = new RelayCommand(x => DeleteMethod());
            ClearFilterCommand = new RelayCommand(x => ClearFilterMethod());
        }
        
        private void InitFilter()
        {
            ComplexFilterParams = new CardFilterParams();

            var staffRepository = new StaffRepository();
            var filterDepartmentList = staffRepository.GetDepartments();
            var filterStaffList = staffRepository.GetAll(staff => staff.Name.DisplayName);

            _filterStaffCheckList = new ObservableCollection<CheckBoxItem<Staff>>();
            foreach (var staff in filterStaffList)
                FilterStaffCheckList.Add(new CheckBoxItem<Staff> { Item = staff });
            OnPropertyChanged("FilterStaffCheckList");

            _filterDepartmentCheckList = new ObservableCollection<CheckBoxItem<string>>();
            foreach (var department in filterDepartmentList)
                FilterDepartmentCheckList.Add(new CheckBoxItem<string> { Item = department });
            OnPropertyChanged("FilterDepartmentCheckList");
        }
        
        private void ClearFilterMethod()
        {
            Filter = new CardFilter();
            ComplexFilterParams = new CardFilterParams();
            InitFilter();

            if (string.IsNullOrEmpty(SearchString)) IsSearched = false;

            if (RefreshCommand != null)
                RefreshCommand.Execute(null);
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

                if (ShowDialogMessage(text, caption))
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

            try
            {
                repository.Delete(item);
            }
            catch (Exception ex)
            {
                ShowInfoMessage(ex.Message, "Ошибка");
            }
        }
    }
}
