using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CCC.Math.Graph
{
    [System.Serializable]
    public class GraphDrawer
    {
        private const string SHADER_NAME = "CCC/Internal/GraphDrawer";
        private static Material _material;

        public Vector2 min = Vector2.zero;
        public Vector2 max = Vector2.one * 10;

        public List<ColoredCurve> curves = new List<ColoredCurve>();
        public List<ColoredPoint> points = new List<ColoredPoint>();

        public bool AutoSize = true;

        /// <summary>
        /// Seulement utilisé lorsque AutoSize est actif
        /// </summary>
        [ShowIf("AutoSize")]
        public float ScreenPadding = 15;


        private float ScreenWidth
        {
            get { return Camera.main.pixelWidth; }
        }
        private float ScreenHeight
        {
            get { return Camera.main.pixelHeight; }
        }
        public float Width
        {
            get { return max.x - min.x; }
        }
        public float Height
        {
            get { return max.y - min.y; }
        }

        /// <summary>
        /// MUST be called from OnRenderObject
        /// </summary>
        public void Draw()
        {
            if (GetMaterial() != null)
                GetMaterial().SetPass(0);

            if (AutoSize)
            {
                GetGraphMinMax(out min, out max, ScreenPadding);
            }

            GL.PushMatrix();

            GL.Begin(GL.LINES);
            GL.Color(Color.white);
            GL.LoadPixelMatrix();

            // Draw curves
            for (int i = 0; i < curves.Count; i++)
            {
                GL.Color(curves[i].Color);
                for (int j = 1; j < curves[i].Positions.Count; j++)
                {
                    AddLine_GS(curves[i].Positions[j], curves[i].Positions[j - 1]);
                }
            }

            // Draw points
            for (int i = 0; i < points.Count; i++)
            {
                GL.Color(points[i].color);
                AddCross_GS(points[i].position);
            }
            GL.End();

            GL.PopMatrix();
        }

        void GetGraphMinMax(out Vector2 min, out Vector2 max, float screenSpacePadding)
        {
            Vector2 t_min = new Vector2(int.MaxValue, int.MaxValue);
            Vector2 t_max = new Vector2(int.MinValue, int.MinValue);

            Action<Vector2> checkPoint = (p) =>
            {
                if (p.x < t_min.x)
                    t_min.x = p.x;
                if (p.y < t_min.y)
                    t_min.y = p.y;

                if (p.x > t_max.x)
                    t_max.x = p.x;
                if (p.y > t_max.y)
                    t_max.y = p.y;
            };

            // Check curves
            for (int i = 0; i < curves.Count; i++)
                for (int j = 0; j < curves[i].Positions.Count; j++)
                    checkPoint(curves[i].Positions[j]);

            // Check points
            for (int i = 0; i < points.Count; i++)
                checkPoint(points[i].position);

            // Padding
            var verticalPadding = screenSpacePadding * Height / ScreenHeight;
            var horizontalPadding = screenSpacePadding * Width / ScreenWidth;
            t_min.x -= horizontalPadding;
            t_min.y -= verticalPadding;
            t_max.x += horizontalPadding;
            t_max.y += verticalPadding;

            // Si ya aucun point, on mes des valeurs par défaut
            if (t_max.x == int.MinValue)
            {
                t_min = Vector2.zero;
                t_max = Vector2.one;
            }

            min = t_min;
            max = t_max;
        }

        void AddLine_GS(Vector2 a, Vector2 b)
        {
            AddLine_SS(GraphToScreenPos(a), GraphToScreenPos(b));
        }
        void AddLine_SS(Vector2 a, Vector2 b)
        {
            GL.Vertex(a);
            GL.Vertex(b);
        }

        private const int CROSS_SIZE = 4;
        void AddCross_GS(Vector2 a)
        {
            AddCross_SS(GraphToScreenPos(a));
        }
        void AddCross_SS(Vector2 a)
        {
            AddLine_SS(a + Vector2.left * CROSS_SIZE, a + Vector2.right * CROSS_SIZE);
            AddLine_SS(a + Vector2.down * CROSS_SIZE, a + Vector2.up * CROSS_SIZE);
        }

        Vector2 GraphToScreenPos(Vector2 point)
        {
            return new Vector2((point.x - min.x) / Width * ScreenWidth, (point.y - min.y) / Height * ScreenHeight);
        }
        private static Material GetMaterial()
        {
            if (_material == null)
            {
                Shader shader = Shader.Find(SHADER_NAME);
                if (shader == null)
                {
                    Debug.LogError("No shader for the GraphDrawer");
                    return null;
                }
                _material = new Material(shader);
            }
            return _material;
        }
    }

}