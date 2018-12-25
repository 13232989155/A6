using Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL
{
    public class UserBLL : Base.BaseBLL<UserEntity>
    {



        public List<UserEntity> List()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 根据账号获取实体
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public UserEntity GetByAccount(string account)
        {
            return ActionDal.ActionDBAccess.Queryable<UserEntity>().Where(it => it.account == account).First();
        }

        /// <summary>
        /// 根据ID返回实体
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public UserEntity GetById(int userId)
        {
            return ActionDal.ActionDBAccess.Queryable<UserEntity>().Where(it => it.userId == userId).First();
        }

        /// <summary>
        /// 注册用户
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public int Register(string account, string password)
        {
            UserEntity userEntity = new UserEntity()
            {
                account = account,
                address = "",
                birthday = QiNiu.ConvertHelper.DEFAULT_DATE,
                createDate = DateTime.Now,
                email = "",
                forbidden = false,
                gender = -1,
                grade = 1,
                integral = 10000,
                modifyDate = DateTime.Now,
                name = account,
                password = QiNiu.DataEncrypt.DataMd5(password),
                phone = "",
                portrait = "",
                remark = "",
                signature = "",
                state = -1,
                type = -1
            };

            return ActionDal.ActionDBAccess.Insertable(userEntity).ExecuteCommand();

        }

        /// <summary>
        /// 根据账号密码返回实体
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public UserEntity GetByAccountAndPassword(string account, string password)
        {
            password = QiNiu.DataEncrypt.DataMd5(password);

            return ActionDal.ActionDBAccess.Queryable<UserEntity>().Where(it => it.account == account && it.password == password && it.forbidden == false).First();
        }
    }
}
