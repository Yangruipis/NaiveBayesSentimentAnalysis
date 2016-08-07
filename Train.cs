using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
//using Newtonsoft.Json;
using LitJson;

namespace Sentiment_Console
{
    class Train
    {
        /// <summary>
        /// 读取现有训练集Json
        /// </summary>
        /// <param name="filepath">Json训练集路径</param>
        /// <param name="total">词条总数</param>
        /// <returns></returns>
        public static Dictionary<string, AddOneProb> Load(string filepath, out double total)
        {
            string json;
            using (var sr = new StreamReader(filepath, Encoding.Default))
                json = sr.ReadToEnd();

            JsonData jd = JsonMapper.ToObject(json);
            Dictionary<string, AddOneProb> d = new Dictionary<string, AddOneProb>() { { "neg", new AddOneProb() }, { "pos", new AddOneProb() } };

            foreach (var i in jd["d"]["neg"]["d"])
            {
                var arr = i.ToString().Replace("[","").Replace("]","").Split(',');
                int count = 0;
                if (int.TryParse(arr[1],out count ))
                    d["neg"].add(arr[0], count);
            }
            foreach (var i in jd["d"]["pos"]["d"])
            {
                var arr = i.ToString().Replace("[", "").Replace("]", "").Split(',');
                int count = 0;
                if (int.TryParse(arr[1], out count))
                    d["pos"].add(arr[0], count);
            }
            total = Convert.ToDouble (jd["total"].ToString());
            return d;
        }
        /// <summary>
        /// 情感分析正负面值计算
        /// </summary>
        /// <param name="x">词频列表</param>
        /// <param name="d"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        protected static Tuple<string, double> Classify(List<string> x,
            Dictionary<string, AddOneProb> d, double total)
        {
            Dictionary<string, double> temp = new Dictionary<string, double>();
            foreach (var k in d.Keys)
            {
                temp.Add(k, Math.Log(0.5));
                foreach (var word in x)
                {
                    Console.WriteLine(k+" : "+word + " : " + d[k].freq(word));
                    temp[k] += Math.Log(d[k].freq(word));
                }
                Console.ReadKey();
            }
            string ret = "";
            double prob = 0;
            double now;
            foreach (var k in d.Keys)
            {
                now = 0;
                foreach (var otherk in d.Keys)
                {
                    try
                    {
                        now += Math.Exp(temp[otherk] - temp[k]);
                    }
                    catch (Exception)
                    {
                        now = 10000;
                    }
                }
                now = 1 / now;
                if (now > prob)
                {
                    ret = k;
                    prob = now;
                }
            }
            return new Tuple<string, double>(ret, prob);
        }
        public static double classify_(string sent, Dictionary<string, AddOneProb> d,
            double total, string stopwordFilepath)
        {
            Jieba jiebaword = new Jieba();
            jiebaword.doc = sent;
            jiebaword.stopwords = ReadTxtToEnd(stopwordFilepath);
            var retprob = Classify(jiebaword.JiebaCut(), d, total);
            if (retprob.Item1 == "pos")
                return retprob.Item2;
            else
                return 1 - retprob.Item2;
        }
        /// <summary>
        /// 读取stopwords.txt文件
        /// </summary>
        /// <returns>返回字符串，stopword不拆分</returns>
        public static string ReadTxtToEnd(string Filepath)
        {
            using (var sr = new StreamReader(Filepath, Encoding.Default))
                return sr.ReadToEnd();
        }
        /// <summary>
        /// 训练现有样本,返回d和total
        /// </summary>
        /// <param name="negFilePath">负面词训练集路径</param>
        /// <param name="negWords">负面词库文本</param>
        /// <param name="posFilePath">正面词训练集路径</param>
        /// <param name="posWords">正面词库文本</param>
        /// <param name="d">存储正面和负面词集的字典</param>
        /// <param name="total">总数</param>
        /// <param name="stopwordFilepath">排除词路径</param>
        public static void Train_data(string negFilePath, string negWords, string posFilePath, string posWords,
             ref Dictionary<string, AddOneProb> d, ref double total, string stopwordFilepath)
        {
            //d = new Dictionary<string, AddOneProb>() { { "pos", new AddOneProb() }, { "neg", new AddOneProb() } };
            string negfile = "", posfile = "";
            using (var sr1 = new StreamReader(negFilePath, Encoding.Default))
                negfile = sr1.ReadToEnd();
            using (var sr2 = new StreamReader(posFilePath, Encoding.Default))
                posfile = sr2.ReadToEnd();
            string stopwords = ReadTxtToEnd(stopwordFilepath);
            List<Tuple<List<string>, string>> data = new List<Tuple<List<string>, string>>();
            var sent_cut = new Jieba();
            sent_cut.NegWords = negWords;
            sent_cut.PosWords = posWords;
            foreach (var sent in posfile.Replace("\r", "").Split('\n'))
            {
                sent_cut.doc = sent;
                sent_cut.stopwords = stopwords;
                //<Question>why not work
                //var data_pos = new Tuple<List<string>,string>();
                //</Question>
                    data.Add(new Tuple<List<string>, string>(sent_cut.handle_sentiment(), "pos"));
            }
            Console.WriteLine("正面词库导入完毕");
            foreach (var sent in negfile.Replace("\r", "").Split('\n'))
            {
                sent_cut.doc = sent;
                sent_cut.stopwords = stopwords;
                data.Add(new Tuple<List<string>, string>(sent_cut.handle_sentiment(false), "neg"));
            }
            Console.WriteLine("负面词库导入完毕");

            foreach (var d_ in data)
            {
                var c = d_.Item2.ToString();
                if (d_.Item1 == null)
                    continue;
                else
                {
                    foreach (var word in d_.Item1)
                        d[c].add(word, 1);
                }
            }
            ///<question>字典所有值求和
            //d.Sum(x=>d[x])
            ///</question>
            total = 0;
            foreach (var value in d.Values)
            {
                total += value.total;
            }
        }

    }
}
