using Godot;
using System;
using System.Collections.Generic;
using WiimoteLib;

[Tool]
[GlobalClass]
public partial class WiimoteService : Node
{
	private List<Wiimote> _wiimotes = new();
	private WiimoteCollection _collection;

	[Signal]
	public delegate void ButtonPressedEventHandler(int id, string button);

	[Signal]
	public delegate void NunchukButtonPressedEventHandler(int id, string button);

	[Signal]
	public delegate void NunchukJoystickMovedEventHandler(int id, float stickX, float stickY);

	public override void _Ready()
	{
		GD.Print("WiimoteService initialized.");
		TryConnectAll();
	}

	private void TryConnectAll()
	{
		try
		{
			_collection = new WiimoteCollection();
			_collection.FindAllWiimotes();

			for (int i = 0; i < _collection.Count; i++)
			{
				var wm = _collection[i];
				wm.Connect();
				wm.WiimoteChanged += (s, e) => OnWiiChanged(i, e);
				_wiimotes.Add(wm);
				GD.Print($"Connected Wiimote #{i}");
			}
		}
		catch (Exception e)
		{
			GD.PrintErr("Failed to connect Wiimotes: ", e.Message);
		}
	}

	private void OnWiiChanged(int id, WiimoteChangedEventArgs args)
	{
		var state = args.WiimoteState;

		var b = state.ButtonState;

		// Buttons
		if (b.A) CallDeferred(MethodName.EmitSignal, SignalName.ButtonPressed, id, "A");
		if (b.B) CallDeferred(MethodName.EmitSignal, SignalName.ButtonPressed, id, "B");
		if (b.Home) CallDeferred(MethodName.EmitSignal, SignalName.ButtonPressed, id, "Home");
		if (b.Plus) CallDeferred(MethodName.EmitSignal, SignalName.ButtonPressed, id, "Plus");
		if (b.Minus) CallDeferred(MethodName.EmitSignal, SignalName.ButtonPressed, id, "Minus");
		if (b.Down) CallDeferred(MethodName.EmitSignal, SignalName.ButtonPressed, id, "Down");
		if (b.Up) CallDeferred(MethodName.EmitSignal, SignalName.ButtonPressed, id, "Up");
		if (b.Left) CallDeferred(MethodName.EmitSignal, SignalName.ButtonPressed, id, "Left");
		if (b.Right) CallDeferred(MethodName.EmitSignal, SignalName.ButtonPressed, id, "Right");
		if (b.One) CallDeferred(MethodName.EmitSignal, SignalName.ButtonPressed, id, "1");
		if (b.Two) CallDeferred(MethodName.EmitSignal, SignalName.ButtonPressed, id, "2");

		// Nunchuk
		if (state.ExtensionType == ExtensionType.Nunchuk)
		{
			var n = state.NunchukState;

			if (n.C) CallDeferred(MethodName.EmitSignal, SignalName.NunchukButtonPressed, id, "C");
			if (n.Z) CallDeferred(MethodName.EmitSignal, SignalName.NunchukButtonPressed, id, "Z");

			CallDeferred(
				MethodName.EmitSignal,
				SignalName.NunchukJoystickMoved,
				id,
				n.Joystick.X,
				n.Joystick.Y
			);
		}
	}

	public void SetRumble(int id, bool on)
	{
		if (id >= 0 && id < _wiimotes.Count)
			_wiimotes[id]?.SetRumble(on);
	}

	public void SetLed(int id, int ledIndex, bool on)
	{
		if (id < 0 || id >= _wiimotes.Count) return;

		byte mask = 0;
		if (on)
			mask = (byte)(1 << (ledIndex - 1));

		_wiimotes[id].SetLEDs(mask);
	}

	public override void _ExitTree()
	{
		foreach (var wm in _wiimotes)
		{
			wm?.Disconnect();
			wm?.Dispose();
		}
	}
}
