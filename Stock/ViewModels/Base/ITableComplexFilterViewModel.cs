using System.Windows.Input;
using Stock.Core.Filter;

namespace Stock.UI.ViewModels.Base
{
    interface ITableComplexFilterViewModel
    {
        IFilterBase ComplexFilter { get; set; }
        bool ComplexFilterStatus { get; set; }

        ICommand FilterCommand { get; set; }
        ICommand ClearFilterCommand { get; set; }
    }
}
