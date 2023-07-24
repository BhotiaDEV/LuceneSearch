using org.apache.tika;
using Syncfusion.Presentation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TikaOnDotNet.TextExtraction;

namespace Lucene.Services
{
    internal class PptDocService
    {
        public void ReadPpt (string filepath)
        {
            TextExtractor tika = new TextExtractor();
            Console.WriteLine(tika.Extract(filepath).Text);
            Console.ReadLine();
        }
    }
}
