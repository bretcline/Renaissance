using System;
using System.Collections.Generic;
using System.Linq;

namespace Jaxis.POS.Data
{
    public interface IDataManager
    {
        
    }

    public interface IDataManager<T> : IDataManager
    {
        T Create( );
        bool Save(T item);
        bool Save(T item, out IList<string> results);
        bool Delete(T item, out IList<string> results);

        IQueryable<T> GetAll( );
        T Get( Guid ID );
    }
}
