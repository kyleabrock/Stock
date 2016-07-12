using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using Stock.Core.Domain;
using Stock.Core.Repository;
using Stock.UI.ViewModels.Base;

namespace Stock.UI.ViewModels
{
    public class OwnerTableViewModel : TableNavigationViewModel<Owner>
    {
        //TODO: Filter results
        public OwnerTableViewModel()
        {
            InitViewModel();
        }

        private OwnerRepository _repository;

        private void InitViewModel()
        {
            _repository = new OwnerRepository();
            
            AddCommand = new RelayCommand(x => AddMethod());
            EditCommand = new RelayCommand(x => EditMethod());
            DeleteCommand = new RelayCommand(x => DeleteMethod());
            RefreshCommand = new AsyncCommand(x => RefreshMethod());
            RefreshCommand.RunWorkerCompleted += RefreshCommand_RunWorkerCompleted;
        }

        private bool FilterItems(object obj)
        {
            if (!(obj is Owner))
                return false;

            var filterString = SearchString;
            var right = (Owner)obj;

            if (StringContains(right.Name.DisplayName, filterString))
                return true;
            if (StringContains(right.Department, filterString))
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

        private void RefreshCommand_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
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
            TableItemList = _repository.GetAllOrdered();
        }

        private void DeleteMethod()
        {
            var item = SelectedItem as Owner;
            if (item != null)
            {
                const string caption = "Удаление";
                const string text = "Вы действительно хотите удалить эту запись?\r\n" +
                                    "Все устройства будут удалены.";
                const MessageBoxButton buttons = MessageBoxButton.OKCancel;

                if (MessageBox.Show(text, caption, buttons) == MessageBoxResult.OK)
                {
                    DeleteOwner(item);
                    if (RefreshCommand != null)
                        RefreshCommand.Execute(null);
                }
            }
        }

        private void DeleteOwner(Owner item)
        {
            _repository.Delete(item);
        }
    }
}
