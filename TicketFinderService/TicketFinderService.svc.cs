using Jaxis.POS.Data;
using Jaxis.Utility.Encryption;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;
using System.Text;
using Jaxis.POS.CustomerData;
using Jaxis.Utilities.Database;
using Jaxis.POS.CustomerData;

namespace TicketFinderService
{
    public class TicketFinder : ITicketFinder
    {
        List<DataFileParserConfig> m_ConfigElements = null;

        public Guid Login( string _userId, string _password)
        {
            Guid sessionId;

            var dPassword = BlowFishEncryption.Decrypt( _password );
            var hPassword = Encryption.Encrypt(EncryptionType.Hash, dPassword);

            var user = UserProperty.All().FirstOrDefault( u => u.UserName == _userId && u.Password == hPassword);

            if (null != user)
            {
                var session =
                    UserSession.All()
                        .FirstOrDefault(s => s.UserID == user.UserID && !s.SessionEnd.HasValue) ??
                    new UserSession
                        {
                            UserID = user.UserID,
                            SessionID = Guid.NewGuid(),
                            SessionStart = DateTime.Now,
                        };
                session.SessionUpdate = DateTime.Now;

                session.Save();
                sessionId = session.SessionID;
            }
            else
            {
                throw new InvalidUserException();
            }
           
            return sessionId;
        }


        public UserSession ValidateSession(Guid _sessionId)
        {
            var session = UserSession.All().FirstOrDefault(s => s.SessionID == _sessionId);
            if (null != session)
            {
                session.SessionUpdate = DateTime.Now;
                session.Save();
            }
            else
            {
                throw new InvalidSessionException();
            }
            return session;
        }

        public IEnumerable<DataMicrosTimePeriod> GetTimePeriods(Guid _sessionId)
        {
            var session = ValidateSession(_sessionId);
            return MicrosTimePeriod.All().Select(r => r.GetInternalData()).ToList();
        }

        public IEnumerable<DataPOSEstablisment> GetEstablisments( Guid _sessionId )
        {
            ValidateSession(_sessionId);
            return POSEstablisment.All( ).Select( r => r.GetInternalData( ) ).ToList();
        }

        public IEnumerable<DatavwPOSTicket> GetTicketData( Guid _sessionId, string _ticketNumber, DateTime? _startDate, DateTime? _endTime, string _establishment, string _ticketText, int _timePeriod )
        {
            ValidateSession(_sessionId);
            List<DatavwPOSTicket> rc = null;
            try
            {
                var tickets = Jaxis.POS.Data.vwPOSTicket.All();
                if (!string.IsNullOrWhiteSpace(_ticketNumber))
                {
                    tickets = tickets.Where(t => t.CheckNumber == _ticketNumber);
                }
                if (_endTime.HasValue)
                {
                    tickets = tickets.Where(t => t.TicketDate <= _endTime.Value);
                }
                if (_startDate.HasValue)
                {
                    tickets = tickets.Where(t => t.TicketDate >= _startDate.Value);
                }
                if (!string.IsNullOrWhiteSpace(_ticketText))
                {
                    tickets = tickets.Where(t => t.RawData.Contains(_ticketText));
                }
                if (!string.IsNullOrWhiteSpace(_establishment))
                {
                    tickets = tickets.Where(t => t.EstablishmentModified == _establishment);
                }
                //if ( timePeriod > 0 )
                //{
                //    tickets = tickets.Where(t => t.Time_Period_Num == timePeriod);
                //}


                rc = tickets.Select(r => r.GetInternalData()).ToList();
            }
            catch (Exception err)
            {
                throw;
            }
            return rc;
        }

        public string GetServerPath( Guid _customerId )
        {
            string rc;
            try
            {
                rc = "";
            }
            catch( Exception err )
            {
                throw;
            }
            return rc;
        }

