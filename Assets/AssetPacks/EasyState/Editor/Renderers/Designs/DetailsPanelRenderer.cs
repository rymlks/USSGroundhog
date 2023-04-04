using EasyState.Core.Models;
using EasyState.Editor.Renderers.DetailRenderers;
using System;
using UnityEngine.UIElements;

namespace EasyState.Editor.Renderers.Designs
{
    internal class DetailsPanelRenderer : IDisposable
    {
        private readonly VisualElement _body;
        private readonly VisualElement _closeButton;
        private readonly Design _design;
        private readonly VisualElement _panelRoot;
        private readonly Label _titleLabel;
        private Model _detailsModel;
        private DetailRendererProvider _rendererProvider = new DetailRendererProvider();
        private IDetailRenderer _selectedDetailRenderer;

        public DetailsPanelRenderer(Design design, VisualElement panelRoot)
        {
            _design = design;
            _panelRoot = panelRoot;
            _titleLabel = _panelRoot.Q<Label>("title");
            _closeButton = _panelRoot.Q<VisualElement>("close-button");
            _body = _panelRoot.Q<VisualElement>("body");
            _closeButton.AddManipulator(new Clickable(OnCloseClicked));
            _panelRoot.style.display = DisplayStyle.None;
            _design.DetailsPanelRequested += Design_DetailsPanelRequested;
            _design.ModelDeleted += Design_ModelDeleted;
        }

        public void Dispose()
        {
            _design.DetailsPanelRequested -= Design_DetailsPanelRequested;
            _design.ModelDeleted -= Design_ModelDeleted;
        }

        public void HideWindow()
        {
            _panelRoot.style.display = DisplayStyle.None;
            _detailsModel = null;
        }

        public void SetContent(VisualElement content)
        {
            _body.Clear();
            _body.Add(content);
        }

        public void SetTitle(string text)
        {
            _titleLabel.text = text;
        }

        private void Design_DetailsPanelRequested(Model model)
        {
            OnCloseClicked();
            _selectedDetailRenderer = _rendererProvider.GetDetailRenderer(model, _design);
            var response = _selectedDetailRenderer.OnShow();
            SetContent(response.Contents);
            SetTitle(response.Title);
            _panelRoot.style.display = DisplayStyle.Flex;
            _detailsModel = model;
        }

        private void Design_ModelDeleted(Model obj)
        {
            if (obj == _detailsModel)
            {
                HideWindow();
            }
        }

        private void OnCloseClicked()
        {
            if (_selectedDetailRenderer != null)
            {
                _selectedDetailRenderer.OnClose();
                _selectedDetailRenderer = null;
            }
            _panelRoot.style.display = DisplayStyle.None;
            _detailsModel = null;
        }
    }
}