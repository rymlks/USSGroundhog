using UnityEngine;
using UnityEngine.UIElements;

namespace EasyState.Editor.Utility
{
    public static class VisualElementExtensions
    {
        public static void DisableElement(this VisualElement element)
        {
            element.pickingMode = PickingMode.Ignore;
            element.style.opacity = .3f;
        }

        public static void EnableElement(this VisualElement element)
        {
            element.pickingMode = PickingMode.Position;
            element.style.opacity = 1;
        }

        public static void SetBorderColor(this VisualElement element, Color color)
        {
            element.style.borderBottomColor = color;
            element.style.borderLeftColor = color;
            element.style.borderTopColor = color;
            element.style.borderRightColor = color;
        }

        public static void SetWidthAndHeight(this VisualElement element, int width, int? height = null)
        {
            if (height is null)
            {
                height = width;
            }
            element.style.width = width;
            element.style.height = height.Value;
        }
        public static void SetBorderRadius(this VisualElement element, int percentage)
        {
            element.style.borderBottomLeftRadius = new StyleLength(new Length(percentage, LengthUnit.Percent));
            element.style.borderBottomRightRadius = new StyleLength(new Length(percentage, LengthUnit.Percent));
            element.style.borderTopLeftRadius = new StyleLength(new Length(percentage, LengthUnit.Percent));
            element.style.borderTopRightRadius = new StyleLength(new Length(percentage, LengthUnit.Percent));
        }
        public static void SetPlaceholderText(this TextField textField, string placeholder)
        {
            string placeholderClass = TextField.ussClassName + "__placeholder";

            onFocusOut();
            textField.RegisterCallback<FocusInEvent>(evt => onFocusIn());
            textField.RegisterCallback<FocusOutEvent>(evt => onFocusOut());

            void onFocusIn()
            {
                if (textField.ClassListContains(placeholderClass))
                {
                    textField.value = string.Empty;
                    textField.RemoveFromClassList(placeholderClass);
                }
            }

            void onFocusOut()
            {
                if (string.IsNullOrEmpty(textField.text))
                {
                    textField.SetValueWithoutNotify(placeholder);
                    textField.AddToClassList(placeholderClass);
                }
            }
        }
    }
}