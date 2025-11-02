using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Crazy_zoo.Data
{
    public interface IRepository<T>
    {
        void Add(T item);
        void Remove(T item);
        IEnumerable<T> GetAll();
        T? Find(Func<T, bool> predicate);
    }
}
