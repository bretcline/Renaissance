using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace TicketFinderService
{
    public class DataFileParserConfig
    {
        string[] TableData { get; set; }
        string Filename { get; set; }
        char Delimiter { get; set; }
        int ExtraColumns { get; set; }
        bool DateInFile { get; set; }
        string Format { get; set; }

        public static List<DataFileParserConfig> GetConfigs( string configFile )
        {
            var rc = new List<DataFileParserConfig>();
            using (var reader = new StreamReader(configFile))
            {
                var configs = reader.ReadToEnd();
                var configItems = configs.Split('\n');
                foreach (var item in configItems)
                {
                    var elements = item.Split('\t');

                    var config = new DataFileParserConfig();
                    
                    config.TableData = elements[0].Split('.');
                    config.Filename = elements[1];
                    config.Delimiter = GetDelimiterCharacter(elements[2]);
                    config.ExtraColumns = Convert.ToInt32(elements[3]);
                    config.DateInFile = Convert.ToBoolean(elements[4]);
                    config.Format = elements[5];
                    //if ( config.DateInFile )
                    //{
                    //    var format = elements[5];
                    //    var path = Path.GetDirectoryName(file).Trim();
                    //    var fileName = Path.GetFileNameWithoutExtension(file).Trim();
                    //    var extension = Path.GetExtension(file).Trim();
                    //    var date = DateTime.Now.AddDays(-1).Date.ToString(format).Trim();

                    //    file = string.Format(@"{0}\{1}.{2}{3}", path, fileName, date, extension);
                    //}
                }
            }
            return rc;
        }
        protected static char GetDelimiterCharacter(string delimiters)
        {
            char rc = ',';
            switch (delimiters)
            {
                case "\t":
                    {
                        rc = '\t';
                        break;
                    }
                case ",":
                    {
                        rc = ',';
                        break;
                    }
                case "|":
                    {
                        rc = '|';
                        break;
                    }
                default:
                    {
                        rc = delimiters[0];
                        break;
                    }
            }
            return rc;
        }

    }
}