using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BerserksCashbox
{
    public class BerserkMembers
    {
        [Key]
        public int Id { get; set; }
        public string BerserksName { get; set; }
        public int CurrentDebt { get; set; }
        public int StartDebt { get; set; }
        public int CurrentPayment { get; set;}
        public DateTime CurrentDate { get; set; }
        public int MoneyBalance { get; set; }

        public BerserkMembers()
        { }

    }
}

