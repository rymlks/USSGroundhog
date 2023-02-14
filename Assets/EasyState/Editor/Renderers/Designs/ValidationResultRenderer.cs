using EasyState.Core.Models;
using EasyState.Core.Utility;
using EasyState.Core.Validators;
using EasyState.Data;
using EasyState.Editor.Templates;
using System;
using System.Linq;
using UnityEngine.UIElements;

namespace EasyState.Editor.Renderers.Designs
{
    public class ValidationResultRenderer : ModelRenderer<Design>
    {
        private readonly VisualElement _template;
        private readonly VisualElement _contentContainer;
        private readonly VisualElement _popupContent;
        private readonly float _width = 375;
        public ValidationResultRenderer(Design model, VisualElement contentContainer) : base(model)
        { 
            _template = TemplateFactory.CreateValidationResultTemplate();
            contentContainer.Add(_template);
            _template.style.display = DisplayStyle.None;
            model.ValidateDesign += Design_ValidateDesign;
            var closeButton = _template.Q<Label>("close-btn");
            closeButton.AddManipulator(new Clickable(() => _template.style.display = DisplayStyle.None));
            _contentContainer = contentContainer;
            _template.style.width = _width;
            _popupContent = _template.Q<VisualElement>("content");
        }

        private void Design_ValidateDesign()
        {
            _template.style.display = DisplayStyle.Flex;
            float contentWidth = _contentContainer.localBound.width;     
            _template.style.top = 200;
            _template.style.left = (contentWidth / 2f) - (_width / 2f);
            _popupContent.Clear();
            var validationResult = DesignValidator.ValidateDesign(Model.Serialize());
            if (validationResult.AdditionalDesignsToValidate.Any())
            {
                foreach (var designID in validationResult.AdditionalDesignsToValidate)
                {
                    var designData = new DesignDatabase(designID).Load();           
                    var jumpDesignValidationResult = DesignValidator.ValidateDesign(designData);
                    if (!jumpDesignValidationResult)
                    {
                        validationResult.Errors.Add($"This design references design named '{designData.Name}' which is invalid.");
                    }
                }
            }
            if (validationResult)
            {
                var label = new Label($"• No errors detected.");
                label.style.color = EditorColors.Green;
                label.style.paddingLeft = 5;
                _popupContent.Add(label);
            }
            else
            {
                foreach (var error in validationResult.Errors)
                {
                    var label = new Label($"• {error}");
                    label.style.color = EditorColors.Pink;
                    label.style.paddingLeft = 10;
                    label.style.paddingTop = 5;
                    label.style.whiteSpace = WhiteSpace.Normal;
                    _popupContent.Add(label);
                }
            }

        }

        public override VisualElement Element { get => _template; protected set => throw new NotImplementedException(); }

        public override void Dispose()
        {
            Model.ValidateDesign -= Design_ValidateDesign;
        }
    }
}
