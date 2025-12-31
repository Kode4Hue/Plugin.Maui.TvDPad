using System;

namespace Plugin.Maui.TvDPad;

public class FocusNavigationEventArgs : EventArgs
{
	public DPadKey Direction { get; }
	public object? NextFocusElement { get; set; }
	public bool Handled { get; set; }

	public FocusNavigationEventArgs(DPadKey direction)
	{
		Direction = direction;
	}
}
