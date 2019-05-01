using Entity;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;


namespace BLL
{
    public class CourseOrderBLL : Base.BaseBLL<CourseOrderEntity>
    {
        /// <summary>
        /// 根据订单号返回实体
        /// </summary>
        /// <param name="orderNO"></param>
        /// <returns></returns>
        public CourseOrderEntity GetByOrderNO(string orderNO)
        {
            return ActionDal.ActionDBAccess.Queryable<CourseOrderEntity>().Where(it => it.orderNo == orderNO).First();
        }

        /// <summary>
        /// 查询用户已购买订单
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public CourseOrderEntity GetByCourseAndUserId(int courseId, int userId)
        {
            return ActionDal.ActionDBAccess.Queryable<CourseOrderEntity>().Where(it => it.courseId == courseId && it.userId == userId && it.state == 2).First();
        }

        /// <summary>
        /// 已购买的课程
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<CourseEntity> ListByUserId(int userId)
        {
            List<CourseEntity> courseEntities = new List<CourseEntity>();

            courseEntities = ActionDal.ActionDBAccess.Queryable<CourseOrderEntity, CourseEntity>((co, c) => new object[]
                               {
                                    JoinType.Inner, co.courseId == c.courseId && co.userId == userId && co.state == 2 
                               })
                            .Select((co, c) => new CourseEntity
                            {
                                courseId = c.courseId,
                                courseTypeId = c.courseTypeId,
                                coverImage = c.coverImage,
                                createDate = co.payDate,
                                name = c.name,
                                price = c.price,
                                teacherId = c.teacherId
                            })
                            .OrderBy( co => co.createDate, OrderByType.Desc)
                            .Mapper((it, cache) =>
                            {
                                var teacherEntities = cache.Get(list =>
                                {
                                    var ids = list.Select(i => it.teacherId).ToList();
                                    return ActionDal.ActionDBAccess.Queryable<TeacherEntity>()
                                        .In(ids)
                                        .ToList();
                                });

                                it.teacherEntity = teacherEntities.Where(i => i.teacherId == it.teacherId).First();

                            })
                            .ToList();

            return courseEntities;
        }

        /// <summary>
        /// 获取已购买数量
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public int GetCountByCourseId(int courseId)
        {
            int count = 0;
            count = ActionDal.ActionDBAccess.Queryable<CourseOrderEntity>().Where(it => it.state == 2 && it.courseId == courseId).Count();
            return count;
        }
    }
}
