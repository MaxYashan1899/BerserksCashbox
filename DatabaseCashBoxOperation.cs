using System;
using System.Linq;

namespace BerserksCashbox
{
    class DatabaseCashBoxOperation
    {
        enum MonthName { январь=1, февраль, март, апрель, май, июнь, июль, август, сентябрь, октябрь, ноябрь, декабрь};
        public int GetOtherExpenses(CashBoxOperation cashBoxOperation)
        {
            Console.WriteLine("Введите сумму других расходов:");
            var otherExpenses = int.Parse(Console.ReadLine());

            Console.WriteLine($"Сумма других расходов {otherExpenses} грн");
            var newOperation = new CashBoxOperation {OtherExpenses = otherExpenses, CurrentData = DateTime.Now };

            using (var db = new CashBoxDatabase())
            {
                db.CashBoxOperations.Add(newOperation);
                db.SaveChanges();
            }
            return cashBoxOperation.OtherExpenses;
        }
        public int GetOtherIncomes(CashBoxOperation cashBoxOperation)
        {
            Console.WriteLine("Введите сумму других доходов:");
            var otherIncomes = int.Parse(Console.ReadLine());
          
            Console.WriteLine($"Сумма других доходов {otherIncomes} грн");
            var newOperation = new CashBoxOperation { OtherIncomes = otherIncomes, CurrentData = DateTime.Now };

            using (var db = new CashBoxDatabase())
            {
                db.CashBoxOperations.Add(newOperation);
                db.SaveChanges();
            }
            return cashBoxOperation.OtherIncomes;
        }
        public int CommunityHouseRentalPayment(CashBoxOperation cashBoxOperation)
        {
            int monthRentalSum = 800;
            int totalMonthRentalSum = monthRentalSum * MonthDifference();
         
            Console.WriteLine("Введите суму оплаты за аренду общинного дома:");
            int communityHouseRentalPayment = int.Parse(Console.ReadLine());
            
            var newOperation = new CashBoxOperation { CommunityHouseRental = communityHouseRentalPayment, CurrentData = DateTime.Now };

            using (var db = new CashBoxDatabase())
            {
                db.CashBoxOperations.Add(newOperation);
                db.SaveChanges();
                var communityHouseRentalPaymentSum = db.CashBoxOperations.Sum(p => p.CommunityHouseRental);
                if (communityHouseRentalPaymentSum == totalMonthRentalSum)
                    Console.WriteLine($"Аренда общинного дома за {(MonthName)(DateTime.Now.Month)} оплачена");
                else if (communityHouseRentalPaymentSum < totalMonthRentalSum)
                {
                    int difference = totalMonthRentalSum - communityHouseRentalPaymentSum;
                    Console.WriteLine($"Оплачена не полная сума. Долг за аренду общинного дома за {(MonthName)(DateTime.Now.Month)} составляет {difference} грн.");
                }
                else
                {
                    int difference = communityHouseRentalPaymentSum - totalMonthRentalSum;
                    Console.WriteLine($"Переплата за аренду общинного дома за {(MonthName)(DateTime.Now.Month)} на {difference} грн");
                }

            }
            return cashBoxOperation.CommunityHouseRental;
        }
        public int WorkshopRentalPayment(CashBoxOperation cashBoxOperation)
        {

            int monthRentalSum = 1000;
            int totalMonthRentalSum = monthRentalSum * MonthDifference();
            Console.WriteLine("Введите суму оплаты за аренду мастерской:");
            int workshopRentalPayment = int.Parse(Console.ReadLine());
            var newOperation = new CashBoxOperation { WorkshopRental = workshopRentalPayment, CurrentData = DateTime.Now};

            using (var db = new CashBoxDatabase())
            {
                db.CashBoxOperations.Add(newOperation);
                db.SaveChanges();
                var workshopRentalPaymentSum = db.CashBoxOperations.Sum(p => p.WorkshopRental);
                if (workshopRentalPaymentSum == totalMonthRentalSum)
                    Console.WriteLine("Аренда мастерской за {(MonthName)(DateTime.Now.Month)} оплачена");
                else if (workshopRentalPaymentSum < totalMonthRentalSum)
                {
                    int difference = totalMonthRentalSum - workshopRentalPaymentSum;
                    Console.WriteLine($"Оплачена не полная сума. Долг за аренду мастерской за {(MonthName)(DateTime.Now.Month)} составляет {difference} грн.");
                }
                else
                {
                    int difference = workshopRentalPaymentSum - totalMonthRentalSum;
                    Console.WriteLine($"Переплата за аренду мастерской за {(MonthName)(DateTime.Now.Month)} на {difference} грн");
                }
            }
            return cashBoxOperation.WorkshopRental;
        }

