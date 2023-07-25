using Jaxis.POS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace TicketFinderService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ITicketFinder" in both code and config file together.
    [ServiceContract]
    public interface ITicketFinder
    {

        [OperationContract]
        IEnumerable<DataMicrosTimePeriod> GetTimePeriods(Guid _sessionId);

        [OperationContract]
        IEnumerable<DataPOSEstablisment> GetEstablisments(Guid _sessionId);

        [OperationContract]
        IEnumerable<DatavwPOSTicket> GetTicketData(Guid _sessionId, string _ticketNumber, DateTime? _startDate, DateTime? _endTime, string _establishment, string _ticketText, int _timePeriod);

        [OperationContract]
        Guid Login(string _userId, string _password);

        [OperationContract]
        bool SubmitFile(Guid _customerId, POSFile _file);

        [OperationContract]
        string GetDataQuery(Guid _sessionId);


    }
}
