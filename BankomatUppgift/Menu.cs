namespace BankomatUppgift;
internal class Menu {
	private readonly string prompt;
	private readonly string[] options;
	private readonly bool[] state;
	private readonly bool[] disabled;

	private const string TUTORIAL = "[Använd pil upp, ner och enter för att välja. ESC för att stänga]";

	public Menu(string prompt, string[] options) {
		this.prompt = prompt;
		this.options = options;
		state = new bool[options.Length];
		disabled = new bool[options.Length];
	}
	public Menu(string prompt, string[] options, bool[] state, bool[] disabled) {
		this.prompt = prompt;
		this.options = options;
		this.state = state;
		this.disabled = disabled;
	}

	public int GetValue(int startIndex = 0) {
		Console.WriteLine($"{prompt}");
		Console.WriteLine(TUTORIAL);

		(_, int offset) = Console.GetCursorPosition();
		int max = offset + options.Length - 1;
		Tools.SetPosition(0, max);
		Tools.SetPosition(0, offset);

		Console.ForegroundColor = ConsoleColor.DarkGray;
		for(int i = 0; i < options.Length; i++) {
			string option = options[i];
			if(state[i]) {
				Console.ForegroundColor = ConsoleColor.DarkGreen;
				Console.WriteLine("[#] " + option);
			}
			else {
				Console.ForegroundColor = ConsoleColor.DarkGray;
				Console.WriteLine("[ ] " + option);
			}
		}
		Console.ResetColor();


		Tools.SetPosition(1, offset + startIndex);

		while(true) {

			(int left, int top) = Console.GetCursorPosition();
			int index = top - offset;

			Tools.SetPosition(0, top);
			Console.Write("[ ] " + options[index]);
			Tools.SetPosition(left, top);

			ConsoleKey key = Console.ReadKey(true).Key;

			Tools.SetPosition(0, top);
			if(state[index]) {
				Console.ForegroundColor = ConsoleColor.DarkGreen;
				Console.Write("[#] " + options[index]);
			}
			else {
				Console.ForegroundColor = ConsoleColor.DarkGray;
				Console.Write("[ ] " + options[index]);
			}
			Tools.SetPosition(left, top);
			Console.ResetColor();

			if(key is ConsoleKey.UpArrow or ConsoleKey.K) {
				top--;
			}
			else if(key is ConsoleKey.DownArrow or ConsoleKey.J) {
				top++;
			}
			else if(key is ConsoleKey.Enter && !disabled[index]) {
				Tools.SetPosition(0, top);
				Console.ForegroundColor = ConsoleColor.DarkGreen;
				Console.Write("[#] " + options[index]);
				Console.ResetColor();
				Tools.SetPosition(0, max + 1);
				return index;
			}
			else if(key is ConsoleKey.Escape) {
				Tools.SetPosition(0, max + 1);
				return -1;
			}

			// Loop
			if(top > max) {
				top = offset;
			}
			else if(top < offset) {
				top = max;
			}
			Tools.SetPosition(left, top);
		}
	}
}
