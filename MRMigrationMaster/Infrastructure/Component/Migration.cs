using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MRMigrationMaster.Infrastructure.Enum;
using MRMigrationMaster.Infrastructure.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MRMigrationMaster.Infrastructure.Component
{
    public abstract class Migration : IMigration
    {
        public Action<object, LogType> Log { get; set; }
        public Master Master { get; set; }

        protected IServiceCollection _services;
        protected IConfiguration _configuration;

        public void Init(Action<object, LogType> log, Master master, IServiceCollection services, IConfiguration configuration)
        {
            Log = log;
            Master = master;
            _services = services;
            _configuration = configuration;
        }

        public abstract Task Action();
    }
}
