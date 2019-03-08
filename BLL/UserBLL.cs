using Entity;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace BLL
{
    public class UserBLL : Base.BaseBLL<UserEntity>
    {


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
        /// 分页列表
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchString"></param>
        /// <returns></returns>
        public IPagedList<UserEntity> AdminPageList(int pageNumber, int pageSize, string searchString)
        {
            IPagedList<UserEntity> userEntities = ActionDal.ActionDBAccess.Queryable<UserEntity>()
                                                    .WhereIF(!string.IsNullOrWhiteSpace(searchString), it => it.account.Contains(searchString)
                                                       || it.address.Contains(searchString)
                                                       || it.email.Contains(searchString)
                                                       || it.name.Contains(searchString)
                                                       || it.signature.Contains(searchString)
                                                       || SqlFunc.ToString(it.userId).Contains(searchString))
                                                    .OrderBy(it => it.createDate, OrderByType.Desc)
                                                    .ToList()
                                                    .ToPagedList(pageNumber, pageSize);
            return userEntities;
        }

        /// <summary>
        /// 获取全部用户
        /// </summary>
        /// <returns></returns>
        public List<UserEntity> List()
        {
            return ActionDal.ActionDBAccess.Queryable<UserEntity>().ToList();
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
                portrait = "http://image.geekann.com/0.png",
                remark = "",
                signature = "",
                state = -1,
                type = -1
            };

            return ActionDal.ActionDBAccess.Insertable(userEntity).ExecuteCommand();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public UserEntity GetByPhoneAndPassword(string phone, string password)
        {
            return ActionDal.ActionDBAccess.Queryable<UserEntity>().Where(it => it.phone == phone && it.password == password).First();
        }
    }
}
