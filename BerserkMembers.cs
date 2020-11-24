using System;

namespace CHRBerserk.BerserksCashbox
{
    public class BerserkMembers
    {
        public int Id { get; set; }
        /// <summary>
        /// члены клуба
        /// </summary>
        public string BerserksName { get; set; }
        /// <summary>
        /// общий долг
        /// </summary>
        public int TotalDebt { get; set; }
        /// <summary>
        /// базовый клубный взнос(долг)
        /// </summary>
        public int StartDebt { get; set; }
        /// <summary>
        /// текущий взнос(оплата)
        /// </summary>
        public int CurrentPayment { get; set;}
        /// <summary>
        /// дата текущей операции
        /// </summary>
        public DateTime CurrentDate { get; set; }
        /// <summary>
        /// дата первой операции
        /// </summary>
        public DateTime StartDate { get; set; }
        /// <summary>
        /// баланс по задолженности в клуб
        /// </summary>
        public int MoneyBalance { get; set; }

        public BerserkMembers() { }
     }
}
