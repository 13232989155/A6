using System;
using System.Collections.Generic;
using System.Text;

namespace Entity
{
    /// <summary>
    /// 圈子返回的说说和案例辅助排序
    /// </summary>
    public partial class CircleResultHelper
    {
        /// <summary>
        /// 
        /// </summary>
        public CircleResultHelper()
        {

        }


        public int id { set; get; }

        public DateTime createDate { set; get; }

        public int type { set; get; }

        public CaseEntity caseEntity { set; get; }

        public ShareEntity shareEntity { set; get; }
    }
}
