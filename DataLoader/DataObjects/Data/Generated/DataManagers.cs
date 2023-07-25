


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

namespace Jaxis.POS.Data
{


    public partial class DataManagerFactory : IDataManagerFactory
    {
        protected DataManagerFactory( )
        {
            m_Managers.Add(typeof(IBanquetMenu), new BanquetMenuDataManager());
            m_Managers.Add(typeof(IBanquet), new BanquetDataManager());
            m_Managers.Add(typeof(IDailyWeatherForecast), new DailyWeatherForecastDataManager());
            m_Managers.Add(typeof(IDAT3), new DAT3DataManager());
            m_Managers.Add(typeof(IMenuItem), new MenuItemDataManager());
            m_Managers.Add(typeof(IMicrosTimePeriod), new MicrosTimePeriodDataManager());
            m_Managers.Add(typeof(IPOSEstablisment), new POSEstablismentDataManager());
            m_Managers.Add(typeof(IPOSPaymentDatum), new POSPaymentDatumDataManager());
            m_Managers.Add(typeof(IPOSTicketItemModifier), new POSTicketItemModifierDataManager());
            m_Managers.Add(typeof(IPOSTicketItem), new POSTicketItemDataManager());
            m_Managers.Add(typeof(IPOSTicket), new POSTicketDataManager());
            m_Managers.Add(typeof(IPOSTVADatum), new POSTVADatumDataManager());
            m_Managers.Add(typeof(ISageBalance), new SageBalanceDataManager());
            m_Managers.Add(typeof(IvwCategoryDetail), new vwCategoryDetailDataManager());
            m_Managers.Add(typeof(IvwCategorySummaryByMonth), new vwCategorySummaryByMonthDataManager());
            m_Managers.Add(typeof(IvwCheckFoodGroup), new vwCheckFoodGroupDataManager());
            m_Managers.Add(typeof(IvwDailyGuestCount), new vwDailyGuestCountDataManager());
            m_Managers.Add(typeof(IvwDailyPOSTicketItem), new vwDailyPOSTicketItemDataManager());
            m_Managers.Add(typeof(IvwDailyPOSTicket), new vwDailyPOSTicketDataManager());
            m_Managers.Add(typeof(IvwDailyTicketDataSummary), new vwDailyTicketDataSummaryDataManager());
            m_Managers.Add(typeof(IvwDiscountByTicket), new vwDiscountByTicketDataManager());
            m_Managers.Add(typeof(IvwMissingTicket), new vwMissingTicketDataManager());
            m_Managers.Add(typeof(IvwMRSBRFinancial), new vwMRSBRFinancialDataManager());
            m_Managers.Add(typeof(IvwOperaMicrosCheck), new vwOperaMicrosCheckDataManager());
            m_Managers.Add(typeof(IvwPOSTicketItem), new vwPOSTicketItemDataManager());
            m_Managers.Add(typeof(IvwPOSTicket), new vwPOSTicketDataManager());
            m_Managers.Add(typeof(IvwSageCategoryTotal), new vwSageCategoryTotalDataManager());
            m_Managers.Add(typeof(IvwSumaryItemDatum), new vwSumaryItemDatumDataManager());
            m_Managers.Add(typeof(Ibanquet_datum), new banquet_datumDataManager());
            m_Managers.Add(typeof(Ibusiness_block), new business_blockDataManager());
            m_Managers.Add(typeof(Idly_corr_ttl), new dly_corr_ttlDataManager());
            m_Managers.Add(typeof(Idly_discount_ttl), new dly_discount_ttlDataManager());
            m_Managers.Add(typeof(Imaj_grp_def), new maj_grp_defDataManager());
            m_Managers.Add(typeof(Imfd_check_dtl), new mfd_check_dtlDataManager());
            m_Managers.Add(typeof(Imfd_check_ttl), new mfd_check_ttlDataManager());
            m_Managers.Add(typeof(Imi_def), new mi_defDataManager());
            m_Managers.Add(typeof(Imi_price_def), new mi_price_defDataManager());
            m_Managers.Add(typeof(Imi_slu_def), new mi_slu_defDataManager());
            m_Managers.Add(typeof(IMicros_Ticket_Detail), new Micros_Ticket_DetailDataManager());
            m_Managers.Add(typeof(IMicros_Ticket), new Micros_TicketDataManager());
            m_Managers.Add(typeof(Imrsbr_financial), new mrsbr_financialDataManager());
            m_Managers.Add(typeof(IOperaA214), new OperaA214DataManager());
            m_Managers.Add(typeof(IOperaAD120BusinessBlock), new OperaAD120BusinessBlockDataManager());
            m_Managers.Add(typeof(IOperaAD160), new OperaAD160DataManager());
            m_Managers.Add(typeof(IOperaD114), new OperaD114DataManager());
            m_Managers.Add(typeof(IOperaD140), new OperaD140DataManager());
            m_Managers.Add(typeof(IOperaF116), new OperaF116DataManager());
            m_Managers.Add(typeof(IOperaH260), new OperaH260DataManager());
            m_Managers.Add(typeof(IOperaP112Departure), new OperaP112DepartureDataManager());
            m_Managers.Add(typeof(IPOSTicketDatum), new POSTicketDatumDataManager());
            m_Managers.Add(typeof(IPS550GuestList), new PS550GuestListDataManager());
            m_Managers.Add(typeof(Irep_bh_short), new rep_bh_shortDataManager());
            m_Managers.Add(typeof(Iv_R_kds_chk_dtl), new v_R_kds_chk_dtlDataManager());
            m_Managers.Add(typeof(IDailyPOSTopTen), new DailyPOSTopTenDataManager());
            m_Managers.Add(typeof(IvwDailyOperaTotal), new vwDailyOperaTotalDataManager());
            m_Managers.Add(typeof(IvwDailyPOSItemSummary), new vwDailyPOSItemSummaryDataManager());
            m_Managers.Add(typeof(IvwDailyPOSTopTen), new vwDailyPOSTopTenDataManager());
            m_Managers.Add(typeof(IvwDailyPOSTotal), new vwDailyPOSTotalDataManager());
            m_Managers.Add(typeof(IvwDiscountedItemsByReasonPO), new vwDiscountedItemsByReasonPODataManager());
            m_Managers.Add(typeof(IvwOpera214Daily), new vwOpera214DailyDataManager());
            m_Managers.Add(typeof(IvwPackageLoss), new vwPackageLossDataManager());
            m_Managers.Add(typeof(IvwPackageLossByParsedDatum), new vwPackageLossByParsedDatumDataManager());
            m_Managers.Add(typeof(IvwPercentItemsReturned), new vwPercentItemsReturnedDataManager());
            m_Managers.Add(typeof(IvwPercentItemsReturnedMonthly), new vwPercentItemsReturnedMonthlyDataManager());
            m_Managers.Add(typeof(IvwPOSItemSummary), new vwPOSItemSummaryDataManager());
            m_Managers.Add(typeof(IvwPOSTopTen), new vwPOSTopTenDataManager());
            m_Managers.Add(typeof(IvwPOSTotal), new vwPOSTotalDataManager());
        }
    }



// Interface
    public interface IBanquetMenuDataManager : IDataManager<IBanquetMenu> { }

// Data Manager
	public class BanquetMenuDataManager : DataManager<IBanquetMenu, BanquetMenu>, IBanquetMenuDataManager, IDataManager
    {
        #region IDataManager<> Members

