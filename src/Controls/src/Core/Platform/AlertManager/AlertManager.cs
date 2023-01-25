using System.Collections.Generic;
using Microsoft.Maui.Controls.Internals;

namespace Microsoft.Maui.Controls.Platform
{
	internal partial class AlertManager
	{
		readonly Window _window;
		readonly List<AlertRequestHelper> Subscriptions = new();

		public AlertManager(Window window)
		{
			_window = window;
		}

		public Window Window => _window;

		public void Subscribe()
		{
			if (TryCreateSubscription(out var alertRequestHelper))
				Subscriptions.Add(alertRequestHelper);
		}

		public void Unsubscribe()
		{
			foreach (var alertRequestHelper in GetSubscriptions())
			{
				Subscriptions.Remove(alertRequestHelper);
			}
		}

		public void RequestActionSheet(Page page, ActionSheetArguments arguments)
		{
			foreach (var alertRequestHelper in GetSubscriptions())
			{
				alertRequestHelper.OnActionSheetRequested(page, arguments);
			}
		}

		public void RequestAlert(Page page, AlertArguments arguments)
		{
			foreach (var alertRequestHelper in GetSubscriptions())
			{
				alertRequestHelper.OnAlertRequested(page, arguments);
			}
		}

		public void RequestPrompt(Page page, PromptArguments arguments)
		{
			foreach (var alertRequestHelper in GetSubscriptions())
			{
				alertRequestHelper.OnPromptRequested(page, arguments);
			}
		}

		public void RequestPageBusy(Page page, bool isBusy)
		{
			foreach (var alertRequestHelper in GetSubscriptions())
			{
				alertRequestHelper.OnPageBusy(page, isBusy);
			}
		}

		private partial bool TryCreateSubscription(out AlertRequestHelper subscription);

		private partial AlertRequestHelper[] GetSubscriptions();

		internal partial class AlertRequestHelper
		{
			public partial void OnActionSheetRequested(Page sender, ActionSheetArguments arguments);

			public partial void OnAlertRequested(Page sender, AlertArguments arguments);

			public partial void OnPromptRequested(Page sender, PromptArguments arguments);

			public partial void OnPageBusy(Page sender, bool enabled);
		}
	}
}
