using System;
using System.Linq;

namespace CHRBerserk.BerserksCashbox
{
    class CashBoxReport
    {
        enum MonthName { январь = 1, февраль, март, апрель, май, июнь, июль, август, сентябрь, октябрь, ноябрь, декабрь };

        /// <summary>
        /// общая текущая сумма в казне
        /// </summary>
        /// <param name="monthPaymentOperations">месячные клубные взносы</param>
        /// <param name="cashBox">операции по кассе</param>
        /// <param name="cashBoxPaymentsOperation">операции с БД по кассе</param>
       public void TotalSumInCashBox(BerserkMembersMonthPaymentOperations monthPaymentOperations, CashBox cashBox, CashBoxPaymentsOperation cashBoxPaymentsOperation)
        {
            using (var db = new CashBoxDatabase())
            {
                var monthDifference = cashBoxPaymentsOperation.MonthDifference(DateTime.Now);
                if (monthDifference > 0)
                {
                    var previousMonthCashBoxSum = PreviousMonthCashBoxSum(monthPaymentOperations, cashBox.BaseCashBoxSum);
                    CurrentMonthCashBoxSum(monthPaymentOperations, previousMonthCashBoxSum);
                }
                else
                    CurrentMonthCashBoxSum(monthPaymentOperations, cashBox.BaseCashBoxSum);
            }
        }

        /// <summary>
        /// Сумма по казне за текущей месяц
        /// </summary>
        /// <param name="monthPaymentOperations">месячные клубные взносы</param>
        /// <param name="baseCashBoxSum">базовая сумма по клубной казне</param>
        /// <returns>сумма по казне за текущей месяц</returns>
        public int CurrentMonthCashBoxSum(BerserkMembersMonthPaymentOperations monthPaymentOperations, int baseCashBoxSum)
        {
            var currentSumInCashBox = 0;

            using (var db = new CashBoxDatabase())
            {
                var otherIncomesSum = db.CashBoxOperations
                                     .Where(n => n.CurrentDate.Year == DateTime.Now.Year)
                                     .Where(n => n.CurrentDate.Day == DateTime.Now.Day)
                                     .Sum(s => s.OtherIncomes);
                var otherExpencesSum = db.CashBoxOperations
                                      .Where(n => n.CurrentDate.Year == DateTime.Now.Year)
                                      .Where(n => n.CurrentDate.Day == DateTime.Now.Day)
                                      .Sum(s => s.OtherExpenses);
                var workshopRentalSum = db.CashBoxOperations
                                      .Where(n => n.CurrentDate.Year == DateTime.Now.Year)
                                      .Where(n => n.CurrentDate.Day == DateTime.Now.Day)
                                      .Sum(s => s.WorkshopRental);
                var communityHouseRentalSum = db.CashBoxOperations
                                      .Where(n => n.CurrentDate.Year == DateTime.Now.Year)
                                      .Where(n => n.CurrentDate.Day == DateTime.Now.Day)
                                      .Sum(s => s.CommunityHouseRental);
                var monthPaymentSum = monthPaymentOperations.MonthPaymentsSum();

                currentSumInCashBox = baseCashBoxSum + otherIncomesSum + monthPaymentSum
                                      - otherExpencesSum - workshopRentalSum - communityHouseRentalSum;

                Console.WriteLine($"Баланс по кассе за {(MonthName)(DateTime.Now.Month)}: {currentSumInCashBox} грн.");
                Console.WriteLine($"\tКасса на начало месяца: {baseCashBoxSum} грн.  \tРасходы на мастерскую: {workshopRentalSum} грн.");
                Console.WriteLine($"\tОбщая сумма взносов: {monthPaymentSum} грн. \t\tРасходы на общинный дом: {communityHouseRentalSum} грн.");
                Console.WriteLine($"\tСумма доходов: {otherIncomesSum} грн. \t\tСумма расходов: {otherExpencesSum} грн.");
                Console.WriteLine();

            }
            return currentSumInCashBox;
        }

        /// <summary>
        /// Сумма по казне за предыдущий месяц
        /// </summary>
        /// <param name="monthPaymentOperations">месячные клубные взносы</param>
        /// <param name="baseCashBoxSum">базовая сумма по клубной казне</param>
        /// <returns>сумма по казне за предыдущий месяц</returns>
        public int PreviousMonthCashBoxSum(BerserkMembersMonthPaymentOperations monthPaymentOperations, int baseCashBoxSum)
        {
            var currentSumInCashBox = 0;

            using (var db = new CashBoxDatabase())
            {
                var otherIncomesSum = db.CashBoxOperations
                                    .Where(n => n.CurrentDate.Year < DateTime.Now.Year)
                                    .Sum(s => s.OtherIncomes)
                                    + db.CashBoxOperations
                                    .Where(n => n.CurrentDate.Year == DateTime.Now.Year)
                                    .Where(n => n.CurrentDate.Month < DateTime.Now.Month)
                                    .Sum(s => s.OtherIncomes);
                var otherExpencesSum = db.CashBoxOperations
                                      .Where(n => n.CurrentDate.Year < DateTime.Now.Year)
                                      .Sum(s => s.OtherExpenses)
                                      + db.CashBoxOperations
                                      .Where(n => n.CurrentDate.Year == DateTime.Now.Year)
                                      .Where(n => n.CurrentDate.Month < DateTime.Now.Month)
                                      .Sum(s => s.OtherExpenses);
                var workshopRentalSum = db.CashBoxOperations
                                     .Where(n => n.CurrentDate.Year < DateTime.Now.Year)
                                     .Sum(s => s.WorkshopRental)
                                     + db.CashBoxOperations
                                     .Where(n => n.CurrentDate.Year == DateTime.Now.Year)
                                     .Where(n => n.CurrentDate.Month < DateTime.Now.Month)
                                     .Sum(s => s.WorkshopRental);
                var communityHouseRentalSum = db.CashBoxOperations
                                     .Where(n => n.CurrentDate.Year < DateTime.Now.Year)
                                     .Sum(s => s.CommunityHouseRental)
                                     + db.CashBoxOperations
                                     .Where(n => n.CurrentDate.Year == DateTime.Now.Year)
                                     .Where(n => n.CurrentDate.Month < DateTime.Now.Month)
                                     .Sum(s => s.CommunityHouseRental);
                var monthPaymentSum = monthPaymentOperations.PreviousMonthPaymentsSum();

                currentSumInCashBox = baseCashBoxSum + otherIncomesSum + monthPaymentSum
                                      - otherExpencesSum - workshopRentalSum - communityHouseRentalSum;

                Console.WriteLine($"Баланс по кассе за {(MonthName)(DateTime.Now.Month - 1)}: {currentSumInCashBox} грн.");
                Console.WriteLine($"\tКасса на начало месяца: {baseCashBoxSum} грн.  \tРасходы на мастерскую: {workshopRentalSum} грн.");
                Console.WriteLine($"\tОбщая сумма взносов: {monthPaymentSum} грн. \t\tРасходы на общинный дом: {communityHouseRentalSum} грн.");
                Console.WriteLine($"\tСумма доходов: {otherIncomesSum} грн. \t\tСумма расходов: {otherExpencesSum} грн.");
                Console.WriteLine();
            }
            return currentSumInCashBox;
        }
    }
}
// удалить консольный вывод за предыдущий месяц