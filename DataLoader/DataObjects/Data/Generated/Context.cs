


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

namespace Jaxis.POS.Data
{
    public partial class RenAixDB : IQuerySurface
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

        public RenAixDB() 
        { 
            DataProvider = ProviderFactory.GetProvider("RenAix");
            Init();
        }

        public RenAixDB(string connectionStringName)
        {
            DataProvider = ProviderFactory.GetProvider(connectionStringName);
            Init();
        }

		public RenAixDB(string connectionString, string providerName)
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
        public Query<DailyWeatherForecast> DailyWeatherForecasts { get; set; }
        public Query<DAT3> DAT3S { get; set; }
        public Query<MenuItem> MenuItems { get; set; }
        public Query<MicrosTimePeriod> MicrosTimePeriods { get; set; }
        public Query<POSEstablisment> POSEstablisments { get; set; }
        public Query<POSPaymentDatum> POSPaymentData { get; set; }
        public Query<POSTicketItemModifier> POSTicketItemModifiers { get; set; }
        public Query<POSTicketItem> POSTicketItems { get; set; }
        public Query<POSTicket> POSTickets { get; set; }
        public Query<POSTVADatum> POSTVAData { get; set; }
        public Query<SageBalance> SageBalances { get; set; }
        public Query<vwCategoryDetail> vwCategoryDetails { get; set; }
        public Query<vwCategorySummaryByMonth> vwCategorySummaryByMonths { get; set; }
        public Query<vwCheckFoodGroup> vwCheckFoodGroups { get; set; }
        public Query<vwDailyGuestCount> vwDailyGuestCounts { get; set; }
        public Query<vwDailyPOSTicketItem> vwDailyPOSTicketItems { get; set; }
        public Query<vwDailyPOSTicket> vwDailyPOSTickets { get; set; }
        public Query<vwDailyTicketDataSummary> vwDailyTicketDataSummaries { get; set; }
        public Query<vwDiscountByTicket> vwDiscountByTickets { get; set; }
        public Query<vwMissingTicket> vwMissingTickets { get; set; }
        public Query<vwMRSBRFinancial> vwMRSBRFinancials { get; set; }
        public Query<vwOperaMicrosCheck> vwOperaMicrosChecks { get; set; }
        public Query<vwPOSTicketItem> vwPOSTicketItems { get; set; }
        public Query<vwPOSTicket> vwPOSTickets { get; set; }
        public Query<vwSageCategoryTotal> vwSageCategoryTotals { get; set; }
        public Query<vwSumaryItemDatum> vwSumaryItemData { get; set; }
        public Query<banquet_datum> banquet_data { get; set; }
        public Query<business_block> business_blocks { get; set; }
        public Query<dly_corr_ttl> dly_corr_ttls { get; set; }
        public Query<dly_discount_ttl> dly_discount_ttls { get; set; }
        public Query<maj_grp_def> maj_grp_defs { get; set; }
        public Query<mfd_check_dtl> mfd_check_dtls { get; set; }
        public Query<mfd_check_ttl> mfd_check_ttls { get; set; }
        public Query<mi_def> mi_defs { get; set; }
        public Query<mi_price_def> mi_price_defs { get; set; }
        public Query<mi_slu_def> mi_slu_defs { get; set; }
        public Query<Micros_Ticket_Detail> Micros_Ticket_Details { get; set; }
        public Query<Micros_Ticket> Micros_Tickets { get; set; }
        public Query<mrsbr_financial> mrsbr_financials { get; set; }
        public Query<OperaA214> OperaA214s { get; set; }
        public Query<OperaAD120BusinessBlock> OperaAD120BusinessBlocks { get; set; }
        public Query<OperaAD160> OperaAD160s { get; set; }
        public Query<OperaD114> OperaD114s { get; set; }
        public Query<OperaD140> OperaD140s { get; set; }
        public Query<OperaF116> OperaF116s { get; set; }
        public Query<OperaH260> OperaH260s { get; set; }
        public Query<OperaP112Departure> OperaP112Departures { get; set; }
        public Query<POSTicketDatum> POSTicketData { get; set; }
        public Query<PS550GuestList> PS550GuestLists { get; set; }
        public Query<rep_bh_short> rep_bh_shorts { get; set; }
        public Query<v_R_kds_chk_dtl> v_R_kds_chk_dtls { get; set; }
        public Query<DailyPOSTopTen> DailyPOSTopTens { get; set; }
        public Query<vwDailyOperaTotal> vwDailyOperaTotals { get; set; }
        public Query<vwDailyPOSItemSummary> vwDailyPOSItemSummaries { get; set; }
        public Query<vwDailyPOSTopTen> vwDailyPOSTopTens { get; set; }
        public Query<vwDailyPOSTotal> vwDailyPOSTotals { get; set; }
        public Query<vwDiscountedItemsByReasonPO> vwDiscountedItemsByReasonPOs { get; set; }
        public Query<vwOpera214Daily> vwOpera214Dailies { get; set; }
        public Query<vwPackageLoss> vwPackageLosses { get; set; }
        public Query<vwPackageLossByParsedDatum> vwPackageLossByParsedData { get; set; }
        public Query<vwPercentItemsReturned> vwPercentItemsReturneds { get; set; }
        public Query<vwPercentItemsReturnedMonthly> vwPercentItemsReturnedMonthlies { get; set; }
        public Query<vwPOSItemSummary> vwPOSItemSummaries { get; set; }
        public Query<vwPOSTopTen> vwPOSTopTens { get; set; }
        public Query<vwPOSTotal> vwPOSTotals { get; set; }

			

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
            DailyWeatherForecasts = new Query<DailyWeatherForecast>(provider);
            DAT3S = new Query<DAT3>(provider);
            MenuItems = new Query<MenuItem>(provider);
            MicrosTimePeriods = new Query<MicrosTimePeriod>(provider);
            POSEstablisments = new Query<POSEstablisment>(provider);
            POSPaymentData = new Query<POSPaymentDatum>(provider);
            POSTicketItemModifiers = new Query<POSTicketItemModifier>(provider);
            POSTicketItems = new Query<POSTicketItem>(provider);
            POSTickets = new Query<POSTicket>(provider);
            POSTVAData = new Query<POSTVADatum>(provider);
            SageBalances = new Query<SageBalance>(provider);
            vwCategoryDetails = new Query<vwCategoryDetail>(provider);
            vwCategorySummaryByMonths = new Query<vwCategorySummaryByMonth>(provider);
            vwCheckFoodGroups = new Query<vwCheckFoodGroup>(provider);
            vwDailyGuestCounts = new Query<vwDailyGuestCount>(provider);
            vwDailyPOSTicketItems = new Query<vwDailyPOSTicketItem>(provider);
            vwDailyPOSTickets = new Query<vwDailyPOSTicket>(provider);
            vwDailyTicketDataSummaries = new Query<vwDailyTicketDataSummary>(provider);
            vwDiscountByTickets = new Query<vwDiscountByTicket>(provider);
            vwMissingTickets = new Query<vwMissingTicket>(provider);
            vwMRSBRFinancials = new Query<vwMRSBRFinancial>(provider);
            vwOperaMicrosChecks = new Query<vwOperaMicrosCheck>(provider);
            vwPOSTicketItems = new Query<vwPOSTicketItem>(provider);
            vwPOSTickets = new Query<vwPOSTicket>(provider);
            vwSageCategoryTotals = new Query<vwSageCategoryTotal>(provider);
            vwSumaryItemData = new Query<vwSumaryItemDatum>(provider);
            banquet_data = new Query<banquet_datum>(provider);
            business_blocks = new Query<business_block>(provider);
            dly_corr_ttls = new Query<dly_corr_ttl>(provider);
            dly_discount_ttls = new Query<dly_discount_ttl>(provider);
            maj_grp_defs = new Query<maj_grp_def>(provider);
            mfd_check_dtls = new Query<mfd_check_dtl>(provider);
            mfd_check_ttls = new Query<mfd_check_ttl>(provider);
            mi_defs = new Query<mi_def>(provider);
            mi_price_defs = new Query<mi_price_def>(provider);
            mi_slu_defs = new Query<mi_slu_def>(provider);
            Micros_Ticket_Details = new Query<Micros_Ticket_Detail>(provider);
            Micros_Tickets = new Query<Micros_Ticket>(provider);
            mrsbr_financials = new Query<mrsbr_financial>(provider);
            OperaA214s = new Query<OperaA214>(provider);
            OperaAD120BusinessBlocks = new Query<OperaAD120BusinessBlock>(provider);
            OperaAD160s = new Query<OperaAD160>(provider);
            OperaD114s = new Query<OperaD114>(provider);
            OperaD140s = new Query<OperaD140>(provider);
            OperaF116s = new Query<OperaF116>(provider);
            OperaH260s = new Query<OperaH260>(provider);
            OperaP112Departures = new Query<OperaP112Departure>(provider);
            POSTicketData = new Query<POSTicketDatum>(provider);
            PS550GuestLists = new Query<PS550GuestList>(provider);
            rep_bh_shorts = new Query<rep_bh_short>(provider);
            v_R_kds_chk_dtls = new Query<v_R_kds_chk_dtl>(provider);
            DailyPOSTopTens = new Query<DailyPOSTopTen>(provider);
            vwDailyOperaTotals = new Query<vwDailyOperaTotal>(provider);
            vwDailyPOSItemSummaries = new Query<vwDailyPOSItemSummary>(provider);
            vwDailyPOSTopTens = new Query<vwDailyPOSTopTen>(provider);
            vwDailyPOSTotals = new Query<vwDailyPOSTotal>(provider);
            vwDiscountedItemsByReasonPOs = new Query<vwDiscountedItemsByReasonPO>(provider);
            vwOpera214Dailies = new Query<vwOpera214Daily>(provider);
            vwPackageLosses = new Query<vwPackageLoss>(provider);
            vwPackageLossByParsedData = new Query<vwPackageLossByParsedDatum>(provider);
            vwPercentItemsReturneds = new Query<vwPercentItemsReturned>(provider);
            vwPercentItemsReturnedMonthlies = new Query<vwPercentItemsReturnedMonthly>(provider);
            vwPOSItemSummaries = new Query<vwPOSItemSummary>(provider);
            vwPOSTopTens = new Query<vwPOSTopTen>(provider);
            vwPOSTotals = new Query<vwPOSTotal>(provider);
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
				
