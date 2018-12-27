using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entity
{
    ///<summary>
    ///用户
    ///</summary>
    [SugarTable("t_user")]
    public partial class UserEntity
    {
        public UserEntity()
        {


        }

        /// <summary>
        /// 用户一般资料
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="name"></param>
        /// <param name="gender"></param>
        /// <param name="birthday"></param>
        /// <param name="portrait"></param>
        /// <param name="signature"></param>
        /// <param name="attention"></param>
        public UserEntity(int userId, string name, int gender, DateTime birthday, string portrait, string signature, bool attention)
        {
            this.userId = userId;
            this.name = name;
            this.gender = gender;
            this.birthday = birthday;
            this.portrait = portrait;
            this.signature = signature;
            this.attention = attention;
        }

        /// <summary>
        /// Desc:ID
        /// Default:
        /// Nullable:False
        /// </summary>         
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int userId { get; set; }

        /// <summary>
        /// Desc:账号
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string account { get; set; }

        /// <summary>
        /// Desc:密码
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string password { get; set; }

        /// <summary>
        /// Desc:名称
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string name { get; set; }

        /// <summary>
        /// Desc:性别
        /// Default:
        /// Nullable:False
        /// </summary>           
        public int gender { get; set; }

        /// <summary>
        /// Desc:生日
        /// Default:
        /// Nullable:False
        /// </summary>           
        public DateTime birthday { get; set; }

        /// <summary>
        /// Desc:头像
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string portrait { get; set; }

        /// <summary>
        /// Desc:手机号
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string phone { get; set; }

        /// <summary>
        /// Desc:邮箱
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string email { get; set; }

        /// <summary>
        /// Desc:常住地址
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string address { get; set; }

        /// <summary>
        /// Desc:个性签名
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string signature { get; set; }

        /// <summary>
        /// Desc:积分
        /// Default:
        /// Nullable:False
        /// </summary>           
        public int integral { get; set; }

        /// <summary>
        /// Desc:账号类型
        /// Default:
        /// Nullable:False
        /// </summary>           
        public int type { get; set; }

        /// <summary>
        /// Desc:等级
        /// Default:
        /// Nullable:False
        /// </summary>           
        public int grade { get; set; }

        /// <summary>
        /// Desc:状态:
        /// Default:
        /// Nullable:False
        /// </summary>           
        public int state { get; set; }

        /// <summary>
        /// Desc:是否禁用
        /// Default:
        /// Nullable:False
        /// </summary>           
        public bool forbidden { get; set; }

        /// <summary>
        /// Desc:创建时间
        /// Default:
        /// Nullable:False
        /// </summary>           
        public DateTime createDate { get; set; }

        /// <summary>
        /// Desc:修改时间
        /// Default:
        /// Nullable:False
        /// </summary>           
        public DateTime modifyDate { get; set; }

        /// <summary>
        /// Desc:备注
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string remark { get; set; }

        /// <summary>
        /// Desc:关注
        /// Default:
        /// Nullable:False
        /// </summary>          
        [SugarColumn(IsIgnore = true)]
        public bool attention { get; set; }

    }
}
