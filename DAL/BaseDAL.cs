using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace DAL
{
    public class BaseDAL<T> where T : class, new()
    {
        #region 变量

        /// <summary>
        /// 是否多操作，如果一次请求有多个数据库操作则为true否则为false
        /// 当有多操作时则orm对象不使用自动释放的否则使用自动释放的对象
        /// </summary>
        private bool _ism = false;

        /// <summary>
        /// 数据库操作的对象
        /// </summary>
        private SqlSugarClient _ssc = null;

        #endregion

        #region 属性

        /// <summary>
        /// 数据库操作的对象，如果是多操作的对象则需要手动释放如使用using,SDB.Dispose() ,SDB.Close()等
        /// </summary>
        public SqlSugarClient ActionDBAccess
        {
            get
            {
                return this._ssc;
            }
        }

        #endregion

        #region 方法[方法是个例子，具体情况可以不用]

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ism">是否多数据库</param>
        public BaseDAL(bool ism = false)
        {
            this._ism = ism;

            if (ism)
            {
                this._ssc = DBConfig.GetIntance();
            }
            else
            {
                this._ssc = DBConfig.DB;
            }
        }

        /// <summary>
        /// 根据传入的实体将数据插入到数据库中
        /// </summary>
        /// <param name="t">类型实体</param>
        /// <returns>插入时受影响的行数</returns>
        public int InsertData(T t)
        {
            return _ssc.Insertable<T>(t).ExecuteCommand();
        }

        /// <summary>
        /// 更新数据到数据库表，实现行锁
        /// </summary>
        /// <param name="t">类型实体</param>
        /// <returns></returns>
        public int UpdateDataWithRowLock(T t)
        {
            return _ssc.Updateable<T>(t).With(SqlWith.RowLock).ExecuteCommand();
        }

        /// <summary>
        /// 更新数据到数据库表，不锁表
        /// </summary>
        /// <param name="t">类型实体</param>
        /// <returns></returns>
        public int UpdateDataWithNoLock(T t)
        {
            return _ssc.Updateable<T>(t).With(SqlWith.NoLock).ExecuteCommand();
        }

        /// <summary>
        /// 根据传入的实体中的条件将数据库中的数据删除
        /// </summary>
        /// <param name="t">类型实体</param>
        /// <returns></returns>
        public int DeleteData(T t)
        {
            return _ssc.Deleteable<T>(t).ExecuteCommand();
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="orderFun">排序的表达式</param>
        /// <param name="ob">排序方式 默认为正序</param>
        /// <param name="PageIndex">页码 从1开始计</param>
        /// <param name="PageSize">页容量</param>
        /// <param name="where">条件</param>
        /// <param name="pars">参数</param>
        /// <returns></returns>
        public ISugarQueryable<T> GetData(Expression<Func<T, object>> orderFun = null, OrderByType ob = OrderByType.Asc, int PageIndex = 0, int PageSize = 0, string where = "1=1", object pars = null)
        {
            ISugarQueryable<T> Tempdb = _ssc.Queryable<T>().Where(where, pars);

            //需要分页
            if (PageIndex > 0)
            {
                int count = Tempdb.Count();
                int Skip = (PageIndex - 1) * PageSize;
                int SumPage = Convert.ToInt32(Math.Ceiling(count * 1.0 / PageSize));//总页数
                bool dxp = PageIndex > (SumPage / 2);
                if (dxp)//页码大于一半用倒序
                {
                    Tempdb.OrderBy(orderFun, (ob == OrderByType.Asc ? OrderByType.Desc : OrderByType.Asc));
                    int mod = count % PageSize;
                    if (PageIndex * PageSize >= count)
                    {
                        Skip = 0; PageSize = mod == 0 ? PageSize : mod;
                    }
                    else
                    {
                        Skip = (SumPage - PageIndex - 1) * PageSize + mod;
                    }
                }
                else
                {
                    Tempdb.OrderBy(orderFun, ob);//升序
                }

                Tempdb.Skip(Skip);
                Tempdb.Take(PageSize);
            }


            return Tempdb;
        }

        #endregion
    }
}


