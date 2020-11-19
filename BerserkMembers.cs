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
        public DateTime StartData { get; set; }
        public DateTime CurrentData { get; set; }
        public int MoneyBalance { get; set; }

        //public int NovemberPayments { get; set; } = 11;
        //public int DecemberPayments { get; set; } = 12;
        //public int JanuaryPayments { get; set; } = 1;

        public BerserkMembers()
        { }

        internal static object Select(Func<object, object> p)
        {
            throw new NotImplementedException();
        }
    }
}

