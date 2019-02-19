using Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL
{
    public class AdminTokenBLL : Base.BaseBLL<AdminTokenEntity>
    {
        /// <summary>
        /// 根据token获取实体
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public AdminTokenEntity GetByToken(string token)
        {
            return ActionDal.ActionDBAccess.Queryable<AdminTokenEntity>().Where(it => it.token == token).First();
        }

        /// <summary>
        /// 更新时间
        /// </summary>
        /// <param name="adminTokenEntity"></param>
        public void UpdateTime(AdminTokenEntity adminTokenEntity)
        {
            adminTokenEntity.createDate = DateTime.Now;
            ActionDal.ActionDBAccess.Updateable(adminTokenEntity).ExecuteCommand();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="adminId"></param>
        /// <returns></returns>
        public AdminTokenEntity GetByAdminId(int adminId)
        {
            return ActionDal.ActionDBAccess.Queryable<AdminTokenEntity>().Where(it => it.adminId == adminId).First();
        }

        /// <summary>
        /// 创建token
        /// </summary>
        /// <param name="adminId"></param>
        /// <returns></returns>
        public AdminTokenEntity Create(int adminId)
        {
            AdminTokenEntity adminTokenEntity = new AdminTokenEntity()
            {
                adminId = adminId,
                createDate = DateTime.Now,
                token = Helper.DataEncrypt.DataMd5(Guid.NewGuid().ToString())
            };

            return ActionDal.ActionDBAccess.Insertable(adminTokenEntity).ExecuteReturnEntity();
        }

        /// <summary>
        /// 更新token
        /// </summary>
        /// <param name="adminTokenEntity"></param>
        /// <returns></returns>
        public AdminTokenEntity Update(AdminTokenEntity adminTokenEntity)
        {
            adminTokenEntity.createDate = DateTime.Now;
            adminTokenEntity.token = Helper.DataEncrypt.DataMd5(Guid.NewGuid().ToString());

            int rows = ActionDal.ActionDBAccess.Updateable(adminTokenEntity).ExecuteCommand();

            return ActionDal.ActionDBAccess.Queryable<AdminTokenEntity>().Where(it => it.adminTokenId == adminTokenEntity.adminTokenId).First();
        }
    }
}