        public IQueryable<IBanquetMenu> GetAll( )
        {
            return BanquetMenu.All( );
        }

        public IBanquetMenu Get( Guid _id )
        {
            return BanquetMenu.GetByID( _id );
        }

        #endregion
    }

// Interface
    public interface IBanquetDataManager : IDataManager<IBanquet> { }

// Data Manager
	public class BanquetDataManager : DataManager<IBanquet, Banquet>, IBanquetDataManager, IDataManager
    {
        #region IDataManager<> Members

        public IQueryable<IBanquet> GetAll( )
        {
            return Banquet.All( );
        }

        public IBanquet Get( Guid _id )
        {
            return Banquet.GetByID( _id );
        }

        #endregion
    }

// Interface
    public interface IDailyWeatherForecastDataManager : IDataManager<IDailyWeatherForecast> { }

// Data Manager
	public class DailyWeatherForecastDataManager : DataManager<IDailyWeatherForecast, DailyWeatherForecast>, IDailyWeatherForecastDataManager, IDataManager
    {
        #region IDataManager<> Members

        public IQueryable<IDailyWeatherForecast> GetAll( )
        {
            return DailyWeatherForecast.All( );
        }

        public IDailyWeatherForecast Get( Guid _id )
        {
            return DailyWeatherForecast.GetByID( _id );
        }

        #endregion
    }

// Interface
    public interface IDAT3DataManager : IDataManager<IDAT3> { }

// Data Manager
	public class DAT3DataManager : DataManager<IDAT3, DAT3>, IDAT3DataManager, IDataManager
    {
        #region IDataManager<> Members

        public IQueryable<IDAT3> GetAll( )
        {
            return DAT3.All( );
        }

        public IDAT3 Get( Guid _id )
        {
            return null;
        }

        #endregion
    }

// Interface
    public interface IMenuItemDataManager : IDataManager<IMenuItem> { }

// Data Manager
	public class MenuItemDataManager : DataManager<IMenuItem, MenuItem>, IMenuItemDataManager, IDataManager
    {
        #region IDataManager<> Members

        public IQueryable<IMenuItem> GetAll( )
        {
            return MenuItem.All( );
        }

        public IMenuItem Get( Guid _id )
        {
            return MenuItem.GetByID( _id );
        }

        #endregion
    }

// Interface
    public interface IMicrosTimePeriodDataManager : IDataManager<IMicrosTimePeriod> { }

// Data Manager
	public class MicrosTimePeriodDataManager : DataManager<IMicrosTimePeriod, MicrosTimePeriod>, IMicrosTimePeriodDataManager, IDataManager
    {
        #region IDataManager<> Members

        public IQueryable<IMicrosTimePeriod> GetAll( )
        {
            return MicrosTimePeriod.All( );
        }

        public IMicrosTimePeriod Get( Guid _id )
        {
            return null;
        }

        #endregion
    }

// Interface
    public interface IPOSEstablismentDataManager : IDataManager<IPOSEstablisment> { }

// Data Manager
	public class POSEstablismentDataManager : DataManager<IPOSEstablisment, POSEstablisment>, IPOSEstablismentDataManager, IDataManager
    {
        #region IDataManager<> Members

        public IQueryable<IPOSEstablisment> GetAll( )
        {
            return POSEstablisment.All( );
        }

        public IPOSEstablisment Get( Guid _id )
        {
            return POSEstablisment.GetByID( _id );
        }

        #endregion
    }

// Interface
    public interface IPOSPaymentDatumDataManager : IDataManager<IPOSPaymentDatum> { }

// Data Manager
	public class POSPaymentDatumDataManager : DataManager<IPOSPaymentDatum, POSPaymentDatum>, IPOSPaymentDatumDataManager, IDataManager
    {
        #region IDataManager<> Members

        public IQueryable<IPOSPaymentDatum> GetAll( )
        {
            return POSPaymentDatum.All( );
        }

        public IPOSPaymentDatum Get( Guid _id )
        {
            return POSPaymentDatum.GetByID( _id );
        }

