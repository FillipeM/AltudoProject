using System;
using System.Collections.Generic;
using System.Text;

namespace AltudoProjectModels
{
    public class AltudoModel
    {
        public List<String> ImageList { get; set; }
        public List<WordsRanking> WordsRanking { get; set; }
    }

    public class WordsRanking
    {
        public string Word { get; set; }
        public int TimesAppears { get; set; }
    }
}
