using System;

namespace Plugin.Maui.TvDPad;

public class DPadKeyEventArgs : EventArgs
{
	public DPadKey Key { get; }
	public DateTimeOffset Timestamp { get; } = DateTimeOffset.Now;
	public bool Handled { get; set; }
	public bool IsKeyDown { get; }

	public DPadKeyEventArgs(DPadKey key, bool isKeyDown)
	{
		Key = key;
		IsKeyDown = isKeyDown;
	}
}
