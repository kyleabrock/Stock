using System;
using System.Windows.Input;
using Stock.Core.Domain;
using Stock.Core.Filter;

namespace Stock.UI.ViewModels.Base
{
    public class TableNavigationViewModel<T> : TableSearchViewModel<T>, ITableNavigationViewModel, ITableComplexFilterViewModel where T : EntityBase 
    {
        protected TableNavigationViewModel()
        {
            FilterCommand = new RelayCommand(x => FilterMethod());
            ClearFilterCommand = new RelayCommand(x => CancelFilter());
        }

        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public Action AddAction { get; set; }
        public Action EditAction { get; set; }

        private IFilterBase _complexFilter;
        public IFilterBase ComplexFilter
        {
            get { return _complexFilter; }
            set { _complexFilter = value; OnPropertyChanged("ComplexFilter"); }
        }

        public bool ComplexFilterStatus { get; set; }
        public ICommand FilterCommand { get; set; }
        public ICommand ClearFilterCommand { get; set; }

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
    }
}
