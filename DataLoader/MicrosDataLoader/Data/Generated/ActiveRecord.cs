


using System;
using System.Reflection;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SubSonic.DataProviders;
using SubSonic.Extensions;
using System.Linq.Expressions;
using SubSonic.Schema;
using System.Collections;
using SubSonic;
using SubSonic.Repository;
using System.ComponentModel;
using System.Data.Common;
using SubSonic.SqlGeneration.Schema;

namespace Jaxis.Inventory.Data
{
	public interface IWCFDataElement
	{
		IBaseDataObject CreateDBObject( );
	}

    public interface ICallOnCreated
    {
        void CallOnCreated( bool _CallOnCreated);
    }

    public static class ActiveRecordExtensions
    {
        public static T SingleOrDefault<T>( this IRepository<T> _repo, Expression<Func<T, bool>> expression ) where T : IActiveRecord
        {
            var results = _repo.Find( expression );
            T single = default( T );
            foreach ( T i in results )
            {
                single = i;
                single.SetIsLoaded( true );
                single.SetIsNew( false );
                break;
            }
            return single;
        }
    }

	public interface IBaseDataObject
	{
        bool IsNew();
        void SetIsNew(bool isNew);
        bool IsLoaded();
		void Save( );
	}

    public abstract class BaseDataObject<T> : IBaseDataObject where T : class, new( )
    {
        protected ITable tbl;
        protected bool _isNew;

        protected T m_Internal = default(T);

        public T GetInternalData( )
        {
            return m_Internal;
        }
    

        public bool IsNew()
        {
            return _isNew;
        }
        
        public void SetIsNew(bool isNew)
        {
            _isNew=isNew;
        }

        protected bool _isLoaded;
        public bool IsLoaded()
        {
            return _isLoaded;
        }
                
        protected List<IColumn> _dirtyColumns;
        public bool IsDirty()
        {
            return _dirtyColumns.Count>0;
        }
        
        public List<IColumn> GetDirtyColumns ()
        {
            return _dirtyColumns;
        }

        public IList<IColumn> Columns
        {
            get
            {
                return tbl.Columns;
            }
        }

		public abstract void Save( );

    }



    [DataContract]
    public partial class DataBanquetMenu : IWCFDataElement
    {
        [DataMember]
        public Guid BanquetID { get; set; }
        [DataMember]
        public Guid MenuItemID { get; set; }

        public void Copy( DataBanquetMenu _Item )
        {
             MenuItemID = _Item.MenuItemID;			
        }

		public IBaseDataObject CreateDBObject( )
        {
            return new BanquetMenu( this );
        }


    }


    /// <summary>
    /// A class which represents the BanquetMenus table in the BeverageMonitor Database.
    /// </summary>
    public partial class BanquetMenu: BaseDataObject<DataBanquetMenu>, IActiveRecord, ICallOnCreated
    {
        #region Built-in testing
        [NonSerialized]
        static TestRepository<BanquetMenu> _testRepo;
       
        static void SetTestRepo()
        {
            _testRepo = _testRepo ?? new TestRepository<BanquetMenu>(new Jaxis.Inventory.Data.BeverageMonitorDB());
        }

        public static void ResetTestRepo()
        {
            _testRepo = null;
            SetTestRepo();
        }

        public static void Setup(List<BanquetMenu> testlist)
        {
            SetTestRepo();
            foreach (var item in testlist)
            {
                _testRepo._items.Add(item);
            }
        }

        public static void Setup(BanquetMenu item) 
        {
            SetTestRepo();
            _testRepo._items.Add(item);
        }

        public static void Setup(int testItems) 
        {
            SetTestRepo();
            for(int i=0;i<testItems;i++)
            {
                BanquetMenu item=new BanquetMenu();
                _testRepo._items.Add(item);
            }
        }
        
        public bool TestMode = false;
        #endregion

        IRepository<BanquetMenu> _repo;

        partial void OnCreated();
            
        partial void OnLoaded();
        
        partial void OnSaving();
        
        partial void OnSaved();
        
        partial void OnChanged();

        public void SetIsLoaded(bool isLoaded)
        {
            _isLoaded=isLoaded;
            if(isLoaded)
                OnLoaded();
        }
        
        Jaxis.Inventory.Data.BeverageMonitorDB _db;
        
        public BanquetMenu()
        {
            m_Internal = new DataBanquetMenu();
             _db=new Jaxis.Inventory.Data.BeverageMonitorDB();
            Init();   
            this.BanquetID = Guid.NewGuid( );     
        }

        ///<summary>
        ///Set bool to true to assign NewGuid to PrimaryKey (if appropriate) and to call OnCreated
        ///</summary>
        public BanquetMenu( bool _CallOnCreated )
        {
            m_Internal = new DataBanquetMenu();
            _db=new Jaxis.Inventory.Data.BeverageMonitorDB();
            Init();  
            CallOnCreated( _CallOnCreated );
        }

        public BanquetMenu(string connectionString, string providerName) 
        {
            m_Internal = new DataBanquetMenu();
            _db=new Jaxis.Inventory.Data.BeverageMonitorDB(connectionString, providerName);
            Init();            
            this.BanquetID = Guid.NewGuid( );     
        }

        public BanquetMenu( BanquetMenu _Item )
        {
            m_Internal = new DataBanquetMenu();
            Copy( _Item );
            _db=new Jaxis.Inventory.Data.BeverageMonitorDB();
            Init();            
        }

        public BanquetMenu( DataBanquetMenu _Item )
        {
            m_Internal.Copy( _Item );
            _db=new Jaxis.Inventory.Data.BeverageMonitorDB();
            Init();            
        }         
         
        public void Copy( BanquetMenu _Item )
        {
            m_Internal.MenuItemID = _Item.MenuItemID;			
        }         

        public void CallOnCreated( bool _CallOnCreated )
        {
            if( _CallOnCreated )
            {
                m_Internal.BanquetID = Guid.NewGuid( );     
	
                OnCreated( );
            }
        }
         
        void Init()
        {
            TestMode=this._db.DataProvider.ConnectionString.Equals("test", StringComparison.InvariantCultureIgnoreCase);
            _dirtyColumns=new List<IColumn>();
            if(TestMode)
            {
                BanquetMenu.SetTestRepo();
                _repo=_testRepo;
            }
            else
            {
                _repo = new SubSonicRepository<BanquetMenu>(_db);
            }
            tbl=_repo.GetTable();
            SetIsNew(true);
        }

        public BanquetMenu(Expression<Func<BanquetMenu, bool>> expression):this() 
        {
            SetIsLoaded(_repo.Load(this,expression));
        }
        
        internal static IRepository<BanquetMenu> GetRepo(string connectionString, string providerName)
        {
            Jaxis.Inventory.Data.BeverageMonitorDB db;

            db = String.IsNullOrEmpty(connectionString) ? new Jaxis.Inventory.Data.BeverageMonitorDB() : new Jaxis.Inventory.Data.BeverageMonitorDB(connectionString, providerName);

            /*
            if(String.IsNullOrEmpty(connectionString))
            {
                db=new Jaxis.Inventory.Data.BeverageMonitorDB();
            }
            else
            {
                db=new Jaxis.Inventory.Data.BeverageMonitorDB(connectionString, providerName);
            }
            */

            IRepository<BanquetMenu> _repo;
            
            if(db.TestMode)
            {
                BanquetMenu.SetTestRepo();
                _repo=_testRepo;
            }
            else
            {
                _repo = new SubSonicRepository<BanquetMenu>(db);
            }
            return _repo;        
        }       
        
        internal static IRepository<BanquetMenu> GetRepo()
        {
            return GetRepo("","");
        }
        
        public static BanquetMenu SingleOrDefault(Expression<Func<BanquetMenu, bool>> expression) 
        {
            return SingleOrDefault( expression, String.Empty, String.Empty );
        }      
        
        public static BanquetMenu SingleOrDefault(Expression<Func<BanquetMenu, bool>> expression,string connectionString, string providerName) 
        {
            IRepository<BanquetMenu> repo = GetRepo(connectionString,providerName);
            BanquetMenu single = repo.SingleOrDefault<BanquetMenu>( expression );
            if( null != single )
            {
                single.OnLoaded( );
            }
            return single;
        }
        
        public static bool Exists(Expression<Func<BanquetMenu, bool>> expression,string connectionString, string providerName) 
        {
            return All(connectionString,providerName).Any(expression);
        }        
        
        public static bool Exists(Expression<Func<BanquetMenu, bool>> expression) 
        {
            return All().Any(expression);
        }        

        protected static bool IsEmptyBanquetMenuLoaded = false;
        protected static BanquetMenu EmptyBanquetMenuMember = null;

        public static BanquetMenu GetByID(Guid? value) 
        {
            BanquetMenu rc = null;
            if( value.HasValue )
            {
                rc = GetByID( value.Value );
            }
            return rc;
        }
        
        public static BanquetMenu GetByID(Guid value) 
        {
            BanquetMenu rc = null;
            if( null != value )
            {
                if( value == Guid.Empty )
                {
                    if( true == IsEmptyBanquetMenuLoaded )
                    {
                        rc = EmptyBanquetMenuMember;
                    }
                    else
                    {
                        IsEmptyBanquetMenuLoaded = true;
                        rc = BanquetMenu.Find( L => L.BanquetID.Equals( value ) ).FirstOrDefault( );
                        EmptyBanquetMenuMember = rc;
                    }
                }
                else
                {
                    rc = BanquetMenu.Find( L => L.BanquetID.Equals( value ) ).FirstOrDefault( );
                } 
            }
            return rc;
        }

        public static IList<BanquetMenu> Find(Expression<Func<BanquetMenu, bool>> expression) 
        {
            var repo = GetRepo();
            return repo.Find(expression).ToList();
        }
        
        public static IList<BanquetMenu> Find(Expression<Func<BanquetMenu, bool>> expression,string connectionString, string providerName) 
        {
            var repo = GetRepo(connectionString,providerName);
            return repo.Find(expression).ToList();
        }

        public static IQueryable<BanquetMenu> All(string connectionString, string providerName) 
        {
            return GetRepo(connectionString,providerName).GetAll();
        }

        public static IQueryable<BanquetMenu> All() 
        {
            return GetRepo().GetAll();
        }
        
        public static PagedList<BanquetMenu> GetPaged(string sortBy, int pageIndex, int pageSize,string connectionString, string providerName) 
        {
            return GetRepo(connectionString,providerName).GetPaged(sortBy, pageIndex, pageSize);
        }
      
        public static PagedList<BanquetMenu> GetPaged(string sortBy, int pageIndex, int pageSize) 
        {
            return GetRepo().GetPaged(sortBy, pageIndex, pageSize);
        }

        public static PagedList<BanquetMenu> GetPaged(int pageIndex, int pageSize,string connectionString, string providerName) 
        {
            return GetRepo(connectionString,providerName).GetPaged(pageIndex, pageSize);
        }

        public static PagedList<BanquetMenu> GetPaged(int pageIndex, int pageSize) 
        {
            return GetRepo().GetPaged(pageIndex, pageSize);
        }

        public string KeyName()
        {
            return "BanquetID";
        }

        public object KeyValue()
        {
            return this.BanquetID;
        }
        
        public void SetKeyValue(object value) 
        {
            if (value != null && value!=DBNull.Value) 
            {
                var settable = value.ChangeTypeTo<Guid>();
                this.GetType().GetProperty(this.KeyName()).SetValue(this, settable, null);
            }
        }
        
//        public override string ToString()
//        {
//			string rc = string.Empty;
//			if( null != this.MenuItemID )
//			{
//				rc = this.MenuItemID.ToString();
//			}
//            return rc;
//        }

        public override bool Equals(object obj)
        {
            if(obj == null)
                return false;
            if(obj is BanquetMenu)
            {
                BanquetMenu compare=(BanquetMenu)obj;
                return compare.KeyValue().Equals( this.KeyValue() );
                //return compare.KeyValue()==this.KeyValue();
            }
            else
            {
                return base.Equals(obj);
            }
        }


        public override int GetHashCode() 
        {
            return this.BanquetID.GetHashCode( );
        }

        public string DescriptorValue()
        {
            return this.MenuItemID.ToString();
        }

        public string DescriptorColumn() 
        {
            return "MenuItemID";
        }

        public static string GetKeyColumn()
        {
            return "BanquetID";
        }        

        public static string GetDescriptorColumn()
        {
            return "MenuItemID";
        }
        
        #region ' Foreign Keys '
        public IQueryable<Banquet> BanquetsItem
        {
            get
            {
                  var repo=Jaxis.Inventory.Data.Banquet.GetRepo();
                  return from items in repo.GetAll()
                       where items.BanquetID == m_Internal.BanquetID
                       select items;
            }
        }
        public IQueryable<MenuItem> MenuItemsItem
        {
            get
            {
                  var repo=Jaxis.Inventory.Data.MenuItem.GetRepo();
                  return from items in repo.GetAll()
                       where items.MenuItemID == m_Internal.MenuItemID
                       select items;
            }
        }
        #endregion


