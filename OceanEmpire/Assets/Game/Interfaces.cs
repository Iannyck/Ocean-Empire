using System;
using UnityEngine;

namespace Interfaces
{
    public interface ITouchInputs
    {
        void OnTouch(Vector2 position);
    }
    public interface IClickInputs
    {
        void OnClick(Vector2 position);
    }
}
