using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JiebaNet;
using JiebaNet.Segmenter;
namespace Sentiment_Console
{
    class Jieba
    {
        public string doc{get;set;}
        public string PosWords  {get;set;}
        public string NegWords {get;set;}
        public string stopwords  {get;set;}
        public List<string> JiebaCut()
        {
            JiebaSegmenter jiebaseg = new JiebaSegmenter();
            //Console.WriteLine(doc);
            var segment = jiebaseg.Cut(doc);
            List<string> cutresult = new List<string>();
            foreach (var i in segment)
            {
                if (!stopwords.Contains(i))
                    cutresult.Add(i);
            }
            return cutresult;
        }
        public List<string> handle_sentiment(bool pos = true)
        {
            var words = JiebaCut();
            string PosOrNegWords = pos ? PosWords:NegWords;
            List<string> handle_result = new List<string>();
            foreach (var word in words)
            {
                if (PosOrNegWords.Contains(word))
                    handle_result.Add(word);
            }
            return handle_result;
        }
        public List<string> handle_words()
        {
            var words = JiebaCut();
            List<string> handle_result = new List<string>();
            foreach (var word in words)
            {
                if ((PosWords + NegWords).Contains(word))
                    handle_result.Add(word);
            }
            return handle_result;
        }
    }
}
