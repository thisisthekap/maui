﻿#nullable disable
using System;
using Microsoft.Maui.Handlers;

namespace Microsoft.Maui.Controls
{
	public partial class Window
	{
		[Obsolete("Use WindowHandler.Mapper instead.")]
		public static IPropertyMapper<IWindow, WindowHandler> ControlsWindowMapper =
			new PropertyMapper<IWindow, WindowHandler>(WindowHandler.Mapper);

		// ControlsWindowMapper incorrectly typed to WindowHandler
		[Obsolete("Use WindowHandler.Mapper instead.")]
		static IPropertyMapper<IWindow, IWindowHandler> Mapper =
			new PropertyMapper<IWindow, IWindowHandler>(ControlsWindowMapper)
			{
#if ANDROID
				// This property is also on the Application Mapper since that's where the attached property exists
				[PlatformConfiguration.AndroidSpecific.Application.WindowSoftInputModeAdjustProperty.PropertyName] = MapWindowSoftInputModeAdjust,
#endif

#if WINDOWS
				[nameof(ITitledElement.Title)] = MapWindowTitle
#endif
			};

		internal static new void RemapForControls()
		{
#if ANDROID
			// This property is also on the Application Mapper since that's where the attached property exists
			WindowHandler.Mapper.ReplaceMapping<IWindow, IWindowHandler>(PlatformConfiguration.AndroidSpecific.Application.WindowSoftInputModeAdjustProperty.PropertyName, MapWindowSoftInputModeAdjust);
#endif
		}
	}
}
