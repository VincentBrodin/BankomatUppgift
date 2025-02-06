using System.Text.Json;

namespace BankomatUppgift;
internal class TerminalUi {
	private readonly Bank bank;

	public TerminalUi(Bank bank) {
		this.bank = bank;
	}


	public void StartProgramLoop() {
		while(MainMenu()) ;
	}

	public bool MainMenu() {
		const string prompt = "Vad vill du göra: ";
		List<string> options = ["Sätt in pengar", "Ta ut pengar", "Överför pengar", "Visa alla konton", "Skapa konto", "Tabort konto", "Avsluta programmet"];

		Tools.SetPosition(0, 0);
		Console.Clear();
		Menu menu = new(prompt, options);
		switch(menu.GetValue()) {
			case 0:
				DepositUi();
				break;

			case 1:
				WithdrawalUi();
				break;

			case 2:
				TransferUi();
				break;
			case 3:
				ListAccountsUi();
				break;
			//case 4:
				//ListAllReceiptsUi(); Buggy
				//break;
			case 4:
				AddAccountUi();
				break;
			case 5:
				RemoveAccountUi();
				break;
			default:
				// Spara ändringar till bank.json
				string json = JsonSerializer.Serialize(bank);
				File.WriteAllText("bank.json", json);
				return false;
		}
		return true;
	}



	public void DepositUi() {
		Tools.SetPosition(0, 0);
		Console.Clear();
		const string prompt = "Välj ett konto att föra in pengar i: ";

		Menu menu = new(prompt, bank.ListAllAccounts());
		int index = menu.GetValue();
		if(index == -1) {
			Tools.WriteError("Överföringen avbröts av användaren.");
			Tools.PromptInputToContinue();
			return;
		}
		Account account = bank.Accounts[index];


		while(true) {
			Console.Write($"Hur mycket vill du sätta in på {account.FullName} ({account.AmountString})? (Skriv \"exit\" för att avbryta): ");
			string? amountString = Console.ReadLine();

			if(amountString == "exit") {
				Tools.WriteError("Överföringen avbröts av användaren.");
				Tools.PromptInputToContinue();
				break;
			}

			// Try to parse the input amount
			if(decimal.TryParse(amountString, out decimal amount)) {
				Console.WriteLine($"Är du säker på att du vill sätta in {Tools.ToKr(amount)} på {account.FullName}?");
				Console.WriteLine($"Nytt saldo: {Tools.ToKr(amount + account.Balance)}");
				Console.Write("Bekräfta [y/n]: ");
				string? confirmation = Console.ReadLine();

				if(confirmation == "y") {
					if(account.Deposit(bank, amount, true)) {
						Tools.WriteSuccsess($"{Tools.ToKr(amount)} har satts in på {account.FullName}.");
						Tools.PromptInputToContinue();
						return;
					}
					else {
						Tools.WriteError("Överföringen avbröts av banken.");
						Tools.PromptInputToContinue();
						return;
					}
				}
				else {
					Tools.WriteError("Överföringen avbröts av användaren.");
					Tools.PromptInputToContinue();
					return;
				}
			}
			else {
				Console.WriteLine($"Kunde inte omvandla '{amountString}' till ett giltigt nummer.");
			}
		}
	}

	public void WithdrawalUi() {
		Tools.SetPosition(0, 0);
		Console.Clear();
		const string prompt = "Välj ett konto att ta ut pengar ifrån: ";

		Menu menu = new(prompt, bank.ListAllAccounts());
		int index = menu.GetValue();
		if(index == -1) {

			Tools.WriteError("Överföringen avbröts av användaren.");
			Tools.PromptInputToContinue();
			return;
		}
		Account account = bank.Accounts[index];

		while(true) {
			Console.Write($"Hur mycket vill du ta ut från {account.FullName} ({account.AmountString})? (Skriv \"exit\" för att avbryta): ");
			string? amountString = Console.ReadLine();

			if(amountString == "exit") {

				Tools.WriteError("Överföringen avbröts av användaren.");
				Tools.PromptInputToContinue();
				break;
			}

			// Try to parse the input amount
			if(decimal.TryParse(amountString, out decimal amount)) {
				Console.WriteLine($"Är du säker på att du vill ta ut {Tools.ToKr(amount)} från {account.FullName}?");
				Console.WriteLine($"Nytt saldo: {Tools.ToKr(account.Balance - amount)}");
				Console.Write("Bekräfta [y/n]: ");
				string? confirmation = Console.ReadLine();

				if(confirmation == "y") {
					if(account.Withdrawal(bank, amount, true)) {
						Tools.WriteSuccsess($"{Tools.ToKr(amount)} har tagits ut från {account.FullName}.");
						Tools.PromptInputToContinue();
						return;
					}
					else {

						Tools.WriteError("Överföringen avbröts av banken.");
						Tools.PromptInputToContinue();
						return;
					}
				}
				else {

					Tools.WriteError("Överföringen avbröts av användaren.");
					Tools.PromptInputToContinue();
					return;
				}
			}
			else {
				Tools.WriteError($"Kunde inte omvandla '{amountString}' till ett giltigt nummer.");
			}
		}
	}

