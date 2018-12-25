using Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL
{
    public class UserTokenBLL : Base.BaseBLL<UserTokenEntity>
    {
        /// <summary>
        /// 根据用户ID返回实体
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public UserTokenEntity GetByUserId(int userId)
        {
            return ActionDal.ActionDBAccess.Queryable<UserTokenEntity>().Where(it => it.userId == userId).First();
        }

        /// <summary>
        /// 根据用户ID创建token
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public UserTokenEntity Create(int userId)
        {
            UserTokenEntity userTokenEntity = new UserTokenEntity()
            {
                createDate = DateTime.Now,
                token = QiNiu.DataEncrypt.DataMd5( Guid.NewGuid().ToString()),
                type = -1,
                userId = userId
            };

            return ActionDal.ActionDBAccess.Insertable(userTokenEntity).ExecuteReturnEntity();
        }

        /// <summary>
        /// 更新token
        /// </summary>
        /// <param name="userTokenEntity"></param>
        /// <returns></returns>
        public UserTokenEntity Update(UserTokenEntity userTokenEntity)
        {
            userTokenEntity.createDate = DateTime.Now;
            userTokenEntity.token = QiNiu.DataEncrypt.DataMd5(Guid.NewGuid().ToString());

            int rows = ActionDal.ActionDBAccess.Updateable(userTokenEntity).ExecuteCommand();

            return ActionDal.ActionDBAccess.Queryable<UserTokenEntity>().Where(it => it.userTokenId == userTokenEntity.userTokenId).First();
        }

        /// <summary>
        /// 根据token返回实体
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public UserTokenEntity GetByToken(string token)
        {
            return ActionDal.ActionDBAccess.Queryable<UserTokenEntity>().Where(it => it.token == token).First();
        }

        /// <summary>
        /// 更新时间
        /// </summary>
        /// <param name="userTokenEntity"></param>
        public void UpdateTime(UserTokenEntity userTokenEntity)
        {
            userTokenEntity.createDate = DateTime.Now;

            int rows = ActionDal.ActionDBAccess.Updateable(userTokenEntity).ExecuteCommand();
        }
    }
}
