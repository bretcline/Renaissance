


using System;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using SubSonic.DataProviders;
using SubSonic.Extensions;
using SubSonic.Linq.Structure;
using SubSonic.Query;
using SubSonic.Schema;
using System.Data.Common;
using System.Collections.Generic;

namespace Jaxis.POS.CustomerData
{
    public partial class CustomerDataDB : IQuerySurface
    {

        [NonSerialized]
        public IDataProvider DataProvider;
        [NonSerialized]
        public DbQueryProvider provider;
        
        public bool TestMode
		{
            get
			{
                return DataProvider.ConnectionString.Equals("test", StringComparison.InvariantCultureIgnoreCase);
            }
        }

        public CustomerDataDB() 
        { 
            DataProvider = ProviderFactory.GetProvider("CustomerData");
            Init();
        }

        public CustomerDataDB(string connectionStringName)
        {
            DataProvider = ProviderFactory.GetProvider(connectionStringName);
            Init();
        }

		public CustomerDataDB(string connectionString, string providerName)
        {
            DataProvider = ProviderFactory.GetProvider(connectionString,providerName);
            Init();
        }

		public ITable FindByPrimaryKey(string pkName)
        {
            return DataProvider.Schema.Tables.SingleOrDefault(x => x.PrimaryKey.Name.Equals(pkName, StringComparison.InvariantCultureIgnoreCase));
        }

        public Query<T> GetQuery<T>()
        {
            return new Query<T>(provider);
        }
        
        public ITable FindTable(string tableName)
        {
            return DataProvider.FindTable(tableName);
        }
        
        [IgnoreDataMember]
        public IDataProvider Provider
        {
            get { return DataProvider; }
            set {DataProvider=value;}
        }
        
        [IgnoreDataMember]
        public DbQueryProvider QueryProvider
        {
            get { return provider; }
        }
        
        BatchQuery _batch = null;
        public void Queue<T>(IQueryable<T> qry)
        {
            if (_batch == null)
                _batch = new BatchQuery(Provider, QueryProvider);
            _batch.Queue(qry);
        }

        public void Queue(ISqlQuery qry)
        {
            if (_batch == null)
                _batch = new BatchQuery(Provider, QueryProvider);
            _batch.Queue(qry);
        }

        public void ExecuteTransaction(IList<DbCommand> commands)
		{
            if(!TestMode)
			{
                using(var connection = commands[0].Connection)
				{
                   if (connection.State == ConnectionState.Closed)
                        connection.Open();
                   
                   using (var trans = connection.BeginTransaction()) 
				   {
                        foreach (var cmd in commands) 
						{
                            cmd.Transaction = trans;
                            cmd.Connection = connection;
                            cmd.ExecuteNonQuery();
                        }
                        trans.Commit();
                    }
                    connection.Close();
                }
            }
        }

        public IDataReader ExecuteBatch()
        {
            if (_batch == null)
                throw new InvalidOperationException("There's nothing in the queue");
            if(!TestMode)
                return _batch.ExecuteReader();
            return null;
        }
			
        public Query<CustomerEmail> CustomerEmails { get; set; }
        public Query<CustomerProperty> CustomerProperties { get; set; }
        public Query<ReportCustomerEmail> ReportCustomerEmails { get; set; }
        public Query<Report> Reports { get; set; }
        public Query<UserProperty> UserProperties { get; set; }
        public Query<UserSession> UserSessions { get; set; }
        public Query<vwCustomerReportList> vwCustomerReportLists { get; set; }

			

        #region ' Aggregates and SubSonic Queries '
        public Select SelectColumns(params string[] columns)
        {
            return new Select(DataProvider, columns);
        }

        public Select Select
        {
            get { return new Select(this.Provider); }
        }

        public Insert Insert
		{
            get { return new Insert(this.Provider); }
        }

        public Update<T> Update<T>() where T:new()
		{
            return new Update<T>(this.Provider);
        }

        public SqlQuery Delete<T>(Expression<Func<T,bool>> column) where T:new()
        {
            LambdaExpression lamda = column;
            SqlQuery result = new Delete<T>(this.Provider);
            result = result.From<T>();
            result.Constraints=lamda.ParseConstraints().ToList();
            return result;
        }

        public SqlQuery Max<T>(Expression<Func<T,object>> column)
        {
            LambdaExpression lamda = column;
            string colName = lamda.ParseObjectValue();
            string objectName = typeof(T).Name;
            string tableName = DataProvider.FindTable(objectName).Name;
            return new Select(DataProvider, new Aggregate(colName, AggregateFunction.Max)).From(tableName);
        }

        public SqlQuery Min<T>(Expression<Func<T,object>> column)
        {
            LambdaExpression lamda = column;
            string colName = lamda.ParseObjectValue();
            string objectName = typeof(T).Name;
            string tableName = this.Provider.FindTable(objectName).Name;
            return new Select(this.Provider, new Aggregate(colName, AggregateFunction.Min)).From(tableName);
        }

