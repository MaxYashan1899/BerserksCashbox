using System;
using System.Linq;

namespace CHRBerserk.BerserksCashbox
{
    public class CashBoxPaymentsOperation
    {
        enum MonthName { январь = 1, февраль, март, апрель, май, июнь, июль, август, сентябрь, октябрь, ноябрь, декабрь };
       
        /// <summary>
        /// Получаем сумму других расходов с казны
        /// </summary>
        /// <param name="cashBox">операции по казне</param>
        /// <returns>другие расходы с казны</returns>
        public int GetOtherExpenses(CashBox cashBox)
        {
            var otherExpenses = ParseInt("Введите сумму других расходов");

            Console.WriteLine($"Сумма других расходов {otherExpenses} грн");
            var newOperation = new CashBox {OtherExpenses = otherExpenses, CurrentDate = DateTime.Now };

            using (var db = new CashBoxDatabase())
            {
                db.CashBoxOperations.Add(newOperation);
                db.SaveChanges();
            }
            return cashBox.OtherExpenses;
        }

        /// <summary>
        /// Получаем сумму других доходов в казну
        /// </summary>
        /// <param name="cashBox">операции по казне</param>
        /// <returns>другие доходы в казну</returns>
        public int GetOtherIncomes(CashBox cashBox)
        {
            var otherIncomes = ParseInt("Введите сумму других доходов");
          
            Console.WriteLine($"Сумма других доходов {otherIncomes} грн");
            var newOperation = new CashBox { OtherIncomes = otherIncomes, CurrentDate = DateTime.Now };

            using (var db = new CashBoxDatabase())
            {
                db.CashBoxOperations.Add(newOperation);
                db.SaveChanges();
            }
            return cashBox.OtherIncomes;
        }

        /// <summary>
        /// Получаем проплату за аренду общинного дома
        /// </summary>
        /// <param name="cashBox">операции по казне</param>
        /// <returns>проплата за аренду общинного дом</returns>
        public int CommunityHouseRentalPayment(CashBox cashBox)
        {
            int monthRentalSum = 800;
            int totalRentalDebtSum = monthRentalSum * (MonthDifference(DateTime.Now) + 1);

            int communityHouseRentalPayment = ParseInt("Введите суму оплаты за аренду общинного дома");
            
            var newOperation = new CashBox { CommunityHouseRental = communityHouseRentalPayment, CurrentDate = DateTime.Now };

            using (var db = new CashBoxDatabase())
            {
                db.CashBoxOperations.Add(newOperation);
                db.SaveChanges();

                var communityHouseRentalPaymentSum = db.CashBoxOperations.Sum(p => p.CommunityHouseRental);
                RentalPaymentsReport(communityHouseRentalPaymentSum, totalRentalDebtSum, "общинного дома");
            }
            return cashBox.CommunityHouseRental;
        }

        /// <summary>
        /// Получаем проплату за мастерской
        /// </summary>
        /// <param name="cashBox">операции по казне</param>
        /// <returns>проплата за аренду мастерской</returns>
        public int WorkshopRentalPayment(CashBox cashBox)
        {
            int monthRentalSum = 1000;
            int totalRentalDebtSum = monthRentalSum * (MonthDifference(DateTime.Now) + 1);

            int workshopRentalPayment = ParseInt("Введите суму оплаты за аренду мастерской");

            var newOperation = new CashBox { WorkshopRental = workshopRentalPayment, CurrentDate = DateTime.Now};

            using (var db = new CashBoxDatabase())
            {
                db.CashBoxOperations.Add(newOperation);
                db.SaveChanges();

                var workshopRentalPaymentSum = db.CashBoxOperations.Sum(p => p.WorkshopRental);
                RentalPaymentsReport(workshopRentalPaymentSum, totalRentalDebtSum, "мастерской");
            }
            return cashBox.WorkshopRental;
        }

        /// <summary>
        /// проверка введеных данных на корректность
        /// </summary>
        /// <param name="sum">введенные данные</param>
        /// <returns>корректно введенные данные</returns>
        public static int ParseInt(string sum)
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
        /// <summary>
        /// Сообщения о состоянии долга по оплате арендованых помещений
        /// </summary>
        /// <param name="rentalPaymentsSum">оплата арендованых помещений</param>
        /// <param name="rentalDebtSum">долг за аренду помещений</param>
        /// <param name="name">название помещения</param>
        private static void RentalPaymentsReport(int rentalPaymentsSum, int rentalDebtSum, string name)
        {
            if (rentalPaymentsSum == rentalDebtSum)
                Console.WriteLine($"Аренда {name} за {(MonthName)(DateTime.Now.Month)} оплачена");
            else if (rentalPaymentsSum < rentalDebtSum)
            {
                int difference = rentalDebtSum - rentalPaymentsSum;
                Console.WriteLine($"Оплачена не полная сума. Долг за аренду {name} за {(MonthName)(DateTime.Now.Month)} составляет {difference} грн.");
            }
            else
            {
                int difference = rentalPaymentsSum - rentalDebtSum;
                Console.WriteLine($"Переплата за аренду {name} за {(MonthName)(DateTime.Now.Month)} на {difference} грн");
            }
        }
        /// <summary>
        /// разница между первой и последней операцией члена клуба (в месяцах)
        /// </summary>
        /// <returns>разница между первой и последней операцией члена клуба (в месяцах)</returns>
        public int MonthDifference(DateTime currentData)
        {
            int monthDifference = 0;
            using (var db = new BerserkMembersDatabase())
            {
                monthDifference = (currentData.Day - db.BerserkMembers.Find(1).CurrentDate.Day)
                                  + 12 * (currentData.Year - db.BerserkMembers.Find(1).CurrentDate.Year);
            }
            return monthDifference;
        }
    }
}
//  поменять дни на месяца
