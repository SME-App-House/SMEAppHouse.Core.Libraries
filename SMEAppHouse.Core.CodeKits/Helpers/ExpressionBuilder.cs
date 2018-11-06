using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Newtonsoft.Json;

namespace SMEAppHouse.Core.CodeKits.Helpers
{
    public class ExpressionBuilder
    {
        public enum OperatorComparer
        {
            Contains,
            StartsWith,
            EndsWith,
            Equals = ExpressionType.Equal,
            GreaterThan = ExpressionType.GreaterThan,
            GreaterThanOrEqual = ExpressionType.GreaterThanOrEqual,
            LessThan = ExpressionType.LessThan,
            LessThanOrEqualTo = ExpressionType.LessThanOrEqual,
            NotEqual = ExpressionType.NotEqual
        }

        // Define some of our default filtering options
        private static readonly MethodInfo ContainsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
        private static readonly MethodInfo StartsWithMethod = typeof(string).GetMethod("StartsWith", new[] { typeof(string) });
        private static readonly MethodInfo EndsWithMethod = typeof(string).GetMethod("EndsWith", new[] { typeof(string) });

        public static Expression<Func<T, bool>> BuildPredicate<T>(object test, OperatorComparer comparer, params string[] properties)
        {
            var parameterExpression = Expression.Parameter(typeof(T), typeof(T).Name);
            return (Expression<Func<T, bool>>)BuildNavigationExpression(parameterExpression, comparer, test, properties);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filters"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> GetExpression<T>(List<Filter> filters)
        {
            // No filters passed in #KickIT
            if (filters.Count == 0)
                return null;

            // Create the parameter for the ObjectType (typically the 'x' in your expression (x => 'x')
            // The "parm" string is used strictly for debugging purposes
            var param = Expression.Parameter(typeof(T), "parm");

            // Store the result of a calculated Expression
            Expression exp = null;

            if (filters.Count == 1)
                exp = GetExpression<T>(param, filters[0]); // Create expression from a single instance
            else if (filters.Count == 2)
                exp = GetExpression<T>(param, filters[0], filters[1]); // Create expression that utilizes AndAlso mentality
            else
            {
                // Loop through filters until we have created an expression for each
                while (filters.Count > 0)
                {
                    // Grab initial filters remaining in our List
                    var f1 = filters[0];
                    var f2 = filters[1];

                    // Check if we have already set our Expression
                    exp = exp == null ?
                        GetExpression<T>(param, filters[0], filters[1]) :
                        Expression.AndAlso(exp, GetExpression<T>(param, filters[0], filters[1]));

                    filters.Remove(f1);
                    filters.Remove(f2);

                    // Odd number, handle this seperately
                    if (filters.Count == 1)
                    {
                        // Pass in our existing expression and our newly created expression from our last remaining filter
                        exp = Expression.AndAlso(exp, GetExpression<T>(param, filters[0]));

                        // Remove filter to break out of while loop
                        filters.RemoveAt(0);
                    }
                }
            }

            return exp == null ? null
                : Expression.Lambda<Func<T, bool>>(exp, param);
        }

        private static Expression BuildNavigationExpression(Expression parameter, OperatorComparer comparer, object value, params string[] properties)
        {
            Expression resultExpression;
            Type childType = null;

            if (properties.Count() > 1)
            {
                //build path
                parameter = Expression.Property(parameter, properties[0]);
                var isCollection = typeof(IEnumerable).IsAssignableFrom(parameter.Type);
                //if it´s a collection we later need to use the predicate in the methodexpressioncall
                Expression childParameter;
                if (isCollection)
                {
                    childType = parameter.Type.GetGenericArguments()[0];
                    childParameter = Expression.Parameter(childType, childType.Name);
                }
                else
                {
                    childParameter = parameter;
                }
                //skip current property and get navigation property expression recursivly
                var innerProperties = properties.Skip(1).ToArray();
                var predicate = BuildNavigationExpression(childParameter, comparer, value, innerProperties);
                resultExpression = isCollection ? BuildSubQuery(parameter, childType, predicate) : predicate;
            }
            else
            {
                //build final predicate
                resultExpression = BuildCondition(parameter, properties[0], comparer, value);
            }
            return resultExpression;
        }

        private static Expression BuildSubQuery(Expression parameter, Type childType, Expression predicate)
        {
            var anyMethod = typeof(Enumerable).GetMethods().Single(m => m.Name == "Any" && m.GetParameters().Length == 2);
            anyMethod = anyMethod.MakeGenericMethod(childType);
            predicate = Expression.Call(anyMethod, parameter, predicate);
            return MakeLambda(parameter, predicate);
        }

        private static Expression BuildCondition(Expression parameter, string property, OperatorComparer comparer, object value)
        {
            var childProperty = parameter.Type.GetProperty(property);
            var left = Expression.Property(parameter, childProperty);
            var right = Expression.Constant(value);
            var predicate = BuildComparsion(left, comparer, right);
            return MakeLambda(parameter, predicate);
        }

        private static Expression BuildComparsion(Expression left, OperatorComparer comparer, Expression right)
        {
            var mask = new List<OperatorComparer>{
                OperatorComparer.Contains,
                OperatorComparer.StartsWith,
                OperatorComparer.EndsWith
            };
            if (mask.Contains(comparer) && left.Type != typeof(string))
            {
                comparer = OperatorComparer.Equals;
            }
            if (!mask.Contains(comparer))
            {
                return Expression.MakeBinary((ExpressionType)comparer, left, Expression.Convert(right, left.Type));
            }
            return BuildStringCondition(left, comparer, right);
        }

        private static Expression BuildStringCondition(Expression left, OperatorComparer comparer, Expression right)
        {
            var compareMethod = typeof(string).GetMethods().Single(m => m.Name.Equals(Enum.GetName(typeof(OperatorComparer), comparer)) && m.GetParameters().Count() == 1);
            //we assume ignoreCase, so call ToLower on paramter and memberexpression
            var toLowerMethod = typeof(string).GetMethods().Single(m => m.Name.Equals("ToLower") && m.GetParameters().Count() == 0);
            left = Expression.Call(left, toLowerMethod);
            right = Expression.Call(right, toLowerMethod);
            return Expression.Call(left, compareMethod, right);
        }

        private static Expression MakeLambda(Expression parameter, Expression predicate)
        {
            var resultParameterVisitor = new ParameterVisitor();
            resultParameterVisitor.Visit(parameter);
            var resultParameter = resultParameterVisitor.Parameter;
            return Expression.Lambda(predicate, (ParameterExpression)resultParameter);
        }

        private static Expression GetExpression<T>(ParameterExpression param, Filter filter)
        {
            // The member you want to evaluate (x => x.FirstName)
            var member = Expression.Property(param, filter.PropertyName);

            // The value you want to evaluate
            var constant = Expression.Constant(filter.Value);


            // Determine how we want to apply the expression
            switch (filter.Operator)
            {
                case OperatorComparer.Equals:
                    return Expression.Equal(member, constant);

                case OperatorComparer.Contains:
                    return Expression.Call(member, ContainsMethod, constant);

                case OperatorComparer.GreaterThan:
                    return Expression.GreaterThan(member, constant);

                case OperatorComparer.GreaterThanOrEqual:
                    return Expression.GreaterThanOrEqual(member, constant);

                case OperatorComparer.LessThan:
                    return Expression.LessThan(member, constant);

                case OperatorComparer.LessThanOrEqualTo:
                    return Expression.LessThanOrEqual(member, constant);

                case OperatorComparer.StartsWith:
                    return Expression.Call(member, StartsWithMethod, constant);

                case OperatorComparer.EndsWith:
                    return Expression.Call(member, EndsWithMethod, constant);
            }

            return null;
        }

        private static BinaryExpression GetExpression<T>(ParameterExpression param, Filter filter1, Filter filter2)
        {
            var result1 = GetExpression<T>(param, filter1);
            var result2 = GetExpression<T>(param, filter2);
            return Expression.AndAlso(result1, result2);
        }

        private class ParameterVisitor : ExpressionVisitor
        {
            public Expression Parameter
            {
                get;
                private set;
            }
            protected override Expression VisitParameter(ParameterExpression node)
            {
                Parameter = node;
                return node;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public class Filter
        {
            public string PropertyName { get; set; }
            public string Value { get; set; }
            public Type DataType { get; set; }
            public OperatorComparer Operator { get; set; } = OperatorComparer.Contains;

            /*
                var members = arg.Split(':');
                var key = members[0];
                var valRaw = members[1];

                var type = valRaw.Split('[', ']')[1].Trim();
                var value = valRaw.Replace(type, "").Replace("[", "").Replace("]", "").Trim();

                this.Add(key, value);
            */

            /// <summary>
            /// 
            /// </summary>
            public class Filters : List<Filter>
            {
                /// <summary>
                /// 
                /// </summary>
                /// <param name="filtersjson"></param>
                public Filters(string filtersjson)
                {
                    var keyFieldValueFiltersDic = JsonConvert.DeserializeObject<IDictionary<string, string>>(filtersjson);
                    foreach (var arg in keyFieldValueFiltersDic)
                    {
                        var valRaw = arg.Value;
                        var type = valRaw.Split('[', ']')[1].Trim();
                        var value = valRaw.Replace(type, "").Replace("[", "").Replace("]", "").Trim();

                        this.Add(arg.Key, value);
                    }
                }

                /// <summary>
                /// 
                /// </summary>
                /// <param name="filters"></param>
                public Filters(params string[] filters)
                {
                    foreach (var arg in filters)
                    {

                        var members = arg.Split(':');
                        var key = members[0];
                        var valRaw = members[1];

                        var type = valRaw.Split('[', ']')[1].Trim();
                        var value = valRaw.Replace(type, "").Replace("[", "").Replace("]", "").Trim();

                        this.Add(key, value);
                    }
                }

                /// <summary>
                /// 
                /// </summary>
                /// <param name="filters"></param>
                public Filters(IDictionary<string, string> filters)
                {
                    foreach (var f in filters)
                    {
                        var type = f.Value.Split('[', ']')[1].Trim();
                        var value = f.Value.Replace(type, "").Replace("[", "").Replace("]", "").Trim();

                        this.Add(f.Key, value);
                    }
                }




                /// <summary>
                /// 
                /// </summary>
                /// <param name="name"></param>
                /// <param name="value"></param>
                public void Add(string name, string value = "")
                {
                    this.Add(new Filter() { PropertyName = name, Value = value });
                }
            }
        }
    }
}