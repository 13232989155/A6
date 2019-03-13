using Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL
{
    public class WxUserBLL : Base.BaseBLL<WxUserEntity>
    {


        /// <summary>
        /// 根据openid返回实体
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        public WxUserEntity GetByOpenId(string openid)
        {
            return ActionDal.ActionDBAccess.Queryable<WxUserEntity>().Where(it => it.openId == openid).First();
        }

        /// <summary>
        /// 根据用户id获取实体
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public WxUserEntity GetByUserId(int userId)
        {
            return ActionDal.ActionDBAccess.Queryable<WxUserEntity>().Where(it => it.userId == userId).First();
        }
    }
}
