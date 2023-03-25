using EasyState.Core.Models;
using EasyState.Editor.Renderers.DetailRenderers.FieldRenderers;
using UnityEngine.UIElements;

namespace EasyState.Editor.Renderers.DetailRenderers
{
    public abstract class DetailRendererBase<TModel> : IDetailRenderer where TModel : Model
    {
        protected abstract string Title { get; }
        protected readonly VisualElement Container;
        protected readonly Design Design;
        protected readonly TModel Model;
        protected PropertyCollection<TModel> PropertyCollection;

        protected DetailRendererBase(VisualElement container, TModel model, Design design)
        {
            Design = design;
            Container = container;
            Model = model;
        }

        public void OnClose()
        {
            if (PropertyCollection != null)
            {
                PropertyCollection.Dispose();
                PropertyCollection = null;
                OnWindowClosing();
            }
        }

        public DetailRendererResponse OnShow()
        {
            PropertyCollection = new PropertyCollection<TModel>(Container, Model);
            AddPropertyFields();
            return new DetailRendererResponse(Title, Container);
        }

        protected abstract void AddPropertyFields();

        protected virtual void OnWindowClosing()
        {
        }
    }
}