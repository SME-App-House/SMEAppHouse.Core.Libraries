using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SMEAppHouse.Core.Patterns.EF.ModelComposite;

namespace SMEAppHouse.Core.Patterns.EF.Helpers
{
    public static class Extentions
    {
        #region SQL Server Operations/Extensions

        public static async Task<List<T>> SqlQueryAsync<T>(this DatabaseFacade database,
            string sqlQry,
            CommandType sqlCmdType)
            where T : class
        {
            return await SqlQueryAsync<T>(database, sqlQry, sqlCmdType, 60, null, true);
        }

        public static async Task<List<T>> SqlQueryAsync<T>(this DatabaseFacade database,
            string sqlQry,
            CommandType sqlCmdType,
            SqlParameter[] sqlQryParams)
            where T : class
        {
            return await SqlQueryAsync<T>(database, sqlQry, sqlCmdType, 60, sqlQryParams, true);
        }

        public static async Task<List<T>> SqlQueryAsync<T>(this DatabaseFacade database,
            string sqlQry,
            CommandType sqlCmdType,
            int sqlCmdTimeout,
            SqlParameter[] sqlQryParams,
            bool renewConn = true) where T : class
        {
            var conn = (SqlConnection)database.GetDbConnection();

            if (renewConn)
                conn = new SqlConnection(conn.ConnectionString);

            return await SqlServerUtil.SqlGetObjectList<T>(conn, sqlQry, sqlCmdType, sqlCmdTimeout,
                sqlQryParams);
        }

        #endregion

        #region IEntityTypeConfiguration Extensions

