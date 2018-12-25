using Entity;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL
{
    public class ShareBLL : Base.BaseBLL<ShareEntity>
    {
        /// <summary>
        /// 创建说说
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="contents"></param>
        /// <param name="img"></param>
        /// <param name="shareTypeId"></param>
        /// <param name="shareTopicId"></param>
        /// <returns></returns>
        public int Create(int userId, string contents, string img, int shareTypeId, int shareTopicId)
        {
            ShareEntity shareEntity = new ShareEntity()
            {
                contents = contents,
                createDate = DateTime.Now,
                img = img,
                isDel = false,
                modifyDate = DateTime.Now,
                shareTopicId = shareTopicId,
                shareTypeId = shareTypeId,
                userId = userId,
                integral = 0
            };

            return ActionDal.ActionDBAccess.Insertable(shareEntity).ExecuteCommand();
        }

        /// <summary>
        /// 根据ID返回实体
        /// </summary>
        /// <param name="shareId"></param>
        /// <returns></returns>
        public ShareEntity GetById(int shareId)
        {
            return ActionDal.ActionDBAccess.Queryable<ShareEntity>().Where(it => it.shareId == shareId).First();
        }

        /// <summary>
        /// 获取全部说说
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ShareEntity> List()
        {
            return ActionDal.ActionDBAccess.Queryable<ShareEntity>().OrderBy( it => it.createDate, SqlSugar.OrderByType.Desc).ToList();
        }

        /// <summary>
        /// 根据说说类型或话题返回说说列表
        /// </summary>
        /// <param name="shareTypeId"></param>
        /// <param name="shareTypeId"></param>
        /// <param name="userId"></param>
        /// <param name="isDel">是否包含已删除的说说</param>
        /// <param name="forbidden">是否包含禁用的用户的说说</param>
        /// <returns></returns>
        public IEnumerable<ShareEntity> List(int shareTypeId = -1, int shareTopicId = -1, int userId = -1, bool isDel = false, bool forbidden = false)
        {
            return ActionDal.ActionDBAccess.Queryable<ShareEntity, UserEntity>( ( s, u) => new object[]
                    {
                         JoinType.Inner,s.userId == u.userId  
                    })
                    .WhereIF( userId > 10000, (s, u) => s.userId == userId)
                    .WhereIF( !forbidden, ( s, u) => u.forbidden == false )
                    .WhereIF( !isDel, (s, u) => s.isDel == false)
                    .WhereIF(shareTypeId > 10000, (s, u) => s.shareTypeId == shareTypeId)
                    .WhereIF( shareTopicId > 10000, (s, u) => s.shareTopicId == shareTopicId)
                    .OrderBy(s => s.createDate, SqlSugar.OrderByType.Desc)
                    .Select( (s, u) => new ShareEntity
                    {
                        contents = s.contents, 
                        createDate = s.createDate,
                        shareId = s.shareId,
                        img = s.img,
                        integral = s.integral,
                        isDel = s.isDel,
                        modifyDate = s.modifyDate,
                        name = u.name,
                        portrait = u.portrait,
                        shareTopicId = s.shareTopicId,
                        shareTypeId = s.shareTypeId,
                        userId = s.userId
                    })
                    .ToList();
        }
    }
}
