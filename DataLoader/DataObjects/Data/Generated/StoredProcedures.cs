


using System;
using SubSonic;
using SubSonic.Schema;
using SubSonic.DataProviders;
using System.Data;

namespace Jaxis.POS.Data
{
	public partial class RenAixDB
	{
		
		public StoredProcedure procCleanupData(string DataTable,string DataSource)
		{
			StoredProcedure sp=new StoredProcedure("procCleanupData",this.Provider);
								sp.Command.AddParameter("DataTable",DataTable,DbType.String);
								sp.Command.AddParameter("DataSource",DataSource,DbType.String);
							return sp;
		}
		
		public StoredProcedure procClearData()
		{
			StoredProcedure sp=new StoredProcedure("procClearData",this.Provider);
							return sp;
		}
		
		public StoredProcedure procUpdateEstablishments()
		{
			StoredProcedure sp=new StoredProcedure("procUpdateEstablishments",this.Provider);
							return sp;
		}
		
	}

}

