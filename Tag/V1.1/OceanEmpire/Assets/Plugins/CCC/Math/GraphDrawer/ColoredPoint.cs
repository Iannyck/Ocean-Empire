using UnityEngine;

namespace CCC.Math.Graph
{
    [System.Serializable]
    public struct ColoredPoint
    {
        public Color color;
        public Vector2 position;
        public ColoredPoint(Vector2 position, Color color)
        {
            this.position = position;
            this.color = color;
        }
    }
}
