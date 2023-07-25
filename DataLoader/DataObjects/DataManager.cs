using System.Collections.Generic;
using SubSonic.Schema;

namespace Jaxis.POS.Data
{
    public abstract class DataManager<IENTITY, ENTITY> where ENTITY : IENTITY, IActiveRecord, ICallOnCreated, new()//, ENTITY : 
    {
        private static int MAX_ITEMS = 2000;

        public IENTITY Create()
        {
            var rc = new ENTITY();
            rc.CallOnCreated(true);
            return rc;
        }

        public virtual bool Save(IENTITY item, out IList<string> error)
        {
            IList<string> Results = new List<string>();

            ((IActiveRecord)item).Save();
            ((IActiveRecord)item).SetIsLoaded(true);

            error = Results;
            return true;
        }

        public virtual bool Save(IENTITY item)
        {
            ((IActiveRecord)item).Save();
            ((IActiveRecord)item).SetIsLoaded(true);

            return true;
        }

        public virtual bool Delete(IENTITY item, out IList<string> error)
        {
            ((IActiveRecord)item).Delete();
            error = new List<string>();
            return true;
        }


        ///// <summary>
        ///// Get DataObjects given a list of Keys. If the list has up to 2000 items, the SQL generated will be like WHERE Key IN ( a, b, c... )
        ///// and the interim result will be IQueryable, to send to CastToList. If the list has over 2000 items, the generated SQL will read in
        ///// the whole table and the join will be done in code.  The interim result will be IEnumerable.
        ///// </summary>
        ///// <typeparam name="T">The DataObject type</typeparam>
        ///// <typeparam name="U">The Interface return type</typeparam>
        ///// <param name="_IDs">The list of keys</param>
        ///// <param name="GetAll">An IQueryable that selects the table</param>
        ///// <param name="_where">An Expression that provides the filter</param>
        ///// <returns></returns>
        //public List<U> GetFromList<T, U>( List<Guid> _IDs,
        //    Func<IQueryable<T>> GetAll,
        //    Expression<Func<T, bool>> _where )
        //        where U : class where T : class, U, IActiveRecord
        //{
        //    List<U> rc = new List<U>( );
        //    if ( 0 < _IDs.Count )
        //    {
        //        if ( MAX_ITEMS > _IDs.Count )
        //        {
        //            var Results = GetAll( ).Where( _where );
        //            //var Results =
        //            //    from item in GetAll( )
        //            //    select item;

        //            //Results = Results.Where( _where );

        //            rc = Results.CastToList<T, U>( );
        //        }
        //        else
        //        {
        //            var Results =
        //                from id in _IDs
        //                join item in GetAll( ) on id equals item.KeyValue( )
        //                select item as U;
        //            rc = Results.ToList( );
        //        }
        //    }
        //    return rc;
        //}
    }
}