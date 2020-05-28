using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProtectiveWearProductsApi.Services;
using ProtectiveWearProductsApi.Models;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System.IO;
using System;
using System.Reflection;

namespace ProtectiveWearProductsApi
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
            
            services.Configure<ProductsDatabaseSettings>(
        Configuration.GetSection(nameof(ProductsDatabaseSettings)));

            services.AddSingleton<IProductsDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<ProductsDatabaseSettings>>().Value);

            services.AddSingleton<ProductService>();

            services.AddControllers()
                .AddNewtonsoftJson();

            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Consulta, evaluación y creacion de productos para la protección médica",
                    Version = "v1",
                    Description = "REST API  para la  aplicación de consulta y creacion de productos para la protección médica by: gilmo",
                    Contact = new OpenApiContact()
                    {
                        Name = "Guillaumer Gil M.",
                        Email = "yiyo26@outlook.es"
                    }

                });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            
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

            //app.UseAuthorization();

            // Crea un middleware para exponer la documentación en el JSON.
            app.UseSwagger();
            // Crea  un middleware para exponer el UI (HTML, JS, CSS, etc.),
            // Especificamos en que endpoint buscara el json.
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Protective Wear Api V1");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            
        }
    }
}
