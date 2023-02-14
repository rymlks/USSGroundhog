using EasyState.Models.DataType.DataTypeConditions;
using System;

namespace EasyState.Models
{
    public static class DataTypeConditionExtensions
    {
        public static IDataTypeCondition<T> Custom<T, TProperty>(this IConditionBuilder<T, TProperty> builder, Func<TProperty, bool> propertyComparisonFunc) where T : DataTypeBase
        {
            builder.DettachFromSet();

            return new CustomCondition<T, TProperty>(builder.Name, builder.ValueFunction, builder.Set, propertyComparisonFunc);
        }

        public static IDataTypeCondition<T> Custom<T, TProperty>(this IConditionBuilder<T, TProperty> builder, Func<T, TProperty, bool> contextComparisonFunc) where T : DataTypeBase
        {
            builder.DettachFromSet();

            return new CustomCondition<T, TProperty>(builder.Name, builder.ValueFunction, builder.Set, contextComparisonFunc);
        }

        public static IDataTypeCondition<T> EqualTo<T, TProperty>(this IConditionBuilder<T, TProperty> builder, Func<T, TProperty> valueToCompare) where TProperty : IComparable<TProperty>, IComparable where T : DataTypeBase
        {
            builder.DettachFromSet();
            return new EqualToCondition<T, TProperty>(builder.Name, builder.ValueFunction, valueToCompare, builder.Set);
        }

        public static IDataTypeCondition<T> EqualTo<T, TProperty>(this IConditionBuilder<T, TProperty> builder, TProperty valueToCompare) where TProperty : IComparable<TProperty>, IComparable where T : DataTypeBase
        {
            builder.DettachFromSet();
            return new EqualToCondition<T, TProperty>(builder.Name, builder.ValueFunction, valueToCompare, builder.Set);
        }

        public static IDataTypeCondition<T> GreaterThan<T, TProperty>(this IConditionBuilder<T, TProperty> builder, Func<T, TProperty> valueToCompare) where TProperty : IComparable<TProperty>, IComparable where T : DataTypeBase
        {
            builder.DettachFromSet();
            return new GreaterThanCondition<T, TProperty>(builder.Name, builder.ValueFunction, valueToCompare, builder.Set);
        }

        public static IDataTypeCondition<T> GreaterThan<T, TProperty>(this IConditionBuilder<T, TProperty> builder, TProperty valueToCompare) where TProperty : IComparable<TProperty>, IComparable where T : DataTypeBase
        {
            builder.DettachFromSet();
            return new GreaterThanCondition<T, TProperty>(builder.Name, builder.ValueFunction, valueToCompare, builder.Set);
        }

        public static IDataTypeCondition<T> GreaterThanOrEqualTo<T, TProperty>(this IConditionBuilder<T, TProperty> builder, Func<T, TProperty> valueToCompare) where TProperty : IComparable<TProperty>, IComparable where T : DataTypeBase
        {
            builder.DettachFromSet();
            return new GreaterThanEqualToCondition<T, TProperty>(builder.Name, builder.ValueFunction, valueToCompare, builder.Set);
        }

        public static IDataTypeCondition<T> GreaterThanOrEqualTo<T, TProperty>(this IConditionBuilder<T, TProperty> builder, TProperty valueToCompare) where TProperty : IComparable<TProperty>, IComparable where T : DataTypeBase
        {
            builder.DettachFromSet();
            return new GreaterThanEqualToCondition<T, TProperty>(builder.Name, builder.ValueFunction, valueToCompare, builder.Set);
        }

        public static IDataTypeCondition<T> IsNotNull<T, TProperty>(this IConditionBuilder<T, TProperty> builder) where T : DataTypeBase where TProperty : class
        {
            builder.DettachFromSet();

            return new IsNotNullCondition<T, TProperty>(builder.Name, builder.ValueFunction, builder.Set);
        }

        public static IDataTypeCondition<T> IsNull<T, TProperty>(this IConditionBuilder<T, TProperty> builder) where T : DataTypeBase where TProperty : class
        {
            builder.DettachFromSet();

            return new IsNullCondition<T, TProperty>(builder.Name, builder.ValueFunction, builder.Set);
        }

        public static IDataTypeCondition<T> LessThan<T, TProperty>(this IConditionBuilder<T, TProperty> builder, Func<T, TProperty> valueToCompare) where TProperty : IComparable<TProperty>, IComparable where T : DataTypeBase
        {
            builder.DettachFromSet();
            return new LessThanCondition<T, TProperty>(builder.Name, builder.ValueFunction, valueToCompare, builder.Set);
        }

        public static IDataTypeCondition<T> LessThan<T, TProperty>(this IConditionBuilder<T, TProperty> builder, TProperty valueToCompare) where TProperty : IComparable<TProperty>, IComparable where T : DataTypeBase
        {
            builder.DettachFromSet();
            return new LessThanCondition<T, TProperty>(builder.Name, builder.ValueFunction, valueToCompare, builder.Set);
        }

        public static IDataTypeCondition<T> LessThanOrEqualTo<T, TProperty>(this IConditionBuilder<T, TProperty> builder, Func<T, TProperty> valueToCompare) where TProperty : IComparable<TProperty>, IComparable where T : DataTypeBase
        {
            builder.DettachFromSet();
            return new LessThanEqaulToCondition<T, TProperty>(builder.Name, builder.ValueFunction, valueToCompare, builder.Set);
        }

        public static IDataTypeCondition<T> LessThanOrEqualTo<T, TProperty>(this IConditionBuilder<T, TProperty> builder, TProperty valueToCompare) where TProperty : IComparable<TProperty>, IComparable where T : DataTypeBase
        {
            builder.DettachFromSet();
            return new LessThanEqaulToCondition<T, TProperty>(builder.Name, builder.ValueFunction, valueToCompare, builder.Set);
        }
        public static IDataTypeCondition<T> IsTrue<T>(this IConditionBuilder<T, bool> builder) where T : DataTypeBase
        {
            builder.DettachFromSet();
            return new IsTrueCondition<T>(builder.Name, builder.ValueFunction, builder.Set);
        }
        public static IDataTypeCondition<T> IsFalse<T>(this IConditionBuilder<T, bool> builder) where T : DataTypeBase
        {
            builder.DettachFromSet();
            return new IsFalseCondition<T>(builder.Name, builder.ValueFunction, builder.Set);
        }
    }
}