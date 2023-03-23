#nullable disable
namespace Microsoft.Maui.Controls
{
	public partial class Application
	{
		public static IPropertyMapper<IApplication, ApplicationHandler> ControlsApplicationMapper =
			new PropertyMapper<Application, ApplicationHandler>(ApplicationHandler.Mapper)
			{
#if ANDROID
				// There is also a mapper on Window for this property since this property is relevant at the window level for
				// Android not the application level
				[PlatformConfiguration.AndroidSpecific.Application.WindowSoftInputModeAdjustProperty.PropertyName] = MapWindowSoftInputModeAdjust,
#endif
			};

		internal static new void RemapForControls()
		{
#if ANDROID
			// There is also a mapper on Window for this property since this property is relevant at the window level for
			// Android not the application level
			ApplicationHandler.Mapper.ModifyMappingWhen<Application, ApplicationHandler>(PlatformConfiguration.AndroidSpecific.Application.WindowSoftInputModeAdjustProperty.PropertyName, MapWindowSoftInputModeAdjust);
#endif
		}
	}
}
