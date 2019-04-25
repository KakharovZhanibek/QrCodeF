using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoQRCode.Repositories
{
    // Контракт с внешним хранилищем
    // CRUD-операции - Create, Read, Update, Delete
    public interface IRepository<T>
    {
        void Add(T entity, out string message);
        T Read(int id);
        IEnumerable<T> ReadAll();
        void Update(int id, T updated, out string message);
        void Delete(int id, out string message);
    }
}