				// Table: DailyWeatherForecast
				// Primary Key: DailyWeatherForecastID
				ITable DailyWeatherForecastSchema = new DatabaseTable("DailyWeatherForecast", DataProvider) { ClassName = "DailyWeatherForecast", SchemaName = "dbo" };
            	DailyWeatherForecastSchema.Columns.Add(new DatabaseColumn("DailyWeatherForecastID", DailyWeatherForecastSchema)
												{
													IsPrimaryKey = true,
													DataType = DbType.Guid,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	DailyWeatherForecastSchema.Columns.Add(new DatabaseColumn("ForecastDate", DailyWeatherForecastSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	DailyWeatherForecastSchema.Columns.Add(new DatabaseColumn("Summary", DailyWeatherForecastSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	DailyWeatherForecastSchema.Columns.Add(new DatabaseColumn("Icon", DailyWeatherForecastSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	DailyWeatherForecastSchema.Columns.Add(new DatabaseColumn("SunriseTime", DailyWeatherForecastSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int64,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	DailyWeatherForecastSchema.Columns.Add(new DatabaseColumn("SunsetTime", DailyWeatherForecastSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int64,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	DailyWeatherForecastSchema.Columns.Add(new DatabaseColumn("MoonPhase", DailyWeatherForecastSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Double,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	DailyWeatherForecastSchema.Columns.Add(new DatabaseColumn("PrecipAccumulation", DailyWeatherForecastSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Double,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	DailyWeatherForecastSchema.Columns.Add(new DatabaseColumn("PrecipIntensity", DailyWeatherForecastSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Double,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	DailyWeatherForecastSchema.Columns.Add(new DatabaseColumn("PrecipIntensityMax", DailyWeatherForecastSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Double,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	DailyWeatherForecastSchema.Columns.Add(new DatabaseColumn("PrecipIntensityMaxTime", DailyWeatherForecastSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int64,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	DailyWeatherForecastSchema.Columns.Add(new DatabaseColumn("PrecipProbability", DailyWeatherForecastSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Double,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	DailyWeatherForecastSchema.Columns.Add(new DatabaseColumn("PrecipType", DailyWeatherForecastSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	DailyWeatherForecastSchema.Columns.Add(new DatabaseColumn("TemperatureMin", DailyWeatherForecastSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Double,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	DailyWeatherForecastSchema.Columns.Add(new DatabaseColumn("TemperatureMinTime", DailyWeatherForecastSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int64,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	DailyWeatherForecastSchema.Columns.Add(new DatabaseColumn("TemperatureMax", DailyWeatherForecastSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Double,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	DailyWeatherForecastSchema.Columns.Add(new DatabaseColumn("TemperatureMaxTime", DailyWeatherForecastSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int64,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	DailyWeatherForecastSchema.Columns.Add(new DatabaseColumn("ApparentTemperatureMin", DailyWeatherForecastSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Double,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	DailyWeatherForecastSchema.Columns.Add(new DatabaseColumn("ApparentTemperatureMinTime", DailyWeatherForecastSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int64,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	DailyWeatherForecastSchema.Columns.Add(new DatabaseColumn("ApparentTemperatureMax", DailyWeatherForecastSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Double,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	DailyWeatherForecastSchema.Columns.Add(new DatabaseColumn("ApparentTemperatureMaxTime", DailyWeatherForecastSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int64,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	DailyWeatherForecastSchema.Columns.Add(new DatabaseColumn("DewPoint", DailyWeatherForecastSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Double,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	DailyWeatherForecastSchema.Columns.Add(new DatabaseColumn("WindSpeed", DailyWeatherForecastSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Double,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	DailyWeatherForecastSchema.Columns.Add(new DatabaseColumn("WindBearing", DailyWeatherForecastSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Double,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	DailyWeatherForecastSchema.Columns.Add(new DatabaseColumn("CloudCover", DailyWeatherForecastSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Double,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	DailyWeatherForecastSchema.Columns.Add(new DatabaseColumn("Humidity", DailyWeatherForecastSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Double,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	DailyWeatherForecastSchema.Columns.Add(new DatabaseColumn("Pressure", DailyWeatherForecastSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Double,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	DailyWeatherForecastSchema.Columns.Add(new DatabaseColumn("Visibility", DailyWeatherForecastSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Double,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	DailyWeatherForecastSchema.Columns.Add(new DatabaseColumn("Ozone", DailyWeatherForecastSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Double,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	DailyWeatherForecastSchema.Columns.Add(new DatabaseColumn("QueryDate", DailyWeatherForecastSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.AnsiString,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	DailyWeatherForecastSchema.Columns.Add(new DatabaseColumn("Lat", DailyWeatherForecastSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	DailyWeatherForecastSchema.Columns.Add(new DatabaseColumn("Lon", DailyWeatherForecastSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
				// Add DailyWeatherForecast to schema
            	DataProvider.Schema.Tables.Add(DailyWeatherForecastSchema);
				
				// Table: DAT3
				// Primary Key: 
				ITable DAT3Schema = new DatabaseTable("DAT3", DataProvider) { ClassName = "DAT3", SchemaName = "dbo" };
            	DAT3Schema.Columns.Add(new DatabaseColumn("StayDate", DAT3Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.AnsiString,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	DAT3Schema.Columns.Add(new DatabaseColumn("MarketCategory", DAT3Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.AnsiString,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	DAT3Schema.Columns.Add(new DatabaseColumn("RateProgramTier", DAT3Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.AnsiString,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	DAT3Schema.Columns.Add(new DatabaseColumn("RateCategoryCode", DAT3Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.AnsiString,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	DAT3Schema.Columns.Add(new DatabaseColumn("MarketCode", DAT3Schema)
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
            	DAT3Schema.Columns.Add(new DatabaseColumn("ADRNet", DAT3Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Currency,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	DAT3Schema.Columns.Add(new DatabaseColumn("RevenueNet", DAT3Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Currency,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	DAT3Schema.Columns.Add(new DatabaseColumn("AdditionalDemand", DAT3Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	DAT3Schema.Columns.Add(new DatabaseColumn("TotalDemand", DAT3Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	DAT3Schema.Columns.Add(new DatabaseColumn("AverageRoomNights", DAT3Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Currency,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	DAT3Schema.Columns.Add(new DatabaseColumn("AverageRevenue", DAT3Schema)
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
				
				// Table: MicrosTimePeriods
				// Primary Key: 
				ITable MicrosTimePeriodsSchema = new DatabaseTable("MicrosTimePeriods", DataProvider) { ClassName = "MicrosTimePeriod", SchemaName = "dbo" };
            	MicrosTimePeriodsSchema.Columns.Add(new DatabaseColumn("TimePeriodID", MicrosTimePeriodsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	MicrosTimePeriodsSchema.Columns.Add(new DatabaseColumn("TimePeriodName", MicrosTimePeriodsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
				// Add MicrosTimePeriods to schema
            	DataProvider.Schema.Tables.Add(MicrosTimePeriodsSchema);
				
				// Table: POSEstablisments
				// Primary Key: 
				ITable POSEstablismentsSchema = new DatabaseTable("POSEstablisments", DataProvider) { ClassName = "POSEstablisment", SchemaName = "dbo" };
            	POSEstablismentsSchema.Columns.Add(new DatabaseColumn("EstablishmentID", POSEstablismentsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Guid,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	POSEstablismentsSchema.Columns.Add(new DatabaseColumn("Establishment", POSEstablismentsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	POSEstablismentsSchema.Columns.Add(new DatabaseColumn("EstablishmentNumber", POSEstablismentsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
				// Add POSEstablisments to schema
            	DataProvider.Schema.Tables.Add(POSEstablismentsSchema);
				
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
            	POSTicketsSchema.Columns.Add(new DatabaseColumn("TransactionType", POSTicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int64,
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
				
				// Table: SageBalance
				// Primary Key: 
				ITable SageBalanceSchema = new DatabaseTable("SageBalance", DataProvider) { ClassName = "SageBalance", SchemaName = "dbo" };
            	SageBalanceSchema.Columns.Add(new DatabaseColumn("AccountCode", SageBalanceSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int64,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	SageBalanceSchema.Columns.Add(new DatabaseColumn("Category", SageBalanceSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	SageBalanceSchema.Columns.Add(new DatabaseColumn("CategoryName", SageBalanceSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	SageBalanceSchema.Columns.Add(new DatabaseColumn("CategoryNameEng", SageBalanceSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	SageBalanceSchema.Columns.Add(new DatabaseColumn("Debit", SageBalanceSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Currency,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	SageBalanceSchema.Columns.Add(new DatabaseColumn("Credit", SageBalanceSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Currency,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	SageBalanceSchema.Columns.Add(new DatabaseColumn("SalesRecorded", SageBalanceSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Currency,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	SageBalanceSchema.Columns.Add(new DatabaseColumn("AccountingPeriod", SageBalanceSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.AnsiString,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	SageBalanceSchema.Columns.Add(new DatabaseColumn("DataSource", SageBalanceSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
				// Add SageBalance to schema
            	DataProvider.Schema.Tables.Add(SageBalanceSchema);
				
				// Table: vwCategoryDetails
				// Primary Key: 
				ITable vwCategoryDetailsSchema = new DatabaseTable("vwCategoryDetails", DataProvider) { ClassName = "vwCategoryDetail", SchemaName = "dbo" };
            	vwCategoryDetailsSchema.Columns.Add(new DatabaseColumn("BUSINESS_DATE", vwCategoryDetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.AnsiString,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwCategoryDetailsSchema.Columns.Add(new DatabaseColumn("Category", vwCategoryDetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.AnsiString,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwCategoryDetailsSchema.Columns.Add(new DatabaseColumn("CategoryID", vwCategoryDetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwCategoryDetailsSchema.Columns.Add(new DatabaseColumn("CASHIER_DEBIT", vwCategoryDetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwCategoryDetailsSchema.Columns.Add(new DatabaseColumn("CASHIER_CREDIT", vwCategoryDetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwCategoryDetailsSchema.Columns.Add(new DatabaseColumn("GUEST_FULL_NAME", vwCategoryDetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwCategoryDetailsSchema.Columns.Add(new DatabaseColumn("TRX_CODE", vwCategoryDetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwCategoryDetailsSchema.Columns.Add(new DatabaseColumn("TRX_DESC", vwCategoryDetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwCategoryDetailsSchema.Columns.Add(new DatabaseColumn("BUSINESS_TIME", vwCategoryDetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.AnsiString,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwCategoryDetailsSchema.Columns.Add(new DatabaseColumn("REFERENCE", vwCategoryDetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwCategoryDetailsSchema.Columns.Add(new DatabaseColumn("ROOM", vwCategoryDetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwCategoryDetailsSchema.Columns.Add(new DatabaseColumn("ROOM_CLASS", vwCategoryDetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwCategoryDetailsSchema.Columns.Add(new DatabaseColumn("CASHIER_NAME", vwCategoryDetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwCategoryDetailsSchema.Columns.Add(new DatabaseColumn("CASH_ID_USER_NAME", vwCategoryDetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwCategoryDetailsSchema.Columns.Add(new DatabaseColumn("PRINT_CASHIER_CREDIT", vwCategoryDetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwCategoryDetailsSchema.Columns.Add(new DatabaseColumn("PRINT_CASHIER_DEBIT", vwCategoryDetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
				// Add vwCategoryDetails to schema
            	DataProvider.Schema.Tables.Add(vwCategoryDetailsSchema);
				
				// Table: vwCategorySummaryByMonth
				// Primary Key: 
				ITable vwCategorySummaryByMonthSchema = new DatabaseTable("vwCategorySummaryByMonth", DataProvider) { ClassName = "vwCategorySummaryByMonth", SchemaName = "dbo" };
            	vwCategorySummaryByMonthSchema.Columns.Add(new DatabaseColumn("MONTH", vwCategorySummaryByMonthSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwCategorySummaryByMonthSchema.Columns.Add(new DatabaseColumn("Room", vwCategorySummaryByMonthSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwCategorySummaryByMonthSchema.Columns.Add(new DatabaseColumn("Tax", vwCategorySummaryByMonthSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwCategorySummaryByMonthSchema.Columns.Add(new DatabaseColumn("MarriotRewardsAndOther", vwCategorySummaryByMonthSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwCategorySummaryByMonthSchema.Columns.Add(new DatabaseColumn("Breakfast", vwCategorySummaryByMonthSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwCategorySummaryByMonthSchema.Columns.Add(new DatabaseColumn("OtherFood", vwCategorySummaryByMonthSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwCategorySummaryByMonthSchema.Columns.Add(new DatabaseColumn("FBPackageLost", vwCategorySummaryByMonthSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwCategorySummaryByMonthSchema.Columns.Add(new DatabaseColumn("FBPackageProfit", vwCategorySummaryByMonthSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwCategorySummaryByMonthSchema.Columns.Add(new DatabaseColumn("OtherPackageLoss", vwCategorySummaryByMonthSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwCategorySummaryByMonthSchema.Columns.Add(new DatabaseColumn("OtherPackageProfit", vwCategorySummaryByMonthSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwCategorySummaryByMonthSchema.Columns.Add(new DatabaseColumn("Minibar", vwCategorySummaryByMonthSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwCategorySummaryByMonthSchema.Columns.Add(new DatabaseColumn("Banquet", vwCategorySummaryByMonthSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwCategorySummaryByMonthSchema.Columns.Add(new DatabaseColumn("BanquetOther", vwCategorySummaryByMonthSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwCategorySummaryByMonthSchema.Columns.Add(new DatabaseColumn("Misc", vwCategorySummaryByMonthSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwCategorySummaryByMonthSchema.Columns.Add(new DatabaseColumn("Total", vwCategorySummaryByMonthSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
				// Add vwCategorySummaryByMonth to schema
            	DataProvider.Schema.Tables.Add(vwCategorySummaryByMonthSchema);
				
				// Table: vwCheckFoodGroups
				// Primary Key: 
				ITable vwCheckFoodGroupsSchema = new DatabaseTable("vwCheckFoodGroups", DataProvider) { ClassName = "vwCheckFoodGroup", SchemaName = "dbo" };
            	vwCheckFoodGroupsSchema.Columns.Add(new DatabaseColumn("Check_Seq", vwCheckFoodGroupsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwCheckFoodGroupsSchema.Columns.Add(new DatabaseColumn("Check_num", vwCheckFoodGroupsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwCheckFoodGroupsSchema.Columns.Add(new DatabaseColumn("Cover_Cnt", vwCheckFoodGroupsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwCheckFoodGroupsSchema.Columns.Add(new DatabaseColumn("sub_ttl", vwCheckFoodGroupsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwCheckFoodGroupsSchema.Columns.Add(new DatabaseColumn("EntreeCount", vwCheckFoodGroupsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwCheckFoodGroupsSchema.Columns.Add(new DatabaseColumn("PlatCount", vwCheckFoodGroupsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwCheckFoodGroupsSchema.Columns.Add(new DatabaseColumn("DessertCount", vwCheckFoodGroupsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
				// Add vwCheckFoodGroups to schema
            	DataProvider.Schema.Tables.Add(vwCheckFoodGroupsSchema);
				
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
													IsNullable = true,
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
            	vwDailyPOSTicketItemsSchema.Columns.Add(new DatabaseColumn("POSTicketID", vwDailyPOSTicketItemsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Guid,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDailyPOSTicketItemsSchema.Columns.Add(new DatabaseColumn("CheckNumber", vwDailyPOSTicketItemsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.AnsiString,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDailyPOSTicketItemsSchema.Columns.Add(new DatabaseColumn("GuestCountModified", vwDailyPOSTicketItemsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDailyPOSTicketItemsSchema.Columns.Add(new DatabaseColumn("TicketDate", vwDailyPOSTicketItemsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDailyPOSTicketItemsSchema.Columns.Add(new DatabaseColumn("Establishment", vwDailyPOSTicketItemsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
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
													IsNullable = true,
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
            	vwDailyPOSTicketItemsSchema.Columns.Add(new DatabaseColumn("Dtl_Typ_Cnt", vwDailyPOSTicketItemsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDailyPOSTicketItemsSchema.Columns.Add(new DatabaseColumn("RecordType", vwDailyPOSTicketItemsSchema)
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
            	vwDailyPOSTicketsSchema.Columns.Add(new DatabaseColumn("Establishment", vwDailyPOSTicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDailyPOSTicketsSchema.Columns.Add(new DatabaseColumn("CheckNumber", vwDailyPOSTicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.AnsiString,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDailyPOSTicketsSchema.Columns.Add(new DatabaseColumn("TicketDate", vwDailyPOSTicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDailyPOSTicketsSchema.Columns.Add(new DatabaseColumn("Server", vwDailyPOSTicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
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
													IsNullable = true,
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
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDailyPOSTicketsSchema.Columns.Add(new DatabaseColumn("GuestCountModified", vwDailyPOSTicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDailyPOSTicketsSchema.Columns.Add(new DatabaseColumn("TicketTotal", vwDailyPOSTicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDailyPOSTicketsSchema.Columns.Add(new DatabaseColumn("PaymentTotal", vwDailyPOSTicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDailyPOSTicketsSchema.Columns.Add(new DatabaseColumn("EstablishmentModified", vwDailyPOSTicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDailyPOSTicketsSchema.Columns.Add(new DatabaseColumn("POSTicketID", vwDailyPOSTicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Guid,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDailyPOSTicketsSchema.Columns.Add(new DatabaseColumn("EntreeCount", vwDailyPOSTicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDailyPOSTicketsSchema.Columns.Add(new DatabaseColumn("PlatCount", vwDailyPOSTicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDailyPOSTicketsSchema.Columns.Add(new DatabaseColumn("DessertCount", vwDailyPOSTicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDailyPOSTicketsSchema.Columns.Add(new DatabaseColumn("RecordType", vwDailyPOSTicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDailyPOSTicketsSchema.Columns.Add(new DatabaseColumn("TransactionType", vwDailyPOSTicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int64,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDailyPOSTicketsSchema.Columns.Add(new DatabaseColumn("DiscountTotal", vwDailyPOSTicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDailyPOSTicketsSchema.Columns.Add(new DatabaseColumn("TicketPeriod", vwDailyPOSTicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.AnsiString,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDailyPOSTicketsSchema.Columns.Add(new DatabaseColumn("EstablishmentNumber", vwDailyPOSTicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDailyPOSTicketsSchema.Columns.Add(new DatabaseColumn("BusinessDate", vwDailyPOSTicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.AnsiString,
													IsNullable = true,
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
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDailyTicketDataSummarySchema.Columns.Add(new DatabaseColumn("TransactionType", vwDailyTicketDataSummarySchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int64,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDailyTicketDataSummarySchema.Columns.Add(new DatabaseColumn("TotalTicket", vwDailyTicketDataSummarySchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDailyTicketDataSummarySchema.Columns.Add(new DatabaseColumn("TotalPayments", vwDailyTicketDataSummarySchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDailyTicketDataSummarySchema.Columns.Add(new DatabaseColumn("TotalTips", vwDailyTicketDataSummarySchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDailyTicketDataSummarySchema.Columns.Add(new DatabaseColumn("TotalTickets", vwDailyTicketDataSummarySchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDailyTicketDataSummarySchema.Columns.Add(new DatabaseColumn("TotalGuests", vwDailyTicketDataSummarySchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDailyTicketDataSummarySchema.Columns.Add(new DatabaseColumn("TotalDiscounts", vwDailyTicketDataSummarySchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDailyTicketDataSummarySchema.Columns.Add(new DatabaseColumn("MinibarLost", vwDailyTicketDataSummarySchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDailyTicketDataSummarySchema.Columns.Add(new DatabaseColumn("Amenitites", vwDailyTicketDataSummarySchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
				// Add vwDailyTicketDataSummary to schema
            	DataProvider.Schema.Tables.Add(vwDailyTicketDataSummarySchema);
				
				// Table: vwDiscountByTicket
				// Primary Key: 
				ITable vwDiscountByTicketSchema = new DatabaseTable("vwDiscountByTicket", DataProvider) { ClassName = "vwDiscountByTicket", SchemaName = "dbo" };
            	vwDiscountByTicketSchema.Columns.Add(new DatabaseColumn("TicketDate", vwDiscountByTicketSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.AnsiString,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDiscountByTicketSchema.Columns.Add(new DatabaseColumn("CheckNumber", vwDiscountByTicketSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDiscountByTicketSchema.Columns.Add(new DatabaseColumn("DiscountTotal", vwDiscountByTicketSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
				// Add vwDiscountByTicket to schema
            	DataProvider.Schema.Tables.Add(vwDiscountByTicketSchema);
				
				// Table: vwMissingTickets
				// Primary Key: 
				ITable vwMissingTicketsSchema = new DatabaseTable("vwMissingTickets", DataProvider) { ClassName = "vwMissingTicket", SchemaName = "dbo" };
            	vwMissingTicketsSchema.Columns.Add(new DatabaseColumn("BusinessDate", vwMissingTicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.AnsiString,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwMissingTicketsSchema.Columns.Add(new DatabaseColumn("MicrosCheckNumber", vwMissingTicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwMissingTicketsSchema.Columns.Add(new DatabaseColumn("CheckAmount", vwMissingTicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwMissingTicketsSchema.Columns.Add(new DatabaseColumn("EstablishmentNumber", vwMissingTicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwMissingTicketsSchema.Columns.Add(new DatabaseColumn("chk_num", vwMissingTicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwMissingTicketsSchema.Columns.Add(new DatabaseColumn("sub_ttl", vwMissingTicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwMissingTicketsSchema.Columns.Add(new DatabaseColumn("pymnt_ttl", vwMissingTicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwMissingTicketsSchema.Columns.Add(new DatabaseColumn("TicketDifference", vwMissingTicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
				// Add vwMissingTickets to schema
            	DataProvider.Schema.Tables.Add(vwMissingTicketsSchema);
				
				// Table: vwMRSBRFinancial
				// Primary Key: 
				ITable vwMRSBRFinancialSchema = new DatabaseTable("vwMRSBRFinancial", DataProvider) { ClassName = "vwMRSBRFinancial", SchemaName = "dbo" };
            	vwMRSBRFinancialSchema.Columns.Add(new DatabaseColumn("TicketDate", vwMRSBRFinancialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwMRSBRFinancialSchema.Columns.Add(new DatabaseColumn("BusinessDate", vwMRSBRFinancialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwMRSBRFinancialSchema.Columns.Add(new DatabaseColumn("AdjustmentReasonCode", vwMRSBRFinancialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwMRSBRFinancialSchema.Columns.Add(new DatabaseColumn("AdjustmentYN", vwMRSBRFinancialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwMRSBRFinancialSchema.Columns.Add(new DatabaseColumn("AmountPosted", vwMRSBRFinancialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwMRSBRFinancialSchema.Columns.Add(new DatabaseColumn("ARCompressedYN", vwMRSBRFinancialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwMRSBRFinancialSchema.Columns.Add(new DatabaseColumn("ARCreditAmount", vwMRSBRFinancialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwMRSBRFinancialSchema.Columns.Add(new DatabaseColumn("ARDebitAmount", vwMRSBRFinancialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwMRSBRFinancialSchema.Columns.Add(new DatabaseColumn("ARInvoiceNumber", vwMRSBRFinancialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwMRSBRFinancialSchema.Columns.Add(new DatabaseColumn("ARStatus", vwMRSBRFinancialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwMRSBRFinancialSchema.Columns.Add(new DatabaseColumn("ARTransferDate", vwMRSBRFinancialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwMRSBRFinancialSchema.Columns.Add(new DatabaseColumn("ArrangementCode", vwMRSBRFinancialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwMRSBRFinancialSchema.Columns.Add(new DatabaseColumn("ArrangementDescription", vwMRSBRFinancialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwMRSBRFinancialSchema.Columns.Add(new DatabaseColumn("BillNo", vwMRSBRFinancialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwMRSBRFinancialSchema.Columns.Add(new DatabaseColumn("CashierID", vwMRSBRFinancialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwMRSBRFinancialSchema.Columns.Add(new DatabaseColumn("CheckNumber", vwMRSBRFinancialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwMRSBRFinancialSchema.Columns.Add(new DatabaseColumn("CurrencyCode", vwMRSBRFinancialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwMRSBRFinancialSchema.Columns.Add(new DatabaseColumn("DepositLedgerCredit", vwMRSBRFinancialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwMRSBRFinancialSchema.Columns.Add(new DatabaseColumn("DepositLedgerDebit", vwMRSBRFinancialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwMRSBRFinancialSchema.Columns.Add(new DatabaseColumn("DisplayYN", vwMRSBRFinancialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwMRSBRFinancialSchema.Columns.Add(new DatabaseColumn("ExchangeRateExcludingCommission", vwMRSBRFinancialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwMRSBRFinancialSchema.Columns.Add(new DatabaseColumn("ExchangeRateForTransaction", vwMRSBRFinancialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwMRSBRFinancialSchema.Columns.Add(new DatabaseColumn("FixedChargesYN", vwMRSBRFinancialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwMRSBRFinancialSchema.Columns.Add(new DatabaseColumn("FolioName", vwMRSBRFinancialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwMRSBRFinancialSchema.Columns.Add(new DatabaseColumn("FolioWindow", vwMRSBRFinancialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwMRSBRFinancialSchema.Columns.Add(new DatabaseColumn("FoodTax", vwMRSBRFinancialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwMRSBRFinancialSchema.Columns.Add(new DatabaseColumn("ForeignExchangeCommissionAmount", vwMRSBRFinancialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwMRSBRFinancialSchema.Columns.Add(new DatabaseColumn("ForeignExchangeCommissionPercent", vwMRSBRFinancialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwMRSBRFinancialSchema.Columns.Add(new DatabaseColumn("ForeignExchangeType", vwMRSBRFinancialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwMRSBRFinancialSchema.Columns.Add(new DatabaseColumn("Generates", vwMRSBRFinancialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwMRSBRFinancialSchema.Columns.Add(new DatabaseColumn("GrossAmount", vwMRSBRFinancialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwMRSBRFinancialSchema.Columns.Add(new DatabaseColumn("GuestAccountCredit", vwMRSBRFinancialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwMRSBRFinancialSchema.Columns.Add(new DatabaseColumn("GuestAccountDebit", vwMRSBRFinancialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwMRSBRFinancialSchema.Columns.Add(new DatabaseColumn("InsertDate", vwMRSBRFinancialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwMRSBRFinancialSchema.Columns.Add(new DatabaseColumn("InsertUser", vwMRSBRFinancialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwMRSBRFinancialSchema.Columns.Add(new DatabaseColumn("InternalYN", vwMRSBRFinancialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwMRSBRFinancialSchema.Columns.Add(new DatabaseColumn("InvoiceClosingDate", vwMRSBRFinancialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwMRSBRFinancialSchema.Columns.Add(new DatabaseColumn("InvoiceType", vwMRSBRFinancialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwMRSBRFinancialSchema.Columns.Add(new DatabaseColumn("MarketCode", vwMRSBRFinancialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwMRSBRFinancialSchema.Columns.Add(new DatabaseColumn("MinibarTax", vwMRSBRFinancialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwMRSBRFinancialSchema.Columns.Add(new DatabaseColumn("NetAmount", vwMRSBRFinancialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwMRSBRFinancialSchema.Columns.Add(new DatabaseColumn("OtherTax", vwMRSBRFinancialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwMRSBRFinancialSchema.Columns.Add(new DatabaseColumn("PackageAccountCredit", vwMRSBRFinancialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwMRSBRFinancialSchema.Columns.Add(new DatabaseColumn("PackageAccountDebit", vwMRSBRFinancialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwMRSBRFinancialSchema.Columns.Add(new DatabaseColumn("ParrallelCurrencyCode", vwMRSBRFinancialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwMRSBRFinancialSchema.Columns.Add(new DatabaseColumn("Price", vwMRSBRFinancialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwMRSBRFinancialSchema.Columns.Add(new DatabaseColumn("Product", vwMRSBRFinancialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwMRSBRFinancialSchema.Columns.Add(new DatabaseColumn("Property", vwMRSBRFinancialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwMRSBRFinancialSchema.Columns.Add(new DatabaseColumn("QuantityOfProduct", vwMRSBRFinancialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwMRSBRFinancialSchema.Columns.Add(new DatabaseColumn("RateCode", vwMRSBRFinancialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwMRSBRFinancialSchema.Columns.Add(new DatabaseColumn("ReceiptNumber", vwMRSBRFinancialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwMRSBRFinancialSchema.Columns.Add(new DatabaseColumn("Reference", vwMRSBRFinancialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwMRSBRFinancialSchema.Columns.Add(new DatabaseColumn("Remark", vwMRSBRFinancialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwMRSBRFinancialSchema.Columns.Add(new DatabaseColumn("RevenueAmount", vwMRSBRFinancialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwMRSBRFinancialSchema.Columns.Add(new DatabaseColumn("RoomClass", vwMRSBRFinancialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwMRSBRFinancialSchema.Columns.Add(new DatabaseColumn("RoomNumber", vwMRSBRFinancialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwMRSBRFinancialSchema.Columns.Add(new DatabaseColumn("RoomTax", vwMRSBRFinancialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwMRSBRFinancialSchema.Columns.Add(new DatabaseColumn("SourceCode", vwMRSBRFinancialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwMRSBRFinancialSchema.Columns.Add(new DatabaseColumn("SummaryReferenceCode", vwMRSBRFinancialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwMRSBRFinancialSchema.Columns.Add(new DatabaseColumn("TACommissionable", vwMRSBRFinancialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwMRSBRFinancialSchema.Columns.Add(new DatabaseColumn("TaxDeferredYN", vwMRSBRFinancialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwMRSBRFinancialSchema.Columns.Add(new DatabaseColumn("TaxGeneratedYN", vwMRSBRFinancialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwMRSBRFinancialSchema.Columns.Add(new DatabaseColumn("TaxInclusiveYN", vwMRSBRFinancialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwMRSBRFinancialSchema.Columns.Add(new DatabaseColumn("TransactionAmount", vwMRSBRFinancialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwMRSBRFinancialSchema.Columns.Add(new DatabaseColumn("TransactionCode", vwMRSBRFinancialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwMRSBRFinancialSchema.Columns.Add(new DatabaseColumn("TransactionCodeDescription", vwMRSBRFinancialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwMRSBRFinancialSchema.Columns.Add(new DatabaseColumn("TransactionCodeGroup", vwMRSBRFinancialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwMRSBRFinancialSchema.Columns.Add(new DatabaseColumn("TransactionCodeSubgroup", vwMRSBRFinancialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwMRSBRFinancialSchema.Columns.Add(new DatabaseColumn("TransactionDate", vwMRSBRFinancialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwMRSBRFinancialSchema.Columns.Add(new DatabaseColumn("TransactionGroupType", vwMRSBRFinancialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwMRSBRFinancialSchema.Columns.Add(new DatabaseColumn("TransactionType", vwMRSBRFinancialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwMRSBRFinancialSchema.Columns.Add(new DatabaseColumn("TransferDate", vwMRSBRFinancialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwMRSBRFinancialSchema.Columns.Add(new DatabaseColumn("GrossAmount2", vwMRSBRFinancialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwMRSBRFinancialSchema.Columns.Add(new DatabaseColumn("NetAmount2", vwMRSBRFinancialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwMRSBRFinancialSchema.Columns.Add(new DatabaseColumn("UpdateDate", vwMRSBRFinancialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwMRSBRFinancialSchema.Columns.Add(new DatabaseColumn("UpdateUser", vwMRSBRFinancialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwMRSBRFinancialSchema.Columns.Add(new DatabaseColumn("mrsbr_financialID", vwMRSBRFinancialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Guid,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwMRSBRFinancialSchema.Columns.Add(new DatabaseColumn("DataSource", vwMRSBRFinancialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
				// Add vwMRSBRFinancial to schema
            	DataProvider.Schema.Tables.Add(vwMRSBRFinancialSchema);
				
				// Table: vwOperaMicrosChecks
				// Primary Key: 
				ITable vwOperaMicrosChecksSchema = new DatabaseTable("vwOperaMicrosChecks", DataProvider) { ClassName = "vwOperaMicrosCheck", SchemaName = "dbo" };
            	vwOperaMicrosChecksSchema.Columns.Add(new DatabaseColumn("BusinessDate", vwOperaMicrosChecksSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.AnsiString,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwOperaMicrosChecksSchema.Columns.Add(new DatabaseColumn("MicrosCheckNumber", vwOperaMicrosChecksSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwOperaMicrosChecksSchema.Columns.Add(new DatabaseColumn("CheckAmount", vwOperaMicrosChecksSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwOperaMicrosChecksSchema.Columns.Add(new DatabaseColumn("EstablishmentNumber", vwOperaMicrosChecksSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
				// Add vwOperaMicrosChecks to schema
            	DataProvider.Schema.Tables.Add(vwOperaMicrosChecksSchema);
				
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
													DataType = DbType.AnsiString,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwPOSTicketItemsSchema.Columns.Add(new DatabaseColumn("GuestCountModified", vwPOSTicketItemsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwPOSTicketItemsSchema.Columns.Add(new DatabaseColumn("TicketDate", vwPOSTicketItemsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwPOSTicketItemsSchema.Columns.Add(new DatabaseColumn("Establishment", vwPOSTicketItemsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
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
													IsNullable = true,
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
            	vwPOSTicketItemsSchema.Columns.Add(new DatabaseColumn("Dtl_Typ_Cnt", vwPOSTicketItemsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwPOSTicketItemsSchema.Columns.Add(new DatabaseColumn("RecordType", vwPOSTicketItemsSchema)
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
            	vwPOSTicketsSchema.Columns.Add(new DatabaseColumn("Establishment", vwPOSTicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwPOSTicketsSchema.Columns.Add(new DatabaseColumn("CheckNumber", vwPOSTicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.AnsiString,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwPOSTicketsSchema.Columns.Add(new DatabaseColumn("TicketDate", vwPOSTicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwPOSTicketsSchema.Columns.Add(new DatabaseColumn("Server", vwPOSTicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
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
													IsNullable = true,
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
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwPOSTicketsSchema.Columns.Add(new DatabaseColumn("GuestCountModified", vwPOSTicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwPOSTicketsSchema.Columns.Add(new DatabaseColumn("TicketTotal", vwPOSTicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwPOSTicketsSchema.Columns.Add(new DatabaseColumn("PaymentTotal", vwPOSTicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwPOSTicketsSchema.Columns.Add(new DatabaseColumn("EstablishmentModified", vwPOSTicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwPOSTicketsSchema.Columns.Add(new DatabaseColumn("POSTicketID", vwPOSTicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Guid,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwPOSTicketsSchema.Columns.Add(new DatabaseColumn("EntreeCount", vwPOSTicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwPOSTicketsSchema.Columns.Add(new DatabaseColumn("PlatCount", vwPOSTicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwPOSTicketsSchema.Columns.Add(new DatabaseColumn("DessertCount", vwPOSTicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwPOSTicketsSchema.Columns.Add(new DatabaseColumn("RecordType", vwPOSTicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwPOSTicketsSchema.Columns.Add(new DatabaseColumn("TransactionType", vwPOSTicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int64,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwPOSTicketsSchema.Columns.Add(new DatabaseColumn("DiscountTotal", vwPOSTicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwPOSTicketsSchema.Columns.Add(new DatabaseColumn("TicketPeriod", vwPOSTicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.AnsiString,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwPOSTicketsSchema.Columns.Add(new DatabaseColumn("EstablishmentNumber", vwPOSTicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwPOSTicketsSchema.Columns.Add(new DatabaseColumn("BusinessDate", vwPOSTicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.AnsiString,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
				// Add vwPOSTickets to schema
            	DataProvider.Schema.Tables.Add(vwPOSTicketsSchema);
				
				// Table: vwSageCategoryTotals
				// Primary Key: 
				ITable vwSageCategoryTotalsSchema = new DatabaseTable("vwSageCategoryTotals", DataProvider) { ClassName = "vwSageCategoryTotal", SchemaName = "dbo" };
            	vwSageCategoryTotalsSchema.Columns.Add(new DatabaseColumn("Category", vwSageCategoryTotalsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwSageCategoryTotalsSchema.Columns.Add(new DatabaseColumn("DebitTotal", vwSageCategoryTotalsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Currency,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwSageCategoryTotalsSchema.Columns.Add(new DatabaseColumn("CreditTotal", vwSageCategoryTotalsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Currency,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwSageCategoryTotalsSchema.Columns.Add(new DatabaseColumn("SalesTotal", vwSageCategoryTotalsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Currency,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
				// Add vwSageCategoryTotals to schema
            	DataProvider.Schema.Tables.Add(vwSageCategoryTotalsSchema);
				
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
				
				// Table: banquet_data
				// Primary Key: banquet_dataID
				ITable banquet_dataSchema = new DatabaseTable("banquet_data", DataProvider) { ClassName = "banquet_datum", SchemaName = "imp" };
            	banquet_dataSchema.Columns.Add(new DatabaseColumn("ActualAttendees", banquet_dataSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	banquet_dataSchema.Columns.Add(new DatabaseColumn("ActualManual", banquet_dataSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	banquet_dataSchema.Columns.Add(new DatabaseColumn("ActualRevenue", banquet_dataSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	banquet_dataSchema.Columns.Add(new DatabaseColumn("Attendees", banquet_dataSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	banquet_dataSchema.Columns.Add(new DatabaseColumn("BilledReveneu", banquet_dataSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	banquet_dataSchema.Columns.Add(new DatabaseColumn("BusinessBlockID", banquet_dataSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	banquet_dataSchema.Columns.Add(new DatabaseColumn("BusinessBlockProprety", banquet_dataSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	banquet_dataSchema.Columns.Add(new DatabaseColumn("DiscountPercentage", banquet_dataSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	banquet_dataSchema.Columns.Add(new DatabaseColumn("DisplayDoorcardYN", banquet_dataSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	banquet_dataSchema.Columns.Add(new DatabaseColumn("DoNotMoveYN", banquet_dataSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	banquet_dataSchema.Columns.Add(new DatabaseColumn("Doorcard", banquet_dataSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	banquet_dataSchema.Columns.Add(new DatabaseColumn("EndDate", banquet_dataSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	banquet_dataSchema.Columns.Add(new DatabaseColumn("EndTime", banquet_dataSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	banquet_dataSchema.Columns.Add(new DatabaseColumn("EventID", banquet_dataSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	banquet_dataSchema.Columns.Add(new DatabaseColumn("EventName", banquet_dataSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	banquet_dataSchema.Columns.Add(new DatabaseColumn("EventProperty", banquet_dataSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	banquet_dataSchema.Columns.Add(new DatabaseColumn("Eventime", banquet_dataSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	banquet_dataSchema.Columns.Add(new DatabaseColumn("EventType", banquet_dataSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	banquet_dataSchema.Columns.Add(new DatabaseColumn("ExcludeFromForecastYN", banquet_dataSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	banquet_dataSchema.Columns.Add(new DatabaseColumn("ExpectedRevenue", banquet_dataSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	banquet_dataSchema.Columns.Add(new DatabaseColumn("ForecastRevenueOnlyYN", banquet_dataSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	banquet_dataSchema.Columns.Add(new DatabaseColumn("ForecastedRevenue", banquet_dataSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	banquet_dataSchema.Columns.Add(new DatabaseColumn("FunctionSpace", banquet_dataSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	banquet_dataSchema.Columns.Add(new DatabaseColumn("FunctionSpaceCode", banquet_dataSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	banquet_dataSchema.Columns.Add(new DatabaseColumn("GuaranteedRevenue", banquet_dataSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	banquet_dataSchema.Columns.Add(new DatabaseColumn("LoudEventYN", banquet_dataSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	banquet_dataSchema.Columns.Add(new DatabaseColumn("MasterEventID", banquet_dataSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	banquet_dataSchema.Columns.Add(new DatabaseColumn("MinimumGuaranteed", banquet_dataSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	banquet_dataSchema.Columns.Add(new DatabaseColumn("MinimumSetdownTime", banquet_dataSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	banquet_dataSchema.Columns.Add(new DatabaseColumn("MinimumSetupTime", banquet_dataSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	banquet_dataSchema.Columns.Add(new DatabaseColumn("PackageCode", banquet_dataSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	banquet_dataSchema.Columns.Add(new DatabaseColumn("PackageDescription", banquet_dataSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	banquet_dataSchema.Columns.Add(new DatabaseColumn("RateAmount", banquet_dataSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	banquet_dataSchema.Columns.Add(new DatabaseColumn("RateCode", banquet_dataSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	banquet_dataSchema.Columns.Add(new DatabaseColumn("RegisteredAttendees", banquet_dataSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	banquet_dataSchema.Columns.Add(new DatabaseColumn("RegistrationRequiredYN", banquet_dataSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	banquet_dataSchema.Columns.Add(new DatabaseColumn("RoomSetupStyle", banquet_dataSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	banquet_dataSchema.Columns.Add(new DatabaseColumn("SetAttendees", banquet_dataSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	banquet_dataSchema.Columns.Add(new DatabaseColumn("StartDate", banquet_dataSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	banquet_dataSchema.Columns.Add(new DatabaseColumn("StartTime", banquet_dataSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	banquet_dataSchema.Columns.Add(new DatabaseColumn("Status", banquet_dataSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	banquet_dataSchema.Columns.Add(new DatabaseColumn("TotalEventTime", banquet_dataSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	banquet_dataSchema.Columns.Add(new DatabaseColumn("banquet_dataID", banquet_dataSchema)
												{
													IsPrimaryKey = true,
													DataType = DbType.Guid,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	banquet_dataSchema.Columns.Add(new DatabaseColumn("DataSource", banquet_dataSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
				// Add banquet_data to schema
            	DataProvider.Schema.Tables.Add(banquet_dataSchema);
				
				// Table: business_blocks
				// Primary Key: business_blocksID
				ITable business_blocksSchema = new DatabaseTable("business_blocks", DataProvider) { ClassName = "business_block", SchemaName = "imp" };
            	business_blocksSchema.Columns.Add(new DatabaseColumn("ActualNumberOfRooms", business_blocksSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	business_blocksSchema.Columns.Add(new DatabaseColumn("ActualRoomRevenue", business_blocksSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	business_blocksSchema.Columns.Add(new DatabaseColumn("ActualAverageRate", business_blocksSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	business_blocksSchema.Columns.Add(new DatabaseColumn("Attendees", business_blocksSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	business_blocksSchema.Columns.Add(new DatabaseColumn("AvgRate", business_blocksSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	business_blocksSchema.Columns.Add(new DatabaseColumn("BlockCode", business_blocksSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	business_blocksSchema.Columns.Add(new DatabaseColumn("BookingType", business_blocksSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	business_blocksSchema.Columns.Add(new DatabaseColumn("BreakfastIncludedFlag", business_blocksSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	business_blocksSchema.Columns.Add(new DatabaseColumn("BreakfastPrice", business_blocksSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	business_blocksSchema.Columns.Add(new DatabaseColumn("BusinessBlockID", business_blocksSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	business_blocksSchema.Columns.Add(new DatabaseColumn("CateringRevenue", business_blocksSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	business_blocksSchema.Columns.Add(new DatabaseColumn("CateringStatus", business_blocksSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	business_blocksSchema.Columns.Add(new DatabaseColumn("CompanyName", business_blocksSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	business_blocksSchema.Columns.Add(new DatabaseColumn("CompanyID", business_blocksSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	business_blocksSchema.Columns.Add(new DatabaseColumn("ComplimentaryRooms", business_blocksSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	business_blocksSchema.Columns.Add(new DatabaseColumn("ComplimentaryRoomsValue", business_blocksSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	business_blocksSchema.Columns.Add(new DatabaseColumn("ContractNumber", business_blocksSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	business_blocksSchema.Columns.Add(new DatabaseColumn("CreationDate", business_blocksSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	business_blocksSchema.Columns.Add(new DatabaseColumn("EndDate", business_blocksSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	business_blocksSchema.Columns.Add(new DatabaseColumn("RateCode", business_blocksSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	business_blocksSchema.Columns.Add(new DatabaseColumn("ReservationType", business_blocksSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	business_blocksSchema.Columns.Add(new DatabaseColumn("TotalRevenue", business_blocksSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	business_blocksSchema.Columns.Add(new DatabaseColumn("TotalComplimentaryRooms", business_blocksSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	business_blocksSchema.Columns.Add(new DatabaseColumn("UpdateDate", business_blocksSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	business_blocksSchema.Columns.Add(new DatabaseColumn("business_blocksID", business_blocksSchema)
												{
													IsPrimaryKey = true,
													DataType = DbType.Guid,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	business_blocksSchema.Columns.Add(new DatabaseColumn("DataSource", business_blocksSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
				// Add business_blocks to schema
            	DataProvider.Schema.Tables.Add(business_blocksSchema);
				
				// Table: dly_corr_ttl
				// Primary Key: dly_corr_ttlID
				ITable dly_corr_ttlSchema = new DatabaseTable("dly_corr_ttl", DataProvider) { ClassName = "dly_corr_ttl", SchemaName = "imp" };
            	dly_corr_ttlSchema.Columns.Add(new DatabaseColumn("store_id", dly_corr_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	dly_corr_ttlSchema.Columns.Add(new DatabaseColumn("rvc_num", dly_corr_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	dly_corr_ttlSchema.Columns.Add(new DatabaseColumn("rvc_name", dly_corr_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	dly_corr_ttlSchema.Columns.Add(new DatabaseColumn("emp_num", dly_corr_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	dly_corr_ttlSchema.Columns.Add(new DatabaseColumn("emp_last_name", dly_corr_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	dly_corr_ttlSchema.Columns.Add(new DatabaseColumn("emp_first_name", dly_corr_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	dly_corr_ttlSchema.Columns.Add(new DatabaseColumn("emp_check_name", dly_corr_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	dly_corr_ttlSchema.Columns.Add(new DatabaseColumn("man_num", dly_corr_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	dly_corr_ttlSchema.Columns.Add(new DatabaseColumn("man_last_name", dly_corr_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	dly_corr_ttlSchema.Columns.Add(new DatabaseColumn("man_first_name", dly_corr_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	dly_corr_ttlSchema.Columns.Add(new DatabaseColumn("man_check_name", dly_corr_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	dly_corr_ttlSchema.Columns.Add(new DatabaseColumn("open_datetime", dly_corr_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	dly_corr_ttlSchema.Columns.Add(new DatabaseColumn("close_datetime", dly_corr_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	dly_corr_ttlSchema.Columns.Add(new DatabaseColumn("trans_datetime", dly_corr_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	dly_corr_ttlSchema.Columns.Add(new DatabaseColumn("ob_item_corr", dly_corr_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	dly_corr_ttlSchema.Columns.Add(new DatabaseColumn("ob_item_rtn", dly_corr_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	dly_corr_ttlSchema.Columns.Add(new DatabaseColumn("art_num", dly_corr_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	dly_corr_ttlSchema.Columns.Add(new DatabaseColumn("art_name1", dly_corr_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	dly_corr_ttlSchema.Columns.Add(new DatabaseColumn("art_name2", dly_corr_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	dly_corr_ttlSchema.Columns.Add(new DatabaseColumn("cnt", dly_corr_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	dly_corr_ttlSchema.Columns.Add(new DatabaseColumn("ttl", dly_corr_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	dly_corr_ttlSchema.Columns.Add(new DatabaseColumn("sub_ttl", dly_corr_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	dly_corr_ttlSchema.Columns.Add(new DatabaseColumn("fact_num", dly_corr_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	dly_corr_ttlSchema.Columns.Add(new DatabaseColumn("num_dtl", dly_corr_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	dly_corr_ttlSchema.Columns.Add(new DatabaseColumn("covers", dly_corr_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	dly_corr_ttlSchema.Columns.Add(new DatabaseColumn("reason_num", dly_corr_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	dly_corr_ttlSchema.Columns.Add(new DatabaseColumn("reason_name", dly_corr_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	dly_corr_ttlSchema.Columns.Add(new DatabaseColumn("business_date", dly_corr_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	dly_corr_ttlSchema.Columns.Add(new DatabaseColumn("trans_seq", dly_corr_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	dly_corr_ttlSchema.Columns.Add(new DatabaseColumn("dtl_seq", dly_corr_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	dly_corr_ttlSchema.Columns.Add(new DatabaseColumn("sp_error", dly_corr_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	dly_corr_ttlSchema.Columns.Add(new DatabaseColumn("dly_corr_ttlID", dly_corr_ttlSchema)
												{
													IsPrimaryKey = true,
													DataType = DbType.Guid,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	dly_corr_ttlSchema.Columns.Add(new DatabaseColumn("DataSource", dly_corr_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
				// Add dly_corr_ttl to schema
            	DataProvider.Schema.Tables.Add(dly_corr_ttlSchema);
				
				// Table: dly_discount_ttl
				// Primary Key: dly_discount_ttlID
				ITable dly_discount_ttlSchema = new DatabaseTable("dly_discount_ttl", DataProvider) { ClassName = "dly_discount_ttl", SchemaName = "imp" };
            	dly_discount_ttlSchema.Columns.Add(new DatabaseColumn("store_id", dly_discount_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	dly_discount_ttlSchema.Columns.Add(new DatabaseColumn("rvc_num", dly_discount_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	dly_discount_ttlSchema.Columns.Add(new DatabaseColumn("rvc_name", dly_discount_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	dly_discount_ttlSchema.Columns.Add(new DatabaseColumn("emp_num", dly_discount_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	dly_discount_ttlSchema.Columns.Add(new DatabaseColumn("emp_last_name", dly_discount_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	dly_discount_ttlSchema.Columns.Add(new DatabaseColumn("emp_first_name", dly_discount_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	dly_discount_ttlSchema.Columns.Add(new DatabaseColumn("emp_check_name", dly_discount_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	dly_discount_ttlSchema.Columns.Add(new DatabaseColumn("man_num", dly_discount_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	dly_discount_ttlSchema.Columns.Add(new DatabaseColumn("man_last_name", dly_discount_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	dly_discount_ttlSchema.Columns.Add(new DatabaseColumn("man_first_name", dly_discount_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	dly_discount_ttlSchema.Columns.Add(new DatabaseColumn("man_check_name", dly_discount_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	dly_discount_ttlSchema.Columns.Add(new DatabaseColumn("open_datetime", dly_discount_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	dly_discount_ttlSchema.Columns.Add(new DatabaseColumn("close_datetime", dly_discount_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	dly_discount_ttlSchema.Columns.Add(new DatabaseColumn("trans_datetime", dly_discount_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	dly_discount_ttlSchema.Columns.Add(new DatabaseColumn("disc_num", dly_discount_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	dly_discount_ttlSchema.Columns.Add(new DatabaseColumn("disc_name", dly_discount_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	dly_discount_ttlSchema.Columns.Add(new DatabaseColumn("ob_item_disc", dly_discount_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	dly_discount_ttlSchema.Columns.Add(new DatabaseColumn("amount", dly_discount_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	dly_discount_ttlSchema.Columns.Add(new DatabaseColumn("percentage", dly_discount_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	dly_discount_ttlSchema.Columns.Add(new DatabaseColumn("art_num", dly_discount_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	dly_discount_ttlSchema.Columns.Add(new DatabaseColumn("art_name1", dly_discount_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	dly_discount_ttlSchema.Columns.Add(new DatabaseColumn("art_name2", dly_discount_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	dly_discount_ttlSchema.Columns.Add(new DatabaseColumn("cnt", dly_discount_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	dly_discount_ttlSchema.Columns.Add(new DatabaseColumn("ttl", dly_discount_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	dly_discount_ttlSchema.Columns.Add(new DatabaseColumn("sub_ttl", dly_discount_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	dly_discount_ttlSchema.Columns.Add(new DatabaseColumn("fact_num", dly_discount_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	dly_discount_ttlSchema.Columns.Add(new DatabaseColumn("num_dtl", dly_discount_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	dly_discount_ttlSchema.Columns.Add(new DatabaseColumn("covers", dly_discount_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	dly_discount_ttlSchema.Columns.Add(new DatabaseColumn("reason_num", dly_discount_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	dly_discount_ttlSchema.Columns.Add(new DatabaseColumn("reason_name", dly_discount_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	dly_discount_ttlSchema.Columns.Add(new DatabaseColumn("business_date", dly_discount_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	dly_discount_ttlSchema.Columns.Add(new DatabaseColumn("trans_seq", dly_discount_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	dly_discount_ttlSchema.Columns.Add(new DatabaseColumn("dtl_seq", dly_discount_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	dly_discount_ttlSchema.Columns.Add(new DatabaseColumn("sp_error", dly_discount_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	dly_discount_ttlSchema.Columns.Add(new DatabaseColumn("dly_discount_ttlID", dly_discount_ttlSchema)
												{
													IsPrimaryKey = true,
													DataType = DbType.Guid,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	dly_discount_ttlSchema.Columns.Add(new DatabaseColumn("DataSource", dly_discount_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
				// Add dly_discount_ttl to schema
            	DataProvider.Schema.Tables.Add(dly_discount_ttlSchema);
				
				// Table: maj_grp_def
				// Primary Key: maj_grp_defID
				ITable maj_grp_defSchema = new DatabaseTable("maj_grp_def", DataProvider) { ClassName = "maj_grp_def", SchemaName = "imp" };
            	maj_grp_defSchema.Columns.Add(new DatabaseColumn("maj_grp_seq", maj_grp_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	maj_grp_defSchema.Columns.Add(new DatabaseColumn("obj_num", maj_grp_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	maj_grp_defSchema.Columns.Add(new DatabaseColumn("cat", maj_grp_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	maj_grp_defSchema.Columns.Add(new DatabaseColumn("name", maj_grp_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	maj_grp_defSchema.Columns.Add(new DatabaseColumn("cos_grp_seq", maj_grp_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	maj_grp_defSchema.Columns.Add(new DatabaseColumn("acct_grp_seq", maj_grp_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	maj_grp_defSchema.Columns.Add(new DatabaseColumn("last_updated_by", maj_grp_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	maj_grp_defSchema.Columns.Add(new DatabaseColumn("last_updated_date", maj_grp_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	maj_grp_defSchema.Columns.Add(new DatabaseColumn("multi_user_access_seq", maj_grp_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	maj_grp_defSchema.Columns.Add(new DatabaseColumn("maj_grp_defID", maj_grp_defSchema)
												{
													IsPrimaryKey = true,
													DataType = DbType.Guid,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	maj_grp_defSchema.Columns.Add(new DatabaseColumn("DataSource", maj_grp_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
				// Add maj_grp_def to schema
            	DataProvider.Schema.Tables.Add(maj_grp_defSchema);
				
				// Table: mfd_check_dtl
				// Primary Key: mfd_check_dtlID
				ITable mfd_check_dtlSchema = new DatabaseTable("mfd_check_dtl", DataProvider) { ClassName = "mfd_check_dtl", SchemaName = "imp" };
            	mfd_check_dtlSchema.Columns.Add(new DatabaseColumn("Imported", mfd_check_dtlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_dtlSchema.Columns.Add(new DatabaseColumn("Business_Date", mfd_check_dtlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_dtlSchema.Columns.Add(new DatabaseColumn("Store_ID", mfd_check_dtlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_dtlSchema.Columns.Add(new DatabaseColumn("Store_Obj", mfd_check_dtlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_dtlSchema.Columns.Add(new DatabaseColumn("Store_Name", mfd_check_dtlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_dtlSchema.Columns.Add(new DatabaseColumn("RVC_Seq", mfd_check_dtlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_dtlSchema.Columns.Add(new DatabaseColumn("RVC_Obj", mfd_check_dtlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_dtlSchema.Columns.Add(new DatabaseColumn("Check_Num", mfd_check_dtlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_dtlSchema.Columns.Add(new DatabaseColumn("Check_Seq", mfd_check_dtlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_dtlSchema.Columns.Add(new DatabaseColumn("Trans_Seq", mfd_check_dtlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_dtlSchema.Columns.Add(new DatabaseColumn("Dtl_Seq", mfd_check_dtlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_dtlSchema.Columns.Add(new DatabaseColumn("Dtl_Typ", mfd_check_dtlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_dtlSchema.Columns.Add(new DatabaseColumn("Dtl_Trans_Typ", mfd_check_dtlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_dtlSchema.Columns.Add(new DatabaseColumn("Dtl_Typ_Seq", mfd_check_dtlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_dtlSchema.Columns.Add(new DatabaseColumn("Dtl_Typ_Obj", mfd_check_dtlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_dtlSchema.Columns.Add(new DatabaseColumn("Dtl_Typ_Name_1", mfd_check_dtlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_dtlSchema.Columns.Add(new DatabaseColumn("Dtl_Typ_Name_2", mfd_check_dtlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_dtlSchema.Columns.Add(new DatabaseColumn("Dtl_Typ_Cnt", mfd_check_dtlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_dtlSchema.Columns.Add(new DatabaseColumn("Dtl_Typ_Ttl", mfd_check_dtlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_dtlSchema.Columns.Add(new DatabaseColumn("Dtl_Typ_Ref_1", mfd_check_dtlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_dtlSchema.Columns.Add(new DatabaseColumn("Dtl_Typ_Ref_2", mfd_check_dtlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_dtlSchema.Columns.Add(new DatabaseColumn("Dtl_Mi_SalesItmzr", mfd_check_dtlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_dtlSchema.Columns.Add(new DatabaseColumn("Dtl_Mi_DscItmzr", mfd_check_dtlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_dtlSchema.Columns.Add(new DatabaseColumn("Dtl_Mi_SvcItmzr", mfd_check_dtlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_dtlSchema.Columns.Add(new DatabaseColumn("Dtl_Mi_PriceLevel", mfd_check_dtlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_dtlSchema.Columns.Add(new DatabaseColumn("Dtl_Mi_Maj_Group_Seq", mfd_check_dtlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_dtlSchema.Columns.Add(new DatabaseColumn("Dtl_Mi_Maj_Group_Obj", mfd_check_dtlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_dtlSchema.Columns.Add(new DatabaseColumn("Dtl_Mi_Maj_Group_Name", mfd_check_dtlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_dtlSchema.Columns.Add(new DatabaseColumn("Dtl_Mi_Fam_Group_Seq", mfd_check_dtlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_dtlSchema.Columns.Add(new DatabaseColumn("Dtl_Mi_Fam_Group_Obj", mfd_check_dtlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_dtlSchema.Columns.Add(new DatabaseColumn("Dtl_Mi_Fam_Group_Name", mfd_check_dtlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_dtlSchema.Columns.Add(new DatabaseColumn("Dtl_Mi_Tax", mfd_check_dtlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_dtlSchema.Columns.Add(new DatabaseColumn("Dtl_Mi_TaxPcnt", mfd_check_dtlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_dtlSchema.Columns.Add(new DatabaseColumn("Dtl_Mi_Netto_Ttl", mfd_check_dtlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_dtlSchema.Columns.Add(new DatabaseColumn("Dtl_Ref_Spec1", mfd_check_dtlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_dtlSchema.Columns.Add(new DatabaseColumn("Dtl_Ref_Spec2", mfd_check_dtlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_dtlSchema.Columns.Add(new DatabaseColumn("Rvc_Name", mfd_check_dtlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_dtlSchema.Columns.Add(new DatabaseColumn("Trans_Date", mfd_check_dtlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_dtlSchema.Columns.Add(new DatabaseColumn("Order_Type", mfd_check_dtlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_dtlSchema.Columns.Add(new DatabaseColumn("Period_seq", mfd_check_dtlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_dtlSchema.Columns.Add(new DatabaseColumn("Dtl_Dsc_Cnt", mfd_check_dtlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_dtlSchema.Columns.Add(new DatabaseColumn("Dtl_Dsc_Ttl", mfd_check_dtlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_dtlSchema.Columns.Add(new DatabaseColumn("Dtl_Dsc_Netto_Ttl", mfd_check_dtlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_dtlSchema.Columns.Add(new DatabaseColumn("Curr", mfd_check_dtlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_dtlSchema.Columns.Add(new DatabaseColumn("M_Typ", mfd_check_dtlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_dtlSchema.Columns.Add(new DatabaseColumn("Combo_Idx", mfd_check_dtlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_dtlSchema.Columns.Add(new DatabaseColumn("Tax_Typ_1", mfd_check_dtlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_dtlSchema.Columns.Add(new DatabaseColumn("VAT_Amount_1", mfd_check_dtlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_dtlSchema.Columns.Add(new DatabaseColumn("Tax_Typ_2", mfd_check_dtlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_dtlSchema.Columns.Add(new DatabaseColumn("VAT_Amount_2", mfd_check_dtlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_dtlSchema.Columns.Add(new DatabaseColumn("Tax_Typ_3", mfd_check_dtlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_dtlSchema.Columns.Add(new DatabaseColumn("VAT_Amount_3", mfd_check_dtlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_dtlSchema.Columns.Add(new DatabaseColumn("Tax_Typ_4", mfd_check_dtlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_dtlSchema.Columns.Add(new DatabaseColumn("VAT_Amount_4", mfd_check_dtlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_dtlSchema.Columns.Add(new DatabaseColumn("Tax_Typ_5", mfd_check_dtlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_dtlSchema.Columns.Add(new DatabaseColumn("VAT_Amount_5", mfd_check_dtlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_dtlSchema.Columns.Add(new DatabaseColumn("Tax_Typ_6", mfd_check_dtlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_dtlSchema.Columns.Add(new DatabaseColumn("VAT_Amount_6", mfd_check_dtlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_dtlSchema.Columns.Add(new DatabaseColumn("Tax_Typ_7", mfd_check_dtlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_dtlSchema.Columns.Add(new DatabaseColumn("VAT_Amount_7", mfd_check_dtlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_dtlSchema.Columns.Add(new DatabaseColumn("Tax_Typ_8", mfd_check_dtlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_dtlSchema.Columns.Add(new DatabaseColumn("VAT_Amount_8", mfd_check_dtlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_dtlSchema.Columns.Add(new DatabaseColumn("Dtl_Mi_Dsc_Seq", mfd_check_dtlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_dtlSchema.Columns.Add(new DatabaseColumn("M_Item_Weight", mfd_check_dtlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_dtlSchema.Columns.Add(new DatabaseColumn("Dtl_Id", mfd_check_dtlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_dtlSchema.Columns.Add(new DatabaseColumn("sw1", mfd_check_dtlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_dtlSchema.Columns.Add(new DatabaseColumn("Trans_Srv_Period_Seq", mfd_check_dtlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_dtlSchema.Columns.Add(new DatabaseColumn("Trans_Emp_Seq", mfd_check_dtlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_dtlSchema.Columns.Add(new DatabaseColumn("vat", mfd_check_dtlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_dtlSchema.Columns.Add(new DatabaseColumn("vat_ex", mfd_check_dtlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_dtlSchema.Columns.Add(new DatabaseColumn("Seq", mfd_check_dtlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_dtlSchema.Columns.Add(new DatabaseColumn("Uws_Obj", mfd_check_dtlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_dtlSchema.Columns.Add(new DatabaseColumn("mfd_check_dtlID", mfd_check_dtlSchema)
												{
													IsPrimaryKey = true,
													DataType = DbType.Guid,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_dtlSchema.Columns.Add(new DatabaseColumn("DataSource", mfd_check_dtlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
				// Add mfd_check_dtl to schema
            	DataProvider.Schema.Tables.Add(mfd_check_dtlSchema);
				
				// Table: mfd_check_ttl
				// Primary Key: mfd_check_ttlID
				ITable mfd_check_ttlSchema = new DatabaseTable("mfd_check_ttl", DataProvider) { ClassName = "mfd_check_ttl", SchemaName = "imp" };
            	mfd_check_ttlSchema.Columns.Add(new DatabaseColumn("Imported", mfd_check_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_ttlSchema.Columns.Add(new DatabaseColumn("Business_Date", mfd_check_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_ttlSchema.Columns.Add(new DatabaseColumn("Store_ID", mfd_check_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_ttlSchema.Columns.Add(new DatabaseColumn("Store_Obj", mfd_check_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_ttlSchema.Columns.Add(new DatabaseColumn("Store_Name", mfd_check_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_ttlSchema.Columns.Add(new DatabaseColumn("Location_Name", mfd_check_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_ttlSchema.Columns.Add(new DatabaseColumn("RVC_Seq", mfd_check_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_ttlSchema.Columns.Add(new DatabaseColumn("RVC_Obj", mfd_check_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_ttlSchema.Columns.Add(new DatabaseColumn("RVC_Name", mfd_check_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_ttlSchema.Columns.Add(new DatabaseColumn("Employee_Seq", mfd_check_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_ttlSchema.Columns.Add(new DatabaseColumn("Employee_Obj", mfd_check_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_ttlSchema.Columns.Add(new DatabaseColumn("Employee_Name", mfd_check_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_ttlSchema.Columns.Add(new DatabaseColumn("Table_Seq", mfd_check_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_ttlSchema.Columns.Add(new DatabaseColumn("Table_Obj", mfd_check_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_ttlSchema.Columns.Add(new DatabaseColumn("Table_Name", mfd_check_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_ttlSchema.Columns.Add(new DatabaseColumn("Check_Num", mfd_check_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_ttlSchema.Columns.Add(new DatabaseColumn("Check_Seq", mfd_check_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_ttlSchema.Columns.Add(new DatabaseColumn("Uws_Seq", mfd_check_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_ttlSchema.Columns.Add(new DatabaseColumn("Uws_Obj", mfd_check_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_ttlSchema.Columns.Add(new DatabaseColumn("OrderType_Seq", mfd_check_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_ttlSchema.Columns.Add(new DatabaseColumn("OrderType_Name", mfd_check_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_ttlSchema.Columns.Add(new DatabaseColumn("Open_Date", mfd_check_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_ttlSchema.Columns.Add(new DatabaseColumn("Open_Time", mfd_check_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_ttlSchema.Columns.Add(new DatabaseColumn("Close_Date", mfd_check_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_ttlSchema.Columns.Add(new DatabaseColumn("Close_Time", mfd_check_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_ttlSchema.Columns.Add(new DatabaseColumn("Cover_Cnt", mfd_check_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_ttlSchema.Columns.Add(new DatabaseColumn("Auto_Svc_Ttl", mfd_check_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_ttlSchema.Columns.Add(new DatabaseColumn("Other_Svc_Ttl", mfd_check_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_ttlSchema.Columns.Add(new DatabaseColumn("Sub_Ttl", mfd_check_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_ttlSchema.Columns.Add(new DatabaseColumn("Paymnt_Ttl", mfd_check_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_ttlSchema.Columns.Add(new DatabaseColumn("Amt_Due_Ttl", mfd_check_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_ttlSchema.Columns.Add(new DatabaseColumn("Tax_Ttl", mfd_check_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_ttlSchema.Columns.Add(new DatabaseColumn("Prntd_Cnt", mfd_check_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_ttlSchema.Columns.Add(new DatabaseColumn("Num_Dtl", mfd_check_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_ttlSchema.Columns.Add(new DatabaseColumn("Num_mi_Dtl", mfd_check_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_ttlSchema.Columns.Add(new DatabaseColumn("Order_Type", mfd_check_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_ttlSchema.Columns.Add(new DatabaseColumn("Curr", mfd_check_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_ttlSchema.Columns.Add(new DatabaseColumn("sw1", mfd_check_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_ttlSchema.Columns.Add(new DatabaseColumn("CheckId", mfd_check_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_ttlSchema.Columns.Add(new DatabaseColumn("Seq", mfd_check_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_ttlSchema.Columns.Add(new DatabaseColumn("mfd_check_ttlID", mfd_check_ttlSchema)
												{
													IsPrimaryKey = true,
													DataType = DbType.Guid,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mfd_check_ttlSchema.Columns.Add(new DatabaseColumn("DataSource", mfd_check_ttlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
				// Add mfd_check_ttl to schema
            	DataProvider.Schema.Tables.Add(mfd_check_ttlSchema);
				
				// Table: mi_def
				// Primary Key: mi_defID
				ITable mi_defSchema = new DatabaseTable("mi_def", DataProvider) { ClassName = "mi_def", SchemaName = "imp" };
            	mi_defSchema.Columns.Add(new DatabaseColumn("mi_seq", mi_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_defSchema.Columns.Add(new DatabaseColumn("obj_num", mi_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_defSchema.Columns.Add(new DatabaseColumn("name_1", mi_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_defSchema.Columns.Add(new DatabaseColumn("name_2", mi_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_defSchema.Columns.Add(new DatabaseColumn("maj_grp_seq", mi_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_defSchema.Columns.Add(new DatabaseColumn("fam_grp_seq", mi_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_defSchema.Columns.Add(new DatabaseColumn("mi_grp_seq", mi_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_defSchema.Columns.Add(new DatabaseColumn("mi_slu_seq", mi_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_defSchema.Columns.Add(new DatabaseColumn("price_grp_seq", mi_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_defSchema.Columns.Add(new DatabaseColumn("slu_priority", mi_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_defSchema.Columns.Add(new DatabaseColumn("nlu_grp", mi_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_defSchema.Columns.Add(new DatabaseColumn("nlu_num", mi_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_defSchema.Columns.Add(new DatabaseColumn("key_num", mi_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_defSchema.Columns.Add(new DatabaseColumn("icon_id", mi_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_defSchema.Columns.Add(new DatabaseColumn("ob_mi31_chk_mi_avail", mi_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_defSchema.Columns.Add(new DatabaseColumn("ob_mi44_no_edit_in_mgr_proc", mi_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_defSchema.Columns.Add(new DatabaseColumn("ob_item_is_the_no_modifier", mi_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_defSchema.Columns.Add(new DatabaseColumn("ob_lite_mi_dirty", mi_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_defSchema.Columns.Add(new DatabaseColumn("ob_rsvd01", mi_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_defSchema.Columns.Add(new DatabaseColumn("ob_rsvd02", mi_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_defSchema.Columns.Add(new DatabaseColumn("ob_rsvd03", mi_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_defSchema.Columns.Add(new DatabaseColumn("ob_rsvd04", mi_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_defSchema.Columns.Add(new DatabaseColumn("mi_type_seq", mi_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_defSchema.Columns.Add(new DatabaseColumn("cond_grp_mem_seq", mi_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_defSchema.Columns.Add(new DatabaseColumn("cond_req", mi_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_defSchema.Columns.Add(new DatabaseColumn("cond_allowed", mi_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_defSchema.Columns.Add(new DatabaseColumn("crs_mem_seq", mi_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_defSchema.Columns.Add(new DatabaseColumn("crs_sel_seq", mi_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_defSchema.Columns.Add(new DatabaseColumn("mlvl_class_seq", mi_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_defSchema.Columns.Add(new DatabaseColumn("prn_def_class_seq", mi_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_defSchema.Columns.Add(new DatabaseColumn("product_seq_1", mi_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_defSchema.Columns.Add(new DatabaseColumn("product_seq_2", mi_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_defSchema.Columns.Add(new DatabaseColumn("product_seq_3", mi_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_defSchema.Columns.Add(new DatabaseColumn("product_seq_4", mi_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_defSchema.Columns.Add(new DatabaseColumn("comm_amt", mi_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_defSchema.Columns.Add(new DatabaseColumn("comm_pcnt", mi_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_defSchema.Columns.Add(new DatabaseColumn("cross_ref1", mi_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_defSchema.Columns.Add(new DatabaseColumn("cross_ref2", mi_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_defSchema.Columns.Add(new DatabaseColumn("last_updated_by", mi_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_defSchema.Columns.Add(new DatabaseColumn("last_updated_date", mi_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_defSchema.Columns.Add(new DatabaseColumn("ob_workstation_only", mi_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_defSchema.Columns.Add(new DatabaseColumn("mi_slu2_seq", mi_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_defSchema.Columns.Add(new DatabaseColumn("ob_flags", mi_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_defSchema.Columns.Add(new DatabaseColumn("multi_user_access_seq", mi_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_defSchema.Columns.Add(new DatabaseColumn("prep_time", mi_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_defSchema.Columns.Add(new DatabaseColumn("external_type", mi_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_defSchema.Columns.Add(new DatabaseColumn("topping_type_seq", mi_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_defSchema.Columns.Add(new DatabaseColumn("topping_modifier_seq", mi_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_defSchema.Columns.Add(new DatabaseColumn("build_screen_style_seq", mi_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_defSchema.Columns.Add(new DatabaseColumn("hht_build_screen_style_seq", mi_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_defSchema.Columns.Add(new DatabaseColumn("prefix_override_level", mi_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_defSchema.Columns.Add(new DatabaseColumn("prefix_override_count", mi_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_defSchema.Columns.Add(new DatabaseColumn("mi_slu3_seq", mi_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_defSchema.Columns.Add(new DatabaseColumn("mi_slu4_seq", mi_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_defSchema.Columns.Add(new DatabaseColumn("mi_slu5_seq", mi_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_defSchema.Columns.Add(new DatabaseColumn("mi_slu6_seq", mi_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_defSchema.Columns.Add(new DatabaseColumn("mi_slu7_seq", mi_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_defSchema.Columns.Add(new DatabaseColumn("mi_slu8_seq", mi_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_defSchema.Columns.Add(new DatabaseColumn("mi_defID", mi_defSchema)
												{
													IsPrimaryKey = true,
													DataType = DbType.Guid,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_defSchema.Columns.Add(new DatabaseColumn("DataSource", mi_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
				// Add mi_def to schema
            	DataProvider.Schema.Tables.Add(mi_defSchema);
				
				// Table: mi_price_def
				// Primary Key: mi_price_defID
				ITable mi_price_defSchema = new DatabaseTable("mi_price_def", DataProvider) { ClassName = "mi_price_def", SchemaName = "imp" };
            	mi_price_defSchema.Columns.Add(new DatabaseColumn("mi_seq", mi_price_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_price_defSchema.Columns.Add(new DatabaseColumn("mi_price_seq", mi_price_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_price_defSchema.Columns.Add(new DatabaseColumn("effective_from", mi_price_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_price_defSchema.Columns.Add(new DatabaseColumn("effective_to", mi_price_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_price_defSchema.Columns.Add(new DatabaseColumn("preset_amt_1", mi_price_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_price_defSchema.Columns.Add(new DatabaseColumn("preset_amt_2", mi_price_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_price_defSchema.Columns.Add(new DatabaseColumn("preset_amt_3", mi_price_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_price_defSchema.Columns.Add(new DatabaseColumn("preset_amt_4", mi_price_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_price_defSchema.Columns.Add(new DatabaseColumn("preset_amt_5", mi_price_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_price_defSchema.Columns.Add(new DatabaseColumn("preset_amt_6", mi_price_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_price_defSchema.Columns.Add(new DatabaseColumn("preset_amt_7", mi_price_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_price_defSchema.Columns.Add(new DatabaseColumn("preset_amt_8", mi_price_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_price_defSchema.Columns.Add(new DatabaseColumn("preset_amt_9", mi_price_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_price_defSchema.Columns.Add(new DatabaseColumn("preset_amt_10", mi_price_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_price_defSchema.Columns.Add(new DatabaseColumn("vat_txbl_1", mi_price_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_price_defSchema.Columns.Add(new DatabaseColumn("vat_txbl_2", mi_price_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_price_defSchema.Columns.Add(new DatabaseColumn("vat_txbl_3", mi_price_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_price_defSchema.Columns.Add(new DatabaseColumn("vat_txbl_4", mi_price_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_price_defSchema.Columns.Add(new DatabaseColumn("vat_txbl_5", mi_price_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_price_defSchema.Columns.Add(new DatabaseColumn("vat_txbl_6", mi_price_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_price_defSchema.Columns.Add(new DatabaseColumn("vat_txbl_7", mi_price_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_price_defSchema.Columns.Add(new DatabaseColumn("vat_txbl_8", mi_price_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_price_defSchema.Columns.Add(new DatabaseColumn("vat_txbl_9", mi_price_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_price_defSchema.Columns.Add(new DatabaseColumn("vat_txbl_10", mi_price_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_price_defSchema.Columns.Add(new DatabaseColumn("cost_1", mi_price_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_price_defSchema.Columns.Add(new DatabaseColumn("cost_2", mi_price_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_price_defSchema.Columns.Add(new DatabaseColumn("cost_3", mi_price_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_price_defSchema.Columns.Add(new DatabaseColumn("cost_4", mi_price_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_price_defSchema.Columns.Add(new DatabaseColumn("cost_5", mi_price_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_price_defSchema.Columns.Add(new DatabaseColumn("cost_6", mi_price_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_price_defSchema.Columns.Add(new DatabaseColumn("cost_7", mi_price_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_price_defSchema.Columns.Add(new DatabaseColumn("cost_8", mi_price_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_price_defSchema.Columns.Add(new DatabaseColumn("cost_9", mi_price_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_price_defSchema.Columns.Add(new DatabaseColumn("cost_10", mi_price_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_price_defSchema.Columns.Add(new DatabaseColumn("tare_weight", mi_price_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_price_defSchema.Columns.Add(new DatabaseColumn("surcharge_tax", mi_price_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_price_defSchema.Columns.Add(new DatabaseColumn("em_chg_set_seq", mi_price_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_price_defSchema.Columns.Add(new DatabaseColumn("price_grp_seq", mi_price_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_price_defSchema.Columns.Add(new DatabaseColumn("price_tier_seq", mi_price_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_price_defSchema.Columns.Add(new DatabaseColumn("em_repl_status", mi_price_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_price_defSchema.Columns.Add(new DatabaseColumn("comments", mi_price_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_price_defSchema.Columns.Add(new DatabaseColumn("ob_rsvd01", mi_price_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_price_defSchema.Columns.Add(new DatabaseColumn("ob_rsvd02", mi_price_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_price_defSchema.Columns.Add(new DatabaseColumn("ob_rsvd03", mi_price_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_price_defSchema.Columns.Add(new DatabaseColumn("ob_rsvd04", mi_price_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_price_defSchema.Columns.Add(new DatabaseColumn("last_updated_by", mi_price_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_price_defSchema.Columns.Add(new DatabaseColumn("menu_panel_price_01", mi_price_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_price_defSchema.Columns.Add(new DatabaseColumn("menu_panel_price_02", mi_price_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_price_defSchema.Columns.Add(new DatabaseColumn("menu_panel_price_03", mi_price_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_price_defSchema.Columns.Add(new DatabaseColumn("menu_panel_price_04", mi_price_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_price_defSchema.Columns.Add(new DatabaseColumn("menu_panel_price_05", mi_price_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_price_defSchema.Columns.Add(new DatabaseColumn("menu_panel_price_06", mi_price_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_price_defSchema.Columns.Add(new DatabaseColumn("menu_panel_price_07", mi_price_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_price_defSchema.Columns.Add(new DatabaseColumn("menu_panel_price_08", mi_price_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_price_defSchema.Columns.Add(new DatabaseColumn("menu_panel_price_09", mi_price_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_price_defSchema.Columns.Add(new DatabaseColumn("menu_panel_price_10", mi_price_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_price_defSchema.Columns.Add(new DatabaseColumn("mi_price_defID", mi_price_defSchema)
												{
													IsPrimaryKey = true,
													DataType = DbType.Guid,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_price_defSchema.Columns.Add(new DatabaseColumn("DataSource", mi_price_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
				// Add mi_price_def to schema
            	DataProvider.Schema.Tables.Add(mi_price_defSchema);
				
				// Table: mi_slu_def
				// Primary Key: mi_slu_defID
				ITable mi_slu_defSchema = new DatabaseTable("mi_slu_def", DataProvider) { ClassName = "mi_slu_def", SchemaName = "imp" };
            	mi_slu_defSchema.Columns.Add(new DatabaseColumn("mi_slu_seq", mi_slu_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_slu_defSchema.Columns.Add(new DatabaseColumn("obj_num", mi_slu_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_slu_defSchema.Columns.Add(new DatabaseColumn("name", mi_slu_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_slu_defSchema.Columns.Add(new DatabaseColumn("ts_style_seq", mi_slu_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_slu_defSchema.Columns.Add(new DatabaseColumn("last_updated_by", mi_slu_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_slu_defSchema.Columns.Add(new DatabaseColumn("last_updated_date", mi_slu_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_slu_defSchema.Columns.Add(new DatabaseColumn("hht_ts_style_seq", mi_slu_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_slu_defSchema.Columns.Add(new DatabaseColumn("mi_slu_defID", mi_slu_defSchema)
												{
													IsPrimaryKey = true,
													DataType = DbType.Guid,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mi_slu_defSchema.Columns.Add(new DatabaseColumn("DataSource", mi_slu_defSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
				// Add mi_slu_def to schema
            	DataProvider.Schema.Tables.Add(mi_slu_defSchema);
				
				// Table: Micros_Ticket_Details
				// Primary Key: Micros_Ticket_DetailsID
				ITable Micros_Ticket_DetailsSchema = new DatabaseTable("Micros_Ticket_Details", DataProvider) { ClassName = "Micros_Ticket_Detail", SchemaName = "imp" };
            	Micros_Ticket_DetailsSchema.Columns.Add(new DatabaseColumn("BUSINESS_DATE", Micros_Ticket_DetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_Ticket_DetailsSchema.Columns.Add(new DatabaseColumn("RVC_SEQ", Micros_Ticket_DetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_Ticket_DetailsSchema.Columns.Add(new DatabaseColumn("RVC_OBJ_NUM", Micros_Ticket_DetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_Ticket_DetailsSchema.Columns.Add(new DatabaseColumn("NAME", Micros_Ticket_DetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_Ticket_DetailsSchema.Columns.Add(new DatabaseColumn("UWS_OBJ_NUM", Micros_Ticket_DetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_Ticket_DetailsSchema.Columns.Add(new DatabaseColumn("CHK_NUM", Micros_Ticket_DetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_Ticket_DetailsSchema.Columns.Add(new DatabaseColumn("ORDER_TYPE_SEQ", Micros_Ticket_DetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_Ticket_DetailsSchema.Columns.Add(new DatabaseColumn("CHK_SEQ", Micros_Ticket_DetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_Ticket_DetailsSchema.Columns.Add(new DatabaseColumn("SRV_PERIOD_SEQ", Micros_Ticket_DetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_Ticket_DetailsSchema.Columns.Add(new DatabaseColumn("TRANS_EMP_SEQ", Micros_Ticket_DetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_Ticket_DetailsSchema.Columns.Add(new DatabaseColumn("CHK_EMP_SEQ", Micros_Ticket_DetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_Ticket_DetailsSchema.Columns.Add(new DatabaseColumn("TRAINING_STATUS", Micros_Ticket_DetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_Ticket_DetailsSchema.Columns.Add(new DatabaseColumn("FIXED_PERIOD_SEQ", Micros_Ticket_DetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_Ticket_DetailsSchema.Columns.Add(new DatabaseColumn("DATE_TIME", Micros_Ticket_DetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_Ticket_DetailsSchema.Columns.Add(new DatabaseColumn("DTL_SEQ", Micros_Ticket_DetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_Ticket_DetailsSchema.Columns.Add(new DatabaseColumn("TRANS_SEQ", Micros_Ticket_DetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_Ticket_DetailsSchema.Columns.Add(new DatabaseColumn("DTL_TYPE", Micros_Ticket_DetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_Ticket_DetailsSchema.Columns.Add(new DatabaseColumn("SEAT", Micros_Ticket_DetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_Ticket_DetailsSchema.Columns.Add(new DatabaseColumn("RECORD_TYPE", Micros_Ticket_DetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_Ticket_DetailsSchema.Columns.Add(new DatabaseColumn("DTL_INDEX", Micros_Ticket_DetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_Ticket_DetailsSchema.Columns.Add(new DatabaseColumn("SHARED_NUMERATOR", Micros_Ticket_DetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_Ticket_DetailsSchema.Columns.Add(new DatabaseColumn("SHARED_DENOMINATOR", Micros_Ticket_DetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_Ticket_DetailsSchema.Columns.Add(new DatabaseColumn("RPT_INCLUSIVE_TAX_TTL", Micros_Ticket_DetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_Ticket_DetailsSchema.Columns.Add(new DatabaseColumn("RPT_INCLUSIVE_TAX_TTL_EX", Micros_Ticket_DetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_Ticket_DetailsSchema.Columns.Add(new DatabaseColumn("ACTIVE_TAXES", Micros_Ticket_DetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_Ticket_DetailsSchema.Columns.Add(new DatabaseColumn("COMM_TTL", Micros_Ticket_DetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_Ticket_DetailsSchema.Columns.Add(new DatabaseColumn("CHK_CNT", Micros_Ticket_DetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_Ticket_DetailsSchema.Columns.Add(new DatabaseColumn("CHK_TTL", Micros_Ticket_DetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_Ticket_DetailsSchema.Columns.Add(new DatabaseColumn("RPT_CNT", Micros_Ticket_DetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_Ticket_DetailsSchema.Columns.Add(new DatabaseColumn("RPT_TTL", Micros_Ticket_DetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_Ticket_DetailsSchema.Columns.Add(new DatabaseColumn("DTL_ID", Micros_Ticket_DetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_Ticket_DetailsSchema.Columns.Add(new DatabaseColumn("DTL_STATUS", Micros_Ticket_DetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_Ticket_DetailsSchema.Columns.Add(new DatabaseColumn("OB_DTL05_VOID_FLAG", Micros_Ticket_DetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_Ticket_DetailsSchema.Columns.Add(new DatabaseColumn("MI_SEQ", Micros_Ticket_DetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_Ticket_DetailsSchema.Columns.Add(new DatabaseColumn("MI_OBJ_NUM", Micros_Ticket_DetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_Ticket_DetailsSchema.Columns.Add(new DatabaseColumn("NAME_1", Micros_Ticket_DetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_Ticket_DetailsSchema.Columns.Add(new DatabaseColumn("NAME_2", Micros_Ticket_DetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_Ticket_DetailsSchema.Columns.Add(new DatabaseColumn("CRS", Micros_Ticket_DetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_Ticket_DetailsSchema.Columns.Add(new DatabaseColumn("OB_TAX_1_ACTIVE", Micros_Ticket_DetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_Ticket_DetailsSchema.Columns.Add(new DatabaseColumn("OB_TAX_2_ACTIVE", Micros_Ticket_DetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_Ticket_DetailsSchema.Columns.Add(new DatabaseColumn("OB_TAX_3_ACTIVE", Micros_Ticket_DetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_Ticket_DetailsSchema.Columns.Add(new DatabaseColumn("OB_TAX_4_ACTIVE", Micros_Ticket_DetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_Ticket_DetailsSchema.Columns.Add(new DatabaseColumn("OB_TAX_5_ACTIVE", Micros_Ticket_DetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_Ticket_DetailsSchema.Columns.Add(new DatabaseColumn("OB_TAX_6_ACTIVE", Micros_Ticket_DetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_Ticket_DetailsSchema.Columns.Add(new DatabaseColumn("OB_TAX_7_ACTIVE", Micros_Ticket_DetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_Ticket_DetailsSchema.Columns.Add(new DatabaseColumn("OB_TAX_8_ACTIVE", Micros_Ticket_DetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_Ticket_DetailsSchema.Columns.Add(new DatabaseColumn("SLS_ITMZR_SEQ", Micros_Ticket_DetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_Ticket_DetailsSchema.Columns.Add(new DatabaseColumn("DSC_ITMZR", Micros_Ticket_DetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_Ticket_DetailsSchema.Columns.Add(new DatabaseColumn("SVC_ITMZR", Micros_Ticket_DetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_Ticket_DetailsSchema.Columns.Add(new DatabaseColumn("PRICE_LVL", Micros_Ticket_DetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_Ticket_DetailsSchema.Columns.Add(new DatabaseColumn("SURCHARGE_TAX_TTL", Micros_Ticket_DetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_Ticket_DetailsSchema.Columns.Add(new DatabaseColumn("OB_DTL04_RTN", Micros_Ticket_DetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_Ticket_DetailsSchema.Columns.Add(new DatabaseColumn("TMED_SEQ", Micros_Ticket_DetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_Ticket_DetailsSchema.Columns.Add(new DatabaseColumn("OTHER_EMP_SEQ", Micros_Ticket_DetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_Ticket_DetailsSchema.Columns.Add(new DatabaseColumn("OB_TAX_1_EXEMPT_A", Micros_Ticket_DetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_Ticket_DetailsSchema.Columns.Add(new DatabaseColumn("OB_TAX_2_EXEMPT_A", Micros_Ticket_DetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_Ticket_DetailsSchema.Columns.Add(new DatabaseColumn("OB_TAX_3_EXEMPT_A", Micros_Ticket_DetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_Ticket_DetailsSchema.Columns.Add(new DatabaseColumn("OB_TAX_4_EXEMPT_A", Micros_Ticket_DetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_Ticket_DetailsSchema.Columns.Add(new DatabaseColumn("OB_TAX_5_EXEMPT_A", Micros_Ticket_DetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_Ticket_DetailsSchema.Columns.Add(new DatabaseColumn("OB_TAX_6_EXEMPT_A", Micros_Ticket_DetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_Ticket_DetailsSchema.Columns.Add(new DatabaseColumn("OB_TAX_7_EXEMPT_A", Micros_Ticket_DetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_Ticket_DetailsSchema.Columns.Add(new DatabaseColumn("OB_TAX_8_EXEMPT_A", Micros_Ticket_DetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_Ticket_DetailsSchema.Columns.Add(new DatabaseColumn("EXPIRATION_DATE", Micros_Ticket_DetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_Ticket_DetailsSchema.Columns.Add(new DatabaseColumn("CHG_TIP_TTL", Micros_Ticket_DetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_Ticket_DetailsSchema.Columns.Add(new DatabaseColumn("FRGN_CNCY_TTL", Micros_Ticket_DetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_Ticket_DetailsSchema.Columns.Add(new DatabaseColumn("FRGN_CNCY_NUM_DECIMAL_PLACES", Micros_Ticket_DetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_Ticket_DetailsSchema.Columns.Add(new DatabaseColumn("FRGN_CNCY_SEQ", Micros_Ticket_DetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_Ticket_DetailsSchema.Columns.Add(new DatabaseColumn("OB_TIPS_PAID", Micros_Ticket_DetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_Ticket_DetailsSchema.Columns.Add(new DatabaseColumn("ALLOCATED_TAX_TTL", Micros_Ticket_DetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_Ticket_DetailsSchema.Columns.Add(new DatabaseColumn("REF", Micros_Ticket_DetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_Ticket_DetailsSchema.Columns.Add(new DatabaseColumn("PARENT_DTL_SEQ_A", Micros_Ticket_DetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_Ticket_DetailsSchema.Columns.Add(new DatabaseColumn("PARENT_TRANS_SEQ_A", Micros_Ticket_DetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_Ticket_DetailsSchema.Columns.Add(new DatabaseColumn("REF2", Micros_Ticket_DetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_Ticket_DetailsSchema.Columns.Add(new DatabaseColumn("DSVC_SEQ", Micros_Ticket_DetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_Ticket_DetailsSchema.Columns.Add(new DatabaseColumn("EMP_MEAL_EMP", Micros_Ticket_DetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_Ticket_DetailsSchema.Columns.Add(new DatabaseColumn("PERCENTAGE", Micros_Ticket_DetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_Ticket_DetailsSchema.Columns.Add(new DatabaseColumn("OB_TAX_1_EXEMPT_B", Micros_Ticket_DetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_Ticket_DetailsSchema.Columns.Add(new DatabaseColumn("OB_TAX_2_EXEMPT_B", Micros_Ticket_DetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_Ticket_DetailsSchema.Columns.Add(new DatabaseColumn("OB_TAX_3_EXEMPT_B", Micros_Ticket_DetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_Ticket_DetailsSchema.Columns.Add(new DatabaseColumn("OB_TAX_4_EXEMPT_B", Micros_Ticket_DetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_Ticket_DetailsSchema.Columns.Add(new DatabaseColumn("OB_TAX_5_EXEMPT_B", Micros_Ticket_DetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_Ticket_DetailsSchema.Columns.Add(new DatabaseColumn("OB_TAX_6_EXEMPT_B", Micros_Ticket_DetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_Ticket_DetailsSchema.Columns.Add(new DatabaseColumn("OB_TAX_7_EXEMPT_B", Micros_Ticket_DetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_Ticket_DetailsSchema.Columns.Add(new DatabaseColumn("OB_TAX_8_EXEMPT_B", Micros_Ticket_DetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_Ticket_DetailsSchema.Columns.Add(new DatabaseColumn("PARENT_DTL_SEQ_B", Micros_Ticket_DetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_Ticket_DetailsSchema.Columns.Add(new DatabaseColumn("PARENT_TRANS_SEQ_B", Micros_Ticket_DetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_Ticket_DetailsSchema.Columns.Add(new DatabaseColumn("PARENT_DTL_ID", Micros_Ticket_DetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_Ticket_DetailsSchema.Columns.Add(new DatabaseColumn("TID_REF", Micros_Ticket_DetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_Ticket_DetailsSchema.Columns.Add(new DatabaseColumn("TID_INST_ID", Micros_Ticket_DetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_Ticket_DetailsSchema.Columns.Add(new DatabaseColumn("D_NI_SEQ", Micros_Ticket_DetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_Ticket_DetailsSchema.Columns.Add(new DatabaseColumn("OB_CHK_REOPENED", Micros_Ticket_DetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_Ticket_DetailsSchema.Columns.Add(new DatabaseColumn("OB_CLOSED_CHECK_EDIT", Micros_Ticket_DetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_Ticket_DetailsSchema.Columns.Add(new DatabaseColumn("ITEM_WEIGHT", Micros_Ticket_DetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_Ticket_DetailsSchema.Columns.Add(new DatabaseColumn("TYPE", Micros_Ticket_DetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_Ticket_DetailsSchema.Columns.Add(new DatabaseColumn("DSC_SUM", Micros_Ticket_DetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_Ticket_DetailsSchema.Columns.Add(new DatabaseColumn("DSC_CNT", Micros_Ticket_DetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_Ticket_DetailsSchema.Columns.Add(new DatabaseColumn("DSC_TAX", Micros_Ticket_DetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_Ticket_DetailsSchema.Columns.Add(new DatabaseColumn("Micros_Ticket_DetailsID", Micros_Ticket_DetailsSchema)
												{
													IsPrimaryKey = true,
													DataType = DbType.Guid,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_Ticket_DetailsSchema.Columns.Add(new DatabaseColumn("DataSource", Micros_Ticket_DetailsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
				// Add Micros_Ticket_Details to schema
            	DataProvider.Schema.Tables.Add(Micros_Ticket_DetailsSchema);
				
				// Table: Micros_Tickets
				// Primary Key: Micros_TicketsID
				ITable Micros_TicketsSchema = new DatabaseTable("Micros_Tickets", DataProvider) { ClassName = "Micros_Ticket", SchemaName = "imp" };
            	Micros_TicketsSchema.Columns.Add(new DatabaseColumn("business_date", Micros_TicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_TicketsSchema.Columns.Add(new DatabaseColumn("rvc_seq", Micros_TicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_TicketsSchema.Columns.Add(new DatabaseColumn("obj_num", Micros_TicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_TicketsSchema.Columns.Add(new DatabaseColumn("name", Micros_TicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_TicketsSchema.Columns.Add(new DatabaseColumn("emp_seq", Micros_TicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_TicketsSchema.Columns.Add(new DatabaseColumn("obj_num_emp", Micros_TicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_TicketsSchema.Columns.Add(new DatabaseColumn("last_name", Micros_TicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_TicketsSchema.Columns.Add(new DatabaseColumn("Tbl_Seq", Micros_TicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_TicketsSchema.Columns.Add(new DatabaseColumn("Obj_Num_tbl", Micros_TicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_TicketsSchema.Columns.Add(new DatabaseColumn("Obj_Name", Micros_TicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_TicketsSchema.Columns.Add(new DatabaseColumn("chk_seq", Micros_TicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_TicketsSchema.Columns.Add(new DatabaseColumn("chk_num", Micros_TicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_TicketsSchema.Columns.Add(new DatabaseColumn("last_uws_seq", Micros_TicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_TicketsSchema.Columns.Add(new DatabaseColumn("obj_num_uws", Micros_TicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_TicketsSchema.Columns.Add(new DatabaseColumn("order_type_seq", Micros_TicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_TicketsSchema.Columns.Add(new DatabaseColumn("order_type_name", Micros_TicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_TicketsSchema.Columns.Add(new DatabaseColumn("chk_open_date_time", Micros_TicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_TicketsSchema.Columns.Add(new DatabaseColumn("chk_clsd_date_time", Micros_TicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_TicketsSchema.Columns.Add(new DatabaseColumn("cov_cnt", Micros_TicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_TicketsSchema.Columns.Add(new DatabaseColumn("auto_svc_ttl", Micros_TicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_TicketsSchema.Columns.Add(new DatabaseColumn("other_svc_ttl", Micros_TicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_TicketsSchema.Columns.Add(new DatabaseColumn("sub_ttl", Micros_TicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_TicketsSchema.Columns.Add(new DatabaseColumn("pymnt_ttl", Micros_TicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_TicketsSchema.Columns.Add(new DatabaseColumn("amt_due_ttl", Micros_TicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_TicketsSchema.Columns.Add(new DatabaseColumn("tax_ttl", Micros_TicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_TicketsSchema.Columns.Add(new DatabaseColumn("chk_prntd_cnt", Micros_TicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_TicketsSchema.Columns.Add(new DatabaseColumn("num_dtl", Micros_TicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_TicketsSchema.Columns.Add(new DatabaseColumn("num_mi_dtl", Micros_TicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_TicketsSchema.Columns.Add(new DatabaseColumn("Micros_TicketsID", Micros_TicketsSchema)
												{
													IsPrimaryKey = true,
													DataType = DbType.Guid,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	Micros_TicketsSchema.Columns.Add(new DatabaseColumn("DataSource", Micros_TicketsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
				// Add Micros_Tickets to schema
            	DataProvider.Schema.Tables.Add(Micros_TicketsSchema);
				
				// Table: mrsbr_financial
				// Primary Key: mrsbr_financialID
				ITable mrsbr_financialSchema = new DatabaseTable("mrsbr_financial", DataProvider) { ClassName = "mrsbr_financial", SchemaName = "imp" };
            	mrsbr_financialSchema.Columns.Add(new DatabaseColumn("BusinessDate", mrsbr_financialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mrsbr_financialSchema.Columns.Add(new DatabaseColumn("AdjustmentReasonCode", mrsbr_financialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mrsbr_financialSchema.Columns.Add(new DatabaseColumn("AdjustmentYN", mrsbr_financialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mrsbr_financialSchema.Columns.Add(new DatabaseColumn("AmountPosted", mrsbr_financialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mrsbr_financialSchema.Columns.Add(new DatabaseColumn("ARCompressedYN", mrsbr_financialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mrsbr_financialSchema.Columns.Add(new DatabaseColumn("ARCreditAmount", mrsbr_financialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mrsbr_financialSchema.Columns.Add(new DatabaseColumn("ARDebitAmount", mrsbr_financialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mrsbr_financialSchema.Columns.Add(new DatabaseColumn("ARInvoiceNumber", mrsbr_financialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mrsbr_financialSchema.Columns.Add(new DatabaseColumn("ARStatus", mrsbr_financialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mrsbr_financialSchema.Columns.Add(new DatabaseColumn("ARTransferDate", mrsbr_financialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mrsbr_financialSchema.Columns.Add(new DatabaseColumn("ArrangementCode", mrsbr_financialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mrsbr_financialSchema.Columns.Add(new DatabaseColumn("ArrangementDescription", mrsbr_financialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mrsbr_financialSchema.Columns.Add(new DatabaseColumn("BillNo", mrsbr_financialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mrsbr_financialSchema.Columns.Add(new DatabaseColumn("CashierID", mrsbr_financialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mrsbr_financialSchema.Columns.Add(new DatabaseColumn("CheckNumber", mrsbr_financialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mrsbr_financialSchema.Columns.Add(new DatabaseColumn("CurrencyCode", mrsbr_financialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mrsbr_financialSchema.Columns.Add(new DatabaseColumn("DepositLedgerCredit", mrsbr_financialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mrsbr_financialSchema.Columns.Add(new DatabaseColumn("DepositLedgerDebit", mrsbr_financialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mrsbr_financialSchema.Columns.Add(new DatabaseColumn("DisplayYN", mrsbr_financialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mrsbr_financialSchema.Columns.Add(new DatabaseColumn("ExchangeRateExcludingCommission", mrsbr_financialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mrsbr_financialSchema.Columns.Add(new DatabaseColumn("ExchangeRateForTransaction", mrsbr_financialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mrsbr_financialSchema.Columns.Add(new DatabaseColumn("FixedChargesYN", mrsbr_financialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mrsbr_financialSchema.Columns.Add(new DatabaseColumn("FolioName", mrsbr_financialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mrsbr_financialSchema.Columns.Add(new DatabaseColumn("FolioWindow", mrsbr_financialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mrsbr_financialSchema.Columns.Add(new DatabaseColumn("FoodTax", mrsbr_financialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mrsbr_financialSchema.Columns.Add(new DatabaseColumn("ForeignExchangeCommissionAmount", mrsbr_financialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mrsbr_financialSchema.Columns.Add(new DatabaseColumn("ForeignExchangeCommissionPercent", mrsbr_financialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mrsbr_financialSchema.Columns.Add(new DatabaseColumn("ForeignExchangeType", mrsbr_financialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mrsbr_financialSchema.Columns.Add(new DatabaseColumn("Generates", mrsbr_financialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mrsbr_financialSchema.Columns.Add(new DatabaseColumn("GrossAmount", mrsbr_financialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mrsbr_financialSchema.Columns.Add(new DatabaseColumn("GuestAccountCredit", mrsbr_financialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mrsbr_financialSchema.Columns.Add(new DatabaseColumn("GuestAccountDebit", mrsbr_financialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mrsbr_financialSchema.Columns.Add(new DatabaseColumn("InsertDate", mrsbr_financialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mrsbr_financialSchema.Columns.Add(new DatabaseColumn("InsertUser", mrsbr_financialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mrsbr_financialSchema.Columns.Add(new DatabaseColumn("InternalYN", mrsbr_financialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mrsbr_financialSchema.Columns.Add(new DatabaseColumn("InvoiceClosingDate", mrsbr_financialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mrsbr_financialSchema.Columns.Add(new DatabaseColumn("InvoiceType", mrsbr_financialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mrsbr_financialSchema.Columns.Add(new DatabaseColumn("MarketCode", mrsbr_financialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mrsbr_financialSchema.Columns.Add(new DatabaseColumn("MinibarTax", mrsbr_financialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mrsbr_financialSchema.Columns.Add(new DatabaseColumn("NetAmount", mrsbr_financialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mrsbr_financialSchema.Columns.Add(new DatabaseColumn("OtherTax", mrsbr_financialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mrsbr_financialSchema.Columns.Add(new DatabaseColumn("PackageAccountCredit", mrsbr_financialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mrsbr_financialSchema.Columns.Add(new DatabaseColumn("PackageAccountDebit", mrsbr_financialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mrsbr_financialSchema.Columns.Add(new DatabaseColumn("ParrallelCurrencyCode", mrsbr_financialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mrsbr_financialSchema.Columns.Add(new DatabaseColumn("Price", mrsbr_financialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mrsbr_financialSchema.Columns.Add(new DatabaseColumn("Product", mrsbr_financialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mrsbr_financialSchema.Columns.Add(new DatabaseColumn("Property", mrsbr_financialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mrsbr_financialSchema.Columns.Add(new DatabaseColumn("QuantityOfProduct", mrsbr_financialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mrsbr_financialSchema.Columns.Add(new DatabaseColumn("RateCode", mrsbr_financialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mrsbr_financialSchema.Columns.Add(new DatabaseColumn("ReceiptNumber", mrsbr_financialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mrsbr_financialSchema.Columns.Add(new DatabaseColumn("Reference", mrsbr_financialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mrsbr_financialSchema.Columns.Add(new DatabaseColumn("Remark", mrsbr_financialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mrsbr_financialSchema.Columns.Add(new DatabaseColumn("RevenueAmount", mrsbr_financialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mrsbr_financialSchema.Columns.Add(new DatabaseColumn("RoomClass", mrsbr_financialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mrsbr_financialSchema.Columns.Add(new DatabaseColumn("RoomNumber", mrsbr_financialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mrsbr_financialSchema.Columns.Add(new DatabaseColumn("RoomTax", mrsbr_financialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mrsbr_financialSchema.Columns.Add(new DatabaseColumn("SourceCode", mrsbr_financialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mrsbr_financialSchema.Columns.Add(new DatabaseColumn("SummaryReferenceCode", mrsbr_financialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mrsbr_financialSchema.Columns.Add(new DatabaseColumn("TACommissionable", mrsbr_financialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mrsbr_financialSchema.Columns.Add(new DatabaseColumn("TaxDeferredYN", mrsbr_financialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mrsbr_financialSchema.Columns.Add(new DatabaseColumn("TaxGeneratedYN", mrsbr_financialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mrsbr_financialSchema.Columns.Add(new DatabaseColumn("TaxInclusiveYN", mrsbr_financialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mrsbr_financialSchema.Columns.Add(new DatabaseColumn("TransactionAmount", mrsbr_financialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mrsbr_financialSchema.Columns.Add(new DatabaseColumn("TransactionCode", mrsbr_financialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mrsbr_financialSchema.Columns.Add(new DatabaseColumn("TransactionCodeDescription", mrsbr_financialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mrsbr_financialSchema.Columns.Add(new DatabaseColumn("TransactionCodeGroup", mrsbr_financialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mrsbr_financialSchema.Columns.Add(new DatabaseColumn("TransactionCodeSubgroup", mrsbr_financialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mrsbr_financialSchema.Columns.Add(new DatabaseColumn("TransactionDate", mrsbr_financialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mrsbr_financialSchema.Columns.Add(new DatabaseColumn("TransactionGroupType", mrsbr_financialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mrsbr_financialSchema.Columns.Add(new DatabaseColumn("TransactionType", mrsbr_financialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mrsbr_financialSchema.Columns.Add(new DatabaseColumn("TransferDate", mrsbr_financialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mrsbr_financialSchema.Columns.Add(new DatabaseColumn("GrossAmount2", mrsbr_financialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mrsbr_financialSchema.Columns.Add(new DatabaseColumn("NetAmount2", mrsbr_financialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mrsbr_financialSchema.Columns.Add(new DatabaseColumn("UpdateDate", mrsbr_financialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mrsbr_financialSchema.Columns.Add(new DatabaseColumn("UpdateUser", mrsbr_financialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mrsbr_financialSchema.Columns.Add(new DatabaseColumn("mrsbr_financialID", mrsbr_financialSchema)
												{
													IsPrimaryKey = true,
													DataType = DbType.Guid,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	mrsbr_financialSchema.Columns.Add(new DatabaseColumn("DataSource", mrsbr_financialSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
				// Add mrsbr_financial to schema
            	DataProvider.Schema.Tables.Add(mrsbr_financialSchema);
				
				// Table: OperaA214
				// Primary Key: OperaA214ID
				ITable OperaA214Schema = new DatabaseTable("OperaA214", DataProvider) { ClassName = "OperaA214", SchemaName = "imp" };
            	OperaA214Schema.Columns.Add(new DatabaseColumn("NO_OF_STAYS", OperaA214Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaA214Schema.Columns.Add(new DatabaseColumn("NO_OF_ROOMS", OperaA214Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaA214Schema.Columns.Add(new DatabaseColumn("NIGHTS", OperaA214Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaA214Schema.Columns.Add(new DatabaseColumn("ADULTS", OperaA214Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaA214Schema.Columns.Add(new DatabaseColumn("CHILDREN", OperaA214Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaA214Schema.Columns.Add(new DatabaseColumn("RESERVATION_PREFERENCES", OperaA214Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaA214Schema.Columns.Add(new DatabaseColumn("SPECIAL_REQUESTS", OperaA214Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaA214Schema.Columns.Add(new DatabaseColumn("COMMENTS", OperaA214Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaA214Schema.Columns.Add(new DatabaseColumn("FIXED_CHARGE", OperaA214Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaA214Schema.Columns.Add(new DatabaseColumn("MEMBERSHIP_LEVEL", OperaA214Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaA214Schema.Columns.Add(new DatabaseColumn("PAYMENT_METHOD", OperaA214Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaA214Schema.Columns.Add(new DatabaseColumn("TRUNC_BEGIN", OperaA214Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaA214Schema.Columns.Add(new DatabaseColumn("ARRIVAL_TIME", OperaA214Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaA214Schema.Columns.Add(new DatabaseColumn("C_T_S_NAME", OperaA214Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaA214Schema.Columns.Add(new DatabaseColumn("UDFC30", OperaA214Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaA214Schema.Columns.Add(new DatabaseColumn("MEMBERSHIP_NUMBER", OperaA214Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaA214Schema.Columns.Add(new DatabaseColumn("MEMBERSHIP_TYPE", OperaA214Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaA214Schema.Columns.Add(new DatabaseColumn("RATE_CODE1", OperaA214Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaA214Schema.Columns.Add(new DatabaseColumn("COMM_CODE", OperaA214Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaA214Schema.Columns.Add(new DatabaseColumn("ROOM", OperaA214Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaA214Schema.Columns.Add(new DatabaseColumn("FULL_NAME", OperaA214Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaA214Schema.Columns.Add(new DatabaseColumn("DEPARTURE", OperaA214Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaA214Schema.Columns.Add(new DatabaseColumn("COMPANY_NAME", OperaA214Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaA214Schema.Columns.Add(new DatabaseColumn("TRAVEL_AGENT_NAME", OperaA214Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaA214Schema.Columns.Add(new DatabaseColumn("PERSONS", OperaA214Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaA214Schema.Columns.Add(new DatabaseColumn("ROOM_CATEGORY_LABEL", OperaA214Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaA214Schema.Columns.Add(new DatabaseColumn("BLOCK_CODE", OperaA214Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaA214Schema.Columns.Add(new DatabaseColumn("EFFECTIVE_RATE_AMOUNT", OperaA214Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaA214Schema.Columns.Add(new DatabaseColumn("VIP", OperaA214Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaA214Schema.Columns.Add(new DatabaseColumn("COMPUTED_RESV_STATUS", OperaA214Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaA214Schema.Columns.Add(new DatabaseColumn("COMPANY_ID", OperaA214Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaA214Schema.Columns.Add(new DatabaseColumn("TRAVEL_AGENT_ID", OperaA214Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaA214Schema.Columns.Add(new DatabaseColumn("GROUP_NAME", OperaA214Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaA214Schema.Columns.Add(new DatabaseColumn("GROUP_ID", OperaA214Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaA214Schema.Columns.Add(new DatabaseColumn("SOURCE_NAME", OperaA214Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaA214Schema.Columns.Add(new DatabaseColumn("SOURCE_ID", OperaA214Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaA214Schema.Columns.Add(new DatabaseColumn("MARKET_CODE", OperaA214Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaA214Schema.Columns.Add(new DatabaseColumn("CONFIRMATION_NO", OperaA214Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaA214Schema.Columns.Add(new DatabaseColumn("RESV_NAME_ID", OperaA214Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaA214Schema.Columns.Add(new DatabaseColumn("GUEST_NAME_ID", OperaA214Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaA214Schema.Columns.Add(new DatabaseColumn("ARRIVAL", OperaA214Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaA214Schema.Columns.Add(new DatabaseColumn("CF_PERSONS", OperaA214Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaA214Schema.Columns.Add(new DatabaseColumn("CF_SPECIALS", OperaA214Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaA214Schema.Columns.Add(new DatabaseColumn("OperaA214ID", OperaA214Schema)
												{
													IsPrimaryKey = true,
													DataType = DbType.Guid,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaA214Schema.Columns.Add(new DatabaseColumn("DataSource", OperaA214Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
				// Add OperaA214 to schema
            	DataProvider.Schema.Tables.Add(OperaA214Schema);
				
				// Table: OperaAD120BusinessBlock
				// Primary Key: AD120BusinessBlockID
				ITable OperaAD120BusinessBlockSchema = new DatabaseTable("OperaAD120BusinessBlock", DataProvider) { ClassName = "OperaAD120BusinessBlock", SchemaName = "imp" };
            	OperaAD120BusinessBlockSchema.Columns.Add(new DatabaseColumn("BLOCK_CODE", OperaAD120BusinessBlockSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaAD120BusinessBlockSchema.Columns.Add(new DatabaseColumn("RESORT", OperaAD120BusinessBlockSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaAD120BusinessBlockSchema.Columns.Add(new DatabaseColumn("ALLOTMENT_HEADER_ID", OperaAD120BusinessBlockSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaAD120BusinessBlockSchema.Columns.Add(new DatabaseColumn("DESCRIPTION", OperaAD120BusinessBlockSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaAD120BusinessBlockSchema.Columns.Add(new DatabaseColumn("START_DATE", OperaAD120BusinessBlockSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaAD120BusinessBlockSchema.Columns.Add(new DatabaseColumn("END_DATE", OperaAD120BusinessBlockSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaAD120BusinessBlockSchema.Columns.Add(new DatabaseColumn("BOOKING_STATUS", OperaAD120BusinessBlockSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaAD120BusinessBlockSchema.Columns.Add(new DatabaseColumn("CAT_STATUS", OperaAD120BusinessBlockSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaAD120BusinessBlockSchema.Columns.Add(new DatabaseColumn("RMS_DECISION_DATE", OperaAD120BusinessBlockSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaAD120BusinessBlockSchema.Columns.Add(new DatabaseColumn("CAT_DECISION", OperaAD120BusinessBlockSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaAD120BusinessBlockSchema.Columns.Add(new DatabaseColumn("CUTOFF_DATE", OperaAD120BusinessBlockSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaAD120BusinessBlockSchema.Columns.Add(new DatabaseColumn("RMS_FOLLOWUP", OperaAD120BusinessBlockSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaAD120BusinessBlockSchema.Columns.Add(new DatabaseColumn("RMS_OWNER_CODE", OperaAD120BusinessBlockSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaAD120BusinessBlockSchema.Columns.Add(new DatabaseColumn("CAT_FOLLOWUP_DATE", OperaAD120BusinessBlockSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaAD120BusinessBlockSchema.Columns.Add(new DatabaseColumn("CAT_OWNER_CODE", OperaAD120BusinessBlockSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaAD120BusinessBlockSchema.Columns.Add(new DatabaseColumn("RANKING_CODE", OperaAD120BusinessBlockSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaAD120BusinessBlockSchema.Columns.Add(new DatabaseColumn("CONVERSION_CODE", OperaAD120BusinessBlockSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaAD120BusinessBlockSchema.Columns.Add(new DatabaseColumn("RMS_BLOCKED", OperaAD120BusinessBlockSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaAD120BusinessBlockSchema.Columns.Add(new DatabaseColumn("REV_BLOCKED", OperaAD120BusinessBlockSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaAD120BusinessBlockSchema.Columns.Add(new DatabaseColumn("AVG_RATE", OperaAD120BusinessBlockSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaAD120BusinessBlockSchema.Columns.Add(new DatabaseColumn("TOTAL_REVENUE", OperaAD120BusinessBlockSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaAD120BusinessBlockSchema.Columns.Add(new DatabaseColumn("REV_TYPE1", OperaAD120BusinessBlockSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaAD120BusinessBlockSchema.Columns.Add(new DatabaseColumn("REV_TYPE2", OperaAD120BusinessBlockSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaAD120BusinessBlockSchema.Columns.Add(new DatabaseColumn("REV_TYPE3", OperaAD120BusinessBlockSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaAD120BusinessBlockSchema.Columns.Add(new DatabaseColumn("REV_TYPE4", OperaAD120BusinessBlockSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaAD120BusinessBlockSchema.Columns.Add(new DatabaseColumn("CF_TOTAL_REVENUE", OperaAD120BusinessBlockSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaAD120BusinessBlockSchema.Columns.Add(new DatabaseColumn("CF_CONTACTNAME", OperaAD120BusinessBlockSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaAD120BusinessBlockSchema.Columns.Add(new DatabaseColumn("CF_ACCOUNTNAME", OperaAD120BusinessBlockSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaAD120BusinessBlockSchema.Columns.Add(new DatabaseColumn("CAT_TOTAL", OperaAD120BusinessBlockSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaAD120BusinessBlockSchema.Columns.Add(new DatabaseColumn("AD120BusinessBlockID", OperaAD120BusinessBlockSchema)
												{
													IsPrimaryKey = true,
													DataType = DbType.Guid,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaAD120BusinessBlockSchema.Columns.Add(new DatabaseColumn("DataSource", OperaAD120BusinessBlockSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
				// Add OperaAD120BusinessBlock to schema
            	DataProvider.Schema.Tables.Add(OperaAD120BusinessBlockSchema);
				
				// Table: OperaAD160
				// Primary Key: OperaAD160ID
				ITable OperaAD160Schema = new DatabaseTable("OperaAD160", DataProvider) { ClassName = "OperaAD160", SchemaName = "imp" };
            	OperaAD160Schema.Columns.Add(new DatabaseColumn("SHORT_DATE1", OperaAD160Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaAD160Schema.Columns.Add(new DatabaseColumn("CF_STARTDATE", OperaAD160Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaAD160Schema.Columns.Add(new DatabaseColumn("BOOKINGNAME", OperaAD160Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaAD160Schema.Columns.Add(new DatabaseColumn("STARTDATE", OperaAD160Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaAD160Schema.Columns.Add(new DatabaseColumn("ENDDATE", OperaAD160Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaAD160Schema.Columns.Add(new DatabaseColumn("ALLOTMENTHEADERID", OperaAD160Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaAD160Schema.Columns.Add(new DatabaseColumn("BOOKSTATUS", OperaAD160Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaAD160Schema.Columns.Add(new DatabaseColumn("CATSTATUS", OperaAD160Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaAD160Schema.Columns.Add(new DatabaseColumn("ACCOUNTNAME", OperaAD160Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaAD160Schema.Columns.Add(new DatabaseColumn("REPRESENTATIVE", OperaAD160Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaAD160Schema.Columns.Add(new DatabaseColumn("ACCOUNTNAMEID", OperaAD160Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaAD160Schema.Columns.Add(new DatabaseColumn("CONTACTNAMEID", OperaAD160Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaAD160Schema.Columns.Add(new DatabaseColumn("RESORT", OperaAD160Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaAD160Schema.Columns.Add(new DatabaseColumn("CF_ROOM", OperaAD160Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaAD160Schema.Columns.Add(new DatabaseColumn("CF_PM", OperaAD160Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaAD160Schema.Columns.Add(new DatabaseColumn("CF_BOOKCATSTATUS", OperaAD160Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaAD160Schema.Columns.Add(new DatabaseColumn("CF_CATSALMANAGER", OperaAD160Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaAD160Schema.Columns.Add(new DatabaseColumn("CF_DISTRIBUTED", OperaAD160Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaAD160Schema.Columns.Add(new DatabaseColumn("CF_TEMP_SALMANAGER", OperaAD160Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaAD160Schema.Columns.Add(new DatabaseColumn("CF_TEMP_CATMANAGER", OperaAD160Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaAD160Schema.Columns.Add(new DatabaseColumn("EVENT_LINK_TYPE", OperaAD160Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaAD160Schema.Columns.Add(new DatabaseColumn("EVENT_LINK_ID", OperaAD160Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaAD160Schema.Columns.Add(new DatabaseColumn("EVENT_RESORT", OperaAD160Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaAD160Schema.Columns.Add(new DatabaseColumn("EVENTTYPE", OperaAD160Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaAD160Schema.Columns.Add(new DatabaseColumn("SETUPTIME", OperaAD160Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaAD160Schema.Columns.Add(new DatabaseColumn("SETDOWNTIME", OperaAD160Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaAD160Schema.Columns.Add(new DatabaseColumn("EVENTNAME", OperaAD160Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaAD160Schema.Columns.Add(new DatabaseColumn("EVENTID", OperaAD160Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaAD160Schema.Columns.Add(new DatabaseColumn("EVENTSTATUS", OperaAD160Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaAD160Schema.Columns.Add(new DatabaseColumn("EVENTROOM", OperaAD160Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaAD160Schema.Columns.Add(new DatabaseColumn("ATTENDEES", OperaAD160Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaAD160Schema.Columns.Add(new DatabaseColumn("EXPECTED_ATTENDEES", OperaAD160Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaAD160Schema.Columns.Add(new DatabaseColumn("ACTUAL_ATTENDEES", OperaAD160Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaAD160Schema.Columns.Add(new DatabaseColumn("GUARANTEED_ATTENDEES", OperaAD160Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaAD160Schema.Columns.Add(new DatabaseColumn("ROOMSETUP", OperaAD160Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaAD160Schema.Columns.Add(new DatabaseColumn("CF_SETUPDOWN_TIME", OperaAD160Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaAD160Schema.Columns.Add(new DatabaseColumn("CF_TIME", OperaAD160Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaAD160Schema.Columns.Add(new DatabaseColumn("CF_TIME_SETUP_SETDOWN", OperaAD160Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaAD160Schema.Columns.Add(new DatabaseColumn("CF_DOORCARD", OperaAD160Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaAD160Schema.Columns.Add(new DatabaseColumn("CF_EVENTROOMNAME", OperaAD160Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaAD160Schema.Columns.Add(new DatabaseColumn("CF_EVENTROOMSETUP", OperaAD160Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaAD160Schema.Columns.Add(new DatabaseColumn("EVENT_ID", OperaAD160Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaAD160Schema.Columns.Add(new DatabaseColumn("NOTES", OperaAD160Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaAD160Schema.Columns.Add(new DatabaseColumn("OperaAD160ID", OperaAD160Schema)
												{
													IsPrimaryKey = true,
													DataType = DbType.Guid,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaAD160Schema.Columns.Add(new DatabaseColumn("DataSource", OperaAD160Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
				// Add OperaAD160 to schema
            	DataProvider.Schema.Tables.Add(OperaAD160Schema);
				
				// Table: OperaD114
				// Primary Key: OperaD114ID
				ITable OperaD114Schema = new DatabaseTable("OperaD114", DataProvider) { ClassName = "OperaD114", SchemaName = "imp" };
            	OperaD114Schema.Columns.Add(new DatabaseColumn("LAST_YEAR", OperaD114Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaD114Schema.Columns.Add(new DatabaseColumn("GRP1_SORT_COL1", OperaD114Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaD114Schema.Columns.Add(new DatabaseColumn("GRP1_SORT_COL2", OperaD114Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaD114Schema.Columns.Add(new DatabaseColumn("TC_GROUP", OperaD114Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaD114Schema.Columns.Add(new DatabaseColumn("GRP2_SORT_COL1", OperaD114Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaD114Schema.Columns.Add(new DatabaseColumn("GRP2_SORT_COL2", OperaD114Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaD114Schema.Columns.Add(new DatabaseColumn("TC_SUBGROUP", OperaD114Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaD114Schema.Columns.Add(new DatabaseColumn("SORT_TRX_CODE", OperaD114Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaD114Schema.Columns.Add(new DatabaseColumn("TRX_CODE", OperaD114Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaD114Schema.Columns.Add(new DatabaseColumn("DESCRIPTION", OperaD114Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaD114Schema.Columns.Add(new DatabaseColumn("HEADING_ORDER", OperaD114Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaD114Schema.Columns.Add(new DatabaseColumn("HEADING1", OperaD114Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaD114Schema.Columns.Add(new DatabaseColumn("HEADER_YEAR", OperaD114Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaD114Schema.Columns.Add(new DatabaseColumn("HEADING2", OperaD114Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaD114Schema.Columns.Add(new DatabaseColumn("SUM_AMT_TOTAL", OperaD114Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaD114Schema.Columns.Add(new DatabaseColumn("SUM_AMT_GROUP", OperaD114Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaD114Schema.Columns.Add(new DatabaseColumn("SUM_AMT_SUBGROUP", OperaD114Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaD114Schema.Columns.Add(new DatabaseColumn("SUM_CUR_AMT_TOTAL", OperaD114Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaD114Schema.Columns.Add(new DatabaseColumn("SUM_CUR_AMT_GROUP", OperaD114Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaD114Schema.Columns.Add(new DatabaseColumn("SUM_CUR_AMT_SUBGROUP", OperaD114Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaD114Schema.Columns.Add(new DatabaseColumn("TRX_TYPE", OperaD114Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaD114Schema.Columns.Add(new DatabaseColumn("AMOUNT", OperaD114Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaD114Schema.Columns.Add(new DatabaseColumn("CUR_AMOUNT", OperaD114Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaD114Schema.Columns.Add(new DatabaseColumn("OperaD114ID", OperaD114Schema)
												{
													IsPrimaryKey = true,
													DataType = DbType.Guid,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaD114Schema.Columns.Add(new DatabaseColumn("DataSource", OperaD114Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
				// Add OperaD114 to schema
            	DataProvider.Schema.Tables.Add(OperaD114Schema);
				
				// Table: OperaD140
				// Primary Key: OperaD140ID
				ITable OperaD140Schema = new DatabaseTable("OperaD140", DataProvider) { ClassName = "OperaD140", SchemaName = "imp" };
            	OperaD140Schema.Columns.Add(new DatabaseColumn("IS_INTERNAL_YN", OperaD140Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaD140Schema.Columns.Add(new DatabaseColumn("INTERNAL_DEBIT", OperaD140Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaD140Schema.Columns.Add(new DatabaseColumn("INTERNAL_CREDIT", OperaD140Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaD140Schema.Columns.Add(new DatabaseColumn("FIRST", OperaD140Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaD140Schema.Columns.Add(new DatabaseColumn("FIRST_DEBIT", OperaD140Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaD140Schema.Columns.Add(new DatabaseColumn("FIRST_CREDIT", OperaD140Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaD140Schema.Columns.Add(new DatabaseColumn("SECOND", OperaD140Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaD140Schema.Columns.Add(new DatabaseColumn("SECOND_DEBIT", OperaD140Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaD140Schema.Columns.Add(new DatabaseColumn("SECOND_CREDIT", OperaD140Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaD140Schema.Columns.Add(new DatabaseColumn("THIRD", OperaD140Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaD140Schema.Columns.Add(new DatabaseColumn("THIRD_DEBIT", OperaD140Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaD140Schema.Columns.Add(new DatabaseColumn("THIRD_CREDIT", OperaD140Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaD140Schema.Columns.Add(new DatabaseColumn("EXP_DATE", OperaD140Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaD140Schema.Columns.Add(new DatabaseColumn("RECEIPT_NO", OperaD140Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaD140Schema.Columns.Add(new DatabaseColumn("GUEST_FULL_NAME", OperaD140Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaD140Schema.Columns.Add(new DatabaseColumn("TARGET_RESORT", OperaD140Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaD140Schema.Columns.Add(new DatabaseColumn("TRX_DESC", OperaD140Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaD140Schema.Columns.Add(new DatabaseColumn("MARKET_CODE", OperaD140Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaD140Schema.Columns.Add(new DatabaseColumn("BUSINESS_FORMAT_DATE", OperaD140Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaD140Schema.Columns.Add(new DatabaseColumn("BUSINESS_TIME", OperaD140Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.AnsiString,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaD140Schema.Columns.Add(new DatabaseColumn("BUSINESS_DATE", OperaD140Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.AnsiString,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaD140Schema.Columns.Add(new DatabaseColumn("REFERENCE", OperaD140Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaD140Schema.Columns.Add(new DatabaseColumn("TRX_NO", OperaD140Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaD140Schema.Columns.Add(new DatabaseColumn("CASHIER_DEBIT", OperaD140Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaD140Schema.Columns.Add(new DatabaseColumn("CASHIER_CREDIT", OperaD140Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaD140Schema.Columns.Add(new DatabaseColumn("ROOM", OperaD140Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaD140Schema.Columns.Add(new DatabaseColumn("CREDIT_CARD_SUPPLEMENT", OperaD140Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaD140Schema.Columns.Add(new DatabaseColumn("CURRENCY1", OperaD140Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaD140Schema.Columns.Add(new DatabaseColumn("TRX_CODE", OperaD140Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaD140Schema.Columns.Add(new DatabaseColumn("CASHIER_ID", OperaD140Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaD140Schema.Columns.Add(new DatabaseColumn("REMARK", OperaD140Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaD140Schema.Columns.Add(new DatabaseColumn("INSERT_USER", OperaD140Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaD140Schema.Columns.Add(new DatabaseColumn("INSERT_DATE", OperaD140Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaD140Schema.Columns.Add(new DatabaseColumn("CHEQUE_NUMBER", OperaD140Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaD140Schema.Columns.Add(new DatabaseColumn("ROOM_CLASS", OperaD140Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaD140Schema.Columns.Add(new DatabaseColumn("CC_CODE", OperaD140Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaD140Schema.Columns.Add(new DatabaseColumn("CASHIER_NAME", OperaD140Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaD140Schema.Columns.Add(new DatabaseColumn("USER_NAME", OperaD140Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaD140Schema.Columns.Add(new DatabaseColumn("DEP_NET_TAX_AMT", OperaD140Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaD140Schema.Columns.Add(new DatabaseColumn("DEPOSIT_DEBIT", OperaD140Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaD140Schema.Columns.Add(new DatabaseColumn("CASH_ID_USER_NAME", OperaD140Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaD140Schema.Columns.Add(new DatabaseColumn("PRINT_CASHIER_DEBIT", OperaD140Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaD140Schema.Columns.Add(new DatabaseColumn("PRINT_CASHIER_CREDIT", OperaD140Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaD140Schema.Columns.Add(new DatabaseColumn("OperaD140ID", OperaD140Schema)
												{
													IsPrimaryKey = true,
													DataType = DbType.Guid,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaD140Schema.Columns.Add(new DatabaseColumn("DataSource", OperaD140Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
				// Add OperaD140 to schema
            	DataProvider.Schema.Tables.Add(OperaD140Schema);
				
				// Table: OperaF116
				// Primary Key: OperaF116ID
				ITable OperaF116Schema = new DatabaseTable("OperaF116", DataProvider) { ClassName = "OperaF116", SchemaName = "imp" };
            	OperaF116Schema.Columns.Add(new DatabaseColumn("MASTER_VALUE_ORDER", OperaF116Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaF116Schema.Columns.Add(new DatabaseColumn("MASTER_VALUE", OperaF116Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaF116Schema.Columns.Add(new DatabaseColumn("RESORT", OperaF116Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaF116Schema.Columns.Add(new DatabaseColumn("CS_HEADING_COUNT_MASTER", OperaF116Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaF116Schema.Columns.Add(new DatabaseColumn("CS_FS_ARR_ROOMS_MASTER", OperaF116Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaF116Schema.Columns.Add(new DatabaseColumn("CS_FS_DEP_ROOMS_MASTER", OperaF116Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaF116Schema.Columns.Add(new DatabaseColumn("CS_FS_NO_ROOMS_MASTER", OperaF116Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaF116Schema.Columns.Add(new DatabaseColumn("CS_FS_GUESTS_MASTER", OperaF116Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaF116Schema.Columns.Add(new DatabaseColumn("CS_FS_TOTAL_REVENUE_MASTER", OperaF116Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaF116Schema.Columns.Add(new DatabaseColumn("CS_FS_ROOM_REVENUE_MASTER", OperaF116Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaF116Schema.Columns.Add(new DatabaseColumn("CS_FS_INVENTORY_ROOMS_MASTER", OperaF116Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaF116Schema.Columns.Add(new DatabaseColumn("CF_FS_PERC_OCC_ROOMS_MASTER", OperaF116Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaF116Schema.Columns.Add(new DatabaseColumn("CF_FS_AVG_ROOM_RATE_MASTER", OperaF116Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaF116Schema.Columns.Add(new DatabaseColumn("LAST_YEAR_01", OperaF116Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaF116Schema.Columns.Add(new DatabaseColumn("SUB_GRP_1_ORDER", OperaF116Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaF116Schema.Columns.Add(new DatabaseColumn("SUB_GRP_1", OperaF116Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaF116Schema.Columns.Add(new DatabaseColumn("DESCRIPTION", OperaF116Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaF116Schema.Columns.Add(new DatabaseColumn("AMOUNT_FORMAT_TYPE", OperaF116Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaF116Schema.Columns.Add(new DatabaseColumn("PRINT_LINE_AFTER_YN", OperaF116Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaF116Schema.Columns.Add(new DatabaseColumn("HEADING_1_ORDER", OperaF116Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaF116Schema.Columns.Add(new DatabaseColumn("HEADING_1", OperaF116Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaF116Schema.Columns.Add(new DatabaseColumn("HEADING_2", OperaF116Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaF116Schema.Columns.Add(new DatabaseColumn("SUM_AMOUNT", OperaF116Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaF116Schema.Columns.Add(new DatabaseColumn("FORMATTED_AMOUNT", OperaF116Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaF116Schema.Columns.Add(new DatabaseColumn("OperaF116ID", OperaF116Schema)
												{
													IsPrimaryKey = true,
													DataType = DbType.Guid,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaF116Schema.Columns.Add(new DatabaseColumn("DataSource", OperaF116Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
				// Add OperaF116 to schema
            	DataProvider.Schema.Tables.Add(OperaF116Schema);
				
				// Table: OperaH260
				// Primary Key: OperaH260ID
				ITable OperaH260Schema = new DatabaseTable("OperaH260", DataProvider) { ClassName = "OperaH260", SchemaName = "imp" };
            	OperaH260Schema.Columns.Add(new DatabaseColumn("REPORT_ACTION_TYPE", OperaH260Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaH260Schema.Columns.Add(new DatabaseColumn("REPORT_ACTION_DESCRIPTION", OperaH260Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaH260Schema.Columns.Add(new DatabaseColumn("CONFIRMATION_NO", OperaH260Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaH260Schema.Columns.Add(new DatabaseColumn("ACTION_INSTANCE_ID", OperaH260Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaH260Schema.Columns.Add(new DatabaseColumn("CHAR_INSERT_DATE", OperaH260Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaH260Schema.Columns.Add(new DatabaseColumn("CHAR_INSERT_TIME", OperaH260Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaH260Schema.Columns.Add(new DatabaseColumn("ROOM", OperaH260Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaH260Schema.Columns.Add(new DatabaseColumn("INSERT_DATE", OperaH260Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaH260Schema.Columns.Add(new DatabaseColumn("LOG_USER", OperaH260Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaH260Schema.Columns.Add(new DatabaseColumn("ACTION_DESCRIPTION", OperaH260Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaH260Schema.Columns.Add(new DatabaseColumn("ACTION_TYPE", OperaH260Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaH260Schema.Columns.Add(new DatabaseColumn("OperaH260ID", OperaH260Schema)
												{
													IsPrimaryKey = true,
													DataType = DbType.Guid,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaH260Schema.Columns.Add(new DatabaseColumn("DataSource", OperaH260Schema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
				// Add OperaH260 to schema
            	DataProvider.Schema.Tables.Add(OperaH260Schema);
				
				// Table: OperaP112Departures
				// Primary Key: P112DeparturesID
				ITable OperaP112DeparturesSchema = new DatabaseTable("OperaP112Departures", DataProvider) { ClassName = "OperaP112Departure", SchemaName = "imp" };
            	OperaP112DeparturesSchema.Columns.Add(new DatabaseColumn("DEPARTURE", OperaP112DeparturesSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaP112DeparturesSchema.Columns.Add(new DatabaseColumn("GRP_BY_COL", OperaP112DeparturesSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaP112DeparturesSchema.Columns.Add(new DatabaseColumn("GRP_BY_DESC", OperaP112DeparturesSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaP112DeparturesSchema.Columns.Add(new DatabaseColumn("SEC_RMNO", OperaP112DeparturesSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaP112DeparturesSchema.Columns.Add(new DatabaseColumn("CHAR_DEPDATE", OperaP112DeparturesSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaP112DeparturesSchema.Columns.Add(new DatabaseColumn("SUM_CHD", OperaP112DeparturesSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaP112DeparturesSchema.Columns.Add(new DatabaseColumn("SUM_ADTS", OperaP112DeparturesSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaP112DeparturesSchema.Columns.Add(new DatabaseColumn("SUM_NTS", OperaP112DeparturesSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaP112DeparturesSchema.Columns.Add(new DatabaseColumn("SUM_RMS", OperaP112DeparturesSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaP112DeparturesSchema.Columns.Add(new DatabaseColumn("SUM_BALANCE", OperaP112DeparturesSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaP112DeparturesSchema.Columns.Add(new DatabaseColumn("RESORT1", OperaP112DeparturesSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaP112DeparturesSchema.Columns.Add(new DatabaseColumn("IS_SHARED_YN", OperaP112DeparturesSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaP112DeparturesSchema.Columns.Add(new DatabaseColumn("ROOM", OperaP112DeparturesSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaP112DeparturesSchema.Columns.Add(new DatabaseColumn("NIGHTS", OperaP112DeparturesSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaP112DeparturesSchema.Columns.Add(new DatabaseColumn("ARRIVAL", OperaP112DeparturesSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaP112DeparturesSchema.Columns.Add(new DatabaseColumn("NO_OF_ROOMS", OperaP112DeparturesSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaP112DeparturesSchema.Columns.Add(new DatabaseColumn("BALANCE", OperaP112DeparturesSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaP112DeparturesSchema.Columns.Add(new DatabaseColumn("RESV_STATUS", OperaP112DeparturesSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaP112DeparturesSchema.Columns.Add(new DatabaseColumn("DEPARTURE_TIME", OperaP112DeparturesSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaP112DeparturesSchema.Columns.Add(new DatabaseColumn("COMPUTED_RESV_STATUS", OperaP112DeparturesSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaP112DeparturesSchema.Columns.Add(new DatabaseColumn("GUEST_NAME", OperaP112DeparturesSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaP112DeparturesSchema.Columns.Add(new DatabaseColumn("ADULTS", OperaP112DeparturesSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaP112DeparturesSchema.Columns.Add(new DatabaseColumn("CHILDREN", OperaP112DeparturesSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaP112DeparturesSchema.Columns.Add(new DatabaseColumn("BLOCK_CODE", OperaP112DeparturesSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaP112DeparturesSchema.Columns.Add(new DatabaseColumn("ALLOTMENT_HEADER_ID", OperaP112DeparturesSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaP112DeparturesSchema.Columns.Add(new DatabaseColumn("ROOM_CATEGORY_LABEL", OperaP112DeparturesSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaP112DeparturesSchema.Columns.Add(new DatabaseColumn("COMPANY_NAME", OperaP112DeparturesSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaP112DeparturesSchema.Columns.Add(new DatabaseColumn("TRAVEL_AGENT_NAME", OperaP112DeparturesSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaP112DeparturesSchema.Columns.Add(new DatabaseColumn("SOURCE_NAME", OperaP112DeparturesSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaP112DeparturesSchema.Columns.Add(new DatabaseColumn("GROUP_NAME", OperaP112DeparturesSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaP112DeparturesSchema.Columns.Add(new DatabaseColumn("ROOM_CATEGORY", OperaP112DeparturesSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaP112DeparturesSchema.Columns.Add(new DatabaseColumn("PAYMENT_DESC", OperaP112DeparturesSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaP112DeparturesSchema.Columns.Add(new DatabaseColumn("RESV_NAME_ID", OperaP112DeparturesSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaP112DeparturesSchema.Columns.Add(new DatabaseColumn("GUEST_NAME_ID", OperaP112DeparturesSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaP112DeparturesSchema.Columns.Add(new DatabaseColumn("COMPUTED_RESV_STATUS_DISPLAY", OperaP112DeparturesSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaP112DeparturesSchema.Columns.Add(new DatabaseColumn("RATE_CODE", OperaP112DeparturesSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaP112DeparturesSchema.Columns.Add(new DatabaseColumn("SPECIAL_REQUESTS", OperaP112DeparturesSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaP112DeparturesSchema.Columns.Add(new DatabaseColumn("VIP", OperaP112DeparturesSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaP112DeparturesSchema.Columns.Add(new DatabaseColumn("SHARE_NAMES", OperaP112DeparturesSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaP112DeparturesSchema.Columns.Add(new DatabaseColumn("EXTERNAL_REFERENCE", OperaP112DeparturesSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaP112DeparturesSchema.Columns.Add(new DatabaseColumn("CHAR_DEPART", OperaP112DeparturesSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaP112DeparturesSchema.Columns.Add(new DatabaseColumn("CHAR_ARRIVAL", OperaP112DeparturesSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaP112DeparturesSchema.Columns.Add(new DatabaseColumn("PROF_ATTACHED", OperaP112DeparturesSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaP112DeparturesSchema.Columns.Add(new DatabaseColumn("PROF_COUNT", OperaP112DeparturesSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaP112DeparturesSchema.Columns.Add(new DatabaseColumn("RES_COUNT", OperaP112DeparturesSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaP112DeparturesSchema.Columns.Add(new DatabaseColumn("RESV_NAME_ID1", OperaP112DeparturesSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaP112DeparturesSchema.Columns.Add(new DatabaseColumn("RESORT", OperaP112DeparturesSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaP112DeparturesSchema.Columns.Add(new DatabaseColumn("MEMBERSHIP_TYPE", OperaP112DeparturesSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaP112DeparturesSchema.Columns.Add(new DatabaseColumn("MEMBERSHIP_CARD_NO", OperaP112DeparturesSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaP112DeparturesSchema.Columns.Add(new DatabaseColumn("MEMBERSHIP_LEVEL", OperaP112DeparturesSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaP112DeparturesSchema.Columns.Add(new DatabaseColumn("P112DeparturesID", OperaP112DeparturesSchema)
												{
													IsPrimaryKey = true,
													DataType = DbType.Guid,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	OperaP112DeparturesSchema.Columns.Add(new DatabaseColumn("DataSource", OperaP112DeparturesSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
				// Add OperaP112Departures to schema
            	DataProvider.Schema.Tables.Add(OperaP112DeparturesSchema);
				
				// Table: POSTicketData
				// Primary Key: POSTicketDataID
				ITable POSTicketDataSchema = new DatabaseTable("POSTicketData", DataProvider) { ClassName = "POSTicketDatum", SchemaName = "imp" };
            	POSTicketDataSchema.Columns.Add(new DatabaseColumn("CheckNumber", POSTicketDataSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	POSTicketDataSchema.Columns.Add(new DatabaseColumn("CheckSequence", POSTicketDataSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int64,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	POSTicketDataSchema.Columns.Add(new DatabaseColumn("TicketDate", POSTicketDataSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	POSTicketDataSchema.Columns.Add(new DatabaseColumn("TransactionID", POSTicketDataSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	POSTicketDataSchema.Columns.Add(new DatabaseColumn("DetailSequence", POSTicketDataSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	POSTicketDataSchema.Columns.Add(new DatabaseColumn("ItemDescription", POSTicketDataSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	POSTicketDataSchema.Columns.Add(new DatabaseColumn("ItemDescriptionCont", POSTicketDataSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	POSTicketDataSchema.Columns.Add(new DatabaseColumn("SeatNumber", POSTicketDataSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	POSTicketDataSchema.Columns.Add(new DatabaseColumn("GuestCount", POSTicketDataSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	POSTicketDataSchema.Columns.Add(new DatabaseColumn("TicketTotal", POSTicketDataSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Currency,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	POSTicketDataSchema.Columns.Add(new DatabaseColumn("Tax", POSTicketDataSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Currency,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	POSTicketDataSchema.Columns.Add(new DatabaseColumn("SubTotal", POSTicketDataSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Currency,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	POSTicketDataSchema.Columns.Add(new DatabaseColumn("AmountDue", POSTicketDataSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Currency,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	POSTicketDataSchema.Columns.Add(new DatabaseColumn("ServerNumber", POSTicketDataSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	POSTicketDataSchema.Columns.Add(new DatabaseColumn("ServerFirstName", POSTicketDataSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	POSTicketDataSchema.Columns.Add(new DatabaseColumn("ServerLastName", POSTicketDataSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	POSTicketDataSchema.Columns.Add(new DatabaseColumn("ServerCheckName", POSTicketDataSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	POSTicketDataSchema.Columns.Add(new DatabaseColumn("Department", POSTicketDataSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	POSTicketDataSchema.Columns.Add(new DatabaseColumn("Category", POSTicketDataSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	POSTicketDataSchema.Columns.Add(new DatabaseColumn("POSTicketDataID", POSTicketDataSchema)
												{
													IsPrimaryKey = true,
													DataType = DbType.Guid,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	POSTicketDataSchema.Columns.Add(new DatabaseColumn("DataSource", POSTicketDataSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
				// Add POSTicketData to schema
            	DataProvider.Schema.Tables.Add(POSTicketDataSchema);
				
				// Table: PS550GuestList
				// Primary Key: PS550GuestListID
				ITable PS550GuestListSchema = new DatabaseTable("PS550GuestList", DataProvider) { ClassName = "PS550GuestList", SchemaName = "imp" };
            	PS550GuestListSchema.Columns.Add(new DatabaseColumn("EmailAddress", PS550GuestListSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	PS550GuestListSchema.Columns.Add(new DatabaseColumn("FullName", PS550GuestListSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	PS550GuestListSchema.Columns.Add(new DatabaseColumn("Address1", PS550GuestListSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	PS550GuestListSchema.Columns.Add(new DatabaseColumn("Address2", PS550GuestListSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	PS550GuestListSchema.Columns.Add(new DatabaseColumn("Address3", PS550GuestListSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	PS550GuestListSchema.Columns.Add(new DatabaseColumn("City", PS550GuestListSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	PS550GuestListSchema.Columns.Add(new DatabaseColumn("PostCode", PS550GuestListSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	PS550GuestListSchema.Columns.Add(new DatabaseColumn("State", PS550GuestListSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	PS550GuestListSchema.Columns.Add(new DatabaseColumn("Territory", PS550GuestListSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	PS550GuestListSchema.Columns.Add(new DatabaseColumn("VIPCode", PS550GuestListSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	PS550GuestListSchema.Columns.Add(new DatabaseColumn("MembershipLevel", PS550GuestListSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	PS550GuestListSchema.Columns.Add(new DatabaseColumn("MemberName", PS550GuestListSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	PS550GuestListSchema.Columns.Add(new DatabaseColumn("MarketCode", PS550GuestListSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	PS550GuestListSchema.Columns.Add(new DatabaseColumn("IndustryCode", PS550GuestListSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	PS550GuestListSchema.Columns.Add(new DatabaseColumn("ArrivalDate", PS550GuestListSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	PS550GuestListSchema.Columns.Add(new DatabaseColumn("DepartureDate", PS550GuestListSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	PS550GuestListSchema.Columns.Add(new DatabaseColumn("PS550GuestListID", PS550GuestListSchema)
												{
													IsPrimaryKey = true,
													DataType = DbType.Guid,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	PS550GuestListSchema.Columns.Add(new DatabaseColumn("DataSource", PS550GuestListSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
				// Add PS550GuestList to schema
            	DataProvider.Schema.Tables.Add(PS550GuestListSchema);
				
				// Table: rep_bh_short
				// Primary Key: rep_bh_shortID
				ITable rep_bh_shortSchema = new DatabaseTable("rep_bh_short", DataProvider) { ClassName = "rep_bh_short", SchemaName = "imp" };
            	rep_bh_shortSchema.Columns.Add(new DatabaseColumn("BLOCK_CODE", rep_bh_shortSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	rep_bh_shortSchema.Columns.Add(new DatabaseColumn("RESORT", rep_bh_shortSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	rep_bh_shortSchema.Columns.Add(new DatabaseColumn("ALLOTMENT_HEADER_ID", rep_bh_shortSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	rep_bh_shortSchema.Columns.Add(new DatabaseColumn("DESCRIPTION", rep_bh_shortSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	rep_bh_shortSchema.Columns.Add(new DatabaseColumn("START_DATE", rep_bh_shortSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	rep_bh_shortSchema.Columns.Add(new DatabaseColumn("END_DATE", rep_bh_shortSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	rep_bh_shortSchema.Columns.Add(new DatabaseColumn("BOOKING_STATUS", rep_bh_shortSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	rep_bh_shortSchema.Columns.Add(new DatabaseColumn("CAT_STATUS", rep_bh_shortSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	rep_bh_shortSchema.Columns.Add(new DatabaseColumn("RMS_DECISION_DATE", rep_bh_shortSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	rep_bh_shortSchema.Columns.Add(new DatabaseColumn("CAT_DECISION", rep_bh_shortSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	rep_bh_shortSchema.Columns.Add(new DatabaseColumn("CUTOFF_DATE", rep_bh_shortSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	rep_bh_shortSchema.Columns.Add(new DatabaseColumn("RMS_FOLLOWUP", rep_bh_shortSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	rep_bh_shortSchema.Columns.Add(new DatabaseColumn("RMS_OWNER_CODE", rep_bh_shortSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	rep_bh_shortSchema.Columns.Add(new DatabaseColumn("CAT_FOLLOWUP_DATE", rep_bh_shortSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	rep_bh_shortSchema.Columns.Add(new DatabaseColumn("CAT_OWNER_CODE", rep_bh_shortSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	rep_bh_shortSchema.Columns.Add(new DatabaseColumn("RANKING_CODE", rep_bh_shortSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	rep_bh_shortSchema.Columns.Add(new DatabaseColumn("CONVERSION_CODE", rep_bh_shortSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	rep_bh_shortSchema.Columns.Add(new DatabaseColumn("RMS_BLOCKED", rep_bh_shortSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	rep_bh_shortSchema.Columns.Add(new DatabaseColumn("REV_BLOCKED", rep_bh_shortSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	rep_bh_shortSchema.Columns.Add(new DatabaseColumn("AVG_RATE", rep_bh_shortSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	rep_bh_shortSchema.Columns.Add(new DatabaseColumn("TOTAL_REVENUE", rep_bh_shortSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	rep_bh_shortSchema.Columns.Add(new DatabaseColumn("REV_TYPE1", rep_bh_shortSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	rep_bh_shortSchema.Columns.Add(new DatabaseColumn("REV_TYPE2", rep_bh_shortSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	rep_bh_shortSchema.Columns.Add(new DatabaseColumn("REV_TYPE3", rep_bh_shortSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	rep_bh_shortSchema.Columns.Add(new DatabaseColumn("REV_TYPE4", rep_bh_shortSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	rep_bh_shortSchema.Columns.Add(new DatabaseColumn("CF_TOTAL_REVENUE", rep_bh_shortSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	rep_bh_shortSchema.Columns.Add(new DatabaseColumn("CF_CONTACTNAME", rep_bh_shortSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	rep_bh_shortSchema.Columns.Add(new DatabaseColumn("CF_ACCOUNTNAME", rep_bh_shortSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	rep_bh_shortSchema.Columns.Add(new DatabaseColumn("CAT_TOTAL", rep_bh_shortSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	rep_bh_shortSchema.Columns.Add(new DatabaseColumn("rep_bh_shortID", rep_bh_shortSchema)
												{
													IsPrimaryKey = true,
													DataType = DbType.Guid,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	rep_bh_shortSchema.Columns.Add(new DatabaseColumn("DataSource", rep_bh_shortSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
				// Add rep_bh_short to schema
            	DataProvider.Schema.Tables.Add(rep_bh_shortSchema);
				
				// Table: v_R_kds_chk_dtl
				// Primary Key: v_R_kds_chk_dtlID
				ITable v_R_kds_chk_dtlSchema = new DatabaseTable("v_R_kds_chk_dtl", DataProvider) { ClassName = "v_R_kds_chk_dtl", SchemaName = "imp" };
            	v_R_kds_chk_dtlSchema.Columns.Add(new DatabaseColumn("business_date", v_R_kds_chk_dtlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	v_R_kds_chk_dtlSchema.Columns.Add(new DatabaseColumn("rvc_num", v_R_kds_chk_dtlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	v_R_kds_chk_dtlSchema.Columns.Add(new DatabaseColumn("time_period_num", v_R_kds_chk_dtlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	v_R_kds_chk_dtlSchema.Columns.Add(new DatabaseColumn("time_period_name", v_R_kds_chk_dtlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	v_R_kds_chk_dtlSchema.Columns.Add(new DatabaseColumn("emp_num", v_R_kds_chk_dtlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	v_R_kds_chk_dtlSchema.Columns.Add(new DatabaseColumn("chk_seq", v_R_kds_chk_dtlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	v_R_kds_chk_dtlSchema.Columns.Add(new DatabaseColumn("v_R_kds_chk_dtlID", v_R_kds_chk_dtlSchema)
												{
													IsPrimaryKey = true,
													DataType = DbType.Guid,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	v_R_kds_chk_dtlSchema.Columns.Add(new DatabaseColumn("DataSource", v_R_kds_chk_dtlSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
				// Add v_R_kds_chk_dtl to schema
            	DataProvider.Schema.Tables.Add(v_R_kds_chk_dtlSchema);
				
				// Table: DailyPOSTopTen
				// Primary Key: 
				ITable DailyPOSTopTenSchema = new DatabaseTable("DailyPOSTopTen", DataProvider) { ClassName = "DailyPOSTopTen", SchemaName = "rpt" };
            	DailyPOSTopTenSchema.Columns.Add(new DatabaseColumn("BUSINESS_DATE", DailyPOSTopTenSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	DailyPOSTopTenSchema.Columns.Add(new DatabaseColumn("ESTABLISHMENT", DailyPOSTopTenSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	DailyPOSTopTenSchema.Columns.Add(new DatabaseColumn("PRODUCT", DailyPOSTopTenSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	DailyPOSTopTenSchema.Columns.Add(new DatabaseColumn("TotalPrice", DailyPOSTopTenSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	DailyPOSTopTenSchema.Columns.Add(new DatabaseColumn("TotalItems", DailyPOSTopTenSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	DailyPOSTopTenSchema.Columns.Add(new DatabaseColumn("OrderedItems", DailyPOSTopTenSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	DailyPOSTopTenSchema.Columns.Add(new DatabaseColumn("ReturnedItems", DailyPOSTopTenSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	DailyPOSTopTenSchema.Columns.Add(new DatabaseColumn("RowNum", DailyPOSTopTenSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int64,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
				// Add DailyPOSTopTen to schema
            	DataProvider.Schema.Tables.Add(DailyPOSTopTenSchema);
				
				// Table: vwDailyOperaTotals
				// Primary Key: 
				ITable vwDailyOperaTotalsSchema = new DatabaseTable("vwDailyOperaTotals", DataProvider) { ClassName = "vwDailyOperaTotal", SchemaName = "rpt" };
            	vwDailyOperaTotalsSchema.Columns.Add(new DatabaseColumn("Establishment", vwDailyOperaTotalsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDailyOperaTotalsSchema.Columns.Add(new DatabaseColumn("TicketCount", vwDailyOperaTotalsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDailyOperaTotalsSchema.Columns.Add(new DatabaseColumn("TotalGuests", vwDailyOperaTotalsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDailyOperaTotalsSchema.Columns.Add(new DatabaseColumn("MinTicket", vwDailyOperaTotalsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDailyOperaTotalsSchema.Columns.Add(new DatabaseColumn("MaxTicket", vwDailyOperaTotalsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDailyOperaTotalsSchema.Columns.Add(new DatabaseColumn("SumDiscounts", vwDailyOperaTotalsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDailyOperaTotalsSchema.Columns.Add(new DatabaseColumn("MinibarLost", vwDailyOperaTotalsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDailyOperaTotalsSchema.Columns.Add(new DatabaseColumn("Aminities", vwDailyOperaTotalsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDailyOperaTotalsSchema.Columns.Add(new DatabaseColumn("TicketTotal", vwDailyOperaTotalsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDailyOperaTotalsSchema.Columns.Add(new DatabaseColumn("Tips", vwDailyOperaTotalsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDailyOperaTotalsSchema.Columns.Add(new DatabaseColumn("TotalCredits", vwDailyOperaTotalsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDailyOperaTotalsSchema.Columns.Add(new DatabaseColumn("PaymentTotal", vwDailyOperaTotalsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDailyOperaTotalsSchema.Columns.Add(new DatabaseColumn("EstablishmentNumber", vwDailyOperaTotalsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDailyOperaTotalsSchema.Columns.Add(new DatabaseColumn("BusinessDate", vwDailyOperaTotalsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.AnsiString,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
				// Add vwDailyOperaTotals to schema
            	DataProvider.Schema.Tables.Add(vwDailyOperaTotalsSchema);
				
				// Table: vwDailyPOSItemSummary
				// Primary Key: 
				ITable vwDailyPOSItemSummarySchema = new DatabaseTable("vwDailyPOSItemSummary", DataProvider) { ClassName = "vwDailyPOSItemSummary", SchemaName = "rpt" };
            	vwDailyPOSItemSummarySchema.Columns.Add(new DatabaseColumn("BUSINESS_DATE", vwDailyPOSItemSummarySchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDailyPOSItemSummarySchema.Columns.Add(new DatabaseColumn("ESTABLISHMENT", vwDailyPOSItemSummarySchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDailyPOSItemSummarySchema.Columns.Add(new DatabaseColumn("PRODUCT", vwDailyPOSItemSummarySchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDailyPOSItemSummarySchema.Columns.Add(new DatabaseColumn("TotalPrice", vwDailyPOSItemSummarySchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDailyPOSItemSummarySchema.Columns.Add(new DatabaseColumn("TotalItems", vwDailyPOSItemSummarySchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDailyPOSItemSummarySchema.Columns.Add(new DatabaseColumn("OrderedItems", vwDailyPOSItemSummarySchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDailyPOSItemSummarySchema.Columns.Add(new DatabaseColumn("ReturnedItems", vwDailyPOSItemSummarySchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDailyPOSItemSummarySchema.Columns.Add(new DatabaseColumn("Name", vwDailyPOSItemSummarySchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDailyPOSItemSummarySchema.Columns.Add(new DatabaseColumn("maj_grp_seq", vwDailyPOSItemSummarySchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
				// Add vwDailyPOSItemSummary to schema
            	DataProvider.Schema.Tables.Add(vwDailyPOSItemSummarySchema);
				
				// Table: vwDailyPOSTopTen
				// Primary Key: 
				ITable vwDailyPOSTopTenSchema = new DatabaseTable("vwDailyPOSTopTen", DataProvider) { ClassName = "vwDailyPOSTopTen", SchemaName = "rpt" };
            	vwDailyPOSTopTenSchema.Columns.Add(new DatabaseColumn("BUSINESS_DATE", vwDailyPOSTopTenSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDailyPOSTopTenSchema.Columns.Add(new DatabaseColumn("ESTABLISHMENT", vwDailyPOSTopTenSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDailyPOSTopTenSchema.Columns.Add(new DatabaseColumn("PRODUCT", vwDailyPOSTopTenSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDailyPOSTopTenSchema.Columns.Add(new DatabaseColumn("TotalPrice", vwDailyPOSTopTenSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDailyPOSTopTenSchema.Columns.Add(new DatabaseColumn("TotalItems", vwDailyPOSTopTenSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDailyPOSTopTenSchema.Columns.Add(new DatabaseColumn("OrderedItems", vwDailyPOSTopTenSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDailyPOSTopTenSchema.Columns.Add(new DatabaseColumn("ReturnedItems", vwDailyPOSTopTenSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDailyPOSTopTenSchema.Columns.Add(new DatabaseColumn("RowNum", vwDailyPOSTopTenSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int64,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
				// Add vwDailyPOSTopTen to schema
            	DataProvider.Schema.Tables.Add(vwDailyPOSTopTenSchema);
				
				// Table: vwDailyPOSTotals
				// Primary Key: 
				ITable vwDailyPOSTotalsSchema = new DatabaseTable("vwDailyPOSTotals", DataProvider) { ClassName = "vwDailyPOSTotal", SchemaName = "rpt" };
            	vwDailyPOSTotalsSchema.Columns.Add(new DatabaseColumn("Establishment", vwDailyPOSTotalsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDailyPOSTotalsSchema.Columns.Add(new DatabaseColumn("TicketCount", vwDailyPOSTotalsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDailyPOSTotalsSchema.Columns.Add(new DatabaseColumn("TotalGuests", vwDailyPOSTotalsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDailyPOSTotalsSchema.Columns.Add(new DatabaseColumn("MinTicket", vwDailyPOSTotalsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDailyPOSTotalsSchema.Columns.Add(new DatabaseColumn("MaxTicket", vwDailyPOSTotalsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDailyPOSTotalsSchema.Columns.Add(new DatabaseColumn("SumDiscounts", vwDailyPOSTotalsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDailyPOSTotalsSchema.Columns.Add(new DatabaseColumn("MinibarLost", vwDailyPOSTotalsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDailyPOSTotalsSchema.Columns.Add(new DatabaseColumn("Aminities", vwDailyPOSTotalsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDailyPOSTotalsSchema.Columns.Add(new DatabaseColumn("TicketTotal", vwDailyPOSTotalsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDailyPOSTotalsSchema.Columns.Add(new DatabaseColumn("Tips", vwDailyPOSTotalsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDailyPOSTotalsSchema.Columns.Add(new DatabaseColumn("TotalCredits", vwDailyPOSTotalsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDailyPOSTotalsSchema.Columns.Add(new DatabaseColumn("PaymentTotal", vwDailyPOSTotalsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDailyPOSTotalsSchema.Columns.Add(new DatabaseColumn("EstablishmentNumber", vwDailyPOSTotalsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDailyPOSTotalsSchema.Columns.Add(new DatabaseColumn("BusinessDate", vwDailyPOSTotalsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.AnsiString,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
				// Add vwDailyPOSTotals to schema
            	DataProvider.Schema.Tables.Add(vwDailyPOSTotalsSchema);
				
				// Table: vwDiscountedItemsByReasonPOS
				// Primary Key: 
				ITable vwDiscountedItemsByReasonPOSSchema = new DatabaseTable("vwDiscountedItemsByReasonPOS", DataProvider) { ClassName = "vwDiscountedItemsByReasonPO", SchemaName = "rpt" };
            	vwDiscountedItemsByReasonPOSSchema.Columns.Add(new DatabaseColumn("reason_name", vwDiscountedItemsByReasonPOSSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDiscountedItemsByReasonPOSSchema.Columns.Add(new DatabaseColumn("ItemCount", vwDiscountedItemsByReasonPOSSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDiscountedItemsByReasonPOSSchema.Columns.Add(new DatabaseColumn("TotalDiscount", vwDiscountedItemsByReasonPOSSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDiscountedItemsByReasonPOSSchema.Columns.Add(new DatabaseColumn("AverageDiscount", vwDiscountedItemsByReasonPOSSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDiscountedItemsByReasonPOSSchema.Columns.Add(new DatabaseColumn("StartDate", vwDiscountedItemsByReasonPOSSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwDiscountedItemsByReasonPOSSchema.Columns.Add(new DatabaseColumn("EndDate", vwDiscountedItemsByReasonPOSSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
				// Add vwDiscountedItemsByReasonPOS to schema
            	DataProvider.Schema.Tables.Add(vwDiscountedItemsByReasonPOSSchema);
				
				// Table: vwOpera214Daily
				// Primary Key: 
				ITable vwOpera214DailySchema = new DatabaseTable("vwOpera214Daily", DataProvider) { ClassName = "vwOpera214Daily", SchemaName = "rpt" };
            	vwOpera214DailySchema.Columns.Add(new DatabaseColumn("RoomNo", vwOpera214DailySchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwOpera214DailySchema.Columns.Add(new DatabaseColumn("RoomType", vwOpera214DailySchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwOpera214DailySchema.Columns.Add(new DatabaseColumn("Name", vwOpera214DailySchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwOpera214DailySchema.Columns.Add(new DatabaseColumn("ReservationPreferences", vwOpera214DailySchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwOpera214DailySchema.Columns.Add(new DatabaseColumn("SpecialRequests", vwOpera214DailySchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwOpera214DailySchema.Columns.Add(new DatabaseColumn("Adults", vwOpera214DailySchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwOpera214DailySchema.Columns.Add(new DatabaseColumn("Children", vwOpera214DailySchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwOpera214DailySchema.Columns.Add(new DatabaseColumn("NumOfStays", vwOpera214DailySchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwOpera214DailySchema.Columns.Add(new DatabaseColumn("VIP", vwOpera214DailySchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwOpera214DailySchema.Columns.Add(new DatabaseColumn("MembershipLevel", vwOpera214DailySchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwOpera214DailySchema.Columns.Add(new DatabaseColumn("RateCode", vwOpera214DailySchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwOpera214DailySchema.Columns.Add(new DatabaseColumn("EffectiveRate", vwOpera214DailySchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwOpera214DailySchema.Columns.Add(new DatabaseColumn("Arrival", vwOpera214DailySchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwOpera214DailySchema.Columns.Add(new DatabaseColumn("ArrivalTime", vwOpera214DailySchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwOpera214DailySchema.Columns.Add(new DatabaseColumn("Departure", vwOpera214DailySchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwOpera214DailySchema.Columns.Add(new DatabaseColumn("PayMtd", vwOpera214DailySchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwOpera214DailySchema.Columns.Add(new DatabaseColumn("Market", vwOpera214DailySchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwOpera214DailySchema.Columns.Add(new DatabaseColumn("Nights", vwOpera214DailySchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwOpera214DailySchema.Columns.Add(new DatabaseColumn("Company", vwOpera214DailySchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwOpera214DailySchema.Columns.Add(new DatabaseColumn("TravelAgent", vwOpera214DailySchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwOpera214DailySchema.Columns.Add(new DatabaseColumn("Source", vwOpera214DailySchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwOpera214DailySchema.Columns.Add(new DatabaseColumn("Comments", vwOpera214DailySchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwOpera214DailySchema.Columns.Add(new DatabaseColumn("NoOfRooms", vwOpera214DailySchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwOpera214DailySchema.Columns.Add(new DatabaseColumn("FixedCharges", vwOpera214DailySchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
				// Add vwOpera214Daily to schema
            	DataProvider.Schema.Tables.Add(vwOpera214DailySchema);
				
				// Table: vwPackageLoss
				// Primary Key: 
				ITable vwPackageLossSchema = new DatabaseTable("vwPackageLoss", DataProvider) { ClassName = "vwPackageLoss", SchemaName = "rpt" };
            	vwPackageLossSchema.Columns.Add(new DatabaseColumn("BUSINESS_DATE", vwPackageLossSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.AnsiString,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwPackageLossSchema.Columns.Add(new DatabaseColumn("BUSINESS_MONTH", vwPackageLossSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwPackageLossSchema.Columns.Add(new DatabaseColumn("ROOM", vwPackageLossSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwPackageLossSchema.Columns.Add(new DatabaseColumn("ROOM_CLASS", vwPackageLossSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwPackageLossSchema.Columns.Add(new DatabaseColumn("CASHIER_DEBIT", vwPackageLossSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwPackageLossSchema.Columns.Add(new DatabaseColumn("ChargedPrice", vwPackageLossSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwPackageLossSchema.Columns.Add(new DatabaseColumn("ListPrice", vwPackageLossSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwPackageLossSchema.Columns.Add(new DatabaseColumn("Type", vwPackageLossSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.AnsiString,
													IsNullable = false,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwPackageLossSchema.Columns.Add(new DatabaseColumn("GuestCount", vwPackageLossSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
				// Add vwPackageLoss to schema
            	DataProvider.Schema.Tables.Add(vwPackageLossSchema);
				
				// Table: vwPackageLossByParsedData
				// Primary Key: 
				ITable vwPackageLossByParsedDataSchema = new DatabaseTable("vwPackageLossByParsedData", DataProvider) { ClassName = "vwPackageLossByParsedDatum", SchemaName = "rpt" };
            	vwPackageLossByParsedDataSchema.Columns.Add(new DatabaseColumn("BUSINESS_MONTH", vwPackageLossByParsedDataSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwPackageLossByParsedDataSchema.Columns.Add(new DatabaseColumn("Loss", vwPackageLossByParsedDataSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwPackageLossByParsedDataSchema.Columns.Add(new DatabaseColumn("Profit", vwPackageLossByParsedDataSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwPackageLossByParsedDataSchema.Columns.Add(new DatabaseColumn("Potental", vwPackageLossByParsedDataSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwPackageLossByParsedDataSchema.Columns.Add(new DatabaseColumn("TotalCount", vwPackageLossByParsedDataSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwPackageLossByParsedDataSchema.Columns.Add(new DatabaseColumn("Difference", vwPackageLossByParsedDataSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwPackageLossByParsedDataSchema.Columns.Add(new DatabaseColumn("SixEuroDiscount", vwPackageLossByParsedDataSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwPackageLossByParsedDataSchema.Columns.Add(new DatabaseColumn("OtherDiscount", vwPackageLossByParsedDataSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwPackageLossByParsedDataSchema.Columns.Add(new DatabaseColumn("TotalComp", vwPackageLossByParsedDataSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwPackageLossByParsedDataSchema.Columns.Add(new DatabaseColumn("RowCounts", vwPackageLossByParsedDataSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwPackageLossByParsedDataSchema.Columns.Add(new DatabaseColumn("GuestCount", vwPackageLossByParsedDataSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
				// Add vwPackageLossByParsedData to schema
            	DataProvider.Schema.Tables.Add(vwPackageLossByParsedDataSchema);
				
				// Table: vwPercentItemsReturned
				// Primary Key: 
				ITable vwPercentItemsReturnedSchema = new DatabaseTable("vwPercentItemsReturned", DataProvider) { ClassName = "vwPercentItemsReturned", SchemaName = "rpt" };
            	vwPercentItemsReturnedSchema.Columns.Add(new DatabaseColumn("Establishment", vwPercentItemsReturnedSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwPercentItemsReturnedSchema.Columns.Add(new DatabaseColumn("Product", vwPercentItemsReturnedSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwPercentItemsReturnedSchema.Columns.Add(new DatabaseColumn("TotalReturnedItems", vwPercentItemsReturnedSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwPercentItemsReturnedSchema.Columns.Add(new DatabaseColumn("TotalOrderedItems", vwPercentItemsReturnedSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwPercentItemsReturnedSchema.Columns.Add(new DatabaseColumn("TotalItems", vwPercentItemsReturnedSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwPercentItemsReturnedSchema.Columns.Add(new DatabaseColumn("PercentReturned", vwPercentItemsReturnedSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
				// Add vwPercentItemsReturned to schema
            	DataProvider.Schema.Tables.Add(vwPercentItemsReturnedSchema);
				
				// Table: vwPercentItemsReturnedMonthly
				// Primary Key: 
				ITable vwPercentItemsReturnedMonthlySchema = new DatabaseTable("vwPercentItemsReturnedMonthly", DataProvider) { ClassName = "vwPercentItemsReturnedMonthly", SchemaName = "rpt" };
            	vwPercentItemsReturnedMonthlySchema.Columns.Add(new DatabaseColumn("BusinessMonth", vwPercentItemsReturnedMonthlySchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwPercentItemsReturnedMonthlySchema.Columns.Add(new DatabaseColumn("Establishment", vwPercentItemsReturnedMonthlySchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwPercentItemsReturnedMonthlySchema.Columns.Add(new DatabaseColumn("Product", vwPercentItemsReturnedMonthlySchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwPercentItemsReturnedMonthlySchema.Columns.Add(new DatabaseColumn("TotalReturnedItems", vwPercentItemsReturnedMonthlySchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwPercentItemsReturnedMonthlySchema.Columns.Add(new DatabaseColumn("TotalOrderedItems", vwPercentItemsReturnedMonthlySchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwPercentItemsReturnedMonthlySchema.Columns.Add(new DatabaseColumn("TotalItems", vwPercentItemsReturnedMonthlySchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwPercentItemsReturnedMonthlySchema.Columns.Add(new DatabaseColumn("PercentReturned", vwPercentItemsReturnedMonthlySchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
				// Add vwPercentItemsReturnedMonthly to schema
            	DataProvider.Schema.Tables.Add(vwPercentItemsReturnedMonthlySchema);
				
				// Table: vwPOSItemSummary
				// Primary Key: 
				ITable vwPOSItemSummarySchema = new DatabaseTable("vwPOSItemSummary", DataProvider) { ClassName = "vwPOSItemSummary", SchemaName = "rpt" };
            	vwPOSItemSummarySchema.Columns.Add(new DatabaseColumn("BUSINESS_DATE", vwPOSItemSummarySchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwPOSItemSummarySchema.Columns.Add(new DatabaseColumn("ESTABLISHMENT", vwPOSItemSummarySchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwPOSItemSummarySchema.Columns.Add(new DatabaseColumn("PRODUCT", vwPOSItemSummarySchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwPOSItemSummarySchema.Columns.Add(new DatabaseColumn("TotalPrice", vwPOSItemSummarySchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwPOSItemSummarySchema.Columns.Add(new DatabaseColumn("TotalItems", vwPOSItemSummarySchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwPOSItemSummarySchema.Columns.Add(new DatabaseColumn("OrderedItems", vwPOSItemSummarySchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwPOSItemSummarySchema.Columns.Add(new DatabaseColumn("ReturnedItems", vwPOSItemSummarySchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwPOSItemSummarySchema.Columns.Add(new DatabaseColumn("Name", vwPOSItemSummarySchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwPOSItemSummarySchema.Columns.Add(new DatabaseColumn("maj_grp_seq", vwPOSItemSummarySchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
				// Add vwPOSItemSummary to schema
            	DataProvider.Schema.Tables.Add(vwPOSItemSummarySchema);
				
				// Table: vwPOSTopTen
				// Primary Key: 
				ITable vwPOSTopTenSchema = new DatabaseTable("vwPOSTopTen", DataProvider) { ClassName = "vwPOSTopTen", SchemaName = "rpt" };
            	vwPOSTopTenSchema.Columns.Add(new DatabaseColumn("BUSINESS_DATE", vwPOSTopTenSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.DateTime,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwPOSTopTenSchema.Columns.Add(new DatabaseColumn("ESTABLISHMENT", vwPOSTopTenSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwPOSTopTenSchema.Columns.Add(new DatabaseColumn("PRODUCT", vwPOSTopTenSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwPOSTopTenSchema.Columns.Add(new DatabaseColumn("TotalPrice", vwPOSTopTenSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwPOSTopTenSchema.Columns.Add(new DatabaseColumn("TotalItems", vwPOSTopTenSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwPOSTopTenSchema.Columns.Add(new DatabaseColumn("OrderedItems", vwPOSTopTenSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwPOSTopTenSchema.Columns.Add(new DatabaseColumn("ReturnedItems", vwPOSTopTenSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwPOSTopTenSchema.Columns.Add(new DatabaseColumn("RowNum", vwPOSTopTenSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int64,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
				// Add vwPOSTopTen to schema
            	DataProvider.Schema.Tables.Add(vwPOSTopTenSchema);
				
				// Table: vwPOSTotals
				// Primary Key: 
				ITable vwPOSTotalsSchema = new DatabaseTable("vwPOSTotals", DataProvider) { ClassName = "vwPOSTotal", SchemaName = "rpt" };
            	vwPOSTotalsSchema.Columns.Add(new DatabaseColumn("Establishment", vwPOSTotalsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.String,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwPOSTotalsSchema.Columns.Add(new DatabaseColumn("TicketCount", vwPOSTotalsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwPOSTotalsSchema.Columns.Add(new DatabaseColumn("TotalGuests", vwPOSTotalsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwPOSTotalsSchema.Columns.Add(new DatabaseColumn("MinTicket", vwPOSTotalsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwPOSTotalsSchema.Columns.Add(new DatabaseColumn("MaxTicket", vwPOSTotalsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwPOSTotalsSchema.Columns.Add(new DatabaseColumn("SumDiscounts", vwPOSTotalsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwPOSTotalsSchema.Columns.Add(new DatabaseColumn("MinibarLost", vwPOSTotalsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwPOSTotalsSchema.Columns.Add(new DatabaseColumn("Aminities", vwPOSTotalsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwPOSTotalsSchema.Columns.Add(new DatabaseColumn("TicketTotal", vwPOSTotalsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwPOSTotalsSchema.Columns.Add(new DatabaseColumn("Tips", vwPOSTotalsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwPOSTotalsSchema.Columns.Add(new DatabaseColumn("TotalCredits", vwPOSTotalsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwPOSTotalsSchema.Columns.Add(new DatabaseColumn("PaymentTotal", vwPOSTotalsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Decimal,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwPOSTotalsSchema.Columns.Add(new DatabaseColumn("EstablishmentNumber", vwPOSTotalsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.Int32,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
            	vwPOSTotalsSchema.Columns.Add(new DatabaseColumn("BusinessDate", vwPOSTotalsSchema)
												{
													IsPrimaryKey = false,
													DataType = DbType.AnsiString,
													IsNullable = true,
													AutoIncrement = false,
													IsForeignKey = false
												});
				// Add vwPOSTotals to schema
            	DataProvider.Schema.Tables.Add(vwPOSTotalsSchema);
            }
            #endregion
        }
    }
}