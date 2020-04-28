namespace Instaq.Database.Storage.Mysql.Generated
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using static System.String;

    public partial class InstaqProdContext : DbContext
    {
        private readonly string _defaultConnection;
        private ILoggerFactory _loggerFactory;

        public InstaqProdContext(string defaultConnection)
        {
            _defaultConnection = defaultConnection;
        }

        public InstaqProdContext(string defaultConnection, ILoggerFactory loggerFactory)
        {
            _defaultConnection = defaultConnection;
            _loggerFactory     = loggerFactory;
        }

        public InstaqProdContext(DbContextOptions<InstaqProdContext> options, string defaultConnection)
            : base(options)
        {
            _defaultConnection = defaultConnection;
        }

        public InstaqProdContext(DbContextOptions<InstaqProdContext> options, ILoggerFactory loggerFactory)
            : base(options)
        {
            _loggerFactory = loggerFactory;
        }

        public InstaqProdContext(DbContextOptions<InstaqProdContext> options, string defaultConnection, ILoggerFactory loggerFactory)
            : base(options)
        {
            _defaultConnection = defaultConnection;
            _loggerFactory     = loggerFactory;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured)
            {
                return;
            }
            if (!IsNullOrEmpty(this._defaultConnection))
            {
                optionsBuilder.UseMySql(this._defaultConnection);
            }

            if (_loggerFactory != null)
            {
                optionsBuilder.UseLoggerFactory(_loggerFactory);
            }
        }

    }
}
