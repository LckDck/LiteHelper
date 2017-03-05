using System;
namespace LiteHelper.Controls
{
	public static class ViewExtension
	{
		public static void CancelAllAnimations (this Xamarin.Forms.VisualElement view)
		{
			if (view == null)
				throw new ArgumentNullException (@"view");

			Xamarin.Forms.ViewExtensions.CancelAnimations (view);
		}
	}
}
