using System;
using System.Linq.Expressions;

namespace SMEAppHouse.Core.CodeKits.Helpers.Expressions
{
    public sealed class EntityCastRemoverVisitor<TEntity> : ExpressionVisitor where TEntity : class, new()
    {
        public static Expression<Func<T, bool>> Convert<T>(
            Expression<Func<T, bool>> predicate)
        {
            var visitor = new EntityCastRemoverVisitor<TEntity>();

            var visitedExpression = visitor.Visit(predicate);

            return (Expression<Func<T, bool>>)visitedExpression;
        }

        protected override Expression VisitUnary(UnaryExpression node)
        {
            if (node.NodeType == ExpressionType.Convert && node.Type == typeof(TEntity))
            {
                return node.Operand;
            }

            return base.VisitUnary(node);
        }
    }
}
