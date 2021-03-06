﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System;
using System.IO;
using ProtectiveWearSecurity.Services;
using ProtectiveWearSecurity.Filters;
using ProtectiveWearSecurity.Models;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace ProtectiveWearSecurity
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public virtual void ConfigureServices(IServiceCollection services)
        {
            // for testing debug local machine
            //services.AddDbContext<ProtectiveWearApiDbContext>(options =>
            //options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

            var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");

            services.AddDbContext<ProtectiveWearApiDbContext>(options =>
                options.UseNpgsql(
                    connectionString
                )
            );

            services.AddSingleton<ProductService>();

            services.AddIdentity<ApplicationUser, IdentityRole>()
                                                                .AddEntityFrameworkStores<ProtectiveWearApiDbContext>()
                                                                .AddDefaultTokenProviders();

            services.AddAuthentication(option =>
             {
                 option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                 option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
             }
            )
            .AddCookie()
            .AddJwtBearer(jwtBearerOptions =>
            {
                jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateActor = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Token:Issuer"],
                    ValidAudience = Configuration["Token:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Token:Key"]))
                };
            });


           

            // El tiempo de espera predeterminado de inactividad es de 14 días. El siguiente código establece el tiempo de espera de inactividad en 5 días:
            services.ConfigureApplicationCookie(o => {
                o.ExpireTimeSpan = TimeSpan.FromDays(5);
                o.SlidingExpiration = true;
            });

            // El siguiente código cambia el tiempo de espera de todos los tokens de protección de datos a 3 horas
            services.Configure<DataProtectionTokenProviderOptions>(o =>
            o.TokenLifespan = TimeSpan.FromHours(3));

            // configure section email sender
            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));
            services.AddTransient<IEmailSender, EmailSender>();

            // redireccion a puerto https cuando se solicite.
            //services.AddHttpsRedirection(options =>
            //{
            //    options.HttpsPort = 443;
            //});

            services.AddControllers();
            services.AddCors();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Direccionamiento y distribución de recursos hacia el consumo de todo el proceso de Ropa para la protección en medicina",
                    Version = "v1",
                    Description = "REST API  para la  aplicación de filtro de identificación y seguridad hacia el consumo de recursos para proceso de productos para la protección médica by: gilmo",
                    Contact = new OpenApiContact()
                    {
                        Name = "Guillaumer Gil M.",
                        Email = "yiyo26@outlook.es"
                    }

                });
                c.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer {token} scheme.",
                });

                /// este es un esquema de seguridad global, si es lo que se desea
                //c.AddSecurityRequirement(new OpenApiSecurityRequirement
                //{
                //    {
                //          new OpenApiSecurityScheme
                //            {
                //                Reference = new OpenApiReference
                //                {
                //                    Type = ReferenceType.SecurityScheme,
                //                    Id = "Bearer"
                //                }
                //            },
                //            new string[] {}

                //    }
                //});

                //////Add Operation Specific Authorization///////
                /// nos permite agregar filtros solo a API específicas basadas en la verificación de atributos como "Auhtorize"
                c.OperationFilter<AuthOperationFilter>();
                ////////////////////////////////////////////////

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

            });

            services.AddMvc(options =>
            {
                options.Filters.Add(item: new HttpExceptionFilter());
            });

            
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
                app.UseHsts();
            }
            // Crea un middleware para exponer la documentación en el JSON.
            app.UseSwagger();
            // Crea  un middleware para exponer el UI (HTML, JS, CSS, etc.),
            // Especificamos en que endpoint buscara el json.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Protective Wear Security Api V1");
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(x => x
               .AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader());

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
