using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entity
{

    [SugarTable("t_phone_code")]
    public partial class PhoneCodeEntity
    {
        public PhoneCodeEntity()
        {

        }

        /// <summary>
        /// Desc:主键ID
        /// Default:
        /// Nullable:False
        /// </summary>      
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int phoneCodeId { get; set; }

        /// <summary>
        /// Desc:手机号码
        /// Default:-1
        /// Nullable:False
        /// </summary>           
        public string phone { get; set; }

        /// <summary>
        /// Desc:验证码
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string code { get; set; }

        /// <summary>
        /// Desc:创建时间
        /// Default:DateTime.Now
        /// Nullable:False
        /// </summary>           
        public DateTime createDate { get; set; }
    }
}
