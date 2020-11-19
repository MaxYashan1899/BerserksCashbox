using System;
using System.Collections.Generic;
using System.Linq;

namespace BerserksCashbox
{
  public class MonthPaymentOperations
    {
    
        //enum Month
        //{
        //    NovemberPayments = 1,
        //    December = 2,
        //    January = 3
        // }
 
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
                var newPaymentOperation = new BerserkMembers { BerserksName = name, CurrentPayment = paymentSum, CurrentData = DateTime.Now, StartData = DateTime.Now };
                db.BerserkMembers.Add(newPaymentOperation);
                currentMemberName.CurrentData = DateTime.Now;

                // int[] monthPaymentsArray = new int[] {currentMemberName.NovemberPayments, currentMemberName.DecemberPayments,currentMemberName.JanuaryPayments};
                // //for (int i = 0; i < monthPaymentsArray.Length; i++)
                // //{
                // //    if (MonthDifference(currentMemberName.CurrentData) == monthPaymentsArray[])
                // //}
                //foreach (var item in monthPaymentsArray)
                // {
                //     if (MonthDifference(currentMemberName.CurrentData) == item)
                //     { 

                //     }
                // }


                //currentMemberName.CurrentPayment += paymentSum;
                Console.WriteLine($"{name} внес {paymentSum} грн.");
                db.SaveChanges();
            }
        }
        public int MonthDifference(DateTime currentData)
        {
            int monthDifference = 0;
            using (var db = new BerserkMembersDatabase())
            {
                var members = db.BerserkMembers;
                foreach (var item in members)
                {
                    monthDifference = (currentData.Month - item.StartData.Month) + 12 * (currentData.Year - item.StartData.Year);
                }
            }
            return monthDifference;

        }
    }
}
  // парсинг
  // проверки

