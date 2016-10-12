using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using BusinessLogic.Interfaces;
using Core.Entities;

namespace BusinessLogic.Repositories
{
    public class Repository<T> : IRepository<T> where T : EntityBase
    {
        public Repository(DbDataContext context)
        {
            _context = context;
        }
        public List<T> Get()
        {
            return _context.Set<T>().ToList();
        }

        public T Create(T obj)
        {
            _context.Set<T>().Add(obj);
            _context.SaveChanges();

            return _context.Set<T>().Find(obj.Id);
        }

        public T Update(T obj)
        {
            _context.Entry(obj).State = EntityState.Modified;
            _context.SaveChanges();

            return _context.Set<T>().Find(obj.Id);
        }

        public int Delete(T obj)
        {
            if (_context.Set<T>().Find(obj.Id) != null)
                _context.Set<T>().Remove(obj);

            return _context.SaveChanges();
        }

        private readonly DbDataContext _context;
    }
}
