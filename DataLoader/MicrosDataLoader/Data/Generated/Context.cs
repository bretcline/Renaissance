


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

namespace Jaxis.Inventory.Data
{
    public partial class BeverageMonitorDB : IQuerySurface
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

        public BeverageMonitorDB() 
        { 
            DataProvider = ProviderFactory.GetProvider("BeverageMonitor");
            Init();
        }

        public BeverageMonitorDB(string connectionStringName)
        {
            DataProvider = ProviderFactory.GetProvider(connectionStringName);
            Init();
        }

		public BeverageMonitorDB(string connectionString, string providerName)
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
			
        public Query<BanquetMenu> BanquetMenus { get; set; }
        public Query<Banquet> Banquets { get; set; }
        public Query<DAT3> DAT3S { get; set; }
        public Query<MenuItem> MenuItems { get; set; }
        public Query<POSPaymentDatum> POSPaymentData { get; set; }
        public Query<POSTicketItemModifier> POSTicketItemModifiers { get; set; }
        public Query<POSTicketItem> POSTicketItems { get; set; }
        public Query<POSTicket> POSTickets { get; set; }
        public Query<POSTVADatum> POSTVAData { get; set; }
        public Query<SageImport> SageImports { get; set; }
        public Query<vwDailyGuestCount> vwDailyGuestCounts { get; set; }
        public Query<vwDailyPOSTicketItem> vwDailyPOSTicketItems { get; set; }
        public Query<vwDailyPOSTicket> vwDailyPOSTickets { get; set; }
        public Query<vwDailyTicketDataSummary> vwDailyTicketDataSummaries { get; set; }
        public Query<vwPOSTicketItem> vwPOSTicketItems { get; set; }
        public Query<vwPOSTicket> vwPOSTickets { get; set; }
        public Query<vwSumaryItemDatum> vwSumaryItemData { get; set; }

			

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
            BanquetMenus = new Query<BanquetMenu>(provider);
            Banquets = new Query<Banquet>(provider);
            DAT3S = new Query<DAT3>(provider);
            MenuItems = new Query<MenuItem>(provider);
            POSPaymentData = new Query<POSPaymentDatum>(provider);
            POSTicketItemModifiers = new Query<POSTicketItemModifier>(provider);
            POSTicketItems = new Query<POSTicketItem>(provider);
            POSTickets = new Query<POSTicket>(provider);
            POSTVAData = new Query<POSTVADatum>(provider);
            SageImports = new Query<SageImport>(provider);
            vwDailyGuestCounts = new Query<vwDailyGuestCount>(provider);
            vwDailyPOSTicketItems = new Query<vwDailyPOSTicketItem>(provider);
            vwDailyPOSTickets = new Query<vwDailyPOSTicket>(provider);
            vwDailyTicketDataSummaries = new Query<vwDailyTicketDataSummary>(provider);
            vwPOSTicketItems = new Query<vwPOSTicketItem>(provider);
            vwPOSTickets = new Query<vwPOSTicket>(provider);
            vwSumaryItemData = new Query<vwSumaryItemDatum>(provider);
            #endregion


