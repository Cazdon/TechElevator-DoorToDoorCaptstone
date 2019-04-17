using DoorToDoorLibrary.DatabaseObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoorToDoorWeb.Models
{
    public class TransactionsViewModel
    {
        public IList<TransactionItem> Transactions { get; set; }
        public AddTransactionViewModel AddTransaction { get; set; }
    }
}
