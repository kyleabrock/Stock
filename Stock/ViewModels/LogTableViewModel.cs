using System;
using System.ComponentModel;
using System.Windows.Data;
using Stock.Core.Domain;
using Stock.Core.Repository;
using Stock.UI.ViewModels.Base;

namespace Stock.UI.ViewModels
{
    public class LogTableViewModel : TableSearchViewModel<Log>
    {
        //TODO: IsSearched results
        public LogTableViewModel()
        {
            InitViewModel();
        }

        private Repository<Log> _logRepository;

        private void InitViewModel()
        {
            _logRepository = new Repository<Log>();

            RefreshCommand = new AsyncCommand(x => RefreshMethod());
            RefreshCommand.RunWorkerCompleted += RefreshCommand_RunWorkerCompleted;
        }

        void RefreshCommand_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
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
            TableItemList = _logRepository.GetAll(x => x.Date, false);
        }

        private bool FilterItems(object obj)
        {
            if (!(obj is Log))
                return false;

            var filterString = SearchString;
            var right = (Log)obj;

            if (StringContains(right.Date.ToString("dd.MM.yyyy"), filterString))
                return true;
            if (StringContains(right.UserName, filterString))
                return true;
            return StringContains(right.Message, filterString);
        }
    }
}
