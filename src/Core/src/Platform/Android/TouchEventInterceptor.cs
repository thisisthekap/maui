using Android.Views;

namespace Microsoft.Maui.Platform
{
	/// <summary>
	/// A helper class to correctly intercept touches if a view is overlapping another view.
	/// </summary>
	/// <remarks>
	/// Normally dispatchTouchEvent feeds the touch events to its children one at a time, top child first,
	/// (and only to the children in the hit-test area of the event) stopping as soon as one of them has handled
	/// the event.
	/// <br/><br/>
	/// But to be consistent across the platforms, we don't want this behavior; if an element is not input transparent
	/// we don't want an event to "pass through it" and be handled by an element "behind/under" it. We just want the processing
	/// to end after the first non-transparent child, regardless of whether the event has been handled.
	/// <br/><br/>
	/// This is only an issue for a couple of controls; the interactive controls (switch, button, slider, etc) already "handle" their touches
	/// and the events don't propagate to other child controls. But for image, label, and box that doesn't happen. We can't have those controls
	/// lie about their events being handled because then the events won't propagate to *parent* controls (e.g., a frame with a label in it would
	/// never get a tap gesture from the label). In other words, we *want* parent propagation, but *do not want* sibling propagation. So we need to short-circuit
	/// base.DispatchTouchEvent here, but still return "false".
	/// <br/><br/>
	/// Duplicating the logic of ViewGroup.dispatchTouchEvent and modifying it slightly for our purposes is a non-starter; the method is too
	/// complex and does a lot of micro-optimization. Instead, we provide a signalling mechanism for the controls which don't already "handle" touch
	/// events to tell us that they will be lying about handling their event; they then return "true" to short-circuit base.DispatchTouchEvent.
	/// <br/><br/>
	/// The container gets this message and after it gets the "handled" result from dispatchTouchEvent,
	/// it then knows to ignore that result and return false/unhandled. This allows the event to propagate up the tree.
	/// </remarks>
	static class TouchEventInterceptor
	{
		/// <summary>
		/// This method should be called by any view that will "intercept" the touches.
		/// </summary>
		/// <param name="view">The view that is recieving the touch event.</param>
		/// <param name="e">The touch event.</param>
		/// <returns>Returns true if the touch was "handled", false if the view does not want to do anything.</returns>
		public static bool OnTouchEvent(View? view, MotionEvent? e)
		{
			if (view is null || !view.IsAlive())
				return false;

			if (e is null || e.Action == MotionEventActions.Cancel)
				return false;

			var parent = view.Parent as ITouchInterceptingView;
			if (parent is null || ShouldPassThroughElement(parent))
				return false;

			// Let the container know that we're "fake" handling this event.
			parent.TouchEventNotReallyHandled = true;
			return true;
		}

		/// <summary>
		/// This method should only be called before base.DispatchTouchEvent()
		/// and by the layout that handles the interception logic.
		/// </summary>
		public static bool DispatchingTouchEvent<T>(T layout, MotionEvent? e)
			where T : View, ITouchInterceptingView
		{
			layout.TouchEventNotReallyHandled = false;

			// Always return true because this always happens.
			return true;
		}

		/// <summary>
		/// This method should only be called after base.DispatchTouchEvent()
		/// and by the layout that handles the interception logic.
		/// </summary>
		public static bool DispatchedTouchEvent<T>(T layout, MotionEvent? e, View.IOnTouchListener? touchListener)
			where T : View, ITouchInterceptingView
		{
			if (layout.TouchEventNotReallyHandled)
			{
				// If the child control returned true from its touch event handler but signalled that it was a fake "true", then we
				// don't consider the event truly "handled" yet.

				// Since a child control short-circuited the normal dispatchTouchEvent stuff, this layout never got the chance for
				// IOnTouchListener.OnTouch and the OnTouchEvent override to try handling the touches; we'll do that now.

				// Any associated Touch Listeners are called from DispatchTouchEvents if all children of this view return false
				// So here we are simulating both calls that would have typically been called from inside DispatchTouchEvent
				// but were not called due to the fake "true".

				var result = touchListener?.OnTouch(layout, e) ?? false;
				return result || layout.OnTouchEvent(e);
			}

			return true;
		}

		static bool ShouldPassThroughElement(ITouchInterceptingView layout)
		{
			// If the layout is not input transparent, then the event should not pass through it
			if (!layout.InputTransparent)
				return false;

			// If the event is being bubbled up from a child which is not input transparent, we do not want
			// it to be passed through (just up the tree)
			if (layout.TouchEventNotReallyHandled)
				return false;

			return false;
		}

		public static readonly View.IOnTouchListener Listener = new OnTouchListener();

		/// <summary>
		/// Set up a view to "intercept" touch events without inheritance.
		/// </summary>
		/// <remarks>
		/// Non-interactive controls also need to "block" touch events if they are covering
		/// other interactive controls.
		/// </remarks>
		public static void ConnectListener(View view) =>
			view.SetOnTouchListener(Listener);

		/// <summary>
		/// Tear down the listener for a view that is "intercepting" touch events without inheritance.
		/// </summary>
		public static void DisconnectListener(View view) =>
			view.SetOnTouchListener(null);

		class OnTouchListener : Java.Lang.Object, View.IOnTouchListener
		{
			public bool OnTouch(View? v, MotionEvent? e) => OnTouchEvent(v, e);
		}
	}

	interface ITouchInterceptingView
	{
		bool InputTransparent { get; set; }

		bool TouchEventNotReallyHandled { get; set; }
	}
}
