#nullable disable
using System;

namespace Microsoft.Maui.Controls
{
	public partial class Entry
	{
		[Obsolete("Use EntryHandler.Mapper instead.")]
		public static IPropertyMapper<IEntry, EntryHandler> ControlsEntryMapper =
			new PropertyMapper<Entry, EntryHandler>(EntryHandler.Mapper)
			{
#if ANDROID
				[PlatformConfiguration.AndroidSpecific.Entry.ImeOptionsProperty.PropertyName] = MapImeOptions,
#elif WINDOWS
				[PlatformConfiguration.WindowsSpecific.InputView.DetectReadingOrderFromContentProperty.PropertyName] = MapDetectReadingOrderFromContent,
#elif IOS
				[PlatformConfiguration.iOSSpecific.Entry.CursorColorProperty.PropertyName] = MapCursorColor,
				[PlatformConfiguration.iOSSpecific.Entry.AdjustsFontSizeToFitWidthProperty.PropertyName] = MapAdjustsFontSizeToFitWidth,
#endif
				[nameof(Text)] = MapText,
				[nameof(TextTransform)] = MapText,
			};

		internal static new void RemapForControls()
		{
			// Adjust the mappings to preserve Controls.Entry legacy behaviors
#if ANDROID
			EntryHandler.Mapper.ModifyMappingWhen<Entry, IEntryHandler>(PlatformConfiguration.AndroidSpecific.Entry.ImeOptionsProperty.PropertyName, MapImeOptions);
#elif WINDOWS
			EntryHandler.Mapper.ModifyMappingWhen<Entry, IEntryHandler>(PlatformConfiguration.WindowsSpecific.InputView.DetectReadingOrderFromContentProperty.PropertyName, MapDetectReadingOrderFromContent);
#elif IOS
			EntryHandler.Mapper.ModifyMappingWhen<Entry, IEntryHandler>(PlatformConfiguration.iOSSpecific.Entry.CursorColorProperty.PropertyName, MapCursorColor);
			EntryHandler.Mapper.ModifyMappingWhen<Entry, IEntryHandler>(PlatformConfiguration.iOSSpecific.Entry.AdjustsFontSizeToFitWidthProperty.PropertyName, MapAdjustsFontSizeToFitWidth);
#endif
			EntryHandler.Mapper.ModifyMappingWhen<Entry, IEntryHandler>(nameof(Text), MapText);
			EntryHandler.Mapper.ModifyMappingWhen<Entry, IEntryHandler>(nameof(TextTransform), MapText);
		}
	}
}
