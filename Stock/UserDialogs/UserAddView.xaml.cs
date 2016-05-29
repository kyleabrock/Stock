using Core.Domain;

namespace Stock.UserDialogs
{
	/// <summary>
	/// Interaction logic for PrinterAddDialog.xaml
	/// </summary>
	public partial class UserAddView
	{
		public UserAddView()
		{
            InitializeComponent();
            SetDefaultValues();
            SetActions();
		}

        public UserAddView(UserAcc arg)
	    {
            InitializeComponent();
            ViewModel = new UserAddViewModel(arg);
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