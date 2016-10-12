using System.Collections.Generic;
using Core.Entities;

namespace BusinessLogic.Interfaces
{
    public interface IRepository<T> where T : EntityBase
    {
        List<T> Get();
        T Create(T obj);
        T Update(T obj);
        int Delete(T obj);
    }
}
