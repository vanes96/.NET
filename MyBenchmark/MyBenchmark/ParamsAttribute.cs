using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBenchmark
{
    public class ParamsAttribute : Attribute
    {
        public object[] Params { get; set; }

        public ParamsAttribute(params object[] args)
        {
            this.Params = args;
        }
    }
}