using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Micros.DataLoader.Parsers
{


    public class BaseMessage : IMessage
    {
        public string Driver { get; set; }

        public UInt64 Type { get; internal set; }

        public DateTime ReadTime { get; set; }
    }

    public class Ticket : BaseMessage, ITicket
    {
        public Ticket( )
        {
            //Type = MessageType.RawData;
            Items = new List<ITicketItem>( );
            TVAItems = new List<ITVAData>();
            PaymentItems = new List<IPaymentData>();
            RawData = new List<string>();
        }

        #region ITicket Elements
        public string CheckNumber { get; set; }

        public string TransactionID { get; set; }

        public string Comments { get; set; }

        public string Date { get; set; }

        public string Establishment { get; set; }

        public string Server { get; set; }

        public string ServerNumber { get; set; }

        public int GuestCount { get; set; }

        public List<ITicketItem> Items { get; set; }

        public List<ITVAData> TVAItems { get; set; }

        public List<IPaymentData> PaymentItems { get; set; }

        private decimal m_TipAmount;
        public decimal TipAmount
        {
            get { return m_TipAmount; }
            set { m_TipAmount = value; TipAmountModified = true; }
        }

        public List<string> RawData { get; set; }

        public string Table { get; set; }

        public int TouchCount { get; set; }

        public string DataSource { get; set; }
        #endregion

        public string GetRawData()
        {
            var data = new StringBuilder();
            foreach (var line in RawData)
            {
                data.Append($"{line}{Environment.NewLine}");
            }
            return data.ToString();
        }

        private decimal m_CheckTotal;
        public decimal CheckTotal
        {
            get { return m_CheckTotal; }
            set { m_CheckTotal = value; CheckTotalModified = true; }
        }
        public bool CheckTotalModified { get; set; }
        public bool TipAmountModified { get; set; }

        public int TransactionType { get; set; }


        public override string ToString( )
        {
            StringBuilder rc = new StringBuilder( );
            rc.Append( string.Format( "Establishment: {0}{1}", Establishment, System.Environment.NewLine ) );
            rc.Append( string.Format( "Server: {0}{1}", Server, System.Environment.NewLine ) );
            rc.Append( string.Format( "Check Number: {0}{1}", CheckNumber, System.Environment.NewLine ) );
            rc.Append( string.Format( "Table: {0}{1}", Table, System.Environment.NewLine ) );
            rc.Append( string.Format( "Date: {0}{1}", Date, System.Environment.NewLine ) );
            rc.Append( string.Format( "Items: {1}", Date, System.Environment.NewLine ) );
            if( null != Items )
            {
                foreach( var Item in Items )
                {
                    rc.Append( string.Format( "{0}{1}", Item.ToString( ), System.Environment.NewLine ) );
                }
            }
            if (null != Items)
            {
                foreach (var Item in TVAItems)
                {
                    rc.Append(string.Format("{0}{1}", Item.ToString(), System.Environment.NewLine));
                }
            }
            if (null != Items)
            {
                foreach (var Item in PaymentItems)
                {
                    rc.Append(string.Format("{0}{1}", Item.ToString(), System.Environment.NewLine));
                }
            }

            return rc.ToString( );
        }
    }

    public class TicketItem : ITicketItem
    {
        public TicketItem( )
        {
            Modifiers = new List<ITicketItemModifier>( );
        }

        public string Comment { get; set; }

        public string Description { get; set; }

        public string UPC { get; set; }

        public decimal Price { get; set; }

        public bool Credit { get; set; }

        public int Quantity { get; set; }

        public bool IsVoid { get; set; }

        public List<ITicketItemModifier> Modifiers { get; set; }

        public override string ToString( )
        {
            StringBuilder rc = new StringBuilder( );
            rc.Append( string.Format( "\tQuantity: {0}{1}", Quantity, System.Environment.NewLine ) );
            rc.Append( string.Format( "\tDescription: {0}{1}", Description, System.Environment.NewLine ) );
            rc.Append( string.Format( "\tPrice: {0}{1}", Price, System.Environment.NewLine ) );
            rc.Append(string.Format("\tCredit: {0}{1}", Credit, System.Environment.NewLine));
            rc.Append( string.Format( "\tComment: {0}{1}", Comment, System.Environment.NewLine ) );
            if( null != Modifiers )
            {
                foreach( TicketItemModifier Item in Modifiers )
                {
                    rc.Append( string.Format( "{0}{1}", Item.ToString( ), System.Environment.NewLine ) );
                }
            }
            return rc.ToString( );
        }
    }

    public class TicketItemModifier : ITicketItemModifier
    {
        public string Name { get; set; }

        public decimal Price { get; set; }

        public override string ToString( )
        {
            StringBuilder rc = new StringBuilder( );
            rc.Append( string.Format( "\t\tName: {0}{1}", Name, System.Environment.NewLine ) );
            rc.Append( string.Format( "\t\tPrice: {0}{1}", Price, System.Environment.NewLine ) );
            return rc.ToString( );
        }
    }

    public class TVAData : ITVAData
    {
        public decimal Amount { get; set; }

        public decimal Percentage { get; set; }

        public decimal Total { get; set; }

        public override string ToString()
        {
            StringBuilder rc = new StringBuilder();
            rc.Append(string.Format("\tAmount: {0}{1}", Amount, System.Environment.NewLine));
            rc.Append(string.Format("\tPercentage: {0}{1}", Percentage, System.Environment.NewLine));
            rc.Append(string.Format("\tTotal: {0}{1}", Total, System.Environment.NewLine));
            return rc.ToString();
        }
    }

    public class PaymentData : IPaymentData
    {
        public string AccountNumber { get; set; }

        public string CustomerName { get; set; }

        public decimal Payment { get; set; }

        public string PaymentType { get; set; }

        public string RoomNumber { get; set; }

        public override string ToString()
        {
            StringBuilder rc = new StringBuilder();
            rc.Append(string.Format("\tAccountNumber: {0}{1}", AccountNumber, System.Environment.NewLine));
            rc.Append(string.Format("\tCustomerName: {0}{1}", CustomerName, System.Environment.NewLine));
            rc.Append(string.Format("\tPayment: {0}{1}", Payment, System.Environment.NewLine));
            rc.Append(string.Format("\tPaymentType: {0}{1}", PaymentType, System.Environment.NewLine));
            rc.Append(string.Format("\tRoomNumber: {0}{1}", RoomNumber, System.Environment.NewLine));
            return rc.ToString();
        }
    }
}
