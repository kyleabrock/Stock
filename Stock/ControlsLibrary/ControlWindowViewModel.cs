using System.Collections.Generic;
using System.ComponentModel;
using Stock.Core.Repository;

namespace Stock.UI.ControlsLibrary
{
    public class ControlWindowViewModel : INotifyPropertyChanged
    {
        public ControlWindowViewModel()
        {
            var repository = new UnitRepository();
            Collection = repository.GetManufactureList();
        }

        public IList<string> Collection { get; set; }

        private string _manufacture;
        public string Manufacture
        {
            get { return _manufacture; }
            set
            {
                _manufacture = value; 
                OnPropertyChanged("Manufacture");
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        protected void OnPropertyChanged(string prop)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
