using System;
using Microsoft.Maui.Controls.Internals;

namespace Microsoft.Maui.Controls.Platform
{
	internal partial class AlertManager
	{
		private partial bool TryCreateSubscription(out AlertRequestHelper subscription)
		{
			subscription = null!;
			return false;
		}

		private partial AlertRequestHelper[] GetSubscriptions()
		{
			return Array.Empty<AlertRequestHelper>();
		}

		internal partial class AlertRequestHelper
		{
			public partial void OnActionSheetRequested(Page sender, ActionSheetArguments arguments) { }

			public partial void OnAlertRequested(Page sender, AlertArguments arguments) { }

			public partial void OnPromptRequested(Page sender, PromptArguments arguments) { }

			public partial void OnPageBusy(Page sender, bool enabled) { }
		}
	}
}
