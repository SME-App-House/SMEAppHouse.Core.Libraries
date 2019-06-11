using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace SMEAppHouse.Core.Patterns.EF.StrategyForDBCtxt
{
    /// <inheritdoc />
    /// <summary>
    /// 
    /// </summary>
    public class AppDbContextExtended<TDbContext> : DbContext
        where TDbContext : DbContext, new()
    {

        public DbContextOptions<TDbContext> DbContextOptions { get; set; }

        #region constructor

        public AppDbContextExtended() : base()
        {
        }

        public AppDbContextExtended(string connectionString) : this(GetOptions(connectionString))
        {

        }

        public AppDbContextExtended(DbContextOptions<TDbContext> options)
            : base(options)
        {
            this.DbContextOptions = options;
        }

        #endregion

        /// <summary>
        /// https://blog.oneunicorn.com/2016/10/24/ef-core-1-1-creating-dbcontext-instances/
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="migrationTblName"></param>
        /// <param name="dbSchema"></param>
        /// <returns></returns>
        public static TDbContext CreateDbContext(string connectionString, string migrationTblName, string dbSchema)
        {
            var dbCntxOpt = GetOptions(connectionString, migrationTblName, dbSchema);
            return (TDbContext)(DbContext)new AppDbContextExtended<TDbContext>(dbCntxOpt);
        }
        public static DbContextOptions<TDbContext> GetOptions(string connectionString)
        {
            return new DbContextOptionsBuilder<TDbContext>()
                    .UseSqlServer(connectionString)
                    .Options;
        }
        public static DbContextOptions<TDbContext> GetOptions(string connectionString, string migrationTblName, string dbSchema)
        {
            return new DbContextOptionsBuilder<TDbContext>()
                    .UseSqlServer(connectionString, builder =>
                    {
                        builder.MigrationsHistoryTable(migrationTblName, dbSchema);
                    })
                    .Options;
        }
        

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int SaveChanges()
        {
            var entities = (from entry in ChangeTracker.Entries()
                            where entry.State == EntityState.Modified || entry.State == EntityState.Added
                            select entry.Entity);

            var validationResults = new List<ValidationResult>();
            if (entities.Any(entity => !Validator.TryValidateObject(entity, new ValidationContext(entity), validationResults)))
            {
                throw new ValidationException(); //or do whatever you want
            }
            return base.SaveChanges();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }

    
}
