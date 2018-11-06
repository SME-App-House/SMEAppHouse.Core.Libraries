using System;
using BNS.Core.EntityAdapter.GenericEntity;
using BNS.Core.EntityAdapter.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BNS.Core.EntityAdapter.DBEntityStrategy
{
    public class ModelConfigurationBaseV2<TEntity, TPk> : DbEntityConfiguration<TEntity>
        where TEntity : class, IIdentifiableEntity<TPk>
        where TPk : struct
    {
        private readonly string _alternativeTableName; // {APPENDDEFAULT}+{PLURALIZE}
        private readonly string _alternativePKeyId; // {APPENDDEFAULT}
        private readonly bool _pluralizeTableName = true;
        private readonly string _schema;

        #region constructors

        public ModelConfigurationBaseV2()
            : this("", "Id", true, "dbo")
        {
        }

        public ModelConfigurationBaseV2(string schema = "dbo")
            : this("", "Id", true, schema)
        {
        }

        public ModelConfigurationBaseV2(string alternativeTableName = ""
                                        , string alternativePKeyId = "Id"
                                        , bool pluralizeTableName = true
                                        , string schema = "dbo")
        {
            _alternativeTableName = alternativeTableName;
            _pluralizeTableName = pluralizeTableName;
            _alternativePKeyId = alternativePKeyId;
            _schema = schema;
        }

        #endregion

        public override void Map(EntityTypeBuilder<TEntity> builder)
        {
            var tblName = typeof(TEntity).Name;
            tblName = (_pluralizeTableName ? tblName.Pluralize() : tblName);
            if (!string.IsNullOrEmpty(_alternativeTableName))
            {
                if (_alternativeTableName.ToUpper().Contains("{PRE}") &&
                   _alternativeTableName.ToUpper().Contains("{SUF}"))
                    throw new Exception("Either only prefix or suffix tag can be acceptable");

                if (_alternativeTableName.ToUpper().Contains("{PRE}"))
                    tblName = _alternativeTableName + tblName;
                else if (_alternativeTableName.ToUpper().Contains("{SUF}"))
                    tblName = tblName + _alternativeTableName;

                tblName = tblName.Replace("{PRE}", "").Replace("{SUF}", "");
            }

            var pKeyId = _alternativePKeyId;

            if (!pKeyId.Contains("{PRE}") ||
                !pKeyId.Contains("{SUF}") ||
                !pKeyId.Contains("{DEF}"))
            {
            }

            if (!string.IsNullOrEmpty(pKeyId) && !pKeyId
                                                    .ToUpper()
                                                    .Replace("{PRE}", "")
                                                    .Replace("{SUF}", "")
                                                    .Replace("{DEF}", "")
                                                    .Trim()
                                                    .Equals("ID"))
            {
                if (_alternativePKeyId.ToUpper().Contains("{PRE}") &&
                    _alternativePKeyId.ToUpper().Contains("{SUF}"))
                    throw new Exception("Either only prefix or suffix tag can be acceptable");

                if (_alternativePKeyId.ToUpper().Contains("{PRE}"))
                {
                    if (_alternativePKeyId.ToUpper().Contains("{DEF}"))
                        pKeyId = _alternativePKeyId.Replace("{DEF}", typeof(TEntity).Name) + "Id";
                    else
                        pKeyId = _alternativePKeyId + "Id";
                }
                else if (_alternativePKeyId.ToUpper().Contains("{SUF}"))
                {
                    if (_alternativePKeyId.ToUpper().Contains("{DEF}"))
                        pKeyId = "Id"+_alternativePKeyId.Replace("{DEF}", typeof(TEntity).Name);
                    else
                        pKeyId = "Id" + _alternativePKeyId;
                }
                pKeyId = pKeyId.Replace("{PRE}", "").Replace("{SUF}", "").Replace("{DEF}", "");
            }
            
            builder
                .ToTable(_schema + $"{(!string.IsNullOrEmpty(_schema) ? "." : "")}{tblName}")
                .HasKey(i => i.Id);

            builder
                .Property(i => i.Id)
                .HasAnnotation("DatabaseGenerationOption", "Identity")
                .HasColumnName(pKeyId)
                .IsRequired();

            builder.Property(x => x.Ordinal).HasColumnName("ordinal").HasDefaultValue(0);
            builder.Property(x => x.IsActive).HasColumnName("isActive");
            builder.Property(x => x.DateCreated).HasColumnName("dateCreated").HasDefaultValue(DateTime.Now);
            builder.Property(x => x.DateRevised).HasColumnName("dateRevised").HasDefaultValue(DateTime.Now);
            builder.Property(x => x.CreatedBy).HasColumnName("createdBy").IsRequired(false);
            builder.Property(x => x.RevisedBy).HasColumnName("revisedBy").IsRequired(false);
        }

    }
}