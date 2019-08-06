using System;
using System.Linq.Expressions;
using System.Reflection;

namespace metering
{
    /// <summary>
    /// A helper for expressions
    /// </summary>
    public static class ExpressionHelpers
    {
        /// <summary>
        /// Complies an expression and gets the functions return value
        /// </summary>
        /// <typeparam name="T">Type of the return value</typeparam>
        /// <param name="lambda">expression to compile</param>
        /// <returns></returns>
        public static T GetPropertyValue<T>(this Expression<Func<T>> lambda)
        {
            return lambda.Compile().Invoke();
        }

        /// <summary>
        /// Set the underlying properties value to the given value 
        /// from an expression that contains the property
        /// </summary>
        /// <typeparam name="T">The type of valie to set</typeparam>
        /// <param name="lambda">the expression</param>
        /// <param name="value">the value to set the property to</param>
        public static void SetPropertyValue<T>(this Expression<Func<T>> lambda, T value)
        {
            // converts a lambda () => some.Property to to some.Property
            var expression = (lambda as LambdaExpression).Body as MemberExpression;

            // get the property information to set it
            var propertyInfo = (PropertyInfo)expression.Member;

            // the class that the member is on
            var target = Expression.Lambda(expression.Expression).Compile().DynamicInvoke();

            // set property value
            propertyInfo.SetValue(target, value);
        }
    }
}
