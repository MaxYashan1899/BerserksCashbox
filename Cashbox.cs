using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BerserksCashbox
{
    public class CashBoxOperation
    {
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
