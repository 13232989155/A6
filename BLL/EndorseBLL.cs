using Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL
{
    public class EndorseBLL : Base.BaseBLL<EndorseEntity>
    {

        /// <summary>
        /// 创建点赞记录 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="type"></param>
        /// <param name="objId"></param>
        /// <returns></returns>
        public int Create(int userId, int type, int objId)
        {
            EndorseEntity endorseEntity = new EndorseEntity()
            {
                createDate = DateTime.Now,
                isDel = false,
                modifyDate = DateTime.Now,
                objId = objId,
                type = type,
                userId = userId
            };

            return ActionDal.ActionDBAccess.Insertable(endorseEntity).ExecuteCommand();
        }

        /// <summary>
        /// 返回查找记录
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="type"></param>
        /// <param name="objId"></param>
        /// <returns></returns>
        public EndorseEntity GetUserIdAndTypeAndObjId(int userId, int type, int objId)
        {
            return ActionDal.ActionDBAccess.Queryable<EndorseEntity>().Where(it => it.userId == userId && it.type == type && it.objId == objId).First();
        }

        /// <summary>
        /// 根据ID数组获取列表
        /// </summary>
        /// <param name="type"></param>
        /// <param name="objIdInts"></param>
        /// <returns></returns>
        public List<EndorseEntity> ListByTypeAndObjIdInts(int type, int[] objIdInts, bool isDel = false)
        {
            return ActionDal.ActionDBAccess.Queryable<EndorseEntity>()
                    .WhereIF( !isDel, it => it.isDel == false)
                    .Where(it => it.type == type)
                    .In(it => it.objId, objIdInts)
                    .OrderBy( it => it.createDate, SqlSugar.OrderByType.Desc)
                    .ToList();
        }

        /// <summary>
        /// 获取对象点赞列表
        /// </summary>
        /// <param name="type"></param>
        /// <param name="objId"></param>
        /// <param name="isDel"></param>
        /// <returns></returns>
        public List<EndorseEntity> ListByTypeAndObjId(int type, int objId, bool isDel = false)
        {
            return ActionDal.ActionDBAccess.Queryable<EndorseEntity>()
                   .WhereIF(!isDel, it => it.isDel == false)
                   .Where(it => it.type == type && it.objId == objId)
                   .OrderBy(it => it.createDate, SqlSugar.OrderByType.Desc)
                   .ToList();
        }
    }
}