        /// <summary>
        /// https://stackoverflow.com/a/12420603
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="fieldSelector"></param>
        /// <returns></returns>
        internal static string GetFieldNameFromSelector<TEntity>(Expression<Func<TEntity, object>> fieldSelector)
            where TEntity : class, IEntityBase
        {
            if (fieldSelector.Body is MemberExpression expression)
                return expression.Member.Name;
            else
            {
                var op = ((UnaryExpression)fieldSelector.Body).Operand;
                return ((MemberExpression)op).Member.Name;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="conventionFieldsToIgnore"></param>
        /// <returns></returns>
        internal static IList<string> ToListOfFields<TEntity>(this Expression<Func<TEntity, object>>[] conventionFieldsToIgnore)
            where TEntity : class, IEntityBase
        {
            var includes = new List<string>();
            if (conventionFieldsToIgnore == null || !conventionFieldsToIgnore.Any()) return includes;
            includes.AddRange(conventionFieldsToIgnore.Select(GetFieldNameFromSelector<TEntity>));
            return includes;
        }

        internal static void IgnoreConventionFields<TEntity>(this ModelBuilder modelBuilder,
            Expression<Func<TEntity, object>>[] conventionFieldsToIgnore)
            where TEntity : class, IEntityBase
        {
            var includes = conventionFieldsToIgnore.ToListOfFields<TEntity>();
            IgnoreConventionFields<TEntity>(modelBuilder, includes);
        }

        internal static void IgnoreConventionFields<TEntity>(this ModelBuilder modelBuilder, IList<string> ignoreLIst)
            where TEntity : class, IEntityBase
        {

            /*

            Expression<Func<TEntity, object>> selector = p => p.Ordinal;
            fNm = GetFieldNameFromSelector(selector);
            if (includes.Contains(fNm))
                builder.Entity<T>().Ignore(p => p.Ordinal);
            else builder.Entity<T>()
                    .Property(p => p.Ordinal)
                    .HasColumnName("ordinal")
                    .HasDefaultValue(0);

            selector = p => p.IsActive;
            fNm = GetFieldNameFromSelector(selector);
            if (includes.Contains(fNm))
                builder.Entity<T>().Ignore(p => p.IsActive);
            else builder.Entity<T>()
                .Property(p => p.Ordinal)
                .HasColumnName("ordinal")
                .HasDefaultValue(0);

            selector = p => p.CreatedBy;
            fNm = GetFieldNameFromSelector(selector);
            if (includes.Contains(fNm))
                builder.Entity<T>().Ignore(p => p.CreatedBy);
            else builder.Entity<T>()
                .Property(p => p.Ordinal)
                .HasColumnName("ordinal")
                .HasDefaultValue(0);

            selector = p => p.RevisedBy;
            fNm = GetFieldNameFromSelector(selector);
            if (includes.Contains(fNm))
                builder.Entity<T>().Ignore(p => p.RevisedBy);
            else builder.Entity<T>()
                .Property(p => p.Ordinal)
                .HasColumnName("ordinal")
                .HasDefaultValue(0);

            selector = p => p.DateCreated;
            fNm = GetFieldNameFromSelector(selector);
            if (includes.Contains(fNm))
                builder.Entity<T>().Ignore(p => p.DateCreated);
            else builder.Entity<T>()
                .Property(p => p.Ordinal)
                .HasColumnName("ordinal")
                .HasDefaultValue(0);

            selector = p => p.DateRevised;
            fNm = GetFieldNameFromSelector(selector);
            if (includes.Contains(fNm))
                builder.Entity<T>().Ignore(p => p.DateRevised);
            else builder.Entity<T>()
                .Property(p => p.Ordinal)
                .HasColumnName("ordinal")
                .HasDefaultValue(0);

            */
        }

        #endregion

        #region IEntity Config Extensions

        public static EntityTypeBuilder<T> DefineDbField<T>(this EntityTypeBuilder<T> builder,
            Expression<Func<T, object>> fieldSelector, bool isRequired, string propertyType) 
            where T : class, IEntityBase //IGenericEntityBase<int>
        {
            return builder.DefineDbField(fieldSelector, isRequired, 0, "", propertyType, out var pB);
        }

        public static EntityTypeBuilder<T> DefineDbField<T>(this EntityTypeBuilder<T> builder,
            Expression<Func<T, object>> fieldSelector) 
            where T : class, IEntityBase//IGenericEntityBase<int>
        {
            return builder.DefineDbField(fieldSelector, false, 0, "", "", out var pB);
        }

        public static EntityTypeBuilder<T> DefineDbField<T>(this EntityTypeBuilder<T> builder,
            Expression<Func<T, object>> fieldSelector,
            bool isRequired) 
            where T : class, IEntityBase
        {
            return builder.DefineDbField(fieldSelector, isRequired, 0, "", "", out var pB);
        }

        public static EntityTypeBuilder<T> DefineDbField<T>(this EntityTypeBuilder<T> builder,
            Expression<Func<T, object>> fieldSelector,
            bool isRequired,
            int maxLength) 
            where T : class, IEntityBase
        {
            return builder.DefineDbField(fieldSelector, isRequired, maxLength, "", "", out var pB);
        }

        public static EntityTypeBuilder<T> DefineDbField<T>(this EntityTypeBuilder<T> builder,
                                                                Expression<Func<T, object>> fieldSelector,
                                                                bool isRequired,
                                                                int maxLength,
                                                                string fieldName,
                                                                string propertyType,
                                                                out PropertyBuilder<object> propBldr)
            where T : class, IEntityBase // IGenericEntityBase<int>
        {
            var pB = builder.Property(fieldSelector);
            var memberName = GetFieldNameFromSelector(fieldSelector);

            pB.HasColumnName(string.IsNullOrEmpty(fieldName) ? memberName : fieldName);
            if (maxLength > 0)
                pB.HasMaxLength(maxLength);

            if (!string.IsNullOrEmpty(propertyType))
                pB.HasColumnType(propertyType);

            if (!isRequired)
                pB.IsRequired(false);

            propBldr = pB;
            return builder;
        }


        #endregion

        /// <summary>
        /// FILTER BY PRIMARY KEYS
        /// https://stackoverflow.com/a/42244905
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbContext"></param>
        /// <returns></returns>
        public static IReadOnlyList<IProperty> GetPrimaryKeyProperties<T>(this DbContext dbContext)
        {
            return dbContext.Model.FindEntityType(typeof(T)).FindPrimaryKey().Properties;
        }
        public static Expression<Func<T, bool>> FilterByPrimaryKeyPredicate<T>(this DbContext dbContext, object[] id)
        {
            var keyProperties = dbContext.GetPrimaryKeyProperties<T>();
            var parameter = Expression.Parameter(typeof(T), "e");
            var body = keyProperties
                // e => e.PK[i] == id[i]
                .Select((p, i) => Expression.Equal(
                    Expression.Property(parameter, p.Name),
                    Expression.Convert(
                        Expression.PropertyOrField(Expression.Constant(new { id = id[i] }), "id"),
                        p.ClrType)))
                .Aggregate(Expression.AndAlso);
            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }
        public static IQueryable<TEntity> FilterByPrimaryKey<TEntity>(this DbSet<TEntity> dbSet, DbContext context, object[] id)
            where TEntity : class
        {
            return dbSet.Where(context.FilterByPrimaryKeyPredicate<TEntity>(id));
        }

        /// <summary>
        /// https://stackoverflow.com/a/42475617
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TPk"></typeparam>
        /// <param name="context"></param>
        /// <param name="entity"></param>
        /// <param name="entryId"></param>
        public static void DetachLocal<TEntity, TPk>(this DbContext context, TEntity entity, string entryId)
            where TEntity : class, IGenericEntityBase<TPk>
            where TPk : struct
        {
            var local = context.Set<TEntity>()
                            .Local
                            .FirstOrDefault(entry => entry.Id.Equals(entryId));

            if (local != null)
                context.Entry(local).State = EntityState.Detached;

            context.Entry(entity).State = EntityState.Modified;
        }
    }

}