	public void TransferUi() {
		Tools.SetPosition(0, 0);
		Console.Clear();

		Menu menu = new("Välj ett konto att ta pengar ifrån: ", bank.ListAllAccounts());
		int index = menu.GetValue();
		if(index == -1) {
			Tools.WriteError("Överföringen avbröts av användaren.");
			Tools.PromptInputToContinue();
			return;
		}
		bool[] state = new bool[bank.Accounts.Count];
		state[index] = true;
		Account accountA = bank.Accounts[index];

		Tools.SetPosition(0, 0);
		Console.Clear();
		menu = new Menu("Välj ett konto att föra in pengar i: ", bank.ListAllAccounts(), state, state);

		index = menu.GetValue(index);
		if(index == -1) {
			Tools.WriteError("Överföringen avbröts av användaren.");
			Tools.PromptInputToContinue();
			return;
		}
		Account accountB = bank.Accounts[index];
		Account account = bank.Accounts[index];

		while(true) {
			Console.Write($"Hur mycket vill du överföra från {accountA.FullName} ({accountA.AmountString}) " +
						  $"till {accountB.FullName} ({accountB.AmountString})? (Skriv \"exit\" för att avbryta): ");
			string? amountString = Console.ReadLine();

			if(amountString == "exit") {
				Tools.WriteError("Överföringen avbröts av användaren.");
				Tools.PromptInputToContinue();

				break;
			}

			// Try to parse the input amount
			if(decimal.TryParse(amountString, out decimal amount)) {
				Console.WriteLine($"Är du säker på att du vill överföra {Tools.ToKr(amount)} " +
								  $"från {accountA.FullName} till {accountB.FullName}?");
				Console.WriteLine($"{accountA.FullName} – nytt saldo: {Tools.ToKr(accountA.Balance - amount)}");
				Console.WriteLine($"{accountB.FullName} – nytt saldo: {Tools.ToKr(accountB.Balance + amount)}");
				Console.Write("Bekräfta [y/n]: ");
				string? confirmation = Console.ReadLine();

				if(confirmation == "y") {
					if(accountA.Transfer(bank, accountB, amount, true)) {
						Tools.WriteSuccsess($"{Tools.ToKr(amount)} har överförts från {accountA.FullName} till {accountB.FullName}.");
						Tools.PromptInputToContinue();
						return;
					}
					else {

						Tools.WriteError("Överföringen avbröts av banken.");
						Tools.PromptInputToContinue();
						return;
					}
				}
				else {
					Tools.WriteError("Överföringen avbröts av användaren.");
					Tools.PromptInputToContinue();
					return;
				}
			}
			else {
				Tools.WriteError($"Kunde inte omvandla '{amountString}' till ett giltigt nummer.");
			}
		}
	}

