using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;
using System.ComponentModel;
using System.IO;
using System.Globalization;
using System.Reflection;

namespace ExcelLoaders
{

    public class ExcelExport<T>
    {
        public BindingList<T> DataList { get; set; }

        public void ExportData( string fileName )
        {
            if( File.Exists( fileName ) )
            {
                File.Delete(fileName);
            }
            //var newfile = File.Create(fileName);
            //newfile.Close();

            var file = new FileInfo(fileName);

            using (var package = new ExcelPackage(file))
            {
                var sheetName = "Sheet1";

                package.Workbook.Worksheets.Add(sheetName);
                ExcelWorksheet ws = package.Workbook.Worksheets[1];
                ws.Name = sheetName; 
                ws.Cells.Style.Font.Size = 11; 
                ws.Cells.Style.Font.Name = "Calibri";

                ws.InsertRow(1, DataList.Count + 1);

                var props = typeof(T).GetProperties().ToList();

                CleanupProperties(props);

                for(int i = 0; i < props.Count; ++i)
                {
                    ws.SetValue(1, i+1, props[i].Name);

                    ws.Column(i + 1).AutoFit();
                }

                for( int r = 0; r < DataList.Count; ++r )
                {
                    for (int i = 0; i < props.Count; ++i)
                    {
                        ws.SetValue(r+2, i+1, props[i].GetValue(DataList[r]));
                        if( typeof( DateTime ) == props[i].PropertyType )
                        {
                            ws.Cells[r + 2, i + 1].Style.Numberformat.Format = "yyyy-mm-dd h:mm:ss";
                        }
                    }
                }
                package.Save();
            }
        }

        private void CleanupProperties(List<PropertyInfo> props)
        {
            var remove = new List<PropertyInfo>();
            foreach( var prop in props )
            {
                if( prop.Name == "Columns" || prop.Name == "ObjectID" || prop.Name == "POSTicketID" )
                {
                    remove.Add(prop);
                }
            }

            foreach( var prop in remove )
            {
                props.Remove(prop);
            }
        }
    }
}
