using Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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
        public async Task<UserEntity> GetByAccount(string account)
        {
            return await ActionDal.ActionDBAccess.Queryable<UserEntity>().Where(it => it.account == account).FirstAsync();
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
        public async Task<int> Register(string account, string password)
        {
            UserEntity userEntity = new UserEntity()
            {
                account = account,
                address = "",
                birthday = Helper.ConvertHelper.DEFAULT_DATE,
                createDate = DateTime.Now,
                email = "",
                forbidden = false,
                gender = -1,
                grade = 1,
                integral = 10000,
                modifyDate = DateTime.Now,
                name = account,
                password = Helper.DataEncrypt.DataMd5(password),
                phone = "",
                portrait = "",
                remark = "",
                signature = "",
                state = -1,
                type = -1
            };

            return await ActionDal.ActionDBAccess.Insertable(userEntity).ExecuteCommandAsync();

        }

        /// <summary>
        /// 根据账号密码返回实体
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public UserEntity GetByAccountAndPassword(string account, string password)
        {
            password = Helper.DataEncrypt.DataMd5(password);

            return ActionDal.ActionDBAccess.Queryable<UserEntity>().Where(it => it.account == account && it.password == password && it.forbidden == false).First();
        }

        /// <summary>
        /// 根据手机号码获取实体
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public UserEntity GetByPhone(string phone)
        {
            return ActionDal.ActionDBAccess.Queryable<UserEntity>().Where(it => it.phone == phone && it.forbidden == false).First();
        }

        /// <summary>
        /// 根据手机号码创建账号
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public int CreateToPhone(string phone)
        {
            UserEntity userEntity = new UserEntity()
            {
                account = "",
                address = "",
                birthday = Helper.ConvertHelper.DEFAULT_DATE,
                createDate = DateTime.Now,
                email = "",
                forbidden = false,
                gender = -1,
                grade = 1,
                integral = 10000,
                modifyDate = DateTime.Now,
                name = phone,
                password = "",
                phone = phone,
                portrait = "",
                remark = "",
                signature = "",
                state = -1,
                type = -1
            };

            return ActionDal.ActionDBAccess.Insertable(userEntity).ExecuteCommand();
        }
    }
}
