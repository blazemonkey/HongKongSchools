using HongKongSchools.DataParser.Models.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HongKongSchools.DataParser.Services.ExcelReaderService
{
    public interface IExcelReaderService
    {
        IEnumerable<T> Read<T>(string filePath) where T : ExcelFile, new();
    }
}
