using System.Collections.Generic;

namespace Stock.Core.Domain
{
    public class Account : EntityBase
    {
        public virtual UserAcc UserAcc { get; set; }
        public virtual string Login { get; set; }
        public virtual string Salt { get; set; }
        public virtual string HashedPassword { get; set; }

        public virtual IList<UserSetting> UserSettingList { get; set; }
    }
}
