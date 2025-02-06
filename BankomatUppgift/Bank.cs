
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
}
