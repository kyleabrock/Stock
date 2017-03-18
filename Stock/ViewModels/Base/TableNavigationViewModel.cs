using System;
using System.Windows.Input;
using Stock.Core.Domain;
using Stock.Core.Filter.FilterParams;

namespace Stock.UI.ViewModels.Base
{
    public class TableNavigationViewModel<T> : TableViewModel<T> where T : EntityBase 
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
        public Action<string, string> ShowInfoMessage { get; set; }
        public Func<string, string, bool> ShowDialogMessage { get; set; }

        private IFilterParams _complexFilterParams;
        public IFilterParams ComplexFilterParams
        {
            get { return _complexFilterParams; }
            set { _complexFilterParams = value; OnPropertyChanged("ComplexFilterParams"); }
        }

        public bool ComplexFilterStatus { get; set; }
        public ICommand FilterCommand { get; set; }
        public ICommand ClearFilterCommand { get; set; }

        private void FilterMethod()
        {
            IsSearched = true;
            if (RefreshCommand != null)
                RefreshCommand.Execute(null);
        }

        private void CancelFilter()
        {
            ComplexFilterStatus = false;
            ComplexFilterParams.ClearFilter();
            OnPropertyChanged("ComplexFilterParams");

            if (RefreshCommand != null)
                RefreshCommand.Execute(null);
        }
    }
}
