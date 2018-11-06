using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using SMEAppHouse.Core.CodeKits.Extensions;

namespace SMEAppHouse.Core.CodeKits.Helpers.Expressions
{
    /*

        USAGE:
        
        class Test
        {
            public string Foo { get; set; }
            public string Bar { get; set; }
            
            static void Main()
            {
                bool test1 = FuncTest<Test>.FuncEqual(x => x.Bar, y => y.Bar);
                bool test2 = FuncTest<Test>.FuncEqual(x => x.Foo, y => y.Bar);
            }
        }
    
    */

    /// <summary>
    /// http://stackoverflow.com/questions/283537/most-efficient-way-to-test-equality-of-lambda-expressions
    /// by: Marc Gravel
    /// </summary>
    public static class ExpressionsHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool FuncEqual<TSource, TValue>(
            Expression<Func<TSource, TValue>> x,
            Expression<Func<TSource, TValue>> y)
        {
            return ExpressionEqual(x, y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private static bool ExpressionEqual(Expression x, Expression y)
        {
            while (true)
            {
                // deal with the simple cases first...
                if (ReferenceEquals(x, y)) return true;
                if (x == null || y == null) return false;
                if (x.NodeType != y.NodeType || x.Type != y.Type) return false;

                switch (x.NodeType)
                {
                    case ExpressionType.Lambda:
                        x = ((LambdaExpression)x).Body;
                        y = ((LambdaExpression)y).Body;
                        continue;
                    case ExpressionType.MemberAccess:
                        MemberExpression mex = (MemberExpression)x, mey = (MemberExpression)y;
                        return mex.Member == mey.Member; // should really test down-stream expression
                    default:
                        throw new NotImplementedException(x.NodeType.ToString());
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static MemberExpression GetMemberExpression(MemberExpression expression)
        {
            try
            {
                if (expression != null)
                    return (MemberExpression)expression.Expression;
                else throw new InvalidOperationException($"{nameof(expression)} is null.");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static MemberExpression GetMemberExpression(UnaryExpression expression)
        {
            try
            {
                if (expression != null)
                    return (MemberExpression)expression.Operand;
                else throw new InvalidOperationException($"{nameof(expression)} is null.");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static MemberExpression GetMemberExpression<T>(Expression<Func<T>> expression)
        {
            try
            {
                if (expression.Body is MemberExpression)
                    return (MemberExpression)expression.Body;

                var op = ((UnaryExpression)expression.Body).Operand;
                return (MemberExpression)op;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static MemberExpression GetMemberExpression<TSource, TProperty>(Expression<Func<TSource, TProperty>> expression)
        {
            try
            {
                if (expression.Body is MemberExpression)
                    return (MemberExpression)expression.Body;

                var op = ((UnaryExpression)expression.Body).Operand;
                return (MemberExpression)op;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static bool TypeHasProperty<TSource, TProperty>(Expression<Func<TSource, TProperty>> accessor)
        {
            try
            {
                var memberExpr = GetMemberExpression(accessor);
                var propertyName = memberExpr.Member.Name; //((MemberExpression)accessor.Body).Member.Name;
                var properties = typeof(TSource).GetProperties();
                return properties.Any(a => a.Name == propertyName);
            }
            catch (Exception exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static Type GetType<T>(Expression<Func<T>> expression)
        {
            //get the returned type
            if ((expression.Body.NodeType != ExpressionType.Convert) &&
                (expression.Body.NodeType != ExpressionType.ConvertChecked))
                return expression.Body.Type;

            var unary = expression.Body as UnaryExpression;

            return unary?.Operand.Type ?? expression.Body.Type;
        }

        public static string GetName<T>(Expression<Func<T>> expression)
        {
            try
            {
                var memberExpr = GetMemberExpression(expression);
                return memberExpr.Member.Name;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static object GetPropertyValue<T>(Expression<Func<T>> expression)
        {
            Type returnedType = null;
            return GetPropertyValue(expression, ref returnedType);
        }

        public static object GetPropertyValue<T>(Expression<Func<T>> expression, ref Type returnedType)
        {
            var propertyName = string.Empty;
            return GetPropertyValue(expression, ref returnedType, ref propertyName);
        }

        public static object GetPropertyValue<T>(Expression<Func<T>> expression, ref Type returnedType, ref string propertyName)
        {
            try
            {
                var member = (MemberExpression)expression.Body;
                var value = expression.Compile()();

                propertyName = member.Member.Name;

                //get the returned type
                if ((expression.Body.NodeType == ExpressionType.Convert)
                    || (expression.Body.NodeType == ExpressionType.ConvertChecked))
                {
                    var unary = expression.Body as UnaryExpression;
                    if (unary != null)
                        returnedType = unary.Operand.Type;
                }
                else returnedType = expression.Body.Type;

                return value;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        public static object GetPropertyValue<T>(T item) where T : class
        {
            var propGetter = typeof(T).GetProperties()[0].GetGetMethod();
            var value = propGetter.Invoke(item, null);

            return value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <param name="memberLamda"></param>
        /// <param name="value"></param>
        public static void SetPropertyValue<T>(this T target, Expression<Func<T, object>> memberLamda, object value)
        {
            var memberSelectorExpression = memberLamda.Body as MemberExpression;
            if (memberSelectorExpression == null) return;
            var property = memberSelectorExpression.Member as PropertyInfo;
            property?.SetValue(target, value, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="e"></param>
        /// <param name="resultAction"></param>
        public static void PrintPropertyAndObject<T>(Expression<Func<T>> e, Action<string, object> resultAction)
        {
            var member = (MemberExpression)e.Body;
            var strExpr = member.Expression;  //Expression object

            if (strExpr.Type == typeof(string))
            {
                var str = Expression.Lambda<Func<string>>(strExpr).Compile()();
                Console.WriteLine("String: {0}", str);
            }
            var propertyName = member.Member.Name;
            var value = e.Compile()();

            resultAction?.Invoke(propertyName, value);

            //Console.WriteLine("{0} : {1}", propertyName, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="source"></param>
        /// <param name="property"></param>
        /// <param name="methodName"></param>
        /// <returns></returns>
        public static IOrderedQueryable<TEntity> ApplyOrder<TEntity>(this IQueryable<TEntity> source, string property, SortMethodEnum methodName)
        {
            var props = property.Split('.');
            var type = typeof(TEntity);
            var arg = Expression.Parameter(type, "x");

            Expression expr = arg;

            foreach (var prop in props)
            {
                // use reflection (not ComponentModel) to mirror LINQ
                var pi = type.GetProperty(prop);
                expr = Expression.Property(expr, pi);
                type = pi.PropertyType;
            }

            var delegateType = typeof(Func<,>).MakeGenericType(typeof(TEntity), type);
            var lambda = Expression.Lambda(delegateType, expr, arg);
            var result = typeof(Queryable).GetMethods().Single(
                    method => method.Name == methodName.GetDescription()
                            && method.IsGenericMethodDefinition
                            && method.GetGenericArguments().Length == 2
                            && method.GetParameters().Length == 2)
                    .MakeGenericMethod(typeof(TEntity), type)
                    .Invoke(null, new object[] { source, lambda });
            return (IOrderedQueryable<TEntity>)result;
        }
    }

    /// <summary>
    /// This only exists to make it easier to call, i.e. so that I can use FuncTest<T> with
    /// generic-type-inference; if you use the doubly-generic method, you need to specify
    /// both arguments, which is a pain...
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    public static class ExpressionsHelper<TSource>
    {
        public static bool FuncEqual<TValue>(
            Expression<Func<TSource, TValue>> x,
            Expression<Func<TSource, TValue>> y)
        {
            return ExpressionsHelper.FuncEqual(x, y);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public enum SortMethodEnum
    {
        [Description("OrderBy")]
        OrderBy,
        [Description("OrderByDescending")]
        OrderByDescending,
        [Description("ThenBy")]
        ThenBy,
        [Description("ThenByDescending")]
        ThenByDescending,
    }
}
