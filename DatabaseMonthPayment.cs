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
                    //item.MoneyBalance = item.CurrentPayment - item.CurrentDebt;
                    //item.CurrentPayment = item.CurrentPayment;
                }
                db.SaveChanges();
              
                Console.WriteLine();
                Console.WriteLine("Задолженность по людям:");
              
                berserkMembers = db.BerserkMembers.ToList();
                var uniqueBerserksName = berserkMembers.GroupBy(n => n.BerserksName).Select(m => m.FirstOrDefault());
                
                foreach (var item in uniqueBerserksName)
                {
                    var monthPaymentsSum = db.BerserkMembers.Where(d => d.CurrentData.Day == DateTime.Now.Day).Where(n=>n.BerserksName == item.BerserksName).Sum(s => s.CurrentPayment);
                    var monthPaymentBalance = monthPaymentsSum - item.CurrentDebt; 
                    Console.WriteLine($"Имя:{item.BerserksName}\t - долг:{item.CurrentDebt} грн. \t - взнос:{monthPaymentsSum} грн. \t баланс: {monthPaymentBalance} грн.");
                }
            }
        }
        
       public int MonthDifference(DateTime currentData)
        {
            int monthDifference = 0;
            using (var db = new BerserkMembersDatabase())
            {
                monthDifference = (currentData.Day - db.BerserkMembers.Find(1).StartData.Day) + 12 * (currentData.Year - db.BerserkMembers.Find(1).StartData.Year);
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
        public int PreviousMonthPaymentsSum(DatabaseMonthPayment databaseMonthPayment)
        {
            var monthPaymentsSum = 0;
            using (var db = new BerserkMembersDatabase())
            {
                monthPaymentsSum = db.BerserkMembers.Where(d => d.CurrentData.Day < DateTime.Now.Day).Sum(p => p.CurrentPayment);
            }
             return monthPaymentsSum;
        }
    }
    
}

//  указать месяц (ноябрь, декабрь)  - касса за декабрь

// поменять дни на месяца в рассчетах данных
// парсинг
// проверки