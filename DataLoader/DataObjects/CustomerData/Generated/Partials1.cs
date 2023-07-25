


using System;
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

namespace Jaxis.POS.CustomerData
{
    /// <summary>
    /// A class which represents the CustomerProperties table in the CustomerData Database.
    /// </summary>
    public interface ICustomerProperty
    {
    }
    
    /// <summary>
    /// A class which represents the database_firewall_rules table in the CustomerData Database.
    /// </summary>
    public interface Idatabase_firewall_rule
    {
    }
    


    /// <summary>
    /// A class which represents the CustomerProperties table in the CustomerData Database.
    /// </summary>
    public partial class CustomerProperty: ICustomerProperty, IDataObject<ICustomerProperty>
    {
        public System.Linq.IQueryable<ICustomerProperty> GetAll( )
        {
            return CustomerProperty.All();
        }
    }
    


    /// <summary>
    /// A class which represents the database_firewall_rules table in the CustomerData Database.
    /// </summary>
    public partial class database_firewall_rule: Idatabase_firewall_rule, IDataObject<Idatabase_firewall_rule>
    {
        public System.Linq.IQueryable<Idatabase_firewall_rule> GetAll( )
        {
            return database_firewall_rule.All();
        }
    }
    
}
