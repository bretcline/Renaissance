using System;
using System.Collections.Generic;
using Jaxis.POS.Data;

namespace Jaxis.POS.Data
{

    public class MissingManagerException : Exception
    {
        public MissingManagerException( string _error )
            : base( _error )
        {
        }
    }


    public partial class DataManagerFactory : IDataManagerFactory
    {
        private static DataManagerFactory m_Factory;
        private readonly Dictionary<Type, IDataManager> m_Managers = new Dictionary<Type, IDataManager>( );

        public Guid UserSession { get; set; }        

        public static IDataManagerFactory Get()
        {
            return m_Factory ?? ( m_Factory = new DataManagerFactory( ) );
        }

        #region IDataManagerFactory Members

        public IDataManager<T> Manage<T> ( )
        {
            return m_Managers[ typeof ( T ) ] as IDataManager< T >;
        }

        #endregion
    }
}