        public SqlQuery Sum<T>(Expression<Func<T,object>> column)
        {
            LambdaExpression lamda = column;
            string colName = lamda.ParseObjectValue();
            string objectName = typeof(T).Name;
            string tableName = this.Provider.FindTable(objectName).Name;
            return new Select(this.Provider, new Aggregate(colName, AggregateFunction.Sum)).From(tableName);
        }

        public SqlQuery Avg<T>(Expression<Func<T,object>> column)
        {
            LambdaExpression lamda = column;
            string colName = lamda.ParseObjectValue();
            string objectName = typeof(T).Name;
            string tableName = this.Provider.FindTable(objectName).Name;
            return new Select(this.Provider, new Aggregate(colName, AggregateFunction.Avg)).From(tableName);
        }

        public SqlQuery Count<T>(Expression<Func<T,object>> column)
        {
            LambdaExpression lamda = column;
            string colName = lamda.ParseObjectValue();
            string objectName = typeof(T).Name;
            string tableName = this.Provider.FindTable(objectName).Name;
            return new Select(this.Provider, new Aggregate(colName, AggregateFunction.Count)).From(tableName);
        }

        public SqlQuery Variance<T>(Expression<Func<T,object>> column)
        {
            LambdaExpression lamda = column;
            string colName = lamda.ParseObjectValue();
            string objectName = typeof(T).Name;
            string tableName = this.Provider.FindTable(objectName).Name;
            return new Select(this.Provider, new Aggregate(colName, AggregateFunction.Var)).From(tableName);
        }

        public SqlQuery StandardDeviation<T>(Expression<Func<T,object>> column)
        {
            LambdaExpression lamda = column;
            string colName = lamda.ParseObjectValue();
            string objectName = typeof(T).Name;
            string tableName = this.Provider.FindTable(objectName).Name;
            return new Select(this.Provider, new Aggregate(colName, AggregateFunction.StDev)).From(tableName);
        }

        #endregion

