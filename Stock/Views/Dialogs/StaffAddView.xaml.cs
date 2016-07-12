using Stock.Core.Domain;
using Stock.UI.ViewModels.Dialogs;

namespace Stock.UI.Views.Dialogs
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