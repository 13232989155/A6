using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entity
{

    ///<summary>
    ///管理员会话秘钥
    ///</summary>
    [SugarTable("t_admin_token")]
    public partial class AdminTokenEntity
    {
        public AdminTokenEntity()
        {


        }
        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>     
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int adminTokenId { get; set; }

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
        public int adminId { get; set; }

        /// <summary>
        /// Desc:登录时间
        /// Default:DateTime.Now
        /// Nullable:False
        /// </summary>           
        public DateTime createDate { get; set; }
    }
}
