using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entity
{
    /// <summary>
    /// 管理员信息
    /// </summary>
    [SugarTable("t_admin")]
    public partial class AdminEntity
    {
        public AdminEntity()
        {
        }

        /// <summary>
        /// 管理员ID
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int adminId
        {
            get;
            set;
        }

        /// <summary>
        /// 用户ID
        /// </summary>
        public int userId
        {
            get;
            set;
        }

        /// <summary>
        /// 名称
        /// </summary>
        [Required(ErrorMessage = "不能为空")]
        public string name
        {
            get;
            set;
        }


        /// <summary>
        /// 账户
        /// </summary>
        [Required(ErrorMessage = "不能为空")]
        //[StringLength(18,MinimumLength = 1, ErrorMessage = "长度必须大于1")]
        public string account
        {
            get;
            set;
        }

        /// <summary>
        /// 密码
        /// </summary>
        [Required(ErrorMessage = "不能为空")]
        //[StringLength(40, MinimumLength = 1, ErrorMessage = "长度必须大于1")]
        public string password
        {
            get;
            set;
        }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string email
        {
            get;
            set;
        }

        /// <summary>
        /// 手机号码
        /// </summary>
        public string phone
        {
            get;
            set;
        }

        /// <summary>
        /// 是否禁用
        /// </summary>
        public bool forbidden { get; set; }

        /// <summary>
        /// 最高管理员
        /// </summary>
        public bool administrator { set; get; }

        /// <summary>
        /// 新增日期
        /// </summary>
        public DateTime createDate
        {
            get;
            set;
        }

        /// <summary>
        /// 修改日期
        /// </summary>
        public DateTime modifyDate
        {
            get;
            set;
        }
    }
}
