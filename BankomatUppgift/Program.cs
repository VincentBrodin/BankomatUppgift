using System.Text.Json;

namespace BankomatUppgift {
	internal static class Program {
		private static void Main(string[] args) {
			// Ladda bank existerande bank information om det finns annars skapar vi random data
			Bank bank;

			if(File.Exists("bank.json")) {
				string json = File.ReadAllText("bank.json");
				bank = JsonSerializer.Deserialize<Bank>(json) ?? new Bank();
			}
			else {
				bank = new Bank();
			}

			if(bank.Accounts.Count == 0) {
				RandomAccounts(bank);
			}

			TerminalUi tui = new(bank);
			tui.StartProgramLoop();
		}

		private static void RandomAccounts(Bank bank, int amount = 10) {
			string[] firstNames = [
				"Erik", "Lars", "Johan", "Anders", "Per", "Karl", "Björn", "Fredrik", "Gustav", "Oskar",
				"Eva", "Anna", "Maria", "Lena", "Sofia", "Karin", "Elin", "Maja", "Ingrid", "Emelie"];

			string[] lastNames = [
				"Andersson", "Johansson", "Karlsson", "Nilsson", "Eriksson", "Larsson", "Olsson", "Persson", "Svensson", "Gustafsson",
				"Berg", "Lindberg", "Lundgren", "Ström", "Wikström", "Björk", "Nyström", "Lindholm", "Holm", "Sandström"];

			Random rnd = new();
			for(int i = 0; i < amount; i++) {
				string firstName = firstNames[rnd.Next(firstNames.Length)];
				string lastName = lastNames[rnd.Next(lastNames.Length)];
				DateOnly birthDay = DateOnly.FromDateTime(DateTime.Now);
				birthDay = birthDay.AddYears(-rnd.Next(18, 99));
				birthDay = birthDay.AddMonths(-rnd.Next(0, 11));
				birthDay = birthDay.AddDays(-rnd.Next(0, 364));
				Account account = bank.AddAccount(firstName, lastName, birthDay);
				// Tar ett random värde mellan 0-100,000 och sedan multiplicerar det med ett värde från 0-1 för att få decimal tal
				// Round * 100 / 100 gör så att värdet har max 2 decimaler
				decimal depositAmount = (decimal)Math.Round((rnd.NextDouble() * rnd.Next(0, 100000)*100)/100);
				account.Deposit(bank, depositAmount);
			}
		}
	}
}
