using System;
using System.ComponentModel.DataAnnotations;

namespace CHRBerserk.BerserksCashbox
{
    public class CashBoxOperation
    {
        [Key]
        public int Id { get; set; }
        public int WorkshopRental { get; set; }
        public int CommunityHouseRental { get; set; }
        public int OtherExpenses { get; set; }
        public int OtherIncomes { get; set; }
        public int BaseCashBoxSum { get; set; }
        public DateTime CurrentDate { get; set; }

        public CashBoxOperation() {  }
     }
}
