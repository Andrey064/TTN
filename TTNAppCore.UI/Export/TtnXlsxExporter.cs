using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TTNAppCore.Model;
using static System.Reflection.Metadata.BlobBuilder;

namespace TTNAppCore.UI.Export
{
    public class TtnXlsxExporter : ITtnXlsxExporter
    {
        public TtnXlsxExporter()
        {

        }

        private PropertyInfo[] GetListOfProperties(Ttn ttn)
        {
            Type type = ttn.GetType();
            PropertyInfo[] properties = type.GetProperties();
            foreach (PropertyInfo property in properties)
            {
                //propertiesList.Add
                Console.WriteLine("Name: " + property.Name + ", Value: " + property.GetValue(ttn, null));
            }
            return properties;
        }
        public void Export(Ttn ttn)
        {
            GetListOfProperties(ttn);

            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.Filter = "Excel files (*.xlsx;xls)|*.xlsx;*.xls";
            dlg.FileName = $"ТТН №{ttn.Num} от {ttn.Date.ToShortDateString()}";

            if (dlg.ShowDialog() == true)
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                var directory = System.AppDomain.CurrentDomain.BaseDirectory;
                var templateFile = $"{directory}Templates\\Ttn.xlsx";

                using (var package = new ExcelPackage(new FileInfo(templateFile)))
                {
                    var ws = package.Workbook.Worksheets["Организация"];

                    foreach (var property in GetListOfProperties(ttn))
                    {
                        // search in all cells
                        var query = from cell in ws.Cells["A:XFD"]
                                    where cell.Value?.ToString().Contains(property.Name) == true
                                    select cell;


                        foreach (var cell in query)
                        {
                            if (property.Name == "Date")
                            {
                                DateTime result;
                                var d = DateTime.TryParse(property.GetValue(ttn, null).ToString(), out result);
                                cell.Value = cell.Value.ToString().Replace(property.Name, result.ToShortDateString());
                            }
                            else if (property.Name == "Amount")
                            {
                                cell.Value = cell.Value.ToString().Replace(property.Name, property.GetValue(ttn, null).ToString());
                            }
                            else
                                cell.Value = cell.Value.ToString().Replace(property.Name, (string?)property.GetValue(ttn, null));
                        }
                    }

                    package.SaveAs(new FileInfo(dlg.FileName));

                    var psi = new ProcessStartInfo(dlg.FileName)
                    {
                        UseShellExecute = true
                    };
                    Process.Start(psi);

                }
            }
        }
    }
}
