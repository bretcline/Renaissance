using Jaxis.Util.Log4Net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLoad
{
    class Program
    {
        static void Main(string[] args)
        {
            var processAll = false;
            Log.Debug("Start DataLoad");

            if ( args.Length > 1 )
            {
                switch (args[1].ToUpper())
                {
                    case "VALIDATE":
                    {
                        Log.Debug("Start ValidatePath");
                        ValidatePaths();
                        break;
                    }
                    case "ALL":
                    {
                        processAll = true;
                        break;
                    }
                }
            }


            LoadJournalFiles();


            var configFile = args[0];
            Log.Debug( string.Format( "Loading Config: {0}", configFile) );

            using (var reader = new StreamReader(configFile))
            {
                var configs = reader.ReadToEnd();
                var configItems = configs.Split('\n');
                foreach (var item in configItems)
                {
                    if (!string.IsNullOrWhiteSpace(item))
                    {
                        var elements = item.Split('\t');

                        var tableData = elements[0].Split('.');
                        var file = elements[1];
                        var delimiter = GetDelimiterCharacter(elements[2]);
                        var extraColumns = Convert.ToInt32(elements[3]);
                        var dateInFile = Convert.ToBoolean(elements[4]);
                        var format = elements[5];
                        var path = Path.GetDirectoryName(file).Trim();
                        var fileName = Path.GetFileNameWithoutExtension(file).Trim();
                        var extension = Path.GetExtension(file).Trim();

                        if (processAll)
                        {
                            var files = Directory.GetFiles(path, $"{fileName}*.*");
                            foreach (var currentFile in files)
                            {
                                Log.Debug($"File: {file}");
                                ProcessFile(currentFile, tableData, delimiter, extraColumns);
                            }
                        }
                        else if (dateInFile)
                        {
                            var date = DateTime.Now.AddDays(-1).Date.ToString(format).Trim();

                            file = $@"{path}\{fileName}.{date}{extension}";

                            Log.Debug($"File: {file}");
                            ProcessFile(file, tableData, delimiter, extraColumns);
                        }
                        else
                        {
                            Log.Debug($"File: {file}");
                            ProcessFile(file, tableData, delimiter, extraColumns);
                        }
                    }
                }
            }
        }

        private static void ProcessFile(string file, string[] tableData, char delimiter, int extraColumns)
        {
            if (File.Exists(file))
            {
                Log.Debug(string.Format("Load File: {0}", file));

                var loader = new ExcelLoaders.CSVBulkLoad();
                loader.LoadData(tableData[1], file, delimiter, 10000, tableData[0], extraColumns);
            }
            else
            {
                Log.Debug("No such file");
                Log.Debug(file);
            }
        }

        private static void ValidatePaths()
        {
            for (int i = 1; i < 13; ++i)
            {
                Log.Debug(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(i).Replace('é', 'e').Replace('û', 'u'));
            }

            var path = System.Configuration.ConfigurationManager.AppSettings["JournalPath"];
            var config = System.Configuration.ConfigurationManager.AppSettings["ConfigFile"];
            Log.Debug(string.Format("{0} - {1}", config, File.Exists(config)));

            var currentDate = DateTime.Today.AddDays(-1);

            string strMonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(currentDate.Month).Replace('é', 'e').Replace('û', 'u');

            var fileName = string.Format("{0}.jnl", currentDate.ToString("ddMMyyy"));
            var monthPath = Path.Combine(path, strMonthName, fileName);

            var processPath = Path.Combine(path, "..\\JournalProcess");

            Log.Debug(processPath);
            Log.Debug(string.Format("{0} - {1}", monthPath, File.Exists(monthPath)));
        }

        private static void LoadJournalFiles()
        {
            var path = System.Configuration.ConfigurationManager.AppSettings["JournalPath"];
            Log.Debug(path);
            var processPath = CopyJournalFiles(path);
            Log.Debug(processPath);

            var config = System.Configuration.ConfigurationManager.AppSettings["ConfigFile"];
            var loader = new Micros.DataLoader.JournalLoader(config, false, "");

            Log.Debug("Load Journal: " + processPath);
            loader.LoadJournal(processPath);
        }

        private static string CopyJournalFiles(string path)
        {
            string processPath = path;
            try
            {
                var containsMonth = Convert.ToBoolean(  System.Configuration.ConfigurationManager.AppSettings["JournalPathWithMonth"] );
                var currentDate = DateTime.Today.AddDays( -1 );

                string strMonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(currentDate.Month).Replace('é', 'e').Replace('û', 'u');
                
                var fileName = string.Format("{0}.{1}.jnl", currentDate.ToString("ddMMyyy"), currentDate.ToString("dd.MM.yyy"));
                var monthPath = Path.Combine(path, fileName);
                if (containsMonth)
                {
                    monthPath = Path.Combine(path, strMonthName, fileName);
                }
                Log.Debug(string.Format("File to copy: {0}", monthPath));

                processPath = Path.Combine(path, "..\\JournalProcess");
                if( !Directory.Exists( processPath ))
                {
                    Directory.CreateDirectory(processPath);
                }
                if( File.Exists( monthPath ) )
                {
                    Log.Debug("Copy Journal: " + monthPath);
                    Log.Debug("Copy Journal: " + fileName);

                    File.Move(monthPath, Path.Combine(processPath, fileName));
                }
                else
                {
                    Log.Debug("file does not exist: " + monthPath);
                }
            }
            catch ( Exception err)
            {
                Log.Debug(err.Message);
            }
            return processPath;
        }

        protected static char GetDelimiterCharacter( string delimiters )
        {
            char rc = ',';
            switch( delimiters )
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
