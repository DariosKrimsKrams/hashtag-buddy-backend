namespace Instaq.Database.Storage.Mysql.Generated
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Design;

    public class InstaqContextFactory : IDesignTimeDbContextFactory<InstaqContext>
    {
        public InstaqContext CreateDbContext(string[] args)
        {
            var connectionString = "INSERT CONNECTION STRING HERE WHEN EXECUTING MIGRATIONS";
            var optionsBuilder   = new DbContextOptionsBuilder<InstaqContext>();
            optionsBuilder.UseMySql(connectionString);
            return new InstaqContext(optionsBuilder.Options);
        }
    }
}
