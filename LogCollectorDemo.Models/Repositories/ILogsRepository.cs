using LogCollectorDemo.Core.Entities;
using System.Collections.Generic;

namespace LogCollectorDemo.Core.Repositories
{
    public interface IRepository<T> where T : IEntity
    {
        void Insert(T entity);
        List<T> GetAll();
    }
}
