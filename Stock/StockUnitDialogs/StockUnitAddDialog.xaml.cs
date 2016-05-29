using System;
using System.Windows.Input;
using Core.Domain;

namespace Stock.StockUnitDialogs
{
	/// <summary>
	/// Interaction logic for PrinterAddDialog.xaml
	/// </summary>
	public partial class StockUnitAddDialog
	{
		public StockUnitAddDialog()
		{
		    InitializeComponent();
            SetDefaultValues();
            SetActions();
		}

        public StockUnitAddDialog(StockUnit arg)
	    {
            InitializeComponent();
            ViewModel = new StockUnitViewModel(arg);
            DataContext = ViewModel;
            SetActions();
	    }

        private void SetDefaultValues()
        {
            if (StatusCb.Items.Count > 0) StatusCb.SelectedIndex = 0;
            if (OwnerCb.Items.Count > 0) OwnerCb.SelectedIndex = 0;
            CreationDateDtPk.Value = DateTime.Now;
            
            StockNumberTb.Focus();
        }

	    private void SetActions()
	    {
            if (ViewModel.CloseAction == null)
                ViewModel.CloseAction = Close;
	    }

        private void MainDataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
                ViewModel.RemoveUnit();
        }
	}
}