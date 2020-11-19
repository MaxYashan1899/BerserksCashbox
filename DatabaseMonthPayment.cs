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
       public void DatabaseInfo(List<BerserkMembers> berserkMembers, DatabaseMonthPayment databaseMonthPayment)
        {
               using (var db = new BerserkMembersDatabase())
            {
               
                var members = db.BerserkMembers;
                var monthDifference = MonthDifference(DateTime.Now);
                //var monthPaymentSum = MonthPaymentsSum(databaseMonthPayment);
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
                var max = berserkMembers.GroupBy(n => n.BerserksName).Select(m => m.FirstOrDefault());
                
                foreach (var item in max)
                {
                    var a = db.BerserkMembers.Where(n => n.CurrentData.Month == DateTime.Now.Month).Where(m=>m.BerserksName == item.BerserksName).Sum(s => s.CurrentPayment);
                    var b = a - item.CurrentDebt; 
                    Console.WriteLine($"Имя:{item.BerserksName}\t - долг:{item.CurrentDebt} грн. \t - взнос:{a} грн. \t баланс: {b} грн.");
                }
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
               monthPaymentsSum = db.BerserkMembers.Where(d=>d.CurrentData.Month == DateTime.Now.Month).Sum(p => p.CurrentPayment);
            }
            
              return monthPaymentsSum;
        }
   }
}


// общая сумма взносов, общий баланс по людям   - правильно назвать переменные  + проверки!!!

// парсинг
// проверки