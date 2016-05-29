using Core.Domain;

namespace Stock.OwnerDialogs
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