#nullable disable
using System;

namespace Microsoft.Maui.Controls
{
	public partial class Editor
	{
		[Obsolete("Use EditorHandler.Mapper instead.")]
		public static IPropertyMapper<IEditor, EditorHandler> ControlsEditorMapper =
			new PropertyMapper<Editor, EditorHandler>(EditorHandler.Mapper)
			{
#if WINDOWS
				[PlatformConfiguration.WindowsSpecific.InputView.DetectReadingOrderFromContentProperty.PropertyName] = MapDetectReadingOrderFromContent,
#endif
				[nameof(Text)] = MapText,
				[nameof(TextTransform)] = MapText,
			};

		internal static new void RemapForControls()
		{
			// Adjust the mappings to preserve Controls.Editor legacy behaviors
#if WINDOWS
			EditorHandler.Mapper.ModifyMappingWhen<Editor, IEditorHandler>(PlatformConfiguration.WindowsSpecific.InputView.DetectReadingOrderFromContentProperty.PropertyName, MapDetectReadingOrderFromContent);
#endif
			EditorHandler.Mapper.ModifyMappingWhen<Editor, IEditorHandler>(nameof(Text), MapText);
			EditorHandler.Mapper.ModifyMappingWhen<Editor, IEditorHandler>(nameof(TextTransform), MapText);

#if ANDROID
			EditorHandler.CommandMapper.PrependToMapping(nameof(IEditor.Focus), MapFocus);
#endif
		}
	}
}