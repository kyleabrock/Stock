using Stock.Core.Domain;
using Stock.Core.Filter;
using Stock.Core.Repository;
using Stock.UI.ViewModels.Base;

namespace Stock.UI.ViewModels
{
    public class LogTableViewModel : TableViewModel<Log>
    {
        //TODO: IsSearched results
        public LogTableViewModel()
        {
            InitViewModel();
        }

        private void InitViewModel()
        {
            Repository = new Repository<Log>();
            Filter = new LogFilter();
        }
    }
}
