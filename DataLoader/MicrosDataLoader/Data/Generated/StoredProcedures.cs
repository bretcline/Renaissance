


using System;
using SubSonic;
using SubSonic.Schema;
using SubSonic.DataProviders;
using System.Data;

namespace Jaxis.Inventory.Data
{
	public partial class BeverageMonitorDB
	{
		
		public StoredProcedure procCleanupData()
		{
			StoredProcedure sp=new StoredProcedure("procCleanupData",this.Provider);
							return sp;
		}
		
		public StoredProcedure procClearData()
		{
			StoredProcedure sp=new StoredProcedure("procClearData",this.Provider);
							return sp;
		}
		
	}

}

