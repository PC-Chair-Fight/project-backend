using FluentValidation.AspNetCore;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using project_backend.Models.Validators.BidController.AddBid;
using project_backend.Models.Validators.BidController.EditJob;
using project_backend.Models.Validators.JobController.AddJob;
using project_backend.Models.Validators.JobController.GetJobs;
using project_backend.Providers.BidProvider;
using project_backend.Providers.JobProvider;
using project_backend.Providers.UserProvider;
using project_backend.Repos;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.Json.Serialization;

namespace project_backend
{
    public class Startup
    {

        IWebHostEnvironment WebHostEnvironment;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            WebHostEnvironment = env;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddControllers()
                .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()))
                .AddFluentValidation(options => options.RegisterValidatorsFromAssemblyContaining<GetJobsQueryValidator>())
                .AddFluentValidation(options => options.RegisterValidatorsFromAssemblyContaining<AddJobQueryValidator>())
                .AddFluentValidation(options => options.RegisterValidatorsFromAssemblyContaining<EditBidQueryValidator>())
                .AddFluentValidation(options => options.RegisterValidatorsFromAssemblyContaining<AddBidQueryValidator>());


            if (this.WebHostEnvironment.IsProduction())
            {
                services.AddDbContext<DatabaseContext>(options =>
                {
                    options.UseSqlServer(Configuration.GetConnectionString("ProductionDbConnection"));
                });
            }
            else
            {
                services.AddDbContext<DatabaseContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDb");
                });
            }

            services.AddTransient<IUserProvider, UserProvider>();
            services.AddTransient<IJobProvider, JobProvider>();
            services.AddTransient<IBidProvider, BidProvider>();


            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER"),
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_KEY"))),
                    ClockSkew = TimeSpan.Zero
                };
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "project_backend", Version = "v1" });
                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = "JWT Authentication",
                    Description = "Enter JWT Bearer token **_only_**",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };
                c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {securityScheme, Array.Empty<string>()}
                    });
            });
            services.AddFluentValidationRulesToSwagger();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "project_backend v1"));

            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            // This allows the usage of the wwwroot folder. In there, we can store static content, such as images and static pages (maybe 401/404 pages)
            // Right now, as you can see, it is protected by authentication, so if you don't have the Bearer token in the header then the server will drop the request
            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = ctx =>
                {
                    if (!ctx.Context.User.Identity.IsAuthenticated)
                    {
                        ctx.Context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;

                        // If you want to redirect, consider creating a 401 page and redirecting to that using
                        // ctx.Context.Response.Redirect("/my-401-page");
                        ctx.Context.Response.ContentLength = 0;
                        ctx.Context.Response.Headers.Add("Cache-Control", "no-store");
                        ctx.Context.Response.Body = Stream.Null;
                    }
                }
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