        public void TotalSumInCashBox(DatabaseMonthPayment databaseMonthPayment, CashBoxOperation cashBoxOperation)
        {
             using (var db = new CashBoxDatabase())
            {
                var firstDatabaseElement = db.CashBoxOperations.Find(1);
                var monthDifference = (DateTime.Now.Day - firstDatabaseElement.CurrentData.Day) + 12 * (DateTime.Now.Year - firstDatabaseElement.CurrentData.Year);
                if (monthDifference > 0)
                {
                    var previousMonthCashBoxSum = PreviousMonthCashBoxSum(databaseMonthPayment, cashBoxOperation, cashBoxOperation.BaseCashBoxSum);
                    CurrentMonthCashBoxSum(databaseMonthPayment, cashBoxOperation, previousMonthCashBoxSum);
                }
                else
                {
                   CurrentMonthCashBoxSum(databaseMonthPayment, cashBoxOperation, cashBoxOperation.BaseCashBoxSum);
                }
             }
        }
       
        public int CurrentMonthCashBoxSum(DatabaseMonthPayment databaseMonthPayment, CashBoxOperation cashBoxOperation, int baseCashBoxSum)
        {
            var currentSumInCashBox = 0;

            using (var db = new CashBoxDatabase())
            {
                var otherIncomesSum = db.CashBoxOperations
                                     .Where(n => n.CurrentData.Year == DateTime.Now.Year)
                                     .Where(n => n.CurrentData.Day == DateTime.Now.Day)
                                     .Sum(s => s.OtherIncomes);
                var otherExpencesSum = db.CashBoxOperations
                                      .Where(n => n.CurrentData.Year == DateTime.Now.Year)
                                      .Where(n => n.CurrentData.Day == DateTime.Now.Day)
                                      .Sum(s => s.OtherExpenses);
                var workshopRentalSum = db.CashBoxOperations
                                      .Where(n => n.CurrentData.Year == DateTime.Now.Year)
                                      .Where(n => n.CurrentData.Day == DateTime.Now.Day)
                                      .Sum(s => s.WorkshopRental);
                var communityHouseRental = db.CashBoxOperations
                                      .Where(n => n.CurrentData.Year == DateTime.Now.Year)
                                      .Where(n => n.CurrentData.Day == DateTime.Now.Day)
                                      .Sum(s => s.CommunityHouseRental);
                var monthPaymentSum = databaseMonthPayment.MonthPaymentsSum(databaseMonthPayment);

                currentSumInCashBox = baseCashBoxSum + otherIncomesSum + monthPaymentSum
                                      - otherExpencesSum - workshopRentalSum - communityHouseRental;
                
                Console.WriteLine($"Баланс по кассе за {(MonthName)(DateTime.Now.Month)}: {currentSumInCashBox} грн.");
                Console.WriteLine($"\tКасса на начало месяца: {cashBoxOperation.BaseCashBoxSum} грн.  \tРасходы на мастерскую: {workshopRentalSum} грн.");
                Console.WriteLine($"\tОбщая сумма взносов: {monthPaymentSum} грн. \t\tРасходы на ангар: {communityHouseRental} грн.");
                Console.WriteLine($"\tСумма доходов: {otherIncomesSum} грн. \t\tСумма расходов: {otherExpencesSum} грн.");
                Console.WriteLine();
              
            }
            return currentSumInCashBox;
        }
        public int PreviousMonthCashBoxSum(DatabaseMonthPayment databaseMonthPayment, CashBoxOperation cashBoxOperation, int baseCashBoxSum)
        {
            var currentSumInCashBox = 0;

            using (var db = new CashBoxDatabase())
            {
                var otherIncomesSum = db.CashBoxOperations
                                    .Where(n => n.CurrentData.Year < DateTime.Now.Year)
                                    .Where(n => n.CurrentData.Day < DateTime.Now.Day+12)
                                    .Sum(s => s.OtherIncomes) 
                                    + db.CashBoxOperations
                                    .Where(n => n.CurrentData.Year == DateTime.Now.Year)
                                    .Where(n => n.CurrentData.Day < DateTime.Now.Day)
                                    .Sum(s => s.OtherIncomes);
               var otherExpencesSum = db.CashBoxOperations
                                     .Where(n => n.CurrentData.Year < DateTime.Now.Year)
                                     .Where(n => n.CurrentData.Day < DateTime.Now.Day + 12)
                                     .Sum(s => s.OtherExpenses)
                                     + db.CashBoxOperations
                                     .Where(n => n.CurrentData.Year == DateTime.Now.Year)
                                     .Where(n => n.CurrentData.Day < DateTime.Now.Day)
                                     .Sum(s => s.OtherExpenses);
                var workshopRentalSum = db.CashBoxOperations
                                     .Where(n => n.CurrentData.Year < DateTime.Now.Year)
                                     .Where(n => n.CurrentData.Day < DateTime.Now.Day + 12)
                                     .Sum(s => s.WorkshopRental)
                                     + db.CashBoxOperations
                                     .Where(n => n.CurrentData.Year == DateTime.Now.Year)
                                     .Where(n => n.CurrentData.Day < DateTime.Now.Day)
                                     .Sum(s => s.WorkshopRental);
                var communityHouseRental = db.CashBoxOperations
                                     .Where(n => n.CurrentData.Year < DateTime.Now.Year)
                                     .Where(n => n.CurrentData.Day < DateTime.Now.Day + 12)
                                     .Sum(s => s.CommunityHouseRental)
                                     + db.CashBoxOperations
                                     .Where(n => n.CurrentData.Year == DateTime.Now.Year)
                                     .Where(n => n.CurrentData.Day < DateTime.Now.Day)
                                     .Sum(s => s.CommunityHouseRental);
                var monthPaymentSum = databaseMonthPayment.PreviousMonthPaymentsSum(databaseMonthPayment);
              
                currentSumInCashBox = baseCashBoxSum + otherIncomesSum + monthPaymentSum
                                      - otherExpencesSum - workshopRentalSum - communityHouseRental;
             
                Console.WriteLine($"Баланс по кассе за {(MonthName)(DateTime.Now.Month-1)}: {currentSumInCashBox} грн.");
                Console.WriteLine($"\tКасса на начало месяца: {cashBoxOperation.BaseCashBoxSum} грн.  \tРасходы на мастерскую: {workshopRentalSum} грн.");
                Console.WriteLine($"\tОбщая сумма взносов: {monthPaymentSum} грн. \t\tРасходы на ангар: {communityHouseRental} грн.");
                Console.WriteLine($"\tСумма доходов: {otherIncomesSum} грн. \t\tСумма расходов: {otherExpencesSum} грн.");
                Console.WriteLine();
                // удалить консольный вывод за предыдущий месяц
            }
            return currentSumInCashBox;
        }


        public int MonthDifference()
        {
            var monthDifference = 0;
            using (var db = new CashBoxDatabase())
            {
                var firstDatabaseElement = db.CashBoxOperations.Find(1);
                monthDifference = (DateTime.Now.Day - firstDatabaseElement.CurrentData.Day)
                                    + 12 * (DateTime.Now.Year - firstDatabaseElement.CurrentData.Year);
            }
            return monthDifference;
        }
        //  поменять дни на месяца

       // методы Парсинга

    }
}
