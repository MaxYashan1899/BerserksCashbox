using System;

namespace CHRBerserk.BerserksCashbox
{
    public class CashBox
    {
        public int Id { get; set; }
        /// <summary>
        /// оплата аренды мастерской
        /// </summary>
        public int WorkshopRental { get; set; }
        /// <summary>
        /// оплата аренды общинного дома
        /// </summary>
        public int CommunityHouseRental { get; set; }
        /// <summary>
        /// другие расходы
        /// </summary>
        public int OtherExpenses { get; set; }
        /// <summary>
        /// другие доходы
        /// </summary>
        public int OtherIncomes { get; set; }
        /// <summary>
        /// базовая сумма в клубной казне
        /// </summary>
        public int BaseCashBoxSum { get; set; }
        /// <summary>
        /// дата текущей операции
        /// </summary>
        public DateTime CurrentDate { get; set; }

        public CashBox() {  }
     }
}
