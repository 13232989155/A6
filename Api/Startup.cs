using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Api.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PaySharp.Wechatpay;
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

            services.AddPaySharp(a =>
            {
                var wechatpayMerchant = new Merchant
                {
                    AppId = WeChat.LoginHelper.AppID,
                    MchId = WeChat.PayEntity.MchId,
                    Key = WeChat.PayEntity.Key,
                    AppSecret = WeChat.LoginHelper.AppSecret,
                    SslCertPath = "",
                    SslCertPassword = "",
                    NotifyUrl = WeChat.PayEntity.notifyUrl
                };
                a.Add(new WechatpayGateway(wechatpayMerchant));
                a.UseWechatpay(Configuration);
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

            app.UsePaySharp();

        }
    }
}
