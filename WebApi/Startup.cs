using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Data;
using WebApi.Error;
using WebApi.Identity;
using WebApi.Jwt;
using WebApi.Repository;

namespace WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            LogManager.LoadConfiguration(System.String.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();


            services.AddDbContext<ApplicationDbContext>(options =>
                  options.UseSqlServer(
              Configuration.GetValue<string>("MSSqlConnectionString:DevConnection")));

              services.AddIdentity<IdentityUser, IdentityRole>()
               .AddEntityFrameworkStores<ApplicationDbContext>()
               .AddDefaultTokenProviders();



            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                var Key = Encoding.UTF8.GetBytes(Configuration["JWT:Secret"]);
                o.SaveToken = true;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["JWT:ValidIssuer"],
                    ValidAudience = Configuration["JWT:ValidAudience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Key)
                };
            });


            //var key = "This is my first Test Key";

            //services.AddAuthentication(x =>
            //{
            //   x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            //}).AddJwtBearer(x =>
            //{
            //    x.RequireHttpsMetadata = false;
            //    x.SaveToken = true;
            //    x.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidateIssuerSigningKey = true,
            //        ValidateLifetime = true,


            //        ValidateAudience = false,
            //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["JWT:Secret"]))
            //    };
            //});

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                    });
            });



            services.AddControllers().AddNewtonsoftJson();

            services.AddSingleton<IJwtAuth>(new Auth(Configuration["JWT:Secret"]));

            services.AddTransient<IOpdPatientRepository, OpdPatientRepository>();

            services.AddTransient<IIPdPatientRepository, IPDPatientRespository>();

            services.AddTransient<IJsonFormatRepository, JsonFormatRepository>();

            services.AddTransient<IRequestHeaderRepository, RequestHeaderRepository>();

            services.AddTransient<ITest, Test>();

          //  services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();



            services.AddSingleton<ILog, LogNLog>();
           

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            //app.Use(async (context, next) => 
            //{
            //    await context.Response.WriteAsync("your are");
            //    await next();
            //    //await context.Response.WriteAsync("your are");
            //});

            //app.Run(async context =>
            //{
            //    await context.Response.WriteAsync("your are run");
            //    //await next();
            //    //await context.Response.WriteAsync("your are");
            //});

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error-production");
                app.UseHsts();
            }


            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors();

            
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
