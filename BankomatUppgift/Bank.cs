
namespace BankomatUppgift;
internal class Bank {
	public List<Account> Accounts { get; init; }
	public List<Receipt> Receipts { get; init; }
	public Bank() {
		Accounts = [];
		Receipts = [];
	}

	internal Account AddAccount(string firstName, string lastName, DateOnly birthday) {
		Account account = new(firstName, lastName, birthday);
		Accounts.Add(account);
		return account;
	}

	internal void RemoveAccount(Account account) {
		Accounts.Remove(account);
	}

	internal List<string> ListAllAccounts() {
		int namePadding = Accounts.Max(a => a.FullName.Length);
		int amountPadding = Accounts.Max(a => a.AmountString.Length);
		return Accounts.ConvertAll(a => $"[{a.Id}] {a.FullName.PadRight(namePadding)} {a.AmountString.PadLeft(amountPadding)}");
	}

	internal List<string> ListAllReceipts() {
		return Receipts.OrderByDescending(r => r.Time).ToList().ConvertAll(r => $"{r.TransactionId} - {r.Time} - {r.Type}");
	}
}