		[ObfuscationAttribute(Exclude=true)]
        public virtual Guid ObjectID 
        {
            get
            {
                return m_Internal.BanquetID;
            }
            set
            {
                m_Internal.BanquetID = value;
            }
        }


//        Guid _BanquetID;
        [SubSonicPrimaryKey]
        [LocalData]
        public Guid BanquetID
        {
            get { return m_Internal.BanquetID; }
            set
            {
                if(m_Internal.BanquetID!=value){
                    m_Internal.BanquetID=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="BanquetID");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        Guid _MenuItemID;
        [LocalData]
        public Guid MenuItemID
        {
            get { return m_Internal.MenuItemID; }
            set
            {
                if(m_Internal.MenuItemID!=value){
                    m_Internal.MenuItemID=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="MenuItemID");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }


        public DbCommand GetUpdateCommand() 
        {
            if(TestMode)
                return _db.DataProvider.CreateCommand();
            else
                return this.ToUpdateQuery(_db.Provider).GetCommand().ToDbCommand();
            
        }

        public DbCommand GetInsertCommand() 
        {
            if(TestMode)
                return _db.DataProvider.CreateCommand();
            else
                return this.ToInsertQuery(_db.Provider).GetCommand().ToDbCommand();
        }
        
        public DbCommand GetDeleteCommand() 
        {
            if(TestMode)
                return _db.DataProvider.CreateCommand();
            else
                return this.ToDeleteQuery(_db.Provider).GetCommand().ToDbCommand();
        }
       
        public void Update()
        {
            Update(_db.DataProvider);
        }
        
        public void Update(IDataProvider provider)
        {
            OnSaving( );
            
            if(this._dirtyColumns.Count>0)
            {
                _repo.Update(this,provider);
                _dirtyColumns.Clear();    
            }
            OnSaved();
       }
 
        public void Add()
        {
            Add(_db.DataProvider);
        }
        
        public void Add(IDataProvider provider)
        {
            OnSaving( );

            var key=KeyValue();
            if(key==null)
            {
                var newKey=_repo.Add(this,provider);
                this.SetKeyValue(newKey);
            }
            else
            {
                _repo.Add(this,provider);
            }
            SetIsNew(false);
            OnSaved();
        }
        
        public override void Save() 
        {
            Save(_db.DataProvider);
        }      

        public void Save(IDataProvider provider) 
        {
            if (_isNew) 
            {
                Add(provider);
            }
            else 
            {
                Update(provider);
            }
            SetIsLoaded( true );
        }

        public void Delete(IDataProvider provider) 
        {
                   
                 
            _repo.Delete(KeyValue());
                    }

        public void Delete() 
        {
            Delete(_db.DataProvider);
        }

        public static void Delete(Expression<Func<BanquetMenu, bool>> expression) 
        {
            var repo = GetRepo();
            
       
            repo.DeleteMany(expression);
        }

        
        public void Load(IDataReader rdr) 
        {
            Load(rdr, true);
        }

        public void Load(IDataReader rdr, bool closeReader) 
        {
            if (rdr.Read()) 
            {
                try 
                {
                    rdr.Load(this);
                    SetIsNew(false);
                    SetIsLoaded(true);
                }
                catch
                {
                    SetIsLoaded(false);
                    throw;
                }
            }
            else
            {
                SetIsLoaded(false);
            
            }

            if (closeReader)
                rdr.Dispose();
        }
    } 

    [DataContract]
    public partial class DataBanquet : IWCFDataElement
    {
        [DataMember]
        public Guid BanquetID { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string CustomerName { get; set; }

        public void Copy( DataBanquet _Item )
        {
             Name = _Item.Name;			
             CustomerName = _Item.CustomerName;			
        }

		public IBaseDataObject CreateDBObject( )
        {
            return new Banquet( this );
        }


    }


    /// <summary>
    /// A class which represents the Banquets table in the BeverageMonitor Database.
    /// </summary>
    public partial class Banquet: BaseDataObject<DataBanquet>, IActiveRecord, ICallOnCreated
    {
        #region Built-in testing
        [NonSerialized]
        static TestRepository<Banquet> _testRepo;
       
        static void SetTestRepo()
        {
            _testRepo = _testRepo ?? new TestRepository<Banquet>(new Jaxis.Inventory.Data.BeverageMonitorDB());
        }

        public static void ResetTestRepo()
        {
            _testRepo = null;
            SetTestRepo();
        }

        public static void Setup(List<Banquet> testlist)
        {
            SetTestRepo();
            foreach (var item in testlist)
            {
                _testRepo._items.Add(item);
            }
        }

        public static void Setup(Banquet item) 
        {
            SetTestRepo();
            _testRepo._items.Add(item);
        }

        public static void Setup(int testItems) 
        {
            SetTestRepo();
            for(int i=0;i<testItems;i++)
            {
                Banquet item=new Banquet();
                _testRepo._items.Add(item);
            }
        }
        
        public bool TestMode = false;
        #endregion

        IRepository<Banquet> _repo;

        partial void OnCreated();
            
        partial void OnLoaded();
        
        partial void OnSaving();
        
        partial void OnSaved();
        
        partial void OnChanged();

        public void SetIsLoaded(bool isLoaded)
        {
            _isLoaded=isLoaded;
            if(isLoaded)
                OnLoaded();
        }
        
        Jaxis.Inventory.Data.BeverageMonitorDB _db;
        
        public Banquet()
        {
            m_Internal = new DataBanquet();
             _db=new Jaxis.Inventory.Data.BeverageMonitorDB();
            Init();   
            this.BanquetID = Guid.NewGuid( );     
        }

        ///<summary>
        ///Set bool to true to assign NewGuid to PrimaryKey (if appropriate) and to call OnCreated
        ///</summary>
        public Banquet( bool _CallOnCreated )
        {
            m_Internal = new DataBanquet();
            _db=new Jaxis.Inventory.Data.BeverageMonitorDB();
            Init();  
            CallOnCreated( _CallOnCreated );
        }

        public Banquet(string connectionString, string providerName) 
        {
            m_Internal = new DataBanquet();
            _db=new Jaxis.Inventory.Data.BeverageMonitorDB(connectionString, providerName);
            Init();            
            this.BanquetID = Guid.NewGuid( );     
        }

        public Banquet( Banquet _Item )
        {
            m_Internal = new DataBanquet();
            Copy( _Item );
            _db=new Jaxis.Inventory.Data.BeverageMonitorDB();
            Init();            
        }

        public Banquet( DataBanquet _Item )
        {
            m_Internal.Copy( _Item );
            _db=new Jaxis.Inventory.Data.BeverageMonitorDB();
            Init();            
        }         
         
        public void Copy( Banquet _Item )
        {
            m_Internal.Name = _Item.Name;			
            m_Internal.CustomerName = _Item.CustomerName;			
        }         

        public void CallOnCreated( bool _CallOnCreated )
        {
            if( _CallOnCreated )
            {
                m_Internal.BanquetID = Guid.NewGuid( );     
	
                OnCreated( );
            }
        }
         
        void Init()
        {
            TestMode=this._db.DataProvider.ConnectionString.Equals("test", StringComparison.InvariantCultureIgnoreCase);
            _dirtyColumns=new List<IColumn>();
            if(TestMode)
            {
                Banquet.SetTestRepo();
                _repo=_testRepo;
            }
            else
            {
                _repo = new SubSonicRepository<Banquet>(_db);
            }
            tbl=_repo.GetTable();
            SetIsNew(true);
        }

        public Banquet(Expression<Func<Banquet, bool>> expression):this() 
        {
            SetIsLoaded(_repo.Load(this,expression));
        }
        
        internal static IRepository<Banquet> GetRepo(string connectionString, string providerName)
        {
            Jaxis.Inventory.Data.BeverageMonitorDB db;

            db = String.IsNullOrEmpty(connectionString) ? new Jaxis.Inventory.Data.BeverageMonitorDB() : new Jaxis.Inventory.Data.BeverageMonitorDB(connectionString, providerName);

            /*
            if(String.IsNullOrEmpty(connectionString))
            {
                db=new Jaxis.Inventory.Data.BeverageMonitorDB();
            }
            else
            {
                db=new Jaxis.Inventory.Data.BeverageMonitorDB(connectionString, providerName);
            }
            */

            IRepository<Banquet> _repo;
            
            if(db.TestMode)
            {
                Banquet.SetTestRepo();
                _repo=_testRepo;
            }
            else
            {
                _repo = new SubSonicRepository<Banquet>(db);
            }
            return _repo;        
        }       
        
        internal static IRepository<Banquet> GetRepo()
        {
            return GetRepo("","");
        }
        
        public static Banquet SingleOrDefault(Expression<Func<Banquet, bool>> expression) 
        {
            return SingleOrDefault( expression, String.Empty, String.Empty );
        }      
        
        public static Banquet SingleOrDefault(Expression<Func<Banquet, bool>> expression,string connectionString, string providerName) 
        {
            IRepository<Banquet> repo = GetRepo(connectionString,providerName);
            Banquet single = repo.SingleOrDefault<Banquet>( expression );
            if( null != single )
            {
                single.OnLoaded( );
            }
            return single;
        }
        
        public static bool Exists(Expression<Func<Banquet, bool>> expression,string connectionString, string providerName) 
        {
            return All(connectionString,providerName).Any(expression);
        }        
        
        public static bool Exists(Expression<Func<Banquet, bool>> expression) 
        {
            return All().Any(expression);
        }        

        protected static bool IsEmptyBanquetLoaded = false;
        protected static Banquet EmptyBanquetMember = null;

        public static Banquet GetByID(Guid? value) 
        {
            Banquet rc = null;
            if( value.HasValue )
            {
                rc = GetByID( value.Value );
            }
            return rc;
        }
        
        public static Banquet GetByID(Guid value) 
        {
            Banquet rc = null;
            if( null != value )
            {
                if( value == Guid.Empty )
                {
                    if( true == IsEmptyBanquetLoaded )
                    {
                        rc = EmptyBanquetMember;
                    }
                    else
                    {
                        IsEmptyBanquetLoaded = true;
                        rc = Banquet.Find( L => L.BanquetID.Equals( value ) ).FirstOrDefault( );
                        EmptyBanquetMember = rc;
                    }
                }
                else
                {
                    rc = Banquet.Find( L => L.BanquetID.Equals( value ) ).FirstOrDefault( );
                } 
            }
            return rc;
        }

        public static IList<Banquet> Find(Expression<Func<Banquet, bool>> expression) 
        {
            var repo = GetRepo();
            return repo.Find(expression).ToList();
        }
        
        public static IList<Banquet> Find(Expression<Func<Banquet, bool>> expression,string connectionString, string providerName) 
        {
            var repo = GetRepo(connectionString,providerName);
            return repo.Find(expression).ToList();
        }

        public static IQueryable<Banquet> All(string connectionString, string providerName) 
        {
            return GetRepo(connectionString,providerName).GetAll();
        }

        public static IQueryable<Banquet> All() 
        {
            return GetRepo().GetAll();
        }
        
        public static PagedList<Banquet> GetPaged(string sortBy, int pageIndex, int pageSize,string connectionString, string providerName) 
        {
            return GetRepo(connectionString,providerName).GetPaged(sortBy, pageIndex, pageSize);
        }
      
        public static PagedList<Banquet> GetPaged(string sortBy, int pageIndex, int pageSize) 
        {
            return GetRepo().GetPaged(sortBy, pageIndex, pageSize);
        }

        public static PagedList<Banquet> GetPaged(int pageIndex, int pageSize,string connectionString, string providerName) 
        {
            return GetRepo(connectionString,providerName).GetPaged(pageIndex, pageSize);
        }

        public static PagedList<Banquet> GetPaged(int pageIndex, int pageSize) 
        {
            return GetRepo().GetPaged(pageIndex, pageSize);
        }

        public string KeyName()
        {
            return "BanquetID";
        }

        public object KeyValue()
        {
            return this.BanquetID;
        }
        
        public void SetKeyValue(object value) 
        {
            if (value != null && value!=DBNull.Value) 
            {
                var settable = value.ChangeTypeTo<Guid>();
                this.GetType().GetProperty(this.KeyName()).SetValue(this, settable, null);
            }
        }
        
//        public override string ToString()
//        {
//			string rc = string.Empty;
//			if( null != this.Name )
//			{
//				rc = this.Name.ToString();
//			}
//            return rc;
//        }

        public override bool Equals(object obj)
        {
            if(obj == null)
                return false;
            if(obj is Banquet)
            {
                Banquet compare=(Banquet)obj;
                return compare.KeyValue().Equals( this.KeyValue() );
                //return compare.KeyValue()==this.KeyValue();
            }
            else
            {
                return base.Equals(obj);
            }
        }


        public override int GetHashCode() 
        {
            return this.BanquetID.GetHashCode( );
        }

        public string DescriptorValue()
        {
            return this.Name.ToString();
        }

        public string DescriptorColumn() 
        {
            return "Name";
        }

        public static string GetKeyColumn()
        {
            return "BanquetID";
        }        

        public static string GetDescriptorColumn()
        {
            return "Name";
        }
        
        #region ' Foreign Keys '
        public IQueryable<BanquetMenu> BanquetMenus
        {
            get
            {
                  var repo=Jaxis.Inventory.Data.BanquetMenu.GetRepo();
                  return from items in repo.GetAll()
                       where items.BanquetID == m_Internal.BanquetID
                       select items;
            }
        }
        #endregion


		[ObfuscationAttribute(Exclude=true)]
        public virtual Guid ObjectID 
        {
            get
            {
                return m_Internal.BanquetID;
            }
            set
            {
                m_Internal.BanquetID = value;
            }
        }


//        Guid _BanquetID;
        [SubSonicPrimaryKey]
        [LocalData]
        public Guid BanquetID
        {
            get { return m_Internal.BanquetID; }
            set
            {
                if(m_Internal.BanquetID!=value){
                    m_Internal.BanquetID=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="BanquetID");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        string _Name;
        [LocalData]
        public string Name
        {
            get { return m_Internal.Name; }
            set
            {
                if(m_Internal.Name!=value){
                    m_Internal.Name=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="Name");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        string _CustomerName;
        [LocalData]
        public string CustomerName
        {
            get { return m_Internal.CustomerName; }
            set
            {
                if(m_Internal.CustomerName!=value){
                    m_Internal.CustomerName=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="CustomerName");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }


        public DbCommand GetUpdateCommand() 
        {
            if(TestMode)
                return _db.DataProvider.CreateCommand();
            else
                return this.ToUpdateQuery(_db.Provider).GetCommand().ToDbCommand();
            
        }

        public DbCommand GetInsertCommand() 
        {
            if(TestMode)
                return _db.DataProvider.CreateCommand();
            else
                return this.ToInsertQuery(_db.Provider).GetCommand().ToDbCommand();
        }
        
        public DbCommand GetDeleteCommand() 
        {
            if(TestMode)
                return _db.DataProvider.CreateCommand();
            else
                return this.ToDeleteQuery(_db.Provider).GetCommand().ToDbCommand();
        }
       
        public void Update()
        {
            Update(_db.DataProvider);
        }
        
        public void Update(IDataProvider provider)
        {
            OnSaving( );
            
            if(this._dirtyColumns.Count>0)
            {
                _repo.Update(this,provider);
                _dirtyColumns.Clear();    
            }
            OnSaved();
       }
 
        public void Add()
        {
            Add(_db.DataProvider);
        }
        
        public void Add(IDataProvider provider)
        {
            OnSaving( );

            var key=KeyValue();
            if(key==null)
            {
                var newKey=_repo.Add(this,provider);
                this.SetKeyValue(newKey);
            }
            else
            {
                _repo.Add(this,provider);
            }
            SetIsNew(false);
            OnSaved();
        }
        
        public override void Save() 
        {
            Save(_db.DataProvider);
        }      

        public void Save(IDataProvider provider) 
        {
            if (_isNew) 
            {
                Add(provider);
            }
            else 
            {
                Update(provider);
            }
            SetIsLoaded( true );
        }

        public void Delete(IDataProvider provider) 
        {
                   
                 
            _repo.Delete(KeyValue());
                    }

        public void Delete() 
        {
            Delete(_db.DataProvider);
        }

        public static void Delete(Expression<Func<Banquet, bool>> expression) 
        {
            var repo = GetRepo();
            
       
            repo.DeleteMany(expression);
        }

        
        public void Load(IDataReader rdr) 
        {
            Load(rdr, true);
        }

        public void Load(IDataReader rdr, bool closeReader) 
        {
            if (rdr.Read()) 
            {
                try 
                {
                    rdr.Load(this);
                    SetIsNew(false);
                    SetIsLoaded(true);
                }
                catch
                {
                    SetIsLoaded(false);
                    throw;
                }
            }
            else
            {
                SetIsLoaded(false);
            
            }

            if (closeReader)
                rdr.Dispose();
        }
    } 

    [DataContract]
    public partial class DataDAT3 : IWCFDataElement
    {
        [DataMember]
        public DateTime StayDate { get; set; }
        [DataMember]
        public string MarketCategory { get; set; }
        [DataMember]
        public string RateProgramTier { get; set; }
        [DataMember]
        public string RateCategoryCode { get; set; }
        [DataMember]
        public string MarketCode { get; set; }
        [DataMember]
        public int? RoomNights { get; set; }
        [DataMember]
        public decimal? ADRNet { get; set; }
        [DataMember]
        public decimal? RevenueNet { get; set; }
        [DataMember]
        public int? AdditionalDemand { get; set; }
        [DataMember]
        public int? TotalDemand { get; set; }
        [DataMember]
        public decimal? AverageRoomNights { get; set; }
        [DataMember]
        public decimal? AverageRevenue { get; set; }

        public void Copy( DataDAT3 _Item )
        {
             MarketCategory = _Item.MarketCategory;			
             RateProgramTier = _Item.RateProgramTier;			
             RateCategoryCode = _Item.RateCategoryCode;			
             MarketCode = _Item.MarketCode;			
             RoomNights = _Item.RoomNights;			
             ADRNet = _Item.ADRNet;			
             RevenueNet = _Item.RevenueNet;			
             AdditionalDemand = _Item.AdditionalDemand;			
             TotalDemand = _Item.TotalDemand;			
             AverageRoomNights = _Item.AverageRoomNights;			
             AverageRevenue = _Item.AverageRevenue;			
        }

		public IBaseDataObject CreateDBObject( )
        {
            return new DAT3( this );
        }


    }


    /// <summary>
    /// A class which represents the DAT3 table in the BeverageMonitor Database.
    /// </summary>
    public partial class DAT3: BaseDataObject<DataDAT3>, IActiveRecord, ICallOnCreated
    {
        #region Built-in testing
        [NonSerialized]
        static TestRepository<DAT3> _testRepo;
       
        static void SetTestRepo()
        {
            _testRepo = _testRepo ?? new TestRepository<DAT3>(new Jaxis.Inventory.Data.BeverageMonitorDB());
        }

        public static void ResetTestRepo()
        {
            _testRepo = null;
            SetTestRepo();
        }

        public static void Setup(List<DAT3> testlist)
        {
            SetTestRepo();
            foreach (var item in testlist)
            {
                _testRepo._items.Add(item);
            }
        }

        public static void Setup(DAT3 item) 
        {
            SetTestRepo();
            _testRepo._items.Add(item);
        }

        public static void Setup(int testItems) 
        {
            SetTestRepo();
            for(int i=0;i<testItems;i++)
            {
                DAT3 item=new DAT3();
                _testRepo._items.Add(item);
            }
        }
        
        public bool TestMode = false;
        #endregion

        IRepository<DAT3> _repo;

        partial void OnCreated();
            
        partial void OnLoaded();
        
        partial void OnSaving();
        
        partial void OnSaved();
        
        partial void OnChanged();

        public void SetIsLoaded(bool isLoaded)
        {
            _isLoaded=isLoaded;
            if(isLoaded)
                OnLoaded();
        }
        
        Jaxis.Inventory.Data.BeverageMonitorDB _db;
        
        public DAT3()
        {
            m_Internal = new DataDAT3();
             _db=new Jaxis.Inventory.Data.BeverageMonitorDB();
            Init();   
        }

        ///<summary>
        ///Set bool to true to assign NewGuid to PrimaryKey (if appropriate) and to call OnCreated
        ///</summary>
        public DAT3( bool _CallOnCreated )
        {
            m_Internal = new DataDAT3();
            _db=new Jaxis.Inventory.Data.BeverageMonitorDB();
            Init();  
            CallOnCreated( _CallOnCreated );
        }

        public DAT3(string connectionString, string providerName) 
        {
            m_Internal = new DataDAT3();
            _db=new Jaxis.Inventory.Data.BeverageMonitorDB(connectionString, providerName);
            Init();            
        }

        public DAT3( DAT3 _Item )
        {
            m_Internal = new DataDAT3();
            Copy( _Item );
            _db=new Jaxis.Inventory.Data.BeverageMonitorDB();
            Init();            
        }

        public DAT3( DataDAT3 _Item )
        {
            m_Internal.Copy( _Item );
            _db=new Jaxis.Inventory.Data.BeverageMonitorDB();
            Init();            
        }         
         
        public void Copy( DAT3 _Item )
        {
            m_Internal.MarketCategory = _Item.MarketCategory;			
            m_Internal.RateProgramTier = _Item.RateProgramTier;			
            m_Internal.RateCategoryCode = _Item.RateCategoryCode;			
            m_Internal.MarketCode = _Item.MarketCode;			
            m_Internal.RoomNights = _Item.RoomNights;			
            m_Internal.ADRNet = _Item.ADRNet;			
            m_Internal.RevenueNet = _Item.RevenueNet;			
            m_Internal.AdditionalDemand = _Item.AdditionalDemand;			
            m_Internal.TotalDemand = _Item.TotalDemand;			
            m_Internal.AverageRoomNights = _Item.AverageRoomNights;			
            m_Internal.AverageRevenue = _Item.AverageRevenue;			
        }         

        public void CallOnCreated( bool _CallOnCreated )
        {
            if( _CallOnCreated )
            {
	
                OnCreated( );
            }
        }
         
        void Init()
        {
            TestMode=this._db.DataProvider.ConnectionString.Equals("test", StringComparison.InvariantCultureIgnoreCase);
            _dirtyColumns=new List<IColumn>();
            if(TestMode)
            {
                DAT3.SetTestRepo();
                _repo=_testRepo;
            }
            else
            {
                _repo = new SubSonicRepository<DAT3>(_db);
            }
            tbl=_repo.GetTable();
            SetIsNew(true);
        }

        public DAT3(Expression<Func<DAT3, bool>> expression):this() 
        {
            SetIsLoaded(_repo.Load(this,expression));
        }
        
        internal static IRepository<DAT3> GetRepo(string connectionString, string providerName)
        {
            Jaxis.Inventory.Data.BeverageMonitorDB db;

            db = String.IsNullOrEmpty(connectionString) ? new Jaxis.Inventory.Data.BeverageMonitorDB() : new Jaxis.Inventory.Data.BeverageMonitorDB(connectionString, providerName);

            /*
            if(String.IsNullOrEmpty(connectionString))
            {
                db=new Jaxis.Inventory.Data.BeverageMonitorDB();
            }
            else
            {
                db=new Jaxis.Inventory.Data.BeverageMonitorDB(connectionString, providerName);
            }
            */

            IRepository<DAT3> _repo;
            
            if(db.TestMode)
            {
                DAT3.SetTestRepo();
                _repo=_testRepo;
            }
            else
            {
                _repo = new SubSonicRepository<DAT3>(db);
            }
            return _repo;        
        }       
        
        internal static IRepository<DAT3> GetRepo()
        {
            return GetRepo("","");
        }
        
        public static DAT3 SingleOrDefault(Expression<Func<DAT3, bool>> expression) 
        {
            return SingleOrDefault( expression, String.Empty, String.Empty );
        }      
        
        public static DAT3 SingleOrDefault(Expression<Func<DAT3, bool>> expression,string connectionString, string providerName) 
        {
            IRepository<DAT3> repo = GetRepo(connectionString,providerName);
            DAT3 single = repo.SingleOrDefault<DAT3>( expression );
            if( null != single )
            {
                single.OnLoaded( );
            }
            return single;
        }
        
        public static bool Exists(Expression<Func<DAT3, bool>> expression,string connectionString, string providerName) 
        {
            return All(connectionString,providerName).Any(expression);
        }        
        
        public static bool Exists(Expression<Func<DAT3, bool>> expression) 
        {
            return All().Any(expression);
        }        

        
        public static DAT3 GetByID(DateTime value) 
        {
            return DAT3.Find( L => L.StayDate.Equals( value ) ).FirstOrDefault( );
        }

        public static IList<DAT3> Find(Expression<Func<DAT3, bool>> expression) 
        {
            var repo = GetRepo();
            return repo.Find(expression).ToList();
        }
        
        public static IList<DAT3> Find(Expression<Func<DAT3, bool>> expression,string connectionString, string providerName) 
        {
            var repo = GetRepo(connectionString,providerName);
            return repo.Find(expression).ToList();
        }

        public static IQueryable<DAT3> All(string connectionString, string providerName) 
        {
            return GetRepo(connectionString,providerName).GetAll();
        }

        public static IQueryable<DAT3> All() 
        {
            return GetRepo().GetAll();
        }
        
        public static PagedList<DAT3> GetPaged(string sortBy, int pageIndex, int pageSize,string connectionString, string providerName) 
        {
            return GetRepo(connectionString,providerName).GetPaged(sortBy, pageIndex, pageSize);
        }
      
        public static PagedList<DAT3> GetPaged(string sortBy, int pageIndex, int pageSize) 
        {
            return GetRepo().GetPaged(sortBy, pageIndex, pageSize);
        }

        public static PagedList<DAT3> GetPaged(int pageIndex, int pageSize,string connectionString, string providerName) 
        {
            return GetRepo(connectionString,providerName).GetPaged(pageIndex, pageSize);
        }

        public static PagedList<DAT3> GetPaged(int pageIndex, int pageSize) 
        {
            return GetRepo().GetPaged(pageIndex, pageSize);
        }

        public string KeyName()
        {
            return "StayDate";
        }

        public object KeyValue()
        {
            return this.StayDate;
        }
        
        public void SetKeyValue(object value) 
        {
            if (value != null && value!=DBNull.Value) 
            {
                var settable = value.ChangeTypeTo<DateTime>();
                this.GetType().GetProperty(this.KeyName()).SetValue(this, settable, null);
            }
        }
        
//        public override string ToString()
//        {
//			string rc = string.Empty;
//			if( null != this.MarketCategory )
//			{
//				rc = this.MarketCategory.ToString();
//			}
//            return rc;
//        }

        public override bool Equals(object obj)
        {
            if(obj == null)
                return false;
            if(obj is DAT3)
            {
                DAT3 compare=(DAT3)obj;
                return compare.KeyValue().Equals( this.KeyValue() );
                //return compare.KeyValue()==this.KeyValue();
            }
            else
            {
                return base.Equals(obj);
            }
        }



        public string DescriptorValue()
        {
            return this.MarketCategory.ToString();
        }

        public string DescriptorColumn() 
        {
            return "MarketCategory";
        }

        public static string GetKeyColumn()
        {
            return "StayDate";
        }        

        public static string GetDescriptorColumn()
        {
            return "MarketCategory";
        }
        
        #region ' Foreign Keys '
        #endregion



//        DateTime? _StayDate;
        [SubSonicPrimaryKey]
        [LocalData]
        public DateTime StayDate
        {
            get { return m_Internal.StayDate; }
            set
            {
                if(m_Internal.StayDate!=value){
                    m_Internal.StayDate=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="StayDate");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        string _MarketCategory;
        [LocalData]
        public string MarketCategory
        {
            get { return m_Internal.MarketCategory; }
            set
            {
                if(m_Internal.MarketCategory!=value){
                    m_Internal.MarketCategory=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="MarketCategory");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        string _RateProgramTier;
        [LocalData]
        public string RateProgramTier
        {
            get { return m_Internal.RateProgramTier; }
            set
            {
                if(m_Internal.RateProgramTier!=value){
                    m_Internal.RateProgramTier=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="RateProgramTier");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        string _RateCategoryCode;
        [LocalData]
        public string RateCategoryCode
        {
            get { return m_Internal.RateCategoryCode; }
            set
            {
                if(m_Internal.RateCategoryCode!=value){
                    m_Internal.RateCategoryCode=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="RateCategoryCode");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        string _MarketCode;
        [LocalData]
        public string MarketCode
        {
            get { return m_Internal.MarketCode; }
            set
            {
                if(m_Internal.MarketCode!=value){
                    m_Internal.MarketCode=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="MarketCode");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        int? _RoomNights;
        [LocalData]
        public int? RoomNights
        {
            get { return m_Internal.RoomNights; }
            set
            {
                if(m_Internal.RoomNights!=value){
                    m_Internal.RoomNights=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="RoomNights");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        decimal? _ADRNet;
        [LocalData]
        public decimal? ADRNet
        {
            get { return m_Internal.ADRNet; }
            set
            {
                if(m_Internal.ADRNet!=value){
                    m_Internal.ADRNet=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="ADRNet");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        decimal? _RevenueNet;
        [LocalData]
        public decimal? RevenueNet
        {
            get { return m_Internal.RevenueNet; }
            set
            {
                if(m_Internal.RevenueNet!=value){
                    m_Internal.RevenueNet=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="RevenueNet");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        int? _AdditionalDemand;
        [LocalData]
        public int? AdditionalDemand
        {
            get { return m_Internal.AdditionalDemand; }
            set
            {
                if(m_Internal.AdditionalDemand!=value){
                    m_Internal.AdditionalDemand=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="AdditionalDemand");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        int? _TotalDemand;
        [LocalData]
        public int? TotalDemand
        {
            get { return m_Internal.TotalDemand; }
            set
            {
                if(m_Internal.TotalDemand!=value){
                    m_Internal.TotalDemand=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="TotalDemand");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        decimal? _AverageRoomNights;
        [LocalData]
        public decimal? AverageRoomNights
        {
            get { return m_Internal.AverageRoomNights; }
            set
            {
                if(m_Internal.AverageRoomNights!=value){
                    m_Internal.AverageRoomNights=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="AverageRoomNights");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        decimal? _AverageRevenue;
        [LocalData]
        public decimal? AverageRevenue
        {
            get { return m_Internal.AverageRevenue; }
            set
            {
                if(m_Internal.AverageRevenue!=value){
                    m_Internal.AverageRevenue=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="AverageRevenue");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }


        public DbCommand GetUpdateCommand() 
        {
            if(TestMode)
                return _db.DataProvider.CreateCommand();
            else
                return this.ToUpdateQuery(_db.Provider).GetCommand().ToDbCommand();
            
        }

        public DbCommand GetInsertCommand() 
        {
            if(TestMode)
                return _db.DataProvider.CreateCommand();
            else
                return this.ToInsertQuery(_db.Provider).GetCommand().ToDbCommand();
        }
        
        public DbCommand GetDeleteCommand() 
        {
            if(TestMode)
                return _db.DataProvider.CreateCommand();
            else
                return this.ToDeleteQuery(_db.Provider).GetCommand().ToDbCommand();
        }
       
        public void Update()
        {
            Update(_db.DataProvider);
        }
        
        public void Update(IDataProvider provider)
        {
            OnSaving( );
            
            if(this._dirtyColumns.Count>0)
            {
                _repo.Update(this,provider);
                _dirtyColumns.Clear();    
            }
            OnSaved();
       }
 
        public void Add()
        {
            Add(_db.DataProvider);
        }
        
        public void Add(IDataProvider provider)
        {
            OnSaving( );

            var key=KeyValue();
            if(key==null)
            {
                var newKey=_repo.Add(this,provider);
                this.SetKeyValue(newKey);
            }
            else
            {
                _repo.Add(this,provider);
            }
            SetIsNew(false);
            OnSaved();
        }
        
        public override void Save() 
        {
            Save(_db.DataProvider);
        }      

        public void Save(IDataProvider provider) 
        {
            if (_isNew) 
            {
                Add(provider);
            }
            else 
            {
                Update(provider);
            }
            SetIsLoaded( true );
        }

        public void Delete(IDataProvider provider) 
        {
                   
                 
            _repo.Delete(KeyValue());
                    }

        public void Delete() 
        {
            Delete(_db.DataProvider);
        }

        public static void Delete(Expression<Func<DAT3, bool>> expression) 
        {
            var repo = GetRepo();
            
       
            repo.DeleteMany(expression);
        }

        
        public void Load(IDataReader rdr) 
        {
            Load(rdr, true);
        }

        public void Load(IDataReader rdr, bool closeReader) 
        {
            if (rdr.Read()) 
            {
                try 
                {
                    rdr.Load(this);
                    SetIsNew(false);
                    SetIsLoaded(true);
                }
                catch
                {
                    SetIsLoaded(false);
                    throw;
                }
            }
            else
            {
                SetIsLoaded(false);
            
            }

            if (closeReader)
                rdr.Dispose();
        }
    } 

    [DataContract]
    public partial class DataMenuItem : IWCFDataElement
    {
        [DataMember]
        public Guid MenuItemID { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public decimal Cost { get; set; }
        [DataMember]
        public decimal Price { get; set; }
        [DataMember]
        public DateTime InsertDate { get; set; }
        [DataMember]
        public DateTime UpdateDate { get; set; }

        public void Copy( DataMenuItem _Item )
        {
             Name = _Item.Name;			
             Description = _Item.Description;			
             Cost = _Item.Cost;			
             Price = _Item.Price;			
             InsertDate = _Item.InsertDate;			
             UpdateDate = _Item.UpdateDate;			
        }

		public IBaseDataObject CreateDBObject( )
        {
            return new MenuItem( this );
        }


    }


    /// <summary>
    /// A class which represents the MenuItems table in the BeverageMonitor Database.
    /// </summary>
    public partial class MenuItem: BaseDataObject<DataMenuItem>, IActiveRecord, ICallOnCreated
    {
        #region Built-in testing
        [NonSerialized]
        static TestRepository<MenuItem> _testRepo;
       
        static void SetTestRepo()
        {
            _testRepo = _testRepo ?? new TestRepository<MenuItem>(new Jaxis.Inventory.Data.BeverageMonitorDB());
        }

        public static void ResetTestRepo()
        {
            _testRepo = null;
            SetTestRepo();
        }

        public static void Setup(List<MenuItem> testlist)
        {
            SetTestRepo();
            foreach (var item in testlist)
            {
                _testRepo._items.Add(item);
            }
        }

        public static void Setup(MenuItem item) 
        {
            SetTestRepo();
            _testRepo._items.Add(item);
        }

        public static void Setup(int testItems) 
        {
            SetTestRepo();
            for(int i=0;i<testItems;i++)
            {
                MenuItem item=new MenuItem();
                _testRepo._items.Add(item);
            }
        }
        
        public bool TestMode = false;
        #endregion

        IRepository<MenuItem> _repo;

        partial void OnCreated();
            
        partial void OnLoaded();
        
        partial void OnSaving();
        
        partial void OnSaved();
        
        partial void OnChanged();

        public void SetIsLoaded(bool isLoaded)
        {
            _isLoaded=isLoaded;
            if(isLoaded)
                OnLoaded();
        }
        
        Jaxis.Inventory.Data.BeverageMonitorDB _db;
        
        public MenuItem()
        {
            m_Internal = new DataMenuItem();
             _db=new Jaxis.Inventory.Data.BeverageMonitorDB();
            Init();   
            this.MenuItemID = Guid.NewGuid( );     
        }

        ///<summary>
        ///Set bool to true to assign NewGuid to PrimaryKey (if appropriate) and to call OnCreated
        ///</summary>
        public MenuItem( bool _CallOnCreated )
        {
            m_Internal = new DataMenuItem();
            _db=new Jaxis.Inventory.Data.BeverageMonitorDB();
            Init();  
            CallOnCreated( _CallOnCreated );
        }

        public MenuItem(string connectionString, string providerName) 
        {
            m_Internal = new DataMenuItem();
            _db=new Jaxis.Inventory.Data.BeverageMonitorDB(connectionString, providerName);
            Init();            
            this.MenuItemID = Guid.NewGuid( );     
        }

        public MenuItem( MenuItem _Item )
        {
            m_Internal = new DataMenuItem();
            Copy( _Item );
            _db=new Jaxis.Inventory.Data.BeverageMonitorDB();
            Init();            
        }

        public MenuItem( DataMenuItem _Item )
        {
            m_Internal.Copy( _Item );
            _db=new Jaxis.Inventory.Data.BeverageMonitorDB();
            Init();            
        }         
         
        public void Copy( MenuItem _Item )
        {
            m_Internal.Name = _Item.Name;			
            m_Internal.Description = _Item.Description;			
            m_Internal.Cost = _Item.Cost;			
            m_Internal.Price = _Item.Price;			
            m_Internal.InsertDate = _Item.InsertDate;			
            m_Internal.UpdateDate = _Item.UpdateDate;			
        }         

        public void CallOnCreated( bool _CallOnCreated )
        {
            if( _CallOnCreated )
            {
                m_Internal.MenuItemID = Guid.NewGuid( );     
	
                OnCreated( );
            }
        }
         
        void Init()
        {
            TestMode=this._db.DataProvider.ConnectionString.Equals("test", StringComparison.InvariantCultureIgnoreCase);
            _dirtyColumns=new List<IColumn>();
            if(TestMode)
            {
                MenuItem.SetTestRepo();
                _repo=_testRepo;
            }
            else
            {
                _repo = new SubSonicRepository<MenuItem>(_db);
            }
            tbl=_repo.GetTable();
            SetIsNew(true);
        }

        public MenuItem(Expression<Func<MenuItem, bool>> expression):this() 
        {
            SetIsLoaded(_repo.Load(this,expression));
        }
        
        internal static IRepository<MenuItem> GetRepo(string connectionString, string providerName)
        {
            Jaxis.Inventory.Data.BeverageMonitorDB db;

            db = String.IsNullOrEmpty(connectionString) ? new Jaxis.Inventory.Data.BeverageMonitorDB() : new Jaxis.Inventory.Data.BeverageMonitorDB(connectionString, providerName);

            /*
            if(String.IsNullOrEmpty(connectionString))
            {
                db=new Jaxis.Inventory.Data.BeverageMonitorDB();
            }
            else
            {
                db=new Jaxis.Inventory.Data.BeverageMonitorDB(connectionString, providerName);
            }
            */

            IRepository<MenuItem> _repo;
            
            if(db.TestMode)
            {
                MenuItem.SetTestRepo();
                _repo=_testRepo;
            }
            else
            {
                _repo = new SubSonicRepository<MenuItem>(db);
            }
            return _repo;        
        }       
        
        internal static IRepository<MenuItem> GetRepo()
        {
            return GetRepo("","");
        }
        
        public static MenuItem SingleOrDefault(Expression<Func<MenuItem, bool>> expression) 
        {
            return SingleOrDefault( expression, String.Empty, String.Empty );
        }      
        
        public static MenuItem SingleOrDefault(Expression<Func<MenuItem, bool>> expression,string connectionString, string providerName) 
        {
            IRepository<MenuItem> repo = GetRepo(connectionString,providerName);
            MenuItem single = repo.SingleOrDefault<MenuItem>( expression );
            if( null != single )
            {
                single.OnLoaded( );
            }
            return single;
        }
        
        public static bool Exists(Expression<Func<MenuItem, bool>> expression,string connectionString, string providerName) 
        {
            return All(connectionString,providerName).Any(expression);
        }        
        
        public static bool Exists(Expression<Func<MenuItem, bool>> expression) 
        {
            return All().Any(expression);
        }        

        protected static bool IsEmptyMenuItemLoaded = false;
        protected static MenuItem EmptyMenuItemMember = null;

        public static MenuItem GetByID(Guid? value) 
        {
            MenuItem rc = null;
            if( value.HasValue )
            {
                rc = GetByID( value.Value );
            }
            return rc;
        }
        
        public static MenuItem GetByID(Guid value) 
        {
            MenuItem rc = null;
            if( null != value )
            {
                if( value == Guid.Empty )
                {
                    if( true == IsEmptyMenuItemLoaded )
                    {
                        rc = EmptyMenuItemMember;
                    }
                    else
                    {
                        IsEmptyMenuItemLoaded = true;
                        rc = MenuItem.Find( L => L.MenuItemID.Equals( value ) ).FirstOrDefault( );
                        EmptyMenuItemMember = rc;
                    }
                }
                else
                {
                    rc = MenuItem.Find( L => L.MenuItemID.Equals( value ) ).FirstOrDefault( );
                } 
            }
            return rc;
        }

        public static IList<MenuItem> Find(Expression<Func<MenuItem, bool>> expression) 
        {
            var repo = GetRepo();
            return repo.Find(expression).ToList();
        }
        
        public static IList<MenuItem> Find(Expression<Func<MenuItem, bool>> expression,string connectionString, string providerName) 
        {
            var repo = GetRepo(connectionString,providerName);
            return repo.Find(expression).ToList();
        }

        public static IQueryable<MenuItem> All(string connectionString, string providerName) 
        {
            return GetRepo(connectionString,providerName).GetAll();
        }

        public static IQueryable<MenuItem> All() 
        {
            return GetRepo().GetAll();
        }
        
        public static PagedList<MenuItem> GetPaged(string sortBy, int pageIndex, int pageSize,string connectionString, string providerName) 
        {
            return GetRepo(connectionString,providerName).GetPaged(sortBy, pageIndex, pageSize);
        }
      
        public static PagedList<MenuItem> GetPaged(string sortBy, int pageIndex, int pageSize) 
        {
            return GetRepo().GetPaged(sortBy, pageIndex, pageSize);
        }

        public static PagedList<MenuItem> GetPaged(int pageIndex, int pageSize,string connectionString, string providerName) 
        {
            return GetRepo(connectionString,providerName).GetPaged(pageIndex, pageSize);
        }

        public static PagedList<MenuItem> GetPaged(int pageIndex, int pageSize) 
        {
            return GetRepo().GetPaged(pageIndex, pageSize);
        }

        public string KeyName()
        {
            return "MenuItemID";
        }

        public object KeyValue()
        {
            return this.MenuItemID;
        }
        
        public void SetKeyValue(object value) 
        {
            if (value != null && value!=DBNull.Value) 
            {
                var settable = value.ChangeTypeTo<Guid>();
                this.GetType().GetProperty(this.KeyName()).SetValue(this, settable, null);
            }
        }
        
//        public override string ToString()
//        {
//			string rc = string.Empty;
//			if( null != this.Name )
//			{
//				rc = this.Name.ToString();
//			}
//            return rc;
//        }

        public override bool Equals(object obj)
        {
            if(obj == null)
                return false;
            if(obj is MenuItem)
            {
                MenuItem compare=(MenuItem)obj;
                return compare.KeyValue().Equals( this.KeyValue() );
                //return compare.KeyValue()==this.KeyValue();
            }
            else
            {
                return base.Equals(obj);
            }
        }


        public override int GetHashCode() 
        {
            return this.MenuItemID.GetHashCode( );
        }

        public string DescriptorValue()
        {
            return this.Name.ToString();
        }

        public string DescriptorColumn() 
        {
            return "Name";
        }

        public static string GetKeyColumn()
        {
            return "MenuItemID";
        }        

        public static string GetDescriptorColumn()
        {
            return "Name";
        }
        
        #region ' Foreign Keys '
        public IQueryable<BanquetMenu> BanquetMenus
        {
            get
            {
                  var repo=Jaxis.Inventory.Data.BanquetMenu.GetRepo();
                  return from items in repo.GetAll()
                       where items.MenuItemID == m_Internal.MenuItemID
                       select items;
            }
        }
        #endregion


		[ObfuscationAttribute(Exclude=true)]
        public virtual Guid ObjectID 
        {
            get
            {
                return m_Internal.MenuItemID;
            }
            set
            {
                m_Internal.MenuItemID = value;
            }
        }


//        Guid _MenuItemID;
        [SubSonicPrimaryKey]
        [LocalData]
        public Guid MenuItemID
        {
            get { return m_Internal.MenuItemID; }
            set
            {
                if(m_Internal.MenuItemID!=value){
                    m_Internal.MenuItemID=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="MenuItemID");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        string _Name;
        [LocalData]
        public string Name
        {
            get { return m_Internal.Name; }
            set
            {
                if(m_Internal.Name!=value){
                    m_Internal.Name=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="Name");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        string _Description;
        [LocalData]
        public string Description
        {
            get { return m_Internal.Description; }
            set
            {
                if(m_Internal.Description!=value){
                    m_Internal.Description=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="Description");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        decimal _Cost;
        [LocalData]
        public decimal Cost
        {
            get { return m_Internal.Cost; }
            set
            {
                if(m_Internal.Cost!=value){
                    m_Internal.Cost=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="Cost");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        decimal _Price;
        [LocalData]
        public decimal Price
        {
            get { return m_Internal.Price; }
            set
            {
                if(m_Internal.Price!=value){
                    m_Internal.Price=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="Price");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        DateTime _InsertDate;
        [LocalData]
        public DateTime InsertDate
        {
            get { return m_Internal.InsertDate; }
            set
            {
                if(m_Internal.InsertDate!=value){
                    m_Internal.InsertDate=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="InsertDate");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        DateTime _UpdateDate;
        [LocalData]
        public DateTime UpdateDate
        {
            get { return m_Internal.UpdateDate; }
            set
            {
                if(m_Internal.UpdateDate!=value){
                    m_Internal.UpdateDate=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="UpdateDate");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }


        public DbCommand GetUpdateCommand() 
        {
            if(TestMode)
                return _db.DataProvider.CreateCommand();
            else
                return this.ToUpdateQuery(_db.Provider).GetCommand().ToDbCommand();
            
        }

        public DbCommand GetInsertCommand() 
        {
            if(TestMode)
                return _db.DataProvider.CreateCommand();
            else
                return this.ToInsertQuery(_db.Provider).GetCommand().ToDbCommand();
        }
        
        public DbCommand GetDeleteCommand() 
        {
            if(TestMode)
                return _db.DataProvider.CreateCommand();
            else
                return this.ToDeleteQuery(_db.Provider).GetCommand().ToDbCommand();
        }
       
        public void Update()
        {
            Update(_db.DataProvider);
        }
        
        public void Update(IDataProvider provider)
        {
            OnSaving( );
            
            if(this._dirtyColumns.Count>0)
            {
                _repo.Update(this,provider);
                _dirtyColumns.Clear();    
            }
            OnSaved();
       }
 
        public void Add()
        {
            Add(_db.DataProvider);
        }
        
        public void Add(IDataProvider provider)
        {
            OnSaving( );

            var key=KeyValue();
            if(key==null)
            {
                var newKey=_repo.Add(this,provider);
                this.SetKeyValue(newKey);
            }
            else
            {
                _repo.Add(this,provider);
            }
            SetIsNew(false);
            OnSaved();
        }
        
        public override void Save() 
        {
            Save(_db.DataProvider);
        }      

        public void Save(IDataProvider provider) 
        {
            if (_isNew) 
            {
                Add(provider);
            }
            else 
            {
                Update(provider);
            }
            SetIsLoaded( true );
        }

        public void Delete(IDataProvider provider) 
        {
                   
                 
            _repo.Delete(KeyValue());
                    }

        public void Delete() 
        {
            Delete(_db.DataProvider);
        }

        public static void Delete(Expression<Func<MenuItem, bool>> expression) 
        {
            var repo = GetRepo();
            
       
            repo.DeleteMany(expression);
        }

        
        public void Load(IDataReader rdr) 
        {
            Load(rdr, true);
        }

        public void Load(IDataReader rdr, bool closeReader) 
        {
            if (rdr.Read()) 
            {
                try 
                {
                    rdr.Load(this);
                    SetIsNew(false);
                    SetIsLoaded(true);
                }
                catch
                {
                    SetIsLoaded(false);
                    throw;
                }
            }
            else
            {
                SetIsLoaded(false);
            
            }

            if (closeReader)
                rdr.Dispose();
        }
    } 

    [DataContract]
    public partial class DataPOSPaymentDatum : IWCFDataElement
    {
        [DataMember]
        public Guid PaymentDataID { get; set; }
        [DataMember]
        public Guid POSTicketID { get; set; }
        [DataMember]
        public string AccountNumber { get; set; }
        [DataMember]
        public string RoomNumber { get; set; }
        [DataMember]
        public string CustomerName { get; set; }
        [DataMember]
        public string PaymentType { get; set; }
        [DataMember]
        public decimal Payment { get; set; }

        public void Copy( DataPOSPaymentDatum _Item )
        {
             POSTicketID = _Item.POSTicketID;			
             AccountNumber = _Item.AccountNumber;			
             RoomNumber = _Item.RoomNumber;			
             CustomerName = _Item.CustomerName;			
             PaymentType = _Item.PaymentType;			
             Payment = _Item.Payment;			
        }

		public IBaseDataObject CreateDBObject( )
        {
            return new POSPaymentDatum( this );
        }


    }


    /// <summary>
    /// A class which represents the POSPaymentData table in the BeverageMonitor Database.
    /// </summary>
    public partial class POSPaymentDatum: BaseDataObject<DataPOSPaymentDatum>, IActiveRecord, ICallOnCreated
    {
        #region Built-in testing
        [NonSerialized]
        static TestRepository<POSPaymentDatum> _testRepo;
       
        static void SetTestRepo()
        {
            _testRepo = _testRepo ?? new TestRepository<POSPaymentDatum>(new Jaxis.Inventory.Data.BeverageMonitorDB());
        }

        public static void ResetTestRepo()
        {
            _testRepo = null;
            SetTestRepo();
        }

        public static void Setup(List<POSPaymentDatum> testlist)
        {
            SetTestRepo();
            foreach (var item in testlist)
            {
                _testRepo._items.Add(item);
            }
        }

        public static void Setup(POSPaymentDatum item) 
        {
            SetTestRepo();
            _testRepo._items.Add(item);
        }

        public static void Setup(int testItems) 
        {
            SetTestRepo();
            for(int i=0;i<testItems;i++)
            {
                POSPaymentDatum item=new POSPaymentDatum();
                _testRepo._items.Add(item);
            }
        }
        
        public bool TestMode = false;
        #endregion

        IRepository<POSPaymentDatum> _repo;

        partial void OnCreated();
            
        partial void OnLoaded();
        
        partial void OnSaving();
        
        partial void OnSaved();
        
        partial void OnChanged();

        public void SetIsLoaded(bool isLoaded)
        {
            _isLoaded=isLoaded;
            if(isLoaded)
                OnLoaded();
        }
        
        Jaxis.Inventory.Data.BeverageMonitorDB _db;
        
        public POSPaymentDatum()
        {
            m_Internal = new DataPOSPaymentDatum();
             _db=new Jaxis.Inventory.Data.BeverageMonitorDB();
            Init();   
            this.PaymentDataID = Guid.NewGuid( );     
        }

        ///<summary>
        ///Set bool to true to assign NewGuid to PrimaryKey (if appropriate) and to call OnCreated
        ///</summary>
        public POSPaymentDatum( bool _CallOnCreated )
        {
            m_Internal = new DataPOSPaymentDatum();
            _db=new Jaxis.Inventory.Data.BeverageMonitorDB();
            Init();  
            CallOnCreated( _CallOnCreated );
        }

        public POSPaymentDatum(string connectionString, string providerName) 
        {
            m_Internal = new DataPOSPaymentDatum();
            _db=new Jaxis.Inventory.Data.BeverageMonitorDB(connectionString, providerName);
            Init();            
            this.PaymentDataID = Guid.NewGuid( );     
        }

        public POSPaymentDatum( POSPaymentDatum _Item )
        {
            m_Internal = new DataPOSPaymentDatum();
            Copy( _Item );
            _db=new Jaxis.Inventory.Data.BeverageMonitorDB();
            Init();            
        }

        public POSPaymentDatum( DataPOSPaymentDatum _Item )
        {
            m_Internal.Copy( _Item );
            _db=new Jaxis.Inventory.Data.BeverageMonitorDB();
            Init();            
        }         
         
        public void Copy( POSPaymentDatum _Item )
        {
            m_Internal.POSTicketID = _Item.POSTicketID;			
            m_Internal.AccountNumber = _Item.AccountNumber;			
            m_Internal.RoomNumber = _Item.RoomNumber;			
            m_Internal.CustomerName = _Item.CustomerName;			
            m_Internal.PaymentType = _Item.PaymentType;			
            m_Internal.Payment = _Item.Payment;			
        }         

        public void CallOnCreated( bool _CallOnCreated )
        {
            if( _CallOnCreated )
            {
                m_Internal.PaymentDataID = Guid.NewGuid( );     
	
                OnCreated( );
            }
        }
         
        void Init()
        {
            TestMode=this._db.DataProvider.ConnectionString.Equals("test", StringComparison.InvariantCultureIgnoreCase);
            _dirtyColumns=new List<IColumn>();
            if(TestMode)
            {
                POSPaymentDatum.SetTestRepo();
                _repo=_testRepo;
            }
            else
            {
                _repo = new SubSonicRepository<POSPaymentDatum>(_db);
            }
            tbl=_repo.GetTable();
            SetIsNew(true);
        }

        public POSPaymentDatum(Expression<Func<POSPaymentDatum, bool>> expression):this() 
        {
            SetIsLoaded(_repo.Load(this,expression));
        }
        
        internal static IRepository<POSPaymentDatum> GetRepo(string connectionString, string providerName)
        {
            Jaxis.Inventory.Data.BeverageMonitorDB db;

            db = String.IsNullOrEmpty(connectionString) ? new Jaxis.Inventory.Data.BeverageMonitorDB() : new Jaxis.Inventory.Data.BeverageMonitorDB(connectionString, providerName);

            /*
            if(String.IsNullOrEmpty(connectionString))
            {
                db=new Jaxis.Inventory.Data.BeverageMonitorDB();
            }
            else
            {
                db=new Jaxis.Inventory.Data.BeverageMonitorDB(connectionString, providerName);
            }
            */

            IRepository<POSPaymentDatum> _repo;
            
            if(db.TestMode)
            {
                POSPaymentDatum.SetTestRepo();
                _repo=_testRepo;
            }
            else
            {
                _repo = new SubSonicRepository<POSPaymentDatum>(db);
            }
            return _repo;        
        }       
        
        internal static IRepository<POSPaymentDatum> GetRepo()
        {
            return GetRepo("","");
        }
        
        public static POSPaymentDatum SingleOrDefault(Expression<Func<POSPaymentDatum, bool>> expression) 
        {
            return SingleOrDefault( expression, String.Empty, String.Empty );
        }      
        
        public static POSPaymentDatum SingleOrDefault(Expression<Func<POSPaymentDatum, bool>> expression,string connectionString, string providerName) 
        {
            IRepository<POSPaymentDatum> repo = GetRepo(connectionString,providerName);
            POSPaymentDatum single = repo.SingleOrDefault<POSPaymentDatum>( expression );
            if( null != single )
            {
                single.OnLoaded( );
            }
            return single;
        }
        
        public static bool Exists(Expression<Func<POSPaymentDatum, bool>> expression,string connectionString, string providerName) 
        {
            return All(connectionString,providerName).Any(expression);
        }        
        
        public static bool Exists(Expression<Func<POSPaymentDatum, bool>> expression) 
        {
            return All().Any(expression);
        }        

        protected static bool IsEmptyPOSPaymentDatumLoaded = false;
        protected static POSPaymentDatum EmptyPOSPaymentDatumMember = null;

        public static POSPaymentDatum GetByID(Guid? value) 
        {
            POSPaymentDatum rc = null;
            if( value.HasValue )
            {
                rc = GetByID( value.Value );
            }
            return rc;
        }
        
        public static POSPaymentDatum GetByID(Guid value) 
        {
            POSPaymentDatum rc = null;
            if( null != value )
            {
                if( value == Guid.Empty )
                {
                    if( true == IsEmptyPOSPaymentDatumLoaded )
                    {
                        rc = EmptyPOSPaymentDatumMember;
                    }
                    else
                    {
                        IsEmptyPOSPaymentDatumLoaded = true;
                        rc = POSPaymentDatum.Find( L => L.PaymentDataID.Equals( value ) ).FirstOrDefault( );
                        EmptyPOSPaymentDatumMember = rc;
                    }
                }
                else
                {
                    rc = POSPaymentDatum.Find( L => L.PaymentDataID.Equals( value ) ).FirstOrDefault( );
                } 
            }
            return rc;
        }

        public static IList<POSPaymentDatum> Find(Expression<Func<POSPaymentDatum, bool>> expression) 
        {
            var repo = GetRepo();
            return repo.Find(expression).ToList();
        }
        
        public static IList<POSPaymentDatum> Find(Expression<Func<POSPaymentDatum, bool>> expression,string connectionString, string providerName) 
        {
            var repo = GetRepo(connectionString,providerName);
            return repo.Find(expression).ToList();
        }

        public static IQueryable<POSPaymentDatum> All(string connectionString, string providerName) 
        {
            return GetRepo(connectionString,providerName).GetAll();
        }

        public static IQueryable<POSPaymentDatum> All() 
        {
            return GetRepo().GetAll();
        }
        
        public static PagedList<POSPaymentDatum> GetPaged(string sortBy, int pageIndex, int pageSize,string connectionString, string providerName) 
        {
            return GetRepo(connectionString,providerName).GetPaged(sortBy, pageIndex, pageSize);
        }
      
        public static PagedList<POSPaymentDatum> GetPaged(string sortBy, int pageIndex, int pageSize) 
        {
            return GetRepo().GetPaged(sortBy, pageIndex, pageSize);
        }

        public static PagedList<POSPaymentDatum> GetPaged(int pageIndex, int pageSize,string connectionString, string providerName) 
        {
            return GetRepo(connectionString,providerName).GetPaged(pageIndex, pageSize);
        }

        public static PagedList<POSPaymentDatum> GetPaged(int pageIndex, int pageSize) 
        {
            return GetRepo().GetPaged(pageIndex, pageSize);
        }

        public string KeyName()
        {
            return "PaymentDataID";
        }

        public object KeyValue()
        {
            return this.PaymentDataID;
        }
        
        public void SetKeyValue(object value) 
        {
            if (value != null && value!=DBNull.Value) 
            {
                var settable = value.ChangeTypeTo<Guid>();
                this.GetType().GetProperty(this.KeyName()).SetValue(this, settable, null);
            }
        }
        
//        public override string ToString()
//        {
//			string rc = string.Empty;
//			if( null != this.AccountNumber )
//			{
//				rc = this.AccountNumber.ToString();
//			}
//            return rc;
//        }

        public override bool Equals(object obj)
        {
            if(obj == null)
                return false;
            if(obj is POSPaymentDatum)
            {
                POSPaymentDatum compare=(POSPaymentDatum)obj;
                return compare.KeyValue().Equals( this.KeyValue() );
                //return compare.KeyValue()==this.KeyValue();
            }
            else
            {
                return base.Equals(obj);
            }
        }


        public override int GetHashCode() 
        {
            return this.PaymentDataID.GetHashCode( );
        }

        public string DescriptorValue()
        {
            return this.AccountNumber.ToString();
        }

        public string DescriptorColumn() 
        {
            return "AccountNumber";
        }

        public static string GetKeyColumn()
        {
            return "PaymentDataID";
        }        

        public static string GetDescriptorColumn()
        {
            return "AccountNumber";
        }
        
        #region ' Foreign Keys '
        public IQueryable<POSTicket> POSTicketsItem
        {
            get
            {
                  var repo=Jaxis.Inventory.Data.POSTicket.GetRepo();
                  return from items in repo.GetAll()
                       where items.POSTicketID == m_Internal.POSTicketID
                       select items;
            }
        }
        #endregion


		[ObfuscationAttribute(Exclude=true)]
        public virtual Guid ObjectID 
        {
            get
            {
                return m_Internal.PaymentDataID;
            }
            set
            {
                m_Internal.PaymentDataID = value;
            }
        }


//        Guid _PaymentDataID;
        [SubSonicPrimaryKey]
        [LocalData]
        public Guid PaymentDataID
        {
            get { return m_Internal.PaymentDataID; }
            set
            {
                if(m_Internal.PaymentDataID!=value){
                    m_Internal.PaymentDataID=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="PaymentDataID");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        Guid _POSTicketID;
        [LocalData]
        public Guid POSTicketID
        {
            get { return m_Internal.POSTicketID; }
            set
            {
                if(m_Internal.POSTicketID!=value){
                    m_Internal.POSTicketID=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="POSTicketID");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        string _AccountNumber;
        [LocalData]
        public string AccountNumber
        {
            get { return m_Internal.AccountNumber; }
            set
            {
                if(m_Internal.AccountNumber!=value){
                    m_Internal.AccountNumber=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="AccountNumber");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        string _RoomNumber;
        [LocalData]
        public string RoomNumber
        {
            get { return m_Internal.RoomNumber; }
            set
            {
                if(m_Internal.RoomNumber!=value){
                    m_Internal.RoomNumber=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="RoomNumber");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        string _CustomerName;
        [LocalData]
        public string CustomerName
        {
            get { return m_Internal.CustomerName; }
            set
            {
                if(m_Internal.CustomerName!=value){
                    m_Internal.CustomerName=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="CustomerName");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        string _PaymentType;
        [LocalData]
        public string PaymentType
        {
            get { return m_Internal.PaymentType; }
            set
            {
                if(m_Internal.PaymentType!=value){
                    m_Internal.PaymentType=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="PaymentType");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        decimal _Payment;
        [LocalData]
        public decimal Payment
        {
            get { return m_Internal.Payment; }
            set
            {
                if(m_Internal.Payment!=value){
                    m_Internal.Payment=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="Payment");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }


        public DbCommand GetUpdateCommand() 
        {
            if(TestMode)
                return _db.DataProvider.CreateCommand();
            else
                return this.ToUpdateQuery(_db.Provider).GetCommand().ToDbCommand();
            
        }

        public DbCommand GetInsertCommand() 
        {
            if(TestMode)
                return _db.DataProvider.CreateCommand();
            else
                return this.ToInsertQuery(_db.Provider).GetCommand().ToDbCommand();
        }
        
        public DbCommand GetDeleteCommand() 
        {
            if(TestMode)
                return _db.DataProvider.CreateCommand();
            else
                return this.ToDeleteQuery(_db.Provider).GetCommand().ToDbCommand();
        }
       
        public void Update()
        {
            Update(_db.DataProvider);
        }
        
        public void Update(IDataProvider provider)
        {
            OnSaving( );
            
            if(this._dirtyColumns.Count>0)
            {
                _repo.Update(this,provider);
                _dirtyColumns.Clear();    
            }
            OnSaved();
       }
 
        public void Add()
        {
            Add(_db.DataProvider);
        }
        
        public void Add(IDataProvider provider)
        {
            OnSaving( );

            var key=KeyValue();
            if(key==null)
            {
                var newKey=_repo.Add(this,provider);
                this.SetKeyValue(newKey);
            }
            else
            {
                _repo.Add(this,provider);
            }
            SetIsNew(false);
            OnSaved();
        }
        
        public override void Save() 
        {
            Save(_db.DataProvider);
        }      

        public void Save(IDataProvider provider) 
        {
            if (_isNew) 
            {
                Add(provider);
            }
            else 
            {
                Update(provider);
            }
            SetIsLoaded( true );
        }

        public void Delete(IDataProvider provider) 
        {
                   
                 
            _repo.Delete(KeyValue());
                    }

        public void Delete() 
        {
            Delete(_db.DataProvider);
        }

        public static void Delete(Expression<Func<POSPaymentDatum, bool>> expression) 
        {
            var repo = GetRepo();
            
       
            repo.DeleteMany(expression);
        }

        
        public void Load(IDataReader rdr) 
        {
            Load(rdr, true);
        }

        public void Load(IDataReader rdr, bool closeReader) 
        {
            if (rdr.Read()) 
            {
                try 
                {
                    rdr.Load(this);
                    SetIsNew(false);
                    SetIsLoaded(true);
                }
                catch
                {
                    SetIsLoaded(false);
                    throw;
                }
            }
            else
            {
                SetIsLoaded(false);
            
            }

            if (closeReader)
                rdr.Dispose();
        }
    } 

    [DataContract]
    public partial class DataPOSTicketItemModifier : IWCFDataElement
    {
        [DataMember]
        public Guid POSTicketITemModifierID { get; set; }
        [DataMember]
        public Guid POSTicketItemID { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public decimal? Price { get; set; }

        public void Copy( DataPOSTicketItemModifier _Item )
        {
             POSTicketItemID = _Item.POSTicketItemID;			
             Name = _Item.Name;			
             Price = _Item.Price;			
        }

		public IBaseDataObject CreateDBObject( )
        {
            return new POSTicketItemModifier( this );
        }


    }


    /// <summary>
    /// A class which represents the POSTicketItemModifiers table in the BeverageMonitor Database.
    /// </summary>
    public partial class POSTicketItemModifier: BaseDataObject<DataPOSTicketItemModifier>, IActiveRecord, ICallOnCreated
    {
        #region Built-in testing
        [NonSerialized]
        static TestRepository<POSTicketItemModifier> _testRepo;
       
        static void SetTestRepo()
        {
            _testRepo = _testRepo ?? new TestRepository<POSTicketItemModifier>(new Jaxis.Inventory.Data.BeverageMonitorDB());
        }

        public static void ResetTestRepo()
        {
            _testRepo = null;
            SetTestRepo();
        }

        public static void Setup(List<POSTicketItemModifier> testlist)
        {
            SetTestRepo();
            foreach (var item in testlist)
            {
                _testRepo._items.Add(item);
            }
        }

        public static void Setup(POSTicketItemModifier item) 
        {
            SetTestRepo();
            _testRepo._items.Add(item);
        }

        public static void Setup(int testItems) 
        {
            SetTestRepo();
            for(int i=0;i<testItems;i++)
            {
                POSTicketItemModifier item=new POSTicketItemModifier();
                _testRepo._items.Add(item);
            }
        }
        
        public bool TestMode = false;
        #endregion

        IRepository<POSTicketItemModifier> _repo;

        partial void OnCreated();
            
        partial void OnLoaded();
        
        partial void OnSaving();
        
        partial void OnSaved();
        
        partial void OnChanged();

        public void SetIsLoaded(bool isLoaded)
        {
            _isLoaded=isLoaded;
            if(isLoaded)
                OnLoaded();
        }
        
        Jaxis.Inventory.Data.BeverageMonitorDB _db;
        
        public POSTicketItemModifier()
        {
            m_Internal = new DataPOSTicketItemModifier();
             _db=new Jaxis.Inventory.Data.BeverageMonitorDB();
            Init();   
            this.POSTicketITemModifierID = Guid.NewGuid( );     
        }

        ///<summary>
        ///Set bool to true to assign NewGuid to PrimaryKey (if appropriate) and to call OnCreated
        ///</summary>
        public POSTicketItemModifier( bool _CallOnCreated )
        {
            m_Internal = new DataPOSTicketItemModifier();
            _db=new Jaxis.Inventory.Data.BeverageMonitorDB();
            Init();  
            CallOnCreated( _CallOnCreated );
        }

        public POSTicketItemModifier(string connectionString, string providerName) 
        {
            m_Internal = new DataPOSTicketItemModifier();
            _db=new Jaxis.Inventory.Data.BeverageMonitorDB(connectionString, providerName);
            Init();            
            this.POSTicketITemModifierID = Guid.NewGuid( );     
        }

        public POSTicketItemModifier( POSTicketItemModifier _Item )
        {
            m_Internal = new DataPOSTicketItemModifier();
            Copy( _Item );
            _db=new Jaxis.Inventory.Data.BeverageMonitorDB();
            Init();            
        }

        public POSTicketItemModifier( DataPOSTicketItemModifier _Item )
        {
            m_Internal.Copy( _Item );
            _db=new Jaxis.Inventory.Data.BeverageMonitorDB();
            Init();            
        }         
         
        public void Copy( POSTicketItemModifier _Item )
        {
            m_Internal.POSTicketItemID = _Item.POSTicketItemID;			
            m_Internal.Name = _Item.Name;			
            m_Internal.Price = _Item.Price;			
        }         

        public void CallOnCreated( bool _CallOnCreated )
        {
            if( _CallOnCreated )
            {
                m_Internal.POSTicketITemModifierID = Guid.NewGuid( );     
	
                OnCreated( );
            }
        }
         
        void Init()
        {
            TestMode=this._db.DataProvider.ConnectionString.Equals("test", StringComparison.InvariantCultureIgnoreCase);
            _dirtyColumns=new List<IColumn>();
            if(TestMode)
            {
                POSTicketItemModifier.SetTestRepo();
                _repo=_testRepo;
            }
            else
            {
                _repo = new SubSonicRepository<POSTicketItemModifier>(_db);
            }
            tbl=_repo.GetTable();
            SetIsNew(true);
        }

        public POSTicketItemModifier(Expression<Func<POSTicketItemModifier, bool>> expression):this() 
        {
            SetIsLoaded(_repo.Load(this,expression));
        }
        
        internal static IRepository<POSTicketItemModifier> GetRepo(string connectionString, string providerName)
        {
            Jaxis.Inventory.Data.BeverageMonitorDB db;

            db = String.IsNullOrEmpty(connectionString) ? new Jaxis.Inventory.Data.BeverageMonitorDB() : new Jaxis.Inventory.Data.BeverageMonitorDB(connectionString, providerName);

            /*
            if(String.IsNullOrEmpty(connectionString))
            {
                db=new Jaxis.Inventory.Data.BeverageMonitorDB();
            }
            else
            {
                db=new Jaxis.Inventory.Data.BeverageMonitorDB(connectionString, providerName);
            }
            */

            IRepository<POSTicketItemModifier> _repo;
            
            if(db.TestMode)
            {
                POSTicketItemModifier.SetTestRepo();
                _repo=_testRepo;
            }
            else
            {
                _repo = new SubSonicRepository<POSTicketItemModifier>(db);
            }
            return _repo;        
        }       
        
        internal static IRepository<POSTicketItemModifier> GetRepo()
        {
            return GetRepo("","");
        }
        
        public static POSTicketItemModifier SingleOrDefault(Expression<Func<POSTicketItemModifier, bool>> expression) 
        {
            return SingleOrDefault( expression, String.Empty, String.Empty );
        }      
        
        public static POSTicketItemModifier SingleOrDefault(Expression<Func<POSTicketItemModifier, bool>> expression,string connectionString, string providerName) 
        {
            IRepository<POSTicketItemModifier> repo = GetRepo(connectionString,providerName);
            POSTicketItemModifier single = repo.SingleOrDefault<POSTicketItemModifier>( expression );
            if( null != single )
            {
                single.OnLoaded( );
            }
            return single;
        }
        
        public static bool Exists(Expression<Func<POSTicketItemModifier, bool>> expression,string connectionString, string providerName) 
        {
            return All(connectionString,providerName).Any(expression);
        }        
        
        public static bool Exists(Expression<Func<POSTicketItemModifier, bool>> expression) 
        {
            return All().Any(expression);
        }        

        protected static bool IsEmptyPOSTicketItemModifierLoaded = false;
        protected static POSTicketItemModifier EmptyPOSTicketItemModifierMember = null;

        public static POSTicketItemModifier GetByID(Guid? value) 
        {
            POSTicketItemModifier rc = null;
            if( value.HasValue )
            {
                rc = GetByID( value.Value );
            }
            return rc;
        }
        
        public static POSTicketItemModifier GetByID(Guid value) 
        {
            POSTicketItemModifier rc = null;
            if( null != value )
            {
                if( value == Guid.Empty )
                {
                    if( true == IsEmptyPOSTicketItemModifierLoaded )
                    {
                        rc = EmptyPOSTicketItemModifierMember;
                    }
                    else
                    {
                        IsEmptyPOSTicketItemModifierLoaded = true;
                        rc = POSTicketItemModifier.Find( L => L.POSTicketITemModifierID.Equals( value ) ).FirstOrDefault( );
                        EmptyPOSTicketItemModifierMember = rc;
                    }
                }
                else
                {
                    rc = POSTicketItemModifier.Find( L => L.POSTicketITemModifierID.Equals( value ) ).FirstOrDefault( );
                } 
            }
            return rc;
        }

        public static IList<POSTicketItemModifier> Find(Expression<Func<POSTicketItemModifier, bool>> expression) 
        {
            var repo = GetRepo();
            return repo.Find(expression).ToList();
        }
        
        public static IList<POSTicketItemModifier> Find(Expression<Func<POSTicketItemModifier, bool>> expression,string connectionString, string providerName) 
        {
            var repo = GetRepo(connectionString,providerName);
            return repo.Find(expression).ToList();
        }

        public static IQueryable<POSTicketItemModifier> All(string connectionString, string providerName) 
        {
            return GetRepo(connectionString,providerName).GetAll();
        }

        public static IQueryable<POSTicketItemModifier> All() 
        {
            return GetRepo().GetAll();
        }
        
        public static PagedList<POSTicketItemModifier> GetPaged(string sortBy, int pageIndex, int pageSize,string connectionString, string providerName) 
        {
            return GetRepo(connectionString,providerName).GetPaged(sortBy, pageIndex, pageSize);
        }
      
        public static PagedList<POSTicketItemModifier> GetPaged(string sortBy, int pageIndex, int pageSize) 
        {
            return GetRepo().GetPaged(sortBy, pageIndex, pageSize);
        }

        public static PagedList<POSTicketItemModifier> GetPaged(int pageIndex, int pageSize,string connectionString, string providerName) 
        {
            return GetRepo(connectionString,providerName).GetPaged(pageIndex, pageSize);
        }

        public static PagedList<POSTicketItemModifier> GetPaged(int pageIndex, int pageSize) 
        {
            return GetRepo().GetPaged(pageIndex, pageSize);
        }

        public string KeyName()
        {
            return "POSTicketITemModifierID";
        }

        public object KeyValue()
        {
            return this.POSTicketITemModifierID;
        }
        
        public void SetKeyValue(object value) 
        {
            if (value != null && value!=DBNull.Value) 
            {
                var settable = value.ChangeTypeTo<Guid>();
                this.GetType().GetProperty(this.KeyName()).SetValue(this, settable, null);
            }
        }
        
//        public override string ToString()
//        {
//			string rc = string.Empty;
//			if( null != this.Name )
//			{
//				rc = this.Name.ToString();
//			}
//            return rc;
//        }

        public override bool Equals(object obj)
        {
            if(obj == null)
                return false;
            if(obj is POSTicketItemModifier)
            {
                POSTicketItemModifier compare=(POSTicketItemModifier)obj;
                return compare.KeyValue().Equals( this.KeyValue() );
                //return compare.KeyValue()==this.KeyValue();
            }
            else
            {
                return base.Equals(obj);
            }
        }


        public override int GetHashCode() 
        {
            return this.POSTicketITemModifierID.GetHashCode( );
        }

        public string DescriptorValue()
        {
            return this.Name.ToString();
        }

        public string DescriptorColumn() 
        {
            return "Name";
        }

        public static string GetKeyColumn()
        {
            return "POSTicketITemModifierID";
        }        

        public static string GetDescriptorColumn()
        {
            return "Name";
        }
        
        #region ' Foreign Keys '
        public IQueryable<POSTicketItem> POSTicketItemsItem
        {
            get
            {
                  var repo=Jaxis.Inventory.Data.POSTicketItem.GetRepo();
                  return from items in repo.GetAll()
                       where items.POSTicketItemID == m_Internal.POSTicketItemID
                       select items;
            }
        }
        #endregion


		[ObfuscationAttribute(Exclude=true)]
        public virtual Guid ObjectID 
        {
            get
            {
                return m_Internal.POSTicketITemModifierID;
            }
            set
            {
                m_Internal.POSTicketITemModifierID = value;
            }
        }


//        Guid _POSTicketITemModifierID;
        [SubSonicPrimaryKey]
        [LocalData]
        public Guid POSTicketITemModifierID
        {
            get { return m_Internal.POSTicketITemModifierID; }
            set
            {
                if(m_Internal.POSTicketITemModifierID!=value){
                    m_Internal.POSTicketITemModifierID=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="POSTicketITemModifierID");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        Guid _POSTicketItemID;
        [LocalData]
        public Guid POSTicketItemID
        {
            get { return m_Internal.POSTicketItemID; }
            set
            {
                if(m_Internal.POSTicketItemID!=value){
                    m_Internal.POSTicketItemID=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="POSTicketItemID");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        string _Name;
        [LocalData]
        public string Name
        {
            get { return m_Internal.Name; }
            set
            {
                if(m_Internal.Name!=value){
                    m_Internal.Name=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="Name");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        decimal? _Price;
        [LocalData]
        public decimal? Price
        {
            get { return m_Internal.Price; }
            set
            {
                if(m_Internal.Price!=value){
                    m_Internal.Price=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="Price");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }


        public DbCommand GetUpdateCommand() 
        {
            if(TestMode)
                return _db.DataProvider.CreateCommand();
            else
                return this.ToUpdateQuery(_db.Provider).GetCommand().ToDbCommand();
            
        }

        public DbCommand GetInsertCommand() 
        {
            if(TestMode)
                return _db.DataProvider.CreateCommand();
            else
                return this.ToInsertQuery(_db.Provider).GetCommand().ToDbCommand();
        }
        
        public DbCommand GetDeleteCommand() 
        {
            if(TestMode)
                return _db.DataProvider.CreateCommand();
            else
                return this.ToDeleteQuery(_db.Provider).GetCommand().ToDbCommand();
        }
       
        public void Update()
        {
            Update(_db.DataProvider);
        }
        
        public void Update(IDataProvider provider)
        {
            OnSaving( );
            
            if(this._dirtyColumns.Count>0)
            {
                _repo.Update(this,provider);
                _dirtyColumns.Clear();    
            }
            OnSaved();
       }
 
        public void Add()
        {
            Add(_db.DataProvider);
        }
        
        public void Add(IDataProvider provider)
        {
            OnSaving( );

            var key=KeyValue();
            if(key==null)
            {
                var newKey=_repo.Add(this,provider);
                this.SetKeyValue(newKey);
            }
            else
            {
                _repo.Add(this,provider);
            }
            SetIsNew(false);
            OnSaved();
        }
        
        public override void Save() 
        {
            Save(_db.DataProvider);
        }      

        public void Save(IDataProvider provider) 
        {
            if (_isNew) 
            {
                Add(provider);
            }
            else 
            {
                Update(provider);
            }
            SetIsLoaded( true );
        }

        public void Delete(IDataProvider provider) 
        {
                   
                 
            _repo.Delete(KeyValue());
                    }

        public void Delete() 
        {
            Delete(_db.DataProvider);
        }

        public static void Delete(Expression<Func<POSTicketItemModifier, bool>> expression) 
        {
            var repo = GetRepo();
            
       
            repo.DeleteMany(expression);
        }

        
        public void Load(IDataReader rdr) 
        {
            Load(rdr, true);
        }

        public void Load(IDataReader rdr, bool closeReader) 
        {
            if (rdr.Read()) 
            {
                try 
                {
                    rdr.Load(this);
                    SetIsNew(false);
                    SetIsLoaded(true);
                }
                catch
                {
                    SetIsLoaded(false);
                    throw;
                }
            }
            else
            {
                SetIsLoaded(false);
            
            }

            if (closeReader)
                rdr.Dispose();
        }
    } 

    [DataContract]
    public partial class DataPOSTicketItem : IWCFDataElement
    {
        [DataMember]
        public Guid POSTicketItemID { get; set; }
        [DataMember]
        public Guid POSTicketID { get; set; }
        [DataMember]
        public string Comment { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public decimal? Price { get; set; }
        [DataMember]
        public int Reconciled { get; set; }
        [DataMember]
        public int Quantity { get; set; }
        [DataMember]
        public int Status { get; set; }
        [DataMember]
        public bool Credit { get; set; }

        public void Copy( DataPOSTicketItem _Item )
        {
             POSTicketID = _Item.POSTicketID;			
             Comment = _Item.Comment;			
             Description = _Item.Description;			
             Price = _Item.Price;			
             Reconciled = _Item.Reconciled;			
             Quantity = _Item.Quantity;			
             Status = _Item.Status;			
             Credit = _Item.Credit;			
        }

		public IBaseDataObject CreateDBObject( )
        {
            return new POSTicketItem( this );
        }


    }


    /// <summary>
    /// A class which represents the POSTicketItems table in the BeverageMonitor Database.
    /// </summary>
    public partial class POSTicketItem: BaseDataObject<DataPOSTicketItem>, IActiveRecord, ICallOnCreated
    {
        #region Built-in testing
        [NonSerialized]
        static TestRepository<POSTicketItem> _testRepo;
       
        static void SetTestRepo()
        {
            _testRepo = _testRepo ?? new TestRepository<POSTicketItem>(new Jaxis.Inventory.Data.BeverageMonitorDB());
        }

        public static void ResetTestRepo()
        {
            _testRepo = null;
            SetTestRepo();
        }

        public static void Setup(List<POSTicketItem> testlist)
        {
            SetTestRepo();
            foreach (var item in testlist)
            {
                _testRepo._items.Add(item);
            }
        }

        public static void Setup(POSTicketItem item) 
        {
            SetTestRepo();
            _testRepo._items.Add(item);
        }

        public static void Setup(int testItems) 
        {
            SetTestRepo();
            for(int i=0;i<testItems;i++)
            {
                POSTicketItem item=new POSTicketItem();
                _testRepo._items.Add(item);
            }
        }
        
        public bool TestMode = false;
        #endregion

        IRepository<POSTicketItem> _repo;

        partial void OnCreated();
            
        partial void OnLoaded();
        
        partial void OnSaving();
        
        partial void OnSaved();
        
        partial void OnChanged();

        public void SetIsLoaded(bool isLoaded)
        {
            _isLoaded=isLoaded;
            if(isLoaded)
                OnLoaded();
        }
        
        Jaxis.Inventory.Data.BeverageMonitorDB _db;
        
        public POSTicketItem()
        {
            m_Internal = new DataPOSTicketItem();
             _db=new Jaxis.Inventory.Data.BeverageMonitorDB();
            Init();   
            this.POSTicketItemID = Guid.NewGuid( );     
        }

        ///<summary>
        ///Set bool to true to assign NewGuid to PrimaryKey (if appropriate) and to call OnCreated
        ///</summary>
        public POSTicketItem( bool _CallOnCreated )
        {
            m_Internal = new DataPOSTicketItem();
            _db=new Jaxis.Inventory.Data.BeverageMonitorDB();
            Init();  
            CallOnCreated( _CallOnCreated );
        }

        public POSTicketItem(string connectionString, string providerName) 
        {
            m_Internal = new DataPOSTicketItem();
            _db=new Jaxis.Inventory.Data.BeverageMonitorDB(connectionString, providerName);
            Init();            
            this.POSTicketItemID = Guid.NewGuid( );     
        }

        public POSTicketItem( POSTicketItem _Item )
        {
            m_Internal = new DataPOSTicketItem();
            Copy( _Item );
            _db=new Jaxis.Inventory.Data.BeverageMonitorDB();
            Init();            
        }

        public POSTicketItem( DataPOSTicketItem _Item )
        {
            m_Internal.Copy( _Item );
            _db=new Jaxis.Inventory.Data.BeverageMonitorDB();
            Init();            
        }         
         
        public void Copy( POSTicketItem _Item )
        {
            m_Internal.POSTicketID = _Item.POSTicketID;			
            m_Internal.Comment = _Item.Comment;			
            m_Internal.Description = _Item.Description;			
            m_Internal.Price = _Item.Price;			
            m_Internal.Reconciled = _Item.Reconciled;			
            m_Internal.Quantity = _Item.Quantity;			
            m_Internal.Status = _Item.Status;			
            m_Internal.Credit = _Item.Credit;			
        }         

        public void CallOnCreated( bool _CallOnCreated )
        {
            if( _CallOnCreated )
            {
                m_Internal.POSTicketItemID = Guid.NewGuid( );     
	
                OnCreated( );
            }
        }
         
        void Init()
        {
            TestMode=this._db.DataProvider.ConnectionString.Equals("test", StringComparison.InvariantCultureIgnoreCase);
            _dirtyColumns=new List<IColumn>();
            if(TestMode)
            {
                POSTicketItem.SetTestRepo();
                _repo=_testRepo;
            }
            else
            {
                _repo = new SubSonicRepository<POSTicketItem>(_db);
            }
            tbl=_repo.GetTable();
            SetIsNew(true);
        }

        public POSTicketItem(Expression<Func<POSTicketItem, bool>> expression):this() 
        {
            SetIsLoaded(_repo.Load(this,expression));
        }
        
        internal static IRepository<POSTicketItem> GetRepo(string connectionString, string providerName)
        {
            Jaxis.Inventory.Data.BeverageMonitorDB db;

            db = String.IsNullOrEmpty(connectionString) ? new Jaxis.Inventory.Data.BeverageMonitorDB() : new Jaxis.Inventory.Data.BeverageMonitorDB(connectionString, providerName);

            /*
            if(String.IsNullOrEmpty(connectionString))
            {
                db=new Jaxis.Inventory.Data.BeverageMonitorDB();
            }
            else
            {
                db=new Jaxis.Inventory.Data.BeverageMonitorDB(connectionString, providerName);
            }
            */

            IRepository<POSTicketItem> _repo;
            
            if(db.TestMode)
            {
                POSTicketItem.SetTestRepo();
                _repo=_testRepo;
            }
            else
            {
                _repo = new SubSonicRepository<POSTicketItem>(db);
            }
            return _repo;        
        }       
        
        internal static IRepository<POSTicketItem> GetRepo()
        {
            return GetRepo("","");
        }
        
        public static POSTicketItem SingleOrDefault(Expression<Func<POSTicketItem, bool>> expression) 
        {
            return SingleOrDefault( expression, String.Empty, String.Empty );
        }      
        
        public static POSTicketItem SingleOrDefault(Expression<Func<POSTicketItem, bool>> expression,string connectionString, string providerName) 
        {
            IRepository<POSTicketItem> repo = GetRepo(connectionString,providerName);
            POSTicketItem single = repo.SingleOrDefault<POSTicketItem>( expression );
            if( null != single )
            {
                single.OnLoaded( );
            }
            return single;
        }
        
        public static bool Exists(Expression<Func<POSTicketItem, bool>> expression,string connectionString, string providerName) 
        {
            return All(connectionString,providerName).Any(expression);
        }        
        
        public static bool Exists(Expression<Func<POSTicketItem, bool>> expression) 
        {
            return All().Any(expression);
        }        

        protected static bool IsEmptyPOSTicketItemLoaded = false;
        protected static POSTicketItem EmptyPOSTicketItemMember = null;

        public static POSTicketItem GetByID(Guid? value) 
        {
            POSTicketItem rc = null;
            if( value.HasValue )
            {
                rc = GetByID( value.Value );
            }
            return rc;
        }
        
        public static POSTicketItem GetByID(Guid value) 
        {
            POSTicketItem rc = null;
            if( null != value )
            {
                if( value == Guid.Empty )
                {
                    if( true == IsEmptyPOSTicketItemLoaded )
                    {
                        rc = EmptyPOSTicketItemMember;
                    }
                    else
                    {
                        IsEmptyPOSTicketItemLoaded = true;
                        rc = POSTicketItem.Find( L => L.POSTicketItemID.Equals( value ) ).FirstOrDefault( );
                        EmptyPOSTicketItemMember = rc;
                    }
                }
                else
                {
                    rc = POSTicketItem.Find( L => L.POSTicketItemID.Equals( value ) ).FirstOrDefault( );
                } 
            }
            return rc;
        }

        public static IList<POSTicketItem> Find(Expression<Func<POSTicketItem, bool>> expression) 
        {
            var repo = GetRepo();
            return repo.Find(expression).ToList();
        }
        
        public static IList<POSTicketItem> Find(Expression<Func<POSTicketItem, bool>> expression,string connectionString, string providerName) 
        {
            var repo = GetRepo(connectionString,providerName);
            return repo.Find(expression).ToList();
        }

        public static IQueryable<POSTicketItem> All(string connectionString, string providerName) 
        {
            return GetRepo(connectionString,providerName).GetAll();
        }

        public static IQueryable<POSTicketItem> All() 
        {
            return GetRepo().GetAll();
        }
        
        public static PagedList<POSTicketItem> GetPaged(string sortBy, int pageIndex, int pageSize,string connectionString, string providerName) 
        {
            return GetRepo(connectionString,providerName).GetPaged(sortBy, pageIndex, pageSize);
        }
      
        public static PagedList<POSTicketItem> GetPaged(string sortBy, int pageIndex, int pageSize) 
        {
            return GetRepo().GetPaged(sortBy, pageIndex, pageSize);
        }

        public static PagedList<POSTicketItem> GetPaged(int pageIndex, int pageSize,string connectionString, string providerName) 
        {
            return GetRepo(connectionString,providerName).GetPaged(pageIndex, pageSize);
        }

        public static PagedList<POSTicketItem> GetPaged(int pageIndex, int pageSize) 
        {
            return GetRepo().GetPaged(pageIndex, pageSize);
        }

        public string KeyName()
        {
            return "POSTicketItemID";
        }

        public object KeyValue()
        {
            return this.POSTicketItemID;
        }
        
        public void SetKeyValue(object value) 
        {
            if (value != null && value!=DBNull.Value) 
            {
                var settable = value.ChangeTypeTo<Guid>();
                this.GetType().GetProperty(this.KeyName()).SetValue(this, settable, null);
            }
        }
        
//        public override string ToString()
//        {
//			string rc = string.Empty;
//			if( null != this.Comment )
//			{
//				rc = this.Comment.ToString();
//			}
//            return rc;
//        }

        public override bool Equals(object obj)
        {
            if(obj == null)
                return false;
            if(obj is POSTicketItem)
            {
                POSTicketItem compare=(POSTicketItem)obj;
                return compare.KeyValue().Equals( this.KeyValue() );
                //return compare.KeyValue()==this.KeyValue();
            }
            else
            {
                return base.Equals(obj);
            }
        }


        public override int GetHashCode() 
        {
            return this.POSTicketItemID.GetHashCode( );
        }

        public string DescriptorValue()
        {
            return this.Comment.ToString();
        }

        public string DescriptorColumn() 
        {
            return "Comment";
        }

        public static string GetKeyColumn()
        {
            return "POSTicketItemID";
        }        

        public static string GetDescriptorColumn()
        {
            return "Comment";
        }
        
        #region ' Foreign Keys '
        public IQueryable<POSTicket> POSTicketsItem
        {
            get
            {
                  var repo=Jaxis.Inventory.Data.POSTicket.GetRepo();
                  return from items in repo.GetAll()
                       where items.POSTicketID == m_Internal.POSTicketID
                       select items;
            }
        }
        public IQueryable<POSTicketItemModifier> POSTicketItemModifiers
        {
            get
            {
                  var repo=Jaxis.Inventory.Data.POSTicketItemModifier.GetRepo();
                  return from items in repo.GetAll()
                       where items.POSTicketItemID == m_Internal.POSTicketItemID
                       select items;
            }
        }
        #endregion


		[ObfuscationAttribute(Exclude=true)]
        public virtual Guid ObjectID 
        {
            get
            {
                return m_Internal.POSTicketItemID;
            }
            set
            {
                m_Internal.POSTicketItemID = value;
            }
        }


//        Guid _POSTicketItemID;
        [SubSonicPrimaryKey]
        [LocalData]
        public Guid POSTicketItemID
        {
            get { return m_Internal.POSTicketItemID; }
            set
            {
                if(m_Internal.POSTicketItemID!=value){
                    m_Internal.POSTicketItemID=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="POSTicketItemID");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        Guid _POSTicketID;
        [LocalData]
        public Guid POSTicketID
        {
            get { return m_Internal.POSTicketID; }
            set
            {
                if(m_Internal.POSTicketID!=value){
                    m_Internal.POSTicketID=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="POSTicketID");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        string _Comment;
        [LocalData]
        public string Comment
        {
            get { return m_Internal.Comment; }
            set
            {
                if(m_Internal.Comment!=value){
                    m_Internal.Comment=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="Comment");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        string _Description;
        [LocalData]
        public string Description
        {
            get { return m_Internal.Description; }
            set
            {
                if(m_Internal.Description!=value){
                    m_Internal.Description=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="Description");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        decimal? _Price;
        [LocalData]
        public decimal? Price
        {
            get { return m_Internal.Price; }
            set
            {
                if(m_Internal.Price!=value){
                    m_Internal.Price=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="Price");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        int _Reconciled;
        [LocalData]
        public int Reconciled
        {
            get { return m_Internal.Reconciled; }
            set
            {
                if(m_Internal.Reconciled!=value){
                    m_Internal.Reconciled=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="Reconciled");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        int _Quantity;
        [LocalData]
        public int Quantity
        {
            get { return m_Internal.Quantity; }
            set
            {
                if(m_Internal.Quantity!=value){
                    m_Internal.Quantity=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="Quantity");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        int _Status;
        [LocalData]
        public int Status
        {
            get { return m_Internal.Status; }
            set
            {
                if(m_Internal.Status!=value){
                    m_Internal.Status=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="Status");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        bool _Credit;
        [LocalData]
        public bool Credit
        {
            get { return m_Internal.Credit; }
            set
            {
                if(m_Internal.Credit!=value){
                    m_Internal.Credit=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="Credit");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }


        public DbCommand GetUpdateCommand() 
        {
            if(TestMode)
                return _db.DataProvider.CreateCommand();
            else
                return this.ToUpdateQuery(_db.Provider).GetCommand().ToDbCommand();
            
        }

        public DbCommand GetInsertCommand() 
        {
            if(TestMode)
                return _db.DataProvider.CreateCommand();
            else
                return this.ToInsertQuery(_db.Provider).GetCommand().ToDbCommand();
        }
        
        public DbCommand GetDeleteCommand() 
        {
            if(TestMode)
                return _db.DataProvider.CreateCommand();
            else
                return this.ToDeleteQuery(_db.Provider).GetCommand().ToDbCommand();
        }
       
        public void Update()
        {
            Update(_db.DataProvider);
        }
        
        public void Update(IDataProvider provider)
        {
            OnSaving( );
            
            if(this._dirtyColumns.Count>0)
            {
                _repo.Update(this,provider);
                _dirtyColumns.Clear();    
            }
            OnSaved();
       }
 
        public void Add()
        {
            Add(_db.DataProvider);
        }
        
        public void Add(IDataProvider provider)
        {
            OnSaving( );

            var key=KeyValue();
            if(key==null)
            {
                var newKey=_repo.Add(this,provider);
                this.SetKeyValue(newKey);
            }
            else
            {
                _repo.Add(this,provider);
            }
            SetIsNew(false);
            OnSaved();
        }
        
        public override void Save() 
        {
            Save(_db.DataProvider);
        }      

        public void Save(IDataProvider provider) 
        {
            if (_isNew) 
            {
                Add(provider);
            }
            else 
            {
                Update(provider);
            }
            SetIsLoaded( true );
        }

        public void Delete(IDataProvider provider) 
        {
                   
                 
            _repo.Delete(KeyValue());
                    }

        public void Delete() 
        {
            Delete(_db.DataProvider);
        }

        public static void Delete(Expression<Func<POSTicketItem, bool>> expression) 
        {
            var repo = GetRepo();
            
       
            repo.DeleteMany(expression);
        }

        
        public void Load(IDataReader rdr) 
        {
            Load(rdr, true);
        }

        public void Load(IDataReader rdr, bool closeReader) 
        {
            if (rdr.Read()) 
            {
                try 
                {
                    rdr.Load(this);
                    SetIsNew(false);
                    SetIsLoaded(true);
                }
                catch
                {
                    SetIsLoaded(false);
                    throw;
                }
            }
            else
            {
                SetIsLoaded(false);
            
            }

            if (closeReader)
                rdr.Dispose();
        }
    } 

    [DataContract]
    public partial class DataPOSTicket : IWCFDataElement
    {
        [DataMember]
        public Guid POSTicketID { get; set; }
        [DataMember]
        public string CheckNumber { get; set; }
        [DataMember]
        public string Comments { get; set; }
        [DataMember]
        public DateTime TicketDate { get; set; }
        [DataMember]
        public string Establishment { get; set; }
        [DataMember]
        public string Server { get; set; }
        [DataMember]
        public string ServerName { get; set; }
        [DataMember]
        public int GuestCount { get; set; }
        [DataMember]
        public string CustomerTable { get; set; }
        [DataMember]
        public string RawData { get; set; }
        [DataMember]
        public int TouchCount { get; set; }
        [DataMember]
        public decimal TipAmount { get; set; }
        [DataMember]
        public int? GuestCountModified { get; set; }
        [DataMember]
        public decimal? TicketTotal { get; set; }
        [DataMember]
        public string DataSource { get; set; }
        [DataMember]
        public string TransactionID { get; set; }

        public void Copy( DataPOSTicket _Item )
        {
             CheckNumber = _Item.CheckNumber;			
             Comments = _Item.Comments;			
             TicketDate = _Item.TicketDate;			
             Establishment = _Item.Establishment;			
             Server = _Item.Server;			
             ServerName = _Item.ServerName;			
             GuestCount = _Item.GuestCount;			
             CustomerTable = _Item.CustomerTable;			
             RawData = _Item.RawData;			
             TouchCount = _Item.TouchCount;			
             TipAmount = _Item.TipAmount;			
             GuestCountModified = _Item.GuestCountModified;			
             TicketTotal = _Item.TicketTotal;			
             DataSource = _Item.DataSource;			
             TransactionID = _Item.TransactionID;			
        }

		public IBaseDataObject CreateDBObject( )
        {
            return new POSTicket( this );
        }


    }


    /// <summary>
    /// A class which represents the POSTickets table in the BeverageMonitor Database.
    /// </summary>
    public partial class POSTicket: BaseDataObject<DataPOSTicket>, IActiveRecord, ICallOnCreated
    {
        #region Built-in testing
        [NonSerialized]
        static TestRepository<POSTicket> _testRepo;
       
        static void SetTestRepo()
        {
            _testRepo = _testRepo ?? new TestRepository<POSTicket>(new Jaxis.Inventory.Data.BeverageMonitorDB());
        }

        public static void ResetTestRepo()
        {
            _testRepo = null;
            SetTestRepo();
        }

        public static void Setup(List<POSTicket> testlist)
        {
            SetTestRepo();
            foreach (var item in testlist)
            {
                _testRepo._items.Add(item);
            }
        }

        public static void Setup(POSTicket item) 
        {
            SetTestRepo();
            _testRepo._items.Add(item);
        }

        public static void Setup(int testItems) 
        {
            SetTestRepo();
            for(int i=0;i<testItems;i++)
            {
                POSTicket item=new POSTicket();
                _testRepo._items.Add(item);
            }
        }
        
        public bool TestMode = false;
        #endregion

        IRepository<POSTicket> _repo;

        partial void OnCreated();
            
        partial void OnLoaded();
        
        partial void OnSaving();
        
        partial void OnSaved();
        
        partial void OnChanged();

        public void SetIsLoaded(bool isLoaded)
        {
            _isLoaded=isLoaded;
            if(isLoaded)
                OnLoaded();
        }
        
        Jaxis.Inventory.Data.BeverageMonitorDB _db;
        
        public POSTicket()
        {
            m_Internal = new DataPOSTicket();
             _db=new Jaxis.Inventory.Data.BeverageMonitorDB();
            Init();   
            this.POSTicketID = Guid.NewGuid( );     
        }

        ///<summary>
        ///Set bool to true to assign NewGuid to PrimaryKey (if appropriate) and to call OnCreated
        ///</summary>
        public POSTicket( bool _CallOnCreated )
        {
            m_Internal = new DataPOSTicket();
            _db=new Jaxis.Inventory.Data.BeverageMonitorDB();
            Init();  
            CallOnCreated( _CallOnCreated );
        }

        public POSTicket(string connectionString, string providerName) 
        {
            m_Internal = new DataPOSTicket();
            _db=new Jaxis.Inventory.Data.BeverageMonitorDB(connectionString, providerName);
            Init();            
            this.POSTicketID = Guid.NewGuid( );     
        }

        public POSTicket( POSTicket _Item )
        {
            m_Internal = new DataPOSTicket();
            Copy( _Item );
            _db=new Jaxis.Inventory.Data.BeverageMonitorDB();
            Init();            
        }

        public POSTicket( DataPOSTicket _Item )
        {
            m_Internal.Copy( _Item );
            _db=new Jaxis.Inventory.Data.BeverageMonitorDB();
            Init();            
        }         
         
        public void Copy( POSTicket _Item )
        {
            m_Internal.CheckNumber = _Item.CheckNumber;			
            m_Internal.Comments = _Item.Comments;			
            m_Internal.TicketDate = _Item.TicketDate;			
            m_Internal.Establishment = _Item.Establishment;			
            m_Internal.Server = _Item.Server;			
            m_Internal.ServerName = _Item.ServerName;			
            m_Internal.GuestCount = _Item.GuestCount;			
            m_Internal.CustomerTable = _Item.CustomerTable;			
            m_Internal.RawData = _Item.RawData;			
            m_Internal.TouchCount = _Item.TouchCount;			
            m_Internal.TipAmount = _Item.TipAmount;			
            m_Internal.GuestCountModified = _Item.GuestCountModified;			
            m_Internal.TicketTotal = _Item.TicketTotal;			
            m_Internal.DataSource = _Item.DataSource;			
            m_Internal.TransactionID = _Item.TransactionID;			
        }         

        public void CallOnCreated( bool _CallOnCreated )
        {
            if( _CallOnCreated )
            {
                m_Internal.POSTicketID = Guid.NewGuid( );     
	
                OnCreated( );
            }
        }
         
        void Init()
        {
            TestMode=this._db.DataProvider.ConnectionString.Equals("test", StringComparison.InvariantCultureIgnoreCase);
            _dirtyColumns=new List<IColumn>();
            if(TestMode)
            {
                POSTicket.SetTestRepo();
                _repo=_testRepo;
            }
            else
            {
                _repo = new SubSonicRepository<POSTicket>(_db);
            }
            tbl=_repo.GetTable();
            SetIsNew(true);
        }

        public POSTicket(Expression<Func<POSTicket, bool>> expression):this() 
        {
            SetIsLoaded(_repo.Load(this,expression));
        }
        
        internal static IRepository<POSTicket> GetRepo(string connectionString, string providerName)
        {
            Jaxis.Inventory.Data.BeverageMonitorDB db;

            db = String.IsNullOrEmpty(connectionString) ? new Jaxis.Inventory.Data.BeverageMonitorDB() : new Jaxis.Inventory.Data.BeverageMonitorDB(connectionString, providerName);

            /*
            if(String.IsNullOrEmpty(connectionString))
            {
                db=new Jaxis.Inventory.Data.BeverageMonitorDB();
            }
            else
            {
                db=new Jaxis.Inventory.Data.BeverageMonitorDB(connectionString, providerName);
            }
            */

            IRepository<POSTicket> _repo;
            
            if(db.TestMode)
            {
                POSTicket.SetTestRepo();
                _repo=_testRepo;
            }
            else
            {
                _repo = new SubSonicRepository<POSTicket>(db);
            }
            return _repo;        
        }       
        
        internal static IRepository<POSTicket> GetRepo()
        {
            return GetRepo("","");
        }
        
        public static POSTicket SingleOrDefault(Expression<Func<POSTicket, bool>> expression) 
        {
            return SingleOrDefault( expression, String.Empty, String.Empty );
        }      
        
        public static POSTicket SingleOrDefault(Expression<Func<POSTicket, bool>> expression,string connectionString, string providerName) 
        {
            IRepository<POSTicket> repo = GetRepo(connectionString,providerName);
            POSTicket single = repo.SingleOrDefault<POSTicket>( expression );
            if( null != single )
            {
                single.OnLoaded( );
            }
            return single;
        }
        
        public static bool Exists(Expression<Func<POSTicket, bool>> expression,string connectionString, string providerName) 
        {
            return All(connectionString,providerName).Any(expression);
        }        
        
        public static bool Exists(Expression<Func<POSTicket, bool>> expression) 
        {
            return All().Any(expression);
        }        

        protected static bool IsEmptyPOSTicketLoaded = false;
        protected static POSTicket EmptyPOSTicketMember = null;

        public static POSTicket GetByID(Guid? value) 
        {
            POSTicket rc = null;
            if( value.HasValue )
            {
                rc = GetByID( value.Value );
            }
            return rc;
        }
        
        public static POSTicket GetByID(Guid value) 
        {
            POSTicket rc = null;
            if( null != value )
            {
                if( value == Guid.Empty )
                {
                    if( true == IsEmptyPOSTicketLoaded )
                    {
                        rc = EmptyPOSTicketMember;
                    }
                    else
                    {
                        IsEmptyPOSTicketLoaded = true;
                        rc = POSTicket.Find( L => L.POSTicketID.Equals( value ) ).FirstOrDefault( );
                        EmptyPOSTicketMember = rc;
                    }
                }
                else
                {
                    rc = POSTicket.Find( L => L.POSTicketID.Equals( value ) ).FirstOrDefault( );
                } 
            }
            return rc;
        }

        public static IList<POSTicket> Find(Expression<Func<POSTicket, bool>> expression) 
        {
            var repo = GetRepo();
            return repo.Find(expression).ToList();
        }
        
        public static IList<POSTicket> Find(Expression<Func<POSTicket, bool>> expression,string connectionString, string providerName) 
        {
            var repo = GetRepo(connectionString,providerName);
            return repo.Find(expression).ToList();
        }

        public static IQueryable<POSTicket> All(string connectionString, string providerName) 
        {
            return GetRepo(connectionString,providerName).GetAll();
        }

        public static IQueryable<POSTicket> All() 
        {
            return GetRepo().GetAll();
        }
        
        public static PagedList<POSTicket> GetPaged(string sortBy, int pageIndex, int pageSize,string connectionString, string providerName) 
        {
            return GetRepo(connectionString,providerName).GetPaged(sortBy, pageIndex, pageSize);
        }
      
        public static PagedList<POSTicket> GetPaged(string sortBy, int pageIndex, int pageSize) 
        {
            return GetRepo().GetPaged(sortBy, pageIndex, pageSize);
        }

        public static PagedList<POSTicket> GetPaged(int pageIndex, int pageSize,string connectionString, string providerName) 
        {
            return GetRepo(connectionString,providerName).GetPaged(pageIndex, pageSize);
        }

        public static PagedList<POSTicket> GetPaged(int pageIndex, int pageSize) 
        {
            return GetRepo().GetPaged(pageIndex, pageSize);
        }

        public string KeyName()
        {
            return "POSTicketID";
        }

        public object KeyValue()
        {
            return this.POSTicketID;
        }
        
        public void SetKeyValue(object value) 
        {
            if (value != null && value!=DBNull.Value) 
            {
                var settable = value.ChangeTypeTo<Guid>();
                this.GetType().GetProperty(this.KeyName()).SetValue(this, settable, null);
            }
        }
        
//        public override string ToString()
//        {
//			string rc = string.Empty;
//			if( null != this.CheckNumber )
//			{
//				rc = this.CheckNumber.ToString();
//			}
//            return rc;
//        }

        public override bool Equals(object obj)
        {
            if(obj == null)
                return false;
            if(obj is POSTicket)
            {
                POSTicket compare=(POSTicket)obj;
                return compare.KeyValue().Equals( this.KeyValue() );
                //return compare.KeyValue()==this.KeyValue();
            }
            else
            {
                return base.Equals(obj);
            }
        }


        public override int GetHashCode() 
        {
            return this.POSTicketID.GetHashCode( );
        }

        public string DescriptorValue()
        {
            return this.CheckNumber.ToString();
        }

        public string DescriptorColumn() 
        {
            return "CheckNumber";
        }

        public static string GetKeyColumn()
        {
            return "POSTicketID";
        }        

        public static string GetDescriptorColumn()
        {
            return "CheckNumber";
        }
        
        #region ' Foreign Keys '
        public IQueryable<POSPaymentDatum> POSPaymentData
        {
            get
            {
                  var repo=Jaxis.Inventory.Data.POSPaymentDatum.GetRepo();
                  return from items in repo.GetAll()
                       where items.POSTicketID == m_Internal.POSTicketID
                       select items;
            }
        }
        public IQueryable<POSTicketItem> POSTicketItems
        {
            get
            {
                  var repo=Jaxis.Inventory.Data.POSTicketItem.GetRepo();
                  return from items in repo.GetAll()
                       where items.POSTicketID == m_Internal.POSTicketID
                       select items;
            }
        }
        public IQueryable<POSTVADatum> POSTVAData
        {
            get
            {
                  var repo=Jaxis.Inventory.Data.POSTVADatum.GetRepo();
                  return from items in repo.GetAll()
                       where items.POSTicketID == m_Internal.POSTicketID
                       select items;
            }
        }
        #endregion


		[ObfuscationAttribute(Exclude=true)]
        public virtual Guid ObjectID 
        {
            get
            {
                return m_Internal.POSTicketID;
            }
            set
            {
                m_Internal.POSTicketID = value;
            }
        }


//        Guid _POSTicketID;
        [SubSonicPrimaryKey]
        [LocalData]
        public Guid POSTicketID
        {
            get { return m_Internal.POSTicketID; }
            set
            {
                if(m_Internal.POSTicketID!=value){
                    m_Internal.POSTicketID=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="POSTicketID");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        string _CheckNumber;
        [LocalData]
        public string CheckNumber
        {
            get { return m_Internal.CheckNumber; }
            set
            {
                if(m_Internal.CheckNumber!=value){
                    m_Internal.CheckNumber=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="CheckNumber");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        string _Comments;
        [LocalData]
        public string Comments
        {
            get { return m_Internal.Comments; }
            set
            {
                if(m_Internal.Comments!=value){
                    m_Internal.Comments=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="Comments");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        DateTime _TicketDate;
        [LocalData]
        public DateTime TicketDate
        {
            get { return m_Internal.TicketDate; }
            set
            {
                if(m_Internal.TicketDate!=value){
                    m_Internal.TicketDate=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="TicketDate");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        string _Establishment;
        [LocalData]
        public string Establishment
        {
            get { return m_Internal.Establishment; }
            set
            {
                if(m_Internal.Establishment!=value){
                    m_Internal.Establishment=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="Establishment");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        string _Server;
        [LocalData]
        public string Server
        {
            get { return m_Internal.Server; }
            set
            {
                if(m_Internal.Server!=value){
                    m_Internal.Server=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="Server");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        string _ServerName;
        [LocalData]
        public string ServerName
        {
            get { return m_Internal.ServerName; }
            set
            {
                if(m_Internal.ServerName!=value){
                    m_Internal.ServerName=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="ServerName");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        int _GuestCount;
        [LocalData]
        public int GuestCount
        {
            get { return m_Internal.GuestCount; }
            set
            {
                if(m_Internal.GuestCount!=value){
                    m_Internal.GuestCount=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="GuestCount");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        string _CustomerTable;
        [LocalData]
        public string CustomerTable
        {
            get { return m_Internal.CustomerTable; }
            set
            {
                if(m_Internal.CustomerTable!=value){
                    m_Internal.CustomerTable=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="CustomerTable");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        string _RawData;
        [LocalData]
        public string RawData
        {
            get { return m_Internal.RawData; }
            set
            {
                if(m_Internal.RawData!=value){
                    m_Internal.RawData=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="RawData");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        int _TouchCount;
        [LocalData]
        public int TouchCount
        {
            get { return m_Internal.TouchCount; }
            set
            {
                if(m_Internal.TouchCount!=value){
                    m_Internal.TouchCount=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="TouchCount");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        decimal _TipAmount;
        [LocalData]
        public decimal TipAmount
        {
            get { return m_Internal.TipAmount; }
            set
            {
                if(m_Internal.TipAmount!=value){
                    m_Internal.TipAmount=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="TipAmount");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        int? _GuestCountModified;
        [LocalData]
        public int? GuestCountModified
        {
            get { return m_Internal.GuestCountModified; }
            set
            {
                if(m_Internal.GuestCountModified!=value){
                    m_Internal.GuestCountModified=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="GuestCountModified");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        decimal? _TicketTotal;
        [LocalData]
        public decimal? TicketTotal
        {
            get { return m_Internal.TicketTotal; }
            set
            {
                if(m_Internal.TicketTotal!=value){
                    m_Internal.TicketTotal=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="TicketTotal");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        string _DataSource;
        [LocalData]
        public string DataSource
        {
            get { return m_Internal.DataSource; }
            set
            {
                if(m_Internal.DataSource!=value){
                    m_Internal.DataSource=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="DataSource");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        string _TransactionID;
        [LocalData]
        public string TransactionID
        {
            get { return m_Internal.TransactionID; }
            set
            {
                if(m_Internal.TransactionID!=value){
                    m_Internal.TransactionID=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="TransactionID");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }


        public DbCommand GetUpdateCommand() 
        {
            if(TestMode)
                return _db.DataProvider.CreateCommand();
            else
                return this.ToUpdateQuery(_db.Provider).GetCommand().ToDbCommand();
            
        }

        public DbCommand GetInsertCommand() 
        {
            if(TestMode)
                return _db.DataProvider.CreateCommand();
            else
                return this.ToInsertQuery(_db.Provider).GetCommand().ToDbCommand();
        }
        
        public DbCommand GetDeleteCommand() 
        {
            if(TestMode)
                return _db.DataProvider.CreateCommand();
            else
                return this.ToDeleteQuery(_db.Provider).GetCommand().ToDbCommand();
        }
       
        public void Update()
        {
            Update(_db.DataProvider);
        }
        
        public void Update(IDataProvider provider)
        {
            OnSaving( );
            
            if(this._dirtyColumns.Count>0)
            {
                _repo.Update(this,provider);
                _dirtyColumns.Clear();    
            }
            OnSaved();
       }
 
        public void Add()
        {
            Add(_db.DataProvider);
        }
        
        public void Add(IDataProvider provider)
        {
            OnSaving( );

            var key=KeyValue();
            if(key==null)
            {
                var newKey=_repo.Add(this,provider);
                this.SetKeyValue(newKey);
            }
            else
            {
                _repo.Add(this,provider);
            }
            SetIsNew(false);
            OnSaved();
        }
        
        public override void Save() 
        {
            Save(_db.DataProvider);
        }      

        public void Save(IDataProvider provider) 
        {
            if (_isNew) 
            {
                Add(provider);
            }
            else 
            {
                Update(provider);
            }
            SetIsLoaded( true );
        }

        public void Delete(IDataProvider provider) 
        {
                   
                 
            _repo.Delete(KeyValue());
                    }

        public void Delete() 
        {
            Delete(_db.DataProvider);
        }

        public static void Delete(Expression<Func<POSTicket, bool>> expression) 
        {
            var repo = GetRepo();
            
       
            repo.DeleteMany(expression);
        }

        
        public void Load(IDataReader rdr) 
        {
            Load(rdr, true);
        }

        public void Load(IDataReader rdr, bool closeReader) 
        {
            if (rdr.Read()) 
            {
                try 
                {
                    rdr.Load(this);
                    SetIsNew(false);
                    SetIsLoaded(true);
                }
                catch
                {
                    SetIsLoaded(false);
                    throw;
                }
            }
            else
            {
                SetIsLoaded(false);
            
            }

            if (closeReader)
                rdr.Dispose();
        }
    } 

    [DataContract]
    public partial class DataPOSTVADatum : IWCFDataElement
    {
        [DataMember]
        public Guid TVADataID { get; set; }
        [DataMember]
        public Guid POSTicketID { get; set; }
        [DataMember]
        public decimal Amount { get; set; }
        [DataMember]
        public decimal? Percentage { get; set; }
        [DataMember]
        public decimal Total { get; set; }

        public void Copy( DataPOSTVADatum _Item )
        {
             POSTicketID = _Item.POSTicketID;			
             Amount = _Item.Amount;			
             Percentage = _Item.Percentage;			
             Total = _Item.Total;			
        }

		public IBaseDataObject CreateDBObject( )
        {
            return new POSTVADatum( this );
        }


    }


    /// <summary>
    /// A class which represents the POSTVAData table in the BeverageMonitor Database.
    /// </summary>
    public partial class POSTVADatum: BaseDataObject<DataPOSTVADatum>, IActiveRecord, ICallOnCreated
    {
        #region Built-in testing
        [NonSerialized]
        static TestRepository<POSTVADatum> _testRepo;
       
        static void SetTestRepo()
        {
            _testRepo = _testRepo ?? new TestRepository<POSTVADatum>(new Jaxis.Inventory.Data.BeverageMonitorDB());
        }

        public static void ResetTestRepo()
        {
            _testRepo = null;
            SetTestRepo();
        }

        public static void Setup(List<POSTVADatum> testlist)
        {
            SetTestRepo();
            foreach (var item in testlist)
            {
                _testRepo._items.Add(item);
            }
        }

        public static void Setup(POSTVADatum item) 
        {
            SetTestRepo();
            _testRepo._items.Add(item);
        }

        public static void Setup(int testItems) 
        {
            SetTestRepo();
            for(int i=0;i<testItems;i++)
            {
                POSTVADatum item=new POSTVADatum();
                _testRepo._items.Add(item);
            }
        }
        
        public bool TestMode = false;
        #endregion

        IRepository<POSTVADatum> _repo;

        partial void OnCreated();
            
        partial void OnLoaded();
        
        partial void OnSaving();
        
        partial void OnSaved();
        
        partial void OnChanged();

        public void SetIsLoaded(bool isLoaded)
        {
            _isLoaded=isLoaded;
            if(isLoaded)
                OnLoaded();
        }
        
        Jaxis.Inventory.Data.BeverageMonitorDB _db;
        
        public POSTVADatum()
        {
            m_Internal = new DataPOSTVADatum();
             _db=new Jaxis.Inventory.Data.BeverageMonitorDB();
            Init();   
            this.TVADataID = Guid.NewGuid( );     
        }

        ///<summary>
        ///Set bool to true to assign NewGuid to PrimaryKey (if appropriate) and to call OnCreated
        ///</summary>
        public POSTVADatum( bool _CallOnCreated )
        {
            m_Internal = new DataPOSTVADatum();
            _db=new Jaxis.Inventory.Data.BeverageMonitorDB();
            Init();  
            CallOnCreated( _CallOnCreated );
        }

        public POSTVADatum(string connectionString, string providerName) 
        {
            m_Internal = new DataPOSTVADatum();
            _db=new Jaxis.Inventory.Data.BeverageMonitorDB(connectionString, providerName);
            Init();            
            this.TVADataID = Guid.NewGuid( );     
        }

        public POSTVADatum( POSTVADatum _Item )
        {
            m_Internal = new DataPOSTVADatum();
            Copy( _Item );
            _db=new Jaxis.Inventory.Data.BeverageMonitorDB();
            Init();            
        }

        public POSTVADatum( DataPOSTVADatum _Item )
        {
            m_Internal.Copy( _Item );
            _db=new Jaxis.Inventory.Data.BeverageMonitorDB();
            Init();            
        }         
         
        public void Copy( POSTVADatum _Item )
        {
            m_Internal.POSTicketID = _Item.POSTicketID;			
            m_Internal.Amount = _Item.Amount;			
            m_Internal.Percentage = _Item.Percentage;			
            m_Internal.Total = _Item.Total;			
        }         

        public void CallOnCreated( bool _CallOnCreated )
        {
            if( _CallOnCreated )
            {
                m_Internal.TVADataID = Guid.NewGuid( );     
	
                OnCreated( );
            }
        }
         
        void Init()
        {
            TestMode=this._db.DataProvider.ConnectionString.Equals("test", StringComparison.InvariantCultureIgnoreCase);
            _dirtyColumns=new List<IColumn>();
            if(TestMode)
            {
                POSTVADatum.SetTestRepo();
                _repo=_testRepo;
            }
            else
            {
                _repo = new SubSonicRepository<POSTVADatum>(_db);
            }
            tbl=_repo.GetTable();
            SetIsNew(true);
        }

        public POSTVADatum(Expression<Func<POSTVADatum, bool>> expression):this() 
        {
            SetIsLoaded(_repo.Load(this,expression));
        }
        
        internal static IRepository<POSTVADatum> GetRepo(string connectionString, string providerName)
        {
            Jaxis.Inventory.Data.BeverageMonitorDB db;

            db = String.IsNullOrEmpty(connectionString) ? new Jaxis.Inventory.Data.BeverageMonitorDB() : new Jaxis.Inventory.Data.BeverageMonitorDB(connectionString, providerName);

            /*
            if(String.IsNullOrEmpty(connectionString))
            {
                db=new Jaxis.Inventory.Data.BeverageMonitorDB();
            }
            else
            {
                db=new Jaxis.Inventory.Data.BeverageMonitorDB(connectionString, providerName);
            }
            */

            IRepository<POSTVADatum> _repo;
            
            if(db.TestMode)
            {
                POSTVADatum.SetTestRepo();
                _repo=_testRepo;
            }
            else
            {
                _repo = new SubSonicRepository<POSTVADatum>(db);
            }
            return _repo;        
        }       
        
        internal static IRepository<POSTVADatum> GetRepo()
        {
            return GetRepo("","");
        }
        
        public static POSTVADatum SingleOrDefault(Expression<Func<POSTVADatum, bool>> expression) 
        {
            return SingleOrDefault( expression, String.Empty, String.Empty );
        }      
        
        public static POSTVADatum SingleOrDefault(Expression<Func<POSTVADatum, bool>> expression,string connectionString, string providerName) 
        {
            IRepository<POSTVADatum> repo = GetRepo(connectionString,providerName);
            POSTVADatum single = repo.SingleOrDefault<POSTVADatum>( expression );
            if( null != single )
            {
                single.OnLoaded( );
            }
            return single;
        }
        
        public static bool Exists(Expression<Func<POSTVADatum, bool>> expression,string connectionString, string providerName) 
        {
            return All(connectionString,providerName).Any(expression);
        }        
        
        public static bool Exists(Expression<Func<POSTVADatum, bool>> expression) 
        {
            return All().Any(expression);
        }        

        protected static bool IsEmptyPOSTVADatumLoaded = false;
        protected static POSTVADatum EmptyPOSTVADatumMember = null;

        public static POSTVADatum GetByID(Guid? value) 
        {
            POSTVADatum rc = null;
            if( value.HasValue )
            {
                rc = GetByID( value.Value );
            }
            return rc;
        }
        
        public static POSTVADatum GetByID(Guid value) 
        {
            POSTVADatum rc = null;
            if( null != value )
            {
                if( value == Guid.Empty )
                {
                    if( true == IsEmptyPOSTVADatumLoaded )
                    {
                        rc = EmptyPOSTVADatumMember;
                    }
                    else
                    {
                        IsEmptyPOSTVADatumLoaded = true;
                        rc = POSTVADatum.Find( L => L.TVADataID.Equals( value ) ).FirstOrDefault( );
                        EmptyPOSTVADatumMember = rc;
                    }
                }
                else
                {
                    rc = POSTVADatum.Find( L => L.TVADataID.Equals( value ) ).FirstOrDefault( );
                } 
            }
            return rc;
        }

        public static IList<POSTVADatum> Find(Expression<Func<POSTVADatum, bool>> expression) 
        {
            var repo = GetRepo();
            return repo.Find(expression).ToList();
        }
        
        public static IList<POSTVADatum> Find(Expression<Func<POSTVADatum, bool>> expression,string connectionString, string providerName) 
        {
            var repo = GetRepo(connectionString,providerName);
            return repo.Find(expression).ToList();
        }

        public static IQueryable<POSTVADatum> All(string connectionString, string providerName) 
        {
            return GetRepo(connectionString,providerName).GetAll();
        }

        public static IQueryable<POSTVADatum> All() 
        {
            return GetRepo().GetAll();
        }
        
        public static PagedList<POSTVADatum> GetPaged(string sortBy, int pageIndex, int pageSize,string connectionString, string providerName) 
        {
            return GetRepo(connectionString,providerName).GetPaged(sortBy, pageIndex, pageSize);
        }
      
        public static PagedList<POSTVADatum> GetPaged(string sortBy, int pageIndex, int pageSize) 
        {
            return GetRepo().GetPaged(sortBy, pageIndex, pageSize);
        }

        public static PagedList<POSTVADatum> GetPaged(int pageIndex, int pageSize,string connectionString, string providerName) 
        {
            return GetRepo(connectionString,providerName).GetPaged(pageIndex, pageSize);
        }

        public static PagedList<POSTVADatum> GetPaged(int pageIndex, int pageSize) 
        {
            return GetRepo().GetPaged(pageIndex, pageSize);
        }

        public string KeyName()
        {
            return "TVADataID";
        }

        public object KeyValue()
        {
            return this.TVADataID;
        }
        
        public void SetKeyValue(object value) 
        {
            if (value != null && value!=DBNull.Value) 
            {
                var settable = value.ChangeTypeTo<Guid>();
                this.GetType().GetProperty(this.KeyName()).SetValue(this, settable, null);
            }
        }
        
//        public override string ToString()
//        {
//			string rc = string.Empty;
//			if( null != this.POSTicketID )
//			{
//				rc = this.POSTicketID.ToString();
//			}
//            return rc;
//        }

        public override bool Equals(object obj)
        {
            if(obj == null)
                return false;
            if(obj is POSTVADatum)
            {
                POSTVADatum compare=(POSTVADatum)obj;
                return compare.KeyValue().Equals( this.KeyValue() );
                //return compare.KeyValue()==this.KeyValue();
            }
            else
            {
                return base.Equals(obj);
            }
        }


        public override int GetHashCode() 
        {
            return this.TVADataID.GetHashCode( );
        }

        public string DescriptorValue()
        {
            return this.POSTicketID.ToString();
        }

        public string DescriptorColumn() 
        {
            return "POSTicketID";
        }

        public static string GetKeyColumn()
        {
            return "TVADataID";
        }        

        public static string GetDescriptorColumn()
        {
            return "POSTicketID";
        }
        
        #region ' Foreign Keys '
        public IQueryable<POSTicket> POSTicketsItem
        {
            get
            {
                  var repo=Jaxis.Inventory.Data.POSTicket.GetRepo();
                  return from items in repo.GetAll()
                       where items.POSTicketID == m_Internal.POSTicketID
                       select items;
            }
        }
        #endregion


		[ObfuscationAttribute(Exclude=true)]
        public virtual Guid ObjectID 
        {
            get
            {
                return m_Internal.TVADataID;
            }
            set
            {
                m_Internal.TVADataID = value;
            }
        }


//        Guid _TVADataID;
        [SubSonicPrimaryKey]
        [LocalData]
        public Guid TVADataID
        {
            get { return m_Internal.TVADataID; }
            set
            {
                if(m_Internal.TVADataID!=value){
                    m_Internal.TVADataID=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="TVADataID");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        Guid _POSTicketID;
        [LocalData]
        public Guid POSTicketID
        {
            get { return m_Internal.POSTicketID; }
            set
            {
                if(m_Internal.POSTicketID!=value){
                    m_Internal.POSTicketID=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="POSTicketID");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        decimal _Amount;
        [LocalData]
        public decimal Amount
        {
            get { return m_Internal.Amount; }
            set
            {
                if(m_Internal.Amount!=value){
                    m_Internal.Amount=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="Amount");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        decimal? _Percentage;
        [LocalData]
        public decimal? Percentage
        {
            get { return m_Internal.Percentage; }
            set
            {
                if(m_Internal.Percentage!=value){
                    m_Internal.Percentage=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="Percentage");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        decimal _Total;
        [LocalData]
        public decimal Total
        {
            get { return m_Internal.Total; }
            set
            {
                if(m_Internal.Total!=value){
                    m_Internal.Total=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="Total");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }


        public DbCommand GetUpdateCommand() 
        {
            if(TestMode)
                return _db.DataProvider.CreateCommand();
            else
                return this.ToUpdateQuery(_db.Provider).GetCommand().ToDbCommand();
            
        }

        public DbCommand GetInsertCommand() 
        {
            if(TestMode)
                return _db.DataProvider.CreateCommand();
            else
                return this.ToInsertQuery(_db.Provider).GetCommand().ToDbCommand();
        }
        
        public DbCommand GetDeleteCommand() 
        {
            if(TestMode)
                return _db.DataProvider.CreateCommand();
            else
                return this.ToDeleteQuery(_db.Provider).GetCommand().ToDbCommand();
        }
       
        public void Update()
        {
            Update(_db.DataProvider);
        }
        
        public void Update(IDataProvider provider)
        {
            OnSaving( );
            
            if(this._dirtyColumns.Count>0)
            {
                _repo.Update(this,provider);
                _dirtyColumns.Clear();    
            }
            OnSaved();
       }
 
        public void Add()
        {
            Add(_db.DataProvider);
        }
        
        public void Add(IDataProvider provider)
        {
            OnSaving( );

            var key=KeyValue();
            if(key==null)
            {
                var newKey=_repo.Add(this,provider);
                this.SetKeyValue(newKey);
            }
            else
            {
                _repo.Add(this,provider);
            }
            SetIsNew(false);
            OnSaved();
        }
        
        public override void Save() 
        {
            Save(_db.DataProvider);
        }      

        public void Save(IDataProvider provider) 
        {
            if (_isNew) 
            {
                Add(provider);
            }
            else 
            {
                Update(provider);
            }
            SetIsLoaded( true );
        }

        public void Delete(IDataProvider provider) 
        {
                   
                 
            _repo.Delete(KeyValue());
                    }

        public void Delete() 
        {
            Delete(_db.DataProvider);
        }

        public static void Delete(Expression<Func<POSTVADatum, bool>> expression) 
        {
            var repo = GetRepo();
            
       
            repo.DeleteMany(expression);
        }

        
        public void Load(IDataReader rdr) 
        {
            Load(rdr, true);
        }

        public void Load(IDataReader rdr, bool closeReader) 
        {
            if (rdr.Read()) 
            {
                try 
                {
                    rdr.Load(this);
                    SetIsNew(false);
                    SetIsLoaded(true);
                }
                catch
                {
                    SetIsLoaded(false);
                    throw;
                }
            }
            else
            {
                SetIsLoaded(false);
            
            }

            if (closeReader)
                rdr.Dispose();
        }
    } 

    [DataContract]
    public partial class DataSageImport : IWCFDataElement
    {
        [DataMember]
        public long AccountCode { get; set; }
        [DataMember]
        public int Category { get; set; }
        [DataMember]
        public string CategoryName { get; set; }
        [DataMember]
        public string CategoryNameEng { get; set; }
        [DataMember]
        public decimal? Debit { get; set; }
        [DataMember]
        public decimal? Credit { get; set; }
        [DataMember]
        public decimal? SalesRecorded { get; set; }

        public void Copy( DataSageImport _Item )
        {
             Category = _Item.Category;			
             CategoryName = _Item.CategoryName;			
             CategoryNameEng = _Item.CategoryNameEng;			
             Debit = _Item.Debit;			
             Credit = _Item.Credit;			
             SalesRecorded = _Item.SalesRecorded;			
        }

		public IBaseDataObject CreateDBObject( )
        {
            return new SageImport( this );
        }


    }


    /// <summary>
    /// A class which represents the SageImport table in the BeverageMonitor Database.
    /// </summary>
    public partial class SageImport: BaseDataObject<DataSageImport>, IActiveRecord, ICallOnCreated
    {
        #region Built-in testing
        [NonSerialized]
        static TestRepository<SageImport> _testRepo;
       
        static void SetTestRepo()
        {
            _testRepo = _testRepo ?? new TestRepository<SageImport>(new Jaxis.Inventory.Data.BeverageMonitorDB());
        }

        public static void ResetTestRepo()
        {
            _testRepo = null;
            SetTestRepo();
        }

        public static void Setup(List<SageImport> testlist)
        {
            SetTestRepo();
            foreach (var item in testlist)
            {
                _testRepo._items.Add(item);
            }
        }

        public static void Setup(SageImport item) 
        {
            SetTestRepo();
            _testRepo._items.Add(item);
        }

        public static void Setup(int testItems) 
        {
            SetTestRepo();
            for(int i=0;i<testItems;i++)
            {
                SageImport item=new SageImport();
                _testRepo._items.Add(item);
            }
        }
        
        public bool TestMode = false;
        #endregion

        IRepository<SageImport> _repo;

        partial void OnCreated();
            
        partial void OnLoaded();
        
        partial void OnSaving();
        
        partial void OnSaved();
        
        partial void OnChanged();

        public void SetIsLoaded(bool isLoaded)
        {
            _isLoaded=isLoaded;
            if(isLoaded)
                OnLoaded();
        }
        
        Jaxis.Inventory.Data.BeverageMonitorDB _db;
        
        public SageImport()
        {
            m_Internal = new DataSageImport();
             _db=new Jaxis.Inventory.Data.BeverageMonitorDB();
            Init();   
        }

        ///<summary>
        ///Set bool to true to assign NewGuid to PrimaryKey (if appropriate) and to call OnCreated
        ///</summary>
        public SageImport( bool _CallOnCreated )
        {
            m_Internal = new DataSageImport();
            _db=new Jaxis.Inventory.Data.BeverageMonitorDB();
            Init();  
            CallOnCreated( _CallOnCreated );
        }

        public SageImport(string connectionString, string providerName) 
        {
            m_Internal = new DataSageImport();
            _db=new Jaxis.Inventory.Data.BeverageMonitorDB(connectionString, providerName);
            Init();            
        }

        public SageImport( SageImport _Item )
        {
            m_Internal = new DataSageImport();
            Copy( _Item );
            _db=new Jaxis.Inventory.Data.BeverageMonitorDB();
            Init();            
        }

        public SageImport( DataSageImport _Item )
        {
            m_Internal.Copy( _Item );
            _db=new Jaxis.Inventory.Data.BeverageMonitorDB();
            Init();            
        }         
         
        public void Copy( SageImport _Item )
        {
            m_Internal.Category = _Item.Category;			
            m_Internal.CategoryName = _Item.CategoryName;			
            m_Internal.CategoryNameEng = _Item.CategoryNameEng;			
            m_Internal.Debit = _Item.Debit;			
            m_Internal.Credit = _Item.Credit;			
            m_Internal.SalesRecorded = _Item.SalesRecorded;			
        }         

        public void CallOnCreated( bool _CallOnCreated )
        {
            if( _CallOnCreated )
            {
	
                OnCreated( );
            }
        }
         
        void Init()
        {
            TestMode=this._db.DataProvider.ConnectionString.Equals("test", StringComparison.InvariantCultureIgnoreCase);
            _dirtyColumns=new List<IColumn>();
            if(TestMode)
            {
                SageImport.SetTestRepo();
                _repo=_testRepo;
            }
            else
            {
                _repo = new SubSonicRepository<SageImport>(_db);
            }
            tbl=_repo.GetTable();
            SetIsNew(true);
        }

        public SageImport(Expression<Func<SageImport, bool>> expression):this() 
        {
            SetIsLoaded(_repo.Load(this,expression));
        }
        
        internal static IRepository<SageImport> GetRepo(string connectionString, string providerName)
        {
            Jaxis.Inventory.Data.BeverageMonitorDB db;

            db = String.IsNullOrEmpty(connectionString) ? new Jaxis.Inventory.Data.BeverageMonitorDB() : new Jaxis.Inventory.Data.BeverageMonitorDB(connectionString, providerName);

            /*
            if(String.IsNullOrEmpty(connectionString))
            {
                db=new Jaxis.Inventory.Data.BeverageMonitorDB();
            }
            else
            {
                db=new Jaxis.Inventory.Data.BeverageMonitorDB(connectionString, providerName);
            }
            */

            IRepository<SageImport> _repo;
            
            if(db.TestMode)
            {
                SageImport.SetTestRepo();
                _repo=_testRepo;
            }
            else
            {
                _repo = new SubSonicRepository<SageImport>(db);
            }
            return _repo;        
        }       
        
        internal static IRepository<SageImport> GetRepo()
        {
            return GetRepo("","");
        }
        
        public static SageImport SingleOrDefault(Expression<Func<SageImport, bool>> expression) 
        {
            return SingleOrDefault( expression, String.Empty, String.Empty );
        }      
        
        public static SageImport SingleOrDefault(Expression<Func<SageImport, bool>> expression,string connectionString, string providerName) 
        {
            IRepository<SageImport> repo = GetRepo(connectionString,providerName);
            SageImport single = repo.SingleOrDefault<SageImport>( expression );
            if( null != single )
            {
                single.OnLoaded( );
            }
            return single;
        }
        
        public static bool Exists(Expression<Func<SageImport, bool>> expression,string connectionString, string providerName) 
        {
            return All(connectionString,providerName).Any(expression);
        }        
        
        public static bool Exists(Expression<Func<SageImport, bool>> expression) 
        {
            return All().Any(expression);
        }        

        
        public static SageImport GetByID(long value) 
        {
            return SageImport.Find( L => L.AccountCode.Equals( value ) ).FirstOrDefault( );
        }

        public static IList<SageImport> Find(Expression<Func<SageImport, bool>> expression) 
        {
            var repo = GetRepo();
            return repo.Find(expression).ToList();
        }
        
        public static IList<SageImport> Find(Expression<Func<SageImport, bool>> expression,string connectionString, string providerName) 
        {
            var repo = GetRepo(connectionString,providerName);
            return repo.Find(expression).ToList();
        }

        public static IQueryable<SageImport> All(string connectionString, string providerName) 
        {
            return GetRepo(connectionString,providerName).GetAll();
        }

        public static IQueryable<SageImport> All() 
        {
            return GetRepo().GetAll();
        }
        
        public static PagedList<SageImport> GetPaged(string sortBy, int pageIndex, int pageSize,string connectionString, string providerName) 
        {
            return GetRepo(connectionString,providerName).GetPaged(sortBy, pageIndex, pageSize);
        }
      
        public static PagedList<SageImport> GetPaged(string sortBy, int pageIndex, int pageSize) 
        {
            return GetRepo().GetPaged(sortBy, pageIndex, pageSize);
        }

        public static PagedList<SageImport> GetPaged(int pageIndex, int pageSize,string connectionString, string providerName) 
        {
            return GetRepo(connectionString,providerName).GetPaged(pageIndex, pageSize);
        }

        public static PagedList<SageImport> GetPaged(int pageIndex, int pageSize) 
        {
            return GetRepo().GetPaged(pageIndex, pageSize);
        }

        public string KeyName()
        {
            return "AccountCode";
        }

        public object KeyValue()
        {
            return this.AccountCode;
        }
        
        public void SetKeyValue(object value) 
        {
            if (value != null && value!=DBNull.Value) 
            {
                var settable = value.ChangeTypeTo<long>();
                this.GetType().GetProperty(this.KeyName()).SetValue(this, settable, null);
            }
        }
        
//        public override string ToString()
//        {
//			string rc = string.Empty;
//			if( null != this.CategoryName )
//			{
//				rc = this.CategoryName.ToString();
//			}
//            return rc;
//        }

        public override bool Equals(object obj)
        {
            if(obj == null)
                return false;
            if(obj is SageImport)
            {
                SageImport compare=(SageImport)obj;
                return compare.KeyValue().Equals( this.KeyValue() );
                //return compare.KeyValue()==this.KeyValue();
            }
            else
            {
                return base.Equals(obj);
            }
        }



        public string DescriptorValue()
        {
            return this.CategoryName.ToString();
        }

        public string DescriptorColumn() 
        {
            return "CategoryName";
        }

        public static string GetKeyColumn()
        {
            return "AccountCode";
        }        

        public static string GetDescriptorColumn()
        {
            return "CategoryName";
        }
        
        #region ' Foreign Keys '
        #endregion



//        long _AccountCode;
        [SubSonicPrimaryKey]
        [LocalData]
        public long AccountCode
        {
            get { return m_Internal.AccountCode; }
            set
            {
                if(m_Internal.AccountCode!=value){
                    m_Internal.AccountCode=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="AccountCode");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        int _Category;
        [LocalData]
        public int Category
        {
            get { return m_Internal.Category; }
            set
            {
                if(m_Internal.Category!=value){
                    m_Internal.Category=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="Category");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        string _CategoryName;
        [LocalData]
        public string CategoryName
        {
            get { return m_Internal.CategoryName; }
            set
            {
                if(m_Internal.CategoryName!=value){
                    m_Internal.CategoryName=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="CategoryName");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        string _CategoryNameEng;
        [LocalData]
        public string CategoryNameEng
        {
            get { return m_Internal.CategoryNameEng; }
            set
            {
                if(m_Internal.CategoryNameEng!=value){
                    m_Internal.CategoryNameEng=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="CategoryNameEng");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        decimal? _Debit;
        [LocalData]
        public decimal? Debit
        {
            get { return m_Internal.Debit; }
            set
            {
                if(m_Internal.Debit!=value){
                    m_Internal.Debit=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="Debit");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        decimal? _Credit;
        [LocalData]
        public decimal? Credit
        {
            get { return m_Internal.Credit; }
            set
            {
                if(m_Internal.Credit!=value){
                    m_Internal.Credit=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="Credit");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        decimal? _SalesRecorded;
        [LocalData]
        public decimal? SalesRecorded
        {
            get { return m_Internal.SalesRecorded; }
            set
            {
                if(m_Internal.SalesRecorded!=value){
                    m_Internal.SalesRecorded=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="SalesRecorded");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }


        public DbCommand GetUpdateCommand() 
        {
            if(TestMode)
                return _db.DataProvider.CreateCommand();
            else
                return this.ToUpdateQuery(_db.Provider).GetCommand().ToDbCommand();
            
        }

        public DbCommand GetInsertCommand() 
        {
            if(TestMode)
                return _db.DataProvider.CreateCommand();
            else
                return this.ToInsertQuery(_db.Provider).GetCommand().ToDbCommand();
        }
        
        public DbCommand GetDeleteCommand() 
        {
            if(TestMode)
                return _db.DataProvider.CreateCommand();
            else
                return this.ToDeleteQuery(_db.Provider).GetCommand().ToDbCommand();
        }
       
        public void Update()
        {
            Update(_db.DataProvider);
        }
        
        public void Update(IDataProvider provider)
        {
            OnSaving( );
            
            if(this._dirtyColumns.Count>0)
            {
                _repo.Update(this,provider);
                _dirtyColumns.Clear();    
            }
            OnSaved();
       }
 
        public void Add()
        {
            Add(_db.DataProvider);
        }
        
        public void Add(IDataProvider provider)
        {
            OnSaving( );

            var key=KeyValue();
            if(key==null)
            {
                var newKey=_repo.Add(this,provider);
                this.SetKeyValue(newKey);
            }
            else
            {
                _repo.Add(this,provider);
            }
            SetIsNew(false);
            OnSaved();
        }
        
        public override void Save() 
        {
            Save(_db.DataProvider);
        }      

        public void Save(IDataProvider provider) 
        {
            if (_isNew) 
            {
                Add(provider);
            }
            else 
            {
                Update(provider);
            }
            SetIsLoaded( true );
        }

        public void Delete(IDataProvider provider) 
        {
                   
                 
            _repo.Delete(KeyValue());
                    }

        public void Delete() 
        {
            Delete(_db.DataProvider);
        }

        public static void Delete(Expression<Func<SageImport, bool>> expression) 
        {
            var repo = GetRepo();
            
       
            repo.DeleteMany(expression);
        }

        
        public void Load(IDataReader rdr) 
        {
            Load(rdr, true);
        }

        public void Load(IDataReader rdr, bool closeReader) 
        {
            if (rdr.Read()) 
            {
                try 
                {
                    rdr.Load(this);
                    SetIsNew(false);
                    SetIsLoaded(true);
                }
                catch
                {
                    SetIsLoaded(false);
                    throw;
                }
            }
            else
            {
                SetIsLoaded(false);
            
            }

            if (closeReader)
                rdr.Dispose();
        }
    } 

    [DataContract]
    public partial class DatavwDailyGuestCount : IWCFDataElement
    {
        [DataMember]
        public DateTime TicketDate { get; set; }
        [DataMember]
        public string EstablishmentModified { get; set; }
        [DataMember]
        public int? GuestCount { get; set; }
        [DataMember]
        public int? ModifiedGuestCount { get; set; }

        public void Copy( DatavwDailyGuestCount _Item )
        {
             EstablishmentModified = _Item.EstablishmentModified;			
             GuestCount = _Item.GuestCount;			
             ModifiedGuestCount = _Item.ModifiedGuestCount;			
        }

		public IBaseDataObject CreateDBObject( )
        {
            return new vwDailyGuestCount( this );
        }


    }


    /// <summary>
    /// A class which represents the vwDailyGuestCount table in the BeverageMonitor Database.
    /// </summary>
    public partial class vwDailyGuestCount: BaseDataObject<DatavwDailyGuestCount>, IActiveRecord, ICallOnCreated
    {
        #region Built-in testing
        [NonSerialized]
        static TestRepository<vwDailyGuestCount> _testRepo;
       
        static void SetTestRepo()
        {
            _testRepo = _testRepo ?? new TestRepository<vwDailyGuestCount>(new Jaxis.Inventory.Data.BeverageMonitorDB());
        }

        public static void ResetTestRepo()
        {
            _testRepo = null;
            SetTestRepo();
        }

        public static void Setup(List<vwDailyGuestCount> testlist)
        {
            SetTestRepo();
            foreach (var item in testlist)
            {
                _testRepo._items.Add(item);
            }
        }

        public static void Setup(vwDailyGuestCount item) 
        {
            SetTestRepo();
            _testRepo._items.Add(item);
        }

        public static void Setup(int testItems) 
        {
            SetTestRepo();
            for(int i=0;i<testItems;i++)
            {
                vwDailyGuestCount item=new vwDailyGuestCount();
                _testRepo._items.Add(item);
            }
        }
        
        public bool TestMode = false;
        #endregion

        IRepository<vwDailyGuestCount> _repo;

        partial void OnCreated();
            
        partial void OnLoaded();
        
        partial void OnSaving();
        
        partial void OnSaved();
        
        partial void OnChanged();

        public void SetIsLoaded(bool isLoaded)
        {
            _isLoaded=isLoaded;
            if(isLoaded)
                OnLoaded();
        }
        
        Jaxis.Inventory.Data.BeverageMonitorDB _db;
        
        public vwDailyGuestCount()
        {
            m_Internal = new DatavwDailyGuestCount();
             _db=new Jaxis.Inventory.Data.BeverageMonitorDB();
            Init();   
        }

        ///<summary>
        ///Set bool to true to assign NewGuid to PrimaryKey (if appropriate) and to call OnCreated
        ///</summary>
        public vwDailyGuestCount( bool _CallOnCreated )
        {
            m_Internal = new DatavwDailyGuestCount();
            _db=new Jaxis.Inventory.Data.BeverageMonitorDB();
            Init();  
            CallOnCreated( _CallOnCreated );
        }

        public vwDailyGuestCount(string connectionString, string providerName) 
        {
            m_Internal = new DatavwDailyGuestCount();
            _db=new Jaxis.Inventory.Data.BeverageMonitorDB(connectionString, providerName);
            Init();            
        }

        public vwDailyGuestCount( vwDailyGuestCount _Item )
        {
            m_Internal = new DatavwDailyGuestCount();
            Copy( _Item );
            _db=new Jaxis.Inventory.Data.BeverageMonitorDB();
            Init();            
        }

        public vwDailyGuestCount( DatavwDailyGuestCount _Item )
        {
            m_Internal.Copy( _Item );
            _db=new Jaxis.Inventory.Data.BeverageMonitorDB();
            Init();            
        }         
         
        public void Copy( vwDailyGuestCount _Item )
        {
            m_Internal.EstablishmentModified = _Item.EstablishmentModified;			
            m_Internal.GuestCount = _Item.GuestCount;			
            m_Internal.ModifiedGuestCount = _Item.ModifiedGuestCount;			
        }         

        public void CallOnCreated( bool _CallOnCreated )
        {
            if( _CallOnCreated )
            {
	
                OnCreated( );
            }
        }
         
        void Init()
        {
            TestMode=this._db.DataProvider.ConnectionString.Equals("test", StringComparison.InvariantCultureIgnoreCase);
            _dirtyColumns=new List<IColumn>();
            if(TestMode)
            {
                vwDailyGuestCount.SetTestRepo();
                _repo=_testRepo;
            }
            else
            {
                _repo = new SubSonicRepository<vwDailyGuestCount>(_db);
            }
            tbl=_repo.GetTable();
            SetIsNew(true);
        }

        public vwDailyGuestCount(Expression<Func<vwDailyGuestCount, bool>> expression):this() 
        {
            SetIsLoaded(_repo.Load(this,expression));
        }
        
        internal static IRepository<vwDailyGuestCount> GetRepo(string connectionString, string providerName)
        {
            Jaxis.Inventory.Data.BeverageMonitorDB db;

            db = String.IsNullOrEmpty(connectionString) ? new Jaxis.Inventory.Data.BeverageMonitorDB() : new Jaxis.Inventory.Data.BeverageMonitorDB(connectionString, providerName);

            /*
            if(String.IsNullOrEmpty(connectionString))
            {
                db=new Jaxis.Inventory.Data.BeverageMonitorDB();
            }
            else
            {
                db=new Jaxis.Inventory.Data.BeverageMonitorDB(connectionString, providerName);
            }
            */

            IRepository<vwDailyGuestCount> _repo;
            
            if(db.TestMode)
            {
                vwDailyGuestCount.SetTestRepo();
                _repo=_testRepo;
            }
            else
            {
                _repo = new SubSonicRepository<vwDailyGuestCount>(db);
            }
            return _repo;        
        }       
        
        internal static IRepository<vwDailyGuestCount> GetRepo()
        {
            return GetRepo("","");
        }
        
        public static vwDailyGuestCount SingleOrDefault(Expression<Func<vwDailyGuestCount, bool>> expression) 
        {
            return SingleOrDefault( expression, String.Empty, String.Empty );
        }      
        
        public static vwDailyGuestCount SingleOrDefault(Expression<Func<vwDailyGuestCount, bool>> expression,string connectionString, string providerName) 
        {
            IRepository<vwDailyGuestCount> repo = GetRepo(connectionString,providerName);
            vwDailyGuestCount single = repo.SingleOrDefault<vwDailyGuestCount>( expression );
            if( null != single )
            {
                single.OnLoaded( );
            }
            return single;
        }
        
        public static bool Exists(Expression<Func<vwDailyGuestCount, bool>> expression,string connectionString, string providerName) 
        {
            return All(connectionString,providerName).Any(expression);
        }        
        
        public static bool Exists(Expression<Func<vwDailyGuestCount, bool>> expression) 
        {
            return All().Any(expression);
        }        

        
        public static vwDailyGuestCount GetByID(DateTime value) 
        {
            return vwDailyGuestCount.Find( L => L.TicketDate.Equals( value ) ).FirstOrDefault( );
        }

        public static IList<vwDailyGuestCount> Find(Expression<Func<vwDailyGuestCount, bool>> expression) 
        {
            var repo = GetRepo();
            return repo.Find(expression).ToList();
        }
        
        public static IList<vwDailyGuestCount> Find(Expression<Func<vwDailyGuestCount, bool>> expression,string connectionString, string providerName) 
        {
            var repo = GetRepo(connectionString,providerName);
            return repo.Find(expression).ToList();
        }

        public static IQueryable<vwDailyGuestCount> All(string connectionString, string providerName) 
        {
            return GetRepo(connectionString,providerName).GetAll();
        }

        public static IQueryable<vwDailyGuestCount> All() 
        {
            return GetRepo().GetAll();
        }
        
        public static PagedList<vwDailyGuestCount> GetPaged(string sortBy, int pageIndex, int pageSize,string connectionString, string providerName) 
        {
            return GetRepo(connectionString,providerName).GetPaged(sortBy, pageIndex, pageSize);
        }
      
        public static PagedList<vwDailyGuestCount> GetPaged(string sortBy, int pageIndex, int pageSize) 
        {
            return GetRepo().GetPaged(sortBy, pageIndex, pageSize);
        }

        public static PagedList<vwDailyGuestCount> GetPaged(int pageIndex, int pageSize,string connectionString, string providerName) 
        {
            return GetRepo(connectionString,providerName).GetPaged(pageIndex, pageSize);
        }

        public static PagedList<vwDailyGuestCount> GetPaged(int pageIndex, int pageSize) 
        {
            return GetRepo().GetPaged(pageIndex, pageSize);
        }

        public string KeyName()
        {
            return "TicketDate";
        }

        public object KeyValue()
        {
            return this.TicketDate;
        }
        
        public void SetKeyValue(object value) 
        {
            if (value != null && value!=DBNull.Value) 
            {
                var settable = value.ChangeTypeTo<DateTime>();
                this.GetType().GetProperty(this.KeyName()).SetValue(this, settable, null);
            }
        }
        
//        public override string ToString()
//        {
//			string rc = string.Empty;
//			if( null != this.EstablishmentModified )
//			{
//				rc = this.EstablishmentModified.ToString();
//			}
//            return rc;
//        }

        public override bool Equals(object obj)
        {
            if(obj == null)
                return false;
            if(obj is vwDailyGuestCount)
            {
                vwDailyGuestCount compare=(vwDailyGuestCount)obj;
                return compare.KeyValue().Equals( this.KeyValue() );
                //return compare.KeyValue()==this.KeyValue();
            }
            else
            {
                return base.Equals(obj);
            }
        }



        public string DescriptorValue()
        {
            return this.EstablishmentModified.ToString();
        }

        public string DescriptorColumn() 
        {
            return "EstablishmentModified";
        }

        public static string GetKeyColumn()
        {
            return "TicketDate";
        }        

        public static string GetDescriptorColumn()
        {
            return "EstablishmentModified";
        }
        
        #region ' Foreign Keys '
        #endregion



//        DateTime? _TicketDate;
        [SubSonicPrimaryKey]
        [LocalData]
        public DateTime TicketDate
        {
            get { return m_Internal.TicketDate; }
            set
            {
                if(m_Internal.TicketDate!=value){
                    m_Internal.TicketDate=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="TicketDate");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        string _EstablishmentModified;
        [LocalData]
        public string EstablishmentModified
        {
            get { return m_Internal.EstablishmentModified; }
            set
            {
                if(m_Internal.EstablishmentModified!=value){
                    m_Internal.EstablishmentModified=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="EstablishmentModified");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        int? _GuestCount;
        [LocalData]
        public int? GuestCount
        {
            get { return m_Internal.GuestCount; }
            set
            {
                if(m_Internal.GuestCount!=value){
                    m_Internal.GuestCount=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="GuestCount");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        int? _ModifiedGuestCount;
        [LocalData]
        public int? ModifiedGuestCount
        {
            get { return m_Internal.ModifiedGuestCount; }
            set
            {
                if(m_Internal.ModifiedGuestCount!=value){
                    m_Internal.ModifiedGuestCount=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="ModifiedGuestCount");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }


        public DbCommand GetUpdateCommand() 
        {
            if(TestMode)
                return _db.DataProvider.CreateCommand();
            else
                return this.ToUpdateQuery(_db.Provider).GetCommand().ToDbCommand();
            
        }

        public DbCommand GetInsertCommand() 
        {
            if(TestMode)
                return _db.DataProvider.CreateCommand();
            else
                return this.ToInsertQuery(_db.Provider).GetCommand().ToDbCommand();
        }
        
        public DbCommand GetDeleteCommand() 
        {
            if(TestMode)
                return _db.DataProvider.CreateCommand();
            else
                return this.ToDeleteQuery(_db.Provider).GetCommand().ToDbCommand();
        }
       
        public void Update()
        {
            Update(_db.DataProvider);
        }
        
        public void Update(IDataProvider provider)
        {
            OnSaving( );
            
            if(this._dirtyColumns.Count>0)
            {
                _repo.Update(this,provider);
                _dirtyColumns.Clear();    
            }
            OnSaved();
       }
 
        public void Add()
        {
            Add(_db.DataProvider);
        }
        
        public void Add(IDataProvider provider)
        {
            OnSaving( );

            var key=KeyValue();
            if(key==null)
            {
                var newKey=_repo.Add(this,provider);
                this.SetKeyValue(newKey);
            }
            else
            {
                _repo.Add(this,provider);
            }
            SetIsNew(false);
            OnSaved();
        }
        
        public override void Save() 
        {
            Save(_db.DataProvider);
        }      

        public void Save(IDataProvider provider) 
        {
            if (_isNew) 
            {
                Add(provider);
            }
            else 
            {
                Update(provider);
            }
            SetIsLoaded( true );
        }

        public void Delete(IDataProvider provider) 
        {
                   
                 
            _repo.Delete(KeyValue());
                    }

        public void Delete() 
        {
            Delete(_db.DataProvider);
        }

        public static void Delete(Expression<Func<vwDailyGuestCount, bool>> expression) 
        {
            var repo = GetRepo();
            
       
            repo.DeleteMany(expression);
        }

        
        public void Load(IDataReader rdr) 
        {
            Load(rdr, true);
        }

        public void Load(IDataReader rdr, bool closeReader) 
        {
            if (rdr.Read()) 
            {
                try 
                {
                    rdr.Load(this);
                    SetIsNew(false);
                    SetIsLoaded(true);
                }
                catch
                {
                    SetIsLoaded(false);
                    throw;
                }
            }
            else
            {
                SetIsLoaded(false);
            
            }

            if (closeReader)
                rdr.Dispose();
        }
    } 

    [DataContract]
    public partial class DatavwDailyPOSTicketItem : IWCFDataElement
    {
        [DataMember]
        public string CheckNumber { get; set; }
        [DataMember]
        public int GuestCountModified { get; set; }
        [DataMember]
        public DateTime TicketDate { get; set; }
        [DataMember]
        public string Establishment { get; set; }
        [DataMember]
        public string ServerName { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string Comment { get; set; }
        [DataMember]
        public decimal? Price { get; set; }
        [DataMember]
        public int Quantity { get; set; }

        public void Copy( DatavwDailyPOSTicketItem _Item )
        {
             GuestCountModified = _Item.GuestCountModified;			
             TicketDate = _Item.TicketDate;			
             Establishment = _Item.Establishment;			
             ServerName = _Item.ServerName;			
             Description = _Item.Description;			
             Comment = _Item.Comment;			
             Price = _Item.Price;			
             Quantity = _Item.Quantity;			
        }

		public IBaseDataObject CreateDBObject( )
        {
            return new vwDailyPOSTicketItem( this );
        }


    }


    /// <summary>
    /// A class which represents the vwDailyPOSTicketItems table in the BeverageMonitor Database.
    /// </summary>
    public partial class vwDailyPOSTicketItem: BaseDataObject<DatavwDailyPOSTicketItem>, IActiveRecord, ICallOnCreated
    {
        #region Built-in testing
        [NonSerialized]
        static TestRepository<vwDailyPOSTicketItem> _testRepo;
       
        static void SetTestRepo()
        {
            _testRepo = _testRepo ?? new TestRepository<vwDailyPOSTicketItem>(new Jaxis.Inventory.Data.BeverageMonitorDB());
        }

        public static void ResetTestRepo()
        {
            _testRepo = null;
            SetTestRepo();
        }

        public static void Setup(List<vwDailyPOSTicketItem> testlist)
        {
            SetTestRepo();
            foreach (var item in testlist)
            {
                _testRepo._items.Add(item);
            }
        }

        public static void Setup(vwDailyPOSTicketItem item) 
        {
            SetTestRepo();
            _testRepo._items.Add(item);
        }

        public static void Setup(int testItems) 
        {
            SetTestRepo();
            for(int i=0;i<testItems;i++)
            {
                vwDailyPOSTicketItem item=new vwDailyPOSTicketItem();
                _testRepo._items.Add(item);
            }
        }
        
        public bool TestMode = false;
        #endregion

        IRepository<vwDailyPOSTicketItem> _repo;

        partial void OnCreated();
            
        partial void OnLoaded();
        
        partial void OnSaving();
        
        partial void OnSaved();
        
        partial void OnChanged();

        public void SetIsLoaded(bool isLoaded)
        {
            _isLoaded=isLoaded;
            if(isLoaded)
                OnLoaded();
        }
        
        Jaxis.Inventory.Data.BeverageMonitorDB _db;
        
        public vwDailyPOSTicketItem()
        {
            m_Internal = new DatavwDailyPOSTicketItem();
             _db=new Jaxis.Inventory.Data.BeverageMonitorDB();
            Init();   
        }

        ///<summary>
        ///Set bool to true to assign NewGuid to PrimaryKey (if appropriate) and to call OnCreated
        ///</summary>
        public vwDailyPOSTicketItem( bool _CallOnCreated )
        {
            m_Internal = new DatavwDailyPOSTicketItem();
            _db=new Jaxis.Inventory.Data.BeverageMonitorDB();
            Init();  
            CallOnCreated( _CallOnCreated );
        }

        public vwDailyPOSTicketItem(string connectionString, string providerName) 
        {
            m_Internal = new DatavwDailyPOSTicketItem();
            _db=new Jaxis.Inventory.Data.BeverageMonitorDB(connectionString, providerName);
            Init();            
        }

        public vwDailyPOSTicketItem( vwDailyPOSTicketItem _Item )
        {
            m_Internal = new DatavwDailyPOSTicketItem();
            Copy( _Item );
            _db=new Jaxis.Inventory.Data.BeverageMonitorDB();
            Init();            
        }

        public vwDailyPOSTicketItem( DatavwDailyPOSTicketItem _Item )
        {
            m_Internal.Copy( _Item );
            _db=new Jaxis.Inventory.Data.BeverageMonitorDB();
            Init();            
        }         
         
        public void Copy( vwDailyPOSTicketItem _Item )
        {
            m_Internal.GuestCountModified = _Item.GuestCountModified;			
            m_Internal.TicketDate = _Item.TicketDate;			
            m_Internal.Establishment = _Item.Establishment;			
            m_Internal.ServerName = _Item.ServerName;			
            m_Internal.Description = _Item.Description;			
            m_Internal.Comment = _Item.Comment;			
            m_Internal.Price = _Item.Price;			
            m_Internal.Quantity = _Item.Quantity;			
        }         

        public void CallOnCreated( bool _CallOnCreated )
        {
            if( _CallOnCreated )
            {
	
                OnCreated( );
            }
        }
         
        void Init()
        {
            TestMode=this._db.DataProvider.ConnectionString.Equals("test", StringComparison.InvariantCultureIgnoreCase);
            _dirtyColumns=new List<IColumn>();
            if(TestMode)
            {
                vwDailyPOSTicketItem.SetTestRepo();
                _repo=_testRepo;
            }
            else
            {
                _repo = new SubSonicRepository<vwDailyPOSTicketItem>(_db);
            }
            tbl=_repo.GetTable();
            SetIsNew(true);
        }

        public vwDailyPOSTicketItem(Expression<Func<vwDailyPOSTicketItem, bool>> expression):this() 
        {
            SetIsLoaded(_repo.Load(this,expression));
        }
        
        internal static IRepository<vwDailyPOSTicketItem> GetRepo(string connectionString, string providerName)
        {
            Jaxis.Inventory.Data.BeverageMonitorDB db;

            db = String.IsNullOrEmpty(connectionString) ? new Jaxis.Inventory.Data.BeverageMonitorDB() : new Jaxis.Inventory.Data.BeverageMonitorDB(connectionString, providerName);

            /*
            if(String.IsNullOrEmpty(connectionString))
            {
                db=new Jaxis.Inventory.Data.BeverageMonitorDB();
            }
            else
            {
                db=new Jaxis.Inventory.Data.BeverageMonitorDB(connectionString, providerName);
            }
            */

            IRepository<vwDailyPOSTicketItem> _repo;
            
            if(db.TestMode)
            {
                vwDailyPOSTicketItem.SetTestRepo();
                _repo=_testRepo;
            }
            else
            {
                _repo = new SubSonicRepository<vwDailyPOSTicketItem>(db);
            }
            return _repo;        
        }       
        
        internal static IRepository<vwDailyPOSTicketItem> GetRepo()
        {
            return GetRepo("","");
        }
        
        public static vwDailyPOSTicketItem SingleOrDefault(Expression<Func<vwDailyPOSTicketItem, bool>> expression) 
        {
            return SingleOrDefault( expression, String.Empty, String.Empty );
        }      
        
        public static vwDailyPOSTicketItem SingleOrDefault(Expression<Func<vwDailyPOSTicketItem, bool>> expression,string connectionString, string providerName) 
        {
            IRepository<vwDailyPOSTicketItem> repo = GetRepo(connectionString,providerName);
            vwDailyPOSTicketItem single = repo.SingleOrDefault<vwDailyPOSTicketItem>( expression );
            if( null != single )
            {
                single.OnLoaded( );
            }
            return single;
        }
        
        public static bool Exists(Expression<Func<vwDailyPOSTicketItem, bool>> expression,string connectionString, string providerName) 
        {
            return All(connectionString,providerName).Any(expression);
        }        
        
        public static bool Exists(Expression<Func<vwDailyPOSTicketItem, bool>> expression) 
        {
            return All().Any(expression);
        }        

        
        public static vwDailyPOSTicketItem GetByID(string value) 
        {
            return vwDailyPOSTicketItem.Find( L => L.CheckNumber.Equals( value ) ).FirstOrDefault( );
        }

        public static IList<vwDailyPOSTicketItem> Find(Expression<Func<vwDailyPOSTicketItem, bool>> expression) 
        {
            var repo = GetRepo();
            return repo.Find(expression).ToList();
        }
        
        public static IList<vwDailyPOSTicketItem> Find(Expression<Func<vwDailyPOSTicketItem, bool>> expression,string connectionString, string providerName) 
        {
            var repo = GetRepo(connectionString,providerName);
            return repo.Find(expression).ToList();
        }

        public static IQueryable<vwDailyPOSTicketItem> All(string connectionString, string providerName) 
        {
            return GetRepo(connectionString,providerName).GetAll();
        }

        public static IQueryable<vwDailyPOSTicketItem> All() 
        {
            return GetRepo().GetAll();
        }
        
        public static PagedList<vwDailyPOSTicketItem> GetPaged(string sortBy, int pageIndex, int pageSize,string connectionString, string providerName) 
        {
            return GetRepo(connectionString,providerName).GetPaged(sortBy, pageIndex, pageSize);
        }
      
        public static PagedList<vwDailyPOSTicketItem> GetPaged(string sortBy, int pageIndex, int pageSize) 
        {
            return GetRepo().GetPaged(sortBy, pageIndex, pageSize);
        }

        public static PagedList<vwDailyPOSTicketItem> GetPaged(int pageIndex, int pageSize,string connectionString, string providerName) 
        {
            return GetRepo(connectionString,providerName).GetPaged(pageIndex, pageSize);
        }

        public static PagedList<vwDailyPOSTicketItem> GetPaged(int pageIndex, int pageSize) 
        {
            return GetRepo().GetPaged(pageIndex, pageSize);
        }

        public string KeyName()
        {
            return "CheckNumber";
        }

        public object KeyValue()
        {
            return this.CheckNumber;
        }
        
        public void SetKeyValue(object value) 
        {
            if (value != null && value!=DBNull.Value) 
            {
                var settable = value.ChangeTypeTo<string>();
                this.GetType().GetProperty(this.KeyName()).SetValue(this, settable, null);
            }
        }
        
//        public override string ToString()
//        {
//			string rc = string.Empty;
//			if( null != this.CheckNumber )
//			{
//				rc = this.CheckNumber.ToString();
//			}
//            return rc;
//        }

        public override bool Equals(object obj)
        {
            if(obj == null)
                return false;
            if(obj is vwDailyPOSTicketItem)
            {
                vwDailyPOSTicketItem compare=(vwDailyPOSTicketItem)obj;
                return compare.KeyValue().Equals( this.KeyValue() );
                //return compare.KeyValue()==this.KeyValue();
            }
            else
            {
                return base.Equals(obj);
            }
        }



        public string DescriptorValue()
        {
            return this.CheckNumber.ToString();
        }

        public string DescriptorColumn() 
        {
            return "CheckNumber";
        }

        public static string GetKeyColumn()
        {
            return "CheckNumber";
        }        

        public static string GetDescriptorColumn()
        {
            return "CheckNumber";
        }
        
        #region ' Foreign Keys '
        #endregion



//        string _CheckNumber;
        [SubSonicPrimaryKey]
        [LocalData]
        public string CheckNumber
        {
            get { return m_Internal.CheckNumber; }
            set
            {
                if(m_Internal.CheckNumber!=value){
                    m_Internal.CheckNumber=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="CheckNumber");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        int _GuestCountModified;
        [LocalData]
        public int GuestCountModified
        {
            get { return m_Internal.GuestCountModified; }
            set
            {
                if(m_Internal.GuestCountModified!=value){
                    m_Internal.GuestCountModified=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="GuestCountModified");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        DateTime _TicketDate;
        [LocalData]
        public DateTime TicketDate
        {
            get { return m_Internal.TicketDate; }
            set
            {
                if(m_Internal.TicketDate!=value){
                    m_Internal.TicketDate=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="TicketDate");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        string _Establishment;
        [LocalData]
        public string Establishment
        {
            get { return m_Internal.Establishment; }
            set
            {
                if(m_Internal.Establishment!=value){
                    m_Internal.Establishment=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="Establishment");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        string _ServerName;
        [LocalData]
        public string ServerName
        {
            get { return m_Internal.ServerName; }
            set
            {
                if(m_Internal.ServerName!=value){
                    m_Internal.ServerName=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="ServerName");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        string _Description;
        [LocalData]
        public string Description
        {
            get { return m_Internal.Description; }
            set
            {
                if(m_Internal.Description!=value){
                    m_Internal.Description=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="Description");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        string _Comment;
        [LocalData]
        public string Comment
        {
            get { return m_Internal.Comment; }
            set
            {
                if(m_Internal.Comment!=value){
                    m_Internal.Comment=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="Comment");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        decimal? _Price;
        [LocalData]
        public decimal? Price
        {
            get { return m_Internal.Price; }
            set
            {
                if(m_Internal.Price!=value){
                    m_Internal.Price=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="Price");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        int _Quantity;
        [LocalData]
        public int Quantity
        {
            get { return m_Internal.Quantity; }
            set
            {
                if(m_Internal.Quantity!=value){
                    m_Internal.Quantity=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="Quantity");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }


        public DbCommand GetUpdateCommand() 
        {
            if(TestMode)
                return _db.DataProvider.CreateCommand();
            else
                return this.ToUpdateQuery(_db.Provider).GetCommand().ToDbCommand();
            
        }

        public DbCommand GetInsertCommand() 
        {
            if(TestMode)
                return _db.DataProvider.CreateCommand();
            else
                return this.ToInsertQuery(_db.Provider).GetCommand().ToDbCommand();
        }
        
        public DbCommand GetDeleteCommand() 
        {
            if(TestMode)
                return _db.DataProvider.CreateCommand();
            else
                return this.ToDeleteQuery(_db.Provider).GetCommand().ToDbCommand();
        }
       
        public void Update()
        {
            Update(_db.DataProvider);
        }
        
        public void Update(IDataProvider provider)
        {
            OnSaving( );
            
            if(this._dirtyColumns.Count>0)
            {
                _repo.Update(this,provider);
                _dirtyColumns.Clear();    
            }
            OnSaved();
       }
 
        public void Add()
        {
            Add(_db.DataProvider);
        }
        
        public void Add(IDataProvider provider)
        {
            OnSaving( );

            var key=KeyValue();
            if(key==null)
            {
                var newKey=_repo.Add(this,provider);
                this.SetKeyValue(newKey);
            }
            else
            {
                _repo.Add(this,provider);
            }
            SetIsNew(false);
            OnSaved();
        }
        
        public override void Save() 
        {
            Save(_db.DataProvider);
        }      

        public void Save(IDataProvider provider) 
        {
            if (_isNew) 
            {
                Add(provider);
            }
            else 
            {
                Update(provider);
            }
            SetIsLoaded( true );
        }

        public void Delete(IDataProvider provider) 
        {
                   
                 
            _repo.Delete(KeyValue());
                    }

        public void Delete() 
        {
            Delete(_db.DataProvider);
        }

        public static void Delete(Expression<Func<vwDailyPOSTicketItem, bool>> expression) 
        {
            var repo = GetRepo();
            
       
            repo.DeleteMany(expression);
        }

        
        public void Load(IDataReader rdr) 
        {
            Load(rdr, true);
        }

        public void Load(IDataReader rdr, bool closeReader) 
        {
            if (rdr.Read()) 
            {
                try 
                {
                    rdr.Load(this);
                    SetIsNew(false);
                    SetIsLoaded(true);
                }
                catch
                {
                    SetIsLoaded(false);
                    throw;
                }
            }
            else
            {
                SetIsLoaded(false);
            
            }

            if (closeReader)
                rdr.Dispose();
        }
    } 

    [DataContract]
    public partial class DatavwDailyPOSTicket : IWCFDataElement
    {
        [DataMember]
        public Guid POSTicketID { get; set; }
        [DataMember]
        public string CheckNumber { get; set; }
        [DataMember]
        public string Comments { get; set; }
        [DataMember]
        public DateTime TicketDate { get; set; }
        [DataMember]
        public string Establishment { get; set; }
        [DataMember]
        public string Server { get; set; }
        [DataMember]
        public string ServerName { get; set; }
        [DataMember]
        public int GuestCount { get; set; }
        [DataMember]
        public string CustomerTable { get; set; }
        [DataMember]
        public string RawData { get; set; }
        [DataMember]
        public int TouchCount { get; set; }
        [DataMember]
        public decimal TipAmount { get; set; }
        [DataMember]
        public int GuestCountModified { get; set; }
        [DataMember]
        public decimal? TicketTotal { get; set; }
        [DataMember]
        public string EstablishmentModified { get; set; }

        public void Copy( DatavwDailyPOSTicket _Item )
        {
             CheckNumber = _Item.CheckNumber;			
             Comments = _Item.Comments;			
             TicketDate = _Item.TicketDate;			
             Establishment = _Item.Establishment;			
             Server = _Item.Server;			
             ServerName = _Item.ServerName;			
             GuestCount = _Item.GuestCount;			
             CustomerTable = _Item.CustomerTable;			
             RawData = _Item.RawData;			
             TouchCount = _Item.TouchCount;			
             TipAmount = _Item.TipAmount;			
             GuestCountModified = _Item.GuestCountModified;			
             TicketTotal = _Item.TicketTotal;			
             EstablishmentModified = _Item.EstablishmentModified;			
        }

		public IBaseDataObject CreateDBObject( )
        {
            return new vwDailyPOSTicket( this );
        }


    }


    /// <summary>
    /// A class which represents the vwDailyPOSTickets table in the BeverageMonitor Database.
    /// </summary>
    public partial class vwDailyPOSTicket: BaseDataObject<DatavwDailyPOSTicket>, IActiveRecord, ICallOnCreated
    {
        #region Built-in testing
        [NonSerialized]
        static TestRepository<vwDailyPOSTicket> _testRepo;
       
        static void SetTestRepo()
        {
            _testRepo = _testRepo ?? new TestRepository<vwDailyPOSTicket>(new Jaxis.Inventory.Data.BeverageMonitorDB());
        }

        public static void ResetTestRepo()
        {
            _testRepo = null;
            SetTestRepo();
        }

        public static void Setup(List<vwDailyPOSTicket> testlist)
        {
            SetTestRepo();
            foreach (var item in testlist)
            {
                _testRepo._items.Add(item);
            }
        }

        public static void Setup(vwDailyPOSTicket item) 
        {
            SetTestRepo();
            _testRepo._items.Add(item);
        }

        public static void Setup(int testItems) 
        {
            SetTestRepo();
            for(int i=0;i<testItems;i++)
            {
                vwDailyPOSTicket item=new vwDailyPOSTicket();
                _testRepo._items.Add(item);
            }
        }
        
        public bool TestMode = false;
        #endregion

        IRepository<vwDailyPOSTicket> _repo;

        partial void OnCreated();
            
        partial void OnLoaded();
        
        partial void OnSaving();
        
        partial void OnSaved();
        
        partial void OnChanged();

        public void SetIsLoaded(bool isLoaded)
        {
            _isLoaded=isLoaded;
            if(isLoaded)
                OnLoaded();
        }
        
        Jaxis.Inventory.Data.BeverageMonitorDB _db;
        
        public vwDailyPOSTicket()
        {
            m_Internal = new DatavwDailyPOSTicket();
             _db=new Jaxis.Inventory.Data.BeverageMonitorDB();
            Init();   
            this.POSTicketID = Guid.NewGuid( );     
        }

        ///<summary>
        ///Set bool to true to assign NewGuid to PrimaryKey (if appropriate) and to call OnCreated
        ///</summary>
        public vwDailyPOSTicket( bool _CallOnCreated )
        {
            m_Internal = new DatavwDailyPOSTicket();
            _db=new Jaxis.Inventory.Data.BeverageMonitorDB();
            Init();  
            CallOnCreated( _CallOnCreated );
        }

        public vwDailyPOSTicket(string connectionString, string providerName) 
        {
            m_Internal = new DatavwDailyPOSTicket();
            _db=new Jaxis.Inventory.Data.BeverageMonitorDB(connectionString, providerName);
            Init();            
            this.POSTicketID = Guid.NewGuid( );     
        }

        public vwDailyPOSTicket( vwDailyPOSTicket _Item )
        {
            m_Internal = new DatavwDailyPOSTicket();
            Copy( _Item );
            _db=new Jaxis.Inventory.Data.BeverageMonitorDB();
            Init();            
        }

        public vwDailyPOSTicket( DatavwDailyPOSTicket _Item )
        {
            m_Internal.Copy( _Item );
            _db=new Jaxis.Inventory.Data.BeverageMonitorDB();
            Init();            
        }         
         
        public void Copy( vwDailyPOSTicket _Item )
        {
            m_Internal.CheckNumber = _Item.CheckNumber;			
            m_Internal.Comments = _Item.Comments;			
            m_Internal.TicketDate = _Item.TicketDate;			
            m_Internal.Establishment = _Item.Establishment;			
            m_Internal.Server = _Item.Server;			
            m_Internal.ServerName = _Item.ServerName;			
            m_Internal.GuestCount = _Item.GuestCount;			
            m_Internal.CustomerTable = _Item.CustomerTable;			
            m_Internal.RawData = _Item.RawData;			
            m_Internal.TouchCount = _Item.TouchCount;			
            m_Internal.TipAmount = _Item.TipAmount;			
            m_Internal.GuestCountModified = _Item.GuestCountModified;			
            m_Internal.TicketTotal = _Item.TicketTotal;			
            m_Internal.EstablishmentModified = _Item.EstablishmentModified;			
        }         

        public void CallOnCreated( bool _CallOnCreated )
        {
            if( _CallOnCreated )
            {
                m_Internal.POSTicketID = Guid.NewGuid( );     
	
                OnCreated( );
            }
        }
         
        void Init()
        {
            TestMode=this._db.DataProvider.ConnectionString.Equals("test", StringComparison.InvariantCultureIgnoreCase);
            _dirtyColumns=new List<IColumn>();
            if(TestMode)
            {
                vwDailyPOSTicket.SetTestRepo();
                _repo=_testRepo;
            }
            else
            {
                _repo = new SubSonicRepository<vwDailyPOSTicket>(_db);
            }
            tbl=_repo.GetTable();
            SetIsNew(true);
        }

        public vwDailyPOSTicket(Expression<Func<vwDailyPOSTicket, bool>> expression):this() 
        {
            SetIsLoaded(_repo.Load(this,expression));
        }
        
        internal static IRepository<vwDailyPOSTicket> GetRepo(string connectionString, string providerName)
        {
            Jaxis.Inventory.Data.BeverageMonitorDB db;

            db = String.IsNullOrEmpty(connectionString) ? new Jaxis.Inventory.Data.BeverageMonitorDB() : new Jaxis.Inventory.Data.BeverageMonitorDB(connectionString, providerName);

            /*
            if(String.IsNullOrEmpty(connectionString))
            {
                db=new Jaxis.Inventory.Data.BeverageMonitorDB();
            }
            else
            {
                db=new Jaxis.Inventory.Data.BeverageMonitorDB(connectionString, providerName);
            }
            */

            IRepository<vwDailyPOSTicket> _repo;
            
            if(db.TestMode)
            {
                vwDailyPOSTicket.SetTestRepo();
                _repo=_testRepo;
            }
            else
            {
                _repo = new SubSonicRepository<vwDailyPOSTicket>(db);
            }
            return _repo;        
        }       
        
        internal static IRepository<vwDailyPOSTicket> GetRepo()
        {
            return GetRepo("","");
        }
        
        public static vwDailyPOSTicket SingleOrDefault(Expression<Func<vwDailyPOSTicket, bool>> expression) 
        {
            return SingleOrDefault( expression, String.Empty, String.Empty );
        }      
        
        public static vwDailyPOSTicket SingleOrDefault(Expression<Func<vwDailyPOSTicket, bool>> expression,string connectionString, string providerName) 
        {
            IRepository<vwDailyPOSTicket> repo = GetRepo(connectionString,providerName);
            vwDailyPOSTicket single = repo.SingleOrDefault<vwDailyPOSTicket>( expression );
            if( null != single )
            {
                single.OnLoaded( );
            }
            return single;
        }
        
        public static bool Exists(Expression<Func<vwDailyPOSTicket, bool>> expression,string connectionString, string providerName) 
        {
            return All(connectionString,providerName).Any(expression);
        }        
        
        public static bool Exists(Expression<Func<vwDailyPOSTicket, bool>> expression) 
        {
            return All().Any(expression);
        }        

        protected static bool IsEmptyvwDailyPOSTicketLoaded = false;
        protected static vwDailyPOSTicket EmptyvwDailyPOSTicketMember = null;

        public static vwDailyPOSTicket GetByID(Guid? value) 
        {
            vwDailyPOSTicket rc = null;
            if( value.HasValue )
            {
                rc = GetByID( value.Value );
            }
            return rc;
        }
        
        public static vwDailyPOSTicket GetByID(Guid value) 
        {
            vwDailyPOSTicket rc = null;
            if( null != value )
            {
                if( value == Guid.Empty )
                {
                    if( true == IsEmptyvwDailyPOSTicketLoaded )
                    {
                        rc = EmptyvwDailyPOSTicketMember;
                    }
                    else
                    {
                        IsEmptyvwDailyPOSTicketLoaded = true;
                        rc = vwDailyPOSTicket.Find( L => L.POSTicketID.Equals( value ) ).FirstOrDefault( );
                        EmptyvwDailyPOSTicketMember = rc;
                    }
                }
                else
                {
                    rc = vwDailyPOSTicket.Find( L => L.POSTicketID.Equals( value ) ).FirstOrDefault( );
                } 
            }
            return rc;
        }

        public static IList<vwDailyPOSTicket> Find(Expression<Func<vwDailyPOSTicket, bool>> expression) 
        {
            var repo = GetRepo();
            return repo.Find(expression).ToList();
        }
        
        public static IList<vwDailyPOSTicket> Find(Expression<Func<vwDailyPOSTicket, bool>> expression,string connectionString, string providerName) 
        {
            var repo = GetRepo(connectionString,providerName);
            return repo.Find(expression).ToList();
        }

        public static IQueryable<vwDailyPOSTicket> All(string connectionString, string providerName) 
        {
            return GetRepo(connectionString,providerName).GetAll();
        }

        public static IQueryable<vwDailyPOSTicket> All() 
        {
            return GetRepo().GetAll();
        }
        
        public static PagedList<vwDailyPOSTicket> GetPaged(string sortBy, int pageIndex, int pageSize,string connectionString, string providerName) 
        {
            return GetRepo(connectionString,providerName).GetPaged(sortBy, pageIndex, pageSize);
        }
      
        public static PagedList<vwDailyPOSTicket> GetPaged(string sortBy, int pageIndex, int pageSize) 
        {
            return GetRepo().GetPaged(sortBy, pageIndex, pageSize);
        }

        public static PagedList<vwDailyPOSTicket> GetPaged(int pageIndex, int pageSize,string connectionString, string providerName) 
        {
            return GetRepo(connectionString,providerName).GetPaged(pageIndex, pageSize);
        }

        public static PagedList<vwDailyPOSTicket> GetPaged(int pageIndex, int pageSize) 
        {
            return GetRepo().GetPaged(pageIndex, pageSize);
        }

        public string KeyName()
        {
            return "POSTicketID";
        }

        public object KeyValue()
        {
            return this.POSTicketID;
        }
        
        public void SetKeyValue(object value) 
        {
            if (value != null && value!=DBNull.Value) 
            {
                var settable = value.ChangeTypeTo<Guid>();
                this.GetType().GetProperty(this.KeyName()).SetValue(this, settable, null);
            }
        }
        
//        public override string ToString()
//        {
//			string rc = string.Empty;
//			if( null != this.CheckNumber )
//			{
//				rc = this.CheckNumber.ToString();
//			}
//            return rc;
//        }

        public override bool Equals(object obj)
        {
            if(obj == null)
                return false;
            if(obj is vwDailyPOSTicket)
            {
                vwDailyPOSTicket compare=(vwDailyPOSTicket)obj;
                return compare.KeyValue().Equals( this.KeyValue() );
                //return compare.KeyValue()==this.KeyValue();
            }
            else
            {
                return base.Equals(obj);
            }
        }


        public override int GetHashCode() 
        {
            return this.POSTicketID.GetHashCode( );
        }

        public string DescriptorValue()
        {
            return this.CheckNumber.ToString();
        }

        public string DescriptorColumn() 
        {
            return "CheckNumber";
        }

        public static string GetKeyColumn()
        {
            return "POSTicketID";
        }        

        public static string GetDescriptorColumn()
        {
            return "CheckNumber";
        }
        
        #region ' Foreign Keys '
        #endregion


		[ObfuscationAttribute(Exclude=true)]
        public virtual Guid ObjectID 
        {
            get
            {
                return m_Internal.POSTicketID;
            }
            set
            {
                m_Internal.POSTicketID = value;
            }
        }


//        Guid _POSTicketID;
        [SubSonicPrimaryKey]
        [LocalData]
        public Guid POSTicketID
        {
            get { return m_Internal.POSTicketID; }
            set
            {
                if(m_Internal.POSTicketID!=value){
                    m_Internal.POSTicketID=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="POSTicketID");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        string _CheckNumber;
        [LocalData]
        public string CheckNumber
        {
            get { return m_Internal.CheckNumber; }
            set
            {
                if(m_Internal.CheckNumber!=value){
                    m_Internal.CheckNumber=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="CheckNumber");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        string _Comments;
        [LocalData]
        public string Comments
        {
            get { return m_Internal.Comments; }
            set
            {
                if(m_Internal.Comments!=value){
                    m_Internal.Comments=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="Comments");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        DateTime _TicketDate;
        [LocalData]
        public DateTime TicketDate
        {
            get { return m_Internal.TicketDate; }
            set
            {
                if(m_Internal.TicketDate!=value){
                    m_Internal.TicketDate=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="TicketDate");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        string _Establishment;
        [LocalData]
        public string Establishment
        {
            get { return m_Internal.Establishment; }
            set
            {
                if(m_Internal.Establishment!=value){
                    m_Internal.Establishment=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="Establishment");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        string _Server;
        [LocalData]
        public string Server
        {
            get { return m_Internal.Server; }
            set
            {
                if(m_Internal.Server!=value){
                    m_Internal.Server=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="Server");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        string _ServerName;
        [LocalData]
        public string ServerName
        {
            get { return m_Internal.ServerName; }
            set
            {
                if(m_Internal.ServerName!=value){
                    m_Internal.ServerName=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="ServerName");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        int _GuestCount;
        [LocalData]
        public int GuestCount
        {
            get { return m_Internal.GuestCount; }
            set
            {
                if(m_Internal.GuestCount!=value){
                    m_Internal.GuestCount=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="GuestCount");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        string _CustomerTable;
        [LocalData]
        public string CustomerTable
        {
            get { return m_Internal.CustomerTable; }
            set
            {
                if(m_Internal.CustomerTable!=value){
                    m_Internal.CustomerTable=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="CustomerTable");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        string _RawData;
        [LocalData]
        public string RawData
        {
            get { return m_Internal.RawData; }
            set
            {
                if(m_Internal.RawData!=value){
                    m_Internal.RawData=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="RawData");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        int _TouchCount;
        [LocalData]
        public int TouchCount
        {
            get { return m_Internal.TouchCount; }
            set
            {
                if(m_Internal.TouchCount!=value){
                    m_Internal.TouchCount=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="TouchCount");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        decimal _TipAmount;
        [LocalData]
        public decimal TipAmount
        {
            get { return m_Internal.TipAmount; }
            set
            {
                if(m_Internal.TipAmount!=value){
                    m_Internal.TipAmount=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="TipAmount");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        int _GuestCountModified;
        [LocalData]
        public int GuestCountModified
        {
            get { return m_Internal.GuestCountModified; }
            set
            {
                if(m_Internal.GuestCountModified!=value){
                    m_Internal.GuestCountModified=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="GuestCountModified");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        decimal? _TicketTotal;
        [LocalData]
        public decimal? TicketTotal
        {
            get { return m_Internal.TicketTotal; }
            set
            {
                if(m_Internal.TicketTotal!=value){
                    m_Internal.TicketTotal=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="TicketTotal");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        string _EstablishmentModified;
        [LocalData]
        public string EstablishmentModified
        {
            get { return m_Internal.EstablishmentModified; }
            set
            {
                if(m_Internal.EstablishmentModified!=value){
                    m_Internal.EstablishmentModified=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="EstablishmentModified");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }


        public DbCommand GetUpdateCommand() 
        {
            if(TestMode)
                return _db.DataProvider.CreateCommand();
            else
                return this.ToUpdateQuery(_db.Provider).GetCommand().ToDbCommand();
            
        }

        public DbCommand GetInsertCommand() 
        {
            if(TestMode)
                return _db.DataProvider.CreateCommand();
            else
                return this.ToInsertQuery(_db.Provider).GetCommand().ToDbCommand();
        }
        
        public DbCommand GetDeleteCommand() 
        {
            if(TestMode)
                return _db.DataProvider.CreateCommand();
            else
                return this.ToDeleteQuery(_db.Provider).GetCommand().ToDbCommand();
        }
       
        public void Update()
        {
            Update(_db.DataProvider);
        }
        
        public void Update(IDataProvider provider)
        {
            OnSaving( );
            
            if(this._dirtyColumns.Count>0)
            {
                _repo.Update(this,provider);
                _dirtyColumns.Clear();    
            }
            OnSaved();
       }
 
        public void Add()
        {
            Add(_db.DataProvider);
        }
        
        public void Add(IDataProvider provider)
        {
            OnSaving( );

            var key=KeyValue();
            if(key==null)
            {
                var newKey=_repo.Add(this,provider);
                this.SetKeyValue(newKey);
            }
            else
            {
                _repo.Add(this,provider);
            }
            SetIsNew(false);
            OnSaved();
        }
        
        public override void Save() 
        {
            Save(_db.DataProvider);
        }      

        public void Save(IDataProvider provider) 
        {
            if (_isNew) 
            {
                Add(provider);
            }
            else 
            {
                Update(provider);
            }
            SetIsLoaded( true );
        }

        public void Delete(IDataProvider provider) 
        {
                   
                 
            _repo.Delete(KeyValue());
                    }

        public void Delete() 
        {
            Delete(_db.DataProvider);
        }

        public static void Delete(Expression<Func<vwDailyPOSTicket, bool>> expression) 
        {
            var repo = GetRepo();
            
       
            repo.DeleteMany(expression);
        }

        
        public void Load(IDataReader rdr) 
        {
            Load(rdr, true);
        }

        public void Load(IDataReader rdr, bool closeReader) 
        {
            if (rdr.Read()) 
            {
                try 
                {
                    rdr.Load(this);
                    SetIsNew(false);
                    SetIsLoaded(true);
                }
                catch
                {
                    SetIsLoaded(false);
                    throw;
                }
            }
            else
            {
                SetIsLoaded(false);
            
            }

            if (closeReader)
                rdr.Dispose();
        }
    } 

    [DataContract]
    public partial class DatavwDailyTicketDataSummary : IWCFDataElement
    {
        [DataMember]
        public DateTime TicketDate { get; set; }
        [DataMember]
        public string EstablishmentModified { get; set; }
        [DataMember]
        public int? GuestCount { get; set; }
        [DataMember]
        public int? ModifiedGuestCount { get; set; }
        [DataMember]
        public decimal? TicketTotal { get; set; }
        [DataMember]
        public decimal? PaymentAmount { get; set; }
        [DataMember]
        public decimal? TVAAmount { get; set; }

        public void Copy( DatavwDailyTicketDataSummary _Item )
        {
             EstablishmentModified = _Item.EstablishmentModified;			
             GuestCount = _Item.GuestCount;			
             ModifiedGuestCount = _Item.ModifiedGuestCount;			
             TicketTotal = _Item.TicketTotal;			
             PaymentAmount = _Item.PaymentAmount;			
             TVAAmount = _Item.TVAAmount;			
        }

		public IBaseDataObject CreateDBObject( )
        {
            return new vwDailyTicketDataSummary( this );
        }


    }


    /// <summary>
    /// A class which represents the vwDailyTicketDataSummary table in the BeverageMonitor Database.
    /// </summary>
    public partial class vwDailyTicketDataSummary: BaseDataObject<DatavwDailyTicketDataSummary>, IActiveRecord, ICallOnCreated
    {
        #region Built-in testing
        [NonSerialized]
        static TestRepository<vwDailyTicketDataSummary> _testRepo;
       
        static void SetTestRepo()
        {
            _testRepo = _testRepo ?? new TestRepository<vwDailyTicketDataSummary>(new Jaxis.Inventory.Data.BeverageMonitorDB());
        }

        public static void ResetTestRepo()
        {
            _testRepo = null;
            SetTestRepo();
        }

        public static void Setup(List<vwDailyTicketDataSummary> testlist)
        {
            SetTestRepo();
            foreach (var item in testlist)
            {
                _testRepo._items.Add(item);
            }
        }

        public static void Setup(vwDailyTicketDataSummary item) 
        {
            SetTestRepo();
            _testRepo._items.Add(item);
        }

        public static void Setup(int testItems) 
        {
            SetTestRepo();
            for(int i=0;i<testItems;i++)
            {
                vwDailyTicketDataSummary item=new vwDailyTicketDataSummary();
                _testRepo._items.Add(item);
            }
        }
        
        public bool TestMode = false;
        #endregion

        IRepository<vwDailyTicketDataSummary> _repo;

        partial void OnCreated();
            
        partial void OnLoaded();
        
        partial void OnSaving();
        
        partial void OnSaved();
        
        partial void OnChanged();

        public void SetIsLoaded(bool isLoaded)
        {
            _isLoaded=isLoaded;
            if(isLoaded)
                OnLoaded();
        }
        
        Jaxis.Inventory.Data.BeverageMonitorDB _db;
        
        public vwDailyTicketDataSummary()
        {
            m_Internal = new DatavwDailyTicketDataSummary();
             _db=new Jaxis.Inventory.Data.BeverageMonitorDB();
            Init();   
        }

        ///<summary>
        ///Set bool to true to assign NewGuid to PrimaryKey (if appropriate) and to call OnCreated
        ///</summary>
        public vwDailyTicketDataSummary( bool _CallOnCreated )
        {
            m_Internal = new DatavwDailyTicketDataSummary();
            _db=new Jaxis.Inventory.Data.BeverageMonitorDB();
            Init();  
            CallOnCreated( _CallOnCreated );
        }

        public vwDailyTicketDataSummary(string connectionString, string providerName) 
        {
            m_Internal = new DatavwDailyTicketDataSummary();
            _db=new Jaxis.Inventory.Data.BeverageMonitorDB(connectionString, providerName);
            Init();            
        }

        public vwDailyTicketDataSummary( vwDailyTicketDataSummary _Item )
        {
            m_Internal = new DatavwDailyTicketDataSummary();
            Copy( _Item );
            _db=new Jaxis.Inventory.Data.BeverageMonitorDB();
            Init();            
        }

        public vwDailyTicketDataSummary( DatavwDailyTicketDataSummary _Item )
        {
            m_Internal.Copy( _Item );
            _db=new Jaxis.Inventory.Data.BeverageMonitorDB();
            Init();            
        }         
         
        public void Copy( vwDailyTicketDataSummary _Item )
        {
            m_Internal.EstablishmentModified = _Item.EstablishmentModified;			
            m_Internal.GuestCount = _Item.GuestCount;			
            m_Internal.ModifiedGuestCount = _Item.ModifiedGuestCount;			
            m_Internal.TicketTotal = _Item.TicketTotal;			
            m_Internal.PaymentAmount = _Item.PaymentAmount;			
            m_Internal.TVAAmount = _Item.TVAAmount;			
        }         

        public void CallOnCreated( bool _CallOnCreated )
        {
            if( _CallOnCreated )
            {
	
                OnCreated( );
            }
        }
         
        void Init()
        {
            TestMode=this._db.DataProvider.ConnectionString.Equals("test", StringComparison.InvariantCultureIgnoreCase);
            _dirtyColumns=new List<IColumn>();
            if(TestMode)
            {
                vwDailyTicketDataSummary.SetTestRepo();
                _repo=_testRepo;
            }
            else
            {
                _repo = new SubSonicRepository<vwDailyTicketDataSummary>(_db);
            }
            tbl=_repo.GetTable();
            SetIsNew(true);
        }

        public vwDailyTicketDataSummary(Expression<Func<vwDailyTicketDataSummary, bool>> expression):this() 
        {
            SetIsLoaded(_repo.Load(this,expression));
        }
        
        internal static IRepository<vwDailyTicketDataSummary> GetRepo(string connectionString, string providerName)
        {
            Jaxis.Inventory.Data.BeverageMonitorDB db;

            db = String.IsNullOrEmpty(connectionString) ? new Jaxis.Inventory.Data.BeverageMonitorDB() : new Jaxis.Inventory.Data.BeverageMonitorDB(connectionString, providerName);

            /*
            if(String.IsNullOrEmpty(connectionString))
            {
                db=new Jaxis.Inventory.Data.BeverageMonitorDB();
            }
            else
            {
                db=new Jaxis.Inventory.Data.BeverageMonitorDB(connectionString, providerName);
            }
            */

            IRepository<vwDailyTicketDataSummary> _repo;
            
            if(db.TestMode)
            {
                vwDailyTicketDataSummary.SetTestRepo();
                _repo=_testRepo;
            }
            else
            {
                _repo = new SubSonicRepository<vwDailyTicketDataSummary>(db);
            }
            return _repo;        
        }       
        
        internal static IRepository<vwDailyTicketDataSummary> GetRepo()
        {
            return GetRepo("","");
        }
        
        public static vwDailyTicketDataSummary SingleOrDefault(Expression<Func<vwDailyTicketDataSummary, bool>> expression) 
        {
            return SingleOrDefault( expression, String.Empty, String.Empty );
        }      
        
        public static vwDailyTicketDataSummary SingleOrDefault(Expression<Func<vwDailyTicketDataSummary, bool>> expression,string connectionString, string providerName) 
        {
            IRepository<vwDailyTicketDataSummary> repo = GetRepo(connectionString,providerName);
            vwDailyTicketDataSummary single = repo.SingleOrDefault<vwDailyTicketDataSummary>( expression );
            if( null != single )
            {
                single.OnLoaded( );
            }
            return single;
        }
        
        public static bool Exists(Expression<Func<vwDailyTicketDataSummary, bool>> expression,string connectionString, string providerName) 
        {
            return All(connectionString,providerName).Any(expression);
        }        
        
        public static bool Exists(Expression<Func<vwDailyTicketDataSummary, bool>> expression) 
        {
            return All().Any(expression);
        }        

        
        public static vwDailyTicketDataSummary GetByID(DateTime value) 
        {
            return vwDailyTicketDataSummary.Find( L => L.TicketDate.Equals( value ) ).FirstOrDefault( );
        }

        public static IList<vwDailyTicketDataSummary> Find(Expression<Func<vwDailyTicketDataSummary, bool>> expression) 
        {
            var repo = GetRepo();
            return repo.Find(expression).ToList();
        }
        
        public static IList<vwDailyTicketDataSummary> Find(Expression<Func<vwDailyTicketDataSummary, bool>> expression,string connectionString, string providerName) 
        {
            var repo = GetRepo(connectionString,providerName);
            return repo.Find(expression).ToList();
        }

        public static IQueryable<vwDailyTicketDataSummary> All(string connectionString, string providerName) 
        {
            return GetRepo(connectionString,providerName).GetAll();
        }

        public static IQueryable<vwDailyTicketDataSummary> All() 
        {
            return GetRepo().GetAll();
        }
        
        public static PagedList<vwDailyTicketDataSummary> GetPaged(string sortBy, int pageIndex, int pageSize,string connectionString, string providerName) 
        {
            return GetRepo(connectionString,providerName).GetPaged(sortBy, pageIndex, pageSize);
        }
      
        public static PagedList<vwDailyTicketDataSummary> GetPaged(string sortBy, int pageIndex, int pageSize) 
        {
            return GetRepo().GetPaged(sortBy, pageIndex, pageSize);
        }

        public static PagedList<vwDailyTicketDataSummary> GetPaged(int pageIndex, int pageSize,string connectionString, string providerName) 
        {
            return GetRepo(connectionString,providerName).GetPaged(pageIndex, pageSize);
        }

        public static PagedList<vwDailyTicketDataSummary> GetPaged(int pageIndex, int pageSize) 
        {
            return GetRepo().GetPaged(pageIndex, pageSize);
        }

        public string KeyName()
        {
            return "TicketDate";
        }

        public object KeyValue()
        {
            return this.TicketDate;
        }
        
        public void SetKeyValue(object value) 
        {
            if (value != null && value!=DBNull.Value) 
            {
                var settable = value.ChangeTypeTo<DateTime>();
                this.GetType().GetProperty(this.KeyName()).SetValue(this, settable, null);
            }
        }
        
//        public override string ToString()
//        {
//			string rc = string.Empty;
//			if( null != this.EstablishmentModified )
//			{
//				rc = this.EstablishmentModified.ToString();
//			}
//            return rc;
//        }

        public override bool Equals(object obj)
        {
            if(obj == null)
                return false;
            if(obj is vwDailyTicketDataSummary)
            {
                vwDailyTicketDataSummary compare=(vwDailyTicketDataSummary)obj;
                return compare.KeyValue().Equals( this.KeyValue() );
                //return compare.KeyValue()==this.KeyValue();
            }
            else
            {
                return base.Equals(obj);
            }
        }



        public string DescriptorValue()
        {
            return this.EstablishmentModified.ToString();
        }

        public string DescriptorColumn() 
        {
            return "EstablishmentModified";
        }

        public static string GetKeyColumn()
        {
            return "TicketDate";
        }        

        public static string GetDescriptorColumn()
        {
            return "EstablishmentModified";
        }
        
        #region ' Foreign Keys '
        #endregion



//        DateTime? _TicketDate;
        [SubSonicPrimaryKey]
        [LocalData]
        public DateTime TicketDate
        {
            get { return m_Internal.TicketDate; }
            set
            {
                if(m_Internal.TicketDate!=value){
                    m_Internal.TicketDate=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="TicketDate");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        string _EstablishmentModified;
        [LocalData]
        public string EstablishmentModified
        {
            get { return m_Internal.EstablishmentModified; }
            set
            {
                if(m_Internal.EstablishmentModified!=value){
                    m_Internal.EstablishmentModified=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="EstablishmentModified");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        int? _GuestCount;
        [LocalData]
        public int? GuestCount
        {
            get { return m_Internal.GuestCount; }
            set
            {
                if(m_Internal.GuestCount!=value){
                    m_Internal.GuestCount=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="GuestCount");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        int? _ModifiedGuestCount;
        [LocalData]
        public int? ModifiedGuestCount
        {
            get { return m_Internal.ModifiedGuestCount; }
            set
            {
                if(m_Internal.ModifiedGuestCount!=value){
                    m_Internal.ModifiedGuestCount=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="ModifiedGuestCount");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        decimal? _TicketTotal;
        [LocalData]
        public decimal? TicketTotal
        {
            get { return m_Internal.TicketTotal; }
            set
            {
                if(m_Internal.TicketTotal!=value){
                    m_Internal.TicketTotal=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="TicketTotal");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        decimal? _PaymentAmount;
        [LocalData]
        public decimal? PaymentAmount
        {
            get { return m_Internal.PaymentAmount; }
            set
            {
                if(m_Internal.PaymentAmount!=value){
                    m_Internal.PaymentAmount=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="PaymentAmount");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        decimal? _TVAAmount;
        [LocalData]
        public decimal? TVAAmount
        {
            get { return m_Internal.TVAAmount; }
            set
            {
                if(m_Internal.TVAAmount!=value){
                    m_Internal.TVAAmount=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="TVAAmount");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }


        public DbCommand GetUpdateCommand() 
        {
            if(TestMode)
                return _db.DataProvider.CreateCommand();
            else
                return this.ToUpdateQuery(_db.Provider).GetCommand().ToDbCommand();
            
        }

        public DbCommand GetInsertCommand() 
        {
            if(TestMode)
                return _db.DataProvider.CreateCommand();
            else
                return this.ToInsertQuery(_db.Provider).GetCommand().ToDbCommand();
        }
        
        public DbCommand GetDeleteCommand() 
        {
            if(TestMode)
                return _db.DataProvider.CreateCommand();
            else
                return this.ToDeleteQuery(_db.Provider).GetCommand().ToDbCommand();
        }
       
        public void Update()
        {
            Update(_db.DataProvider);
        }
        
        public void Update(IDataProvider provider)
        {
            OnSaving( );
            
            if(this._dirtyColumns.Count>0)
            {
                _repo.Update(this,provider);
                _dirtyColumns.Clear();    
            }
            OnSaved();
       }
 
        public void Add()
        {
            Add(_db.DataProvider);
        }
        
        public void Add(IDataProvider provider)
        {
            OnSaving( );

            var key=KeyValue();
            if(key==null)
            {
                var newKey=_repo.Add(this,provider);
                this.SetKeyValue(newKey);
            }
            else
            {
                _repo.Add(this,provider);
            }
            SetIsNew(false);
            OnSaved();
        }
        
        public override void Save() 
        {
            Save(_db.DataProvider);
        }      

        public void Save(IDataProvider provider) 
        {
            if (_isNew) 
            {
                Add(provider);
            }
            else 
            {
                Update(provider);
            }
            SetIsLoaded( true );
        }

        public void Delete(IDataProvider provider) 
        {
                   
                 
            _repo.Delete(KeyValue());
                    }

        public void Delete() 
        {
            Delete(_db.DataProvider);
        }

        public static void Delete(Expression<Func<vwDailyTicketDataSummary, bool>> expression) 
        {
            var repo = GetRepo();
            
       
            repo.DeleteMany(expression);
        }

        
        public void Load(IDataReader rdr) 
        {
            Load(rdr, true);
        }

        public void Load(IDataReader rdr, bool closeReader) 
        {
            if (rdr.Read()) 
            {
                try 
                {
                    rdr.Load(this);
                    SetIsNew(false);
                    SetIsLoaded(true);
                }
                catch
                {
                    SetIsLoaded(false);
                    throw;
                }
            }
            else
            {
                SetIsLoaded(false);
            
            }

            if (closeReader)
                rdr.Dispose();
        }
    } 

    [DataContract]
    public partial class DatavwPOSTicketItem : IWCFDataElement
    {
        [DataMember]
        public Guid POSTicketID { get; set; }
        [DataMember]
        public string CheckNumber { get; set; }
        [DataMember]
        public int GuestCountModified { get; set; }
        [DataMember]
        public DateTime TicketDate { get; set; }
        [DataMember]
        public string Establishment { get; set; }
        [DataMember]
        public string ServerName { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string Comment { get; set; }
        [DataMember]
        public decimal? Price { get; set; }
        [DataMember]
        public int Quantity { get; set; }

        public void Copy( DatavwPOSTicketItem _Item )
        {
             CheckNumber = _Item.CheckNumber;			
             GuestCountModified = _Item.GuestCountModified;			
             TicketDate = _Item.TicketDate;			
             Establishment = _Item.Establishment;			
             ServerName = _Item.ServerName;			
             Description = _Item.Description;			
             Comment = _Item.Comment;			
             Price = _Item.Price;			
             Quantity = _Item.Quantity;			
        }

		public IBaseDataObject CreateDBObject( )
        {
            return new vwPOSTicketItem( this );
        }


    }


    /// <summary>
    /// A class which represents the vwPOSTicketItems table in the BeverageMonitor Database.
    /// </summary>
    public partial class vwPOSTicketItem: BaseDataObject<DatavwPOSTicketItem>, IActiveRecord, ICallOnCreated
    {
        #region Built-in testing
        [NonSerialized]
        static TestRepository<vwPOSTicketItem> _testRepo;
       
        static void SetTestRepo()
        {
            _testRepo = _testRepo ?? new TestRepository<vwPOSTicketItem>(new Jaxis.Inventory.Data.BeverageMonitorDB());
        }

        public static void ResetTestRepo()
        {
            _testRepo = null;
            SetTestRepo();
        }

        public static void Setup(List<vwPOSTicketItem> testlist)
        {
            SetTestRepo();
            foreach (var item in testlist)
            {
                _testRepo._items.Add(item);
            }
        }

        public static void Setup(vwPOSTicketItem item) 
        {
            SetTestRepo();
            _testRepo._items.Add(item);
        }

        public static void Setup(int testItems) 
        {
            SetTestRepo();
            for(int i=0;i<testItems;i++)
            {
                vwPOSTicketItem item=new vwPOSTicketItem();
                _testRepo._items.Add(item);
            }
        }
        
        public bool TestMode = false;
        #endregion

        IRepository<vwPOSTicketItem> _repo;

        partial void OnCreated();
            
        partial void OnLoaded();
        
        partial void OnSaving();
        
        partial void OnSaved();
        
        partial void OnChanged();

        public void SetIsLoaded(bool isLoaded)
        {
            _isLoaded=isLoaded;
            if(isLoaded)
                OnLoaded();
        }
        
        Jaxis.Inventory.Data.BeverageMonitorDB _db;
        
        public vwPOSTicketItem()
        {
            m_Internal = new DatavwPOSTicketItem();
             _db=new Jaxis.Inventory.Data.BeverageMonitorDB();
            Init();   
            this.POSTicketID = Guid.NewGuid( );     
        }

        ///<summary>
        ///Set bool to true to assign NewGuid to PrimaryKey (if appropriate) and to call OnCreated
        ///</summary>
        public vwPOSTicketItem( bool _CallOnCreated )
        {
            m_Internal = new DatavwPOSTicketItem();
            _db=new Jaxis.Inventory.Data.BeverageMonitorDB();
            Init();  
            CallOnCreated( _CallOnCreated );
        }

        public vwPOSTicketItem(string connectionString, string providerName) 
        {
            m_Internal = new DatavwPOSTicketItem();
            _db=new Jaxis.Inventory.Data.BeverageMonitorDB(connectionString, providerName);
            Init();            
            this.POSTicketID = Guid.NewGuid( );     
        }

        public vwPOSTicketItem( vwPOSTicketItem _Item )
        {
            m_Internal = new DatavwPOSTicketItem();
            Copy( _Item );
            _db=new Jaxis.Inventory.Data.BeverageMonitorDB();
            Init();            
        }

        public vwPOSTicketItem( DatavwPOSTicketItem _Item )
        {
            m_Internal.Copy( _Item );
            _db=new Jaxis.Inventory.Data.BeverageMonitorDB();
            Init();            
        }         
         
        public void Copy( vwPOSTicketItem _Item )
        {
            m_Internal.CheckNumber = _Item.CheckNumber;			
            m_Internal.GuestCountModified = _Item.GuestCountModified;			
            m_Internal.TicketDate = _Item.TicketDate;			
            m_Internal.Establishment = _Item.Establishment;			
            m_Internal.ServerName = _Item.ServerName;			
            m_Internal.Description = _Item.Description;			
            m_Internal.Comment = _Item.Comment;			
            m_Internal.Price = _Item.Price;			
            m_Internal.Quantity = _Item.Quantity;			
        }         

        public void CallOnCreated( bool _CallOnCreated )
        {
            if( _CallOnCreated )
            {
                m_Internal.POSTicketID = Guid.NewGuid( );     
	
                OnCreated( );
            }
        }
         
        void Init()
        {
            TestMode=this._db.DataProvider.ConnectionString.Equals("test", StringComparison.InvariantCultureIgnoreCase);
            _dirtyColumns=new List<IColumn>();
            if(TestMode)
            {
                vwPOSTicketItem.SetTestRepo();
                _repo=_testRepo;
            }
            else
            {
                _repo = new SubSonicRepository<vwPOSTicketItem>(_db);
            }
            tbl=_repo.GetTable();
            SetIsNew(true);
        }

        public vwPOSTicketItem(Expression<Func<vwPOSTicketItem, bool>> expression):this() 
        {
            SetIsLoaded(_repo.Load(this,expression));
        }
        
        internal static IRepository<vwPOSTicketItem> GetRepo(string connectionString, string providerName)
        {
            Jaxis.Inventory.Data.BeverageMonitorDB db;

            db = String.IsNullOrEmpty(connectionString) ? new Jaxis.Inventory.Data.BeverageMonitorDB() : new Jaxis.Inventory.Data.BeverageMonitorDB(connectionString, providerName);

            /*
            if(String.IsNullOrEmpty(connectionString))
            {
                db=new Jaxis.Inventory.Data.BeverageMonitorDB();
            }
            else
            {
                db=new Jaxis.Inventory.Data.BeverageMonitorDB(connectionString, providerName);
            }
            */

            IRepository<vwPOSTicketItem> _repo;
            
            if(db.TestMode)
            {
                vwPOSTicketItem.SetTestRepo();
                _repo=_testRepo;
            }
            else
            {
                _repo = new SubSonicRepository<vwPOSTicketItem>(db);
            }
            return _repo;        
        }       
        
        internal static IRepository<vwPOSTicketItem> GetRepo()
        {
            return GetRepo("","");
        }
        
        public static vwPOSTicketItem SingleOrDefault(Expression<Func<vwPOSTicketItem, bool>> expression) 
        {
            return SingleOrDefault( expression, String.Empty, String.Empty );
        }      
        
        public static vwPOSTicketItem SingleOrDefault(Expression<Func<vwPOSTicketItem, bool>> expression,string connectionString, string providerName) 
        {
            IRepository<vwPOSTicketItem> repo = GetRepo(connectionString,providerName);
            vwPOSTicketItem single = repo.SingleOrDefault<vwPOSTicketItem>( expression );
            if( null != single )
            {
                single.OnLoaded( );
            }
            return single;
        }
        
        public static bool Exists(Expression<Func<vwPOSTicketItem, bool>> expression,string connectionString, string providerName) 
        {
            return All(connectionString,providerName).Any(expression);
        }        
        
        public static bool Exists(Expression<Func<vwPOSTicketItem, bool>> expression) 
        {
            return All().Any(expression);
        }        

        protected static bool IsEmptyvwPOSTicketItemLoaded = false;
        protected static vwPOSTicketItem EmptyvwPOSTicketItemMember = null;

        public static vwPOSTicketItem GetByID(Guid? value) 
        {
            vwPOSTicketItem rc = null;
            if( value.HasValue )
            {
                rc = GetByID( value.Value );
            }
            return rc;
        }
        
        public static vwPOSTicketItem GetByID(Guid value) 
        {
            vwPOSTicketItem rc = null;
            if( null != value )
            {
                if( value == Guid.Empty )
                {
                    if( true == IsEmptyvwPOSTicketItemLoaded )
                    {
                        rc = EmptyvwPOSTicketItemMember;
                    }
                    else
                    {
                        IsEmptyvwPOSTicketItemLoaded = true;
                        rc = vwPOSTicketItem.Find( L => L.POSTicketID.Equals( value ) ).FirstOrDefault( );
                        EmptyvwPOSTicketItemMember = rc;
                    }
                }
                else
                {
                    rc = vwPOSTicketItem.Find( L => L.POSTicketID.Equals( value ) ).FirstOrDefault( );
                } 
            }
            return rc;
        }

        public static IList<vwPOSTicketItem> Find(Expression<Func<vwPOSTicketItem, bool>> expression) 
        {
            var repo = GetRepo();
            return repo.Find(expression).ToList();
        }
        
        public static IList<vwPOSTicketItem> Find(Expression<Func<vwPOSTicketItem, bool>> expression,string connectionString, string providerName) 
        {
            var repo = GetRepo(connectionString,providerName);
            return repo.Find(expression).ToList();
        }

        public static IQueryable<vwPOSTicketItem> All(string connectionString, string providerName) 
        {
            return GetRepo(connectionString,providerName).GetAll();
        }

        public static IQueryable<vwPOSTicketItem> All() 
        {
            return GetRepo().GetAll();
        }
        
        public static PagedList<vwPOSTicketItem> GetPaged(string sortBy, int pageIndex, int pageSize,string connectionString, string providerName) 
        {
            return GetRepo(connectionString,providerName).GetPaged(sortBy, pageIndex, pageSize);
        }
      
        public static PagedList<vwPOSTicketItem> GetPaged(string sortBy, int pageIndex, int pageSize) 
        {
            return GetRepo().GetPaged(sortBy, pageIndex, pageSize);
        }

        public static PagedList<vwPOSTicketItem> GetPaged(int pageIndex, int pageSize,string connectionString, string providerName) 
        {
            return GetRepo(connectionString,providerName).GetPaged(pageIndex, pageSize);
        }

        public static PagedList<vwPOSTicketItem> GetPaged(int pageIndex, int pageSize) 
        {
            return GetRepo().GetPaged(pageIndex, pageSize);
        }

        public string KeyName()
        {
            return "POSTicketID";
        }

        public object KeyValue()
        {
            return this.POSTicketID;
        }
        
        public void SetKeyValue(object value) 
        {
            if (value != null && value!=DBNull.Value) 
            {
                var settable = value.ChangeTypeTo<Guid>();
                this.GetType().GetProperty(this.KeyName()).SetValue(this, settable, null);
            }
        }
        
//        public override string ToString()
//        {
//			string rc = string.Empty;
//			if( null != this.CheckNumber )
//			{
//				rc = this.CheckNumber.ToString();
//			}
//            return rc;
//        }

        public override bool Equals(object obj)
        {
            if(obj == null)
                return false;
            if(obj is vwPOSTicketItem)
            {
                vwPOSTicketItem compare=(vwPOSTicketItem)obj;
                return compare.KeyValue().Equals( this.KeyValue() );
                //return compare.KeyValue()==this.KeyValue();
            }
            else
            {
                return base.Equals(obj);
            }
        }


        public override int GetHashCode() 
        {
            return this.POSTicketID.GetHashCode( );
        }

        public string DescriptorValue()
        {
            return this.CheckNumber.ToString();
        }

        public string DescriptorColumn() 
        {
            return "CheckNumber";
        }

        public static string GetKeyColumn()
        {
            return "POSTicketID";
        }        

        public static string GetDescriptorColumn()
        {
            return "CheckNumber";
        }
        
        #region ' Foreign Keys '
        #endregion


		[ObfuscationAttribute(Exclude=true)]
        public virtual Guid ObjectID 
        {
            get
            {
                return m_Internal.POSTicketID;
            }
            set
            {
                m_Internal.POSTicketID = value;
            }
        }


//        Guid _POSTicketID;
        [SubSonicPrimaryKey]
        [LocalData]
        public Guid POSTicketID
        {
            get { return m_Internal.POSTicketID; }
            set
            {
                if(m_Internal.POSTicketID!=value){
                    m_Internal.POSTicketID=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="POSTicketID");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        string _CheckNumber;
        [LocalData]
        public string CheckNumber
        {
            get { return m_Internal.CheckNumber; }
            set
            {
                if(m_Internal.CheckNumber!=value){
                    m_Internal.CheckNumber=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="CheckNumber");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        int _GuestCountModified;
        [LocalData]
        public int GuestCountModified
        {
            get { return m_Internal.GuestCountModified; }
            set
            {
                if(m_Internal.GuestCountModified!=value){
                    m_Internal.GuestCountModified=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="GuestCountModified");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        DateTime _TicketDate;
        [LocalData]
        public DateTime TicketDate
        {
            get { return m_Internal.TicketDate; }
            set
            {
                if(m_Internal.TicketDate!=value){
                    m_Internal.TicketDate=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="TicketDate");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        string _Establishment;
        [LocalData]
        public string Establishment
        {
            get { return m_Internal.Establishment; }
            set
            {
                if(m_Internal.Establishment!=value){
                    m_Internal.Establishment=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="Establishment");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        string _ServerName;
        [LocalData]
        public string ServerName
        {
            get { return m_Internal.ServerName; }
            set
            {
                if(m_Internal.ServerName!=value){
                    m_Internal.ServerName=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="ServerName");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        string _Description;
        [LocalData]
        public string Description
        {
            get { return m_Internal.Description; }
            set
            {
                if(m_Internal.Description!=value){
                    m_Internal.Description=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="Description");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        string _Comment;
        [LocalData]
        public string Comment
        {
            get { return m_Internal.Comment; }
            set
            {
                if(m_Internal.Comment!=value){
                    m_Internal.Comment=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="Comment");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        decimal? _Price;
        [LocalData]
        public decimal? Price
        {
            get { return m_Internal.Price; }
            set
            {
                if(m_Internal.Price!=value){
                    m_Internal.Price=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="Price");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        int _Quantity;
        [LocalData]
        public int Quantity
        {
            get { return m_Internal.Quantity; }
            set
            {
                if(m_Internal.Quantity!=value){
                    m_Internal.Quantity=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="Quantity");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }


        public DbCommand GetUpdateCommand() 
        {
            if(TestMode)
                return _db.DataProvider.CreateCommand();
            else
                return this.ToUpdateQuery(_db.Provider).GetCommand().ToDbCommand();
            
        }

        public DbCommand GetInsertCommand() 
        {
            if(TestMode)
                return _db.DataProvider.CreateCommand();
            else
                return this.ToInsertQuery(_db.Provider).GetCommand().ToDbCommand();
        }
        
        public DbCommand GetDeleteCommand() 
        {
            if(TestMode)
                return _db.DataProvider.CreateCommand();
            else
                return this.ToDeleteQuery(_db.Provider).GetCommand().ToDbCommand();
        }
       
        public void Update()
        {
            Update(_db.DataProvider);
        }
        
        public void Update(IDataProvider provider)
        {
            OnSaving( );
            
            if(this._dirtyColumns.Count>0)
            {
                _repo.Update(this,provider);
                _dirtyColumns.Clear();    
            }
            OnSaved();
       }
 
        public void Add()
        {
            Add(_db.DataProvider);
        }
        
        public void Add(IDataProvider provider)
        {
            OnSaving( );

            var key=KeyValue();
            if(key==null)
            {
                var newKey=_repo.Add(this,provider);
                this.SetKeyValue(newKey);
            }
            else
            {
                _repo.Add(this,provider);
            }
            SetIsNew(false);
            OnSaved();
        }
        
        public override void Save() 
        {
            Save(_db.DataProvider);
        }      

        public void Save(IDataProvider provider) 
        {
            if (_isNew) 
            {
                Add(provider);
            }
            else 
            {
                Update(provider);
            }
            SetIsLoaded( true );
        }

        public void Delete(IDataProvider provider) 
        {
                   
                 
            _repo.Delete(KeyValue());
                    }

        public void Delete() 
        {
            Delete(_db.DataProvider);
        }

        public static void Delete(Expression<Func<vwPOSTicketItem, bool>> expression) 
        {
            var repo = GetRepo();
            
       
            repo.DeleteMany(expression);
        }

        
        public void Load(IDataReader rdr) 
        {
            Load(rdr, true);
        }

        public void Load(IDataReader rdr, bool closeReader) 
        {
            if (rdr.Read()) 
            {
                try 
                {
                    rdr.Load(this);
                    SetIsNew(false);
                    SetIsLoaded(true);
                }
                catch
                {
                    SetIsLoaded(false);
                    throw;
                }
            }
            else
            {
                SetIsLoaded(false);
            
            }

            if (closeReader)
                rdr.Dispose();
        }
    } 

    [DataContract]
    public partial class DatavwPOSTicket : IWCFDataElement
    {
        [DataMember]
        public Guid POSTicketID { get; set; }
        [DataMember]
        public string CheckNumber { get; set; }
        [DataMember]
        public string Comments { get; set; }
        [DataMember]
        public DateTime TicketDate { get; set; }
        [DataMember]
        public string Establishment { get; set; }
        [DataMember]
        public string Server { get; set; }
        [DataMember]
        public string ServerName { get; set; }
        [DataMember]
        public int GuestCount { get; set; }
        [DataMember]
        public string CustomerTable { get; set; }
        [DataMember]
        public string RawData { get; set; }
        [DataMember]
        public int TouchCount { get; set; }
        [DataMember]
        public decimal TipAmount { get; set; }
        [DataMember]
        public int GuestCountModified { get; set; }
        [DataMember]
        public decimal? TicketTotal { get; set; }
        [DataMember]
        public string EstablishmentModified { get; set; }

        public void Copy( DatavwPOSTicket _Item )
        {
             CheckNumber = _Item.CheckNumber;			
             Comments = _Item.Comments;			
             TicketDate = _Item.TicketDate;			
             Establishment = _Item.Establishment;			
             Server = _Item.Server;			
             ServerName = _Item.ServerName;			
             GuestCount = _Item.GuestCount;			
             CustomerTable = _Item.CustomerTable;			
             RawData = _Item.RawData;			
             TouchCount = _Item.TouchCount;			
             TipAmount = _Item.TipAmount;			
             GuestCountModified = _Item.GuestCountModified;			
             TicketTotal = _Item.TicketTotal;			
             EstablishmentModified = _Item.EstablishmentModified;			
        }

		public IBaseDataObject CreateDBObject( )
        {
            return new vwPOSTicket( this );
        }


    }


    /// <summary>
    /// A class which represents the vwPOSTickets table in the BeverageMonitor Database.
    /// </summary>
    public partial class vwPOSTicket: BaseDataObject<DatavwPOSTicket>, IActiveRecord, ICallOnCreated
    {
        #region Built-in testing
        [NonSerialized]
        static TestRepository<vwPOSTicket> _testRepo;
       
        static void SetTestRepo()
        {
            _testRepo = _testRepo ?? new TestRepository<vwPOSTicket>(new Jaxis.Inventory.Data.BeverageMonitorDB());
        }

        public static void ResetTestRepo()
        {
            _testRepo = null;
            SetTestRepo();
        }

        public static void Setup(List<vwPOSTicket> testlist)
        {
            SetTestRepo();
            foreach (var item in testlist)
            {
                _testRepo._items.Add(item);
            }
        }

        public static void Setup(vwPOSTicket item) 
        {
            SetTestRepo();
            _testRepo._items.Add(item);
        }

        public static void Setup(int testItems) 
        {
            SetTestRepo();
            for(int i=0;i<testItems;i++)
            {
                vwPOSTicket item=new vwPOSTicket();
                _testRepo._items.Add(item);
            }
        }
        
        public bool TestMode = false;
        #endregion

        IRepository<vwPOSTicket> _repo;

        partial void OnCreated();
            
        partial void OnLoaded();
        
        partial void OnSaving();
        
        partial void OnSaved();
        
        partial void OnChanged();

        public void SetIsLoaded(bool isLoaded)
        {
            _isLoaded=isLoaded;
            if(isLoaded)
                OnLoaded();
        }
        
        Jaxis.Inventory.Data.BeverageMonitorDB _db;
        
        public vwPOSTicket()
        {
            m_Internal = new DatavwPOSTicket();
             _db=new Jaxis.Inventory.Data.BeverageMonitorDB();
            Init();   
            this.POSTicketID = Guid.NewGuid( );     
        }

        ///<summary>
        ///Set bool to true to assign NewGuid to PrimaryKey (if appropriate) and to call OnCreated
        ///</summary>
        public vwPOSTicket( bool _CallOnCreated )
        {
            m_Internal = new DatavwPOSTicket();
            _db=new Jaxis.Inventory.Data.BeverageMonitorDB();
            Init();  
            CallOnCreated( _CallOnCreated );
        }

        public vwPOSTicket(string connectionString, string providerName) 
        {
            m_Internal = new DatavwPOSTicket();
            _db=new Jaxis.Inventory.Data.BeverageMonitorDB(connectionString, providerName);
            Init();            
            this.POSTicketID = Guid.NewGuid( );     
        }

        public vwPOSTicket( vwPOSTicket _Item )
        {
            m_Internal = new DatavwPOSTicket();
            Copy( _Item );
            _db=new Jaxis.Inventory.Data.BeverageMonitorDB();
            Init();            
        }

        public vwPOSTicket( DatavwPOSTicket _Item )
        {
            m_Internal.Copy( _Item );
            _db=new Jaxis.Inventory.Data.BeverageMonitorDB();
            Init();            
        }         
         
        public void Copy( vwPOSTicket _Item )
        {
            m_Internal.CheckNumber = _Item.CheckNumber;			
            m_Internal.Comments = _Item.Comments;			
            m_Internal.TicketDate = _Item.TicketDate;			
            m_Internal.Establishment = _Item.Establishment;			
            m_Internal.Server = _Item.Server;			
            m_Internal.ServerName = _Item.ServerName;			
            m_Internal.GuestCount = _Item.GuestCount;			
            m_Internal.CustomerTable = _Item.CustomerTable;			
            m_Internal.RawData = _Item.RawData;			
            m_Internal.TouchCount = _Item.TouchCount;			
            m_Internal.TipAmount = _Item.TipAmount;			
            m_Internal.GuestCountModified = _Item.GuestCountModified;			
            m_Internal.TicketTotal = _Item.TicketTotal;			
            m_Internal.EstablishmentModified = _Item.EstablishmentModified;			
        }         

        public void CallOnCreated( bool _CallOnCreated )
        {
            if( _CallOnCreated )
            {
                m_Internal.POSTicketID = Guid.NewGuid( );     
	
                OnCreated( );
            }
        }
         
        void Init()
        {
            TestMode=this._db.DataProvider.ConnectionString.Equals("test", StringComparison.InvariantCultureIgnoreCase);
            _dirtyColumns=new List<IColumn>();
            if(TestMode)
            {
                vwPOSTicket.SetTestRepo();
                _repo=_testRepo;
            }
            else
            {
                _repo = new SubSonicRepository<vwPOSTicket>(_db);
            }
            tbl=_repo.GetTable();
            SetIsNew(true);
        }

        public vwPOSTicket(Expression<Func<vwPOSTicket, bool>> expression):this() 
        {
            SetIsLoaded(_repo.Load(this,expression));
        }
        
        internal static IRepository<vwPOSTicket> GetRepo(string connectionString, string providerName)
        {
            Jaxis.Inventory.Data.BeverageMonitorDB db;

            db = String.IsNullOrEmpty(connectionString) ? new Jaxis.Inventory.Data.BeverageMonitorDB() : new Jaxis.Inventory.Data.BeverageMonitorDB(connectionString, providerName);

            /*
            if(String.IsNullOrEmpty(connectionString))
            {
                db=new Jaxis.Inventory.Data.BeverageMonitorDB();
            }
            else
            {
                db=new Jaxis.Inventory.Data.BeverageMonitorDB(connectionString, providerName);
            }
            */

            IRepository<vwPOSTicket> _repo;
            
            if(db.TestMode)
            {
                vwPOSTicket.SetTestRepo();
                _repo=_testRepo;
            }
            else
            {
                _repo = new SubSonicRepository<vwPOSTicket>(db);
            }
            return _repo;        
        }       
        
        internal static IRepository<vwPOSTicket> GetRepo()
        {
            return GetRepo("","");
        }
        
        public static vwPOSTicket SingleOrDefault(Expression<Func<vwPOSTicket, bool>> expression) 
        {
            return SingleOrDefault( expression, String.Empty, String.Empty );
        }      
        
        public static vwPOSTicket SingleOrDefault(Expression<Func<vwPOSTicket, bool>> expression,string connectionString, string providerName) 
        {
            IRepository<vwPOSTicket> repo = GetRepo(connectionString,providerName);
            vwPOSTicket single = repo.SingleOrDefault<vwPOSTicket>( expression );
            if( null != single )
            {
                single.OnLoaded( );
            }
            return single;
        }
        
        public static bool Exists(Expression<Func<vwPOSTicket, bool>> expression,string connectionString, string providerName) 
        {
            return All(connectionString,providerName).Any(expression);
        }        
        
        public static bool Exists(Expression<Func<vwPOSTicket, bool>> expression) 
        {
            return All().Any(expression);
        }        

        protected static bool IsEmptyvwPOSTicketLoaded = false;
        protected static vwPOSTicket EmptyvwPOSTicketMember = null;

        public static vwPOSTicket GetByID(Guid? value) 
        {
            vwPOSTicket rc = null;
            if( value.HasValue )
            {
                rc = GetByID( value.Value );
            }
            return rc;
        }
        
        public static vwPOSTicket GetByID(Guid value) 
        {
            vwPOSTicket rc = null;
            if( null != value )
            {
                if( value == Guid.Empty )
                {
                    if( true == IsEmptyvwPOSTicketLoaded )
                    {
                        rc = EmptyvwPOSTicketMember;
                    }
                    else
                    {
                        IsEmptyvwPOSTicketLoaded = true;
                        rc = vwPOSTicket.Find( L => L.POSTicketID.Equals( value ) ).FirstOrDefault( );
                        EmptyvwPOSTicketMember = rc;
                    }
                }
                else
                {
                    rc = vwPOSTicket.Find( L => L.POSTicketID.Equals( value ) ).FirstOrDefault( );
                } 
            }
            return rc;
        }

        public static IList<vwPOSTicket> Find(Expression<Func<vwPOSTicket, bool>> expression) 
        {
            var repo = GetRepo();
            return repo.Find(expression).ToList();
        }
        
        public static IList<vwPOSTicket> Find(Expression<Func<vwPOSTicket, bool>> expression,string connectionString, string providerName) 
        {
            var repo = GetRepo(connectionString,providerName);
            return repo.Find(expression).ToList();
        }

        public static IQueryable<vwPOSTicket> All(string connectionString, string providerName) 
        {
            return GetRepo(connectionString,providerName).GetAll();
        }

        public static IQueryable<vwPOSTicket> All() 
        {
            return GetRepo().GetAll();
        }
        
        public static PagedList<vwPOSTicket> GetPaged(string sortBy, int pageIndex, int pageSize,string connectionString, string providerName) 
        {
            return GetRepo(connectionString,providerName).GetPaged(sortBy, pageIndex, pageSize);
        }
      
        public static PagedList<vwPOSTicket> GetPaged(string sortBy, int pageIndex, int pageSize) 
        {
            return GetRepo().GetPaged(sortBy, pageIndex, pageSize);
        }

        public static PagedList<vwPOSTicket> GetPaged(int pageIndex, int pageSize,string connectionString, string providerName) 
        {
            return GetRepo(connectionString,providerName).GetPaged(pageIndex, pageSize);
        }

        public static PagedList<vwPOSTicket> GetPaged(int pageIndex, int pageSize) 
        {
            return GetRepo().GetPaged(pageIndex, pageSize);
        }

        public string KeyName()
        {
            return "POSTicketID";
        }

        public object KeyValue()
        {
            return this.POSTicketID;
        }
        
        public void SetKeyValue(object value) 
        {
            if (value != null && value!=DBNull.Value) 
            {
                var settable = value.ChangeTypeTo<Guid>();
                this.GetType().GetProperty(this.KeyName()).SetValue(this, settable, null);
            }
        }
        
//        public override string ToString()
//        {
//			string rc = string.Empty;
//			if( null != this.CheckNumber )
//			{
//				rc = this.CheckNumber.ToString();
//			}
//            return rc;
//        }

        public override bool Equals(object obj)
        {
            if(obj == null)
                return false;
            if(obj is vwPOSTicket)
            {
                vwPOSTicket compare=(vwPOSTicket)obj;
                return compare.KeyValue().Equals( this.KeyValue() );
                //return compare.KeyValue()==this.KeyValue();
            }
            else
            {
                return base.Equals(obj);
            }
        }


        public override int GetHashCode() 
        {
            return this.POSTicketID.GetHashCode( );
        }

        public string DescriptorValue()
        {
            return this.CheckNumber.ToString();
        }

        public string DescriptorColumn() 
        {
            return "CheckNumber";
        }

        public static string GetKeyColumn()
        {
            return "POSTicketID";
        }        

        public static string GetDescriptorColumn()
        {
            return "CheckNumber";
        }
        
        #region ' Foreign Keys '
        #endregion


		[ObfuscationAttribute(Exclude=true)]
        public virtual Guid ObjectID 
        {
            get
            {
                return m_Internal.POSTicketID;
            }
            set
            {
                m_Internal.POSTicketID = value;
            }
        }


//        Guid _POSTicketID;
        [SubSonicPrimaryKey]
        [LocalData]
        public Guid POSTicketID
        {
            get { return m_Internal.POSTicketID; }
            set
            {
                if(m_Internal.POSTicketID!=value){
                    m_Internal.POSTicketID=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="POSTicketID");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        string _CheckNumber;
        [LocalData]
        public string CheckNumber
        {
            get { return m_Internal.CheckNumber; }
            set
            {
                if(m_Internal.CheckNumber!=value){
                    m_Internal.CheckNumber=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="CheckNumber");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        string _Comments;
        [LocalData]
        public string Comments
        {
            get { return m_Internal.Comments; }
            set
            {
                if(m_Internal.Comments!=value){
                    m_Internal.Comments=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="Comments");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        DateTime _TicketDate;
        [LocalData]
        public DateTime TicketDate
        {
            get { return m_Internal.TicketDate; }
            set
            {
                if(m_Internal.TicketDate!=value){
                    m_Internal.TicketDate=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="TicketDate");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        string _Establishment;
        [LocalData]
        public string Establishment
        {
            get { return m_Internal.Establishment; }
            set
            {
                if(m_Internal.Establishment!=value){
                    m_Internal.Establishment=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="Establishment");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        string _Server;
        [LocalData]
        public string Server
        {
            get { return m_Internal.Server; }
            set
            {
                if(m_Internal.Server!=value){
                    m_Internal.Server=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="Server");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        string _ServerName;
        [LocalData]
        public string ServerName
        {
            get { return m_Internal.ServerName; }
            set
            {
                if(m_Internal.ServerName!=value){
                    m_Internal.ServerName=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="ServerName");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        int _GuestCount;
        [LocalData]
        public int GuestCount
        {
            get { return m_Internal.GuestCount; }
            set
            {
                if(m_Internal.GuestCount!=value){
                    m_Internal.GuestCount=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="GuestCount");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        string _CustomerTable;
        [LocalData]
        public string CustomerTable
        {
            get { return m_Internal.CustomerTable; }
            set
            {
                if(m_Internal.CustomerTable!=value){
                    m_Internal.CustomerTable=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="CustomerTable");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        string _RawData;
        [LocalData]
        public string RawData
        {
            get { return m_Internal.RawData; }
            set
            {
                if(m_Internal.RawData!=value){
                    m_Internal.RawData=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="RawData");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        int _TouchCount;
        [LocalData]
        public int TouchCount
        {
            get { return m_Internal.TouchCount; }
            set
            {
                if(m_Internal.TouchCount!=value){
                    m_Internal.TouchCount=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="TouchCount");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        decimal _TipAmount;
        [LocalData]
        public decimal TipAmount
        {
            get { return m_Internal.TipAmount; }
            set
            {
                if(m_Internal.TipAmount!=value){
                    m_Internal.TipAmount=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="TipAmount");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        int _GuestCountModified;
        [LocalData]
        public int GuestCountModified
        {
            get { return m_Internal.GuestCountModified; }
            set
            {
                if(m_Internal.GuestCountModified!=value){
                    m_Internal.GuestCountModified=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="GuestCountModified");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        decimal? _TicketTotal;
        [LocalData]
        public decimal? TicketTotal
        {
            get { return m_Internal.TicketTotal; }
            set
            {
                if(m_Internal.TicketTotal!=value){
                    m_Internal.TicketTotal=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="TicketTotal");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        string _EstablishmentModified;
        [LocalData]
        public string EstablishmentModified
        {
            get { return m_Internal.EstablishmentModified; }
            set
            {
                if(m_Internal.EstablishmentModified!=value){
                    m_Internal.EstablishmentModified=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="EstablishmentModified");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }


        public DbCommand GetUpdateCommand() 
        {
            if(TestMode)
                return _db.DataProvider.CreateCommand();
            else
                return this.ToUpdateQuery(_db.Provider).GetCommand().ToDbCommand();
            
        }

        public DbCommand GetInsertCommand() 
        {
            if(TestMode)
                return _db.DataProvider.CreateCommand();
            else
                return this.ToInsertQuery(_db.Provider).GetCommand().ToDbCommand();
        }
        
        public DbCommand GetDeleteCommand() 
        {
            if(TestMode)
                return _db.DataProvider.CreateCommand();
            else
                return this.ToDeleteQuery(_db.Provider).GetCommand().ToDbCommand();
        }
       
        public void Update()
        {
            Update(_db.DataProvider);
        }
        
        public void Update(IDataProvider provider)
        {
            OnSaving( );
            
            if(this._dirtyColumns.Count>0)
            {
                _repo.Update(this,provider);
                _dirtyColumns.Clear();    
            }
            OnSaved();
       }
 
        public void Add()
        {
            Add(_db.DataProvider);
        }
        
        public void Add(IDataProvider provider)
        {
            OnSaving( );

            var key=KeyValue();
            if(key==null)
            {
                var newKey=_repo.Add(this,provider);
                this.SetKeyValue(newKey);
            }
            else
            {
                _repo.Add(this,provider);
            }
            SetIsNew(false);
            OnSaved();
        }
        
        public override void Save() 
        {
            Save(_db.DataProvider);
        }      

        public void Save(IDataProvider provider) 
        {
            if (_isNew) 
            {
                Add(provider);
            }
            else 
            {
                Update(provider);
            }
            SetIsLoaded( true );
        }

        public void Delete(IDataProvider provider) 
        {
                   
                 
            _repo.Delete(KeyValue());
                    }

        public void Delete() 
        {
            Delete(_db.DataProvider);
        }

        public static void Delete(Expression<Func<vwPOSTicket, bool>> expression) 
        {
            var repo = GetRepo();
            
       
            repo.DeleteMany(expression);
        }

        
        public void Load(IDataReader rdr) 
        {
            Load(rdr, true);
        }

        public void Load(IDataReader rdr, bool closeReader) 
        {
            if (rdr.Read()) 
            {
                try 
                {
                    rdr.Load(this);
                    SetIsNew(false);
                    SetIsLoaded(true);
                }
                catch
                {
                    SetIsLoaded(false);
                    throw;
                }
            }
            else
            {
                SetIsLoaded(false);
            
            }

            if (closeReader)
                rdr.Dispose();
        }
    } 

    [DataContract]
    public partial class DatavwSumaryItemDatum : IWCFDataElement
    {
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public int? Count { get; set; }
        [DataMember]
        public int? Quantity { get; set; }
        [DataMember]
        public decimal? Price { get; set; }
        [DataMember]
        public decimal? AveragePrice { get; set; }

        public void Copy( DatavwSumaryItemDatum _Item )
        {
             Count = _Item.Count;			
             Quantity = _Item.Quantity;			
             Price = _Item.Price;			
             AveragePrice = _Item.AveragePrice;			
        }

		public IBaseDataObject CreateDBObject( )
        {
            return new vwSumaryItemDatum( this );
        }


    }


    /// <summary>
    /// A class which represents the vwSumaryItemData table in the BeverageMonitor Database.
    /// </summary>
    public partial class vwSumaryItemDatum: BaseDataObject<DatavwSumaryItemDatum>, IActiveRecord, ICallOnCreated
    {
        #region Built-in testing
        [NonSerialized]
        static TestRepository<vwSumaryItemDatum> _testRepo;
       
        static void SetTestRepo()
        {
            _testRepo = _testRepo ?? new TestRepository<vwSumaryItemDatum>(new Jaxis.Inventory.Data.BeverageMonitorDB());
        }

        public static void ResetTestRepo()
        {
            _testRepo = null;
            SetTestRepo();
        }

        public static void Setup(List<vwSumaryItemDatum> testlist)
        {
            SetTestRepo();
            foreach (var item in testlist)
            {
                _testRepo._items.Add(item);
            }
        }

        public static void Setup(vwSumaryItemDatum item) 
        {
            SetTestRepo();
            _testRepo._items.Add(item);
        }

        public static void Setup(int testItems) 
        {
            SetTestRepo();
            for(int i=0;i<testItems;i++)
            {
                vwSumaryItemDatum item=new vwSumaryItemDatum();
                _testRepo._items.Add(item);
            }
        }
        
        public bool TestMode = false;
        #endregion

        IRepository<vwSumaryItemDatum> _repo;

        partial void OnCreated();
            
        partial void OnLoaded();
        
        partial void OnSaving();
        
        partial void OnSaved();
        
        partial void OnChanged();

        public void SetIsLoaded(bool isLoaded)
        {
            _isLoaded=isLoaded;
            if(isLoaded)
                OnLoaded();
        }
        
        Jaxis.Inventory.Data.BeverageMonitorDB _db;
        
        public vwSumaryItemDatum()
        {
            m_Internal = new DatavwSumaryItemDatum();
             _db=new Jaxis.Inventory.Data.BeverageMonitorDB();
            Init();   
        }

        ///<summary>
        ///Set bool to true to assign NewGuid to PrimaryKey (if appropriate) and to call OnCreated
        ///</summary>
        public vwSumaryItemDatum( bool _CallOnCreated )
        {
            m_Internal = new DatavwSumaryItemDatum();
            _db=new Jaxis.Inventory.Data.BeverageMonitorDB();
            Init();  
            CallOnCreated( _CallOnCreated );
        }

        public vwSumaryItemDatum(string connectionString, string providerName) 
        {
            m_Internal = new DatavwSumaryItemDatum();
            _db=new Jaxis.Inventory.Data.BeverageMonitorDB(connectionString, providerName);
            Init();            
        }

        public vwSumaryItemDatum( vwSumaryItemDatum _Item )
        {
            m_Internal = new DatavwSumaryItemDatum();
            Copy( _Item );
            _db=new Jaxis.Inventory.Data.BeverageMonitorDB();
            Init();            
        }

        public vwSumaryItemDatum( DatavwSumaryItemDatum _Item )
        {
            m_Internal.Copy( _Item );
            _db=new Jaxis.Inventory.Data.BeverageMonitorDB();
            Init();            
        }         
         
        public void Copy( vwSumaryItemDatum _Item )
        {
            m_Internal.Count = _Item.Count;			
            m_Internal.Quantity = _Item.Quantity;			
            m_Internal.Price = _Item.Price;			
            m_Internal.AveragePrice = _Item.AveragePrice;			
        }         

        public void CallOnCreated( bool _CallOnCreated )
        {
            if( _CallOnCreated )
            {
	
                OnCreated( );
            }
        }
         
        void Init()
        {
            TestMode=this._db.DataProvider.ConnectionString.Equals("test", StringComparison.InvariantCultureIgnoreCase);
            _dirtyColumns=new List<IColumn>();
            if(TestMode)
            {
                vwSumaryItemDatum.SetTestRepo();
                _repo=_testRepo;
            }
            else
            {
                _repo = new SubSonicRepository<vwSumaryItemDatum>(_db);
            }
            tbl=_repo.GetTable();
            SetIsNew(true);
        }

        public vwSumaryItemDatum(Expression<Func<vwSumaryItemDatum, bool>> expression):this() 
        {
            SetIsLoaded(_repo.Load(this,expression));
        }
        
        internal static IRepository<vwSumaryItemDatum> GetRepo(string connectionString, string providerName)
        {
            Jaxis.Inventory.Data.BeverageMonitorDB db;

            db = String.IsNullOrEmpty(connectionString) ? new Jaxis.Inventory.Data.BeverageMonitorDB() : new Jaxis.Inventory.Data.BeverageMonitorDB(connectionString, providerName);

            /*
            if(String.IsNullOrEmpty(connectionString))
            {
                db=new Jaxis.Inventory.Data.BeverageMonitorDB();
            }
            else
            {
                db=new Jaxis.Inventory.Data.BeverageMonitorDB(connectionString, providerName);
            }
            */

            IRepository<vwSumaryItemDatum> _repo;
            
            if(db.TestMode)
            {
                vwSumaryItemDatum.SetTestRepo();
                _repo=_testRepo;
            }
            else
            {
                _repo = new SubSonicRepository<vwSumaryItemDatum>(db);
            }
            return _repo;        
        }       
        
        internal static IRepository<vwSumaryItemDatum> GetRepo()
        {
            return GetRepo("","");
        }
        
        public static vwSumaryItemDatum SingleOrDefault(Expression<Func<vwSumaryItemDatum, bool>> expression) 
        {
            return SingleOrDefault( expression, String.Empty, String.Empty );
        }      
        
        public static vwSumaryItemDatum SingleOrDefault(Expression<Func<vwSumaryItemDatum, bool>> expression,string connectionString, string providerName) 
        {
            IRepository<vwSumaryItemDatum> repo = GetRepo(connectionString,providerName);
            vwSumaryItemDatum single = repo.SingleOrDefault<vwSumaryItemDatum>( expression );
            if( null != single )
            {
                single.OnLoaded( );
            }
            return single;
        }
        
        public static bool Exists(Expression<Func<vwSumaryItemDatum, bool>> expression,string connectionString, string providerName) 
        {
            return All(connectionString,providerName).Any(expression);
        }        
        
        public static bool Exists(Expression<Func<vwSumaryItemDatum, bool>> expression) 
        {
            return All().Any(expression);
        }        

        
        public static vwSumaryItemDatum GetByID(string value) 
        {
            return vwSumaryItemDatum.Find( L => L.Description.Equals( value ) ).FirstOrDefault( );
        }

        public static IList<vwSumaryItemDatum> Find(Expression<Func<vwSumaryItemDatum, bool>> expression) 
        {
            var repo = GetRepo();
            return repo.Find(expression).ToList();
        }
        
        public static IList<vwSumaryItemDatum> Find(Expression<Func<vwSumaryItemDatum, bool>> expression,string connectionString, string providerName) 
        {
            var repo = GetRepo(connectionString,providerName);
            return repo.Find(expression).ToList();
        }

        public static IQueryable<vwSumaryItemDatum> All(string connectionString, string providerName) 
        {
            return GetRepo(connectionString,providerName).GetAll();
        }

        public static IQueryable<vwSumaryItemDatum> All() 
        {
            return GetRepo().GetAll();
        }
        
        public static PagedList<vwSumaryItemDatum> GetPaged(string sortBy, int pageIndex, int pageSize,string connectionString, string providerName) 
        {
            return GetRepo(connectionString,providerName).GetPaged(sortBy, pageIndex, pageSize);
        }
      
        public static PagedList<vwSumaryItemDatum> GetPaged(string sortBy, int pageIndex, int pageSize) 
        {
            return GetRepo().GetPaged(sortBy, pageIndex, pageSize);
        }

        public static PagedList<vwSumaryItemDatum> GetPaged(int pageIndex, int pageSize,string connectionString, string providerName) 
        {
            return GetRepo(connectionString,providerName).GetPaged(pageIndex, pageSize);
        }

        public static PagedList<vwSumaryItemDatum> GetPaged(int pageIndex, int pageSize) 
        {
            return GetRepo().GetPaged(pageIndex, pageSize);
        }

        public string KeyName()
        {
            return "Description";
        }

        public object KeyValue()
        {
            return this.Description;
        }
        
        public void SetKeyValue(object value) 
        {
            if (value != null && value!=DBNull.Value) 
            {
                var settable = value.ChangeTypeTo<string>();
                this.GetType().GetProperty(this.KeyName()).SetValue(this, settable, null);
            }
        }
        
//        public override string ToString()
//        {
//			string rc = string.Empty;
//			if( null != this.Description )
//			{
//				rc = this.Description.ToString();
//			}
//            return rc;
//        }

        public override bool Equals(object obj)
        {
            if(obj == null)
                return false;
            if(obj is vwSumaryItemDatum)
            {
                vwSumaryItemDatum compare=(vwSumaryItemDatum)obj;
                return compare.KeyValue().Equals( this.KeyValue() );
                //return compare.KeyValue()==this.KeyValue();
            }
            else
            {
                return base.Equals(obj);
            }
        }



        public string DescriptorValue()
        {
            return this.Description.ToString();
        }

        public string DescriptorColumn() 
        {
            return "Description";
        }

        public static string GetKeyColumn()
        {
            return "Description";
        }        

        public static string GetDescriptorColumn()
        {
            return "Description";
        }
        
        #region ' Foreign Keys '
        #endregion



//        string _Description;
        [SubSonicPrimaryKey]
        [LocalData]
        public string Description
        {
            get { return m_Internal.Description; }
            set
            {
                if(m_Internal.Description!=value){
                    m_Internal.Description=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="Description");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        int? _Count;
        [LocalData]
        public int? Count
        {
            get { return m_Internal.Count; }
            set
            {
                if(m_Internal.Count!=value){
                    m_Internal.Count=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="Count");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        int? _Quantity;
        [LocalData]
        public int? Quantity
        {
            get { return m_Internal.Quantity; }
            set
            {
                if(m_Internal.Quantity!=value){
                    m_Internal.Quantity=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="Quantity");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        decimal? _Price;
        [LocalData]
        public decimal? Price
        {
            get { return m_Internal.Price; }
            set
            {
                if(m_Internal.Price!=value){
                    m_Internal.Price=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="Price");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        decimal? _AveragePrice;
        [LocalData]
        public decimal? AveragePrice
        {
            get { return m_Internal.AveragePrice; }
            set
            {
                if(m_Internal.AveragePrice!=value){
                    m_Internal.AveragePrice=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="AveragePrice");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }


        public DbCommand GetUpdateCommand() 
        {
            if(TestMode)
                return _db.DataProvider.CreateCommand();
            else
                return this.ToUpdateQuery(_db.Provider).GetCommand().ToDbCommand();
            
        }

        public DbCommand GetInsertCommand() 
        {
            if(TestMode)
                return _db.DataProvider.CreateCommand();
            else
                return this.ToInsertQuery(_db.Provider).GetCommand().ToDbCommand();
        }
        
        public DbCommand GetDeleteCommand() 
        {
            if(TestMode)
                return _db.DataProvider.CreateCommand();
            else
                return this.ToDeleteQuery(_db.Provider).GetCommand().ToDbCommand();
        }
       
        public void Update()
        {
            Update(_db.DataProvider);
        }
        
        public void Update(IDataProvider provider)
        {
            OnSaving( );
            
            if(this._dirtyColumns.Count>0)
            {
                _repo.Update(this,provider);
                _dirtyColumns.Clear();    
            }
            OnSaved();
       }
 
        public void Add()
        {
            Add(_db.DataProvider);
        }
        
        public void Add(IDataProvider provider)
        {
            OnSaving( );

            var key=KeyValue();
            if(key==null)
            {
                var newKey=_repo.Add(this,provider);
                this.SetKeyValue(newKey);
            }
            else
            {
                _repo.Add(this,provider);
            }
            SetIsNew(false);
            OnSaved();
        }
        
        public override void Save() 
        {
            Save(_db.DataProvider);
        }      

        public void Save(IDataProvider provider) 
        {
            if (_isNew) 
            {
                Add(provider);
            }
            else 
            {
                Update(provider);
            }
            SetIsLoaded( true );
        }

        public void Delete(IDataProvider provider) 
        {
                   
                 
            _repo.Delete(KeyValue());
                    }

        public void Delete() 
        {
            Delete(_db.DataProvider);
        }

        public static void Delete(Expression<Func<vwSumaryItemDatum, bool>> expression) 
        {
            var repo = GetRepo();
            
       
            repo.DeleteMany(expression);
        }

        
        public void Load(IDataReader rdr) 
        {
            Load(rdr, true);
        }

        public void Load(IDataReader rdr, bool closeReader) 
        {
            if (rdr.Read()) 
            {
                try 
                {
                    rdr.Load(this);
                    SetIsNew(false);
                    SetIsLoaded(true);
                }
                catch
                {
                    SetIsLoaded(false);
                    throw;
                }
            }
            else
            {
                SetIsLoaded(false);
            
            }

            if (closeReader)
                rdr.Dispose();
        }
    } 
}
