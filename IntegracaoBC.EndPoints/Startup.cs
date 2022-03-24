using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using IntegracaoBC.Services.Interfaces;
using IntegracaoBC.Services.Implementations;
using IntegracaoBC.Domain.Interfaces;
using IntegracaoBC.Infra.Dental021.Repositories;
using IntegracaoBC.Infra.BoaConsulta.Repositories;
using IntegracaoBC.Domain.Implementations;
using Serilog;
using IntegracaoBC.Providers.Interfaces;
using IntegracaoBC.Providers.Implementations;

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

            services.AddSingleton<IProviderBoaConsulta, ProviderBoaConsulta>();
            services.AddSingleton<IProvider021Dental, Provider021Dental>();
            services.AddSingleton<IProviderAgenda021, ProviderAgenda021>();

            services.AddSingleton<ITrataEventosAgendaService, TrataEventosAgendaService>();
            services.AddSingleton<ILimpaConsultoriosService, LimpaConsultoriosService>();
            services.AddSingleton<IListaVagasService, LIstaVagasService>();
            services.AddSingleton<IListaEspecialidadeAgendaService, ListaEspecialidadeAgendaService>();
            services.AddSingleton<IListaSpecialtyService, ListaSpecialtyService>();
            services.AddSingleton<ISincronizaAgendaService, SincronizaAgendaService>();
            services.AddSingleton<ISincronizaConsultoriosService, SincronizaConsultoriosService>();

            services.AddSingleton<IAgendaRepository, AgendaRepository>();
            services.AddSingleton<IAgenda021Repository, Agenda021Repository>();
            services.AddSingleton<IAppointmentRepository, AppointmentRepository>();
            services.AddSingleton<IConsultorioRepository, ConsultorioRepository>();
            services.AddSingleton<IConvenioRepository, ConvenioRepository>();
            services.AddSingleton<IDoctorRepository, DoctorRepository>();
            services.AddSingleton<IEspecialidadeAgendaRepository, EspecialidadeAgendaRepository>();
            services.AddSingleton<IExpedienteDentistaRepository, ExpedienteDentistaRepository>();
            services.AddSingleton<IExpedienteRepository, ExpedienteRepository>();
            services.AddSingleton<ILocationRepository, LocationRepository>();
            services.AddSingleton<IPacienteRepository, PacienteRepository>();
            services.AddSingleton<IReasonRepository, ReasonRepository>();
            services.AddSingleton<ISpecialtyRepository, SpecialtyRepository>();
            services.AddSingleton<ISlot021Repository, Slot021Repository>();

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
