
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using Lucene.Services;
using LuceneConsoleApp;
using LuceneConsoleApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Lucene
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var excel = new ExcelDocService();
            excel.ReadExcelFile("Data/Employee.xlsx");
            Console.ReadLine();

            var lucene = new LuceneSearchEngine();
            lucene.AddDocumentToIndex();
            while (true)
            {
                Console.WriteLine("Enter a Search Query: ");
                var query = Console.ReadLine();
                if(string.IsNullOrEmpty(query)) 
                    continue;

                var results = lucene.SearchOnIndex(query);

                if (!results.Any())
                {
                    Console.WriteLine("No Results Found");
                    continue;
                }

                Console.WriteLine($" {results.Count()}  Hits found! \n");

                foreach(var result in results)
                {
                    Console.WriteLine($"\n Path:{result.Path} | {result.Page} ");
                }
                Console.WriteLine("Press Any Key to Continue : ");
                Console.ReadLine();
                Console.Clear();

            }
        }
    }
}
