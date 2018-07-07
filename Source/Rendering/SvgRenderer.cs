using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace Svg
{
    /// <summary>
    /// Convenience wrapper around a graphics object
    /// </summary>
    public sealed class SvgRenderer : IDisposable, IGraphicsProvider, ISvgRenderer
    {
        private Graphics _innerGraphics;
        private Stack<ISvgBoundable> _boundables = new Stack<ISvgBoundable>();
        private SvgClipRegion _clipRegion = new SvgClipRegion();

        private Bitmap _patternImage;
        public ISvgRenderer BeginPatternRender(float width, float height)
        {
            Debug.Assert(_patternImage == null, "always call begin and end pair");

            _patternImage = new Bitmap((int)width, (int)height);
            return FromImage(_patternImage);
        }
        public Brush EndPatternRender(Matrix brushTransform)
        {
            Debug.Assert(_patternImage != null, "always call begin and end pair");

            TextureBrush textureBrush = new TextureBrush(_patternImage);
            _patternImage = null;
            
            textureBrush.Transform = brushTransform;

            return textureBrush;
        }

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
            get { return _innerGraphics.DpiY; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ISvgRenderer"/> class.
        /// </summary>
        private SvgRenderer(Graphics graphics)
        {
            this._innerGraphics = graphics;
        }

        public void DrawImage(Image image, RectangleF destRect, RectangleF srcRect, GraphicsUnit graphicsUnit)
        {
            _innerGraphics.DrawImage(image, destRect, srcRect, graphicsUnit);
        }
        public void DrawImageUnscaled(Image image, Point location)
        {
            this._innerGraphics.DrawImageUnscaled(image, location);
        }
        public void DrawPath(Pen pen, GraphicsPath path)
        {
            this._innerGraphics.DrawPath(pen, path);
        }
        public void FillPath(Brush brush, GraphicsPath path)
        {
            this._innerGraphics.FillPath(brush, path);
        }
        public Region GetClip()
        {
            return this._innerGraphics.Clip;
        }
        public void RotateTransform(float fAngle, MatrixOrder order = MatrixOrder.Append)
        {
            this._innerGraphics.RotateTransform(fAngle, order);
        }
        public void ScaleTransform(float sx, float sy, MatrixOrder order = MatrixOrder.Append)
        {
            this._innerGraphics.ScaleTransform(sx, sy, order);
        }

        private void SetClipRegion()
        { 
            if (_clipRegion.Paths.Count == 0)
            {
                _innerGraphics.ResetClip();
            }
            else
            {
                var region = new Region(_clipRegion.Paths[0]);
                for (int i = 0; i < _clipRegion.Paths.Count; ++i)
                {
                    region.Intersect(_clipRegion.Paths[i]);
                }

                _innerGraphics.SetClip(region, CombineMode.Replace);
            }
        }

        public void ReplaceClip(SvgClipRegion region)
        {
            _clipRegion = region;
            SetClipRegion();
        }

        public void ReplaceClip(RectangleF rect)
        {
            _clipRegion.Paths.Clear();
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

        public void TranslateTransform(float dx, float dy, MatrixOrder order = MatrixOrder.Append)
        {
            this._innerGraphics.TranslateTransform(dx, dy, order);
        }
        


        public SmoothingMode SmoothingMode
        {
            get { return this._innerGraphics.SmoothingMode; }
            set { this._innerGraphics.SmoothingMode = value; }
        }

        public Matrix Transform
        {
            get { return this._innerGraphics.Transform; }
            set { this._innerGraphics.Transform = value; }
        }

        public void Dispose()
        {
            this._innerGraphics.Dispose();
        }

        Graphics IGraphicsProvider.GetGraphics()
        {
            return _innerGraphics;
        }

        /// <summary>
        /// Creates a new <see cref="ISvgRenderer"/> from the specified <see cref="Image"/>.
        /// </summary>
        /// <param name="image"><see cref="Image"/> from which to create the new <see cref="ISvgRenderer"/>.</param>
        public static ISvgRenderer FromImage(Image image)
        {
            var g = Graphics.FromImage(image);
            g.TextRenderingHint = TextRenderingHint.AntiAlias;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            g.TextContrast = 1;
            return new SvgRenderer(g);
        }

        /// <summary>
        /// Creates a new <see cref="ISvgRenderer"/> from the specified <see cref="Graphics"/>.
        /// </summary>
        /// <param name="graphics">The <see cref="Graphics"/> to create the renderer from.</param>
        public static ISvgRenderer FromGraphics(Graphics graphics)
        {
            return new SvgRenderer(graphics);
        }

        public static ISvgRenderer FromNull()
        {
            var img = new Bitmap(1, 1);
            return SvgRenderer.FromImage(img);
        }

        SvgClipRegion ISvgRenderer.GetClip()
        {
            return (SvgClipRegion)_clipRegion.Clone();
        }
    }
}