using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entity
{
    ///<summary>
    ///课程订单
    ///</summary>
    [SugarTable("t_course_order")]
    public partial class CourseOrderEntity
    {
        public CourseOrderEntity()
        {


        }
        /// <summary>
        /// Desc:主键ID
        /// Default:
        /// Nullable:False
        /// </summary>    
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int courseOrderId { get; set; }

        /// <summary>
        /// Desc:用户ID
        /// Default:-1
        /// Nullable:False
        /// </summary>           
        public int userId { get; set; }

        /// <summary>
        /// Desc:课程ID
        /// Default:-1
        /// Nullable:False
        /// </summary>           
        public int courseId { get; set; }

        /// <summary>
        /// Desc:状态:未付款: 1,已付款: 2
        /// Default:-1
        /// Nullable:False
        /// </summary>           
        public int state { get; set; }

        /// <summary>
        /// Desc:订单号
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string orderNo { get; set; }

        /// <summary>
        /// Desc:订单总价
        /// Default:-1
        /// Nullable:False
        /// </summary>           
        public decimal orderTotal { get; set; }

        /// <summary>
        /// Desc:实付
        /// Default:-1
        /// Nullable:False
        /// </summary>           
        public decimal realTotal { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string payChannel { get; set; }

        /// <summary>
        /// Desc:支付号
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string payNo { get; set; }

        /// <summary>
        /// Desc:创建时间
        /// Default:DateTime.Now
        /// Nullable:False
        /// </summary>           
        public DateTime createDate { get; set; }

        /// <summary>
        /// Desc:修改shijian 
        /// Default:DateTime.Now
        /// Nullable:False
        /// </summary>           
        public DateTime modifyDate { get; set; }

        /// <summary>
        /// Desc:支付时间
        /// Default:9999-12-31
        /// Nullable:False
        /// </summary>           
        public DateTime payDate { get; set; }

    }
}

