using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlTypes;
using System.Windows;
using System.Windows.Input;
using Stock.Core.Domain;
using Stock.Core.Factory;
using Stock.Core.Repository;
using Stock.Report;
using Stock.UI.ViewModels.Base;

namespace Stock.UI.ViewModels.Dialogs
{
    public class CardAddViewModel : ViewModelBase
    {
        public CardAddViewModel()
        {
            _cardRepository = new CardRepository();
            InitViewModel(new Card());
        }

        public CardAddViewModel(Card arg)
        {
            _cardRepository = new CardRepository();
            InitViewModel(_cardRepository.GetById(arg.Id, true));
        }

        public CardAddViewModel(int cardId)
        {
            _cardRepository = new CardRepository();
            if (cardId > 0)
                InitViewModel(_cardRepository.GetById(cardId, true));
        }

        private Card _card;
        public Card Card
        {
            get { return _card; }
            set 
            { 
                _card = value;
                OnPropertyChanged("Card"); 
            }
        }

        private Card _defaultCard;
        public Card DefaultCard
        {
            get { return _defaultCard; }
            set
            {
                _defaultCard = value; 
                OnPropertyChanged("DefaultCard");
            }
        }

        private ObservableCollection<StockUnit> _stockUnitList;
        public ObservableCollection<StockUnit> StockUnitList
        {
            get { return _stockUnitList; }
            set { _stockUnitList = value; OnPropertyChanged("StockUnitList"); }
        }

        private object _selectedItem;
        public object SelectedItem
        {
            get { return _selectedItem; }
            set { _selectedItem = value; OnPropertyChanged("SelectedItem"); }
        }

        public ObservableCollection<Staff> StaffList { get; private set; }
        public ICommand AddCommand { get; set; }
        public ICommand RemoveCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand CloseCommand { get; set; }
        public ICommand ExportCommand { get; set; }
        public Action CloseAction { get; set; }
        public Func<Card, ObservableCollection<StockUnit>> AddFunc { get; set; }

        private readonly CardRepository _cardRepository;
        private readonly List<StockUnit> _itemsToDelete = new List<StockUnit>();

        private void InitViewModel(Card card)
        {
            Card = card;
            //if (Card.IsNew)
            //    Card.StockUnitList = new List<StockUnit>();

            InitLists();
            DefaultCard = _cardRepository.GetDefaultCard();
            StockUnitList = new ObservableCollection<StockUnit>(Card.StockUnitList);

            AddCommand = new RelayCommand(x => AddMethod());
            RemoveCommand = new RelayCommand(x => RemoveMethod());
            SaveCommand = new RelayCommand(x => SaveMethod());
            CloseCommand = new RelayCommand(x => CloseMethod());
            ExportCommand = new RelayCommand(x => ExportMethod());
        }

        private void InitLists()
        {
            var staffRepository = new StaffRepository();
            StaffList = new ObservableCollection<Staff>(staffRepository.GetAllOrdered());
        }

        private void AddMethod()
        {
            var stockUnitRepository = new StockUnitRepository();
            
            var items = AddFunc(DefaultCard);
            if (items == null) return;

            foreach (var item in items)
            {
                DefaultCard.StockUnitList.Remove(item);
                OnPropertyChanged("DefaultCard");
                
                var stockUnit = stockUnitRepository.GetById(item.Id, true);
                stockUnit.Card = _card;

                if (!StockUnitList.Contains(stockUnit))
                    StockUnitList.Add(stockUnit);
                if (_itemsToDelete.Contains(stockUnit))
                    _itemsToDelete.Remove(stockUnit);
            }
        }

        private void RemoveMethod(bool removeFromCollection = true)
        {
            var item = SelectedItem as StockUnit;
            if (item == null) return;

            if (removeFromCollection)
                StockUnitList.Remove(item);
        }

        private void SaveMethod()
        {
            if (!CheckValues()) return;

            Card.StockUnitList = StockUnitList;
            _cardRepository.Save(Card);

            var stockUnitRepository = new StockUnitRepository();
            foreach (var unit in StockUnitList)
                unit.Card = Card;
            stockUnitRepository.Save(StockUnitList);

            stockUnitRepository.Save(_itemsToDelete);

            var user = AppSettings.User;
            ILogFactory logFactory = new LogFactory();
            var logEntity = logFactory.CreateMessage(user, Card);
            IRepository<Log> repository = new LogRepository();
            repository.Save(logEntity);

            CloseAction();
        }

        private bool CheckValues()
        {
            if (Card.CreationDate.Year < SqlDateTime.MinValue.Value.Year)
            {
                MessageBox.Show("Дата создания не может быть меньше 1 января 1753 г.");
                return false;
            }
            if (string.IsNullOrEmpty(Card.CardNumber))
            {
                MessageBox.Show("Укажите номер карточки (в случае отсутствия укажите б/н)");
                return false;
            }
            if (Card.Staff == null)
            {
                MessageBox.Show("Укажите ответственное лицо");
                return false;
            }
            return true;
        }

        private void CloseMethod()
        {
            CloseAction();
        }

        private void ExportMethod()
        {
            var cardReport = new CardReport();
            cardReport.Export(Card, false);
            MessageBox.Show("Карточка успешно экспортирована");
        }
    }
}
