using OfficeOpenXml;
using System;
using System.IO;

namespace Lucene.Services
{
    internal class ExcelDocService
    {
        public void ReadExcel(string filepath)
        {
            ExcelPackage.LicenseContext = LicenseContext.Commercial;
            using (var package = new ExcelPackage(new FileInfo(filepath)))
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
