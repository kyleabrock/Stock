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

        private RepairRepository _repairRepository;
        private bool _initFilterStatus = false;

        private void InitViewModel()
        {
            _repairRepository = new RepairRepository();

            AddCommand = new RelayCommand(x => AddMethod());
            EditCommand = new RelayCommand(x => EditMethod());
            DeleteCommand = new RelayCommand(x => DeleteMethod());
            RefreshCommand = new AsyncCommand(x => RefreshMethod());
            RefreshCommand.RunWorkerCompleted += RefreshCommand_RunWorkerCompleted;

            ComplexFilter = new RepairFilter();
        }

        private void InitFilter()
        {
            var userRepository = new Repository<UserAcc>();
            FilterUserList = userRepository.GetAll();

            _initFilterStatus = true;
        }

        void RefreshCommand_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
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
                _repairRepository.GetAllByComplexFilter(ComplexFilter) 
                : _repairRepository.GetAll(x => x.StartedDate, false, true);
        }

        private bool FilterItems(object obj)
        {
            if (!(obj is Repair))
                return false;

            var filterString = SearchString;
            var right = (Repair)obj;

            if (StringContains(right.StartedDate.ToShortDateString(), filterString))
                return true;
            if (StringContains(right.CompletedDate.ToShortDateString(), filterString))
                return true;
            if (StringContains(right.Defect, filterString))
                return true;
            if (StringContains(right.Result, filterString))
                return true;
            if (StringContains(right.User.Name.DisplayName, filterString))
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
            var item = SelectedItem as Repair;
            if (item != null)
            {
                const string caption = "Удаление";
                const string text = "Вы действительно хотите удалить эту запись?\r\n" +
                                    "Все устройства будут удалены.";
                const MessageBoxButton buttons = MessageBoxButton.OKCancel;

                if (MessageBox.Show(text, caption, buttons) == MessageBoxResult.OK)
                {
                    DeleteRepair(item);
                    if (RefreshCommand != null)
                        RefreshCommand.Execute(null);
                }
            }
        }

        private void DeleteRepair(Repair item)
        {
            _repairRepository.Delete(item);
        }
    }
}
