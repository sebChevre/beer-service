using System;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using BeerApi.Infrastructure.Configuration;
using BeerApi.Controllers.Response;



namespace BeerApi.Controllers
{
    [ApiController]
    [Route("/api/sysinfo")]
    public class SystemInfoController : ControllerBase
    {

        private readonly ILogger<SystemInfoController> _logger;
        IBeerstoreDatabaseSettings _settings;
        public SystemInfoController(IBeerstoreDatabaseSettings settings,ILogger<SystemInfoController> logger){
            _settings = settings;
            _logger = logger;
        }

        [HttpGet]
        public SystemInfo Get()
        {

            string envNet = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            string mongoDbUrl = Environment.GetEnvironmentVariable("MONGODB_URL");
            
            return new SystemInfo
            {
                Ip = HttpContext.Connection.RemoteIpAddress.ToString(),
                Host = HttpContext.Request.Host.Host,
                LocalIp = HttpContext.Connection.LocalIpAddress.ToString(),
                AspNetCoreEnv = envNet,
                MongoDbUrl = mongoDbUrl

            };
            
        }

        [HttpGet]
        [Route("/api/build")]
        public BuildInfo Index()
        {
                return new BuildInfo{
                    BaseTag = ThisAssembly.Git.BaseTag,
                    Branche = ThisAssembly.Git.Branch,
                    Commit = ThisAssembly.Git.Commit,
                    CommitDate = ThisAssembly.Git.CommitDate,
                    Tag = ThisAssembly.Git.Tag,
                    Sha = ThisAssembly.Git.Sha,
                    Version = ThisAssembly.Git.SemVer.Major + "." + ThisAssembly.Git.SemVer.Minor + "." + ThisAssembly.Git.SemVer.Patch,
                    SemVer = ThisAssembly.Git.SemVer.Label
                };
        }
    }
}
