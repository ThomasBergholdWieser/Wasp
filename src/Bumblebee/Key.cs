using OpenQA.Selenium;

namespace Bumblebee;

/// <summary>
/// Represents the key on the keyboard.
/// </summary>
public class Key : IEquatable<Key>
{
	/// <summary>
	/// Represents the function keys found at the top of a standard keyboard.
	/// </summary>
	public static class Function
	{
		public static readonly Key F1 = new(Keys.F1, "F1");
		public static readonly Key F2 = new(Keys.F2, "F2");
		public static readonly Key F3 = new(Keys.F3, "F3");
		public static readonly Key F4 = new(Keys.F4, "F4");
		public static readonly Key F5 = new(Keys.F5, "F5");
		public static readonly Key F6 = new(Keys.F6, "F6");
		public static readonly Key F7 = new(Keys.F7, "F7");
		public static readonly Key F8 = new(Keys.F8, "F8");
		public static readonly Key F9 = new(Keys.F9, "F9");
		public static readonly Key F10 = new(Keys.F10, "F10");
		public static readonly Key F11 = new(Keys.F11, "F11");
		public static readonly Key F12 = new(Keys.F12, "F12");
	}

	/// <summary>
	/// Represents the arrow keys found to the right on a standard keyboard.
	/// </summary>
	public static class Arrows
	{
		public static readonly Key Up = new(Keys.Up, "Up");
		public static readonly Key Down = new(Keys.Down, "Down");
		public static readonly Key Left = new(Keys.Left, "Left");
		public static readonly Key Right = new(Keys.Right, "Right");
	}

	/// <summary>
	/// Represents the keys that are part of the standard 10-key on the far right of a standard keyboard.
	/// </summary>
	public static class Numpad
	{
		public static readonly Key NumberPad0 = new(Keys.NumberPad0, "NumberPad0");
		public static readonly Key NumberPad1 = new(Keys.NumberPad1, "NumberPad1");
		public static readonly Key NumberPad2 = new(Keys.NumberPad2, "NumberPad2");
		public static readonly Key NumberPad3 = new(Keys.NumberPad3, "NumberPad3");
		public static readonly Key NumberPad4 = new(Keys.NumberPad4, "NumberPad4");
		public static readonly Key NumberPad5 = new(Keys.NumberPad5, "NumberPad5");
		public static readonly Key NumberPad6 = new(Keys.NumberPad6, "NumberPad6");
		public static readonly Key NumberPad7 = new(Keys.NumberPad7, "NumberPad7");
		public static readonly Key NumberPad8 = new(Keys.NumberPad8, "NumberPad8");
		public static readonly Key NumberPad9 = new(Keys.NumberPad9, "NumberPad9");

		public static readonly Key Add = new(Keys.Add, "Add");
		public static readonly Key Subtract = new(Keys.Subtract, "Subtract");
		public static readonly Key Multiply = new(Keys.Multiply, "Multiply");
		public static readonly Key Divide = new(Keys.Divide, "Divide");

		public static readonly Key Decimal = new(Keys.Decimal, "Decimal");
	}

	/// <summary>
	/// Represents the keys not found in any of the other regions on the keyboard.
	/// </summary>
	public static class Other
	{
		public static readonly Key Insert = new(Keys.Insert, "Insert");
		public static readonly Key Delete = new(Keys.Delete, "Delete");
		public static readonly Key Home = new(Keys.Home, "Home");
		public static readonly Key End = new(Keys.End, "End");
		public static readonly Key PageUp = new(Keys.PageUp, "PageUp");
		public static readonly Key PageDown = new(Keys.PageDown, "PageDown");

		public static readonly Key Pause = new(Keys.Pause, "Pause");
	}

	public static readonly Key D0 = new("0", "0");
	public static readonly Key D1 = new("1", "1");
	public static readonly Key D2 = new("2", "2");
	public static readonly Key D3 = new("3", "3");
	public static readonly Key D4 = new("4", "4");
	public static readonly Key D5 = new("5", "5");
	public static readonly Key D6 = new("6", "6");
	public static readonly Key D7 = new("7", "7");
	public static readonly Key D8 = new("8", "8");
	public static readonly Key D9 = new("9", "9");

