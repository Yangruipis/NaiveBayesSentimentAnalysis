using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using JiebaNet.Segmenter;
using JiebaNet;
using System.Diagnostics;

namespace Sentiment_Console
{
    public static class Utility
    {
        //public static T print_<T>(this T str)
        //{
        //    Console.WriteLine(str);
        //    return str;
        //}
        public static T[] print<T>(this T[] arr)
        {
            foreach (var ar in arr)
            {
                Console.Write(ar);
                Console.Write("/");
            }
            return arr;
        } 
    }
    class Program
    {
        readonly static string SentimentFilepath = System.Environment.CurrentDirectory+"\\sentiment\\";
        static void Main(string[] args)
        {
            double total = 0;
            var d = Train.Load(SentimentFilepath + "sentiment_json.txt",out total);
            var poswords = Train.ReadTxtToEnd(SentimentFilepath + "pos.csv");
            var negwords = Train.ReadTxtToEnd(SentimentFilepath + "neg.csv");
            Train.Train_data(SentimentFilepath + "neg_train.csv", negwords, SentimentFilepath + "pos_train.csv", poswords,
                ref d, ref total, SentimentFilepath + "stopwords.csv");
            //foreach (var key in d.Keys)
            //{
            //    Console.WriteLine(key);
            //    d[key].DPrint();
            //}

            string testsentence = "很忽悠，不好";
            var sent = Train.classify_(testsentence, d, total, SentimentFilepath + "stopwords.csv");
            Console.WriteLine(sent);
            Console.ReadKey();

            //Dictionary<string, int> d = new Dictionary<string, int>(){{"b",2},{"a",1}};
            //BaseProb base1 = new BaseProb();
            //base1.d.Add("a", 1);
            //bool para1;
            //double para2;
            //Jieba jiebacutstring = new Jieba();
            //jiebacutstring.doc = "我叫杨睿，来自上海对外经贸大学！";
            //jiebacutstring.JiebaCut().ToArray().print();
            //Console.ReadKey();

            //base1.get("b",out para1,out para2);
            //Console.WriteLine(para1.ToString()+","+para2.ToString());
            //AddOneProb _d = new AddOneProb();
            //_d.add("a", 1);
            //_d.add("a", 2);//此处出错，派生类没有继承基类的属性值(已解决)
            
            //Console.WriteLine("_d: "+_d.total);
            //Console.WriteLine(d.Keys.Contains("a"));
            //Console.ReadKey();
        }

        /// <summary>
        /// 分词并剔除排除词
        /// </summary>
        /// <param name="doc"></param>
        /// <returns>分词结果_列表</returns>
        //public static List<string> handle(string doc)
        //{
        //    var segmenter = new JiebaSegmenter();
        //    var segments = segmenter.Cut(doc);
        //    string stopwords = Train.Stopwords(stopwordFilepath);
        //    List<string> result = new List<string>();
        //    foreach (var i in segments)
        //    {
        //        if (!stopwords.Contains(i))
        //            result.Add(i);
        //    }
        //    return result;
        //}

    }
}
