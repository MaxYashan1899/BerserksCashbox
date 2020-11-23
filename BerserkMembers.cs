using System;

namespace CHRBerserk.BerserksCashbox
{
    public class BerserkMembers
    {
        public int Id { get; set; }
        public string BerserksName { get; set; }
        public int CurrentDebt { get; set; }
        public int StartDebt { get; set; }
        public int CurrentPayment { get; set;}
        public DateTime CurrentDate { get; set; }
        public DateTime StartDate { get; set; }
        public int MoneyBalance { get; set; }

        public BerserkMembers()
        { }

    }
}

