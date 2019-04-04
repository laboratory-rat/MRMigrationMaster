using System;

namespace MRMigrationMaster.Infrastructure.Attr
{
    public class MigrationAttr : Attribute
    {
        public string Name { get; set; }
        public string LastUpdated { get; set; }

        public MigrationAttr(string name, string lastUpdated)
        {
            Name = name;
            LastUpdated = lastUpdated;
        }
    }
}
