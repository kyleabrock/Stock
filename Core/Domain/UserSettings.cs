using System.Collections.Generic;

namespace Stock.Core.Domain
{
    public class UserSettings 
    {
        public UserSettings(Account account)
        {
            _account = account;
        }

	    public UserSettings(IEnumerable<UserSetting> settings, Account account)
	    {
		    foreach (var setting in settings)
			    _values.Add(setting.SettingKey, setting);

		    _account = account;
	    }

	    private readonly Dictionary<string, UserSetting> _values = 
            new Dictionary<string, UserSetting>();

	    private readonly Account _account;

	    public string this[string key]
	    {
		    get { return _values[key].SettingValue; }
		    set
		    {
		        if (!_values.ContainsKey(key))
		            _values.Add(key, new UserSetting {SettingKey = key, SettingValue = value, Account = _account});
		        else
		        {
		            _values[key].SettingValue = value;
		        }
		    }
	    }

        public void Add(string key, string value)
        {
            if (_values.ContainsKey(key))
                _values.Remove(key);
            _values.Add(key, new UserSetting {SettingKey = key, SettingValue = value, Account = _account});
        }

        public void Remove(string key)
        {
            if (_values.ContainsKey(key))
                _values.Remove(key);
        }

        public IEnumerable<string> Keys
        {
            get { return _values.Keys; }
        }

        public IEnumerable<UserSetting> Values
        {
            get { return _values.Values; }
        }

        public bool ContainsKey(string key)
        {
            return _values.ContainsKey(key);
        }

	    public int Count
	    {
		    get { return _values.Count; }
	    }
    }
}
