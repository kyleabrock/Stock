using Stock.Core.Domain;
using Stock.UI.ViewModels.Dialogs;

namespace Stock.UI.Views.Dialogs
{
	/// <summary>
	/// Interaction logic for PrinterAddDialog.xaml
	/// </summary>
	public partial class OwnerAddView
	{
		public OwnerAddView()
		{
            InitializeComponent();
            SetDefaultValues();
            SetActions();
		}

        public OwnerAddView(Owner arg)
	    {
            InitializeComponent();
            ViewModel = new OwnerAddViewModel(arg);
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