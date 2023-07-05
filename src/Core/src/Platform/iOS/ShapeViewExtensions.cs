using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Platform;

namespace Microsoft.Maui.Platform
{
	public static class ShapeViewExtensions
	{
		public static void UpdateShape(this PlatformGraphicsView platformView, IShapeView shapeView)
		{
			platformView.Drawable = new ShapeDrawable(shapeView);

			// Allow Shape.MeasureOverride to compute the new drawable's size
			platformView.Bounds = CoreGraphics.CGRect.Empty;
		}

		public static void InvalidateShape(this PlatformGraphicsView platformView, IShapeView shapeView)
		{
			platformView.InvalidateDrawable();
		}
	}
}