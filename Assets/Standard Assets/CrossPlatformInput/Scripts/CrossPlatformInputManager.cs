using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput.PlatformSpecific;

namespace UnityStandardAssets.CrossPlatformInput
{
	public static class CrossPlatformInputManager
	{
		public enum ActiveInputMethod { Hardware, Touch }

		private static VirtualInput activeInput;
		private static readonly VirtualInput touchInput;
		private static readonly VirtualInput hardwareInput;

		static CrossPlatformInputManager()
		{
			touchInput    = new MobileInput();
			hardwareInput = new StandaloneInput();
#if MOBILE_INPUT 
			activeInput = s_TouchInput;
#else
			activeInput = hardwareInput;
#endif
		}
		public static void SwitchActiveInputMethod(ActiveInputMethod activeInputMethod)
		{
			switch (activeInputMethod)
			{
				case ActiveInputMethod.Hardware:
					activeInput = hardwareInput;
					break;
				case ActiveInputMethod.Touch:
					activeInput = touchInput;
					break;
			}
		}
		public static bool AxisExists(string name)   => activeInput.AxisExists(name);
		public static bool ButtonExists(string name) => activeInput.ButtonExists(name);
		public static void RegisterVirtualAxis(VirtualAxis axis) => activeInput.RegisterVirtualAxis(axis);
		public static void RegisterVirtualButton(VirtualButton button) => activeInput.RegisterVirtualButton(button);
		public static void UnRegisterVirtualAxis(string name)
		{
			if (name == null)
				throw new ArgumentNullException("name");
			activeInput.UnRegisterVirtualAxis(name);
		}
		public static void UnRegisterVirtualButton(string name) => activeInput.UnRegisterVirtualButton(name);

		// Returns a reference to a named virtual axis if it exists otherwise null
		public static VirtualAxis VirtualAxisReference(string name) => activeInput.VirtualAxisReference(name);
		
		// Returns the platform appropriate axis for the given name
		public static float GetAxis(string name)    => GetAxis(name, false);
		public static float GetAxisRaw(string name) => GetAxis(name, true);

		// Private function handles both types of axis (raw and not raw)
		private static float GetAxis(string name, bool raw) => activeInput.GetAxis(name, raw);

		// -- Button handling -- //
		public static bool GetButton      (string name)      => activeInput.GetButton(name);
		public static bool GetButtonDown  (string name)      => activeInput.GetButtonDown(name);
		public static bool GetButtonUp    (string name)      => activeInput.GetButtonUp(name);
		public static void SetButtonDown  (string name)      => activeInput.SetButtonDown(name);
		public static void SetButtonUp    (string name)      => activeInput.SetButtonUp(name);
		public static void SetAxisPositive(string name)      => activeInput.SetAxisPositive(name);
		public static void SetAxisNegative(string name)      => activeInput.SetAxisNegative(name);
		public static void SetAxisZero    (string name)      => activeInput.SetAxisZero(name);
		public static void SetAxis(string name, float value) => activeInput.SetAxis(name, value);
		public static Vector3 MousePosition                  => activeInput.MousePosition();
		public static void SetVirtualMousePositionX(float f) => activeInput.SetVirtualMousePositionX(f);
		public static void SetVirtualMousePositionY(float f) => activeInput.SetVirtualMousePositionY(f);
		public static void SetVirtualMousePositionZ(float f) => activeInput.SetVirtualMousePositionZ(f);

		// Virtual axis and button classes - applies to mobile input
		// Can be mapped to touch joysticks, tilt, gyro, etc, depending on desired implementation.
		// Could also be implemented by other input devices - kinect, electronic sensors, etc
		public class VirtualAxis
		{
			public string Name { get; private set; }
			public bool MatchWithInputManager { get; private set; }

			public VirtualAxis(string name) : this(name, true) { }
			public VirtualAxis(string name, bool matchToInputSettings) => (Name, MatchWithInputManager) = (name, matchToInputSettings);

			public void Remove() => UnRegisterVirtualAxis(Name);
			public void Update(float value) => this.GetValue = value;
			public float GetValue { get; private set; }
			public float GetValueRaw => GetValue;
		}

		// A controller gameobject (eg. a virtual GUI button) should call the
		// 'Pressed' function of this class. Other objects can then read the
		// Get/Down/Up state of this button.
		public class VirtualButton
		{
			public string Name { get; private set; }
			public bool MatchWithInputManager { get; private set; }

			private int lastPressedFrame = -5;
			private int releasedFrame = -5;

			public VirtualButton(string name) : this(name, true) { }
			public VirtualButton(string name, bool matchToInputSettings) => (this.Name, MatchWithInputManager) = (name, matchToInputSettings);

			// A controller gameobject should call this function when the button is pressed down
			public void Pressed()
			{
				if (GetButton) return;

				GetButton = true;
				lastPressedFrame = Time.frameCount;
			}

			// A controller gameobject should call this function when the button is released
			public void Released() => (GetButton, releasedFrame) = (false, Time.frameCount);

			// The controller gameobject should call Remove when the button is destroyed or disabled
			public void Remove() => UnRegisterVirtualButton(Name);

			// These are the states of the button which can be read via the cross platform input system
			public bool GetButton { get; private set; }
			public bool GetButtonDown => lastPressedFrame - Time.frameCount == -1;
			public bool GetButtonUp   => (releasedFrame == Time.frameCount - 1);
		}
	}
}