        #endregion
    }

// Interface
    public interface IPOSTicketItemModifierDataManager : IDataManager<IPOSTicketItemModifier> { }

// Data Manager
	public class POSTicketItemModifierDataManager : DataManager<IPOSTicketItemModifier, POSTicketItemModifier>, IPOSTicketItemModifierDataManager, IDataManager
    {
        #region IDataManager<> Members

        public IQueryable<IPOSTicketItemModifier> GetAll( )
        {
            return POSTicketItemModifier.All( );
        }

        public IPOSTicketItemModifier Get( Guid _id )
        {
            return POSTicketItemModifier.GetByID( _id );
        }

        #endregion
    }

// Interface
    public interface IPOSTicketItemDataManager : IDataManager<IPOSTicketItem> { }

// Data Manager
	public class POSTicketItemDataManager : DataManager<IPOSTicketItem, POSTicketItem>, IPOSTicketItemDataManager, IDataManager
    {
        #region IDataManager<> Members

        public IQueryable<IPOSTicketItem> GetAll( )
        {
            return POSTicketItem.All( );
        }

        public IPOSTicketItem Get( Guid _id )
        {
            return POSTicketItem.GetByID( _id );
        }

        #endregion
    }

// Interface
    public interface IPOSTicketDataManager : IDataManager<IPOSTicket> { }

// Data Manager
	public class POSTicketDataManager : DataManager<IPOSTicket, POSTicket>, IPOSTicketDataManager, IDataManager
    {
        #region IDataManager<> Members

        public IQueryable<IPOSTicket> GetAll( )
        {
            return POSTicket.All( );
        }

        public IPOSTicket Get( Guid _id )
        {
            return POSTicket.GetByID( _id );
        }

        #endregion
    }

// Interface
    public interface IPOSTVADatumDataManager : IDataManager<IPOSTVADatum> { }

// Data Manager
	public class POSTVADatumDataManager : DataManager<IPOSTVADatum, POSTVADatum>, IPOSTVADatumDataManager, IDataManager
    {
        #region IDataManager<> Members

        public IQueryable<IPOSTVADatum> GetAll( )
        {
            return POSTVADatum.All( );
        }

        public IPOSTVADatum Get( Guid _id )
        {
            return POSTVADatum.GetByID( _id );
        }

        #endregion
    }

// Interface
    public interface ISageBalanceDataManager : IDataManager<ISageBalance> { }

// Data Manager
	public class SageBalanceDataManager : DataManager<ISageBalance, SageBalance>, ISageBalanceDataManager, IDataManager
    {
        #region IDataManager<> Members

        public IQueryable<ISageBalance> GetAll( )
        {
            return SageBalance.All( );
        }

        public ISageBalance Get( Guid _id )
        {
            return null;
        }

        #endregion
    }

// Interface
    public interface IvwCategoryDetailDataManager : IDataManager<IvwCategoryDetail> { }

// Data Manager
	public class vwCategoryDetailDataManager : DataManager<IvwCategoryDetail, vwCategoryDetail>, IvwCategoryDetailDataManager, IDataManager
    {
        #region IDataManager<> Members

        public IQueryable<IvwCategoryDetail> GetAll( )
        {
            return vwCategoryDetail.All( );
        }

        public IvwCategoryDetail Get( Guid _id )
        {
            return null;
        }

        #endregion
    }

// Interface
    public interface IvwCategorySummaryByMonthDataManager : IDataManager<IvwCategorySummaryByMonth> { }

// Data Manager
	public class vwCategorySummaryByMonthDataManager : DataManager<IvwCategorySummaryByMonth, vwCategorySummaryByMonth>, IvwCategorySummaryByMonthDataManager, IDataManager
    {
        #region IDataManager<> Members

        public IQueryable<IvwCategorySummaryByMonth> GetAll( )
        {
            return vwCategorySummaryByMonth.All( );
        }

        public IvwCategorySummaryByMonth Get( Guid _id )
        {
            return null;
        }

        #endregion
    }

// Interface
    public interface IvwCheckFoodGroupDataManager : IDataManager<IvwCheckFoodGroup> { }

// Data Manager
	public class vwCheckFoodGroupDataManager : DataManager<IvwCheckFoodGroup, vwCheckFoodGroup>, IvwCheckFoodGroupDataManager, IDataManager
    {
        #region IDataManager<> Members

        public IQueryable<IvwCheckFoodGroup> GetAll( )
        {
            return vwCheckFoodGroup.All( );
        }

        public IvwCheckFoodGroup Get( Guid _id )
        {
            return null;
        }

        #endregion
    }

// Interface
    public interface IvwDailyGuestCountDataManager : IDataManager<IvwDailyGuestCount> { }

// Data Manager
	public class vwDailyGuestCountDataManager : DataManager<IvwDailyGuestCount, vwDailyGuestCount>, IvwDailyGuestCountDataManager, IDataManager
    {
        #region IDataManager<> Members

        public IQueryable<IvwDailyGuestCount> GetAll( )
        {
            return vwDailyGuestCount.All( );
        }

        public IvwDailyGuestCount Get( Guid _id )
        {
            return null;
        }

        #endregion
    }

// Interface
    public interface IvwDailyPOSTicketItemDataManager : IDataManager<IvwDailyPOSTicketItem> { }

// Data Manager
	public class vwDailyPOSTicketItemDataManager : DataManager<IvwDailyPOSTicketItem, vwDailyPOSTicketItem>, IvwDailyPOSTicketItemDataManager, IDataManager
    {
        #region IDataManager<> Members

        public IQueryable<IvwDailyPOSTicketItem> GetAll( )
        {
            return vwDailyPOSTicketItem.All( );
        }

