using System;
using System.Collections.Generic;
using System.Linq;

namespace BerserksCashbox
{
  public class MonthPaymentOperations
    {
        public void GetMonthPayment(List<BerserkMembers> berserkMembers)
        {
            Console.WriteLine("Введите имя плательщика:");
            string name = Console.ReadLine();
            if (berserkMembers.Any(n=>n.BerserksName == name))
            {
                    Console.WriteLine("Введите сумму платежа:");
                    if (int.TryParse(Console.ReadLine(), out int paymentSum) && paymentSum > 0)
                        MonthPaymentDatabase(name, paymentSum);
                    else
                        Console.WriteLine("Неверный формат введенных данных");
            }
            else
            {
                Console.WriteLine("Введено неправильное имя");
            }
        }

        public void MonthPaymentDatabase(string name, int paymentSum)
        {
               using (var db = new BerserkMembersDatabase())
            {
                var newPaymentOperation = new BerserkMembers { BerserksName = name, CurrentPayment = paymentSum, CurrentDate = DateTime.Now };
                db.BerserkMembers.Add(newPaymentOperation);
                Console.WriteLine($"{name} внес {paymentSum} грн.");
                db.SaveChanges();
            }
        }
      
    }
}
  // проверки

