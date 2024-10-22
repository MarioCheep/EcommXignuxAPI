//using System.Web.Http.Description;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
//using Swashbuckle.Swagger;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;

namespace EventHubLastMilleApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        [Obsolete]
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<IISServerOptions>(options =>
            {
                options.AutomaticAuthentication = false;
            });

            services.AddControllers()
            .AddJsonOptions(o => o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_0);

            

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                    builder.SetIsOriginAllowed(_ => true)
                    //.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .Build());
            });

            services.AddControllersWithViews().
          AddJsonOptions(options =>
          {
              options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
              options.JsonSerializerOptions.PropertyNamingPolicy = null;
              options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
          });

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v2", new OpenApiInfo
                {
                    Title = "Xignux E-Commerce API",
                    Version = "v1",
                    Description = "Excercise Methods",
                });


                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);


                options.ResolveConflictingActions(ApiDescription => ApiDescription.First());
                options.DescribeAllParametersInCamelCase();


                options.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
                {
                    Description = "`Token only!!!` - without `Bearer_` prefix",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Scheme = "bearer"
                });
                options.OperationFilter<AddAuthHeaderOperationFilter>();


                options.SelectDiscriminatorNameUsing((baseType) => "TypeName");
                options.SelectDiscriminatorValueUsing((subType) => subType.Name);

                options.SchemaFilter<MySwaggerSchemaFilter>();
                options.EnableAnnotations(enableAnnotationsForInheritance: true, enableAnnotationsForPolymorphism: true);

            });
           services.AddSwaggerGenNewtonsoftSupport();
            

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration.GetSection("Jwt").GetSection("Issuer").Value,
                        ValidAudience = Configuration.GetSection("Jwt").GetSection("Issuer").Value,
                        LifetimeValidator = TokenLifetimeValidator.Validate,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetSection("Jwt").GetSection("Key").Value))
                    };
                });

            services.AddMvc();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else if (env.IsProduction())
            {
                app.UseStatusCodePages();
                app.UseHsts();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

           // app.UseCors("AllowSpecificOrigin");
            app.UseCors("CorsPolicy");
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("../swagger/v2/swagger.json", "PlaceInfo Services");

                options.DefaultModelExpandDepth(2);
                options.DefaultModelRendering(ModelRendering.Model);
                options.DefaultModelsExpandDepth(-1);
                options.DisplayOperationId();
                options.DisplayRequestDuration();
                options.DocExpansion(DocExpansion.None);
                options.EnableDeepLinking();
                options.EnableFilter();
                options.MaxDisplayedTags(5);
                options.ShowExtensions();
                options.ShowCommonExtensions();
                options.EnableValidator();
                //options.SupportedSubmitMethods(SubmitMethod.Get, SubmitMethod.Head); //Cancela el ejecutar dese Swagger los metodos
                options.UseRequestInterceptor("(request) => { return request; }");
                options.UseResponseInterceptor("(response) => { return response; }");

                options.InjectStylesheet("/swagger-ui/custom.css");

            });


            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor |
                ForwardedHeaders.XForwardedProto
            });

        }

        private class AddAuthHeaderOperationFilter : Swashbuckle.AspNetCore.SwaggerGen.IOperationFilter
        {
            public void Apply(OpenApiOperation operation, OperationFilterContext context)
            {
                if (operation.Security == null)
                    operation.Security = new List<OpenApiSecurityRequirement>();


                var scheme = new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "bearer" } };
                operation.Security.Add(new OpenApiSecurityRequirement
                {
                    [scheme] = new List<string>()
                });


                //var isAuthorized = (context.MethodInfo.DeclaringType.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any()
                //                    || context.MethodInfo.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any())
                //                    && !context.MethodInfo.GetCustomAttributes(true).OfType<AllowAnonymousAttribute>().Any(); // this excludes methods with AllowAnonymous attribute

                //if (!isAuthorized) return;


                //operation.Responses.TryAdd("401", new OpenApiResponse { Description = "Unauthorized" });
                //operation.Responses.TryAdd("403", new OpenApiResponse { Description = "Forbidden" });

                //var jwtbearerScheme = new OpenApiSecurityScheme
                //{
                //    Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "bearer" }
                //};

                //operation.Security = new List<OpenApiSecurityRequirement>
                //{
                //    new OpenApiSecurityRequirement { [jwtbearerScheme] = new string []{} }
                //};
            }

            //public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
            //{
            //    throw new NotImplementedException();
            //}
        }

        private class MySwaggerSchemaFilter : ISchemaFilter
        {
            public void Apply(OpenApiSchema schema, SchemaFilterContext context)
            {
                if (schema?.Properties == null)
                {
                    return;
                }

                var ignoreDataMemberProperties = context.Type.GetProperties()
                    .Where(t => t.GetCustomAttribute<IgnoreDataMemberAttribute>() != null);

                foreach (var ignoreDataMemberProperty in ignoreDataMemberProperties)
                {
                    var propertyToHide = schema.Properties.Keys
                        .SingleOrDefault(x => x.ToLower() == ignoreDataMemberProperty.Name.ToLower());

                    if (propertyToHide != null)
                    {
                        schema.Properties.Remove(propertyToHide);
                    }
                }
            }
        }

        private static class TokenLifetimeValidator
        {
            public static bool Validate(
                DateTime? notBefore,
                DateTime? expires,
                SecurityToken tokenToValidate,
                TokenValidationParameters @param
            )
            {
                return (expires != null && expires > DateTime.UtcNow);
            }
        }
    }
}
