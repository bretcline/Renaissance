


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

namespace Jaxis.POS.CustomerData
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
    public partial class DataCustomerEmail : IWCFDataElement
    {
        [DataMember]
        public Guid ReportEmailID { get; set; }
        [DataMember]
        public Guid CustomerID { get; set; }
        [DataMember]
        public string EmailAddress { get; set; }
        [DataMember]
        public string Name { get; set; }

        public void Copy( DataCustomerEmail _Item )
        {
             CustomerID = _Item.CustomerID;			
             EmailAddress = _Item.EmailAddress;			
             Name = _Item.Name;			
        }

		public IBaseDataObject CreateDBObject( )
        {
            return new CustomerEmail( this );
        }


    }


    /// <summary>
    /// A class which represents the CustomerEmails table in the CustomerData Database.
    /// </summary>
    public partial class CustomerEmail: BaseDataObject<DataCustomerEmail>, IActiveRecord, ICallOnCreated
    {
        #region Built-in testing
        [NonSerialized]
        static TestRepository<CustomerEmail> _testRepo;
       
        static void SetTestRepo()
        {
            _testRepo = _testRepo ?? new TestRepository<CustomerEmail>(new Jaxis.POS.CustomerData.CustomerDataDB());
        }

        public static void ResetTestRepo()
        {
            _testRepo = null;
            SetTestRepo();
        }

        public static void Setup(List<CustomerEmail> testlist)
        {
            SetTestRepo();
            foreach (var item in testlist)
            {
                _testRepo._items.Add(item);
            }
        }

        public static void Setup(CustomerEmail item) 
        {
            SetTestRepo();
            _testRepo._items.Add(item);
        }

        public static void Setup(int testItems) 
        {
            SetTestRepo();
            for(int i=0;i<testItems;i++)
            {
                CustomerEmail item=new CustomerEmail();
                _testRepo._items.Add(item);
            }
        }
        
        public bool TestMode = false;
        #endregion

        
		IRepository<CustomerEmail> _repo;

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
        
        
		Jaxis.POS.CustomerData.CustomerDataDB _db;
        
        public CustomerEmail()
        {
            m_Internal = new DataCustomerEmail();
             _db=new Jaxis.POS.CustomerData.CustomerDataDB();
            Init();   
            this.ReportEmailID = Guid.NewGuid( );     
        }

        ///<summary>
        ///Set bool to true to assign NewGuid to PrimaryKey (if appropriate) and to call OnCreated
        ///</summary>
        public CustomerEmail( bool _CallOnCreated )
        {
            m_Internal = new DataCustomerEmail();
            _db=new Jaxis.POS.CustomerData.CustomerDataDB();
            Init();  
            CallOnCreated( _CallOnCreated );
        }

        public CustomerEmail(string connectionString, string providerName) 
        {
            m_Internal = new DataCustomerEmail();
            _db=new Jaxis.POS.CustomerData.CustomerDataDB(connectionString, providerName);
            Init();            
            this.ReportEmailID = Guid.NewGuid( );     
        }

        public CustomerEmail( CustomerEmail _Item )
        {
            m_Internal = new DataCustomerEmail();
            Copy( _Item );
            _db=new Jaxis.POS.CustomerData.CustomerDataDB();
            Init();            
        }

        public CustomerEmail( DataCustomerEmail _Item )
        {
            m_Internal.Copy( _Item );
            _db=new Jaxis.POS.CustomerData.CustomerDataDB();
            Init();            
        }         
         
        public void Copy( CustomerEmail _Item )
        {
            m_Internal.CustomerID = _Item.CustomerID;			
            m_Internal.EmailAddress = _Item.EmailAddress;			
            m_Internal.Name = _Item.Name;			
        }         

        public void CallOnCreated( bool _CallOnCreated )
        {
            if( _CallOnCreated )
            {
                m_Internal.ReportEmailID = Guid.NewGuid( );     
	
                OnCreated( );
            }
        }
         
        void Init()
        {
            TestMode=this._db.DataProvider.ConnectionString.Equals("test", StringComparison.InvariantCultureIgnoreCase);
            _dirtyColumns=new List<IColumn>();
            if(TestMode)
            {
                CustomerEmail.SetTestRepo();
                _repo=_testRepo;
            }
            else
            {
                _repo = new SubSonicRepository<CustomerEmail>(_db);
            }
            tbl=_repo.GetTable();
            SetIsNew(true);
        }

        public CustomerEmail(Expression<Func<CustomerEmail, bool>> expression):this() 
        {
            SetIsLoaded(_repo.Load(this,expression));
        }
        
        internal static IRepository<CustomerEmail> GetRepo(string connectionString, string providerName)
        {
            Jaxis.POS.CustomerData.CustomerDataDB db;

            db = String.IsNullOrEmpty(connectionString) ? new Jaxis.POS.CustomerData.CustomerDataDB() : new Jaxis.POS.CustomerData.CustomerDataDB(connectionString, providerName);

            /*
            if(String.IsNullOrEmpty(connectionString))
            {
                db=new Jaxis.POS.CustomerData.CustomerDataDB();
            }
            else
            {
                db=new Jaxis.POS.CustomerData.CustomerDataDB(connectionString, providerName);
            }
            */

            IRepository<CustomerEmail> _repo;
            
            if(db.TestMode)
            {
                CustomerEmail.SetTestRepo();
                _repo=_testRepo;
            }
            else
            {
                _repo = new SubSonicRepository<CustomerEmail>(db);
            }
            return _repo;        
        }       
        
        internal static IRepository<CustomerEmail> GetRepo()
        {
            return GetRepo("","");
        }
        
        public static CustomerEmail SingleOrDefault(Expression<Func<CustomerEmail, bool>> expression) 
        {
            return SingleOrDefault( expression, String.Empty, String.Empty );
        }      
        
        public static CustomerEmail SingleOrDefault(Expression<Func<CustomerEmail, bool>> expression,string connectionString, string providerName) 
        {
            IRepository<CustomerEmail> repo = GetRepo(connectionString,providerName);
            CustomerEmail single = repo.SingleOrDefault<CustomerEmail>( expression );
            if( null != single )
            {
                single.OnLoaded( );
            }
            return single;
        }
        
        public static bool Exists(Expression<Func<CustomerEmail, bool>> expression,string connectionString, string providerName) 
        {
            return All(connectionString,providerName).Any(expression);
        }        
        
        public static bool Exists(Expression<Func<CustomerEmail, bool>> expression) 
        {
            return All().Any(expression);
        }        

        protected static bool IsEmptyCustomerEmailLoaded = false;
        protected static CustomerEmail EmptyCustomerEmailMember = null;

        public static CustomerEmail GetByID(Guid? value) 
        {
            CustomerEmail rc = null;
            if( value.HasValue )
            {
                rc = GetByID( value.Value );
            }
            return rc;
        }
        
        public static CustomerEmail GetByID(Guid value) 
        {
            CustomerEmail rc = null;
            if( null != value )
            {
                if( value == Guid.Empty )
                {
                    if( true == IsEmptyCustomerEmailLoaded )
                    {
                        rc = EmptyCustomerEmailMember;
                    }
                    else
                    {
                        IsEmptyCustomerEmailLoaded = true;
                        rc = CustomerEmail.Find( L => L.ReportEmailID.Equals( value ) ).FirstOrDefault( );
                        EmptyCustomerEmailMember = rc;
                    }
                }
                else
                {
                    rc = CustomerEmail.Find( L => L.ReportEmailID.Equals( value ) ).FirstOrDefault( );
                } 
            }
            return rc;
        }

        public static IList<CustomerEmail> Find(Expression<Func<CustomerEmail, bool>> expression) 
        {
            var repo = GetRepo();
            return repo.Find(expression).ToList();
        }
        
        public static IList<CustomerEmail> Find(Expression<Func<CustomerEmail, bool>> expression,string connectionString, string providerName) 
        {
            var repo = GetRepo(connectionString,providerName);
            return repo.Find(expression).ToList();
        }

        public static IQueryable<CustomerEmail> All(string connectionString, string providerName) 
        {
            return GetRepo(connectionString,providerName).GetAll();
        }

        public static IQueryable<CustomerEmail> All() 
        {
            return GetRepo().GetAll();
        }
        
        public static PagedList<CustomerEmail> GetPaged(string sortBy, int pageIndex, int pageSize,string connectionString, string providerName) 
        {
            return GetRepo(connectionString,providerName).GetPaged(sortBy, pageIndex, pageSize);
        }
      
        public static PagedList<CustomerEmail> GetPaged(string sortBy, int pageIndex, int pageSize) 
        {
            return GetRepo().GetPaged(sortBy, pageIndex, pageSize);
        }

        public static PagedList<CustomerEmail> GetPaged(int pageIndex, int pageSize,string connectionString, string providerName) 
        {
            return GetRepo(connectionString,providerName).GetPaged(pageIndex, pageSize);
        }

        public static PagedList<CustomerEmail> GetPaged(int pageIndex, int pageSize) 
        {
            return GetRepo().GetPaged(pageIndex, pageSize);
        }

        public string KeyName()
        {
            return "ReportEmailID";
        }

        public object KeyValue()
        {
            return this.ReportEmailID;
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
//			if( null != this.EmailAddress )
//			{
//				rc = this.EmailAddress.ToString();
//			}
//            return rc;
//        }

        public override bool Equals(object obj)
        {
            if(obj == null)
                return false;
            if(obj is CustomerEmail)
            {
                CustomerEmail compare=(CustomerEmail)obj;
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
            return this.ReportEmailID.GetHashCode( );
        }

        public string DescriptorValue()
        {
            return this.EmailAddress.ToString();
        }

        public string DescriptorColumn() 
        {
            return "EmailAddress";
        }

        public static string GetKeyColumn()
        {
            return "ReportEmailID";
        }        

        public static string GetDescriptorColumn()
        {
            return "EmailAddress";
        }
        
        #region ' Foreign Keys '
        public IQueryable<CustomerProperty> CustomerPropertiesItem
        {
            get
            {
                  var repo=Jaxis.POS.CustomerData.CustomerProperty.GetRepo();
                  return from items in repo.GetAll()
                       where items.CustomerID == m_Internal.CustomerID
                       select items;
            }
        }
        #endregion


		[ObfuscationAttribute(Exclude=true)]
        public virtual Guid ObjectID 
        {
            get
            {
                return m_Internal.ReportEmailID;
            }
            set
            {
                m_Internal.ReportEmailID = value;
            }
        }


//        Guid _ReportEmailID;
        [SubSonicPrimaryKey]
        [LocalData]
        public Guid ReportEmailID
        {
            get { return m_Internal.ReportEmailID; }
            set
            {
                if(m_Internal.ReportEmailID!=value){
                    m_Internal.ReportEmailID=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="ReportEmailID");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        Guid _CustomerID;
        [LocalData]
        public Guid CustomerID
        {
            get { return m_Internal.CustomerID; }
            set
            {
                if(m_Internal.CustomerID!=value){
                    m_Internal.CustomerID=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="CustomerID");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        string _EmailAddress;
        [LocalData]
        public string EmailAddress
        {
            get { return m_Internal.EmailAddress; }
            set
            {
                if(m_Internal.EmailAddress!=value){
                    m_Internal.EmailAddress=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="EmailAddress");
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

        public static void Delete(Expression<Func<CustomerEmail, bool>> expression) 
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
    public partial class DataCustomerProperty : IWCFDataElement
    {
        [DataMember]
        public Guid CustomerID { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public int? SubscriptionType { get; set; }
        [DataMember]
        public string UploadPath { get; set; }
        [DataMember]
        public string ScriptPath { get; set; }
        [DataMember]
        public string ConnectionString { get; set; }
        [DataMember]
        public string DataFileConfig { get; set; }
        [DataMember]
        public string JournalParser { get; set; }

        public void Copy( DataCustomerProperty _Item )
        {
             Name = _Item.Name;			
             SubscriptionType = _Item.SubscriptionType;			
             UploadPath = _Item.UploadPath;			
             ScriptPath = _Item.ScriptPath;			
             ConnectionString = _Item.ConnectionString;			
             DataFileConfig = _Item.DataFileConfig;			
             JournalParser = _Item.JournalParser;			
        }

		public IBaseDataObject CreateDBObject( )
        {
            return new CustomerProperty( this );
        }


    }


    /// <summary>
    /// A class which represents the CustomerProperties table in the CustomerData Database.
    /// </summary>
    public partial class CustomerProperty: BaseDataObject<DataCustomerProperty>, IActiveRecord, ICallOnCreated
    {
        #region Built-in testing
        [NonSerialized]
        static TestRepository<CustomerProperty> _testRepo;
       
        static void SetTestRepo()
        {
            _testRepo = _testRepo ?? new TestRepository<CustomerProperty>(new Jaxis.POS.CustomerData.CustomerDataDB());
        }

        public static void ResetTestRepo()
        {
            _testRepo = null;
            SetTestRepo();
        }

        public static void Setup(List<CustomerProperty> testlist)
        {
            SetTestRepo();
            foreach (var item in testlist)
            {
                _testRepo._items.Add(item);
            }
        }

        public static void Setup(CustomerProperty item) 
        {
            SetTestRepo();
            _testRepo._items.Add(item);
        }

        public static void Setup(int testItems) 
        {
            SetTestRepo();
            for(int i=0;i<testItems;i++)
            {
                CustomerProperty item=new CustomerProperty();
                _testRepo._items.Add(item);
            }
        }
        
        public bool TestMode = false;
        #endregion

        
		IRepository<CustomerProperty> _repo;

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
        
        
		Jaxis.POS.CustomerData.CustomerDataDB _db;
        
        public CustomerProperty()
        {
            m_Internal = new DataCustomerProperty();
             _db=new Jaxis.POS.CustomerData.CustomerDataDB();
            Init();   
            this.CustomerID = Guid.NewGuid( );     
        }

        ///<summary>
        ///Set bool to true to assign NewGuid to PrimaryKey (if appropriate) and to call OnCreated
        ///</summary>
        public CustomerProperty( bool _CallOnCreated )
        {
            m_Internal = new DataCustomerProperty();
            _db=new Jaxis.POS.CustomerData.CustomerDataDB();
            Init();  
            CallOnCreated( _CallOnCreated );
        }

        public CustomerProperty(string connectionString, string providerName) 
        {
            m_Internal = new DataCustomerProperty();
            _db=new Jaxis.POS.CustomerData.CustomerDataDB(connectionString, providerName);
            Init();            
            this.CustomerID = Guid.NewGuid( );     
        }

        public CustomerProperty( CustomerProperty _Item )
        {
            m_Internal = new DataCustomerProperty();
            Copy( _Item );
            _db=new Jaxis.POS.CustomerData.CustomerDataDB();
            Init();            
        }

        public CustomerProperty( DataCustomerProperty _Item )
        {
            m_Internal.Copy( _Item );
            _db=new Jaxis.POS.CustomerData.CustomerDataDB();
            Init();            
        }         
         
        public void Copy( CustomerProperty _Item )
        {
            m_Internal.Name = _Item.Name;			
            m_Internal.SubscriptionType = _Item.SubscriptionType;			
            m_Internal.UploadPath = _Item.UploadPath;			
            m_Internal.ScriptPath = _Item.ScriptPath;			
            m_Internal.ConnectionString = _Item.ConnectionString;			
            m_Internal.DataFileConfig = _Item.DataFileConfig;			
            m_Internal.JournalParser = _Item.JournalParser;			
        }         

        public void CallOnCreated( bool _CallOnCreated )
        {
            if( _CallOnCreated )
            {
                m_Internal.CustomerID = Guid.NewGuid( );     
	
                OnCreated( );
            }
        }
         
        void Init()
        {
            TestMode=this._db.DataProvider.ConnectionString.Equals("test", StringComparison.InvariantCultureIgnoreCase);
            _dirtyColumns=new List<IColumn>();
            if(TestMode)
            {
                CustomerProperty.SetTestRepo();
                _repo=_testRepo;
            }
            else
            {
                _repo = new SubSonicRepository<CustomerProperty>(_db);
            }
            tbl=_repo.GetTable();
            SetIsNew(true);
        }

        public CustomerProperty(Expression<Func<CustomerProperty, bool>> expression):this() 
        {
            SetIsLoaded(_repo.Load(this,expression));
        }
        
        internal static IRepository<CustomerProperty> GetRepo(string connectionString, string providerName)
        {
            Jaxis.POS.CustomerData.CustomerDataDB db;

            db = String.IsNullOrEmpty(connectionString) ? new Jaxis.POS.CustomerData.CustomerDataDB() : new Jaxis.POS.CustomerData.CustomerDataDB(connectionString, providerName);

            /*
            if(String.IsNullOrEmpty(connectionString))
            {
                db=new Jaxis.POS.CustomerData.CustomerDataDB();
            }
            else
            {
                db=new Jaxis.POS.CustomerData.CustomerDataDB(connectionString, providerName);
            }
            */

            IRepository<CustomerProperty> _repo;
            
            if(db.TestMode)
            {
                CustomerProperty.SetTestRepo();
                _repo=_testRepo;
            }
            else
            {
                _repo = new SubSonicRepository<CustomerProperty>(db);
            }
            return _repo;        
        }       
        
        internal static IRepository<CustomerProperty> GetRepo()
        {
            return GetRepo("","");
        }
        
        public static CustomerProperty SingleOrDefault(Expression<Func<CustomerProperty, bool>> expression) 
        {
            return SingleOrDefault( expression, String.Empty, String.Empty );
        }      
        
        public static CustomerProperty SingleOrDefault(Expression<Func<CustomerProperty, bool>> expression,string connectionString, string providerName) 
        {
            IRepository<CustomerProperty> repo = GetRepo(connectionString,providerName);
            CustomerProperty single = repo.SingleOrDefault<CustomerProperty>( expression );
            if( null != single )
            {
                single.OnLoaded( );
            }
            return single;
        }
        
        public static bool Exists(Expression<Func<CustomerProperty, bool>> expression,string connectionString, string providerName) 
        {
            return All(connectionString,providerName).Any(expression);
        }        
        
        public static bool Exists(Expression<Func<CustomerProperty, bool>> expression) 
        {
            return All().Any(expression);
        }        

        protected static bool IsEmptyCustomerPropertyLoaded = false;
        protected static CustomerProperty EmptyCustomerPropertyMember = null;

        public static CustomerProperty GetByID(Guid? value) 
        {
            CustomerProperty rc = null;
            if( value.HasValue )
            {
                rc = GetByID( value.Value );
            }
            return rc;
        }
        
        public static CustomerProperty GetByID(Guid value) 
        {
            CustomerProperty rc = null;
            if( null != value )
            {
                if( value == Guid.Empty )
                {
                    if( true == IsEmptyCustomerPropertyLoaded )
                    {
                        rc = EmptyCustomerPropertyMember;
                    }
                    else
                    {
                        IsEmptyCustomerPropertyLoaded = true;
                        rc = CustomerProperty.Find( L => L.CustomerID.Equals( value ) ).FirstOrDefault( );
                        EmptyCustomerPropertyMember = rc;
                    }
                }
                else
                {
                    rc = CustomerProperty.Find( L => L.CustomerID.Equals( value ) ).FirstOrDefault( );
                } 
            }
            return rc;
        }

        public static IList<CustomerProperty> Find(Expression<Func<CustomerProperty, bool>> expression) 
        {
            var repo = GetRepo();
            return repo.Find(expression).ToList();
        }
        
        public static IList<CustomerProperty> Find(Expression<Func<CustomerProperty, bool>> expression,string connectionString, string providerName) 
        {
            var repo = GetRepo(connectionString,providerName);
            return repo.Find(expression).ToList();
        }

        public static IQueryable<CustomerProperty> All(string connectionString, string providerName) 
        {
            return GetRepo(connectionString,providerName).GetAll();
        }

        public static IQueryable<CustomerProperty> All() 
        {
            return GetRepo().GetAll();
        }
        
        public static PagedList<CustomerProperty> GetPaged(string sortBy, int pageIndex, int pageSize,string connectionString, string providerName) 
        {
            return GetRepo(connectionString,providerName).GetPaged(sortBy, pageIndex, pageSize);
        }
      
        public static PagedList<CustomerProperty> GetPaged(string sortBy, int pageIndex, int pageSize) 
        {
            return GetRepo().GetPaged(sortBy, pageIndex, pageSize);
        }

        public static PagedList<CustomerProperty> GetPaged(int pageIndex, int pageSize,string connectionString, string providerName) 
        {
            return GetRepo(connectionString,providerName).GetPaged(pageIndex, pageSize);
        }

        public static PagedList<CustomerProperty> GetPaged(int pageIndex, int pageSize) 
        {
            return GetRepo().GetPaged(pageIndex, pageSize);
        }

        public string KeyName()
        {
            return "CustomerID";
        }

        public object KeyValue()
        {
            return this.CustomerID;
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
            if(obj is CustomerProperty)
            {
                CustomerProperty compare=(CustomerProperty)obj;
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
            return this.CustomerID.GetHashCode( );
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
            return "CustomerID";
        }        

        public static string GetDescriptorColumn()
        {
            return "Name";
        }
        
        #region ' Foreign Keys '
        public IQueryable<CustomerEmail> CustomerEmails
        {
            get
            {
                  var repo=Jaxis.POS.CustomerData.CustomerEmail.GetRepo();
                  return from items in repo.GetAll()
                       where items.CustomerID == m_Internal.CustomerID
                       select items;
            }
        }
        public IQueryable<ReportCustomerEmail> ReportCustomerEmails
        {
            get
            {
                  var repo=Jaxis.POS.CustomerData.ReportCustomerEmail.GetRepo();
                  return from items in repo.GetAll()
                       where items.CustomerID == m_Internal.CustomerID
                       select items;
            }
        }
        public IQueryable<UserProperty> UserProperties
        {
            get
            {
                  var repo=Jaxis.POS.CustomerData.UserProperty.GetRepo();
                  return from items in repo.GetAll()
                       where items.CustomerID == m_Internal.CustomerID
                       select items;
            }
        }
        #endregion


		[ObfuscationAttribute(Exclude=true)]
        public virtual Guid ObjectID 
        {
            get
            {
                return m_Internal.CustomerID;
            }
            set
            {
                m_Internal.CustomerID = value;
            }
        }


//        Guid _CustomerID;
        [SubSonicPrimaryKey]
        [LocalData]
        public Guid CustomerID
        {
            get { return m_Internal.CustomerID; }
            set
            {
                if(m_Internal.CustomerID!=value){
                    m_Internal.CustomerID=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="CustomerID");
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

//        int? _SubscriptionType;
        [LocalData]
        public int? SubscriptionType
        {
            get { return m_Internal.SubscriptionType; }
            set
            {
                if(m_Internal.SubscriptionType!=value){
                    m_Internal.SubscriptionType=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="SubscriptionType");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        string _UploadPath;
        [LocalData]
        public string UploadPath
        {
            get { return m_Internal.UploadPath; }
            set
            {
                if(m_Internal.UploadPath!=value){
                    m_Internal.UploadPath=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="UploadPath");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        string _ScriptPath;
        [LocalData]
        public string ScriptPath
        {
            get { return m_Internal.ScriptPath; }
            set
            {
                if(m_Internal.ScriptPath!=value){
                    m_Internal.ScriptPath=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="ScriptPath");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        string _ConnectionString;
        [LocalData]
        public string ConnectionString
        {
            get { return m_Internal.ConnectionString; }
            set
            {
                if(m_Internal.ConnectionString!=value){
                    m_Internal.ConnectionString=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="ConnectionString");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        string _DataFileConfig;
        [LocalData]
        public string DataFileConfig
        {
            get { return m_Internal.DataFileConfig; }
            set
            {
                if(m_Internal.DataFileConfig!=value){
                    m_Internal.DataFileConfig=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="DataFileConfig");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        string _JournalParser;
        [LocalData]
        public string JournalParser
        {
            get { return m_Internal.JournalParser; }
            set
            {
                if(m_Internal.JournalParser!=value){
                    m_Internal.JournalParser=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="JournalParser");
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

        public static void Delete(Expression<Func<CustomerProperty, bool>> expression) 
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
    public partial class DataReportCustomerEmail : IWCFDataElement
    {
        [DataMember]
        public Guid ReportCustomerEmailID { get; set; }
        [DataMember]
        public Guid ReportID { get; set; }
        [DataMember]
        public Guid CustomerID { get; set; }
        [DataMember]
        public Guid ReportEmailID { get; set; }

        public void Copy( DataReportCustomerEmail _Item )
        {
             ReportID = _Item.ReportID;			
             CustomerID = _Item.CustomerID;			
             ReportEmailID = _Item.ReportEmailID;			
        }

		public IBaseDataObject CreateDBObject( )
        {
            return new ReportCustomerEmail( this );
        }


    }


    /// <summary>
    /// A class which represents the ReportCustomerEmails table in the CustomerData Database.
    /// </summary>
    public partial class ReportCustomerEmail: BaseDataObject<DataReportCustomerEmail>, IActiveRecord, ICallOnCreated
    {
        #region Built-in testing
        [NonSerialized]
        static TestRepository<ReportCustomerEmail> _testRepo;
       
        static void SetTestRepo()
        {
            _testRepo = _testRepo ?? new TestRepository<ReportCustomerEmail>(new Jaxis.POS.CustomerData.CustomerDataDB());
        }

        public static void ResetTestRepo()
        {
            _testRepo = null;
            SetTestRepo();
        }

        public static void Setup(List<ReportCustomerEmail> testlist)
        {
            SetTestRepo();
            foreach (var item in testlist)
            {
                _testRepo._items.Add(item);
            }
        }

        public static void Setup(ReportCustomerEmail item) 
        {
            SetTestRepo();
            _testRepo._items.Add(item);
        }

        public static void Setup(int testItems) 
        {
            SetTestRepo();
            for(int i=0;i<testItems;i++)
            {
                ReportCustomerEmail item=new ReportCustomerEmail();
                _testRepo._items.Add(item);
            }
        }
        
        public bool TestMode = false;
        #endregion

        
		IRepository<ReportCustomerEmail> _repo;

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
        
        
		Jaxis.POS.CustomerData.CustomerDataDB _db;
        
        public ReportCustomerEmail()
        {
            m_Internal = new DataReportCustomerEmail();
             _db=new Jaxis.POS.CustomerData.CustomerDataDB();
            Init();   
            this.ReportCustomerEmailID = Guid.NewGuid( );     
        }

        ///<summary>
        ///Set bool to true to assign NewGuid to PrimaryKey (if appropriate) and to call OnCreated
        ///</summary>
        public ReportCustomerEmail( bool _CallOnCreated )
        {
            m_Internal = new DataReportCustomerEmail();
            _db=new Jaxis.POS.CustomerData.CustomerDataDB();
            Init();  
            CallOnCreated( _CallOnCreated );
        }

        public ReportCustomerEmail(string connectionString, string providerName) 
        {
            m_Internal = new DataReportCustomerEmail();
            _db=new Jaxis.POS.CustomerData.CustomerDataDB(connectionString, providerName);
            Init();            
            this.ReportCustomerEmailID = Guid.NewGuid( );     
        }

        public ReportCustomerEmail( ReportCustomerEmail _Item )
        {
            m_Internal = new DataReportCustomerEmail();
            Copy( _Item );
            _db=new Jaxis.POS.CustomerData.CustomerDataDB();
            Init();            
        }

        public ReportCustomerEmail( DataReportCustomerEmail _Item )
        {
            m_Internal.Copy( _Item );
            _db=new Jaxis.POS.CustomerData.CustomerDataDB();
            Init();            
        }         
         
        public void Copy( ReportCustomerEmail _Item )
        {
            m_Internal.ReportID = _Item.ReportID;			
            m_Internal.CustomerID = _Item.CustomerID;			
            m_Internal.ReportEmailID = _Item.ReportEmailID;			
        }         

        public void CallOnCreated( bool _CallOnCreated )
        {
            if( _CallOnCreated )
            {
                m_Internal.ReportCustomerEmailID = Guid.NewGuid( );     
	
                OnCreated( );
            }
        }
         
        void Init()
        {
            TestMode=this._db.DataProvider.ConnectionString.Equals("test", StringComparison.InvariantCultureIgnoreCase);
            _dirtyColumns=new List<IColumn>();
            if(TestMode)
            {
                ReportCustomerEmail.SetTestRepo();
                _repo=_testRepo;
            }
            else
            {
                _repo = new SubSonicRepository<ReportCustomerEmail>(_db);
            }
            tbl=_repo.GetTable();
            SetIsNew(true);
        }

        public ReportCustomerEmail(Expression<Func<ReportCustomerEmail, bool>> expression):this() 
        {
            SetIsLoaded(_repo.Load(this,expression));
        }
        
        internal static IRepository<ReportCustomerEmail> GetRepo(string connectionString, string providerName)
        {
            Jaxis.POS.CustomerData.CustomerDataDB db;

            db = String.IsNullOrEmpty(connectionString) ? new Jaxis.POS.CustomerData.CustomerDataDB() : new Jaxis.POS.CustomerData.CustomerDataDB(connectionString, providerName);

            /*
            if(String.IsNullOrEmpty(connectionString))
            {
                db=new Jaxis.POS.CustomerData.CustomerDataDB();
            }
            else
            {
                db=new Jaxis.POS.CustomerData.CustomerDataDB(connectionString, providerName);
            }
            */

            IRepository<ReportCustomerEmail> _repo;
            
            if(db.TestMode)
            {
                ReportCustomerEmail.SetTestRepo();
                _repo=_testRepo;
            }
            else
            {
                _repo = new SubSonicRepository<ReportCustomerEmail>(db);
            }
            return _repo;        
        }       
        
        internal static IRepository<ReportCustomerEmail> GetRepo()
        {
            return GetRepo("","");
        }
        
        public static ReportCustomerEmail SingleOrDefault(Expression<Func<ReportCustomerEmail, bool>> expression) 
        {
            return SingleOrDefault( expression, String.Empty, String.Empty );
        }      
        
        public static ReportCustomerEmail SingleOrDefault(Expression<Func<ReportCustomerEmail, bool>> expression,string connectionString, string providerName) 
        {
            IRepository<ReportCustomerEmail> repo = GetRepo(connectionString,providerName);
            ReportCustomerEmail single = repo.SingleOrDefault<ReportCustomerEmail>( expression );
            if( null != single )
            {
                single.OnLoaded( );
            }
            return single;
        }
        
        public static bool Exists(Expression<Func<ReportCustomerEmail, bool>> expression,string connectionString, string providerName) 
        {
            return All(connectionString,providerName).Any(expression);
        }        
        
        public static bool Exists(Expression<Func<ReportCustomerEmail, bool>> expression) 
        {
            return All().Any(expression);
        }        

        protected static bool IsEmptyReportCustomerEmailLoaded = false;
        protected static ReportCustomerEmail EmptyReportCustomerEmailMember = null;

        public static ReportCustomerEmail GetByID(Guid? value) 
        {
            ReportCustomerEmail rc = null;
            if( value.HasValue )
            {
                rc = GetByID( value.Value );
            }
            return rc;
        }
        
        public static ReportCustomerEmail GetByID(Guid value) 
        {
            ReportCustomerEmail rc = null;
            if( null != value )
            {
                if( value == Guid.Empty )
                {
                    if( true == IsEmptyReportCustomerEmailLoaded )
                    {
                        rc = EmptyReportCustomerEmailMember;
                    }
                    else
                    {
                        IsEmptyReportCustomerEmailLoaded = true;
                        rc = ReportCustomerEmail.Find( L => L.ReportCustomerEmailID.Equals( value ) ).FirstOrDefault( );
                        EmptyReportCustomerEmailMember = rc;
                    }
                }
                else
                {
                    rc = ReportCustomerEmail.Find( L => L.ReportCustomerEmailID.Equals( value ) ).FirstOrDefault( );
                } 
            }
            return rc;
        }

        public static IList<ReportCustomerEmail> Find(Expression<Func<ReportCustomerEmail, bool>> expression) 
        {
            var repo = GetRepo();
            return repo.Find(expression).ToList();
        }
        
        public static IList<ReportCustomerEmail> Find(Expression<Func<ReportCustomerEmail, bool>> expression,string connectionString, string providerName) 
        {
            var repo = GetRepo(connectionString,providerName);
            return repo.Find(expression).ToList();
        }

        public static IQueryable<ReportCustomerEmail> All(string connectionString, string providerName) 
        {
            return GetRepo(connectionString,providerName).GetAll();
        }

        public static IQueryable<ReportCustomerEmail> All() 
        {
            return GetRepo().GetAll();
        }
        
        public static PagedList<ReportCustomerEmail> GetPaged(string sortBy, int pageIndex, int pageSize,string connectionString, string providerName) 
        {
            return GetRepo(connectionString,providerName).GetPaged(sortBy, pageIndex, pageSize);
        }
      
        public static PagedList<ReportCustomerEmail> GetPaged(string sortBy, int pageIndex, int pageSize) 
        {
            return GetRepo().GetPaged(sortBy, pageIndex, pageSize);
        }

        public static PagedList<ReportCustomerEmail> GetPaged(int pageIndex, int pageSize,string connectionString, string providerName) 
        {
            return GetRepo(connectionString,providerName).GetPaged(pageIndex, pageSize);
        }

        public static PagedList<ReportCustomerEmail> GetPaged(int pageIndex, int pageSize) 
        {
            return GetRepo().GetPaged(pageIndex, pageSize);
        }

        public string KeyName()
        {
            return "ReportCustomerEmailID";
        }

        public object KeyValue()
        {
            return this.ReportCustomerEmailID;
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
//			if( null != this.ReportID )
//			{
//				rc = this.ReportID.ToString();
//			}
//            return rc;
//        }

        public override bool Equals(object obj)
        {
            if(obj == null)
                return false;
            if(obj is ReportCustomerEmail)
            {
                ReportCustomerEmail compare=(ReportCustomerEmail)obj;
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
            return this.ReportCustomerEmailID.GetHashCode( );
        }

        public string DescriptorValue()
        {
            return this.ReportID.ToString();
        }

        public string DescriptorColumn() 
        {
            return "ReportID";
        }

        public static string GetKeyColumn()
        {
            return "ReportCustomerEmailID";
        }        

        public static string GetDescriptorColumn()
        {
            return "ReportID";
        }
        
        #region ' Foreign Keys '
        public IQueryable<CustomerProperty> CustomerPropertiesItem
        {
            get
            {
                  var repo=Jaxis.POS.CustomerData.CustomerProperty.GetRepo();
                  return from items in repo.GetAll()
                       where items.CustomerID == m_Internal.CustomerID
                       select items;
            }
        }
        public IQueryable<Report> ReportsItem
        {
            get
            {
                  var repo=Jaxis.POS.CustomerData.Report.GetRepo();
                  return from items in repo.GetAll()
                       where items.ReportID == m_Internal.ReportID
                       select items;
            }
        }
        #endregion


		[ObfuscationAttribute(Exclude=true)]
        public virtual Guid ObjectID 
        {
            get
            {
                return m_Internal.ReportCustomerEmailID;
            }
            set
            {
                m_Internal.ReportCustomerEmailID = value;
            }
        }


//        Guid _ReportCustomerEmailID;
        [SubSonicPrimaryKey]
        [LocalData]
        public Guid ReportCustomerEmailID
        {
            get { return m_Internal.ReportCustomerEmailID; }
            set
            {
                if(m_Internal.ReportCustomerEmailID!=value){
                    m_Internal.ReportCustomerEmailID=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="ReportCustomerEmailID");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        Guid _ReportID;
        [LocalData]
        public Guid ReportID
        {
            get { return m_Internal.ReportID; }
            set
            {
                if(m_Internal.ReportID!=value){
                    m_Internal.ReportID=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="ReportID");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        Guid _CustomerID;
        [LocalData]
        public Guid CustomerID
        {
            get { return m_Internal.CustomerID; }
            set
            {
                if(m_Internal.CustomerID!=value){
                    m_Internal.CustomerID=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="CustomerID");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        Guid _ReportEmailID;
        [LocalData]
        public Guid ReportEmailID
        {
            get { return m_Internal.ReportEmailID; }
            set
            {
                if(m_Internal.ReportEmailID!=value){
                    m_Internal.ReportEmailID=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="ReportEmailID");
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

        public static void Delete(Expression<Func<ReportCustomerEmail, bool>> expression) 
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
    public partial class DataReport : IWCFDataElement
    {
        [DataMember]
        public Guid ReportID { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Description { get; set; }

        public void Copy( DataReport _Item )
        {
             Name = _Item.Name;			
             Description = _Item.Description;			
        }

		public IBaseDataObject CreateDBObject( )
        {
            return new Report( this );
        }


    }


    /// <summary>
    /// A class which represents the Reports table in the CustomerData Database.
    /// </summary>
    public partial class Report: BaseDataObject<DataReport>, IActiveRecord, ICallOnCreated
    {
        #region Built-in testing
        [NonSerialized]
        static TestRepository<Report> _testRepo;
       
        static void SetTestRepo()
        {
            _testRepo = _testRepo ?? new TestRepository<Report>(new Jaxis.POS.CustomerData.CustomerDataDB());
        }

        public static void ResetTestRepo()
        {
            _testRepo = null;
            SetTestRepo();
        }

        public static void Setup(List<Report> testlist)
        {
            SetTestRepo();
            foreach (var item in testlist)
            {
                _testRepo._items.Add(item);
            }
        }

        public static void Setup(Report item) 
        {
            SetTestRepo();
            _testRepo._items.Add(item);
        }

        public static void Setup(int testItems) 
        {
            SetTestRepo();
            for(int i=0;i<testItems;i++)
            {
                Report item=new Report();
                _testRepo._items.Add(item);
            }
        }
        
        public bool TestMode = false;
        #endregion

        
		IRepository<Report> _repo;

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
        
        
		Jaxis.POS.CustomerData.CustomerDataDB _db;
        
        public Report()
        {
            m_Internal = new DataReport();
             _db=new Jaxis.POS.CustomerData.CustomerDataDB();
            Init();   
            this.ReportID = Guid.NewGuid( );     
        }

        ///<summary>
        ///Set bool to true to assign NewGuid to PrimaryKey (if appropriate) and to call OnCreated
        ///</summary>
        public Report( bool _CallOnCreated )
        {
            m_Internal = new DataReport();
            _db=new Jaxis.POS.CustomerData.CustomerDataDB();
            Init();  
            CallOnCreated( _CallOnCreated );
        }

        public Report(string connectionString, string providerName) 
        {
            m_Internal = new DataReport();
            _db=new Jaxis.POS.CustomerData.CustomerDataDB(connectionString, providerName);
            Init();            
            this.ReportID = Guid.NewGuid( );     
        }

        public Report( Report _Item )
        {
            m_Internal = new DataReport();
            Copy( _Item );
            _db=new Jaxis.POS.CustomerData.CustomerDataDB();
            Init();            
        }

        public Report( DataReport _Item )
        {
            m_Internal.Copy( _Item );
            _db=new Jaxis.POS.CustomerData.CustomerDataDB();
            Init();            
        }         
         
        public void Copy( Report _Item )
        {
            m_Internal.Name = _Item.Name;			
            m_Internal.Description = _Item.Description;			
        }         

        public void CallOnCreated( bool _CallOnCreated )
        {
            if( _CallOnCreated )
            {
                m_Internal.ReportID = Guid.NewGuid( );     
	
                OnCreated( );
            }
        }
         
        void Init()
        {
            TestMode=this._db.DataProvider.ConnectionString.Equals("test", StringComparison.InvariantCultureIgnoreCase);
            _dirtyColumns=new List<IColumn>();
            if(TestMode)
            {
                Report.SetTestRepo();
                _repo=_testRepo;
            }
            else
            {
                _repo = new SubSonicRepository<Report>(_db);
            }
            tbl=_repo.GetTable();
            SetIsNew(true);
        }

        public Report(Expression<Func<Report, bool>> expression):this() 
        {
            SetIsLoaded(_repo.Load(this,expression));
        }
        
        internal static IRepository<Report> GetRepo(string connectionString, string providerName)
        {
            Jaxis.POS.CustomerData.CustomerDataDB db;

            db = String.IsNullOrEmpty(connectionString) ? new Jaxis.POS.CustomerData.CustomerDataDB() : new Jaxis.POS.CustomerData.CustomerDataDB(connectionString, providerName);

            /*
            if(String.IsNullOrEmpty(connectionString))
            {
                db=new Jaxis.POS.CustomerData.CustomerDataDB();
            }
            else
            {
                db=new Jaxis.POS.CustomerData.CustomerDataDB(connectionString, providerName);
            }
            */

            IRepository<Report> _repo;
            
            if(db.TestMode)
            {
                Report.SetTestRepo();
                _repo=_testRepo;
            }
            else
            {
                _repo = new SubSonicRepository<Report>(db);
            }
            return _repo;        
        }       
        
        internal static IRepository<Report> GetRepo()
        {
            return GetRepo("","");
        }
        
        public static Report SingleOrDefault(Expression<Func<Report, bool>> expression) 
        {
            return SingleOrDefault( expression, String.Empty, String.Empty );
        }      
        
        public static Report SingleOrDefault(Expression<Func<Report, bool>> expression,string connectionString, string providerName) 
        {
            IRepository<Report> repo = GetRepo(connectionString,providerName);
            Report single = repo.SingleOrDefault<Report>( expression );
            if( null != single )
            {
                single.OnLoaded( );
            }
            return single;
        }
        
        public static bool Exists(Expression<Func<Report, bool>> expression,string connectionString, string providerName) 
        {
            return All(connectionString,providerName).Any(expression);
        }        
        
        public static bool Exists(Expression<Func<Report, bool>> expression) 
        {
            return All().Any(expression);
        }        

        protected static bool IsEmptyReportLoaded = false;
        protected static Report EmptyReportMember = null;

        public static Report GetByID(Guid? value) 
        {
            Report rc = null;
            if( value.HasValue )
            {
                rc = GetByID( value.Value );
            }
            return rc;
        }
        
        public static Report GetByID(Guid value) 
        {
            Report rc = null;
            if( null != value )
            {
                if( value == Guid.Empty )
                {
                    if( true == IsEmptyReportLoaded )
                    {
                        rc = EmptyReportMember;
                    }
                    else
                    {
                        IsEmptyReportLoaded = true;
                        rc = Report.Find( L => L.ReportID.Equals( value ) ).FirstOrDefault( );
                        EmptyReportMember = rc;
                    }
                }
                else
                {
                    rc = Report.Find( L => L.ReportID.Equals( value ) ).FirstOrDefault( );
                } 
            }
            return rc;
        }

        public static IList<Report> Find(Expression<Func<Report, bool>> expression) 
        {
            var repo = GetRepo();
            return repo.Find(expression).ToList();
        }
        
        public static IList<Report> Find(Expression<Func<Report, bool>> expression,string connectionString, string providerName) 
        {
            var repo = GetRepo(connectionString,providerName);
            return repo.Find(expression).ToList();
        }

        public static IQueryable<Report> All(string connectionString, string providerName) 
        {
            return GetRepo(connectionString,providerName).GetAll();
        }

        public static IQueryable<Report> All() 
        {
            return GetRepo().GetAll();
        }
        
        public static PagedList<Report> GetPaged(string sortBy, int pageIndex, int pageSize,string connectionString, string providerName) 
        {
            return GetRepo(connectionString,providerName).GetPaged(sortBy, pageIndex, pageSize);
        }
      
        public static PagedList<Report> GetPaged(string sortBy, int pageIndex, int pageSize) 
        {
            return GetRepo().GetPaged(sortBy, pageIndex, pageSize);
        }

        public static PagedList<Report> GetPaged(int pageIndex, int pageSize,string connectionString, string providerName) 
        {
            return GetRepo(connectionString,providerName).GetPaged(pageIndex, pageSize);
        }

        public static PagedList<Report> GetPaged(int pageIndex, int pageSize) 
        {
            return GetRepo().GetPaged(pageIndex, pageSize);
        }

        public string KeyName()
        {
            return "ReportID";
        }

        public object KeyValue()
        {
            return this.ReportID;
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
            if(obj is Report)
            {
                Report compare=(Report)obj;
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
            return this.ReportID.GetHashCode( );
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
            return "ReportID";
        }        

        public static string GetDescriptorColumn()
        {
            return "Name";
        }
        
        #region ' Foreign Keys '
        public IQueryable<ReportCustomerEmail> ReportCustomerEmails
        {
            get
            {
                  var repo=Jaxis.POS.CustomerData.ReportCustomerEmail.GetRepo();
                  return from items in repo.GetAll()
                       where items.ReportID == m_Internal.ReportID
                       select items;
            }
        }
        #endregion


		[ObfuscationAttribute(Exclude=true)]
        public virtual Guid ObjectID 
        {
            get
            {
                return m_Internal.ReportID;
            }
            set
            {
                m_Internal.ReportID = value;
            }
        }


//        Guid _ReportID;
        [SubSonicPrimaryKey]
        [LocalData]
        public Guid ReportID
        {
            get { return m_Internal.ReportID; }
            set
            {
                if(m_Internal.ReportID!=value){
                    m_Internal.ReportID=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="ReportID");
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

        public static void Delete(Expression<Func<Report, bool>> expression) 
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
    public partial class DataUserProperty : IWCFDataElement
    {
        [DataMember]
        public Guid UserID { get; set; }
        [DataMember]
        public Guid CustomerID { get; set; }
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public string Password { get; set; }

        public void Copy( DataUserProperty _Item )
        {
             CustomerID = _Item.CustomerID;			
             UserName = _Item.UserName;			
             FirstName = _Item.FirstName;			
             LastName = _Item.LastName;			
             Password = _Item.Password;			
        }

		public IBaseDataObject CreateDBObject( )
        {
            return new UserProperty( this );
        }


    }


    /// <summary>
    /// A class which represents the UserProperties table in the CustomerData Database.
    /// </summary>
    public partial class UserProperty: BaseDataObject<DataUserProperty>, IActiveRecord, ICallOnCreated
    {
        #region Built-in testing
        [NonSerialized]
        static TestRepository<UserProperty> _testRepo;
       
        static void SetTestRepo()
        {
            _testRepo = _testRepo ?? new TestRepository<UserProperty>(new Jaxis.POS.CustomerData.CustomerDataDB());
        }

        public static void ResetTestRepo()
        {
            _testRepo = null;
            SetTestRepo();
        }

        public static void Setup(List<UserProperty> testlist)
        {
            SetTestRepo();
            foreach (var item in testlist)
            {
                _testRepo._items.Add(item);
            }
        }

        public static void Setup(UserProperty item) 
        {
            SetTestRepo();
            _testRepo._items.Add(item);
        }

        public static void Setup(int testItems) 
        {
            SetTestRepo();
            for(int i=0;i<testItems;i++)
            {
                UserProperty item=new UserProperty();
                _testRepo._items.Add(item);
            }
        }
        
        public bool TestMode = false;
        #endregion

        
		IRepository<UserProperty> _repo;

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
        
        
		Jaxis.POS.CustomerData.CustomerDataDB _db;
        
        public UserProperty()
        {
            m_Internal = new DataUserProperty();
             _db=new Jaxis.POS.CustomerData.CustomerDataDB();
            Init();   
            this.UserID = Guid.NewGuid( );     
        }

        ///<summary>
        ///Set bool to true to assign NewGuid to PrimaryKey (if appropriate) and to call OnCreated
        ///</summary>
        public UserProperty( bool _CallOnCreated )
        {
            m_Internal = new DataUserProperty();
            _db=new Jaxis.POS.CustomerData.CustomerDataDB();
            Init();  
            CallOnCreated( _CallOnCreated );
        }

        public UserProperty(string connectionString, string providerName) 
        {
            m_Internal = new DataUserProperty();
            _db=new Jaxis.POS.CustomerData.CustomerDataDB(connectionString, providerName);
            Init();            
            this.UserID = Guid.NewGuid( );     
        }

        public UserProperty( UserProperty _Item )
        {
            m_Internal = new DataUserProperty();
            Copy( _Item );
            _db=new Jaxis.POS.CustomerData.CustomerDataDB();
            Init();            
        }

        public UserProperty( DataUserProperty _Item )
        {
            m_Internal.Copy( _Item );
            _db=new Jaxis.POS.CustomerData.CustomerDataDB();
            Init();            
        }         
         
        public void Copy( UserProperty _Item )
        {
            m_Internal.CustomerID = _Item.CustomerID;			
            m_Internal.UserName = _Item.UserName;			
            m_Internal.FirstName = _Item.FirstName;			
            m_Internal.LastName = _Item.LastName;			
            m_Internal.Password = _Item.Password;			
        }         

        public void CallOnCreated( bool _CallOnCreated )
        {
            if( _CallOnCreated )
            {
                m_Internal.UserID = Guid.NewGuid( );     
	
                OnCreated( );
            }
        }
         
        void Init()
        {
            TestMode=this._db.DataProvider.ConnectionString.Equals("test", StringComparison.InvariantCultureIgnoreCase);
            _dirtyColumns=new List<IColumn>();
            if(TestMode)
            {
                UserProperty.SetTestRepo();
                _repo=_testRepo;
            }
            else
            {
                _repo = new SubSonicRepository<UserProperty>(_db);
            }
            tbl=_repo.GetTable();
            SetIsNew(true);
        }

        public UserProperty(Expression<Func<UserProperty, bool>> expression):this() 
        {
            SetIsLoaded(_repo.Load(this,expression));
        }
        
        internal static IRepository<UserProperty> GetRepo(string connectionString, string providerName)
        {
            Jaxis.POS.CustomerData.CustomerDataDB db;

            db = String.IsNullOrEmpty(connectionString) ? new Jaxis.POS.CustomerData.CustomerDataDB() : new Jaxis.POS.CustomerData.CustomerDataDB(connectionString, providerName);

            /*
            if(String.IsNullOrEmpty(connectionString))
            {
                db=new Jaxis.POS.CustomerData.CustomerDataDB();
            }
            else
            {
                db=new Jaxis.POS.CustomerData.CustomerDataDB(connectionString, providerName);
            }
            */

            IRepository<UserProperty> _repo;
            
            if(db.TestMode)
            {
                UserProperty.SetTestRepo();
                _repo=_testRepo;
            }
            else
            {
                _repo = new SubSonicRepository<UserProperty>(db);
            }
            return _repo;        
        }       
        
        internal static IRepository<UserProperty> GetRepo()
        {
            return GetRepo("","");
        }
        
        public static UserProperty SingleOrDefault(Expression<Func<UserProperty, bool>> expression) 
        {
            return SingleOrDefault( expression, String.Empty, String.Empty );
        }      
        
        public static UserProperty SingleOrDefault(Expression<Func<UserProperty, bool>> expression,string connectionString, string providerName) 
        {
            IRepository<UserProperty> repo = GetRepo(connectionString,providerName);
            UserProperty single = repo.SingleOrDefault<UserProperty>( expression );
            if( null != single )
            {
                single.OnLoaded( );
            }
            return single;
        }
        
        public static bool Exists(Expression<Func<UserProperty, bool>> expression,string connectionString, string providerName) 
        {
            return All(connectionString,providerName).Any(expression);
        }        
        
        public static bool Exists(Expression<Func<UserProperty, bool>> expression) 
        {
            return All().Any(expression);
        }        

        protected static bool IsEmptyUserPropertyLoaded = false;
        protected static UserProperty EmptyUserPropertyMember = null;

        public static UserProperty GetByID(Guid? value) 
        {
            UserProperty rc = null;
            if( value.HasValue )
            {
                rc = GetByID( value.Value );
            }
            return rc;
        }
        
        public static UserProperty GetByID(Guid value) 
        {
            UserProperty rc = null;
            if( null != value )
            {
                if( value == Guid.Empty )
                {
                    if( true == IsEmptyUserPropertyLoaded )
                    {
                        rc = EmptyUserPropertyMember;
                    }
                    else
                    {
                        IsEmptyUserPropertyLoaded = true;
                        rc = UserProperty.Find( L => L.UserID.Equals( value ) ).FirstOrDefault( );
                        EmptyUserPropertyMember = rc;
                    }
                }
                else
                {
                    rc = UserProperty.Find( L => L.UserID.Equals( value ) ).FirstOrDefault( );
                } 
            }
            return rc;
        }

        public static IList<UserProperty> Find(Expression<Func<UserProperty, bool>> expression) 
        {
            var repo = GetRepo();
            return repo.Find(expression).ToList();
        }
        
        public static IList<UserProperty> Find(Expression<Func<UserProperty, bool>> expression,string connectionString, string providerName) 
        {
            var repo = GetRepo(connectionString,providerName);
            return repo.Find(expression).ToList();
        }

        public static IQueryable<UserProperty> All(string connectionString, string providerName) 
        {
            return GetRepo(connectionString,providerName).GetAll();
        }

        public static IQueryable<UserProperty> All() 
        {
            return GetRepo().GetAll();
        }
        
        public static PagedList<UserProperty> GetPaged(string sortBy, int pageIndex, int pageSize,string connectionString, string providerName) 
        {
            return GetRepo(connectionString,providerName).GetPaged(sortBy, pageIndex, pageSize);
        }
      
        public static PagedList<UserProperty> GetPaged(string sortBy, int pageIndex, int pageSize) 
        {
            return GetRepo().GetPaged(sortBy, pageIndex, pageSize);
        }

        public static PagedList<UserProperty> GetPaged(int pageIndex, int pageSize,string connectionString, string providerName) 
        {
            return GetRepo(connectionString,providerName).GetPaged(pageIndex, pageSize);
        }

        public static PagedList<UserProperty> GetPaged(int pageIndex, int pageSize) 
        {
            return GetRepo().GetPaged(pageIndex, pageSize);
        }

        public string KeyName()
        {
            return "UserID";
        }

        public object KeyValue()
        {
            return this.UserID;
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
//			if( null != this.UserName )
//			{
//				rc = this.UserName.ToString();
//			}
//            return rc;
//        }

        public override bool Equals(object obj)
        {
            if(obj == null)
                return false;
            if(obj is UserProperty)
            {
                UserProperty compare=(UserProperty)obj;
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
            return this.UserID.GetHashCode( );
        }

        public string DescriptorValue()
        {
            return this.UserName.ToString();
        }

        public string DescriptorColumn() 
        {
            return "UserName";
        }

        public static string GetKeyColumn()
        {
            return "UserID";
        }        

        public static string GetDescriptorColumn()
        {
            return "UserName";
        }
        
        #region ' Foreign Keys '
        public IQueryable<CustomerProperty> CustomerPropertiesItem
        {
            get
            {
                  var repo=Jaxis.POS.CustomerData.CustomerProperty.GetRepo();
                  return from items in repo.GetAll()
                       where items.CustomerID == m_Internal.CustomerID
                       select items;
            }
        }
        #endregion


		[ObfuscationAttribute(Exclude=true)]
        public virtual Guid ObjectID 
        {
            get
            {
                return m_Internal.UserID;
            }
            set
            {
                m_Internal.UserID = value;
            }
        }


//        Guid _UserID;
        [SubSonicPrimaryKey]
        [LocalData]
        public Guid UserID
        {
            get { return m_Internal.UserID; }
            set
            {
                if(m_Internal.UserID!=value){
                    m_Internal.UserID=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="UserID");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        Guid _CustomerID;
        [LocalData]
        public Guid CustomerID
        {
            get { return m_Internal.CustomerID; }
            set
            {
                if(m_Internal.CustomerID!=value){
                    m_Internal.CustomerID=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="CustomerID");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        string _UserName;
        [LocalData]
        public string UserName
        {
            get { return m_Internal.UserName; }
            set
            {
                if(m_Internal.UserName!=value){
                    m_Internal.UserName=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="UserName");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        string _FirstName;
        [LocalData]
        public string FirstName
        {
            get { return m_Internal.FirstName; }
            set
            {
                if(m_Internal.FirstName!=value){
                    m_Internal.FirstName=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="FirstName");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        string _LastName;
        [LocalData]
        public string LastName
        {
            get { return m_Internal.LastName; }
            set
            {
                if(m_Internal.LastName!=value){
                    m_Internal.LastName=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="LastName");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        string _Password;
        [LocalData]
        public string Password
        {
            get { return m_Internal.Password; }
            set
            {
                if(m_Internal.Password!=value){
                    m_Internal.Password=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="Password");
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

        public static void Delete(Expression<Func<UserProperty, bool>> expression) 
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
    public partial class DataUserSession : IWCFDataElement
    {
        [DataMember]
        public Guid SessionID { get; set; }
        [DataMember]
        public Guid UserID { get; set; }
        [DataMember]
        public DateTime SessionStart { get; set; }
        [DataMember]
        public DateTime SessionUpdate { get; set; }
        [DataMember]
        public DateTime? SessionEnd { get; set; }

        public void Copy( DataUserSession _Item )
        {
             UserID = _Item.UserID;			
             SessionStart = _Item.SessionStart;			
             SessionUpdate = _Item.SessionUpdate;			
             SessionEnd = _Item.SessionEnd;			
        }

		public IBaseDataObject CreateDBObject( )
        {
            return new UserSession( this );
        }


    }


    /// <summary>
    /// A class which represents the UserSessions table in the CustomerData Database.
    /// </summary>
    public partial class UserSession: BaseDataObject<DataUserSession>, IActiveRecord, ICallOnCreated
    {
        #region Built-in testing
        [NonSerialized]
        static TestRepository<UserSession> _testRepo;
       
        static void SetTestRepo()
        {
            _testRepo = _testRepo ?? new TestRepository<UserSession>(new Jaxis.POS.CustomerData.CustomerDataDB());
        }

        public static void ResetTestRepo()
        {
            _testRepo = null;
            SetTestRepo();
        }

        public static void Setup(List<UserSession> testlist)
        {
            SetTestRepo();
            foreach (var item in testlist)
            {
                _testRepo._items.Add(item);
            }
        }

        public static void Setup(UserSession item) 
        {
            SetTestRepo();
            _testRepo._items.Add(item);
        }

        public static void Setup(int testItems) 
        {
            SetTestRepo();
            for(int i=0;i<testItems;i++)
            {
                UserSession item=new UserSession();
                _testRepo._items.Add(item);
            }
        }
        
        public bool TestMode = false;
        #endregion

        
		IRepository<UserSession> _repo;

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
        
        
		Jaxis.POS.CustomerData.CustomerDataDB _db;
        
        public UserSession()
        {
            m_Internal = new DataUserSession();
             _db=new Jaxis.POS.CustomerData.CustomerDataDB();
            Init();   
            this.SessionID = Guid.NewGuid( );     
        }

        ///<summary>
        ///Set bool to true to assign NewGuid to PrimaryKey (if appropriate) and to call OnCreated
        ///</summary>
        public UserSession( bool _CallOnCreated )
        {
            m_Internal = new DataUserSession();
            _db=new Jaxis.POS.CustomerData.CustomerDataDB();
            Init();  
            CallOnCreated( _CallOnCreated );
        }

        public UserSession(string connectionString, string providerName) 
        {
            m_Internal = new DataUserSession();
            _db=new Jaxis.POS.CustomerData.CustomerDataDB(connectionString, providerName);
            Init();            
            this.SessionID = Guid.NewGuid( );     
        }

        public UserSession( UserSession _Item )
        {
            m_Internal = new DataUserSession();
            Copy( _Item );
            _db=new Jaxis.POS.CustomerData.CustomerDataDB();
            Init();            
        }

        public UserSession( DataUserSession _Item )
        {
            m_Internal.Copy( _Item );
            _db=new Jaxis.POS.CustomerData.CustomerDataDB();
            Init();            
        }         
         
        public void Copy( UserSession _Item )
        {
            m_Internal.UserID = _Item.UserID;			
            m_Internal.SessionStart = _Item.SessionStart;			
            m_Internal.SessionUpdate = _Item.SessionUpdate;			
            m_Internal.SessionEnd = _Item.SessionEnd;			
        }         

        public void CallOnCreated( bool _CallOnCreated )
        {
            if( _CallOnCreated )
            {
                m_Internal.SessionID = Guid.NewGuid( );     
	
                OnCreated( );
            }
        }
         
        void Init()
        {
            TestMode=this._db.DataProvider.ConnectionString.Equals("test", StringComparison.InvariantCultureIgnoreCase);
            _dirtyColumns=new List<IColumn>();
            if(TestMode)
            {
                UserSession.SetTestRepo();
                _repo=_testRepo;
            }
            else
            {
                _repo = new SubSonicRepository<UserSession>(_db);
            }
            tbl=_repo.GetTable();
            SetIsNew(true);
        }

        public UserSession(Expression<Func<UserSession, bool>> expression):this() 
        {
            SetIsLoaded(_repo.Load(this,expression));
        }
        
        internal static IRepository<UserSession> GetRepo(string connectionString, string providerName)
        {
            Jaxis.POS.CustomerData.CustomerDataDB db;

            db = String.IsNullOrEmpty(connectionString) ? new Jaxis.POS.CustomerData.CustomerDataDB() : new Jaxis.POS.CustomerData.CustomerDataDB(connectionString, providerName);

            /*
            if(String.IsNullOrEmpty(connectionString))
            {
                db=new Jaxis.POS.CustomerData.CustomerDataDB();
            }
            else
            {
                db=new Jaxis.POS.CustomerData.CustomerDataDB(connectionString, providerName);
            }
            */

            IRepository<UserSession> _repo;
            
            if(db.TestMode)
            {
                UserSession.SetTestRepo();
                _repo=_testRepo;
            }
            else
            {
                _repo = new SubSonicRepository<UserSession>(db);
            }
            return _repo;        
        }       
        
        internal static IRepository<UserSession> GetRepo()
        {
            return GetRepo("","");
        }
        
        public static UserSession SingleOrDefault(Expression<Func<UserSession, bool>> expression) 
        {
            return SingleOrDefault( expression, String.Empty, String.Empty );
        }      
        
        public static UserSession SingleOrDefault(Expression<Func<UserSession, bool>> expression,string connectionString, string providerName) 
        {
            IRepository<UserSession> repo = GetRepo(connectionString,providerName);
            UserSession single = repo.SingleOrDefault<UserSession>( expression );
            if( null != single )
            {
                single.OnLoaded( );
            }
            return single;
        }
        
        public static bool Exists(Expression<Func<UserSession, bool>> expression,string connectionString, string providerName) 
        {
            return All(connectionString,providerName).Any(expression);
        }        
        
        public static bool Exists(Expression<Func<UserSession, bool>> expression) 
        {
            return All().Any(expression);
        }        

        protected static bool IsEmptyUserSessionLoaded = false;
        protected static UserSession EmptyUserSessionMember = null;

        public static UserSession GetByID(Guid? value) 
        {
            UserSession rc = null;
            if( value.HasValue )
            {
                rc = GetByID( value.Value );
            }
            return rc;
        }
        
        public static UserSession GetByID(Guid value) 
        {
            UserSession rc = null;
            if( null != value )
            {
                if( value == Guid.Empty )
                {
                    if( true == IsEmptyUserSessionLoaded )
                    {
                        rc = EmptyUserSessionMember;
                    }
                    else
                    {
                        IsEmptyUserSessionLoaded = true;
                        rc = UserSession.Find( L => L.SessionID.Equals( value ) ).FirstOrDefault( );
                        EmptyUserSessionMember = rc;
                    }
                }
                else
                {
                    rc = UserSession.Find( L => L.SessionID.Equals( value ) ).FirstOrDefault( );
                } 
            }
            return rc;
        }

        public static IList<UserSession> Find(Expression<Func<UserSession, bool>> expression) 
        {
            var repo = GetRepo();
            return repo.Find(expression).ToList();
        }
        
        public static IList<UserSession> Find(Expression<Func<UserSession, bool>> expression,string connectionString, string providerName) 
        {
            var repo = GetRepo(connectionString,providerName);
            return repo.Find(expression).ToList();
        }

        public static IQueryable<UserSession> All(string connectionString, string providerName) 
        {
            return GetRepo(connectionString,providerName).GetAll();
        }

        public static IQueryable<UserSession> All() 
        {
            return GetRepo().GetAll();
        }
        
        public static PagedList<UserSession> GetPaged(string sortBy, int pageIndex, int pageSize,string connectionString, string providerName) 
        {
            return GetRepo(connectionString,providerName).GetPaged(sortBy, pageIndex, pageSize);
        }
      
        public static PagedList<UserSession> GetPaged(string sortBy, int pageIndex, int pageSize) 
        {
            return GetRepo().GetPaged(sortBy, pageIndex, pageSize);
        }

        public static PagedList<UserSession> GetPaged(int pageIndex, int pageSize,string connectionString, string providerName) 
        {
            return GetRepo(connectionString,providerName).GetPaged(pageIndex, pageSize);
        }

        public static PagedList<UserSession> GetPaged(int pageIndex, int pageSize) 
        {
            return GetRepo().GetPaged(pageIndex, pageSize);
        }

        public string KeyName()
        {
            return "SessionID";
        }

        public object KeyValue()
        {
            return this.SessionID;
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
//			if( null != this.UserID )
//			{
//				rc = this.UserID.ToString();
//			}
//            return rc;
//        }

        public override bool Equals(object obj)
        {
            if(obj == null)
                return false;
            if(obj is UserSession)
            {
                UserSession compare=(UserSession)obj;
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
            return this.SessionID.GetHashCode( );
        }

        public string DescriptorValue()
        {
            return this.UserID.ToString();
        }

        public string DescriptorColumn() 
        {
            return "UserID";
        }

        public static string GetKeyColumn()
        {
            return "SessionID";
        }        

        public static string GetDescriptorColumn()
        {
            return "UserID";
        }
        
        #region ' Foreign Keys '
        #endregion


		[ObfuscationAttribute(Exclude=true)]
        public virtual Guid ObjectID 
        {
            get
            {
                return m_Internal.SessionID;
            }
            set
            {
                m_Internal.SessionID = value;
            }
        }


//        Guid _SessionID;
        [SubSonicPrimaryKey]
        [LocalData]
        public Guid SessionID
        {
            get { return m_Internal.SessionID; }
            set
            {
                if(m_Internal.SessionID!=value){
                    m_Internal.SessionID=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="SessionID");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        Guid _UserID;
        [LocalData]
        public Guid UserID
        {
            get { return m_Internal.UserID; }
            set
            {
                if(m_Internal.UserID!=value){
                    m_Internal.UserID=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="UserID");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        DateTime _SessionStart;
        [LocalData]
        public DateTime SessionStart
        {
            get { return m_Internal.SessionStart; }
            set
            {
                if(m_Internal.SessionStart!=value){
                    m_Internal.SessionStart=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="SessionStart");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        DateTime _SessionUpdate;
        [LocalData]
        public DateTime SessionUpdate
        {
            get { return m_Internal.SessionUpdate; }
            set
            {
                if(m_Internal.SessionUpdate!=value){
                    m_Internal.SessionUpdate=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="SessionUpdate");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        DateTime? _SessionEnd;
        [LocalData]
        public DateTime? SessionEnd
        {
            get { return m_Internal.SessionEnd; }
            set
            {
                if(m_Internal.SessionEnd!=value){
                    m_Internal.SessionEnd=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="SessionEnd");
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

        public static void Delete(Expression<Func<UserSession, bool>> expression) 
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
    public partial class DatavwCustomerReportList : IWCFDataElement
    {
        [DataMember]
        public Guid CustomerID { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public Guid ReportID { get; set; }
        [DataMember]
        public string EmailAddress { get; set; }

        public void Copy( DatavwCustomerReportList _Item )
        {
             Name = _Item.Name;			
             ReportID = _Item.ReportID;			
             EmailAddress = _Item.EmailAddress;			
        }

		public IBaseDataObject CreateDBObject( )
        {
            return new vwCustomerReportList( this );
        }


    }


    /// <summary>
    /// A class which represents the vwCustomerReportList table in the CustomerData Database.
    /// </summary>
    public partial class vwCustomerReportList: BaseDataObject<DatavwCustomerReportList>, IActiveRecord, ICallOnCreated
    {
        #region Built-in testing
        [NonSerialized]
        static TestRepository<vwCustomerReportList> _testRepo;
       
        static void SetTestRepo()
        {
            _testRepo = _testRepo ?? new TestRepository<vwCustomerReportList>(new Jaxis.POS.CustomerData.CustomerDataDB());
        }

        public static void ResetTestRepo()
        {
            _testRepo = null;
            SetTestRepo();
        }

        public static void Setup(List<vwCustomerReportList> testlist)
        {
            SetTestRepo();
            foreach (var item in testlist)
            {
                _testRepo._items.Add(item);
            }
        }

        public static void Setup(vwCustomerReportList item) 
        {
            SetTestRepo();
            _testRepo._items.Add(item);
        }

        public static void Setup(int testItems) 
        {
            SetTestRepo();
            for(int i=0;i<testItems;i++)
            {
                vwCustomerReportList item=new vwCustomerReportList();
                _testRepo._items.Add(item);
            }
        }
        
        public bool TestMode = false;
        #endregion

        
		IRepository<vwCustomerReportList> _repo;

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
        
        
		Jaxis.POS.CustomerData.CustomerDataDB _db;
        
        public vwCustomerReportList()
        {
            m_Internal = new DatavwCustomerReportList();
             _db=new Jaxis.POS.CustomerData.CustomerDataDB();
            Init();   
            this.CustomerID = Guid.NewGuid( );     
        }

        ///<summary>
        ///Set bool to true to assign NewGuid to PrimaryKey (if appropriate) and to call OnCreated
        ///</summary>
        public vwCustomerReportList( bool _CallOnCreated )
        {
            m_Internal = new DatavwCustomerReportList();
            _db=new Jaxis.POS.CustomerData.CustomerDataDB();
            Init();  
            CallOnCreated( _CallOnCreated );
        }

        public vwCustomerReportList(string connectionString, string providerName) 
        {
            m_Internal = new DatavwCustomerReportList();
            _db=new Jaxis.POS.CustomerData.CustomerDataDB(connectionString, providerName);
            Init();            
            this.CustomerID = Guid.NewGuid( );     
        }

        public vwCustomerReportList( vwCustomerReportList _Item )
        {
            m_Internal = new DatavwCustomerReportList();
            Copy( _Item );
            _db=new Jaxis.POS.CustomerData.CustomerDataDB();
            Init();            
        }

        public vwCustomerReportList( DatavwCustomerReportList _Item )
        {
            m_Internal.Copy( _Item );
            _db=new Jaxis.POS.CustomerData.CustomerDataDB();
            Init();            
        }         
         
        public void Copy( vwCustomerReportList _Item )
        {
            m_Internal.Name = _Item.Name;			
            m_Internal.ReportID = _Item.ReportID;			
            m_Internal.EmailAddress = _Item.EmailAddress;			
        }         

        public void CallOnCreated( bool _CallOnCreated )
        {
            if( _CallOnCreated )
            {
                m_Internal.CustomerID = Guid.NewGuid( );     
	
                OnCreated( );
            }
        }
         
        void Init()
        {
            TestMode=this._db.DataProvider.ConnectionString.Equals("test", StringComparison.InvariantCultureIgnoreCase);
            _dirtyColumns=new List<IColumn>();
            if(TestMode)
            {
                vwCustomerReportList.SetTestRepo();
                _repo=_testRepo;
            }
            else
            {
                _repo = new SubSonicRepository<vwCustomerReportList>(_db);
            }
            tbl=_repo.GetTable();
            SetIsNew(true);
        }

        public vwCustomerReportList(Expression<Func<vwCustomerReportList, bool>> expression):this() 
        {
            SetIsLoaded(_repo.Load(this,expression));
        }
        
        internal static IRepository<vwCustomerReportList> GetRepo(string connectionString, string providerName)
        {
            Jaxis.POS.CustomerData.CustomerDataDB db;

            db = String.IsNullOrEmpty(connectionString) ? new Jaxis.POS.CustomerData.CustomerDataDB() : new Jaxis.POS.CustomerData.CustomerDataDB(connectionString, providerName);

            /*
            if(String.IsNullOrEmpty(connectionString))
            {
                db=new Jaxis.POS.CustomerData.CustomerDataDB();
            }
            else
            {
                db=new Jaxis.POS.CustomerData.CustomerDataDB(connectionString, providerName);
            }
            */

            IRepository<vwCustomerReportList> _repo;
            
            if(db.TestMode)
            {
                vwCustomerReportList.SetTestRepo();
                _repo=_testRepo;
            }
            else
            {
                _repo = new SubSonicRepository<vwCustomerReportList>(db);
            }
            return _repo;        
        }       
        
        internal static IRepository<vwCustomerReportList> GetRepo()
        {
            return GetRepo("","");
        }
        
        public static vwCustomerReportList SingleOrDefault(Expression<Func<vwCustomerReportList, bool>> expression) 
        {
            return SingleOrDefault( expression, String.Empty, String.Empty );
        }      
        
        public static vwCustomerReportList SingleOrDefault(Expression<Func<vwCustomerReportList, bool>> expression,string connectionString, string providerName) 
        {
            IRepository<vwCustomerReportList> repo = GetRepo(connectionString,providerName);
            vwCustomerReportList single = repo.SingleOrDefault<vwCustomerReportList>( expression );
            if( null != single )
            {
                single.OnLoaded( );
            }
            return single;
        }
        
        public static bool Exists(Expression<Func<vwCustomerReportList, bool>> expression,string connectionString, string providerName) 
        {
            return All(connectionString,providerName).Any(expression);
        }        
        
        public static bool Exists(Expression<Func<vwCustomerReportList, bool>> expression) 
        {
            return All().Any(expression);
        }        

        protected static bool IsEmptyvwCustomerReportListLoaded = false;
        protected static vwCustomerReportList EmptyvwCustomerReportListMember = null;

        public static vwCustomerReportList GetByID(Guid? value) 
        {
            vwCustomerReportList rc = null;
            if( value.HasValue )
            {
                rc = GetByID( value.Value );
            }
            return rc;
        }
        
        public static vwCustomerReportList GetByID(Guid value) 
        {
            vwCustomerReportList rc = null;
            if( null != value )
            {
                if( value == Guid.Empty )
                {
                    if( true == IsEmptyvwCustomerReportListLoaded )
                    {
                        rc = EmptyvwCustomerReportListMember;
                    }
                    else
                    {
                        IsEmptyvwCustomerReportListLoaded = true;
                        rc = vwCustomerReportList.Find( L => L.CustomerID.Equals( value ) ).FirstOrDefault( );
                        EmptyvwCustomerReportListMember = rc;
                    }
                }
                else
                {
                    rc = vwCustomerReportList.Find( L => L.CustomerID.Equals( value ) ).FirstOrDefault( );
                } 
            }
            return rc;
        }

        public static IList<vwCustomerReportList> Find(Expression<Func<vwCustomerReportList, bool>> expression) 
        {
            var repo = GetRepo();
            return repo.Find(expression).ToList();
        }
        
        public static IList<vwCustomerReportList> Find(Expression<Func<vwCustomerReportList, bool>> expression,string connectionString, string providerName) 
        {
            var repo = GetRepo(connectionString,providerName);
            return repo.Find(expression).ToList();
        }

        public static IQueryable<vwCustomerReportList> All(string connectionString, string providerName) 
        {
            return GetRepo(connectionString,providerName).GetAll();
        }

        public static IQueryable<vwCustomerReportList> All() 
        {
            return GetRepo().GetAll();
        }
        
        public static PagedList<vwCustomerReportList> GetPaged(string sortBy, int pageIndex, int pageSize,string connectionString, string providerName) 
        {
            return GetRepo(connectionString,providerName).GetPaged(sortBy, pageIndex, pageSize);
        }
      
        public static PagedList<vwCustomerReportList> GetPaged(string sortBy, int pageIndex, int pageSize) 
        {
            return GetRepo().GetPaged(sortBy, pageIndex, pageSize);
        }

        public static PagedList<vwCustomerReportList> GetPaged(int pageIndex, int pageSize,string connectionString, string providerName) 
        {
            return GetRepo(connectionString,providerName).GetPaged(pageIndex, pageSize);
        }

        public static PagedList<vwCustomerReportList> GetPaged(int pageIndex, int pageSize) 
        {
            return GetRepo().GetPaged(pageIndex, pageSize);
        }

        public string KeyName()
        {
            return "CustomerID";
        }

        public object KeyValue()
        {
            return this.CustomerID;
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
            if(obj is vwCustomerReportList)
            {
                vwCustomerReportList compare=(vwCustomerReportList)obj;
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
            return this.CustomerID.GetHashCode( );
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
            return "CustomerID";
        }        

        public static string GetDescriptorColumn()
        {
            return "Name";
        }
        
        #region ' Foreign Keys '
        #endregion


		[ObfuscationAttribute(Exclude=true)]
        public virtual Guid ObjectID 
        {
            get
            {
                return m_Internal.CustomerID;
            }
            set
            {
                m_Internal.CustomerID = value;
            }
        }


//        Guid _CustomerID;
        [SubSonicPrimaryKey]
        [LocalData]
        public Guid CustomerID
        {
            get { return m_Internal.CustomerID; }
            set
            {
                if(m_Internal.CustomerID!=value){
                    m_Internal.CustomerID=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="CustomerID");
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

//        Guid _ReportID;
        [LocalData]
        public Guid ReportID
        {
            get { return m_Internal.ReportID; }
            set
            {
                if(m_Internal.ReportID!=value){
                    m_Internal.ReportID=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="ReportID");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

//        string _EmailAddress;
        [LocalData]
        public string EmailAddress
        {
            get { return m_Internal.EmailAddress; }
            set
            {
                if(m_Internal.EmailAddress!=value){
                    m_Internal.EmailAddress=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="EmailAddress");
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

        public static void Delete(Expression<Func<vwCustomerReportList, bool>> expression) 
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
