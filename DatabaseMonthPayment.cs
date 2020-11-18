using System;
using System.Collections.Generic;
using System.Linq;

namespace BerserksCashbox
{
   public class DatabaseMonthPayment
    {
        public void DatabaseInitialization(object berserk1, object berserk2, object berserk3, object berserk4)
        {
            using (var db = new BerserkMembersDatabase())
            {
                if (db.BerserkMembers.Count() == 0)
                {
                    db.BerserkMembers.AddRange((BerserkMembers)berserk1, (BerserkMembers)berserk2, (BerserkMembers)berserk3, (BerserkMembers)berserk4);
                    db.SaveChanges();
                }
            }
        }
       public void DatabaseInfo(List<BerserkMembers> berserkMembers)
        {
               using (var db = new BerserkMembersDatabase())
            {
               
                var members = db.BerserkMembers;
                var monthDifference = MonthDifference(DateTime.Now);
            
                foreach (var item in members)
                {
                    item.CurrentDebt = item.StartDebt * (monthDifference + 1);
                    item.MoneyBalance = item.CurrentPayment - item.CurrentDebt;
                }
                db.SaveChanges();
              
                Console.WriteLine();
                Console.WriteLine("Задолженность по людям:");
               
                foreach (var item in members)
                     Console.WriteLine($"Имя:{item.BerserksName}\t - долг:{item.CurrentDebt} грн. \t - взнос:{item.CurrentPayment} грн. \t баланс: {item.MoneyBalance} грн.");
                   
              }
        }
        
       public int MonthDifference(DateTime currentData)
        {
            int monthDifference=0;
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
        public int MonthPaymentsSum(DatabaseMonthPayment databaseMonthPayment)
        {
            var monthPaymentsSum = 0;
            using (var db = new BerserkMembersDatabase())
            {
               monthPaymentsSum = db.BerserkMembers.Where(d=>d.CurrentData.Day == DateTime.Now.Day).Sum(p => p.CurrentPayment);
            }
            
              return monthPaymentsSum;
        }
   }
}

//  найти ошибки с текущем долге
// баланс текущий долг за месяц - сумма взносов за месяц
// парсинг
// проверки