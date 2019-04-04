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

        public void Init(Action<object, LogType> log, Master master)
        {
            Log = log;
            Master = master;
        }

        public abstract Task Action();
    }
}
