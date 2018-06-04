using System.Collections.Generic;
using UnityEngine;

namespace CCC.Math.Graph
{
    [System.Serializable]
    public class ColoredCurve
    {
        public ColoredCurve() { }
        public ColoredCurve(Color color)
        {
            _color = color;
        }
        [System.NonSerialized] private List<Vector2> _positions = new List<Vector2>();
        [SerializeField] private Color _color = Color.white;

        public List<Vector2> Positions
        {
            get
            {
                if (_positions == null)
                    _positions = new List<Vector2>();
                return _positions;
            }
        }
        public Color Color
        {
            get { return _color; }
            set { _color = value; }
        }
    }
}