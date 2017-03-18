using Stock.Core.Domain;

namespace Stock.Core.Factory
{
    public interface ILogFactory
    {
        Log CreateMessage(UserAcc user, ILoggedEntity entity);
    }
}
