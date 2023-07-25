using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using Jaxis.POS.Data;

namespace Jaxis.Data.Service
{
    [ServiceContract]
    public interface IDataLoaderWCF
    {
        [OperationContract]
        bool LoadJournalFiles( Guid _customerId );

        [OperationContract]
        bool LoadDataFiles( Guid _customerId );

        [OperationContract]
        bool SubmitFile(Guid _customerId, POSFile _file);


        [OperationContract]
        IEnumerable<DataMicrosTimePeriod> GetTimePeriods(Guid _sessionId);

        [OperationContract]
        IEnumerable<DataPOSEstablisment> GetEstablisments(Guid _sessionId);

        [OperationContract]
        IEnumerable<DatavwPOSTicket> GetTicketData(Guid _sessionId, string _ticketNumber, DateTime? _startDate, DateTime? _endTime, string _establishment, string _ticketText, int _timePeriod);

        [OperationContract]
        Guid Login(string _userId, string _password);

        [OperationContract]
        string GetDataQuery(Guid _sessionId);
    }

    [DataContract]
    public class POSFile
    {
        string filename;
        string path;
        List<byte> dataStream = new List<byte>();

        [DataMember]
        public int ImportType { get; set; }

        [DataMember]
        public string FileName
        {
            get { return filename; }
            set { filename = value.StartsWith("\\") ? value?.Substring(1) : value; }
        }

        //[DataMember]
        //public string Path
        //{
        //    get { return path; }
        //    set { path = value.StartsWith("\\") ? value?.Substring(1) : value; }
        //}

        [DataMember]
        public byte[] DataStream
        {
            get { return dataStream.ToArray(); }
            set
            {
                if (null != value)
                {
                    if (null == dataStream)
                    {
                        dataStream = new List<byte>();
                    }
                    dataStream.Clear();
                    dataStream.AddRange(value);
                }
            }
        }
    }


    //// Use a data contract as illustrated in the sample below to add composite types to service operations.
    //// You can add XSD files into the project. After building the project, you can directly use the data types defined there, with the namespace "DataLoaderWCF.ContractType".
    //[DataContract]
    //public class CompositeType
    //{
    //    bool boolValue = true;
    //    string stringValue = "Hello ";

    //    [DataMember]
    //    public bool BoolValue
    //    {
    //        get { return boolValue; }
    //        set { boolValue = value; }
    //    }

    //    [DataMember]
    //    public string StringValue
    //    {
    //        get { return stringValue; }
    //        set { stringValue = value; }
    //    }
    //}
}
