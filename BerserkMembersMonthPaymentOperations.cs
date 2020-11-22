using System;
using System.Collections.Generic;
using System.Linq;

namespace CHRBerserk.BerserksCashbox
{
  public class BerserkMembersMonthPaymentOperations
    {
        public void GetMonthPayment(List<BerserkMembers> berserkMembers)
        {
            Console.WriteLine("Введите имя плательщика:");
            string name = Console.ReadLine();
            if (berserkMembers.Any(n=>n.BerserksName == name))
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

        public void MonthPaymentToDatabase(string name, int paymentSum)
        {
               using (var db = new BerserkMembersDatabase())
            {
                var newPaymentOperation = new BerserkMembers { BerserksName = name, CurrentPayment = paymentSum, CurrentDate = DateTime.Now };
                db.BerserkMembers.Add(newPaymentOperation);
                Console.WriteLine($"{name} внес {paymentSum} грн.");
                db.SaveChanges();
            }
        }
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
        public int PreviousMonthPaymentsSum()
        {
            var totalMonthPaymentsSum = 0;
            using (var db = new BerserkMembersDatabase())
            {
                totalMonthPaymentsSum = db.BerserkMembers
                                  .Where(n => n.CurrentDate.Year < DateTime.Now.Year)
                                  .Where(n => n.CurrentDate.Day < DateTime.Now.Day + 12)
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


