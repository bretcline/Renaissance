using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace TicketFinderService
{
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
            set
            {
                if (value.StartsWith("\\"))
                {
                    filename = value.Substring(1);
                }
                else
                {
                    filename = value;
                }
            }
        }

        [DataMember]
        public string Path
        {
            get { return path; }
            set
            {
                if (value.StartsWith("\\"))
                {
                    path = value.Substring(1);
                }
                else
                {
                    path = value;
                }
            }
        }

        [DataMember]
        public byte[] DataStream
        {
            get { return dataStream.ToArray(); }
            set
            {
                if( null != value )
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
}