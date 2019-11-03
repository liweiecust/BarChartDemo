using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp4
{
    public class DataPoint
    {
        public DataPoint(string name, int num)
        {
            Name = name;
            Value = num;
        }
        public string Name { get; set; }
        public int Value { get; set; }
    }
}