        public IvwDailyPOSTicketItem Get( Guid _id )
        {
            return vwDailyPOSTicketItem.GetByID( _id );
        }

        #endregion
    }

// Interface
    public interface IvwDailyPOSTicketDataManager : IDataManager<IvwDailyPOSTicket> { }

// Data Manager
	public class vwDailyPOSTicketDataManager : DataManager<IvwDailyPOSTicket, vwDailyPOSTicket>, IvwDailyPOSTicketDataManager, IDataManager
    {
        #region IDataManager<> Members

        public IQueryable<IvwDailyPOSTicket> GetAll( )
        {
            return vwDailyPOSTicket.All( );
        }

        public IvwDailyPOSTicket Get( Guid _id )
        {
            return null;
        }

        #endregion
    }

// Interface
    public interface IvwDailyTicketDataSummaryDataManager : IDataManager<IvwDailyTicketDataSummary> { }

// Data Manager
	public class vwDailyTicketDataSummaryDataManager : DataManager<IvwDailyTicketDataSummary, vwDailyTicketDataSummary>, IvwDailyTicketDataSummaryDataManager, IDataManager
    {
        #region IDataManager<> Members

        public IQueryable<IvwDailyTicketDataSummary> GetAll( )
        {
            return vwDailyTicketDataSummary.All( );
        }

        public IvwDailyTicketDataSummary Get( Guid _id )
        {
            return null;
        }

        #endregion
    }

// Interface
    public interface IvwDiscountByTicketDataManager : IDataManager<IvwDiscountByTicket> { }

// Data Manager
	public class vwDiscountByTicketDataManager : DataManager<IvwDiscountByTicket, vwDiscountByTicket>, IvwDiscountByTicketDataManager, IDataManager
    {
        #region IDataManager<> Members

        public IQueryable<IvwDiscountByTicket> GetAll( )
        {
            return vwDiscountByTicket.All( );
        }

        public IvwDiscountByTicket Get( Guid _id )
        {
            return null;
        }

        #endregion
    }

// Interface
    public interface IvwMissingTicketDataManager : IDataManager<IvwMissingTicket> { }

// Data Manager
	public class vwMissingTicketDataManager : DataManager<IvwMissingTicket, vwMissingTicket>, IvwMissingTicketDataManager, IDataManager
    {
        #region IDataManager<> Members

        public IQueryable<IvwMissingTicket> GetAll( )
        {
            return vwMissingTicket.All( );
        }

        public IvwMissingTicket Get( Guid _id )
        {
            return null;
        }

        #endregion
    }

// Interface
    public interface IvwMRSBRFinancialDataManager : IDataManager<IvwMRSBRFinancial> { }

// Data Manager
	public class vwMRSBRFinancialDataManager : DataManager<IvwMRSBRFinancial, vwMRSBRFinancial>, IvwMRSBRFinancialDataManager, IDataManager
    {
        #region IDataManager<> Members

        public IQueryable<IvwMRSBRFinancial> GetAll( )
        {
            return vwMRSBRFinancial.All( );
        }

        public IvwMRSBRFinancial Get( Guid _id )
        {
            return null;
        }

        #endregion
    }

// Interface
    public interface IvwOperaMicrosCheckDataManager : IDataManager<IvwOperaMicrosCheck> { }

// Data Manager
	public class vwOperaMicrosCheckDataManager : DataManager<IvwOperaMicrosCheck, vwOperaMicrosCheck>, IvwOperaMicrosCheckDataManager, IDataManager
    {
        #region IDataManager<> Members

        public IQueryable<IvwOperaMicrosCheck> GetAll( )
        {
            return vwOperaMicrosCheck.All( );
        }

        public IvwOperaMicrosCheck Get( Guid _id )
        {
            return null;
        }

        #endregion
    }

// Interface
    public interface IvwPOSTicketItemDataManager : IDataManager<IvwPOSTicketItem> { }

// Data Manager
	public class vwPOSTicketItemDataManager : DataManager<IvwPOSTicketItem, vwPOSTicketItem>, IvwPOSTicketItemDataManager, IDataManager
    {
        #region IDataManager<> Members

        public IQueryable<IvwPOSTicketItem> GetAll( )
        {
            return vwPOSTicketItem.All( );
        }

        public IvwPOSTicketItem Get( Guid _id )
        {
            return vwPOSTicketItem.GetByID( _id );
        }

        #endregion
    }

// Interface
    public interface IvwPOSTicketDataManager : IDataManager<IvwPOSTicket> { }

// Data Manager
	public class vwPOSTicketDataManager : DataManager<IvwPOSTicket, vwPOSTicket>, IvwPOSTicketDataManager, IDataManager
    {
        #region IDataManager<> Members

        public IQueryable<IvwPOSTicket> GetAll( )
        {
            return vwPOSTicket.All( );
        }

        public IvwPOSTicket Get( Guid _id )
        {
            return null;
        }

        #endregion
    }

// Interface
    public interface IvwSageCategoryTotalDataManager : IDataManager<IvwSageCategoryTotal> { }

// Data Manager
	public class vwSageCategoryTotalDataManager : DataManager<IvwSageCategoryTotal, vwSageCategoryTotal>, IvwSageCategoryTotalDataManager, IDataManager
    {
        #region IDataManager<> Members

        public IQueryable<IvwSageCategoryTotal> GetAll( )
        {
            return vwSageCategoryTotal.All( );
        }

        public IvwSageCategoryTotal Get( Guid _id )
        {
            return null;
        }

        #endregion
    }

// Interface
    public interface IvwSumaryItemDatumDataManager : IDataManager<IvwSumaryItemDatum> { }

// Data Manager
	public class vwSumaryItemDatumDataManager : DataManager<IvwSumaryItemDatum, vwSumaryItemDatum>, IvwSumaryItemDatumDataManager, IDataManager
    {
        #region IDataManager<> Members

