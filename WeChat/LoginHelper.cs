using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace WeChat
{
    /// <summary>
    /// 微信登录
    /// </summary>
    public static class LoginHelper
    {
        /// <summary>
        /// AppID
        /// </summary>
        public static readonly string AppID = "wxc793d8dfec06bf31";

        /// <summary>
        /// AppSecret
        /// </summary>
        public static readonly string AppSecret = "5834ca5c640d3ab421db1dde33005d51";


        /// <summary>
        /// 获取access_token
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static AccessTokenEntity GetAccessToken(string code)
        {
            string url = "https://api.weixin.qq.com/sns/oauth2/access_token?appid=" + AppID + "&secret=" + AppSecret + "&code=" + code + "&grant_type=authorization_code";
            string type = "utf-8";

            GetWXUsersHelper userHelper = new GetWXUsersHelper();
            string urlRes = userHelper.GetUrltoHtml(url, type);

            AccessTokenEntity accessTokenEntity = JsonConvert.DeserializeObject<AccessTokenEntity>(urlRes);

            return accessTokenEntity;
        }

        /// <summary>
        /// 获取refresh_token
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        public static AccessTokenEntity GetRefreshToken( string refreshToken)
        {
            string url = "https://api.weixin.qq.com/sns/oauth2/refresh_token?appid="+AppID+"&grant_type=refresh_token&refresh_token=" + refreshToken;
            string type = "utf-8";

            GetWXUsersHelper userHelper = new GetWXUsersHelper();
            string urlRes = userHelper.GetUrltoHtml(url, type);
            AccessTokenEntity accessTokenEntity = JsonConvert.DeserializeObject<AccessTokenEntity>(urlRes);

            return accessTokenEntity;
        }

        /// <summary>
        /// 续期
        /// </summary>
        /// <param name="AccessToken"></param>
        /// <param name="openid"></param>
        /// <returns></returns>
        public static AccessTokenEntity SetExpiresIn(AccessTokenEntity accessTokenEntity)
        {
            string url = "https://api.weixin.qq.com/sns/auth?access_token="+ accessTokenEntity.access_token + "&openid=" + accessTokenEntity.openid;
            string type = "utf-8";

            GetWXUsersHelper userHelper = new GetWXUsersHelper();
            string urlRes = userHelper.GetUrltoHtml(url, type);
            AccessTokenEntity accessToken = JsonConvert.DeserializeObject<AccessTokenEntity>(urlRes);

            if (accessToken.errcode == "0")
            {
                return accessTokenEntity;
            }
            else
            {
                return GetRefreshToken(accessTokenEntity.refresh_token);
            }

        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="openId"></param>
        public static WxUserEntity GetWxUser( string accessToken, string openId)
        {
            string url = "https://api.weixin.qq.com/sns/userinfo?access_token="+accessToken+"&openid="+openId;
            string type = "utf-8";

            GetWXUsersHelper userHelper = new GetWXUsersHelper();
            string urlRes = userHelper.GetUrltoHtml(url, type);
            WxUserEntity wxUserEntity = JsonConvert.DeserializeObject<WxUserEntity>(urlRes);

            return wxUserEntity;
        }


    }
}
