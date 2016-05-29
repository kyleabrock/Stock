using System.Collections.Generic;
using Core.Domain;

namespace Core.Repository
{
    public interface IRepository<T> where T : EntityBase
    {
        IList<T> GetAll();
        T GetById(int id);

        void Save(T arg);
        void Save(IEnumerable<T> arg);
        void Add(T arg);
        void Add(IEnumerable<T> arg);
        void Update(T arg);
        void Update(IEnumerable<T> arg);
        void Delete(T arg);
        void Delete(IEnumerable<T> arg);
    }
}