        public IQueryable<IvwSumaryItemDatum> GetAll( )
        {
            return vwSumaryItemDatum.All( );
        }

        public IvwSumaryItemDatum Get( Guid _id )
        {
            return null;
        }

        #endregion
    }

// Interface
    public interface Ibanquet_datumDataManager : IDataManager<Ibanquet_datum> { }

// Data Manager
	public class banquet_datumDataManager : DataManager<Ibanquet_datum, banquet_datum>, Ibanquet_datumDataManager, IDataManager
    {
        #region IDataManager<> Members

        public IQueryable<Ibanquet_datum> GetAll( )
        {
            return banquet_datum.All( );
        }

        public Ibanquet_datum Get( Guid _id )
        {
            return banquet_datum.GetByID( _id );
        }

        #endregion
    }

// Interface
    public interface Ibusiness_blockDataManager : IDataManager<Ibusiness_block> { }

// Data Manager
	public class business_blockDataManager : DataManager<Ibusiness_block, business_block>, Ibusiness_blockDataManager, IDataManager
    {
        #region IDataManager<> Members

        public IQueryable<Ibusiness_block> GetAll( )
        {
            return business_block.All( );
        }

        public Ibusiness_block Get( Guid _id )
        {
            return business_block.GetByID( _id );
        }

        #endregion
    }

// Interface
    public interface Idly_corr_ttlDataManager : IDataManager<Idly_corr_ttl> { }

// Data Manager
	public class dly_corr_ttlDataManager : DataManager<Idly_corr_ttl, dly_corr_ttl>, Idly_corr_ttlDataManager, IDataManager
    {
        #region IDataManager<> Members

        public IQueryable<Idly_corr_ttl> GetAll( )
        {
            return dly_corr_ttl.All( );
        }

        public Idly_corr_ttl Get( Guid _id )
        {
            return dly_corr_ttl.GetByID( _id );
        }

        #endregion
    }

// Interface
    public interface Idly_discount_ttlDataManager : IDataManager<Idly_discount_ttl> { }

// Data Manager
	public class dly_discount_ttlDataManager : DataManager<Idly_discount_ttl, dly_discount_ttl>, Idly_discount_ttlDataManager, IDataManager
    {
        #region IDataManager<> Members

        public IQueryable<Idly_discount_ttl> GetAll( )
        {
            return dly_discount_ttl.All( );
        }

        public Idly_discount_ttl Get( Guid _id )
        {
            return dly_discount_ttl.GetByID( _id );
        }

        #endregion
    }

// Interface
    public interface Imaj_grp_defDataManager : IDataManager<Imaj_grp_def> { }

// Data Manager
	public class maj_grp_defDataManager : DataManager<Imaj_grp_def, maj_grp_def>, Imaj_grp_defDataManager, IDataManager
    {
        #region IDataManager<> Members

        public IQueryable<Imaj_grp_def> GetAll( )
        {
            return maj_grp_def.All( );
        }

        public Imaj_grp_def Get( Guid _id )
        {
            return maj_grp_def.GetByID( _id );
        }

        #endregion
    }

// Interface
    public interface Imfd_check_dtlDataManager : IDataManager<Imfd_check_dtl> { }

// Data Manager
	public class mfd_check_dtlDataManager : DataManager<Imfd_check_dtl, mfd_check_dtl>, Imfd_check_dtlDataManager, IDataManager
    {
        #region IDataManager<> Members

        public IQueryable<Imfd_check_dtl> GetAll( )
        {
            return mfd_check_dtl.All( );
        }

        public Imfd_check_dtl Get( Guid _id )
        {
            return mfd_check_dtl.GetByID( _id );
        }

        #endregion
    }

// Interface
    public interface Imfd_check_ttlDataManager : IDataManager<Imfd_check_ttl> { }

// Data Manager
	public class mfd_check_ttlDataManager : DataManager<Imfd_check_ttl, mfd_check_ttl>, Imfd_check_ttlDataManager, IDataManager
    {
        #region IDataManager<> Members

        public IQueryable<Imfd_check_ttl> GetAll( )
        {
            return mfd_check_ttl.All( );
        }

        public Imfd_check_ttl Get( Guid _id )
        {
            return mfd_check_ttl.GetByID( _id );
        }

        #endregion
    }

// Interface
    public interface Imi_defDataManager : IDataManager<Imi_def> { }

// Data Manager
	public class mi_defDataManager : DataManager<Imi_def, mi_def>, Imi_defDataManager, IDataManager
    {
        #region IDataManager<> Members

        public IQueryable<Imi_def> GetAll( )
        {
            return mi_def.All( );
        }

        public Imi_def Get( Guid _id )
        {
            return mi_def.GetByID( _id );
        }

        #endregion
    }

// Interface
    public interface Imi_price_defDataManager : IDataManager<Imi_price_def> { }

// Data Manager
	public class mi_price_defDataManager : DataManager<Imi_price_def, mi_price_def>, Imi_price_defDataManager, IDataManager
    {
        #region IDataManager<> Members

        public IQueryable<Imi_price_def> GetAll( )
        {
            return mi_price_def.All( );
        }

        public Imi_price_def Get( Guid _id )
        {
            return mi_price_def.GetByID( _id );
        }

        #endregion
    }

// Interface
    public interface Imi_slu_defDataManager : IDataManager<Imi_slu_def> { }

// Data Manager
	public class mi_slu_defDataManager : DataManager<Imi_slu_def, mi_slu_def>, Imi_slu_defDataManager, IDataManager
    {
        #region IDataManager<> Members

        public IQueryable<Imi_slu_def> GetAll( )
        {
            return mi_slu_def.All( );
        }

