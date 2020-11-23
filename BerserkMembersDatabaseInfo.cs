using System;
using System.Collections.Generic;
using System.Linq;

namespace CHRBerserk.BerserksCashbox
{
   public class BerserkMembersDatabaseInfo
    {
        enum MonthName { январь = 1, февраль, март, апрель, май, июнь, июль, август, сентябрь, октябрь, ноябрь, декабрь };

       public void DatabaseInfo(List<BerserkMembers> berserkMembers)
        {
               using (var db = new BerserkMembersDatabase())
            {
                var members = db.BerserkMembers;
                var monthDifference = MonthDifference(DateTime.Now);

                foreach (var item in members)
                {
                    item.CurrentDebt = item.StartDebt * (monthDifference + 1);
                }
                db.SaveChanges();
              
                Console.WriteLine();
                Console.WriteLine($"Задолженность по людям за {(MonthName)(DateTime.Now.Month)}:") ;
              
                berserkMembers = db.BerserkMembers.ToList();
                var uniqueBerserksName = berserkMembers.GroupBy(n => n.BerserksName)
                                                        .Select(m => m.FirstOrDefault());
                
                foreach (var item in uniqueBerserksName)
                {
                    var memberMonthPaymentsSum = db.BerserkMembers
                                           .Where(y => y.CurrentDate.Year == DateTime.Now.Year)
                                           .Where(d => d.CurrentDate.Day == DateTime.Now.Day)
                                           .Where(n=>n.BerserksName == item.BerserksName)
                                           .Sum(s => s.CurrentPayment);
                    var monthPaymentBalance = memberMonthPaymentsSum - item.CurrentDebt; 
                    Console.WriteLine($"Имя:{item.BerserksName}\t - долг:{item.CurrentDebt} грн." +
                     $" \t - взнос:{memberMonthPaymentsSum} грн. \t баланс: {monthPaymentBalance} грн.");
                }
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
                    monthDifference = (currentData.Day - item.StartDate.Day)
                                  + 12 * (currentData.Year - item.StartDate.Year);
                }
            }
            return monthDifference;
        }
    }
}

// поменять дни на месяца в рассчетах данных
