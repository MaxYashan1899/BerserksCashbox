using System;
using System.Linq;

namespace CHRBerserk.BerserksCashbox
{
    class CashBoxDatabaseOperation
    {
        enum MonthName { январь = 1, февраль, март, апрель, май, июнь, июль, август, сентябрь, октябрь, ноябрь, декабрь };
        public int GetOtherExpenses(CashBoxOperation cashBoxOperation)
        {
            var otherExpenses = ParseInt("Введите сумму других расходов");

            Console.WriteLine($"Сумма других расходов {otherExpenses} грн");
            var newOperation = new CashBoxOperation {OtherExpenses = otherExpenses, CurrentDate = DateTime.Now };

            using (var db = new CashBoxDatabase())
            {
                db.CashBoxOperations.Add(newOperation);
                db.SaveChanges();
            }
            return cashBoxOperation.OtherExpenses;
        }
        public int GetOtherIncomes(CashBoxOperation cashBoxOperation)
        {
            var otherIncomes = ParseInt("Введите сумму других доходов");
          
            Console.WriteLine($"Сумма других доходов {otherIncomes} грн");
            var newOperation = new CashBoxOperation { OtherIncomes = otherIncomes, CurrentDate = DateTime.Now };

            using (var db = new CashBoxDatabase())
            {
                db.CashBoxOperations.Add(newOperation);
                db.SaveChanges();
            }
            return cashBoxOperation.OtherIncomes;
        }
        public int CommunityHouseRentalPayment(CashBoxOperation cashBoxOperation, BerserkMembersDatabaseInfo databaseMonthInfo)
        {
            int monthRentalSum = 800;
            int totalRentalDebtSum = monthRentalSum * (databaseMonthInfo.MonthDifference(DateTime.Now)+1);
         
            int communityHouseRentalPayment = ParseInt("Введите суму оплаты за аренду общинного дома");
            
            var newOperation = new CashBoxOperation { CommunityHouseRental = communityHouseRentalPayment, CurrentDate = DateTime.Now };

            using (var db = new CashBoxDatabase())
            {
                db.CashBoxOperations.Add(newOperation);
                db.SaveChanges();

                var communityHouseRentalPaymentSum = db.CashBoxOperations.Sum(p => p.CommunityHouseRental);
                RentalPaymentsReport(communityHouseRentalPaymentSum, totalRentalDebtSum);
            }
            return cashBoxOperation.CommunityHouseRental;
        }
        public int WorkshopRentalPayment(CashBoxOperation cashBoxOperation, BerserkMembersDatabaseInfo databaseMonthInfo)
        {
            int monthRentalSum = 1000;
            int totalRentalDebtSum = monthRentalSum * (databaseMonthInfo.MonthDifference(DateTime.Now)+1);

            int workshopRentalPayment = ParseInt("Введите суму оплаты за аренду мастерской");

            var newOperation = new CashBoxOperation { WorkshopRental = workshopRentalPayment, CurrentDate = DateTime.Now};

            using (var db = new CashBoxDatabase())
            {
                db.CashBoxOperations.Add(newOperation);
                db.SaveChanges();

                var workshopRentalPaymentSum = db.CashBoxOperations.Sum(p => p.WorkshopRental);
                RentalPaymentsReport(workshopRentalPaymentSum, totalRentalDebtSum);
            }
            return cashBoxOperation.WorkshopRental;
        }

        private static int ParseInt(string sum)
        {
            while (true)
            {
                Console.WriteLine($"{sum}:");
                if (int.TryParse(Console.ReadLine(), out int value) && value >= 0 )
                    return value;
                else
                    Console.WriteLine("Неверный формат введенных данных");
            }
        }
        private static void RentalPaymentsReport(int rentalPaymentsSum, int rentalDebtSum)
        {
            if (rentalPaymentsSum == rentalDebtSum)
                Console.WriteLine($"Аренда общинного дома за {(MonthName)(DateTime.Now.Month)} оплачена");
            else if (rentalPaymentsSum < rentalDebtSum)
            {
                int difference = rentalDebtSum - rentalPaymentsSum;
                Console.WriteLine($"Оплачена не полная сума. Долг за аренду общинного дома за {(MonthName)(DateTime.Now.Month)} составляет {difference} грн.");
            }
            else
            {
                int difference = rentalPaymentsSum - rentalDebtSum;
                Console.WriteLine($"Переплата за аренду общинного дома за {(MonthName)(DateTime.Now.Month)} на {difference} грн");
            }
        }
    }
}
//  поменять дни на месяца
