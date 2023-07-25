using iText.Kernel.Pdf.Canvas.Parser.Listener;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers.Classic;
using Lucene.Net.Search;
using Lucene.Net.Store;
using LuceneConsoleApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.ComponentModel;
using Lucene.Services;
using Lucene;

namespace LuceneConsoleApp
{
    internal class LuceneSearchEngine
    {

        private static string basePath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
        private readonly string indexPath = Path.Combine(basePath, "index");
        private readonly Lucene.Net.Store.Directory directory;
        private readonly CustomAnalyzer _analyzer;
        private readonly IndexWriter _writer;

        public LuceneSearchEngine()
        {
            directory = FSDirectory.Open(indexPath);
            _analyzer = new CustomAnalyzer();
            var indexConfig = new IndexWriterConfig(Lucene.Net.Util.LuceneVersion.LUCENE_48, _analyzer);
            _writer = new IndexWriter(directory, indexConfig);
        }

        public void AddDocumentToIndex()
        {

            // Read all files belonging to a specific file type
            string[] pdffiles = System.IO.Directory.GetFiles("Data", "*.pdf");
            string[] textfiles = System.IO.Directory.GetFiles("Data", "*.txt");
            string[] docfiles = System.IO.Directory.GetFiles("Data", "*.doc");
            string[] pptfiles = System.IO.Directory.GetFiles("Data", "*.ppt*");
            string[] excelfiles = System.IO.Directory.GetFiles("Data", "*.xls*");


            if (pdffiles.Any())
            {
                PdfDocService pdfDocService = new PdfDocService();
                foreach(string pdffile in pdffiles)
                {
                    var doc = new Document();
                    var pdfcontent = pdfDocService.ReadPdf(pdffile);
                    doc.Add(new StringField("Title", "Renewables2022.pdf" , Field.Store.YES));
                    doc.Add(new TextField("Content", pdfcontent , Field.Store.YES));
                    _writer.AddDocument(doc);
                }
            }
            if (pptfiles.Any())
            {
                PptDocService pptDocService = new PptDocService();
                foreach (string pptfile in pptfiles)
                {
                    var doc = new Document();
                    var pptcontent = pptDocService.ReadPpt(pptfile);
                    doc.Add(new StringField("Title", "Renewables2022.pdf", Field.Store.YES));
                    doc.Add(new TextField("Content", pptcontent, Field.Store.YES));
                    _writer.AddDocument(doc);
                }
            }
            if (excelfiles.Any())
            {
                ExcelDocService excelDocService = new ExcelDocService();
                foreach (string excelfile in excelfiles)
                {
                    var doc = new Document();
                    List<string> excelcontents = excelDocService.ReadExcel(excelfile);
                    foreach(string excelcontent in excelcontents)
                    {
                        doc.Add(new StringField("Title", "Renewables2022.pdf", Field.Store.YES));
                        doc.Add(new TextField("Content", excelcontent, Field.Store.YES));
                        _writer.AddDocument(doc);
                    }
                }
            }
            if (textfiles.Any())
            {
                foreach (string file in textfiles)
                {
                    var doc = new Document();
                    var textcontent = File.ReadAllText(file);
                    Console.WriteLine(Path.GetFileName(file));
                    doc.Add(new StringField("Title", file, Field.Store.YES));
                    doc.Add(new TextField("Content", textcontent, Field.Store.YES));
                    _writer.AddDocument(doc);
                }
            }
            if (docfiles.Any())
            {
                foreach (string file in docfiles)
                {
                    var doc = new Document();
                    var doccontent = File.ReadAllText(file);
                    doc.Add(new StringField("Title", file, Field.Store.YES));
                    doc.Add(new TextField("Content", doccontent, Field.Store.YES));
                    _writer.AddDocument(doc);
                }
            }
            _writer.Commit();
        }

        public IEnumerable<DocModel> SearchOnIndex(string search)
        {
            var Results = new List<DocModel>();
            if (search == null)
            {
                return Results;
            }
            
            String[] fields = { "Title","Content" };
            var indexReader = DirectoryReader.Open(directory);
            var indexSearcher = new IndexSearcher(indexReader);

            MultiFieldQueryParser parser = new MultiFieldQueryParser(Lucene.Net.Util.LuceneVersion.LUCENE_48, fields, _analyzer);
            parser.AllowLeadingWildcard = true;
            
            var query = parser.Parse(search);

            var collector = TopScoreDocCollector.Create(10, true);
            indexSearcher.Search(query, collector);


            var topDocs = collector.GetTopDocs();
            var hits = topDocs.ScoreDocs;

            foreach (var hit in hits)
            {
                var id = hit.Doc;
                var doc = indexSearcher.Doc(id);
                Results.Add( new DocModel()
                {
                    Title = doc.Get("Title"),
                    Content = doc.Get("Content")
                });
               Console.WriteLine(doc.Get("Title"));
                // Results.Add(data);
            }
            return Results;
        }
    }
}
