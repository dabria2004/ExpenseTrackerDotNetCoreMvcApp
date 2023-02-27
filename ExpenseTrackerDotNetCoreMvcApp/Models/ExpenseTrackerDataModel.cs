using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseTrackerDotNetCoreMvcApp.Models
{
    [Table("Tbl_ExpenseTracker")]
    public class ExpenseTrackerDataModel
    {
        [Key]
        public int Id { get; set; }
        public string Description { get; set; }
        public string TransactionType { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }

    }
}