	public void AddAccountUi() {
		Tools.SetPosition(0, 0);
		Console.Clear();

		Console.WriteLine("Konto skaparen");
		Console.WriteLine("Skriv \"exit\" för att avbryta");
		Console.Write("Förnamn: ");
		string? firstName = Console.ReadLine();
		while(string.IsNullOrEmpty(firstName)) {
			firstName = Console.ReadLine();
		}
		if(firstName == "exit") {
			Tools.WriteError("Konto skapning avbröts av användaren.");
			Tools.PromptInputToContinue();
			return;
		}

		Console.Write("Efternamn: ");
		string? lastName = Console.ReadLine();
		while(string.IsNullOrEmpty(lastName)) {
			lastName = Console.ReadLine();
		}
		if(lastName == "exit") {
			Tools.WriteError("Konto skapning avbröts av användaren.");
			Tools.PromptInputToContinue();
			return;
		}

		Console.Write("Födelse dag: ");
		string? birthDayString = Console.ReadLine();
		DateOnly birthDay = new();
		while(string.IsNullOrEmpty(lastName) && DateOnly.TryParse(birthDayString, out birthDay)) {
			if(birthDayString == "exit") {
				Tools.WriteError("Konto skapning avbröts av användaren.");
				Tools.PromptInputToContinue();
				return;
			}

			birthDayString = Console.ReadLine();
		}

		Console.WriteLine($"Är du säker på att du vill skpa {firstName} {lastName}s konto?");
		Console.Write("Bekräfta [y/n]: ");
		string? confirmation = Console.ReadLine();
		if(confirmation == "y") {
			bank.AddAccount(firstName, lastName, birthDay);
			Tools.WriteSuccsess("Konto skapats");
			Tools.PromptInputToContinue();
		}
		else {
			Tools.WriteError("Konto skapning avbröts av användaren.");
			Tools.PromptInputToContinue();
		}
	}

	public void RemoveAccountUi() {
		Tools.SetPosition(0, 0);
		Console.Clear();
		const string prompt = "Välj ett konto att tabort: ";

		Menu menu = new(prompt, bank.ListAllAccounts());
		int index = menu.GetValue();
		if(index == -1) {

			Tools.WriteError("Konto bortagning avslutades av användaren.");
			Tools.PromptInputToContinue();
			return;
		}
		Account account = bank.Accounts[index];

		Console.WriteLine($"Är du säker på att du vill ta {account.FullName}s konto?");
		Console.Write("Bekräfta [y/n]: ");
		string? confirmation = Console.ReadLine();

		if(confirmation == "y") {
			bank.RemoveAccount(account);
			Tools.WriteSuccsess("Konto bortagning lyckades.");
			Tools.PromptInputToContinue();
		}
		else {
			Tools.WriteError("Konto bortagning misslyckades.");
			Tools.PromptInputToContinue();
		}
	}

	public void ListAccountsUi() {
		int namePadding = bank.Accounts.Max(a => a.FullName.Length);
		const string prompt = "Välj konto";
		// Prepare options for the menu
		List<string> options = bank.ListAllAccounts();
		options.Add("Visa information");
		bool[] state = new bool[options.Count];
		int startIndex = 0;
		while(true) {
			Tools.SetPosition(0, 0);
			Console.Clear();

			Menu menu = new(prompt, options, state);
			int index = menu.GetValue(startIndex);
			if(index == -1) {
				return;
			}
			else if(index == options.Count - 1) {
				break;
			}
			startIndex = index;

			state[index] = !state[index];
		}
		for(int i = 0; i < state.Length; i++) {
			if(!state[i]) { continue; }
			Account account = bank.Accounts[i];
			account.LookUp(bank, true);
			Tools.WriteSuccsess($"[{account.Id}] {account.FullName} ({account.BirthDay} - {account.Age} år) - {account.AmountString} | Skapat - {account.CreatedDate}");
		}
		Tools.PromptInputToContinue();
	}
	public void ListAllReceiptsUi() {
		const string prompt = "Välj tranaktion";
		List<string> options = bank.ListAllReceipts();
		options.Add("Visa information");
		bool[] state = new bool[options.Count];
		int startIndex = 0;
		while(true) {
			Tools.SetPosition(0, 0);
			Console.Clear();

			Menu menu = new(prompt, options, state);
			int index = menu.GetValue(startIndex);
			if(index == -1) {
				return;
			}
			else if(index == options.Count - 1) {
				break;
			}
			startIndex = index;

			state[index] = !state[index];
		}
		for(int i = 0; i < state.Length; i++) {
			if(!state[i]) { continue; }
			Receipt receipt = bank.Receipts[i];
			Tools.WriteSuccsess($"{receipt.TransactionId}");
		}
		Tools.PromptInputToContinue();
	}
}
