using System;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Collections.Generic;

namespace Svg
{
    public interface ISvgRenderer : IDisposable
    {
        float DpiY { get; }
        void DrawImage(Image image, RectangleF destRect, RectangleF srcRect, GraphicsUnit graphicsUnit);
        void DrawImageUnscaled(Image image, Point location);
        void DrawPath(Pen pen, GraphicsPath path);
        void FillPath(Brush brush, GraphicsPath path);
        ISvgBoundable GetBoundable();
        SvgClipRegion GetClip();
        ISvgBoundable PopBoundable();
        void RotateTransform(float fAngle, MatrixOrder order = MatrixOrder.Append);
        void ScaleTransform(float sx, float sy, MatrixOrder order = MatrixOrder.Append);
        void SetBoundable(ISvgBoundable boundable);
        void ReplaceClip(SvgClipRegion region);
        void ReplaceClip(RectangleF region);
        void SetClip(GraphicsPath path);
        void SetClip(RectangleF rectangle);
        SmoothingMode SmoothingMode { get; set; }
        Matrix Transform { get; set; }
        void TranslateTransform(float dx, float dy, MatrixOrder order = MatrixOrder.Append);
        ISvgRenderer BeginPatternRender(float width, float height);
        Brush EndPatternRender(Matrix patternMatrix);
    }
}
