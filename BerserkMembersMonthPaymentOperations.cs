using System;
using System.Collections.Generic;
using System.Linq;

namespace CHRBerserk.BerserksCashbox
{
  public class BerserkMembersMonthPaymentOperations
    {
        /// <summary>
        /// получаем текущий платеж
        /// </summary>
        /// <param name="berserkMembers"></param>
        public void GetMonthPayment(List<BerserkMembers> berserkMembers)
        {
            Console.WriteLine("Введите имя плательщика:");
            string name = Console.ReadLine();
            using (var db = new BerserkMembersDatabase())
            {
                if (db.BerserkMembers.Any(n => n.BerserksName == name))
                {
                    Console.WriteLine("Введите сумму платежа:");
                    if (int.TryParse(Console.ReadLine(), out int paymentSum) && paymentSum > 0)
                        MonthPaymentToDatabase(name, paymentSum);
                    else
                        Console.WriteLine("Неверный формат введенных данных");
                }
                else
                {
                    Console.WriteLine("Введено неправильное имя");
                }
            }
        }

        /// <summary>
        /// запись клубного взноса в казну 
        /// </summary>
        /// <param name="name">имя члена клуба</param>
        /// <param name="paymentSum">сумма взноса</param>
        public void MonthPaymentToDatabase(string name, int paymentSum)
        {
               using (var db = new BerserkMembersDatabase())
            {
                var firstRecordCurrentMember = db.BerserkMembers.Where(n => n.BerserksName == name).First();
                var newPaymentOperation = new BerserkMembers { BerserksName = name, CurrentPayment = paymentSum, CurrentDate = DateTime.Now, StartDate = firstRecordCurrentMember.StartDate };
                db.BerserkMembers.Add(newPaymentOperation);
                Console.WriteLine($"{name} внес {paymentSum} грн.");
                db.SaveChanges();
            }
        }
        /// <summary>
        /// сумма всех взносов за месяц
        /// </summary>
        /// <returns>сумма всех взносов за месяц</returns>
        public int MonthPaymentsSum()
        {
            var totalMonthPaymentsSum = 0;
            using (var db = new BerserkMembersDatabase())
            {
                totalMonthPaymentsSum = db.BerserkMembers
                                  .Where(y => y.CurrentDate.Year == DateTime.Now.Year)
                                  .Where(d => d.CurrentDate.Day == DateTime.Now.Day)
                                  .Sum(p => p.CurrentPayment);
            }
            return totalMonthPaymentsSum;
        }
        /// <summary>
        /// сумма всех взносов за предыдущий месяц
        /// </summary>
        /// <returns>сумма всех взносов за предыдущий месяц</returns>
        public int PreviousMonthPaymentsSum()
        {
            var totalMonthPaymentsSum = 0;
            using (var db = new BerserkMembersDatabase())
            {
                totalMonthPaymentsSum = db.BerserkMembers
                                  .Where(n => n.CurrentDate.Year < DateTime.Now.Year)
                                  .Sum(p => p.CurrentPayment)
                                  + db.BerserkMembers
                                  .Where(n => n.CurrentDate.Year == DateTime.Now.Year)
                                  .Where(d => d.CurrentDate.Day < DateTime.Now.Day)
                                  .Sum(p => p.CurrentPayment);
            }
            return totalMonthPaymentsSum;
        }
    }
}


