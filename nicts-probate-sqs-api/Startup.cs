using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Amazon;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using Amazon.SQS;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Formatters.Xml;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using nicts_probate_sqs_api.Filters;
using nicts_probate_sqs_api.Models;
using nicts_probate_sqs_api.Services;
using Steeltoe.Extensions.Configuration.CloudFoundry;

namespace nicts_probate_sqs_api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _credentialsModel = new CredentialsModel
            {
                AccessKey = Configuration["vcap:services:user-provided:0:credentials:AccessKey"],
                SecretKey = Configuration["vcap:services:user-provided:0:credentials:SecretKey"],
                Region = Configuration["vcap:services:user-provided:0:credentials:Region"]
            };
        }

        public IConfiguration Configuration { get; }
        private readonly CredentialsModel _credentialsModel;

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// 
        /// It also leverages SteelToe to pull in configuration that has been set on CLoud Foundry,
        /// specifically the AWS credentials for the user of SQS client.
        ///
        /// This is achieved by creating a user provided service on Cloud Foundry that is bound to this app.
        /// Credentials then become accessible to app via VCAP_SERVICES. These can be retrieved in Startup
        /// directly from Configuration object, or by injecting into services, see queue services.
        /// 
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddXmlSerializerFormatters().AddXmlDataContractSerializerFormatters(); ;

            services.AddOptions();
            services.ConfigureCloudFoundryOptions(Configuration);

            services.AddAWSService<IAmazonSQS>(new AWSOptions()
            {
                Credentials = new BasicAWSCredentials(_credentialsModel.AccessKey, _credentialsModel.SecretKey),
                Region = RegionEndpoint.GetBySystemName(_credentialsModel.Region)
            });

            services.AddSwaggerGen(c =>
            {
                c.SchemaFilter<AddNamespaceFilter>();
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "NICTS Probate API", Version = "v1" });
            });
            services.Configure<QueueModel>(Configuration.GetSection("vcap:services:user-provided:0:credentials:Queue"));

            //services.AddScoped<IQueueService, QueueByInjectionService>();
            services.AddScoped<IQueueService, QueueByConfigurationService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "NICTS Probate API");
                c.RoutePrefix = string.Empty;
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
