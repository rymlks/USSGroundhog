using EasyState.Core.Models;
using EasyState.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace EasyState.Core.Layers
{
    public class BaseLayer<T> : IReadOnlyList<T> where T : Model
    {
        public T this[int index] => _items[index];

        public event Action<T> Added;

        public event Action<T> Removed;

        public int Count => _items.Count;
        public Design Design { get; }
        protected readonly List<T> _items = new List<T>();

        public BaseLayer(Design design)
        {
            Design = design;
        }

        public virtual void Add(T item)
        {
            item.GuardNullArg();
            _items.Add(item);
            OnItemAdded(item);
            Added?.Invoke(item);
            Design.Refresh();
        }

        public virtual void Add(IEnumerable<T> items)
        {
            items.GuardNullArg();

            Design.Batch(() =>
            {
                foreach (var item in items)
                {
                    _items.Add(item);
                    OnItemAdded(item);
                    Added?.Invoke(item);
                }
            });
        }

        public bool Contains(T item) => _items.Contains(item);

        public bool Contains(string id) => _items.Any(x => x.Id == id);

        public IEnumerator<T> GetEnumerator() => _items.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _items.GetEnumerator();

        public virtual void OnItemRemoved(T item)
        {
        }

        public virtual bool Remove(T item)
        {
            item.GuardNullArg();
            if (_items.Remove(item))
            {
                OnItemRemoved(item);
                Removed?.Invoke(item);
                Design.OnModelDeleted(item);
                Design.Refresh();
                return true;
            }
            return false;
        }

        public virtual void Remove(IEnumerable<T> items)
        {
            items.GuardNullArg();
            Design.Batch(() =>
            {
                foreach (var item in items.ToList())
                {
                    if (_items.Remove(item))
                    {
                        OnItemRemoved(item);
                        Removed?.Invoke(item);
                        Design.OnModelDeleted(item);
                    }
                }
            });
        }

        protected virtual void OnItemAdded(T item)
        {
        }

        protected virtual void TriggerRemovedEvent(T item) => Removed?.Invoke(item);
    }
}