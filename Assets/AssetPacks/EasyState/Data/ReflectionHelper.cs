using System;
using System.Linq.Expressions;
using System.Reflection;

namespace EasyState.Data
{
    public static  class ReflectionHelper
    {
        /// <summary>
        /// Gets a strong typed delegate to a generated method that allows you to get the field value, that is represented
        /// by the given <paramref name="fieldInfo"/>. The delegate is instance independend, means that you pass the source 
        /// of the field as a parameter to the method and get back the value of it's field.
        /// </summary>
        /// <typeparam name="TSource">The reflecting type. This can be an interface that is implemented by the field's declaring type
        /// or an derrived type of the field's declaring type.</typeparam>
        /// <typeparam name="TValue">The type of the field value.</typeparam>
        /// <param name="fieldInfo">Provides the metadata of the field.</param>
        /// <returns>A strong typed delegeate that can be cached to get the field's value with high performance.</returns>
        public static Func<TSource, TValue> GetGetFieldDelegate<TSource, TValue>(FieldInfo fieldInfo)
        {
            //if using IL2CPP as a scripting backend replace this code with the following line
            // return x => (TValue)fieldInfo.GetValue(x);
            if (fieldInfo == null) throw new ArgumentNullException("fieldInfo");

            Type fieldDeclaringType = fieldInfo.DeclaringType;

            ParameterExpression sourceParameter =
             Expression.Parameter(typeof(TSource), "source");


            Expression sourceExpression = GetCastOrConvertExpression(
                sourceParameter, fieldDeclaringType);

            MemberExpression fieldExpression = Expression.Field(sourceExpression, fieldInfo);

            Expression resultExpression = GetCastOrConvertExpression(
            fieldExpression, typeof(TValue));

            LambdaExpression lambda =
                Expression.Lambda(typeof(Func<TSource, TValue>), resultExpression, sourceParameter);

            Func<TSource, TValue> compiled = (Func<TSource, TValue>)lambda.Compile();
            return compiled;
        }
        /// <summary>
        /// Gets an expression that can be assigned to the given target type. 
        /// Creates a new expression when a cast or conversion is required, 
        /// or returns the given <paramref name="expression"/> if no cast or conversion is required.
        /// </summary>
        /// <param name="expression">The expression which resulting value should be passed to a 
        /// parameter with a different type.</param>
        /// <param name="targetType">The target parameter type.</param>
        /// <returns>The <paramref name="expression"/> if no cast or conversion is required, 
        /// otherwise a new expression instance that wraps the the given <paramref name="expression"/> 
        /// inside the required cast or conversion.</returns>
        private static Expression GetCastOrConvertExpression(Expression expression, Type targetType)
        {
            Expression result;
            Type expressionType = expression.Type;

            // Check if a cast or conversion is required.
            if (targetType.IsAssignableFrom(expressionType))
            {
                result = expression;
            }
            else
            {
                // Check if we can use the as operator for casting or if we must use the convert method
                if (targetType.IsValueType && !IsNullableType(targetType))
                {
                    result = Expression.Convert(expression, targetType);
                }
                else
                {
                    result = Expression.TypeAs(expression, targetType);
                }
            }

            return result;
        }
        /// <summary>
        /// Determines whether the given type is a nullable type.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>true if the given type is a nullable type, otherwise false.</returns>
        public static bool IsNullableType(Type type)
        {
            if (type == null) throw new ArgumentNullException("type");

            bool result = false;
            if (type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                result = true;
            }

            return result;
        }
    }
}
