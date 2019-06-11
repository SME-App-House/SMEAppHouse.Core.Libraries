using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SMEAppHouse.Core.Patterns.EF.ModelComposite;

namespace SMEAppHouse.Core.Patterns.EF.StrategyForModelCfg
{
    public interface IModelConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
        where TEntity : class, IEntityBase
    {
        string Schema { get; }
        Expression<Func<TEntity, object>>[] FieldsToIgnore { get; set; }
        void Map(EntityTypeBuilder<TEntity> entityBuilder);
    }
}