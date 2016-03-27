using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

namespace Svg
{
    public sealed class SvgClipRegion : ICloneable
    {
        public List<GraphicsPath> Paths { get; private set; } = new List<GraphicsPath>();

        public object Clone()
        {
            var cloned = new SvgClipRegion();
            cloned.Paths = Paths.ToList();

            return cloned;
        }

        public void Clear()
        {
            Paths.Clear();
        }

        public void Intersect(GraphicsPath path)
        {
            Paths.Add(path);
        }

        public void Intersect(RectangleF rectangle)
        {
            var path = new GraphicsPath();
            path.AddRectangle(rectangle);

            Paths.Add(path);
        }

        public void Exclude(GraphicsPath path)
        {
            var cloned = (GraphicsPath)path.Clone();
            cloned.Reverse();

            Paths.Add(cloned);
        }
    }
}
