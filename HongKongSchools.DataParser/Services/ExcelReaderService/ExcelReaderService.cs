using HongKongSchools.DataParser.Models.Abstracts;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HongKongSchools.DataParser.Services.ExcelReaderService
{
    public class ExcelReaderService : IExcelReaderService
    {
        public IEnumerable<T> Read<T>(string filePath) where T : ExcelFile, new()
        {
            var inputs = new List<T>();

            var existingFile = new FileInfo(filePath);
            using (var package = new ExcelPackage(existingFile))
            {
                var workBook = package.Workbook;
                if (workBook != null)
                {
                    if (workBook.Worksheets.Count > 0)
                    {
                        var currentWorksheet = workBook.Worksheets.First();
                        
                        for (var x = 2; x <= 10000; x++)
                        {
                            if (currentWorksheet.Cells[x, 1].Value == null)
                                break;

                            var input = new T();
                            var columnCount = input.ColumCount;

                            for (var y = 1; y <= columnCount; y++)
                            {
                                var cell = currentWorksheet.Cells[x, y].Value;
                                var value = cell == null ? "" : cell.ToString();
                                input.SetProperty(y, value);
                            }

                            inputs.Add(input);
                        }
                    }
                }
            }

            return inputs;
        }
    }
}
