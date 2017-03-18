using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Stock.Core.Domain;
using Stock.Core.Filter;
using Stock.Core.Filter.FilterParams;
using Stock.Core.Repository;
using Stock.UI.ViewModels.Base;

namespace Stock.UI.ViewModels.Dialogs
{
    public class StockUnitSearchViewModel : TableViewModel<StockUnit>
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

        public StockUnitSearchViewModel(int cardId)
        {
            IRepository<Card> repository = new Repository<Card>();
            _card = repository.GetById(cardId);
            
            InitViewModel();
        }

        public override string SearchString
        {
            get { return base.SearchString; }
            set
            {
                base.SearchString = value;
                OnPropertyChanged("SearchString");

                if (IsSearched && RefreshCommand != null)
                    RefreshCommand.Execute(null);
            }
        }

        public IList<StockUnit> Result { get; private set; }

        public ICommand OkCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public Action CloseAction { get; set; }
        public Func<ObservableCollection<object>> GetSelectedItems { get; set; }

        private readonly Card _card;

        private void InitViewModel()
        {
            IsSearched = true;

            Repository = new StockUnitRepository();
            if (_card.IsNew)
                Filter = new StockUnitFilter();
            else
            {
                var filter = new StockUnitFilterParams { Card = new List<Card> { _card } };
                Filter = new StockUnitFilter(filter);
            }

            Result = new List<StockUnit>();

            OkCommand = new RelayCommand(x => OkMethod());
            CancelCommand = new RelayCommand(x => CloseAction());

            if (RefreshCommand != null)
                RefreshCommand.Execute(null);
        }

        private void OkMethod()
        {
            var items = GetSelectedItems();
            if (items.Count > 0)
            {
                foreach (var item in items)
                {
                    var result = item as StockUnit;
                    Result.Add(result);
                }
            }

            CloseAction();
        }
    }
}
