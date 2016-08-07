using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sentiment_Console
{
    class BaseProb
    {
        public Dictionary<string, double> d = new Dictionary<string, double>();
        public double total = 0.0;
        public int none = 0;
        public bool exists(string key) { return d.Keys.Contains(key); }
        public double getsum() { return total; }
        public void get(string key, out bool para1, out double para2)
        {
            if (!exists(key))
            {
                para1 = false; para2 = none;
            }
            else
            {
                para1 = true;para2 = d[key];
            }
        }
        public double freq(string key)
        {
            bool para1;
            double para2;
            get(key, out para1, out para2);
            return para2 / total;
        }
        //def samples(self):
        //    return self.d.keys()
    }
    class AddOneProb : BaseProb
    {
        //public Dictionary<string, double> d = new Dictionary<string, double>();
        //public double total = 0.0;
        //public int none = 1;
        public void add(string key, int value)
        {
            total += value;
            if (!exists(key))
            {
                d.Add(key, 1);
                total += 1;
            }
            d[key] += value;
        }

        public void DPrint()
        {
            Console.WriteLine(d.Count);
            Console.ReadKey();
            foreach (var key in d.Keys)
            {
                Console.WriteLine(key+" : "+d[key].ToString());
            }
        }
    }
    
}
