using Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL
{
    public class PhoneCodeBLL:Base.BaseBLL<PhoneCodeEntity>
    {
        /// <summary>
        /// 创建验证码
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public int Create(string phone, string code)
        {
            PhoneCodeEntity phoneCodeEntity = new PhoneCodeEntity()
            {
                code = code,
                createDate = DateTime.Now,
                phone = phone
            };

            return ActionDal.ActionDBAccess.Insertable(phoneCodeEntity).ExecuteCommand();
        }

        /// <summary>
        /// 删除验证码
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public int Delete(string phone)
        {
            List<PhoneCodeEntity> phoneCodeEntities = ActionDal.ActionDBAccess.Queryable<PhoneCodeEntity>().Where(it => it.phone == phone).ToList();
            int rows = 0;

            if ( phoneCodeEntities != null)
            {
                rows = ActionDal.ActionDBAccess.Deleteable(phoneCodeEntities).ExecuteCommand();
            }
            return rows;
        }

        /// <summary>
        /// 删除验证码
        /// </summary>
        /// <param name="phoneCodeId"></param>
        /// <returns></returns>
        public int Delete(int phoneCodeId)
        {
            PhoneCodeEntity phoneCodeEntity = ActionDal.ActionDBAccess.Queryable<PhoneCodeEntity>().Where(it => it.phoneCodeId == phoneCodeId).First();

            return ActionDal.ActionDBAccess.Deleteable(phoneCodeEntity).ExecuteCommand();
        }

        /// <summary>
        /// 更新验证码
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public int Update(string phone, string code)
        {
            PhoneCodeEntity phoneCodeEntity = ActionDal.ActionDBAccess.Queryable<PhoneCodeEntity>().Where(it => it.phone == phone).First();

            phoneCodeEntity.code = code;
            phoneCodeEntity.createDate = DateTime.Now;
            return ActionDal.ActionDBAccess.Updateable(phoneCodeEntity).ExecuteCommand();
        }

        /// <summary>
        /// 查找
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public PhoneCodeEntity GetByPhoneAndCode(string phone, string code)
        {
            return ActionDal.ActionDBAccess.Queryable<PhoneCodeEntity>().Where(it => it.phone == phone && it.code == code).First();
        }
    }
}
