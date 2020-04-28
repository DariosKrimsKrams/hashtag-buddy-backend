namespace Instaq.Database.Storage.Mysql.Generated
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using static System.String;

    public partial class InstaqContext : DbContext
    {
        private readonly string _defaultConnection;
        private ILoggerFactory _loggerFactory;

        public InstaqContext(string defaultConnection)
        {
            _defaultConnection = defaultConnection;
        }

        public InstaqContext(string defaultConnection, ILoggerFactory loggerFactory)
        {
            _defaultConnection = defaultConnection;
            _loggerFactory     = loggerFactory;
        }

        public InstaqContext(DbContextOptions<InstaqContext> options, string defaultConnection)
            : base(options)
        {
            _defaultConnection = defaultConnection;
        }

        public InstaqContext(DbContextOptions<InstaqContext> options, ILoggerFactory loggerFactory)
            : base(options)
        {
            _loggerFactory = loggerFactory;
        }

        public InstaqContext(DbContextOptions<InstaqContext> options, string defaultConnection, ILoggerFactory loggerFactory)
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
