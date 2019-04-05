using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MRMigrationMaster.Infrastructure.Attr;
using MRMigrationMaster.Infrastructure.Component;
using MRMigrationMaster.Infrastructure.Enum;
using MRMigrationMaster.Infrastructure.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using Console = Colorful.Console;

namespace MRMigrationMaster
{
    public abstract class Master
    {
        protected List<MigrationAndMeta> _collection { get; set; }
        protected IConfiguration _configuration { get; set; }
        protected IServiceCollection _serviceCollection { get; set; }

        public Master() { }

        public virtual async Task Start()
        {
            _configuration = ConfigurationInit();
            _serviceCollection = ServicesInit();

            Scan();

            Log("Migration master started.", LogType.INFO);

            if (!_collection.Any())
            {
                Log("No migrations found.", LogType.DANGER);
            }
            else
            {
                while (true)
                {
                    Log($"Select migration to apply", LogType.NONE);

                    string input = string.Empty;
                    for (var i = 0; i < _collection.Count; i++)
                    {
                        var cursor = i + 1;
                        var data = _collection[i];

                        Log($"{cursor}. [{data.Attr.LastUpdated}] {data.Attr.Name}.", LogType.NONE);
                    }

                    Log("-1. Exit.", LogType.NONE);
                    Log("", LogType.NONE);

                    Log("Select migration.", LogType.NONE);

                    input = Console.ReadLine();

                    if(int.TryParse(input, out int selected))
                    {
                        if (selected == -1)
                            break;

                        if(selected > _collection.Count)
                        {
                            Log("Wrong number.", LogType.DANGER);
                            continue;
                        }

                        var data = _collection[selected - 1];

                        Log($"Start migration {data.Attr.Name}?", LogType.NONE);
                        Log("y/n", LogType.NONE);

                        input = Console.ReadLine();
                        if(input?.ToUpperInvariant() == "Y")
                        {
                            Log($"Started {data.Attr.Name}", LogType.INFO);
                            try
                            {
                                await data.Migration.Action();
                                Log($"Migration finished success.", LogType.INFO);
                            }
                            catch (Exception ex)
                            {
                                Log($"Exception in migration. {ex.Message}", LogType.DANGER);
                            }
                        }
                    }
                    else
                    {
                        Log("Bad input.", LogType.DANGER);
                    }
                }
            }

            Log("Migration master finished work. Press enter to exit.", LogType.INFO);
            Console.ReadLine();
        }

        public virtual void Log(object subject, LogType type)
        {
            switch (type)
            {
                case LogType.NONE:
                    Console.WriteLine(subject.ToString());
                    break;
                case LogType.INFO:
                    Console.Write(subject.ToString(), ConsoleColor.Blue);
                    break;
                case LogType.WARNING:
                    Console.Write(subject.ToString(), ConsoleColor.Magenta);
                    break;
                case LogType.DANGER:
                    Console.Write(subject.ToString(), ConsoleColor.Red);
                    break;
                case LogType.SUCCESS:
                    Console.Write(subject.ToString(), ConsoleColor.Green);
                    break;
                default:
                    break;
            }
        }

        protected virtual void Scan()
        {
            _collection = new List<MigrationAndMeta>();

            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type type in assembly.GetTypes())
                {
                    var attribs = type.GetCustomAttributes(typeof(MigrationAttr), false);
                    if (attribs != null && attribs.Length > 0)
                    {
                        var migration = (IMigration)Activator.CreateInstance(type);
                        migration.Init(Log, this, _serviceCollection, _configuration);

                        _collection.Add(new MigrationAndMeta((MigrationAttr)attribs.First(), migration));
                    }
                }
            }
        }

        protected abstract IServiceCollection ServicesInit();
        protected abstract IConfiguration ConfigurationInit();
    }
}
