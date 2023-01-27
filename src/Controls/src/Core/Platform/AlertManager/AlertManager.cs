using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls.Internals;

namespace Microsoft.Maui.Controls.Platform
{
	internal partial class AlertManager
	{
		readonly Window _window;

		IAlertManagerSubscription? _subscription;

		public AlertManager(Window window)
		{
			_window = window;
		}

		public Window Window => _window;

		public IAlertManagerSubscription? Subscription => _subscription;

		public void Subscribe()
		{
			// this really should not be happening so we should probably log this
			if (_subscription is not null)
				return;

			var context = _window.MauiContext;

			_subscription =
				// try use services
				context.Services.GetService<IAlertManagerSubscription>() ??
				// fall back to the platform implementation and a "null implementation" on non-platforms
				CreateSubscription(context);

			// this really should not be happening so we should probably log this
			if (_subscription is null)
				return;
		}

		public void Unsubscribe() =>
			_subscription = null;

		public void RequestActionSheet(Page page, ActionSheetArguments arguments) =>
			_subscription?.OnActionSheetRequested(page, arguments);

		public void RequestAlert(Page page, AlertArguments arguments) =>
			_subscription?.OnAlertRequested(page, arguments);

		public void RequestPrompt(Page page, PromptArguments arguments) =>
			_subscription?.OnPromptRequested(page, arguments);

		public void RequestPageBusy(Page page, bool isBusy) =>
			_subscription?.OnPageBusy(page, isBusy);

		private partial IAlertManagerSubscription CreateSubscription(IMauiContext mauiContext);

		internal interface IAlertManagerSubscription
		{
			void OnActionSheetRequested(Page sender, ActionSheetArguments arguments);

			void OnAlertRequested(Page sender, AlertArguments arguments);

			void OnPromptRequested(Page sender, PromptArguments arguments);

			void OnPageBusy(Page sender, bool enabled);
		}

		internal partial class AlertRequestHelper : IAlertManagerSubscription
		{
			public partial void OnActionSheetRequested(Page sender, ActionSheetArguments arguments);

			public partial void OnAlertRequested(Page sender, AlertArguments arguments);

			public partial void OnPromptRequested(Page sender, PromptArguments arguments);

			public partial void OnPageBusy(Page sender, bool enabled);
		}
	}
}