	public static readonly Key A = new("a", "A");
	public static readonly Key B = new("b", "B");
	public static readonly Key C = new("c", "C");
	public static readonly Key D = new("d", "D");
	public static readonly Key E = new("e", "E");
	public static readonly Key F = new("f", "F");
	public static readonly Key G = new("g", "G");
	public static readonly Key H = new("h", "H");
	public static readonly Key I = new("i", "I");
	public static readonly Key J = new("j", "J");
	public static readonly Key K = new("k", "K");
	public static readonly Key L = new("l", "L");
	public static readonly Key M = new("m", "M");
	public static readonly Key N = new("n", "N");
	public static readonly Key O = new("o", "O");
	public static readonly Key P = new("p", "P");
	public static readonly Key Q = new("q", "Q");
	public static readonly Key R = new("r", "R");
	public static readonly Key S = new("s", "S");
	public static readonly Key T = new("t", "T");
	public static readonly Key U = new("u", "U");
	public static readonly Key V = new("v", "V");
	public static readonly Key W = new("w", "W");
	public static readonly Key X = new("x", "X");
	public static readonly Key Y = new("y", "Y");
	public static readonly Key Z = new("z", "Z");

	public static readonly Key Alt = new(Keys.Alt, "Alt");
	public static readonly Key Apostrophe = new("'", "'");
	public static readonly Key Backslash = new(@"\", @"\");
	public static readonly Key Backspace = new(Keys.Backspace, "Backspace");
	public static readonly Key Comma = new(",", ",");
	public static readonly Key Command = new(Keys.Command, "Command");
	public static readonly Key Control = new(Keys.Control, "Control");
	public static readonly Key Enter = new(Keys.Enter, "Enter");
	public static readonly Key Equal = new(Keys.Equal, "Equal");
	public static readonly Key Escape = new(Keys.Escape, "Escape");
	public static readonly Key Grave = new("`", "`");
	public static readonly Key LeftBracket = new("[", "[");
	public static readonly Key Period = new(".", ".");
	public static readonly Key RightBracket = new("]", "]");
	public static readonly Key Semicolon = new(Keys.Semicolon, ";");
	public static readonly Key Shift = new(Keys.Shift, "Shift");
	public static readonly Key Slash = new("/", "/");
	public static readonly Key Space = new(Keys.Space, "Space");
	public static readonly Key Tab = new(Keys.Tab, "Tab");

	private readonly string _key;
	private readonly string _visualization;

	internal string Value { get { return _key; } }

	private Key(string key, string visualization)
	{
		_key = key;
		_visualization = visualization;
	}

	// TODO: provide a constructor (or implicit assignment operator?) for System.ConsoleKey and System.Windows.Forms.Keys???

	public static Key operator +(Key left, Key right)
	{
		return new Key($"{left.Value}{right.Value}", $"{left}+{right}");
	}

	public static Key operator |(Key left, Key right)
	{
		return new Key($"{left.Value}{right.Value}", $"{left}+{right}");
	}

	public bool Equals(Key other)
	{
		bool result;

		if (ReferenceEquals(null, other))
		{
			result = false;
		}
		else if (ReferenceEquals(this, other))
		{
			result = true;
		}
		else
		{
			result = String.Equals(_key, other._key);
		}

		return result;
	}

	public override bool Equals(object obj)
	{
		bool result;

		if (ReferenceEquals(null, obj))
		{
			result = false;
		}
		else if (ReferenceEquals(this, obj))
		{
			result = true;
		}
		else if (obj.GetType() != GetType())
		{
			result = false;
		}
		else
		{
			result = Equals((Key) obj);
		}

		return result;
	}

	public override int GetHashCode()
	{
		int result = 0;

		if (_key != null)
		{
			result = _key.GetHashCode();
		}

		return result;
	}

	public override string ToString()
	{
		return _visualization;
	}
}