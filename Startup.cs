using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DbUp;
using QA.Data;
using QA.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using QA.Authorization;
namespace QA
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            EnsureDatabase.For.SqlDatabase(connectionString);
            var upgrader = DeployChanges.To.SqlDatabase(connectionString, null)
                                                                            .WithScriptsEmbeddedInAssembly(
                                                                             System.Reflection.Assembly.GetExecutingAssembly())
                                                                            .WithTransaction()
                                                                            .Build();
            if (upgrader.IsUpgradeRequired())
            {
                upgrader.PerformUpgrade();
            }
            services.AddControllersWithViews(); 

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });
            services.AddScoped<IDataRepository,DataRepository>();
            services.AddMemoryCache();
            services.AddSingleton<IQuestionCache,QuestionCache>();
            services.AddAuthentication(options=>
            {
                options.DefaultAuthenticateScheme=JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme=JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options=>
            {
                options.Authority=Configuration["Auth0:Authority"];
                options.Audience=Configuration["Auth0:Audience"];
            });
            services.AddHttpClient();
            services.AddAuthorization(options => {
             options.AddPolicy("MustBeQuestionAuthon",policy =>
             policy.Requirements.Add(new MustBeQuestionAuthorRequirement()));
            });
            services.AddScoped<IAuthorizationHandler,MustBeQuestionAuthorHandler>();
            services.AddHttpContextAccessor();
            services.AddCors(options =>
                                options.AddPolicy("CorsPolicy", builder => builder

                                                   .AllowAnyMethod()
                                                   .AllowAnyHeader()
                                                    .WithOrigins(Configuration["Frontend"])));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseRouting();
            app.UseCors("CorsPolicy");
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }
    }
}
