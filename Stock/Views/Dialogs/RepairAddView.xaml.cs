using System;
using System.Windows.Controls;
using Stock.Core.Domain;
using Stock.UI.ViewModels.Dialogs;

namespace Stock.UI.Views.Dialogs
{
	/// <summary>
	/// Interaction logic for PrinterAddDialog.xaml
	/// </summary>
	public partial class RepairAddView
	{
		public RepairAddView()
		{
            InitializeComponent();
            SetDefaultValues();
            SetActions();
		}

        public RepairAddView(Repair arg)
	    {
            InitializeComponent();
            ViewModel = new RepairAddViewModel(arg);
            DataContext = ViewModel;
            SetActions();
	    }

        private void SetDefaultValues()
        {
            CreationDateDtPk.Value = DateTime.Now;
            CompletedDateDtPk.Value = DateTime.Now;

            CreationDateDtPk.Focus();
        }

        private void SetActions()
        {
            if (ViewModel.CloseAction == null)
                ViewModel.CloseAction = Close;
        }

	    private void StockUnitCb_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
	    {
            ViewModel.RefreshUnitCommand.Execute(null);
	    }
	}
}