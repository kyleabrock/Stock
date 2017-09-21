namespace Stock.Core.Domain
{
    public class UserSetting : EntityBase
    {
        public virtual Account Account { get; set; }

        private string _settingKey = string.Empty;
        public virtual string SettingKey
        {
            get { return _settingKey; } 
            set { _settingKey = value; }
        }

        private string _settingValue = string.Empty;
        public virtual string SettingValue
        {
            get { return _settingValue; }
            set { _settingValue = value; }
        }
    }
}
