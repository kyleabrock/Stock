using System;
using System.Collections.Generic;
using Stock.Core.Domain;
using Stock.Core.Filter;
using Stock.Core.Filter.FilterParams;
using Stock.Core.Repository;
using Stock.UI.ViewModels.Base;

namespace Stock.UI.ViewModels
{
    public class RepairTableViewModel : TableNavigationViewModel<Repair>
    {
        //TODO: IsSearched results
        public RepairTableViewModel()
        {
            InitViewModel();
        }

        private IEnumerable<UserAcc> _filterUserList;
        public IEnumerable<UserAcc> FilterUserList
        {
            get { return _filterUserList; }
            set { _filterUserList = value; OnPropertyChanged("FilterUserList"); }
        }

        protected override void RefreshMethod()
        {
            if (!IsFilterInitialized)
            {
                Filter = new RepairFilter();
                ComplexFilterParams = new RepairFilterParams();
                InitFilter();

                IsFilterInitialized = true;
            }

            base.RefreshMethod();
        }

        private bool IsFilterInitialized { get; set; }

        private void InitViewModel()
        {
            Repository = new RepairRepository();

            AddCommand = new RelayCommand(x => AddMethod());
            EditCommand = new RelayCommand(x => EditMethod());
            DeleteCommand = new RelayCommand(x => DeleteMethod());
            FilterCommand = new RelayCommand(x => FilterMethod());
            ClearFilterCommand = new RelayCommand(x => ClearFilterMethod());
        }

        private void InitFilter()
        {
            var userRepository = new Repository<UserAcc>();
            FilterUserList = userRepository.GetAll();
        }

        private void FilterMethod()
        {
            var filter = ComplexFilterParams as RepairFilterParams;
            if (filter != null)
            {
                Filter = new RepairFilter(filter);
                IsSearched = true;
                if (RefreshCommand != null)
                    RefreshCommand.Execute(null);
            }
        }

        private void ClearFilterMethod()
        {
            Filter = new RepairFilter();
            ComplexFilterParams = new RepairFilterParams();
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
            var item = SelectedItem as Repair;
            if (item != null)
            {
                const string caption = "Удаление";
                const string text = "Вы действительно хотите удалить эту запись?\r\n" +
                                    "Все устройства будут удалены.";
                
                if (ShowDialogMessage(text, caption))
                {
                    DeleteRepair(item);
                    if (RefreshCommand != null)
                        RefreshCommand.Execute(null);
                }
            }
        }

        private void DeleteRepair(Repair item)
        {
            try
            {
                Repository.Delete(item);
            }
            catch (Exception ex)
            {
                ShowInfoMessage(ex.Message, "Ошибка");
            }
        }
    }
}
