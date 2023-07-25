using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Micros.DataLoader.Parsers
{


    public interface IMessage
    {
        UInt64 Type { get; }

        DateTime ReadTime { get; set; }
    }


    public interface ITicket : IMessage
    {
        string CheckNumber { get; set; }

        string TransactionID { get; set; }

        string Comments { get; set; }

        string Date { get; set; }

        string Establishment { get; set; }

        string Server { get; set; }

        string ServerNumber { get; set; }
        
        int GuestCount { get; set; }

        List<ITicketItem> Items { get; }

        List<ITVAData> TVAItems { get; }

        List<IPaymentData> PaymentItems { get; }

        decimal TipAmount { get; set; }
        bool TipAmountModified { get; set; }

        List<string> RawData { get; set; }

        string Table { get; set; }

        int TouchCount { get; set; }
        string GetRawData();

        decimal CheckTotal { get; set; }

        bool CheckTotalModified { get; set; }

        string DataSource { get; set; }

        int TransactionType { get; set; }
    }

    public interface ITicketItem
    {
        string Comment { get; set; }

        string Description { get; set; }

        string UPC { get; set; }

        decimal Price { get; set; }

        bool Credit { get; set; }

        int Quantity { get; set; }

        bool IsVoid { get; set; }

        List<ITicketItemModifier> Modifiers { get; }
    }

    public interface ITicketItemModifier
    {
        string Name { get; }

        decimal Price { get; }
    }

    public interface ITVAData
    {
        decimal Amount { get; }

        decimal Percentage { get; }

        decimal Total { get; }
    }


    public interface IPaymentData
    {
        string AccountNumber { get; set; }
        string RoomNumber { get; set; }
        string CustomerName { get; set; }
        string PaymentType { get; set; }
        decimal Payment { get; set; }
    }

}
