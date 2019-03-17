using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Admin.Models
{
    public class EChartsResult
    {
        /// <summary>
        /// 线形图
        /// </summary>
        public class LineResult
        {
            public List<string> xAxis { set; get; }

            public List<string> yAxis { set; get; }
        }

    }
}
