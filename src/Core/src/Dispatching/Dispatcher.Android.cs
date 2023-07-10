﻿using System;
using Android.OS;
using Android.Systems;
using Java.Lang;

namespace Microsoft.Maui.Dispatching
{
	/// <inheritdoc/>
	public partial class Dispatcher : IDispatcher
	{
		readonly Looper _looper;
		readonly Handler _handler;

		internal Dispatcher(Looper looper)
		{
			_looper = looper ?? throw new ArgumentNullException(nameof(looper));
			_handler = new Handler(_looper);
		}

		bool IsDispatchRequiredImplementation() =>
			_looper != Looper.MyLooper();

		bool DispatchImplementation(Action action) =>
			_handler.Post(() => action());

		bool DispatchDelayedImplementation(TimeSpan delay, Action action) =>
			_handler.PostDelayed(() => action(), (long)delay.TotalMilliseconds);

		IDispatcherTimer CreateTimerImplementation()
		{
			return new DispatcherTimer(_handler);
		}
	}

	/// <inheritdoc/>
	partial class DispatcherTimer : IDispatcherTimer
	{
		readonly Handler _handler;
		readonly IRunnable _runnable;
		bool _hasCallbacks;

		/// <summary>
		/// Initializes a new instance of the <see cref="DispatcherTimer"/> class.
		/// </summary>
		/// <param name="handler">The handler for this dispatcher to use.</param>
		public DispatcherTimer(Handler handler)
		{
			_runnable = new Runnable(OnTimerTick);
			_handler = handler;
		}

		/// <inheritdoc/>
		public TimeSpan Interval { get; set; }

		/// <inheritdoc/>
		public bool IsRepeating { get; set; } = true;

		/// <inheritdoc/>
		public bool IsRunning { get; private set; }

		/// <inheritdoc/>
		public event EventHandler? Tick;

		/// <inheritdoc/>
		public void Start()
		{
			if (IsRunning)
				return;

			IsRunning = true;

			Post();
		}

		void Post()
		{
			if (IsCallbackPosted())
			{
				return;
			}

			_handler.PostDelayed(_runnable, (long)Interval.TotalMilliseconds);

			if (!OperatingSystem.IsAndroidVersionAtLeast(29))
			{
				_hasCallbacks = true;
			}
		}

		bool IsCallbackPosted()
		{
			if (OperatingSystem.IsAndroidVersionAtLeast(29) && _handler.HasCallbacks(_runnable))
			{
				return true;
			}

			return _hasCallbacks;
		}

		/// <inheritdoc/>
		public void Stop()
		{
			if (!IsRunning)
				return;

			IsRunning = false;

			_handler.RemoveCallbacks(_runnable);

			if (!OperatingSystem.IsAndroidVersionAtLeast(29))
			{
				_hasCallbacks = false;
			}
		}

		void OnTimerTick()
		{
			if (!IsRunning)
				return;
			
			if (!OperatingSystem.IsAndroidVersionAtLeast(29))
			{
				_hasCallbacks = false;
			}

			Tick?.Invoke(this, EventArgs.Empty);

			if (IsRepeating)
			{
				Post();
			}
			else
			{
				Stop();
			}
		}
	}

	/// <inheritdoc/>
	public partial class DispatcherProvider
	{
		static IDispatcher? GetForCurrentThreadImplementation()
		{
			var q = Looper.MyLooper();
			if (q == null || q != Looper.MainLooper)
				return null;

			return new Dispatcher(q);
		}
	}
}