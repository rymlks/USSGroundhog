using EasyState.Core.Models;
using System;
using UnityEngine.UIElements;

namespace EasyState.Editor.Renderers.Designs
{
    public abstract class Tab : Model, IDisposable
    {
        public int Index { get; }
        public bool IsSelected { get; private set; }
        public VisualElement TabElement { get; }
        protected readonly TabContainer container;
        protected readonly Label tabLabel;

        private const string _SELECTED_CLASS = "selected-tab";

        public Tab(VisualElement tabElement, TabContainer container, int index, string id = null) : base(id)
        {
            TabElement = tabElement;
            this.container = container;
            Index = index;
            tabLabel = tabElement.Q<Label>("tab-title");

            TabElement.RegisterCallback<MouseDownEvent>(OnTabClicked);
        }

        public virtual void DeSelect()
        {
            IsSelected = false;
            TabElement.RemoveFromClassList(_SELECTED_CLASS);
        }

        public virtual void Dispose()
        {
            TabElement.UnregisterCallback<MouseDownEvent>(OnTabClicked);
        }

        public virtual void Select()
        {
            IsSelected = true;
            TabElement.AddToClassList(_SELECTED_CLASS);
        }

        protected abstract void OnTabClicked(MouseDownEvent evt);
        
    }
}