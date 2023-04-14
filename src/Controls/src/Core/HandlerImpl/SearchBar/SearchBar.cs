#nullable disable
using System;

namespace Microsoft.Maui.Controls
{
	public partial class SearchBar
	{
		[Obsolete("Use SearchBarHandler.Mapper instead.")]
		public static IPropertyMapper<ISearchBar, SearchBarHandler> ControlsSearchBarMapper =
			new PropertyMapper<SearchBar, SearchBarHandler>(SearchBarHandler.Mapper)
			{
#if WINDOWS
				[PlatformConfiguration.WindowsSpecific.SearchBar.IsSpellCheckEnabledProperty.PropertyName] = MapIsSpellCheckEnabled,
#elif IOS
				[PlatformConfiguration.iOSSpecific.SearchBar.SearchBarStyleProperty.PropertyName] = MapSearchBarStyle,
#endif
				[nameof(Text)] = MapText,
				[nameof(TextTransform)] = MapText,
			};

		internal static new void RemapForControls()
		{
			// Adjust the mappings to preserve Controls.SearchBar legacy behaviors
#if WINDOWS
			SearchBarHandler.Mapper.ReplaceMappingWhen<SearchBar, ISearchBarHandler>(PlatformConfiguration.WindowsSpecific.SearchBar.IsSpellCheckEnabledProperty.PropertyName, MapIsSpellCheckEnabled);
#elif IOS
			SearchBarHandler.Mapper.ReplaceMappingWhen<SearchBar, ISearchBarHandler>(PlatformConfiguration.iOSSpecific.SearchBar.SearchBarStyleProperty.PropertyName, MapSearchBarStyle);
#endif
			SearchBarHandler.Mapper.ReplaceMappingWhen<SearchBar, ISearchBarHandler>(nameof(Text), MapText);
			SearchBarHandler.Mapper.ReplaceMappingWhen<SearchBar, ISearchBarHandler>(nameof(TextTransform), MapText);

#if ANDROID
			SearchBarHandler.CommandMapper.AppendToMapping(nameof(ISearchBar.Focus), MapFocus);
#endif
		}
	}
}
