using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Api.Models;
using Essensoft.AspNetCore.Payment.WeChatPay;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;

namespace Api
{
    /// <summary>
    /// 
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// 
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            // 引入HttpClient
            services.AddHttpClient();

            // 可在添加依赖注入时设置参数 一般设置 AppId、MchId、Key，其余默认即可.
            // 退款、转账等需要双向证书的API 需要配置 Certificate 参数，将.p12证书文件转成base64串写入即可.
            // 如：
            services.AddWeChatPay(opt =>
            {
                // 此处为 公众号AppId、小程序AppId、企业号corpid、微信开放平台应用AppId
                opt.AppId = WeChat.LoginHelper.AppID;

                // 微信支付商户号
                opt.MchId = WeChat.PayEntity.MchId;

                // API密钥
                opt.Key = WeChat.PayEntity.Key;
            });

            services.AddMvc(options => options.Filters.Add(new LoginAuthorizeAttribute())).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
                config.IncludeXmlComments(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Api.xml"));
            });

            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //启用中间件服务生成Swagger作为JSON终结点
            app.UseSwagger();
            //启用中间件服务对swagger-ui，指定Swagger JSON终结点
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
            app.UseMvc();

           
        }
    }
}
