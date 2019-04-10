using Entity;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using X.PagedList;

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
                contents = contents ?? "",
                createDate = DateTime.Now,
                img = img ?? "",
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
        /// 分页列表
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchString"></param>
        /// <returns></returns>
        public IPagedList<ShareEntity> AdminPageList(int pageNumber, int pageSize, string searchString)
        {
            IPagedList<ShareEntity> shareEntities = ActionDal.ActionDBAccess.Queryable<ShareEntity>()
                                                   .WhereIF(!string.IsNullOrWhiteSpace(searchString), it => it.contents.Contains(searchString)
                                                      || SqlFunc.ToString(it.userId).Contains(searchString))
                                                   .OrderBy(it => it.createDate, OrderByType.Desc)
                                                   .ToList()
                                                   .ToPagedList(pageNumber, pageSize);
            return shareEntities;
        }

        /// <summary>
        /// 根据ID返回实体
        /// </summary>
        /// <param name="shareId"></param>
        /// <returns></returns>
        public ShareEntity GetById(int shareId)
        {
            return ActionDal.ActionDBAccess.Queryable<ShareEntity, UserEntity>((s, u) => new object[]
                   {
                         JoinType.Inner,s.userId == u.userId
                   })
                   .Where(s => s.shareId == shareId)
                   .Select((s, u) => new ShareEntity
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
                   .First();
        }

        /// <summary>
        /// 获取全部说说
        /// </summary>
        /// <returns></returns>
        public List<ShareEntity> List()
        {
            return ActionDal.ActionDBAccess.Queryable<ShareEntity>().OrderBy( it => it.createDate, SqlSugar.OrderByType.Desc).ToList();
        }

        /// <summary>
        /// 根据说说类型或话题返回说说列表
        /// </summary>
        /// <param name="shareTypeId"></param>
        /// <param name="shareTypeId"></param>
        /// <param name="userId">要查找的用户id</param>
        /// <param name="isDel">是否包含已删除的说说</param>
        /// <param name="forbidden">是否包含禁用的用户的说说</param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="thisUserId">当前用户id</param>
        /// <returns></returns>
        public List<ShareEntity> List(int shareTypeId = -1, int shareTopicId = -1, int userId = -1, bool isDel = false, bool forbidden = false, int pageNumber = 1, int pageSize = 10, int totalCount = 0, int thisUserId = -1)
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
                     //.Mapper((s, cache) =>
                     //{

                     //    var commentEntities = cache.Get(list =>
                     //      {
                     //          var ids = list.Select(i => s.shareId).ToList();
                     //          return ActionDal.ActionDBAccess.Queryable<CommentEntity>().In(ids).Where(it => it.isDel == false && it.objId == s.shareId && it.type == (int)Entity.TypeEnumEntity.TypeEnum.说说).ToList();
                     //      }); 
                     //    s.commentCount = commentEntities.Where(c => c.objId == s.shareId).Count();

                     //    var endorseEntities = cache.Get(list =>
                     //    {
                     //        var ids = list.Select(i => s.shareId).ToList();
                     //        return ActionDal.ActionDBAccess.Queryable<EndorseEntity>().In(ids).Where(it => it.isDel == false && it.objId == s.shareId && it.type == (int)Entity.TypeEnumEntity.TypeEnum.说说).ToList();
                     //    }); 

                     //    var thisEndorseEntities = endorseEntities.Where(e => e.objId == s.shareId).ToList();
                     //    if (thisUserId > 10000)
                     //    {
                     //        if (thisEndorseEntities.FirstOrDefault(te => te.userId == thisUserId) != null)
                     //        {
                     //            s.isEndorse = true;
                     //        }
                     //    }
                     //    s.endorseCount = thisEndorseEntities.Count();

                     //})
                    .ToPageList(pageNumber, pageSize, ref totalCount);
        }

        /// <summary>
        /// 获取说说总条数
        /// </summary>
        /// <param name="shareTypeId"></param>
        /// <param name="shareTopicId"></param>
        /// <param name="userId"></param>
        /// <param name="isDel"></param>
        /// <param name="forbidden"></param>
        /// <returns></returns>
        public int Count(int shareTypeId = -1, int shareTopicId = -1, int userId = -1, bool isDel = false, bool forbidden = false)
        {
            return ActionDal.ActionDBAccess.Queryable<ShareEntity, UserEntity>((s, u) => new object[]
                  {
                         JoinType.Inner,s.userId == u.userId
                  })
                   .WhereIF(userId > 10000, (s, u) => s.userId == userId)
                   .WhereIF(!forbidden, (s, u) => u.forbidden == false)
                   .WhereIF(!isDel, (s, u) => s.isDel == false)
                   .WhereIF(shareTypeId > 10000, (s, u) => s.shareTypeId == shareTypeId)
                   .WhereIF(shareTopicId > 10000, (s, u) => s.shareTopicId == shareTopicId)
                   .OrderBy(s => s.createDate, SqlSugar.OrderByType.Desc)
                   .Count();
        }

        /// <summary>
        /// 获取说说总条数
        /// </summary>
        /// <param name="userIdInts"></param>
        /// <returns></returns>
        public int CountByUserIdInts(int[] userIdInts)
        {
            return ActionDal.ActionDBAccess.Queryable<ShareEntity, UserEntity>((s, u) => new object[]
                  {
                         JoinType.Inner,s.userId == u.userId
                  })
                  .Where( (s, u) => s.isDel == false && u.forbidden == false)
                  .In( (s, u) => u.userId, userIdInts)
                   .OrderBy(s => s.createDate, SqlSugar.OrderByType.Desc)
                   .Count();
        }
    }
}