            #region ' Schemas '
        	if(DataProvider.Schema.Tables.Count == 0)
			{
				
				// Table: BanquetMenus
				// Primary Key: BanquetID
				ITable BanquetMenusSchema = new DatabaseTable("BanquetMenus", DataProvider) { ClassName = "BanquetMenu", SchemaName = "dbo" };
            	BanquetMenusSchema.Columns.Add(new DatabaseColumn("BanquetID", BanquetMenusSchema)
												{
													IsPrimaryKey = true,
													DataType = DbType.Guid,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = true
												});
            	BanquetMenusSchema.Columns.Add(new DatabaseColumn("MenuItemID", BanquetMenusSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Guid,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = true
												});
				// Add BanquetMenus to schema
            	DataProvider.Schema.Tables.Add(BanquetMenusSchema);
				
				// Table: Banquets
				// Primary Key: BanquetID
				ITable BanquetsSchema = new DatabaseTable("Banquets", DataProvider) { ClassName = "Banquet", SchemaName = "dbo" };
            	BanquetsSchema.Columns.Add(new DatabaseColumn("BanquetID", BanquetsSchema)
												{
													IsPrimaryKey = true,
													DataType = DbType.Guid,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = true
												});
            	BanquetsSchema.Columns.Add(new DatabaseColumn("Name", BanquetsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	BanquetsSchema.Columns.Add(new DatabaseColumn("CustomerName", BanquetsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
				// Add Banquets to schema
            	DataProvider.Schema.Tables.Add(BanquetsSchema);
				
				// Table: DAT3
				// Primary Key: 
				ITable DAT3Schema = new DatabaseTable("DAT3", DataProvider) { ClassName = "DAT3", SchemaName = "dbo" };
            	DAT3Schema.Columns.Add(new DatabaseColumn("Stay date", DAT3Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.AnsiString,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	DAT3Schema.Columns.Add(new DatabaseColumn("Market Category", DAT3Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.AnsiString,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	DAT3Schema.Columns.Add(new DatabaseColumn("Rate Program Tier", DAT3Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.AnsiString,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	DAT3Schema.Columns.Add(new DatabaseColumn("Rate Category Code", DAT3Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.AnsiString,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	DAT3Schema.Columns.Add(new DatabaseColumn("Market Code", DAT3Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.AnsiString,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	DAT3Schema.Columns.Add(new DatabaseColumn("RoomNights", DAT3Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	DAT3Schema.Columns.Add(new DatabaseColumn("ADR (Net)", DAT3Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Currency,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	DAT3Schema.Columns.Add(new DatabaseColumn("Revenue (Net)", DAT3Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Currency,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	DAT3Schema.Columns.Add(new DatabaseColumn("Additional Demand", DAT3Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	DAT3Schema.Columns.Add(new DatabaseColumn("Total Demand", DAT3Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	DAT3Schema.Columns.Add(new DatabaseColumn("Average Room Nights", DAT3Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Currency,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	DAT3Schema.Columns.Add(new DatabaseColumn("Average Revenue", DAT3Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Currency,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
				// Add DAT3 to schema
            	DataProvider.Schema.Tables.Add(DAT3Schema);
				
				// Table: MenuItems
				// Primary Key: MenuItemID
				ITable MenuItemsSchema = new DatabaseTable("MenuItems", DataProvider) { ClassName = "MenuItem", SchemaName = "dbo" };
            	MenuItemsSchema.Columns.Add(new DatabaseColumn("MenuItemID", MenuItemsSchema)
												{
													IsPrimaryKey = true,
													DataType = DbType.Guid,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = true
												});
            	MenuItemsSchema.Columns.Add(new DatabaseColumn("Name", MenuItemsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	MenuItemsSchema.Columns.Add(new DatabaseColumn("Description", MenuItemsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	MenuItemsSchema.Columns.Add(new DatabaseColumn("Cost", MenuItemsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Currency,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	MenuItemsSchema.Columns.Add(new DatabaseColumn("Price", MenuItemsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Currency,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	MenuItemsSchema.Columns.Add(new DatabaseColumn("InsertDate", MenuItemsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	MenuItemsSchema.Columns.Add(new DatabaseColumn("UpdateDate", MenuItemsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
				// Add MenuItems to schema
            	DataProvider.Schema.Tables.Add(MenuItemsSchema);
				
				// Table: POSPaymentData
				// Primary Key: PaymentDataID
				ITable POSPaymentDataSchema = new DatabaseTable("POSPaymentData", DataProvider) { ClassName = "POSPaymentDatum", SchemaName = "dbo" };
            	POSPaymentDataSchema.Columns.Add(new DatabaseColumn("PaymentDataID", POSPaymentDataSchema)
												{
													IsPrimaryKey = true,
													DataType = DbType.Guid,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	POSPaymentDataSchema.Columns.Add(new DatabaseColumn("POSTicketID", POSPaymentDataSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Guid,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = true
												});
            	POSPaymentDataSchema.Columns.Add(new DatabaseColumn("AccountNumber", POSPaymentDataSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	POSPaymentDataSchema.Columns.Add(new DatabaseColumn("RoomNumber", POSPaymentDataSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	POSPaymentDataSchema.Columns.Add(new DatabaseColumn("CustomerName", POSPaymentDataSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	POSPaymentDataSchema.Columns.Add(new DatabaseColumn("PaymentType", POSPaymentDataSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	POSPaymentDataSchema.Columns.Add(new DatabaseColumn("Payment", POSPaymentDataSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Currency,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
				// Add POSPaymentData to schema
            	DataProvider.Schema.Tables.Add(POSPaymentDataSchema);
				
				// Table: POSTicketItemModifiers
				// Primary Key: POSTicketITemModifierID
				ITable POSTicketItemModifiersSchema = new DatabaseTable("POSTicketItemModifiers", DataProvider) { ClassName = "POSTicketItemModifier", SchemaName = "dbo" };
            	POSTicketItemModifiersSchema.Columns.Add(new DatabaseColumn("POSTicketITemModifierID", POSTicketItemModifiersSchema)
												{
													IsPrimaryKey = true,
													DataType = DbType.Guid,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	POSTicketItemModifiersSchema.Columns.Add(new DatabaseColumn("POSTicketItemID", POSTicketItemModifiersSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Guid,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = true
												});
            	POSTicketItemModifiersSchema.Columns.Add(new DatabaseColumn("Name", POSTicketItemModifiersSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	POSTicketItemModifiersSchema.Columns.Add(new DatabaseColumn("Price", POSTicketItemModifiersSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Currency,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
				// Add POSTicketItemModifiers to schema
            	DataProvider.Schema.Tables.Add(POSTicketItemModifiersSchema);
				
				// Table: POSTicketItems
				// Primary Key: POSTicketItemID
				ITable POSTicketItemsSchema = new DatabaseTable("POSTicketItems", DataProvider) { ClassName = "POSTicketItem", SchemaName = "dbo" };
            	POSTicketItemsSchema.Columns.Add(new DatabaseColumn("POSTicketItemID", POSTicketItemsSchema)
												{
													IsPrimaryKey = true,
													DataType = DbType.Guid,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = true
												});
            	POSTicketItemsSchema.Columns.Add(new DatabaseColumn("POSTicketID", POSTicketItemsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Guid,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = true
												});
            	POSTicketItemsSchema.Columns.Add(new DatabaseColumn("Comment", POSTicketItemsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	POSTicketItemsSchema.Columns.Add(new DatabaseColumn("Description", POSTicketItemsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	POSTicketItemsSchema.Columns.Add(new DatabaseColumn("Price", POSTicketItemsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Currency,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	POSTicketItemsSchema.Columns.Add(new DatabaseColumn("Reconciled", POSTicketItemsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	POSTicketItemsSchema.Columns.Add(new DatabaseColumn("Quantity", POSTicketItemsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	POSTicketItemsSchema.Columns.Add(new DatabaseColumn("Status", POSTicketItemsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	POSTicketItemsSchema.Columns.Add(new DatabaseColumn("Credit", POSTicketItemsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Boolean,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
				// Add POSTicketItems to schema
            	DataProvider.Schema.Tables.Add(POSTicketItemsSchema);
				
				// Table: POSTickets
				// Primary Key: POSTicketID
				ITable POSTicketsSchema = new DatabaseTable("POSTickets", DataProvider) { ClassName = "POSTicket", SchemaName = "dbo" };
            	POSTicketsSchema.Columns.Add(new DatabaseColumn("POSTicketID", POSTicketsSchema)
												{
													IsPrimaryKey = true,
													DataType = DbType.Guid,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = true
												});
            	POSTicketsSchema.Columns.Add(new DatabaseColumn("CheckNumber", POSTicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	POSTicketsSchema.Columns.Add(new DatabaseColumn("Comments", POSTicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	POSTicketsSchema.Columns.Add(new DatabaseColumn("TicketDate", POSTicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	POSTicketsSchema.Columns.Add(new DatabaseColumn("Establishment", POSTicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	POSTicketsSchema.Columns.Add(new DatabaseColumn("Server", POSTicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	POSTicketsSchema.Columns.Add(new DatabaseColumn("ServerName", POSTicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	POSTicketsSchema.Columns.Add(new DatabaseColumn("GuestCount", POSTicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	POSTicketsSchema.Columns.Add(new DatabaseColumn("CustomerTable", POSTicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	POSTicketsSchema.Columns.Add(new DatabaseColumn("RawData", POSTicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	POSTicketsSchema.Columns.Add(new DatabaseColumn("TouchCount", POSTicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	POSTicketsSchema.Columns.Add(new DatabaseColumn("TipAmount", POSTicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Currency,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	POSTicketsSchema.Columns.Add(new DatabaseColumn("GuestCountModified", POSTicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	POSTicketsSchema.Columns.Add(new DatabaseColumn("TicketTotal", POSTicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Currency,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	POSTicketsSchema.Columns.Add(new DatabaseColumn("DataSource", POSTicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	POSTicketsSchema.Columns.Add(new DatabaseColumn("TransactionID", POSTicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
				// Add POSTickets to schema
            	DataProvider.Schema.Tables.Add(POSTicketsSchema);
				
				// Table: POSTVAData
				// Primary Key: TVADataID
				ITable POSTVADataSchema = new DatabaseTable("POSTVAData", DataProvider) { ClassName = "POSTVADatum", SchemaName = "dbo" };
            	POSTVADataSchema.Columns.Add(new DatabaseColumn("TVADataID", POSTVADataSchema)
												{
													IsPrimaryKey = true,
													DataType = DbType.Guid,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	POSTVADataSchema.Columns.Add(new DatabaseColumn("POSTicketID", POSTVADataSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Guid,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = true
												});
            	POSTVADataSchema.Columns.Add(new DatabaseColumn("Amount", POSTVADataSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Currency,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	POSTVADataSchema.Columns.Add(new DatabaseColumn("Percentage", POSTVADataSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	POSTVADataSchema.Columns.Add(new DatabaseColumn("Total", POSTVADataSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Currency,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
				// Add POSTVAData to schema
            	DataProvider.Schema.Tables.Add(POSTVADataSchema);
				
				// Table: SageImport
				// Primary Key: 
				ITable SageImportSchema = new DatabaseTable("SageImport", DataProvider) { ClassName = "SageImport", SchemaName = "dbo" };
            	SageImportSchema.Columns.Add(new DatabaseColumn("AccountCode", SageImportSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int64,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	SageImportSchema.Columns.Add(new DatabaseColumn("Category", SageImportSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	SageImportSchema.Columns.Add(new DatabaseColumn("CategoryName", SageImportSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	SageImportSchema.Columns.Add(new DatabaseColumn("CategoryNameEng", SageImportSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	SageImportSchema.Columns.Add(new DatabaseColumn("Debit", SageImportSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Currency,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	SageImportSchema.Columns.Add(new DatabaseColumn("Credit", SageImportSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Currency,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	SageImportSchema.Columns.Add(new DatabaseColumn("SalesRecorded", SageImportSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Currency,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
				// Add SageImport to schema
            	DataProvider.Schema.Tables.Add(SageImportSchema);
				
				// Table: vwDailyGuestCount
				// Primary Key: 
				ITable vwDailyGuestCountSchema = new DatabaseTable("vwDailyGuestCount", DataProvider) { ClassName = "vwDailyGuestCount", SchemaName = "dbo" };
            	vwDailyGuestCountSchema.Columns.Add(new DatabaseColumn("TicketDate", vwDailyGuestCountSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.AnsiString,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDailyGuestCountSchema.Columns.Add(new DatabaseColumn("EstablishmentModified", vwDailyGuestCountSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDailyGuestCountSchema.Columns.Add(new DatabaseColumn("GuestCount", vwDailyGuestCountSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDailyGuestCountSchema.Columns.Add(new DatabaseColumn("ModifiedGuestCount", vwDailyGuestCountSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
				// Add vwDailyGuestCount to schema
            	DataProvider.Schema.Tables.Add(vwDailyGuestCountSchema);
				
				// Table: vwDailyPOSTicketItems
				// Primary Key: 
				ITable vwDailyPOSTicketItemsSchema = new DatabaseTable("vwDailyPOSTicketItems", DataProvider) { ClassName = "vwDailyPOSTicketItem", SchemaName = "dbo" };
            	vwDailyPOSTicketItemsSchema.Columns.Add(new DatabaseColumn("CheckNumber", vwDailyPOSTicketItemsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDailyPOSTicketItemsSchema.Columns.Add(new DatabaseColumn("GuestCountModified", vwDailyPOSTicketItemsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDailyPOSTicketItemsSchema.Columns.Add(new DatabaseColumn("TicketDate", vwDailyPOSTicketItemsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDailyPOSTicketItemsSchema.Columns.Add(new DatabaseColumn("Establishment", vwDailyPOSTicketItemsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDailyPOSTicketItemsSchema.Columns.Add(new DatabaseColumn("ServerName", vwDailyPOSTicketItemsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDailyPOSTicketItemsSchema.Columns.Add(new DatabaseColumn("Description", vwDailyPOSTicketItemsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDailyPOSTicketItemsSchema.Columns.Add(new DatabaseColumn("Comment", vwDailyPOSTicketItemsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDailyPOSTicketItemsSchema.Columns.Add(new DatabaseColumn("Price", vwDailyPOSTicketItemsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDailyPOSTicketItemsSchema.Columns.Add(new DatabaseColumn("Quantity", vwDailyPOSTicketItemsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
				// Add vwDailyPOSTicketItems to schema
            	DataProvider.Schema.Tables.Add(vwDailyPOSTicketItemsSchema);
				
				// Table: vwDailyPOSTickets
				// Primary Key: 
				ITable vwDailyPOSTicketsSchema = new DatabaseTable("vwDailyPOSTickets", DataProvider) { ClassName = "vwDailyPOSTicket", SchemaName = "dbo" };
            	vwDailyPOSTicketsSchema.Columns.Add(new DatabaseColumn("POSTicketID", vwDailyPOSTicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Guid,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDailyPOSTicketsSchema.Columns.Add(new DatabaseColumn("CheckNumber", vwDailyPOSTicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDailyPOSTicketsSchema.Columns.Add(new DatabaseColumn("Comments", vwDailyPOSTicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDailyPOSTicketsSchema.Columns.Add(new DatabaseColumn("TicketDate", vwDailyPOSTicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDailyPOSTicketsSchema.Columns.Add(new DatabaseColumn("Establishment", vwDailyPOSTicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDailyPOSTicketsSchema.Columns.Add(new DatabaseColumn("Server", vwDailyPOSTicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDailyPOSTicketsSchema.Columns.Add(new DatabaseColumn("ServerName", vwDailyPOSTicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDailyPOSTicketsSchema.Columns.Add(new DatabaseColumn("GuestCount", vwDailyPOSTicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDailyPOSTicketsSchema.Columns.Add(new DatabaseColumn("CustomerTable", vwDailyPOSTicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDailyPOSTicketsSchema.Columns.Add(new DatabaseColumn("RawData", vwDailyPOSTicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDailyPOSTicketsSchema.Columns.Add(new DatabaseColumn("TouchCount", vwDailyPOSTicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDailyPOSTicketsSchema.Columns.Add(new DatabaseColumn("TipAmount", vwDailyPOSTicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Currency,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDailyPOSTicketsSchema.Columns.Add(new DatabaseColumn("GuestCountModified", vwDailyPOSTicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDailyPOSTicketsSchema.Columns.Add(new DatabaseColumn("TicketTotal", vwDailyPOSTicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Currency,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDailyPOSTicketsSchema.Columns.Add(new DatabaseColumn("EstablishmentModified", vwDailyPOSTicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
				// Add vwDailyPOSTickets to schema
            	DataProvider.Schema.Tables.Add(vwDailyPOSTicketsSchema);
				
				// Table: vwDailyTicketDataSummary
				// Primary Key: 
				ITable vwDailyTicketDataSummarySchema = new DatabaseTable("vwDailyTicketDataSummary", DataProvider) { ClassName = "vwDailyTicketDataSummary", SchemaName = "dbo" };
            	vwDailyTicketDataSummarySchema.Columns.Add(new DatabaseColumn("TicketDate", vwDailyTicketDataSummarySchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.AnsiString,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDailyTicketDataSummarySchema.Columns.Add(new DatabaseColumn("EstablishmentModified", vwDailyTicketDataSummarySchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDailyTicketDataSummarySchema.Columns.Add(new DatabaseColumn("GuestCount", vwDailyTicketDataSummarySchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDailyTicketDataSummarySchema.Columns.Add(new DatabaseColumn("ModifiedGuestCount", vwDailyTicketDataSummarySchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDailyTicketDataSummarySchema.Columns.Add(new DatabaseColumn("TicketTotal", vwDailyTicketDataSummarySchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Currency,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDailyTicketDataSummarySchema.Columns.Add(new DatabaseColumn("PaymentAmount", vwDailyTicketDataSummarySchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Currency,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDailyTicketDataSummarySchema.Columns.Add(new DatabaseColumn("TVAAmount", vwDailyTicketDataSummarySchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Currency,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
				// Add vwDailyTicketDataSummary to schema
            	DataProvider.Schema.Tables.Add(vwDailyTicketDataSummarySchema);
				
				// Table: vwPOSTicketItems
				// Primary Key: 
				ITable vwPOSTicketItemsSchema = new DatabaseTable("vwPOSTicketItems", DataProvider) { ClassName = "vwPOSTicketItem", SchemaName = "dbo" };
            	vwPOSTicketItemsSchema.Columns.Add(new DatabaseColumn("POSTicketID", vwPOSTicketItemsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Guid,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwPOSTicketItemsSchema.Columns.Add(new DatabaseColumn("CheckNumber", vwPOSTicketItemsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwPOSTicketItemsSchema.Columns.Add(new DatabaseColumn("GuestCountModified", vwPOSTicketItemsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwPOSTicketItemsSchema.Columns.Add(new DatabaseColumn("TicketDate", vwPOSTicketItemsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwPOSTicketItemsSchema.Columns.Add(new DatabaseColumn("Establishment", vwPOSTicketItemsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwPOSTicketItemsSchema.Columns.Add(new DatabaseColumn("ServerName", vwPOSTicketItemsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwPOSTicketItemsSchema.Columns.Add(new DatabaseColumn("Description", vwPOSTicketItemsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwPOSTicketItemsSchema.Columns.Add(new DatabaseColumn("Comment", vwPOSTicketItemsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwPOSTicketItemsSchema.Columns.Add(new DatabaseColumn("Price", vwPOSTicketItemsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwPOSTicketItemsSchema.Columns.Add(new DatabaseColumn("Quantity", vwPOSTicketItemsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
				// Add vwPOSTicketItems to schema
            	DataProvider.Schema.Tables.Add(vwPOSTicketItemsSchema);
				
				// Table: vwPOSTickets
				// Primary Key: 
				ITable vwPOSTicketsSchema = new DatabaseTable("vwPOSTickets", DataProvider) { ClassName = "vwPOSTicket", SchemaName = "dbo" };
            	vwPOSTicketsSchema.Columns.Add(new DatabaseColumn("POSTicketID", vwPOSTicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Guid,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwPOSTicketsSchema.Columns.Add(new DatabaseColumn("CheckNumber", vwPOSTicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwPOSTicketsSchema.Columns.Add(new DatabaseColumn("Comments", vwPOSTicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwPOSTicketsSchema.Columns.Add(new DatabaseColumn("TicketDate", vwPOSTicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwPOSTicketsSchema.Columns.Add(new DatabaseColumn("Establishment", vwPOSTicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwPOSTicketsSchema.Columns.Add(new DatabaseColumn("Server", vwPOSTicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwPOSTicketsSchema.Columns.Add(new DatabaseColumn("ServerName", vwPOSTicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwPOSTicketsSchema.Columns.Add(new DatabaseColumn("GuestCount", vwPOSTicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwPOSTicketsSchema.Columns.Add(new DatabaseColumn("CustomerTable", vwPOSTicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwPOSTicketsSchema.Columns.Add(new DatabaseColumn("RawData", vwPOSTicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwPOSTicketsSchema.Columns.Add(new DatabaseColumn("TouchCount", vwPOSTicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwPOSTicketsSchema.Columns.Add(new DatabaseColumn("TipAmount", vwPOSTicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Currency,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwPOSTicketsSchema.Columns.Add(new DatabaseColumn("GuestCountModified", vwPOSTicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwPOSTicketsSchema.Columns.Add(new DatabaseColumn("TicketTotal", vwPOSTicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Currency,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwPOSTicketsSchema.Columns.Add(new DatabaseColumn("EstablishmentModified", vwPOSTicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
				// Add vwPOSTickets to schema
            	DataProvider.Schema.Tables.Add(vwPOSTicketsSchema);
				
				// Table: vwSumaryItemData
				// Primary Key: 
				ITable vwSumaryItemDataSchema = new DatabaseTable("vwSumaryItemData", DataProvider) { ClassName = "vwSumaryItemDatum", SchemaName = "dbo" };
            	vwSumaryItemDataSchema.Columns.Add(new DatabaseColumn("Description", vwSumaryItemDataSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwSumaryItemDataSchema.Columns.Add(new DatabaseColumn("Count", vwSumaryItemDataSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwSumaryItemDataSchema.Columns.Add(new DatabaseColumn("Quantity", vwSumaryItemDataSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwSumaryItemDataSchema.Columns.Add(new DatabaseColumn("Price", vwSumaryItemDataSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Currency,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwSumaryItemDataSchema.Columns.Add(new DatabaseColumn("AveragePrice", vwSumaryItemDataSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Currency,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
				// Add vwSumaryItemData to schema
            	DataProvider.Schema.Tables.Add(vwSumaryItemDataSchema);
            }
            #endregion
        }
    }
}