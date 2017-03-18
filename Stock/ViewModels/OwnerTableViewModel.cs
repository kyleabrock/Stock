using System;
using Stock.Core.Domain;
using Stock.Core.Filter;
using Stock.Core.Repository;
using Stock.UI.ViewModels.Base;

namespace Stock.UI.ViewModels
{
    public class OwnerTableViewModel : TableNavigationViewModel<Owner>
    {
        //TODO: filterParams results
        public OwnerTableViewModel()
        {
            InitViewModel();
        }

        protected override void RefreshMethod()
        {
            if (!IsFilterInitialized)
            {
                Filter = new OwnerFilter();
                
                IsFilterInitialized = true;
            }

            base.RefreshMethod();
        }

        private bool IsFilterInitialized { get; set; }

        private void InitViewModel()
        {
            Repository = new OwnerRepository();

            AddCommand = new RelayCommand(x => AddMethod());
            EditCommand = new RelayCommand(x => EditMethod());
            DeleteCommand = new RelayCommand(x => DeleteMethod());
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
            var item = SelectedItem as Owner;
            if (item != null)
            {
                const string caption = "Удаление";
                const string text = "Вы действительно хотите удалить эту запись?\r\n" +
                                    "Все устройства будут удалены.";
                
                if (ShowDialogMessage(text, caption))
                {
                    DeleteOwner(item);
                    if (RefreshCommand != null)
                        RefreshCommand.Execute(null);
                }
            }
        }

        private void DeleteOwner(Owner item)
        {
            try
            {
                Repository.Delete(item);
            }
            catch (Exception ex)
            {
                ShowInfoMessage(ex.Message, "Ошибка");
                ShowInfoMessage(ex.InnerException.Message, "Ошибка");
            }
        }
    }
}
