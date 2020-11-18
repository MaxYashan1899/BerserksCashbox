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
                int paymentSum = int.Parse(Console.ReadLine());
                MonthPaymentDatabase( name, paymentSum);
            }
            else
            {
                Console.WriteLine("Exception: неправильно введено имя");
            }
        }

        public void MonthPaymentDatabase(string name, int paymentSum)
        {
               using (var db = new BerserkMembersDatabase())
            {
               
                var currentMemberName = db.BerserkMembers.FirstOrDefault(n => n.BerserksName == name);
                //var newPaymentOperation = new BerserkMembers { BerserksName = name, CurrentPayment = paymentSum, CurrentData = DateTime.Now, StartData = DateTime.Now};
                //db.BerserkMembers.Add(newPaymentOperation);
                currentMemberName.CurrentPayment += paymentSum;
                currentMemberName.CurrentData = DateTime.Now;
                Console.WriteLine($"{name} внес {paymentSum} грн.");
                db.SaveChanges();
            }
        }
   }
}
  // парсинг
  // проверки

