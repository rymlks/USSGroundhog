using EasyState.DataModels;
using System;
using UnityEngine;

namespace EasyState.Core.Models
{
    public abstract class MoveableModel : SelectableModel
    {
        public event Action<MoveableModel> Moving;

        public Vector2 Position { get; set; }

        public MoveableModel(MoveableData data) : base(data)
        {
            Position = data.Position;
        }

        public MoveableModel(Vector2? position = null, string id = null) : base(id)
        {
            Position = position ?? Vector2.zero;
        }

        public MoveableModel(string id, Vector2? position = null) : base(id)
        {
            Position = position ?? Vector2.zero;
        }

        public virtual void SetPosition(Vector2 position)
        {
            SetPositionSilently(position);
            Moving?.Invoke(this);
        }

        public virtual void SetPositionSilently(Vector2 position) => Position = position;

        public virtual void UpdatePosition(Vector2 delta)
        {
            UpdatePositionSilently(delta);
            Moving?.Invoke(this);
        }

        public virtual void UpdatePositionSilently(Vector2 delta) => Position += delta;


    }
}