using Microsoft.EntityFrameworkCore;
using Crazy_zoo.Animals;
using System.Collections.Generic;
using System.Linq;


namespace Crazy_zoo.Data
{
    public class EfRepository<T> : IRepository<T> where T : class
    {
        private readonly CrazyZooContext _context;
        private readonly DbSet<T> _dbSet;

        public EfRepository(CrazyZooContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public void Add(T item)
        {
            _dbSet.Add(item);
            _context.SaveChanges();
        }

        public void Remove(T item)
        {
            _dbSet.Remove(item);
            _context.SaveChanges();
        }

        public IEnumerable<T> GetAll()
        {
            return _dbSet.AsNoTracking().ToList();
        }

        public T? Find(Func<T, bool> predicate)
        {
            return GetAll().FirstOrDefault(predicate);
        }
    }
}
