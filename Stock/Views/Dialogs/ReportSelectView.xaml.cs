using Stock.Core.Domain;
using Stock.UI.ViewModels.Dialogs;

namespace Stock.UI.Views.Dialogs
{
	/// <summary>
	/// Interaction logic for ReportSelectView.xaml
	/// </summary>
	public partial class ReportSelectView
	{
		public ReportSelectView(EntityBase arg)
		{
			InitializeComponent();

            _viewModel = new ReportSelectViewModel(arg);

		    DataContext = _viewModel;
		    if (_viewModel.CloseAction == null)
		        _viewModel.CloseAction = CloseWindow;
		}

        private readonly ReportSelectViewModel _viewModel;

	    private void CloseWindow()
	    {
	        Close();
	    }
	}
}