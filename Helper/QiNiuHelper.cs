using Qiniu.Common;
using Qiniu.Http;
using Qiniu.IO;
using Qiniu.IO.Model;
using Qiniu.Util;

namespace Helper
{
    /// <summary>
    /// 七牛上传文件
    /// </summary>
    public static class QiNiuHelper
    {
        private static string AK = "P2T2_P8KA-6AcNq914uGB3NRwQSjHbG1trTUON3B";

        private static string SK = "K0tXo4vkPfWInUZXm4sIT1zf1YOrai0ZvIOOdVmY";

        private static string Bucket = "image";

        private static string domain = "https://image.geekann.com/";


        /// <summary>
        /// 简单上传-上传字节数据
        /// </summary>
        public static string UploadData(string saveKey, byte[] data)
        {
            string fileUrl = string.Empty;

            // 生成(上传)凭证时需要使用此Mac
            Mac mac = new Mac(AK, SK);
            string bucket = Bucket;
            ZoneID zoneId = ZoneID.CN_South;
            Qiniu.Common.Config.SetZone(zoneId, false);
            // 上传策略，参见 
            // https://developer.qiniu.com/kodo/manual/put-policy
            PutPolicy putPolicy = new PutPolicy();
            // 如果需要设置为"覆盖"上传(如果云端已有同名文件则覆盖)，请使用 SCOPE = "BUCKET:KEY"
            // putPolicy.Scope = bucket + ":" + saveKey;
            putPolicy.Scope = bucket;
            // 上传策略有效期(对应于生成的凭证的有效期)          
            putPolicy.SetExpires(3600);
            // 上传到云端多少天后自动删除该文件，如果不设置（即保持默认默认）则不删除
            //putPolicy.DeleteAfterDays = 1;
            // 生成上传凭证，参见
            // https://developer.qiniu.com/kodo/manual/upload-token            
            string jstr = putPolicy.ToJsonString();
            string token = Auth.CreateUploadToken(mac, jstr);
            FormUploader fu = new FormUploader();
            HttpResult result = fu.UploadData(data, saveKey, token);
            // Console.WriteLine(result);

            fileUrl = domain + saveKey;
            return fileUrl;

        }


    }
}
