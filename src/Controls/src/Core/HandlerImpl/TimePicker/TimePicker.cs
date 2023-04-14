#nullable disable
using System;

namespace Microsoft.Maui.Controls
{
	public partial class TimePicker
	{
		[Obsolete("Use TimePickerHandler.Mapper instead.")]
		public static IPropertyMapper<ITimePicker, TimePickerHandler> ControlsTimePickerMapper = new PropertyMapper<TimePicker, TimePickerHandler>(TimePickerHandler.Mapper)
		{
#if IOS
			[PlatformConfiguration.iOSSpecific.TimePicker.UpdateModeProperty.PropertyName] = MapUpdateMode,
#endif
		};

		internal static new void RemapForControls()
		{
			// Adjust the mappings to preserve Controls.TimePicker legacy behaviors
#if IOS
			TimePickerHandler.Mapper.ReplaceMappingWhen<TimePicker, ITimePickerHandler>(PlatformConfiguration.iOSSpecific.TimePicker.UpdateModeProperty.PropertyName, MapUpdateMode);
#endif
		}
	}
}