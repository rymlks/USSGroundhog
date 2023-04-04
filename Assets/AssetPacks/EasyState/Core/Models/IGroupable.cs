using System;
using UnityEngine;

namespace EasyState.Core.Models
{
    public interface IGroupable
    {
        string Id { get; }
        event Action<MoveableModel> Moving;
        Rect Rect{get;set;}
        Group Group { get; set; }
        void OnDelete(Design design);
        void OnGroupRefresh();
        void UpdatePosition(Vector2 delta);
    }
}
