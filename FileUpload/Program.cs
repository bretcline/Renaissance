using FileUpload.svcDataLoaderWCF;
using System;
using System.Configuration;
using System.IO;

namespace FileUpload
{
    class Program
    {
        static void Main(string[] args)
        {
            //var service = new svcTicketFinder.TicketFinderClient();
            var service = new svcDataLoaderWCF.DataLoaderWCFClient();
            var dataPath = ConfigurationManager.AppSettings["DataPath"];
            
            // Read License file and derive the Customer ID from it.

            var customerId = new Guid( ConfigurationManager.AppSettings["CustomerID"] );

            var files = Directory.GetFiles(dataPath);

            foreach( var file in files )
            {
                try
                {
                    var name = Path.GetFileNameWithoutExtension(file);
                    var fileInfo = new FileInfo( file );
                    var extension = fileInfo.Extension;
                    var date = fileInfo.LastWriteTime;
                    var filename = $"{name}.{date.Day:00}.{date.Month:00}.{date.Year:0000}{extension}";
                    File.Move( file, filename );

                    var uploadFile = new POSFile
                    {
                        DataStream = File.ReadAllBytes(filename),
                        FileName = Path.GetFileName(filename),
                    };
                    var rc = service.SubmitFile(customerId, uploadFile);

                    Console.WriteLine(string.Format("{1} - {0}", uploadFile, rc));
                    File.Delete( filename );
                }
                catch (Exception err)
                {
                    Console.WriteLine( err.Message );
                }
            }
            service.LoadDataFiles(customerId);
        }
    }
}