        public bool SubmitFile( Guid _customerId, POSFile _file )
        {
            var rc = false;
            try
            {
                var customer = CustomerProperty.SingleOrDefault(c => c.CustomerID == _customerId);
                if (null != customer)
                {
                    var path = customer.UploadPath;
                    if ( !Directory.Exists( path ) )
                    {
                        Directory.CreateDirectory(path);
                    }

                    var extension = Path.GetExtension(_file.FileName);
                    path = Path.Combine(path, _file.FileName);
                    File.WriteAllBytes(path, _file.DataStream);

                    //if( extension == ".jnl" )
                    //{
                    //    LoadJournalFile(path);
                    //}
                    //else
                    //{
                    //    LoadDataFile(path);
                    //}

                    rc = true;
                }
            }
            catch (Exception err)
            {
                throw err;
            }
            return rc;
        }

        //private void LoadJournalFile(string _path)
        //{
        //    var config = System.Configuration.ConfigurationManager.AppSettings["JournelParseFile"];
        //    var loader = new Micros.DataLoader.JournalLoader(config, false);

        //    loader.LoadFile(_path);
        //}

        //private void LoadDataFile(string _path)
        //{
        //    if (null == m_ConfigElements)
        //    {
        //        var configFile = System.Configuration.ConfigurationManager.AppSettings["ConfigFile"];
        //        m_ConfigElements = DataFileParserConfig.GetConfigs(configFile);
        //    }
        //}

        public string GetDataQuery(Guid _sessionId)
        {
            ValidateSession(_sessionId);

            var rc = string.Empty;
            try
            {
                
            }
            catch( Exception err)
            {

            }
            return rc;
        }

        #region Custom Reports

        public Dictionary<string, string> GetCustomViews(Guid _sessionId)
        {
            var rc = new Dictionary<string, string>();
            ValidateSession(_sessionId);
            try
            {
                var connString = ConfigurationManager.ConnectionStrings["RenAix"].ConnectionString;
                var sqlTool = new SqlTool(connString);

                var query = $"select V.Name from sys.all_views V JOIN sys.schemas S ON V.schema_id = S.schema_id WHERE V.object_id >= 0 AND S.name = 'rpt'";
                using (var reader = sqlTool.ExecuteReader(query))
                {
                    while (reader.Read())
                    {
                        var view = reader.GetString(0);
                        rc.Add( view, GetReadableViewName( view ) );
                    }
                }
            }
            catch (Exception err)
            {

                throw;
            }


            return rc;
        }


        public List<string> GetCustomViewColumnss(Guid _sessionId, string viewName)
        {
            var rc = new List<string>();
            ValidateSession(_sessionId);
            try
            {
                var connString = ConfigurationManager.ConnectionStrings["RenAix"].ConnectionString;
                var sqlTool = new SqlTool(connString);

                var query = $"SELECT C.Name FROM sys.columns C JOIN sys.all_views V ON C.object_id = V.object_id WHERE V.Name = '{viewName}'";
                using (var reader = sqlTool.ExecuteReader(query))
                {
                    while (reader.Read())
                    {
                        var column = reader.GetString(0);
                        rc.Add(column);
                    }
                }
            }
            catch (Exception err)
            {

                throw;
            }


            return rc;
        }

        private string GetReadableViewName(string _viewName)
        {
            var rc = _viewName;

            if (_viewName.StartsWith("vw"))
            {
                rc = _viewName.Remove(0, 2);
            }

            return rc;
        }

        public DataSet GetCustomReportData(Guid _sessionId, string _view, string _parameters )
        {
            ValidateSession(_sessionId);
            var rc = new DataSet();

            try
            {
                var connString = ConfigurationManager.ConnectionStrings["RenAix"].ConnectionString;
                var sqlTool = new SqlTool( connString );

                var query = $"SELECT * FROM {_view} WHERE 1 = 1 AND {_parameters}";
                sqlTool.ExecuteQuery(ref rc, _view, query);

            }
            catch (Exception err)
            {
                
                throw;
            }

            return rc;
        }

        #endregion 
    }

    public class InvalidUserException
        : Exception
    {
        public InvalidUserException()
            : base("Invalid User/Password.")
        {
            
        }
    }

    public class InvalidSessionException 
        : Exception
    {
        public InvalidSessionException()
            : base("Invalid Session.")
        {
            
        }
    }
}
