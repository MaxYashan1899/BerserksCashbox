using System;
using System.Linq;

namespace BerserksCashbox
{
    class DatabaseCashBoxOperation
    {
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
            //int monthRentalSum = 800;
            Console.WriteLine("Введите суму оплаты за аренду общинного дома:");
            int communityHouseRentalPayment = int.Parse(Console.ReadLine());
            
            var newOperation = new CashBoxOperation { CommunityHouseRental = communityHouseRentalPayment, CurrentData = DateTime.Now };

            using (var db = new CashBoxDatabase())
            {
                db.CashBoxOperations.Add(newOperation);
                db.SaveChanges();
                //if (sumOfPayments == monthRentalSum)
                //    Console.WriteLine("Аренда общинного дома оплачена");
                //else if (sumOfPayments < monthRentalSum)
                //{
                //    int difference = monthRentalSum - sumOfPayments;
                //    Console.WriteLine($"Оплачена не полная сума. Долг за аренду общинного дома составляет {difference} грн.");
                //}
                //else
                //{
                //    int difference = sumOfPayments - monthRentalSum;
                //    Console.WriteLine($"Переплата за аренду общинного дома на {difference} грн");
                //}

            }
            return cashBoxOperation.CommunityHouseRental;
        }
        public int WorkshopRentalPayment(CashBoxOperation cashBoxOperation)
        {
          
            //int monthRentalSum = 1000;
            Console.WriteLine("Введите суму оплаты за аренду мастерской:");
            int workshopRentalPayment = int.Parse(Console.ReadLine());
            var newOperation = new CashBoxOperation { WorkshopRental = workshopRentalPayment, CurrentData = DateTime.Now };

            using (var db = new CashBoxDatabase())
            {
                db.CashBoxOperations.Add(newOperation);
                db.SaveChanges();
                //if (sumOfPayment == monthRentalSum)
                //    Console.WriteLine("Аренда мастерской оплачена");
                //else if (sumOfPayment < monthRentalSum)
                //{
                //    int difference = monthRentalSum - sumOfPayment;
                //    Console.WriteLine($"Оплачена не полная сума. Долг за аренду мастерской составляет {difference} грн.");
                //}
                //else
                //{
                //    int difference = sumOfPayment - monthRentalSum;
                //    Console.WriteLine($"Переплата за аренду мастерской на {difference} грн");
                //}
            }
            return cashBoxOperation.WorkshopRental;
        }

        public int TotalSumInCashBox(DatabaseMonthPayment databaseMonthPayment, CashBoxOperation cashBoxOperation)
        {
           var currentSumInCashBox = 0;

           
            using (var db = new CashBoxDatabase())
            {
                var otherIncomesSum = db.CashBoxOperations.Where(n => n.CurrentData.Day == DateTime.Now.Day).Sum(s => s.OtherIncomes);
                var monthPaymentSum = databaseMonthPayment.MonthPaymentsSum(databaseMonthPayment);
                var otherExpencesSum = db.CashBoxOperations.Where(n => n.CurrentData.Day == DateTime.Now.Day).Sum(s => s.OtherExpenses);
                var workshopRentalSum = db.CashBoxOperations.Where(n => n.CurrentData.Day == DateTime.Now.Day).Sum(s => s.WorkshopRental);
                var communityHouseRental = db.CashBoxOperations.Where(n => n.CurrentData.Day == DateTime.Now.Day).Sum(s => s.CommunityHouseRental);
               
                currentSumInCashBox = cashBoxOperation.BaseCashBoxSum + otherIncomesSum + monthPaymentSum
                                      - otherExpencesSum - workshopRentalSum - communityHouseRental;

                var firstDatabaseElement = db.CashBoxOperations.Find(1);
                if (DateTime.Now.Month - firstDatabaseElement.CurrentData.Month > 1)
                    //   cashBoxOperation.BaseCashBoxSum = касса за предыдущий месяц
                    // возможно создать свойство касса за месяц и сохранять его в БД
                    Console.WriteLine();
                else
                    cashBoxOperation.BaseCashBoxSum = 2500;


                Console.WriteLine($"Баланс по кассе: {currentSumInCashBox} грн.");
                Console.WriteLine($"\tКасса на начало месяца: {cashBoxOperation.BaseCashBoxSum} грн.  \tРасходы на мастерскую: {workshopRentalSum} грн.");
                Console.WriteLine($"\tОбщая сумма взносов: {monthPaymentSum} грн. \t\tРасходы на ангар: {communityHouseRental} грн.");
                Console.WriteLine($"\tСумма доходов: {otherIncomesSum} грн. \t\tСумма расходов: {otherExpencesSum} грн.");
                Console.WriteLine();

            }
          return currentSumInCashBox;
        }

      

        // начисление по месяцам всех показателей (проверить изменение за текущий день)!!!
        // написать условия по оплате за ангар и мастерскую
        // изменять базовую сумму вначале месяца (доделать)
        // методы Парсинга

    }
}
