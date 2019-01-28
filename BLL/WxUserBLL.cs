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
        /// 创建微信用户
        /// </summary>
        /// <param name="wxUserEntity"></param>
        /// <returns></returns>
        public int CreateWxUser(global::WeChat.WxUserEntity wxUserEntity)
        {
            UserEntity userEntity = new UserEntity()
            {
                account = "",
                address = "",
                birthday = Helper.ConvertHelper.DEFAULT_DATE,
                createDate = DateTime.Now,
                email = "",
                forbidden = false,
                gender = Convert.ToInt32(wxUserEntity.sex),
                grade = 1,
                integral = 1000,
                modifyDate = DateTime.Now,
                name = wxUserEntity.nickname,
                password = "",
                phone = "",
                portrait = wxUserEntity.headimgurl,
                remark = "",
                signature = "",
                state = -1,
                type = -1
            };

            UserEntity user = ActionDal.ActionDBAccess.Insertable(userEntity).ExecuteReturnEntity();

            Entity.WxUserEntity wxUser = new Entity.WxUserEntity()
            {
                avatarUrl = wxUserEntity.headimgurl,
                city = wxUserEntity.city,
                country = wxUserEntity.country,
                createDate = DateTime.Now,
                gender = wxUserEntity.sex,
                modifyDate = DateTime.Now,
                nickName = wxUserEntity.nickname,
                openId = wxUserEntity.openid,
                province = wxUserEntity.province,
                unionId = wxUserEntity.unionId ?? "",
                userId = user.userId
            };
            int rows = ActionDal.ActionDBAccess.Insertable(wxUser).ExecuteCommand();
            
            return rows;
        }
    }
}
