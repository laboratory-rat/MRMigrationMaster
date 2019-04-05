using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MRMigrationMaster.Infrastructure.Enum;
using System;
using System.Threading.Tasks;

namespace MRMigrationMaster.Infrastructure.Interface
{
    public interface IMigration
    {
        Action<object, LogType> Log { get; set; }
        Master Master { get; set; }

        void Init(Action<object, LogType> log, Master master, IServiceCollection services, IConfiguration configuration);
        Task Action();
    }
}
