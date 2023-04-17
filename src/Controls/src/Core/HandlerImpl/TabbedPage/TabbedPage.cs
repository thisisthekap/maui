#nullable disable
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Maui.Controls
{
	public partial class TabbedPage
	{
		[Obsolete("Use TabbedViewHandler.Mapper instead.")]
		public static IPropertyMapper<ITabbedView, ITabbedViewHandler> ControlsTabbedPageMapper = new PropertyMapper<TabbedPage, ITabbedViewHandler>(TabbedViewHandler.Mapper)
		{
			[nameof(BarBackground)] = MapBarBackground,
			[nameof(BarBackgroundColor)] = MapBarBackgroundColor,
			[nameof(BarTextColor)] = MapBarTextColor,
			[nameof(UnselectedTabColor)] = MapUnselectedTabColor,
			[nameof(SelectedTabColor)] = MapSelectedTabColor,
			[nameof(MultiPage<TabbedPage>.ItemsSource)] = MapItemsSource,
			[nameof(MultiPage<TabbedPage>.ItemTemplate)] = MapItemTemplate,
			[nameof(MultiPage<TabbedPage>.SelectedItem)] = MapSelectedItem,
			[nameof(CurrentPage)] = MapCurrentPage,
#if ANDROID
			[PlatformConfiguration.AndroidSpecific.TabbedPage.IsSwipePagingEnabledProperty.PropertyName] = MapIsSwipePagingEnabled
#endif
		};

		internal new static void RemapForControls()
		{
			TabbedViewHandler.Mapper.ReplaceMapping<TabbedPage, ITabbedViewHandler>(nameof(BarBackground), MapBarBackground);
			TabbedViewHandler.Mapper.ReplaceMapping<TabbedPage, ITabbedViewHandler>(nameof(BarBackgroundColor), MapBarBackgroundColor);
			TabbedViewHandler.Mapper.ReplaceMapping<TabbedPage, ITabbedViewHandler>(nameof(BarTextColor), MapBarTextColor);
			TabbedViewHandler.Mapper.ReplaceMapping<TabbedPage, ITabbedViewHandler>(nameof(UnselectedTabColor), MapUnselectedTabColor);
			TabbedViewHandler.Mapper.ReplaceMapping<TabbedPage, ITabbedViewHandler>(nameof(SelectedTabColor), MapSelectedTabColor);
			TabbedViewHandler.Mapper.ReplaceMapping<TabbedPage, ITabbedViewHandler>(nameof(MultiPage<TabbedPage>.ItemsSource), MapItemsSource);
			TabbedViewHandler.Mapper.ReplaceMapping<TabbedPage, ITabbedViewHandler>(nameof(MultiPage<TabbedPage>.ItemTemplate), MapItemTemplate);
			TabbedViewHandler.Mapper.ReplaceMapping<TabbedPage, ITabbedViewHandler>(nameof(MultiPage<TabbedPage>.SelectedItem), MapSelectedItem);
			TabbedViewHandler.Mapper.ReplaceMapping<TabbedPage, ITabbedViewHandler>(nameof(CurrentPage), MapCurrentPage);
#if ANDROID
			TabbedViewHandler.Mapper.ReplaceMapping<TabbedPage, ITabbedViewHandler>(PlatformConfiguration.AndroidSpecific.TabbedPage.IsSwipePagingEnabledProperty.PropertyName, MapIsSwipePagingEnabled);
#endif

#if WINDOWS || ANDROID || TIZEN
			TabbedViewHandler.PlatformViewFactory = OnCreatePlatformView;
#endif
		}
	}
}
