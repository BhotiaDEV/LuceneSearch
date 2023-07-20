using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lucene.Services
{
    internal class ExcelDocService
    {
        public void ReadExcelFile(string filePath)
        {
            ExcelPackage.LicenseContext = LicenseContext.Commercial;
            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                if (package.Workbook.Worksheets.Count > 0)
                {
                    var worksheet = package.Workbook.Worksheets[0]; // Assuming you want to read the first worksheet
                    int rowCount = worksheet.Dimension.Rows;
                    int colCount = worksheet.Dimension.Columns;

                    for (int row = 1; row <= rowCount; row++)
                    {
                        for (int col = 1; col <= colCount; col++)
                        {
                            var cellValue = worksheet.Cells[row, col].Value;
                            Console.Write($"{cellValue}\t");
                        }
                        Console.WriteLine();
                    }
                }
                else
                {
                    Console.WriteLine("No worksheets found in the Excel file.");
                }
            }
        }
    }
}
