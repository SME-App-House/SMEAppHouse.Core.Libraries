using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SMEAppHouse.Core.Patterns.EF.Helpers;
using SMEAppHouse.Core.Patterns.EF.ModelComposite;

/*
 
    DELETE all tables in db based on schema:

    DECLARE @SqlStatement NVARCHAR(MAX)
SELECT @SqlStatement = 
    COALESCE(@SqlStatement, N'') + N'DROP TABLE [pow].' + QUOTENAME(TABLE_NAME) + N';' + CHAR(13)
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_SCHEMA = 'pow' and TABLE_TYPE = 'BASE TABLE'

PRINT @SqlStatement
     
     */

namespace SMEAppHouse.Core.Patterns.EF.StrategyForModelCfg
{

    public interface IModelConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
        where TEntity : class, IEntity
    {
        string Schema { get; }
        Expression<Func<TEntity, object>>[] FieldsToIgnore { get; set; }
        void Map(EntityTypeBuilder<TEntity> entityBuilder);
    }

    public abstract class ModelConfigurationBase<TEntity, TPk> : IModelConfiguration<TEntity>
        where TEntity : class, IIdentifiableEntity<TPk>
        where TPk : struct
    {
        private readonly string _altTableName;
        private readonly bool _prefixEntityNameToId;
        private readonly bool _prefixAltTblNameToEntity;
        private readonly string _preferredPKeyId;
        private readonly bool _pluralizeTblName;

        public string Schema { get; private set; }
        public Expression<Func<TEntity, object>>[] FieldsToIgnore { get; set; }

        public virtual void Map(EntityTypeBuilder<TEntity> entityBuilder)
        {
            SetupConventions<TEntity>(entityBuilder, FieldsToIgnore);
        }

        #region constructors

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        /// <param name="prefixEntityNameToId"></param>
        /// <param name="prefixAltTblNameToEntity"></param>
        /// <param name="schema"></param>
        /// <param name="pluralizeTblName"></param>
        protected ModelConfigurationBase(bool prefixEntityNameToId = false
                                            , bool prefixAltTblNameToEntity = false
                                            , string schema = "dbo"
                                            , bool pluralizeTblName = true)
                                            : this("", prefixEntityNameToId
                                                  , prefixAltTblNameToEntity
                                                  , schema
                                                  , pluralizeTblName)
        {
        }

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        /// <param name="pluralTableName"></param>
        /// <param name="prefixEntityNameToId"></param>
        /// <param name="prefixAltTblNameToEntity"></param>
        /// <param name="schema"></param>
        /// <param name="pluralizeTblName"></param>
        protected ModelConfigurationBase(string pluralTableName = ""
                                        , bool prefixEntityNameToId = false
                                        , bool prefixAltTblNameToEntity = false
                                        , string schema = "dbo"
                                        , bool pluralizeTblName = true)
                                        : this(pluralTableName
                                              , prefixEntityNameToId
                                              , prefixAltTblNameToEntity
                                              , "Id"
                                              , schema
                                            , pluralizeTblName)
        {
        }

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        /// <param name="pluralTableName"></param>
        /// <param name="prefixAltTblNameToEntity"></param>
        /// <param name="preferredPKeyId"></param>
        /// <param name="schema"></param>
        /// <param name="pluralizeTblName"></param>
        protected ModelConfigurationBase(string pluralTableName = ""
                                        , bool prefixAltTblNameToEntity = false
                                        , string preferredPKeyId = "Id"
                                        , string schema = "dbo"
                                        , bool pluralizeTblName = true)
            : this(pluralTableName
                , false
                , prefixAltTblNameToEntity
                , preferredPKeyId
                , schema
                , pluralizeTblName)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pluralTableName"></param>
        /// <param name="prefixEntityNameToId"></param>
        /// <param name="prefixAltTblNameToEntity"></param>
        /// <param name="preferredPKeyId"></param>
        /// <param name="schema"></param>
        /// <param name="pluralizeTblName"></param>
        private ModelConfigurationBase(string pluralTableName = ""
                                        , bool prefixEntityNameToId = false
                                        , bool prefixAltTblNameToEntity = false
                                        , string preferredPKeyId = "Id"
                                        , string schema = "dbo"
                                        , bool pluralizeTblName = true)
        {
            Schema = schema;

            _altTableName = pluralTableName;
            _prefixEntityNameToId = prefixEntityNameToId;
            _prefixAltTblNameToEntity = prefixAltTblNameToEntity;
            _preferredPKeyId = preferredPKeyId;
            _pluralizeTblName = pluralizeTblName;
        }

        #endregion

        public void Configure(EntityTypeBuilder<TEntity> builder)
        {
            var tblName = string.IsNullOrEmpty(_altTableName) ? typeof(TEntity).Name : _altTableName;

            if (_prefixAltTblNameToEntity && !string.IsNullOrEmpty(_altTableName))
                tblName = tblName + typeof(TEntity).Name;

            var pKeyId = (_prefixEntityNameToId ? typeof(TEntity).Name : "") + _preferredPKeyId;

            builder
                //.ToTable(_schema + $"{(!string.IsNullOrEmpty(_schema) ? "." : "")}{(_pluralizeTblName ? tblName.Pluralize() : tblName)}", _schema)
                .ToTable($"{(_pluralizeTblName ? tblName.Pluralize() : tblName)}", Schema)
                .HasKey(i => i.Id);

            builder
                .Property(i => i.Id)
                .HasAnnotation("DatabaseGenerationOption", "Identity")
                .HasColumnName(pKeyId)
                .IsRequired();

            Map(builder);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TPk"></typeparam>
        /// <param name="entityBuilder"></param>
        /// <param name="conventionFieldsToIgnore"></param>
        private static void SetupConventions<T>(EntityTypeBuilder<TEntity> entityBuilder, Expression<Func<TEntity, object>>[] conventionFieldsToIgnore)
            where T : class, IIdentifiableEntity<TPk>
        {
            if (conventionFieldsToIgnore != null && conventionFieldsToIgnore.Any())
            {
                foreach (var exprssn in conventionFieldsToIgnore)
                {
                    entityBuilder.Ignore(exprssn);
                }
            }

            var fieldsIgnoreList = conventionFieldsToIgnore.ToListOfFields();
            // add the rest not ignored
            entityBuilder.RegisterConventionalField<TEntity>(fieldsIgnoreList, entity => entity.Ordinal)?
                .HasColumnName("ordinal").HasDefaultValue(0);

            entityBuilder.RegisterConventionalField<TEntity>(fieldsIgnoreList, entity => entity.IsActive)?
                .HasColumnName("isActive").HasDefaultValue(true); 

            entityBuilder.RegisterConventionalField<TEntity>(fieldsIgnoreList, entity => entity.DateCreated)?
                .HasColumnName("dateCreated").HasDefaultValue(DateTime.Now);

            entityBuilder.RegisterConventionalField<TEntity>(fieldsIgnoreList, entity => entity.DateRevised)?
                .HasColumnName("dateRevised");

            entityBuilder.RegisterConventionalField<TEntity>(fieldsIgnoreList, entity => entity.CreatedBy)?
                .HasColumnName("createdBy").IsRequired(false);

            entityBuilder.RegisterConventionalField<TEntity>(fieldsIgnoreList, entity => entity.RevisedBy)?
                .HasColumnName("revisedBy").IsRequired(false);
        }

    }

    public static class ModelConfigurationExts
    {
        public static PropertyBuilder<object> RegisterConventionalField<TEntity>(
            this EntityTypeBuilder<TEntity> entityBuilder
            , IList<string> fieldsIgnoreList
            , Expression<Func<TEntity, object>> selector)
            where TEntity : class, IEntity
        {
            var fNm = GetFieldNameFromSelector(selector);
            return fieldsIgnoreList != null && fieldsIgnoreList.Contains(fNm) ? null : entityBuilder.Property(selector);
        }

        // <summary>
        /// https://stackoverflow.com/a/12420603
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="fieldSelector"></param>
        /// <returns></returns>
        internal static string GetFieldNameFromSelector<TEntity>(Expression<Func<TEntity, object>> fieldSelector)
            where TEntity : class, IEntity
        {
            if (fieldSelector.Body is MemberExpression expression)
                return expression.Member.Name;
            else
            {
                var op = ((UnaryExpression)fieldSelector.Body).Operand;
                return ((MemberExpression)op).Member.Name;
            }
        }

    }
}