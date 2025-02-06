using System.Globalization;

namespace BankomatUppgift;
internal class Receipt {
	public enum Types {
		None,
		LookUp,
		Deposit,
		Withdrawal
	}

	public Guid AccountId { get; set; }
	public Guid TransactionId { get; set; }
	public Types Type { get; set; }
	public decimal Amount { get; set; }
	public decimal Balance { get; set; }
	public DateTime Time { get; set; }

	public override string ToString() {
		return $"{AccountId} {Type} ({Amount.ToString("C", new CultureInfo("sv-SE"))}) - {Balance.ToString("C", new CultureInfo("sv-SE"))}";
	}

	public Receipt() { }
	public Receipt(Guid accountId, Types type, decimal amount, decimal balance) {
		TransactionId = Guid.NewGuid();
		AccountId = accountId;
		Type = type;
		Amount = amount;
		Balance = balance;
		Time = DateTime.UtcNow;
	}
	public Receipt(Guid transactionId, Guid accountId, Types type, decimal amount, decimal balance) {
		TransactionId = transactionId;
		AccountId = accountId;
		Type = type;
		Amount = amount;
		Balance = balance;
		Time = DateTime.UtcNow;
	}
}
