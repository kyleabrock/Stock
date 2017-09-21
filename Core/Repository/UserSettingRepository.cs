using System;
using System.Collections.Generic;
using NHibernate;
using NHibernate.Criterion;
using Stock.Core.Domain;

namespace Stock.Core.Repository
{
    public class UserSettingRepository : Repository<UserSetting>
    {
        public UserSettings GetByAccount(Account account)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                if (account.IsNew) throw new Exception();

                var result = session.CreateCriteria<UserSetting>()
                    .CreateCriteria("Account", "a")
                    .Add(Restrictions.Eq("a.Id", account.Id))
                    .List<UserSetting>();

                var settings = new UserSettings(result, account);

                return settings;
            }
        }

        public IList<UserSetting> GetByAccountList(Account account)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                if (account.IsNew) throw new Exception();

                var result = session.CreateCriteria<UserSetting>()
                    .CreateCriteria("Account", "a")
                    .Add(Restrictions.Eq("a.Id", account.Id))
                    .List<UserSetting>();

                return result;
            }
        }

        public void Save(UserSettings settings)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    foreach (var item in settings.Values)
                    {
                        if (item.IsNew)
                            session.Save(item);
                        else
                        {
                            session.Update(item);
                        }
                    }

                    transaction.Commit();
                }
            }
        }

        public static UserSettings Default(Account account)
        {
            IList<UserSetting> settings = new List<UserSetting>();
            settings.Add(new UserSetting { SettingKey = "CardTableColumn0Width", SettingValue = "50", Account = account });
            settings.Add(new UserSetting { SettingKey = "CardTableColumn1Width", SettingValue = "250", Account = account });
            settings.Add(new UserSetting { SettingKey = "CardTableColumn2Width", SettingValue = "85", Account = account });
            settings.Add(new UserSetting { SettingKey = "CardTableColumn3Width", SettingValue = "100", Account = account });
            settings.Add(new UserSetting { SettingKey = "CardTableColumn4Width", SettingValue = "250", Account = account });
            settings.Add(new UserSetting { SettingKey = "DocumentTableColumn0Width", SettingValue = "250", Account = account });
            settings.Add(new UserSetting { SettingKey = "DocumentTableColumn1Width", SettingValue = "150", Account = account });
            settings.Add(new UserSetting { SettingKey = "DocumentTableColumn2Width", SettingValue = "150", Account = account });
            settings.Add(new UserSetting { SettingKey = "DocumentTableColumn3Width", SettingValue = "200", Account = account });
            settings.Add(new UserSetting { SettingKey = "OwnerTableColumn0Width", SettingValue = "250", Account = account });
            settings.Add(new UserSetting { SettingKey = "OwnerTableColumn1Width", SettingValue = "100", Account = account });
            settings.Add(new UserSetting { SettingKey = "OwnerTableColumn2Width", SettingValue = "450", Account = account });
            settings.Add(new UserSetting { SettingKey = "RepairTableColumn0Width", SettingValue = "85", Account = account });
            settings.Add(new UserSetting { SettingKey = "RepairTableColumn1Width", SettingValue = "150", Account = account });
            settings.Add(new UserSetting { SettingKey = "RepairTableColumn2Width", SettingValue = "100", Account = account });
            settings.Add(new UserSetting { SettingKey = "RepairTableColumn3Width", SettingValue = "85", Account = account });
            settings.Add(new UserSetting { SettingKey = "RepairTableColumn4Width", SettingValue = "150", Account = account });
            settings.Add(new UserSetting { SettingKey = "RepairTableColumn5Width", SettingValue = "150", Account = account });
            settings.Add(new UserSetting { SettingKey = "RepairTableColumn6Width", SettingValue = "200", Account = account });
            settings.Add(new UserSetting { SettingKey = "RepairTableColumn7Width", SettingValue = "100", Account = account });
            settings.Add(new UserSetting { SettingKey = "StaffTableColumn0Width", SettingValue = "250", Account = account });
            settings.Add(new UserSetting { SettingKey = "StaffTableColumn1Width", SettingValue = "100", Account = account });
            settings.Add(new UserSetting { SettingKey = "StaffTableColumn2Width", SettingValue = "450", Account = account });
            settings.Add(new UserSetting { SettingKey = "StatusTableColumn0Width", SettingValue = "80", Account = account });
            settings.Add(new UserSetting { SettingKey = "StatusTableColumn1Width", SettingValue = "250", Account = account });
            settings.Add(new UserSetting { SettingKey = "StatusTableColumn2Width", SettingValue = "480", Account = account });
            settings.Add(new UserSetting { SettingKey = "StockUnitTableColumn0Width", SettingValue = "76", Account = account });
            settings.Add(new UserSetting { SettingKey = "StockUnitTableColumn1Width", SettingValue = "100", Account = account });
            settings.Add(new UserSetting { SettingKey = "StockUnitTableColumn2Width", SettingValue = "250", Account = account });
            settings.Add(new UserSetting { SettingKey = "StockUnitTableColumn3Width", SettingValue = "85", Account = account });
            settings.Add(new UserSetting { SettingKey = "StockUnitTableColumn4Width", SettingValue = "100", Account = account });
            settings.Add(new UserSetting { SettingKey = "StockUnitTableColumn5Width", SettingValue = "100", Account = account });
            settings.Add(new UserSetting { SettingKey = "StockUnitTableColumn6Width", SettingValue = "50", Account = account });
            settings.Add(new UserSetting { SettingKey = "StockUnitTableColumn7Width", SettingValue = "120", Account = account });
            settings.Add(new UserSetting { SettingKey = "UnitTableColumn0Width", SettingValue = "76", Account = account });
            settings.Add(new UserSetting { SettingKey = "UnitTableColumn1Width", SettingValue = "90", Account = account });
            settings.Add(new UserSetting { SettingKey = "UnitTableColumn2Width", SettingValue = "120", Account = account });
            settings.Add(new UserSetting { SettingKey = "UnitTableColumn3Width", SettingValue = "120", Account = account });
            settings.Add(new UserSetting { SettingKey = "UnitTableColumn4Width", SettingValue = "120", Account = account });
            settings.Add(new UserSetting { SettingKey = "UnitTableColumn5Width", SettingValue = "150", Account = account });
            settings.Add(new UserSetting { SettingKey = "UnitTableColumn6Width", SettingValue = "120", Account = account });
            settings.Add(new UserSetting { SettingKey = "CardTableColumn0DisplayIndex", SettingValue = "0", Account = account });
            settings.Add(new UserSetting { SettingKey = "CardTableColumn1DisplayIndex", SettingValue = "1", Account = account });
            settings.Add(new UserSetting { SettingKey = "CardTableColumn2DisplayIndex", SettingValue = "2", Account = account });
            settings.Add(new UserSetting { SettingKey = "CardTableColumn3DisplayIndex", SettingValue = "3", Account = account });
            settings.Add(new UserSetting { SettingKey = "CardTableColumn4DisplayIndex", SettingValue = "4", Account = account });
            settings.Add(new UserSetting { SettingKey = "DocumentTableColumn0DisplayIndex", SettingValue = "0", Account = account });
            settings.Add(new UserSetting { SettingKey = "DocumentTableColumn1DisplayIndex", SettingValue = "1", Account = account });
            settings.Add(new UserSetting { SettingKey = "DocumentTableColumn2DisplayIndex", SettingValue = "2", Account = account });
            settings.Add(new UserSetting { SettingKey = "DocumentTableColumn3DisplayIndex", SettingValue = "3", Account = account });
            settings.Add(new UserSetting { SettingKey = "OwnerTableColumn0DisplayIndex", SettingValue = "0", Account = account });
            settings.Add(new UserSetting { SettingKey = "OwnerTableColumn1DisplayIndex", SettingValue = "1", Account = account });
            settings.Add(new UserSetting { SettingKey = "OwnerTableColumn2DisplayIndex", SettingValue = "2", Account = account });
            settings.Add(new UserSetting { SettingKey = "RepairTableColumn0DisplayIndex", SettingValue = "0", Account = account });
            settings.Add(new UserSetting { SettingKey = "RepairTableColumn1DisplayIndex", SettingValue = "1", Account = account });
            settings.Add(new UserSetting { SettingKey = "RepairTableColumn2DisplayIndex", SettingValue = "2", Account = account });
            settings.Add(new UserSetting { SettingKey = "RepairTableColumn3DisplayIndex", SettingValue = "3", Account = account });
            settings.Add(new UserSetting { SettingKey = "RepairTableColumn4DisplayIndex", SettingValue = "4", Account = account });
            settings.Add(new UserSetting { SettingKey = "RepairTableColumn5DisplayIndex", SettingValue = "5", Account = account });
            settings.Add(new UserSetting { SettingKey = "RepairTableColumn6DisplayIndex", SettingValue = "6", Account = account });
            settings.Add(new UserSetting { SettingKey = "RepairTableColumn7DisplayIndex", SettingValue = "7", Account = account });
            settings.Add(new UserSetting { SettingKey = "StaffTableColumn0DisplayIndex", SettingValue = "0", Account = account });
            settings.Add(new UserSetting { SettingKey = "StaffTableColumn1DisplayIndex", SettingValue = "1", Account = account });
            settings.Add(new UserSetting { SettingKey = "StaffTableColumn2DisplayIndex", SettingValue = "2", Account = account });
            settings.Add(new UserSetting { SettingKey = "StatusTableColumn0DisplayIndex", SettingValue = "0", Account = account });
            settings.Add(new UserSetting { SettingKey = "StatusTableColumn1DisplayIndex", SettingValue = "1", Account = account });
            settings.Add(new UserSetting { SettingKey = "StatusTableColumn2DisplayIndex", SettingValue = "2", Account = account });
            settings.Add(new UserSetting { SettingKey = "StockUnitTableColumn0DisplayIndex", SettingValue = "0", Account = account });
            settings.Add(new UserSetting { SettingKey = "StockUnitTableColumn1DisplayIndex", SettingValue = "1", Account = account });
            settings.Add(new UserSetting { SettingKey = "StockUnitTableColumn2DisplayIndex", SettingValue = "2", Account = account });
            settings.Add(new UserSetting { SettingKey = "StockUnitTableColumn3DisplayIndex", SettingValue = "3", Account = account });
            settings.Add(new UserSetting { SettingKey = "StockUnitTableColumn4DisplayIndex", SettingValue = "4", Account = account });
            settings.Add(new UserSetting { SettingKey = "StockUnitTableColumn5DisplayIndex", SettingValue = "5", Account = account });
            settings.Add(new UserSetting { SettingKey = "StockUnitTableColumn6DisplayIndex", SettingValue = "6", Account = account });
            settings.Add(new UserSetting { SettingKey = "StockUnitTableColumn7DisplayIndex", SettingValue = "7", Account = account });
            settings.Add(new UserSetting { SettingKey = "UnitTableColumn0DisplayIndex", SettingValue = "0", Account = account });
            settings.Add(new UserSetting { SettingKey = "UnitTableColumn1DisplayIndex", SettingValue = "1", Account = account });
            settings.Add(new UserSetting { SettingKey = "UnitTableColumn2DisplayIndex", SettingValue = "2", Account = account });
            settings.Add(new UserSetting { SettingKey = "UnitTableColumn3DisplayIndex", SettingValue = "3", Account = account });
            settings.Add(new UserSetting { SettingKey = "UnitTableColumn4DisplayIndex", SettingValue = "4", Account = account });
            settings.Add(new UserSetting { SettingKey = "UnitTableColumn5DisplayIndex", SettingValue = "5", Account = account });
            settings.Add(new UserSetting { SettingKey = "UnitTableColumn6DisplayIndex", SettingValue = "6", Account = account });
            
            var userSettings = new UserSettings(settings, account);
            return userSettings;
        }
    }
}
