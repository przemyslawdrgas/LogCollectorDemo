using LiteDB;
using LogCollectorDemo.Core.Repositories;
using System.Collections.Generic;
using System.Linq;
using LogCollectorDemo.Core.Entities;

namespace LogCollectorDemo.Data.Repositories
{
    public class LiteDBRepository<T> : IRepository<T> where T : IEntity
    {
        private string _connString;
        private LiteDatabase _db;
        protected ILiteCollection<T> _collection;

        public LiteDBRepository(string connectionString)
        {
            _connString = connectionString;
            _db = new LiteDatabase(_connString);
            _collection = _db.GetCollection<T>();
        }

        public void Insert(T entity)
        {
            _collection.Insert(entity);
        }

        public List<T> GetAll()
        {
            return _collection.FindAll().ToList();
        }
    }
}
