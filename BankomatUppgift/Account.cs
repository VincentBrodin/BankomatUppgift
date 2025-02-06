using System.Globalization;
using System.Text.Json.Serialization;

namespace BankomatUppgift;

internal class Account {
	[JsonInclude]
	public Guid Id { get; private set; }
	public string FirstName { get; set; } = string.Empty;
	public string LastName { get; set; } = string.Empty;
	public DateOnly BirthDay { get; set; }
	[JsonInclude]
	public DateTime CreatedDate { get; private set; }

	[JsonIgnore]
	public string FullName => $"{FirstName} {LastName}";

	[JsonIgnore]
	public int Age {
		get {
			DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);
			int years = currentDate.Year - BirthDay.Year;
			if(currentDate.Day < BirthDay.Day) {
				years--;
			}
			return years;
		}
	}
	[JsonInclude]
	// 256 bit om någon är rik :)
	public decimal Balance { get; private set; }

	[JsonIgnore]
	public string AmountString => Balance.ToString("C", new CultureInfo("sv-SE"));

	public Account() { }

	public Account(string firstName, string lastName, DateOnly birthDay, decimal amount = 0m) {
		Id = Guid.NewGuid();
		FirstName = firstName;
		LastName = lastName;
		Balance = amount;
		BirthDay = birthDay;
		CreatedDate = DateTime.Now;
	}

	internal void LookUp(Bank bank, bool animate = false) {
		Receipt receipt = new(Id, Receipt.Types.LookUp, 0m, Balance);
		bank.Receipts.Add(receipt);
		if(animate) {
			PlayAnimation();
		}
	}


	public bool Transfer(Bank bank, Account otherAccount, decimal amount, bool animate = false) {
		bool valid = TryTransfer(otherAccount, amount);
		if(valid) {
			Balance -= amount;
			otherAccount.Balance += amount;

			Receipt receipt = new(Id, Receipt.Types.Withdrawal, amount, Balance);
			bank.Receipts.Add(receipt);
			Receipt otherReceipt = new(receipt.TransactionId, otherAccount.Id, Receipt.Types.Deposit, amount, otherAccount.Balance);
			bank.Receipts.Add(otherReceipt);

		}
		if(animate) {
			PlayAnimation();
		}
		return valid;
	}
	public bool TryTransfer(Account otherAccount, decimal amount) {
		return TryWithdrawal(amount) && otherAccount.TryDeposit(amount);
	}



	public bool Deposit(Bank bank, decimal amount, bool animate = false) {
		bool valid = TryDeposit(amount);
		if(valid) {
			Balance += amount;
			Receipt receipt = new(Id, Receipt.Types.Deposit, amount, Balance);
			bank.Receipts.Add(receipt);
		}
		if(animate) {
			PlayAnimation();
		}
		return valid;
	}

	public bool TryDeposit(decimal amount) {
		return 0m < amount;
	}


	public bool Withdrawal(Bank bank, decimal amount, bool animate = false) {
		bool valid = TryWithdrawal(amount);

		if(valid) {
			Balance -= amount;
			Receipt receipt = new(Id, Receipt.Types.Withdrawal, amount, Balance);
			bank.Receipts.Add(receipt);
		}
		if(animate) {
			PlayAnimation();
		}
		return valid;
	}
	public bool TryWithdrawal(decimal amount) {
		return 0m < amount && amount <= Balance;
	}


	public void PlayAnimation() {
		(int left, int top) = Console.GetCursorPosition();
		Console.WriteLine($"[{new string('-', 50)}]");
		Tools.SetPosition(0, top + 1);
		left = 1;
		for(int i = 0; i < 50; i++) {
			Thread.Sleep(10);
			Tools.SetPosition(left, top);
			Console.Write("#");
			Tools.SetPosition(0, top + 1);
			left++;
		}
		Tools.SetPosition(0, top + 1);
	}

}
