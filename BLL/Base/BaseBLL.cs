using DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Base
{
    public class BaseBLL<T> where T : class, new()
    {
        /// <summary>
        /// 数据层实例
        /// </summary>
        private BaseDAL<T> _dal = new BaseDAL<T>();

        /// <summary>
        /// 数据层实例
        /// </summary>
        public BaseDAL<T> ActionDal
        {
            get
            {
                return _dal;
            }
        }

        /// <summary>
        /// 插入数据,返回受影响的行数
        /// </summary>
        /// <param name="t">实体类</param>
        /// <returns></returns>
        public int InsertData(T t)
        {
            int num = this.ActionDal.InsertData(t);

            return num;
        }

        /// <summary>
        /// 插入数据，并返回自增列
        /// </summary>
        /// <param name="t">实体类</param>
        /// <returns></returns>
        public int InsertDataReturnIdentityKey(T t)
        {
            int keyValue = this.ActionDal.ActionDBAccess.Insertable(t).ExecuteReturnIdentity();
            return keyValue;
        }

        ///// <summary>
        ///// 更新数据[锁行]
        ///// </summary>
        ///// <param name="t"></param>
        ///// <returns></returns>
        //public int UpdateDataWithRowLock(T t)
        //{
        //    int num = this.SuncerDal.UpdateDataWithRowLock(t);

        //    return num;
        //}

        ///// <summary>
        ///// 更新数据[不锁行]
        ///// </summary>
        ///// <param name="t"></param>
        ///// <returns></returns>
        //public int UpdateDataWithNoLock(T t)
        //{
        //    int num = this.SuncerDal.UpdateDataWithNoLock(t);

        //    return num;
        //}

        /// <summary>
        /// 删除数据[不锁行]
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public int DeleteData(T t)
        {
            int num = this.ActionDal.DeleteData(t);

            return num;
        }


    }
}