        public Imi_slu_def Get( Guid _id )
        {
            return mi_slu_def.GetByID( _id );
        }

        #endregion
    }

// Interface
    public interface IMicros_Ticket_DetailDataManager : IDataManager<IMicros_Ticket_Detail> { }

// Data Manager
	public class Micros_Ticket_DetailDataManager : DataManager<IMicros_Ticket_Detail, Micros_Ticket_Detail>, IMicros_Ticket_DetailDataManager, IDataManager
    {
        #region IDataManager<> Members

        public IQueryable<IMicros_Ticket_Detail> GetAll( )
        {
            return Micros_Ticket_Detail.All( );
        }

        public IMicros_Ticket_Detail Get( Guid _id )
        {
            return Micros_Ticket_Detail.GetByID( _id );
        }

        #endregion
    }

// Interface
    public interface IMicros_TicketDataManager : IDataManager<IMicros_Ticket> { }

// Data Manager
	public class Micros_TicketDataManager : DataManager<IMicros_Ticket, Micros_Ticket>, IMicros_TicketDataManager, IDataManager
    {
        #region IDataManager<> Members

        public IQueryable<IMicros_Ticket> GetAll( )
        {
            return Micros_Ticket.All( );
        }

        public IMicros_Ticket Get( Guid _id )
        {
            return Micros_Ticket.GetByID( _id );
        }

        #endregion
    }

// Interface
    public interface Imrsbr_financialDataManager : IDataManager<Imrsbr_financial> { }

// Data Manager
	public class mrsbr_financialDataManager : DataManager<Imrsbr_financial, mrsbr_financial>, Imrsbr_financialDataManager, IDataManager
    {
        #region IDataManager<> Members

        public IQueryable<Imrsbr_financial> GetAll( )
        {
            return mrsbr_financial.All( );
        }

        public Imrsbr_financial Get( Guid _id )
        {
            return mrsbr_financial.GetByID( _id );
        }

        #endregion
    }

// Interface
    public interface IOperaA214DataManager : IDataManager<IOperaA214> { }

// Data Manager
	public class OperaA214DataManager : DataManager<IOperaA214, OperaA214>, IOperaA214DataManager, IDataManager
    {
        #region IDataManager<> Members

        public IQueryable<IOperaA214> GetAll( )
        {
            return OperaA214.All( );
        }

        public IOperaA214 Get( Guid _id )
        {
            return OperaA214.GetByID( _id );
        }

        #endregion
    }

// Interface
    public interface IOperaAD120BusinessBlockDataManager : IDataManager<IOperaAD120BusinessBlock> { }

// Data Manager
	public class OperaAD120BusinessBlockDataManager : DataManager<IOperaAD120BusinessBlock, OperaAD120BusinessBlock>, IOperaAD120BusinessBlockDataManager, IDataManager
    {
        #region IDataManager<> Members

        public IQueryable<IOperaAD120BusinessBlock> GetAll( )
        {
            return OperaAD120BusinessBlock.All( );
        }

        public IOperaAD120BusinessBlock Get( Guid _id )
        {
            return OperaAD120BusinessBlock.GetByID( _id );
        }

        #endregion
    }

// Interface
    public interface IOperaAD160DataManager : IDataManager<IOperaAD160> { }

// Data Manager
	public class OperaAD160DataManager : DataManager<IOperaAD160, OperaAD160>, IOperaAD160DataManager, IDataManager
    {
        #region IDataManager<> Members

        public IQueryable<IOperaAD160> GetAll( )
        {
            return OperaAD160.All( );
        }

        public IOperaAD160 Get( Guid _id )
        {
            return OperaAD160.GetByID( _id );
        }

        #endregion
    }

// Interface
    public interface IOperaD114DataManager : IDataManager<IOperaD114> { }

// Data Manager
	public class OperaD114DataManager : DataManager<IOperaD114, OperaD114>, IOperaD114DataManager, IDataManager
    {
        #region IDataManager<> Members

        public IQueryable<IOperaD114> GetAll( )
        {
            return OperaD114.All( );
        }

        public IOperaD114 Get( Guid _id )
        {
            return OperaD114.GetByID( _id );
        }

        #endregion
    }

// Interface
    public interface IOperaD140DataManager : IDataManager<IOperaD140> { }

// Data Manager
	public class OperaD140DataManager : DataManager<IOperaD140, OperaD140>, IOperaD140DataManager, IDataManager
    {
        #region IDataManager<> Members

        public IQueryable<IOperaD140> GetAll( )
        {
            return OperaD140.All( );
        }

        public IOperaD140 Get( Guid _id )
        {
            return OperaD140.GetByID( _id );
        }

        #endregion
    }

// Interface
    public interface IOperaF116DataManager : IDataManager<IOperaF116> { }

// Data Manager
	public class OperaF116DataManager : DataManager<IOperaF116, OperaF116>, IOperaF116DataManager, IDataManager
    {
        #region IDataManager<> Members

        public IQueryable<IOperaF116> GetAll( )
        {
            return OperaF116.All( );
        }

        public IOperaF116 Get( Guid _id )
        {
            return OperaF116.GetByID( _id );
        }

        #endregion
    }

// Interface
    public interface IOperaH260DataManager : IDataManager<IOperaH260> { }

// Data Manager
	public class OperaH260DataManager : DataManager<IOperaH260, OperaH260>, IOperaH260DataManager, IDataManager
    {
        #region IDataManager<> Members

        public IQueryable<IOperaH260> GetAll( )
        {
            return OperaH260.All( );
        }

        public IOperaH260 Get( Guid _id )
        {
            return OperaH260.GetByID( _id );
        }

