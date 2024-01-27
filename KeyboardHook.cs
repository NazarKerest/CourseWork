using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CourseWork
{
	public class KeyboardHook : IDisposable
	{
		private const int WH_KEYBOARD_LL = 13;
		private const int WM_KEYDOWN = 0x0100;
		private const int WM_KEYUP = 0x0101;

		private readonly LowLevelKeyboardProc _proc;
		private IntPtr _hookId = IntPtr.Zero;

		public event EventHandler<KeyPressedEventArgs> KeyPressed;

		public KeyboardHook()
		{
			_proc = HookCallback;
			_hookId = SetHook(_proc);
		}

		public void Dispose()
		{
			UnhookWindowsHookEx(_hookId);
		}

		private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

		private IntPtr SetHook(LowLevelKeyboardProc proc)
		{
			using (ProcessModule curModule = Process.GetCurrentProcess().MainModule)
			{
				return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
			}
		}

		private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
		{
			if (nCode >= 0 && (wParam == (IntPtr)WM_KEYDOWN || wParam == (IntPtr)WM_KEYUP))
			{
				int vkCode = Marshal.ReadInt32(lParam);
				OnKeyPressed(new KeyPressedEventArgs((Keys)vkCode, vkCode));
			}

			return CallNextHookEx(_hookId, nCode, wParam, lParam);
		}


		protected virtual void OnKeyPressed(KeyPressedEventArgs e)
		{
			KeyPressed?.Invoke(this, e);
		}

		#region WinAPI imports

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool UnhookWindowsHookEx(IntPtr hhk);

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern IntPtr GetModuleHandle(string lpModuleName);

		#endregion
	}

	public class KeyPressedEventArgs : EventArgs
	{
		public Keys Key { get; }
		public int KeyValue { get; }

		public KeyPressedEventArgs(Keys key, int keyValue)
		{
			Key = key;
			KeyValue = keyValue;	
		}
	}
}
