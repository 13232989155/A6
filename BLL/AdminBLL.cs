using Entity;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;
using X.PagedList;

namespace BLL
{
    public class AdminBLL : Base.BaseBLL<AdminEntity>
    {
        /// <summary>
        /// 根据账号和密码获取实体
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public AdminEntity GetAccountAndPassword(string account, string password)
        {
            return ActionDal.ActionDBAccess.Queryable<AdminEntity>().Where(it => it.account == account && it.password == password).First();
        }

        public AdminEntity GetById(int adminId)
        {
            return ActionDal.ActionDBAccess.Queryable<AdminEntity>().Where(it => it.adminId == adminId).First();
        }

        /// <summary>
        /// 分页列表
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchString"></param>
        /// <returns></returns>
        public IPagedList<AdminEntity> AdminPageList(int pageNumber, int pageSize, string searchString)
        {

            IPagedList<AdminEntity> adminEntities = ActionDal.ActionDBAccess.Queryable<AdminEntity>()
                                                   .WhereIF(!string.IsNullOrWhiteSpace(searchString), it => it.account.Contains(searchString)
                                                      || it.email.Contains(searchString)
                                                      || it.name.Contains(searchString)
                                                      || it.phone.Contains(searchString)
                                                      || SqlFunc.ToString(it.adminId).Contains(searchString))
                                                   .OrderBy(it => it.createDate, OrderByType.Desc)
                                                   .ToList()
                                                   .ToPagedList(pageNumber, pageSize);
            return adminEntities;
        }
    }
}
