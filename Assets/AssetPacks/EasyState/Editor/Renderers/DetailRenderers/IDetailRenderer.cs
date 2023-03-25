namespace EasyState.Editor.Renderers.DetailRenderers
{
    internal interface IDetailRenderer
    {
        void OnClose();

        DetailRendererResponse OnShow();
    }
}