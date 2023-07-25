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

namespace LuceneConsoleApp
{
    internal class LuceneSearchEngine
    {

        private static string basePath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
        private readonly string indexPath = Path.Combine(basePath, "index");
        private readonly Lucene.Net.Store.Directory directory;
        private readonly Analyzer _analyzer;
        private readonly IndexWriter _writer;

        public LuceneSearchEngine()
        {
            directory = FSDirectory.Open(indexPath);
            _analyzer = new StandardAnalyzer(Lucene.Net.Util.LuceneVersion.LUCENE_48);
            var indexConfig = new IndexWriterConfig(Lucene.Net.Util.LuceneVersion.LUCENE_48, _analyzer);
            _writer = new IndexWriter(directory, indexConfig);
        }

        public void AddDocumentToIndex()
        {
            var doc = new Document();
            string[] pdffiles = System.IO.Directory.GetFiles("Data", "*.pdf");
            string[] textfiles = System.IO.Directory.GetFiles("Data", "*.txt");
            string[] docfiles = System.IO.Directory.GetFiles("Data", "*.doc");
            if (!pdffiles.Any())
            {
                PdfDocService pdfDocService = new PdfDocService();
                foreach(string pdffile in pdffiles)
                {
                     var pdfcontent = pdfDocService.ReadPdf(pdffile);
                    doc.Add(new TextField("Path", "Data/Renewables2022.pdf" , Field.Store.YES));
                    doc.Add(new TextField("Content", pdfcontent , Field.Store.YES));
                    _writer.AddDocument(doc);
                }
            }
            if (!textfiles.Any())
            {
                foreach (string file in textfiles)
                {
                    var textcontent = File.ReadAllText(file);
                    string textpath = Path.GetFileName(file);
                    doc.Add(new TextField("Path", textpath, Field.Store.YES));
                    doc.Add(new TextField("Content", textcontent, Field.Store.YES));
                    _writer.AddDocument(doc);
                }
            }
            if (!docfiles.Any())
            {
                foreach (string file in docfiles)
                {
                    var doccontent = File.ReadAllText(file);
                    string docpath = Path.GetFileName(file);
                    doc.Add(new TextField("Path", docpath, Field.Store.YES));
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
            
            String[] textfields = { "Path", "Content" };
            var indexReader = DirectoryReader.Open(directory);
            var indexSearcher = new IndexSearcher(indexReader);

            QueryParser parser = new MultiFieldQueryParser(Lucene.Net.Util.LuceneVersion.LUCENE_48, textfields, _analyzer);
            parser.AllowLeadingWildcard = true;
            
            var query = parser.Parse(search);

            var collector = TopScoreDocCollector.Create(100, true);
            indexSearcher.Search(query, collector);


            var topDocs = collector.GetTopDocs();
            var hits = topDocs.ScoreDocs;

            foreach (var hit in hits)
            {
                var id = hit.Doc;
                var doc = indexSearcher.Doc(id);
                DocModel data = new DocModel()
                {
                    Path = doc.Get("Path"),
                    Content = doc.Get("Content")
                };
                Results.Add(data);
            }

            return Results;
        }

    }
}
