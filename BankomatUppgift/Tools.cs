using System.Globalization;

namespace BankomatUppgift;
internal static class Tools {
	public static void SetPosition(int left, int top) {
		if(Console.BufferHeight <= top) {
			Console.BufferHeight = top + 1;
		}
		if(Console.BufferWidth <= left) {
			Console.BufferWidth = left + 1;
		}
		Console.SetCursorPosition(left, top);
	}

	public static void WriteSuccsess(string content) {
		Console.ForegroundColor = ConsoleColor.Green;
		Console.WriteLine(content);
		Console.ResetColor();
	}
	public static void WriteError(string content) {
		Console.ForegroundColor = ConsoleColor.Red;
		Console.WriteLine(content);
		Console.ResetColor();

	}
	public static void PromptInputToContinue() {
		Console.WriteLine("Tryck på Enter för att fortsätta.");
		Console.ReadKey(true);
	}

	public static string ToKr(decimal amount) {
		return amount.ToString("C", new CultureInfo("sv-SE"));
	}

}
