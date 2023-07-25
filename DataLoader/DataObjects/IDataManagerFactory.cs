namespace Jaxis.POS.Data
{
    public interface IDataManagerFactory
    {
        IDataManager< T > Manage< T >( );
    }
}