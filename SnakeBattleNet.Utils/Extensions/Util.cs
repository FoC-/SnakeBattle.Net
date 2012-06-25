using System;
using System.Linq.Expressions;
using System.Reflection;

namespace SnakeBattleNet.Utils.Extensions
{
    public static class Util
    {
        public static string GetElementNameFor<TSource, TProperty>(Expression<Func<TSource, TProperty>> propertyLambda)
        {
            var type = typeof(TSource);

            var memberExpression = propertyLambda.Body as MemberExpression;
            if (memberExpression == null && propertyLambda.Body is UnaryExpression)
            {
                var unaryExpression = (UnaryExpression)propertyLambda.Body;
                memberExpression = unaryExpression.Operand as MemberExpression;
            }
            memberExpression.EnsureNotNull("memberExpression");

            var propInfo = memberExpression.Member as PropertyInfo;
            propInfo.EnsureNotNull("propInfo");

            if (type != propInfo.ReflectedType && !type.IsSubclassOf(propInfo.ReflectedType))
                throw new ArgumentException("Expression '{0}' refers to a property that is not from type {1}.".F(propertyLambda, type));

            return propInfo.Name;
        }

        public static string GetElementNameFor<TSource>(Expression<Func<TSource, object>> propertyLambda)
        {
            return GetElementNameFor<TSource, object>(propertyLambda);
        }
    }
}