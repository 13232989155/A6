using Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL
{
    public class ReadBLL : Base.BaseBLL<ReadEntity>
    {
        /// <summary>
        /// 创建阅读记录
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="type"></param>
        /// <param name="objId"></param>
        /// <returns></returns>
        public int Create(int userId, int type, int objId)
        {
            ReadEntity readEntity = new ReadEntity()
            {
                createDate = DateTime.Now,
                objId = objId,
                type = type,
                userId = userId
            };

            return ActionDal.ActionDBAccess.Insertable(readEntity).ExecuteCommand();
        }
    }
}
