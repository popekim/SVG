using System.Collections.Generic;
using Svg;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace SVGViewer
{
    class DebugRenderer : ISvgRenderer
    {
        private Matrix _transform = new Matrix();
        private Stack<ISvgBoundable> _boundables = new Stack<ISvgBoundable>();
        private SvgClipRegion _clipRegion = new SvgClipRegion();

        public void SetBoundable(ISvgBoundable boundable)
        {
            _boundables.Push(boundable);
        }
        public ISvgBoundable GetBoundable()
        {
            return _boundables.Peek();
        }
        public ISvgBoundable PopBoundable()
        {
            return _boundables.Pop();
        }


        public float DpiY
        {
            get { return 72; }
        }

        public void DrawImage(Image image, RectangleF destRect, RectangleF srcRect, GraphicsUnit graphicsUnit)
        {
            
        }
        public void DrawImageUnscaled(Image image, Point location)
        {
            
        }
        public void DrawPath(Pen pen, GraphicsPath path)
        {
            var newPath = (GraphicsPath)path.Clone();
            newPath.Transform(_transform);

        }
        public void FillPath(Brush brush, GraphicsPath path)
        {
            var newPath = (GraphicsPath)path.Clone();
            newPath.Transform(_transform);
        }
        public SvgClipRegion GetClip()
        {
            return _clipRegion;
        }
        public void RotateTransform(float fAngle, MatrixOrder order = MatrixOrder.Append)
        {
            _transform.Rotate(fAngle, order);
        }
        public void ScaleTransform(float sx, float sy, MatrixOrder order = MatrixOrder.Append)
        {
            _transform.Scale(sx, sy, order);
        }

        public void ReplaceClip(SvgClipRegion region)
        {
            _clipRegion = region;
            SetClipRegion();
        }

        public void ReplaceClip(RectangleF rect)
        {
            _clipRegion.Clear();
            _clipRegion.Intersect(rect);
            SetClipRegion();
        }

        public void SetClip(GraphicsPath path)
        {
            _clipRegion.Intersect(path);
            SetClipRegion();
        }

        public void SetClip(RectangleF rect)
        {
            _clipRegion.Intersect(rect);
            SetClipRegion();
        }

        private void SetClipRegion()
        {
            if (_clipRegion.Paths.Count == 0)
            {
            }
            else
            {
                var region = new Region(_clipRegion.Paths[0]);
                for (int i = 0; i < _clipRegion.Paths.Count; ++i)
                {
                    region.Intersect(_clipRegion.Paths[i]);
                }
            }
        }


        public void TranslateTransform(float dx, float dy, MatrixOrder order = MatrixOrder.Append)
        {
            _transform.Translate(dx, dy, order);
        }

        public CompositingMode CompositingMode
        {
          get { return System.Drawing.Drawing2D.CompositingMode.SourceOver; /* default value */ }
          set { /* Do Nothing */ }
        }

        public SmoothingMode SmoothingMode
        {
            get { return SmoothingMode.Default; }
            set { /* Do Nothing */ }
        }

        public Matrix Transform
        {
            get { return _transform; }
            set { _transform = value; }
        }

        public void Dispose()
        {
            
        }
    }
}
