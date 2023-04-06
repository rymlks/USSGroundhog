using EasyState.Core.Models;
using EasyState.Editor.Templates;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UIElements;

namespace EasyState.Editor.Renderers.Designs
{
    public class ToastRenderer : IDisposable
    {
        private const long _addDelay = 400;
        private readonly VisualElement _toastContainer;
        private readonly VisualTreeAsset _toastMessageTemplate;
        private List<Design> _activeDesigns = new List<Design>();
        private long _lastTimeAdded;
        private TabContainer _tabContainer;

        public ToastRenderer(VisualElement toastContainer, TabContainer tabContainer)
        {
            _toastContainer = toastContainer;
            _toastMessageTemplate = TemplateFactory.GetToastMessageTemplate();
            _tabContainer = tabContainer;
            _tabContainer.DesignAdded += OnDesignAdded;
            _tabContainer.DesignClosed += OnDesignRemoved;
        }

        public void Dispose()
        {
            _tabContainer.DesignAdded -= OnDesignAdded;
            _tabContainer.DesignClosed -= OnDesignRemoved;
            foreach (var design in _activeDesigns)
            {
                design.ShowToast -= ShowMessage;
            }
        }

        public void ShowMessage(string message, long duration)
        {
            var messageClone = _toastMessageTemplate.CloneTree();

            Label label = messageClone.Q<Label>();
            label.text = message;

            _toastContainer.Add(messageClone);

            long waitTime = 0;
            if (_lastTimeAdded + _addDelay > GetCurrentTimeInMS())
            {
                waitTime = _lastTimeAdded + _addDelay - GetCurrentTimeInMS();
            }
            _lastTimeAdded = GetCurrentTimeInMS() + waitTime;
            _toastContainer.schedule.Execute(() =>
            {
                ToastMessage tab = new ToastMessage(messageClone, duration);
                tab.Element.schedule.Execute(() => tab.Update()).Every(10).Until(() => tab.ShouldRemove);
            }
            ).ExecuteLater(waitTime);
        }

        private long GetCurrentTimeInMS()
        {
            return (long)(EditorApplication.timeSinceStartup * 1000);
        }

        private void OnDesignAdded(Design design)
        {
            _activeDesigns.Remove(design);
            design.ShowToast += ShowMessage;
        }

        private void OnDesignRemoved(Design design)
        {
            _activeDesigns.Add(design);
            design.ShowToast -= ShowMessage;
        }

        private class ToastMessage
        {
            public VisualElement Element { get; private set; }
            public bool ShouldRemove { get; private set; }
            private float _opacity => Element.resolvedStyle.opacity;

            private long _duration;

            private bool _hasFadedIn;

            private VisualElement _parent;

            private long _startFadeOutTime;

            public ToastMessage(VisualElement element, long duration)
            {
                Element = element;
                _parent = element.parent;
                Element.style.opacity = 0;
                _duration = duration;
            }

            public void Update()
            {
                if (!_hasFadedIn)
                {
                    FadeIn();
                }
                else
                {
                    if (FadeOut())
                    {
                        ShouldRemove = true;
                    }
                }
            }

            private void FadeIn()
            {
                if (_opacity < 1)
                {
                    Element.style.opacity = _opacity + .1f;
                }
                else
                {
                    _startFadeOutTime = GetCurrentTimeInMS() + _duration;
                    _hasFadedIn = true;
                }
            }

            private bool FadeOut()
            {
                if (GetCurrentTimeInMS() < _startFadeOutTime)
                {
                    return false;
                }
                if (_opacity < 0)
                {
                    _parent.Remove(Element);
                    return true;
                }
                Element.style.opacity = _opacity - .05f;
                return false;
            }

            private long GetCurrentTimeInMS()
            {
                return (long)(EditorApplication.timeSinceStartup * 1000);
            }
        }
    }
}