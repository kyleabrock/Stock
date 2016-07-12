using System;
using System.Windows.Input;

namespace Stock.UI.ViewModels.Base
{
    interface ITableNavigationViewModel
    {
        ICommand AddCommand { get; set; }
        ICommand EditCommand { get; set; }
        ICommand DeleteCommand { get; set; }
        Action AddAction { get; set; }
        Action EditAction { get; set; }
    }
}
