using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Handlers;

namespace Microsoft.Maui.Controls
{

	public partial class Toolbar
	{
		IMauiContext MauiContext => Handler?.MauiContext ?? throw new InvalidOperationException("MauiContext not set");

		[Obsolete("Use ToolbarHandler.Mapper instead.")]
		public static IPropertyMapper<Toolbar, ToolbarHandler> ControlsToolbarMapper =
			new PropertyMapper<Toolbar, ToolbarHandler>(ToolbarHandler.Mapper)
			{
#if ANDROID || WINDOWS || TIZEN
				[nameof(IToolbar.IsVisible)] = MapIsVisible,
				[nameof(IToolbar.BackButtonVisible)] = MapBackButtonVisible,
				[nameof(Toolbar.TitleIcon)] = MapTitleIcon,
				[nameof(Toolbar.TitleView)] = MapTitleView,
				[nameof(Toolbar.IconColor)] = MapIconColor,
				[nameof(Toolbar.ToolbarItems)] = MapToolbarItems,
				[nameof(Toolbar.BackButtonTitle)] = MapBackButtonTitle,
				[nameof(Toolbar.BarBackground)] = MapBarBackground,
				[nameof(Toolbar.BarTextColor)] = MapBarTextColor,
#endif
#if WINDOWS
				[nameof(Toolbar.BackButtonEnabled)] = MapBackButtonEnabled,
				[PlatformConfiguration.WindowsSpecific.Page.ToolbarPlacementProperty.PropertyName] = MapToolbarPlacement,
				[PlatformConfiguration.WindowsSpecific.Page.ToolbarDynamicOverflowEnabledProperty.PropertyName] = MapToolbarDynamicOverflowEnabled,
#endif
			};

		internal static void RemapForControls()
		{
#if ANDROID || WINDOWS || TIZEN
			ToolbarHandler.Mapper.ModifyMappingWhen<Toolbar, IToolbarHandler>(nameof(IToolbar.IsVisible), MapIsVisible);
			ToolbarHandler.Mapper.ModifyMappingWhen<Toolbar, IToolbarHandler>(nameof(IToolbar.BackButtonVisible), MapBackButtonVisible);
			ToolbarHandler.Mapper.ModifyMappingWhen<Toolbar, IToolbarHandler>(nameof(Toolbar.TitleIcon), MapTitleIcon);
			ToolbarHandler.Mapper.ModifyMappingWhen<Toolbar, IToolbarHandler>(nameof(Toolbar.TitleView), MapTitleView);
			ToolbarHandler.Mapper.ModifyMappingWhen<Toolbar, IToolbarHandler>(nameof(Toolbar.IconColor), MapIconColor);
			ToolbarHandler.Mapper.ModifyMappingWhen<Toolbar, IToolbarHandler>(nameof(Toolbar.ToolbarItems), MapToolbarItems);
			ToolbarHandler.Mapper.ModifyMappingWhen<Toolbar, IToolbarHandler>(nameof(Toolbar.BackButtonTitle), MapBackButtonTitle);
			ToolbarHandler.Mapper.ModifyMappingWhen<Toolbar, IToolbarHandler>(nameof(Toolbar.BarBackground), MapBarBackground);
			ToolbarHandler.Mapper.ModifyMappingWhen<Toolbar, IToolbarHandler>(nameof(Toolbar.BarTextColor), MapBarTextColor);
#endif
#if WINDOWS
			ToolbarHandler.Mapper.ModifyMappingWhen<Toolbar, IToolbarHandler>(nameof(Toolbar.BackButtonEnabled), MapBackButtonEnabled);
			ToolbarHandler.Mapper.ModifyMappingWhen<Toolbar, IToolbarHandler>(PlatformConfiguration.WindowsSpecific.Page.ToolbarPlacementProperty.PropertyName, MapToolbarPlacement);
			ToolbarHandler.Mapper.ModifyMappingWhen<Toolbar, IToolbarHandler>(PlatformConfiguration.WindowsSpecific.Page.ToolbarDynamicOverflowEnabledProperty.PropertyName, MapToolbarDynamicOverflowEnabled);
#endif
		}
	}
}