        void Init()
        {
            provider = new DbQueryProvider(this.Provider);

            #region ' Query Defs '
            CustomerEmails = new Query<CustomerEmail>(provider);
            CustomerProperties = new Query<CustomerProperty>(provider);
            ReportCustomerEmails = new Query<ReportCustomerEmail>(provider);
            Reports = new Query<Report>(provider);
            UserProperties = new Query<UserProperty>(provider);
            UserSessions = new Query<UserSession>(provider);
            vwCustomerReportLists = new Query<vwCustomerReportList>(provider);
            #endregion


            #region ' Schemas '
        	if(DataProvider.Schema.Tables.Count == 0)
			{
				
				// Table: CustomerEmails
				// Primary Key: ReportEmailID
				ITable CustomerEmailsSchema = new DatabaseTable("CustomerEmails", DataProvider) { ClassName = "CustomerEmail", SchemaName = "dbo" };
            	CustomerEmailsSchema.Columns.Add(new DatabaseColumn("ReportEmailID", CustomerEmailsSchema)
												{
													IsPrimaryKey = true,
													DataType = DbType.Guid,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	CustomerEmailsSchema.Columns.Add(new DatabaseColumn("CustomerID", CustomerEmailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Guid,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = true
												});
            	CustomerEmailsSchema.Columns.Add(new DatabaseColumn("EmailAddress", CustomerEmailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	CustomerEmailsSchema.Columns.Add(new DatabaseColumn("Name", CustomerEmailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
				// Add CustomerEmails to schema
            	DataProvider.Schema.Tables.Add(CustomerEmailsSchema);
				
				// Table: CustomerProperties
				// Primary Key: CustomerID
				ITable CustomerPropertiesSchema = new DatabaseTable("CustomerProperties", DataProvider) { ClassName = "CustomerProperty", SchemaName = "dbo" };
            	CustomerPropertiesSchema.Columns.Add(new DatabaseColumn("CustomerID", CustomerPropertiesSchema)
												{
													IsPrimaryKey = true,
													DataType = DbType.Guid,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = true
												});
            	CustomerPropertiesSchema.Columns.Add(new DatabaseColumn("Name", CustomerPropertiesSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	CustomerPropertiesSchema.Columns.Add(new DatabaseColumn("SubscriptionType", CustomerPropertiesSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	CustomerPropertiesSchema.Columns.Add(new DatabaseColumn("UploadPath", CustomerPropertiesSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	CustomerPropertiesSchema.Columns.Add(new DatabaseColumn("ScriptPath", CustomerPropertiesSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	CustomerPropertiesSchema.Columns.Add(new DatabaseColumn("ConnectionString", CustomerPropertiesSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	CustomerPropertiesSchema.Columns.Add(new DatabaseColumn("DataFileConfig", CustomerPropertiesSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	CustomerPropertiesSchema.Columns.Add(new DatabaseColumn("JournalParser", CustomerPropertiesSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
				// Add CustomerProperties to schema
            	DataProvider.Schema.Tables.Add(CustomerPropertiesSchema);
				
				// Table: ReportCustomerEmails
				// Primary Key: ReportCustomerEmailID
				ITable ReportCustomerEmailsSchema = new DatabaseTable("ReportCustomerEmails", DataProvider) { ClassName = "ReportCustomerEmail", SchemaName = "dbo" };
            	ReportCustomerEmailsSchema.Columns.Add(new DatabaseColumn("ReportCustomerEmailID", ReportCustomerEmailsSchema)
												{
													IsPrimaryKey = true,
													DataType = DbType.Guid,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	ReportCustomerEmailsSchema.Columns.Add(new DatabaseColumn("ReportID", ReportCustomerEmailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Guid,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = true
												});
            	ReportCustomerEmailsSchema.Columns.Add(new DatabaseColumn("CustomerID", ReportCustomerEmailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Guid,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = true
												});
            	ReportCustomerEmailsSchema.Columns.Add(new DatabaseColumn("ReportEmailID", ReportCustomerEmailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Guid,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
				// Add ReportCustomerEmails to schema
            	DataProvider.Schema.Tables.Add(ReportCustomerEmailsSchema);
				
				// Table: Reports
				// Primary Key: ReportID
				ITable ReportsSchema = new DatabaseTable("Reports", DataProvider) { ClassName = "Report", SchemaName = "dbo" };
            	ReportsSchema.Columns.Add(new DatabaseColumn("ReportID", ReportsSchema)
												{
													IsPrimaryKey = true,
													DataType = DbType.Guid,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = true
												});
            	ReportsSchema.Columns.Add(new DatabaseColumn("Name", ReportsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	ReportsSchema.Columns.Add(new DatabaseColumn("Description", ReportsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
				// Add Reports to schema
            	DataProvider.Schema.Tables.Add(ReportsSchema);
				
				// Table: UserProperties
				// Primary Key: UserID
				ITable UserPropertiesSchema = new DatabaseTable("UserProperties", DataProvider) { ClassName = "UserProperty", SchemaName = "dbo" };
            	UserPropertiesSchema.Columns.Add(new DatabaseColumn("UserID", UserPropertiesSchema)
												{
													IsPrimaryKey = true,
													DataType = DbType.Guid,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	UserPropertiesSchema.Columns.Add(new DatabaseColumn("CustomerID", UserPropertiesSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Guid,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = true
												});
            	UserPropertiesSchema.Columns.Add(new DatabaseColumn("UserName", UserPropertiesSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	UserPropertiesSchema.Columns.Add(new DatabaseColumn("FirstName", UserPropertiesSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	UserPropertiesSchema.Columns.Add(new DatabaseColumn("LastName", UserPropertiesSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	UserPropertiesSchema.Columns.Add(new DatabaseColumn("Password", UserPropertiesSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
				// Add UserProperties to schema
            	DataProvider.Schema.Tables.Add(UserPropertiesSchema);
				
				// Table: UserSessions
				// Primary Key: SessionID
				ITable UserSessionsSchema = new DatabaseTable("UserSessions", DataProvider) { ClassName = "UserSession", SchemaName = "dbo" };
            	UserSessionsSchema.Columns.Add(new DatabaseColumn("SessionID", UserSessionsSchema)
												{
													IsPrimaryKey = true,
													DataType = DbType.Guid,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	UserSessionsSchema.Columns.Add(new DatabaseColumn("UserID", UserSessionsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Guid,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	UserSessionsSchema.Columns.Add(new DatabaseColumn("SessionStart", UserSessionsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	UserSessionsSchema.Columns.Add(new DatabaseColumn("SessionUpdate", UserSessionsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	UserSessionsSchema.Columns.Add(new DatabaseColumn("SessionEnd", UserSessionsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
				// Add UserSessions to schema
            	DataProvider.Schema.Tables.Add(UserSessionsSchema);
				
				// Table: vwCustomerReportList
				// Primary Key: 
				ITable vwCustomerReportListSchema = new DatabaseTable("vwCustomerReportList", DataProvider) { ClassName = "vwCustomerReportList", SchemaName = "dbo" };
            	vwCustomerReportListSchema.Columns.Add(new DatabaseColumn("CustomerID", vwCustomerReportListSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Guid,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwCustomerReportListSchema.Columns.Add(new DatabaseColumn("Name", vwCustomerReportListSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwCustomerReportListSchema.Columns.Add(new DatabaseColumn("ReportID", vwCustomerReportListSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Guid,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwCustomerReportListSchema.Columns.Add(new DatabaseColumn("EmailAddress", vwCustomerReportListSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
				// Add vwCustomerReportList to schema
            	DataProvider.Schema.Tables.Add(vwCustomerReportListSchema);
            }
            #endregion
        }
    }
}