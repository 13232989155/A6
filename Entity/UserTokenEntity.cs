using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entity
{
    ///<summary>
    ///用户会话秘钥
    ///</summary>
    [SugarTable("t_user_token")]
    public partial class UserTokenEntity
    {
        public UserTokenEntity()
        {


        }
        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>     
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int userTokenId { get; set; }

        /// <summary>
        /// Desc:token
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string token { get; set; }

        /// <summary>
        /// Desc:用户ID
        /// Default:-1
        /// Nullable:False
        /// </summary>           
        public int userId { get; set; }

        /// <summary>
        /// Desc:类型: 
        /// Default:-1
        /// Nullable:False
        /// </summary>           
        public int type { get; set; }

        /// <summary>
        /// Desc:登录时间
        /// Default:DateTime.Now
        /// Nullable:False
        /// </summary>           
        public DateTime createDate { get; set; }
    }
}
