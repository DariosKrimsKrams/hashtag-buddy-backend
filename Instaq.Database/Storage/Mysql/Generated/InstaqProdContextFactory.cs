namespace Instaq.Database.Storage.Mysql.Generated
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Design;

    public class InstaqProdContextFactory : IDesignTimeDbContextFactory<InstaqProdContext>
    {
        public InstaqProdContext CreateDbContext(string[] args)
        {
            var connectionString = "Server=89.22.110.69;User Id=instaq_prod;Password=szV15D6mEeuT70GaOVP0GJ2N7NguJmRQztzoyMeqjTwtN1HC8ojC;Database=InstaqProd;TreatTinyAsBoolean=false;Convert Zero Datetime=True";
            var optionsBuilder   = new DbContextOptionsBuilder<InstaqProdContext>();
            optionsBuilder.UseMySql(connectionString);
            return new InstaqProdContext(optionsBuilder.Options);
        }
    }
}
