using System;
using System.IO;

namespace Jaxis.Data.Service
{
    public class DataFileConfig
    {
        public string[] TableData { get; set; }
        public string FullFileName { get; set; }
        public string FileName { get; set; }
        public char Delimiter { get; set; }
        public int ExtraColumns { get; set; }
        public string Extension { get; set; }

        public DataFileConfig(string item)
        {
            if (!string.IsNullOrWhiteSpace(item))
            {
                var elements = item.Split('\t');
                TableData = elements[0].Split('.');
                FullFileName = elements[1];
                Delimiter = GetDelimiterCharacter(elements[2]);
                ExtraColumns = Convert.ToInt32(elements[3]);

                Extension = Path.GetExtension(FullFileName)?.Trim();
                FileName = Path.GetFileNameWithoutExtension(FullFileName)?.Trim();
            }
        }

        private DateTime GetDateFromFileName(string file)
        {
            var filePieces = file.Split('.');
            var pieceCount = filePieces.Length;
            return new DateTime(Convert.ToInt32(filePieces[pieceCount - 2]), Convert.ToInt32(filePieces[pieceCount - 3]), Convert.ToInt32(filePieces[pieceCount - 4]));
        }

        protected static char GetDelimiterCharacter(string delimiters)
        {
            var rc = ',';
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