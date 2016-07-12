using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Stock.Core.Domain;
using Stock.Core.Factory;
using Stock.Core.Repository;
using Stock.UI.ViewModels.Base;

namespace Stock.UI.ViewModels.Dialogs
{
    public class RepairAddViewModel : ViewModelBase
    {
        public RepairAddViewModel()
        {
            _repairRepository = new RepairRepository();
            InitViewModel(new Repair());
        }

        public RepairAddViewModel(Repair arg)
        {
            _repairRepository = new RepairRepository();
            InitViewModel(_repairRepository.GetById(arg.Id, true));
        }

        private Repair _repair;
        public Repair Repair
        {
            get
            {
                return _repair;
            }
            set
            {
                _repair = value;
                OnPropertyChanged("Repair");
            }
        }

        public ObservableCollection<StockUnit> StockUnitList { get; set; }
        public ObservableCollection<UserAcc> UserList { get; set; }

        private ObservableCollection<Unit> _unitList;
        public ObservableCollection<Unit> UnitList
        {
            get { return _unitList; }
            set 
            { 
                _unitList = value;
                OnPropertyChanged("UnitList");
            }
        }

        private Unit _unit;
        public Unit Unit
        {
            get { return _unit; }
            set { _unit = value; OnPropertyChanged("Unit"); }
        }

        public object StockUnitSelectedItem { get; set; }
        
        public ICommand SaveCommand { get; set; }
        public ICommand CloseCommand { get; set; }
        public ICommand RefreshUnitCommand { get; set; }
        public Action CloseAction { get; set; }
        
        private RepairRepository _repairRepository;
        private StockUnitRepository _stockUnitRepository;

        private void InitViewModel(Repair repair)
        {
            _stockUnitRepository = new StockUnitRepository();
            StockUnitList = new ObservableCollection<StockUnit>(_stockUnitRepository.GetAllOrdered());

            var userRepository = new Repository<UserAcc>();
            UserList = new ObservableCollection<UserAcc>(userRepository.GetAll());

            Repair = repair;
            if (!Repair.IsNew)
            {
                StockUnitSelectedItem = Repair.Unit.StockUnit;
                Unit = Repair.Unit;
            }

            SaveCommand = new RelayCommand(x => SaveMethod());
            CloseCommand = new RelayCommand(x => CloseMethod());
            RefreshUnitCommand = new RelayCommand(x => RefreshMethod());
        }

        private void RefreshMethod()
        {
            var selectedStockUnit = StockUnitSelectedItem as StockUnit;
            if (selectedStockUnit != null)
            {
                var stockUnit = _stockUnitRepository.GetById(selectedStockUnit.Id, true);
                UnitList = new ObservableCollection<Unit>(stockUnit.UnitList);
            }
        }

        private void SaveMethod()
        {
            Repair.Unit = Unit;
            _repairRepository.Save(Repair);

            var user = ApplicationState.GetValue<UserAcc>("User");
            ILogFactory logFactory = new LogFactory();
            var logEntity = logFactory.CreateLogMessage(user, Repair);
            var repository = new Repository<Log>();
            repository.Save(logEntity);

            CloseAction();
        }

        private void CloseMethod()
        {
            CloseAction();
        }
    }
}
