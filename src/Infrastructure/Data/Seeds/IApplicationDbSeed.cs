using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tm20trial.Infrastructure.Data.Seeds;
public interface IApplicationDbSeed
{    public Task PreMigrationSeedAsync();

    public Task PostMigrationSeedAsync();
}
