using System;
using System.Linq;

namespace CHRBerserk.BerserksCashbox
{
    class CashBoxReport
    {
        enum MonthName { январь = 1, февраль, март, апрель, май, июнь, июль, август, сентябрь, октябрь, ноябрь, декабрь };
        public void TotalSumInCashBox(BerserkMembersMonthPaymentOperations databaseMonthPayment, BerserkMembersDatabaseInfo databaseInfo, CashBoxOperation cashBoxOperation, CashBoxDatabaseOperation cashBoxDatabaseOperation)
        {
            using (var db = new CashBoxDatabase())
            {
                var monthDifference = cashBoxDatabaseOperation.MonthDifference(DateTime.Now);
                if (monthDifference > 0)
                {
                    var previousMonthCashBoxSum = PreviousMonthCashBoxSum(databaseMonthPayment, databaseInfo, cashBoxOperation, cashBoxOperation.BaseCashBoxSum);
                    CurrentMonthCashBoxSum(databaseMonthPayment, databaseInfo, cashBoxOperation, previousMonthCashBoxSum);
                }
                else
                    CurrentMonthCashBoxSum(databaseMonthPayment, databaseInfo, cashBoxOperation, cashBoxOperation.BaseCashBoxSum);
            }
        }

        public int CurrentMonthCashBoxSum(BerserkMembersMonthPaymentOperations databaseMonthPayment, BerserkMembersDatabaseInfo databaseInfo, CashBoxOperation cashBoxOperation, int baseCashBoxSum)
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
                var monthPaymentSum = databaseMonthPayment.MonthPaymentsSum();

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
        public int PreviousMonthCashBoxSum(BerserkMembersMonthPaymentOperations databaseMonthPayment, BerserkMembersDatabaseInfo databaseInfo, CashBoxOperation cashBoxOperation, int baseCashBoxSum)
        {
            var currentSumInCashBox = 0;

            using (var db = new CashBoxDatabase())
            {
                var otherIncomesSum = db.CashBoxOperations
                                    .Where(n => n.CurrentDate.Year < DateTime.Now.Year)
                                    .Where(n => n.CurrentDate.Day < DateTime.Now.Day + 12)
                                    .Sum(s => s.OtherIncomes)
                                    + db.CashBoxOperations
                                    .Where(n => n.CurrentDate.Year == DateTime.Now.Year)
                                    .Where(n => n.CurrentDate.Day < DateTime.Now.Day)
                                    .Sum(s => s.OtherIncomes);
                var otherExpencesSum = db.CashBoxOperations
                                      .Where(n => n.CurrentDate.Year < DateTime.Now.Year)
                                      .Where(n => n.CurrentDate.Day < DateTime.Now.Day + 12)
                                      .Sum(s => s.OtherExpenses)
                                      + db.CashBoxOperations
                                      .Where(n => n.CurrentDate.Year == DateTime.Now.Year)
                                      .Where(n => n.CurrentDate.Day < DateTime.Now.Day)
                                      .Sum(s => s.OtherExpenses);
                var workshopRentalSum = db.CashBoxOperations
                                     .Where(n => n.CurrentDate.Year < DateTime.Now.Year)
                                     .Where(n => n.CurrentDate.Day < DateTime.Now.Day + 12)
                                     .Sum(s => s.WorkshopRental)
                                     + db.CashBoxOperations
                                     .Where(n => n.CurrentDate.Year == DateTime.Now.Year)
                                     .Where(n => n.CurrentDate.Day < DateTime.Now.Day)
                                     .Sum(s => s.WorkshopRental);
                var communityHouseRentalSum = db.CashBoxOperations
                                     .Where(n => n.CurrentDate.Year < DateTime.Now.Year)
                                     .Where(n => n.CurrentDate.Day < DateTime.Now.Day + 12)
                                     .Sum(s => s.CommunityHouseRental)
                                     + db.CashBoxOperations
                                     .Where(n => n.CurrentDate.Year == DateTime.Now.Year)
                                     .Where(n => n.CurrentDate.Day < DateTime.Now.Day)
                                     .Sum(s => s.CommunityHouseRental);
                var monthPaymentSum = databaseMonthPayment.PreviousMonthPaymentsSum();

                currentSumInCashBox = baseCashBoxSum + otherIncomesSum + monthPaymentSum
                                      - otherExpencesSum - workshopRentalSum - communityHouseRentalSum;

                Console.WriteLine($"Баланс по кассе за {(MonthName)(DateTime.Now.Month - 1)}: {currentSumInCashBox} грн.");
                Console.WriteLine($"\tКасса на начало месяца: {baseCashBoxSum} грн.  \tРасходы на мастерскую: {workshopRentalSum} грн.");
                Console.WriteLine($"\tОбщая сумма взносов: {monthPaymentSum} грн. \t\tРасходы на общинный дом: {communityHouseRentalSum} грн.");
                Console.WriteLine($"\tСумма доходов: {otherIncomesSum} грн. \t\tСумма расходов: {otherExpencesSum} грн.");
                Console.WriteLine();
                // удалить консольный вывод за предыдущий месяц
            }
            return currentSumInCashBox;
        }
    }
}
