using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace EasyState.Core.Validators
{
    public static class ValidationExtension
    {
        public static ValidationModel<TModel> Require<TModel>(this TModel model)
        {
            return new ValidationModel<TModel>(model);
        }

        public static ValidationModelList<TModel> Require<TModel>(this IEnumerable<TModel> models)
        {
            return new ValidationModelList<TModel>(models);
        }

        public static void RequireString(this List<string> errors, string value, string memberName)
        {
            if (string.IsNullOrEmpty(value))
            {
                errors.Add($"{memberName} is a required field.");
            }
        }
       
        public static void String<TModel>(this ValidationModel<TModel> model, List<string> errors, params Expression<Func<TModel, string>>[] requiredStrings)
        {
            foreach (var item in requiredStrings)
            {
                var value = item.Compile().Invoke(model);
                if (string.IsNullOrEmpty(value))
                {
                    errors.Add($"{GetDisplayName(item)} is a required field.");
                }
            }
        }

        public static void Unique<TModel>(this ValidationModelList<TModel> models, List<string> errors, Expression<Func<TModel, string>> uniqueField, string valueToCompare)
        {
            var readVal = uniqueField.Compile();
            if (models.Models.Any(x => valueToCompare.Equals(readVal(x), StringComparison.OrdinalIgnoreCase)))
            {
                errors.Add($"{GetDisplayName(uniqueField)} must be unique.");
            }
        }
        
        private static string GetDisplayName(LambdaExpression exp)
        {
            var memExp = GetMemberExpression(exp);
            return GetDisplayNameFromExpression(memExp);
        }

        private static string GetDisplayNameFromExpression(MemberExpression exp)
        {
            string displayName = exp.Member.Name;
            string[] split = Regex.Split(displayName, @"(?<!^)(?=[A-Z])");
            displayName = string.Join(" ", split);

            return displayName;
        }

        private static MemberExpression GetMemberExpression(LambdaExpression exp)
        {
            switch (exp.Body)
            {
                case MemberExpression m:
                    return m;

                case UnaryExpression u when u.Operand is MemberExpression m:
                    return m;

                default:
                    throw new NotImplementedException(exp.Body.GetType().ToString());
            }
        }
    }

    public class ValidationModel<TModel>
    {
        private readonly TModel _model;

        public ValidationModel(TModel model)
        {
            _model = model;
        }

        public static implicit operator TModel(ValidationModel<TModel> validationModel) => validationModel._model;
    }

    public class ValidationModelList<TModel>
    {
        public readonly IEnumerable<TModel> Models;

        public ValidationModelList(IEnumerable<TModel> models)
        {
            Models = models;
        }
    }
}