using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Data.SqlTypes;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Core.Domain;
using Core.Repository;

namespace Stock.CardDialogs
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
            InitViewModel(_cardRepository.GetByIdFull(arg.Id));
        }

        public void AddStockUnit()
        {
            foreach (var item in NewStockUnitList)
            {
                item.Card = _card;
                if (!StockUnitList.Contains(item))
                    StockUnitList.Add(item);
                if (_itemsToDelete.Contains(item))
                {
                    _itemsToDelete.Remove(item);
                    DefaultCard.StockUnitList.Remove(item);
                }
            }
            
            NewStockUnitList = new ObservableCollection<StockUnit>();
        }

        public void RemoveStockUnit()
        {
            RemoveMethod(false);
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
            set
            {
                _stockUnitList = value; 
                OnPropertyChanged("StockUnitList");
            }
        }

        private ObservableCollection<StockUnit> _newStockUnitList;
        public ObservableCollection<StockUnit> NewStockUnitList
        {
            get { return _newStockUnitList; }
            set
            {
                _newStockUnitList = value;
                OnPropertyChanged("NewStockUnitList");
            }
        }

        private object _selectedItem;
        public object SelectedItem
        {
            get
            {
                return _selectedItem;
            }
            set
            {
                _selectedItem = value;
                OnPropertyChanged("SelectedItem");
            }
        }

        public ObservableCollection<Staff> StaffList { get; private set; }
        public ICommand AddStockUnitCommand { get; set; }
        public ICommand RemoveStockUnitCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand CloseCommand { get; set; }
        public Action AddAction { get; set; }
        public Action CloseAction { get; set; }

        private readonly CardRepository _cardRepository;
        private readonly List<StockUnit> _itemsToDelete = new List<StockUnit>();

        private void InitViewModel(Card card)
        {
            Card = card;
            if (Card.IsNew)
                Card.StockUnitList = new List<StockUnit>();

            InitLists();
            DefaultCard = _cardRepository.GetDefaultCard();
            StockUnitList = new ObservableCollection<StockUnit>(Card.StockUnitList);
            StockUnitList.CollectionChanged += StockUnitList_CollectionChanged;

            AddStockUnitCommand = new RelayCommand(x => AddMethod());
            RemoveStockUnitCommand = new RelayCommand(x => RemoveMethod());
            SaveCommand = new RelayCommand(x => SaveMethod());
            CloseCommand = new RelayCommand(x => CloseMethod());
        }

        private void InitLists()
        {
            var staffRepository = new StaffRepository();
            StaffList = new ObservableCollection<Staff>(staffRepository.GetAllOrdered());
        }

        private void AddMethod()
        {
            throw new NotImplementedException();
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

        private void StockUnitList_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems == null) return;

            foreach (var item in e.OldItems.OfType<StockUnit>())
            {
                item.Card = DefaultCard;
                DefaultCard.StockUnitList.Add(item);
                _itemsToDelete.Add(item);
            }
        }
    }
}
