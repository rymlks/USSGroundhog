using EasyState.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace EasyState.Editor.Renderers.DetailRenderers.FieldRenderers
{
    public class PropertyCollection<TModel> : IDisposable where TModel : Model
    {
        public readonly VisualElement Container;
        public readonly TModel Model;
        private Dictionary<int, object> _cachedSetters = new Dictionary<int, object>();
        private List<IDisposable> _disposables = new List<IDisposable>();

        public PropertyCollection(VisualElement container, TModel model)
        {
            Container = container;
            Model = model;
        }

        public void AddColorProperty(Expression<Func<TModel, Color>> propExp, string elementID)
        {
            _disposables.Add(
                    new PropertyField<ColorField, Color>(x =>
                    {
                        SetPropertyValue(propExp, x);
                        Model.Refresh();
                    }, elementID, Container, (Color)GetPropertyValue(propExp))

                );
        }
        public void AddCheckProperty(Expression<Func<TModel, bool>> propExp, string elementID)
        {
            _disposables.Add(
                    new PropertyField<Toggle, bool>(x =>
                    {
                        SetPropertyValue(propExp, x);
                        Model.Refresh();
                    }, elementID, Container, (bool)GetPropertyValue(propExp))

                );
        }

        public void AddEnumProperty<TEnum>(Expression<Func<TModel, TEnum>> propExp, string elementID, bool isReadOnly = false, Action refreshFunc = null) where TEnum : Enum
        {
            _disposables.Add(
                    new EnumPropertyField<TEnum>(x =>
                                                {
                                                    SetPropertyValue(propExp, x);
                                                    Model.Refresh();
                                                }, 
                                                elementID,
                                                Container,
                                                (TEnum)GetPropertyValue(propExp),
                                                isReadOnly,
                                                refreshFunc
                                                )

                );
        }

        public void AddTextProperty(Expression<Func<TModel, string>> propExp, string elementID)
        {
            _disposables.Add(
                  new PropertyField<TextField, string>(x =>
                  {
                      SetPropertyValue(propExp, x);
                      Model.Refresh();
                  }, elementID, Container, (string)GetPropertyValue(propExp))

              );
        }

        public void Dispose()
        {
            foreach (var item in _disposables)
            {
                item.Dispose();
            }
        }

        private PropertyInfo GetPropertyInfoFromExpression<TValueType>(Expression<Func<TModel, TValueType>> lambda)
        {
            PropertyInfo propertyInfo;
            if (lambda.Body is UnaryExpression)
            {
                var expression = (UnaryExpression)lambda.Body;
                var member = (MemberExpression)expression.Operand;
                propertyInfo = (PropertyInfo)member.Member;
            }
            else
            {
                var memberExpression = (MemberExpression)lambda.Body;
                propertyInfo = (PropertyInfo)memberExpression.Member;
            }
            return propertyInfo;
        }

        private object GetPropertyValue<TValueType>(Expression<Func<TModel, TValueType>> lambda)
        {
            var propertyInfo = GetPropertyInfoFromExpression(lambda);
            return propertyInfo.GetValue(Model);
        }

        private void SetPropertyValue<TValueType>(Expression<Func<TModel, TValueType>> lambda, TValueType value)
        {
            if (_cachedSetters.TryGetValue(lambda.GetHashCode(), out object cachedSetter))
            {
                var castSetter = cachedSetter as Action<TModel, TValueType>;
                castSetter.Invoke(Model, value);
                return;
            }

            var parameter1 = Expression.Parameter(typeof(TModel));
            var parameter2 = Expression.Parameter(typeof(TValueType));

            // turning an expression body into a PropertyInfo is common enough
            // that it's a good idea to extract this to a reusable method

            var propertyInfo = GetPropertyInfoFromExpression(lambda);

            // use the PropertyInfo to make a property expression
            // for the first parameter (the object)
            var property = Expression.Property(parameter1, propertyInfo);

            // assignment expression that assigns the second parameter (value) to the property
            var assignment = Expression.Assign(property, parameter2);

            // then just build the lambda, which takes 2 parameters, and has the assignment
            // expression for its body
            var setter = Expression.Lambda<Action<TModel, TValueType>>(
               assignment,
               parameter1,
               parameter2
            );
            var setterAction = setter.Compile();
            _cachedSetters.Add(lambda.GetHashCode(), setterAction);
            setterAction.Invoke(Model, value);
        }
    }
}