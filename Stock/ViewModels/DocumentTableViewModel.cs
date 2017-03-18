using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using Stock.Core.Domain;
using Stock.Core.Filter;
using Stock.Core.Filter.FilterParams;
using Stock.Core.Repository;
using Stock.UI.ViewModels.Base;

namespace Stock.UI.ViewModels
{
    public class DocumentTableViewModel : TableNavigationViewModel<Document>
    {
        //TODO: IsSearched results
        public DocumentTableViewModel()
        {
            InitViewModel();
        }


        private ObservableCollection<CheckBoxItem<Owner>> _filterOwnerCheckList;
        public ObservableCollection<CheckBoxItem<Owner>> FilterOwnerCheckList
        {
            get { return _filterOwnerCheckList; }
            set { _filterOwnerCheckList = value; OnPropertyChanged("FilterOwnerCheckList"); }
        }

        private ObservableCollection<CheckBoxItem<DocumentType>> _filterDocumentTypeCheckList;
        public ObservableCollection<CheckBoxItem<DocumentType>> FilterDocumentTypeCheckList
        {
            get { return _filterDocumentTypeCheckList; }
            set { _filterDocumentTypeCheckList = value; OnPropertyChanged("FilterDocumentTypeCheckList"); }
        }

        private int _filterOwnerCount;
        public int FilterOwnerCount
        {
            get { return _filterOwnerCount; }
            set { _filterOwnerCount = value; OnPropertyChanged("FilterOwnerCount"); }
        }

        private int _filterDocumentTypeCount;
        public int FilterDocumentTypeCount
        {
            get { return _filterDocumentTypeCount; }
            set { _filterDocumentTypeCount = value; OnPropertyChanged("FilterDocumentTypeCount"); }
        }

        protected override void RefreshMethod()
        {
            if (!IsFilterInitialized)
            {
                InitFilter();
                IsFilterInitialized = true;
            }

            FillFilter();
            base.RefreshMethod();
        }

        private bool IsFilterInitialized { get; set; }

        private void FillFilter()
        {
            var owners = (from item in FilterOwnerCheckList where item.IsChecked select item.Item).ToList();
            var documentTypes = (from item in FilterDocumentTypeCheckList where item.IsChecked select item.Item).ToList();
            FilterOwnerCount = owners.Count;
            FilterDocumentTypeCount = documentTypes.Count;

            ComplexFilterParams = new DocumentFilterParams
                {
                    Owner = owners,
                    DocumentType = documentTypes
                };

            var complexFilterParams = ComplexFilterParams as DocumentFilterParams;
            Filter = new DocumentFilter(complexFilterParams);
        }

        private void InitViewModel()
        {
            Repository = new DocumentRepository();

            AddCommand = new RelayCommand(x => AddMethod());
            EditCommand = new RelayCommand(x => EditMethod());
            DeleteCommand = new RelayCommand(x => DeleteMethod());
            FilterCommand = new RelayCommand(x => FilterMethod());
            ClearFilterCommand = new RelayCommand(x => ClearFilterMethod());
        }

        private void InitFilter()
        {
            ComplexFilterParams = new DocumentFilterParams();

            var ownerRepository = new OwnerRepository();
            var documentTypeRepository = new Repository<DocumentType>();
            
            var filterOwnerList = ownerRepository.GetAll(x => x.Name.DisplayName);
            var filterDocumentTypeList = documentTypeRepository.GetAll(x => x.TypeName);

            _filterOwnerCheckList = new ObservableCollection<CheckBoxItem<Owner>>();
            foreach (var owner in filterOwnerList)
                FilterOwnerCheckList.Add(new CheckBoxItem<Owner> { Item = owner });
            OnPropertyChanged("FilterOwnerCheckList");

            _filterDocumentTypeCheckList = new ObservableCollection<CheckBoxItem<DocumentType>>();
            foreach (var owner in filterDocumentTypeList)
                FilterDocumentTypeCheckList.Add(new CheckBoxItem<DocumentType> { Item = owner });
            OnPropertyChanged("FilterDocumentTypeCheckList");
        }

        private void FilterMethod()
        {
            var filter = ComplexFilterParams as DocumentFilterParams;
            if (filter != null)
            {
                Filter = new DocumentFilter(filter);
                IsSearched = true;
                if (RefreshCommand != null)
                    RefreshCommand.Execute(null);
            }
        }

        private void ClearFilterMethod()
        {
            Filter = new DocumentFilter();
            ComplexFilterParams = new DocumentFilterParams();
            InitFilter();

            if (string.IsNullOrEmpty(SearchString)) IsSearched = false;

            if (RefreshCommand != null)
                RefreshCommand.Execute(null);
        }

        private void AddMethod()
        {
            AddAction();
        }

        private void EditMethod()
        {
            EditAction();
        }

        private void DeleteMethod()
        {
            var item = SelectedItem as Document;
            if (item != null)
            {
                const string caption = "Удаление";
                const string text = "Вы действительно хотите удалить эту запись?\r\n" +
                                    "Все устройства будут удалены.";

                if (ShowDialogMessage(text, caption))
                {
                    DeleteDocument(item);
                    if (RefreshCommand != null)
                        RefreshCommand.Execute(null);
                }
            }
        }

        private void DeleteDocument(Document item)
        {
            try
            {
                Repository.Delete(item);
            }
            catch (Exception ex)
            {
                ShowInfoMessage(ex.Message, "Ошибка");
            }
            
        }
    }
}
