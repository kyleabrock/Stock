using System.Windows;
using Core.Domain;
using Core.Repository;
using Stock.StatusDialogs;

namespace Stock.StaffDialogs
{
	/// <summary>
	/// Interaction logic for PrinterAddDialog.xaml
	/// </summary>
	public partial class StaffAddView
	{
		public StaffAddView()
		{
            InitializeComponent();
            SetDefaultValues();
            SetActions();
		}

        public StaffAddView(Staff arg)
	    {
            InitializeComponent();
            ViewModel = new StaffAddViewModel(arg);
            DataContext = ViewModel;
            SetActions();
	    }

        private void SetDefaultValues()
        {
            LastNameTb.Focus();
        }

        private void SetActions()
        {
            if (ViewModel.CloseAction == null)
                ViewModel.CloseAction = Close;
        }
	}
}