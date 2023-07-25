using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Core;
using Lucene.Net.Analysis.En;
using Lucene.Net.Analysis.Miscellaneous;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lucene
{
    public class CustomAnalyzer : Analyzer
    {
        protected override TokenStreamComponents CreateComponents(string fieldName, TextReader reader)
        {
            StandardTokenizer src = new StandardTokenizer(Net.Util.LuceneVersion.LUCENE_48,reader);
            TokenStream result = new StandardFilter(LuceneVersion.LUCENE_48, src);
            result = new LowerCaseFilter(LuceneVersion.LUCENE_48, result);
            result = new StopFilter(LuceneVersion.LUCENE_48, result, StandardAnalyzer.STOP_WORDS_SET);
            result = new PorterStemFilter(result);

            return new TokenStreamComponents(src, result);
        }
    }
}