        #endregion
    }

// Interface
    public interface IOperaP112DepartureDataManager : IDataManager<IOperaP112Departure> { }

// Data Manager
	public class OperaP112DepartureDataManager : DataManager<IOperaP112Departure, OperaP112Departure>, IOperaP112DepartureDataManager, IDataManager
    {
        #region IDataManager<> Members

        public IQueryable<IOperaP112Departure> GetAll( )
        {
            return OperaP112Departure.All( );
        }

        public IOperaP112Departure Get( Guid _id )
        {
            return OperaP112Departure.GetByID( _id );
        }

        #endregion
    }

// Interface
    public interface IPOSTicketDatumDataManager : IDataManager<IPOSTicketDatum> { }

// Data Manager
	public class POSTicketDatumDataManager : DataManager<IPOSTicketDatum, POSTicketDatum>, IPOSTicketDatumDataManager, IDataManager
    {
        #region IDataManager<> Members

        public IQueryable<IPOSTicketDatum> GetAll( )
        {
            return POSTicketDatum.All( );
        }

        public IPOSTicketDatum Get( Guid _id )
        {
            return POSTicketDatum.GetByID( _id );
        }

        #endregion
    }

// Interface
    public interface IPS550GuestListDataManager : IDataManager<IPS550GuestList> { }

// Data Manager
	public class PS550GuestListDataManager : DataManager<IPS550GuestList, PS550GuestList>, IPS550GuestListDataManager, IDataManager
    {
        #region IDataManager<> Members

        public IQueryable<IPS550GuestList> GetAll( )
        {
            return PS550GuestList.All( );
        }

        public IPS550GuestList Get( Guid _id )
        {
            return PS550GuestList.GetByID( _id );
        }

        #endregion
    }

// Interface
    public interface Irep_bh_shortDataManager : IDataManager<Irep_bh_short> { }

// Data Manager
	public class rep_bh_shortDataManager : DataManager<Irep_bh_short, rep_bh_short>, Irep_bh_shortDataManager, IDataManager
    {
        #region IDataManager<> Members

        public IQueryable<Irep_bh_short> GetAll( )
        {
            return rep_bh_short.All( );
        }

        public Irep_bh_short Get( Guid _id )
        {
            return rep_bh_short.GetByID( _id );
        }

        #endregion
    }

// Interface
    public interface Iv_R_kds_chk_dtlDataManager : IDataManager<Iv_R_kds_chk_dtl> { }

// Data Manager
	public class v_R_kds_chk_dtlDataManager : DataManager<Iv_R_kds_chk_dtl, v_R_kds_chk_dtl>, Iv_R_kds_chk_dtlDataManager, IDataManager
    {
        #region IDataManager<> Members

        public IQueryable<Iv_R_kds_chk_dtl> GetAll( )
        {
            return v_R_kds_chk_dtl.All( );
        }

        public Iv_R_kds_chk_dtl Get( Guid _id )
        {
            return v_R_kds_chk_dtl.GetByID( _id );
        }

        #endregion
    }

// Interface
    public interface IDailyPOSTopTenDataManager : IDataManager<IDailyPOSTopTen> { }

// Data Manager
	public class DailyPOSTopTenDataManager : DataManager<IDailyPOSTopTen, DailyPOSTopTen>, IDailyPOSTopTenDataManager, IDataManager
    {
        #region IDataManager<> Members

        public IQueryable<IDailyPOSTopTen> GetAll( )
        {
            return DailyPOSTopTen.All( );
        }

        public IDailyPOSTopTen Get( Guid _id )
        {
            return null;
        }

        #endregion
    }

// Interface
    public interface IvwDailyOperaTotalDataManager : IDataManager<IvwDailyOperaTotal> { }

// Data Manager
	public class vwDailyOperaTotalDataManager : DataManager<IvwDailyOperaTotal, vwDailyOperaTotal>, IvwDailyOperaTotalDataManager, IDataManager
    {
        #region IDataManager<> Members

        public IQueryable<IvwDailyOperaTotal> GetAll( )
        {
            return vwDailyOperaTotal.All( );
        }

        public IvwDailyOperaTotal Get( Guid _id )
        {
            return null;
        }

        #endregion
    }

// Interface
    public interface IvwDailyPOSItemSummaryDataManager : IDataManager<IvwDailyPOSItemSummary> { }

// Data Manager
	public class vwDailyPOSItemSummaryDataManager : DataManager<IvwDailyPOSItemSummary, vwDailyPOSItemSummary>, IvwDailyPOSItemSummaryDataManager, IDataManager
    {
        #region IDataManager<> Members

        public IQueryable<IvwDailyPOSItemSummary> GetAll( )
        {
            return vwDailyPOSItemSummary.All( );
        }

        public IvwDailyPOSItemSummary Get( Guid _id )
        {
            return null;
        }

        #endregion
    }

// Interface
    public interface IvwDailyPOSTopTenDataManager : IDataManager<IvwDailyPOSTopTen> { }

// Data Manager
	public class vwDailyPOSTopTenDataManager : DataManager<IvwDailyPOSTopTen, vwDailyPOSTopTen>, IvwDailyPOSTopTenDataManager, IDataManager
    {
        #region IDataManager<> Members

        public IQueryable<IvwDailyPOSTopTen> GetAll( )
        {
            return vwDailyPOSTopTen.All( );
        }

        public IvwDailyPOSTopTen Get( Guid _id )
        {
            return null;
        }

        #endregion
    }

// Interface
    public interface IvwDailyPOSTotalDataManager : IDataManager<IvwDailyPOSTotal> { }

// Data Manager
	public class vwDailyPOSTotalDataManager : DataManager<IvwDailyPOSTotal, vwDailyPOSTotal>, IvwDailyPOSTotalDataManager, IDataManager
    {
        #region IDataManager<> Members

