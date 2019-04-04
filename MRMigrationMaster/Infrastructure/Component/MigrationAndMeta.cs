using MRMigrationMaster.Infrastructure.Attr;
using MRMigrationMaster.Infrastructure.Interface;

namespace MRMigrationMaster.Infrastructure.Component
{
    public class MigrationAndMeta
    {
        public MigrationAttr Attr { get; set; }
        public IMigration Migration { get; set; }

        public MigrationAndMeta() { }

        public MigrationAndMeta(MigrationAttr attr, IMigration migration)
        {
            Attr = attr;
            Migration = migration;
        }
    }
}
