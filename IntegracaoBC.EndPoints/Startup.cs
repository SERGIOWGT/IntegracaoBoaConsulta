using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using IntegracaoBC.Services.Interfaces;
using IntegracaoBC.Services.Implementations;
using IntegracaoBC.Infra.BoaConsulta.Repositories;
using IntegracaoBC.Domain.Interfaces;
using IntegracaoBC.Infra.Dental021.Repositories;

namespace IntegracaoBC.EndPoints
{
    public class Startup
    {
        private IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Injeção das Entidades
            services.AddSingleton(Configuration);
            services.AddSingleton<ILocationService, LocationService>();
            services.AddSingleton<IAgendaService, AgendaService>();
            services.AddSingleton<IDentistaRepository, DentistaRepository>();
            services.AddSingleton<IDoctorRepository, DoctorRepository>();
            services.AddSingleton<ILocationRepository, LocationRepository>();
            services.AddSingleton<IConsultorioRepository, ConsultorioRepository>();
            services.AddSingleton<IExpedienteDentistaRepository, ExpedienteDentistaRepository>();
            services.AddSingleton<ILoginBoaConsultaRepository, LoginBoaConsultaRepository>();

            services.AddCors();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            //app.UseHttpsRedirection();
            app.UseRouting();

            app.UseCors(builder =>
            {
                builder.AllowAnyHeader();
                builder.AllowAnyMethod();
                builder.AllowAnyOrigin(); // For anyone access.
            });
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