        public IQueryable<IvwDailyPOSTotal> GetAll( )
        {
            return vwDailyPOSTotal.All( );
        }

        public IvwDailyPOSTotal Get( Guid _id )
        {
            return null;
        }

        #endregion
    }

// Interface
    public interface IvwDiscountedItemsByReasonPODataManager : IDataManager<IvwDiscountedItemsByReasonPO> { }

// Data Manager
	public class vwDiscountedItemsByReasonPODataManager : DataManager<IvwDiscountedItemsByReasonPO, vwDiscountedItemsByReasonPO>, IvwDiscountedItemsByReasonPODataManager, IDataManager
    {
        #region IDataManager<> Members

        public IQueryable<IvwDiscountedItemsByReasonPO> GetAll( )
        {
            return vwDiscountedItemsByReasonPO.All( );
        }

        public IvwDiscountedItemsByReasonPO Get( Guid _id )
        {
            return null;
        }

        #endregion
    }

// Interface
    public interface IvwOpera214DailyDataManager : IDataManager<IvwOpera214Daily> { }

// Data Manager
	public class vwOpera214DailyDataManager : DataManager<IvwOpera214Daily, vwOpera214Daily>, IvwOpera214DailyDataManager, IDataManager
    {
        #region IDataManager<> Members

        public IQueryable<IvwOpera214Daily> GetAll( )
        {
            return vwOpera214Daily.All( );
        }

        public IvwOpera214Daily Get( Guid _id )
        {
            return null;
        }

        #endregion
    }

// Interface
    public interface IvwPackageLossDataManager : IDataManager<IvwPackageLoss> { }

// Data Manager
	public class vwPackageLossDataManager : DataManager<IvwPackageLoss, vwPackageLoss>, IvwPackageLossDataManager, IDataManager
    {
        #region IDataManager<> Members

        public IQueryable<IvwPackageLoss> GetAll( )
        {
            return vwPackageLoss.All( );
        }

        public IvwPackageLoss Get( Guid _id )
        {
            return null;
        }

        #endregion
    }

// Interface
    public interface IvwPackageLossByParsedDatumDataManager : IDataManager<IvwPackageLossByParsedDatum> { }

// Data Manager
	public class vwPackageLossByParsedDatumDataManager : DataManager<IvwPackageLossByParsedDatum, vwPackageLossByParsedDatum>, IvwPackageLossByParsedDatumDataManager, IDataManager
    {
        #region IDataManager<> Members

        public IQueryable<IvwPackageLossByParsedDatum> GetAll( )
        {
            return vwPackageLossByParsedDatum.All( );
        }

        public IvwPackageLossByParsedDatum Get( Guid _id )
        {
            return null;
        }

        #endregion
    }

// Interface
    public interface IvwPercentItemsReturnedDataManager : IDataManager<IvwPercentItemsReturned> { }

// Data Manager
	public class vwPercentItemsReturnedDataManager : DataManager<IvwPercentItemsReturned, vwPercentItemsReturned>, IvwPercentItemsReturnedDataManager, IDataManager
    {
        #region IDataManager<> Members

        public IQueryable<IvwPercentItemsReturned> GetAll( )
        {
            return vwPercentItemsReturned.All( );
        }

        public IvwPercentItemsReturned Get( Guid _id )
        {
            return null;
        }

        #endregion
    }

// Interface
    public interface IvwPercentItemsReturnedMonthlyDataManager : IDataManager<IvwPercentItemsReturnedMonthly> { }

// Data Manager
	public class vwPercentItemsReturnedMonthlyDataManager : DataManager<IvwPercentItemsReturnedMonthly, vwPercentItemsReturnedMonthly>, IvwPercentItemsReturnedMonthlyDataManager, IDataManager
    {
        #region IDataManager<> Members

        public IQueryable<IvwPercentItemsReturnedMonthly> GetAll( )
        {
            return vwPercentItemsReturnedMonthly.All( );
        }

        public IvwPercentItemsReturnedMonthly Get( Guid _id )
        {
            return null;
        }

        #endregion
    }

// Interface
    public interface IvwPOSItemSummaryDataManager : IDataManager<IvwPOSItemSummary> { }

// Data Manager
	public class vwPOSItemSummaryDataManager : DataManager<IvwPOSItemSummary, vwPOSItemSummary>, IvwPOSItemSummaryDataManager, IDataManager
    {
        #region IDataManager<> Members

        public IQueryable<IvwPOSItemSummary> GetAll( )
        {
            return vwPOSItemSummary.All( );
        }

        public IvwPOSItemSummary Get( Guid _id )
        {
            return null;
        }

        #endregion
    }

// Interface
    public interface IvwPOSTopTenDataManager : IDataManager<IvwPOSTopTen> { }

// Data Manager
	public class vwPOSTopTenDataManager : DataManager<IvwPOSTopTen, vwPOSTopTen>, IvwPOSTopTenDataManager, IDataManager
    {
        #region IDataManager<> Members

        public IQueryable<IvwPOSTopTen> GetAll( )
        {
            return vwPOSTopTen.All( );
        }

        public IvwPOSTopTen Get( Guid _id )
        {
            return null;
        }

        #endregion
    }

// Interface
    public interface IvwPOSTotalDataManager : IDataManager<IvwPOSTotal> { }

// Data Manager
	public class vwPOSTotalDataManager : DataManager<IvwPOSTotal, vwPOSTotal>, IvwPOSTotalDataManager, IDataManager
    {
        #region IDataManager<> Members

        public IQueryable<IvwPOSTotal> GetAll( )
        {
            return vwPOSTotal.All( );
        }

        public IvwPOSTotal Get( Guid _id )
        {
            return null;
        }

        #endregion
    }
}
