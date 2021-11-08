using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using Moq;
using Refactoring.JuniorApi.Controllers;
using Refactoring.LegacyService;
using Refactoring.LegacyService.Interfaces;
using System;
using System.Linq;
using System.Net.Http;

namespace Refactoring.JuniorApi.Tests
{
    public class JuniorApiWebApplicationFactory<T> : 
        WebApplicationFactory<Startup> where T : class
    {
        public Mock<ICandidateService> CandidateSrv = new Mock<ICandidateService>() { CallBase = true};
        public Mock<ICandidateDataAccess> CandidateDataAccessSrv = new Mock<ICandidateDataAccess>() { CallBase = true };
        public Mock<ICandidateCreditService> CandidateCreditSrv = new Mock<ICandidateCreditService>() { CallBase = true };
        public Mock<IPositionRepository> PositionRepoSrv = new Mock<IPositionRepository>() { CallBase = true };

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services => 
            {
                var candidateSrv = services.FirstOrDefault(x => x.ServiceType == typeof(ICandidateService));
                if(candidateSrv != null)
                {
                    services.Remove(candidateSrv);
                }

                var candidateDataAccessSrv = services.FirstOrDefault(x => x.ServiceType == typeof(ICandidateDataAccess));
                if (candidateDataAccessSrv != null)
                {
                    services.Remove(candidateDataAccessSrv);
                }

                var candidateCreditSrv = services.FirstOrDefault(x => x.ServiceType == typeof(ICandidateCreditService));
                if (candidateCreditSrv != null)
                {
                    services.Remove(candidateCreditSrv);
                }

                var positionRepoSrv = services.FirstOrDefault(x => x.ServiceType == typeof(IPositionRepository));
                if (positionRepoSrv != null)
                {
                    services.Remove(positionRepoSrv);
                }

                services.AddScoped(typeof(ICandidateService), typeof(CandidateService));
                services.AddSingleton(typeof(ICandidateDataAccess), CandidateDataAccessSrv.Object);
                services.AddSingleton(typeof(ICandidateCreditService), CandidateCreditSrv.Object);
                services.AddSingleton(typeof(IPositionRepository), PositionRepoSrv.Object);
                //services.AddSingleton(typeof(ILogger<CandidatesController>));
            }).ConfigureLogging(builder => 
            {
                builder.AddLog4Net("log4net.config");
                builder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Debug);
            });

        }
    }
}
