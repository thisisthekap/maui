#nullable disable
using System;

namespace Microsoft.Maui.Controls
{
	public partial class WebView
	{
		[Obsolete("Use WebViewHandler.Mapper instead.")]
		public static IPropertyMapper<IWebView, WebViewHandler> ControlsWebViewMapper = new PropertyMapper<WebView, WebViewHandler>(WebViewHandler.Mapper)
		{
#if ANDROID
			[PlatformConfiguration.AndroidSpecific.WebView.DisplayZoomControlsProperty.PropertyName] = MapDisplayZoomControls,
			[PlatformConfiguration.AndroidSpecific.WebView.EnableZoomControlsProperty.PropertyName] = MapEnableZoomControls,
			[PlatformConfiguration.AndroidSpecific.WebView.MixedContentModeProperty.PropertyName] = MapMixedContentMode,
#endif
		};

		internal static new void RemapForControls()
		{
			// Adjust the mappings to preserve Controls.WebView legacy behaviors
#if ANDROID
			WebViewHandler.Mapper.ModifyMappingWhen<WebView, IWebViewHandler>(PlatformConfiguration.AndroidSpecific.WebView.DisplayZoomControlsProperty.PropertyName, MapDisplayZoomControls);
			WebViewHandler.Mapper.ModifyMappingWhen<WebView, IWebViewHandler>(PlatformConfiguration.AndroidSpecific.WebView.EnableZoomControlsProperty.PropertyName, MapEnableZoomControls);
			WebViewHandler.Mapper.ModifyMappingWhen<WebView, IWebViewHandler>(PlatformConfiguration.AndroidSpecific.WebView.MixedContentModeProperty.PropertyName, MapMixedContentMode);
#endif
		}
	}
}
