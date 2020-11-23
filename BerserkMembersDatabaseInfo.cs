using System;
using System.Collections.Generic;
using System.Linq;

namespace CHRBerserk.BerserksCashbox
{
   public class BerserkMembersDatabaseInfo
    {
        enum MonthName { январь = 1, февраль, март, апрель, май, июнь, июль, август, сентябрь, октябрь, ноябрь, декабрь };

        /// <summary>
        /// месячный отчет по клубным взносам
        /// </summary>
        /// <param name="berserkMembers">члены клуба</param>
       public void MembersPaymentsMonthReport(List<BerserkMembers> berserkMembers)
        {
               using (var db = new BerserkMembersDatabase())
            {
                Console.WriteLine();
                Console.WriteLine($"Задолженность по людям за {(MonthName)(DateTime.Now.Month)}:");

                GetTotalDebt();
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
        /// <summary>
        /// общий долг члена клуба
        /// </summary>
        public void GetTotalDebt()
        {
            using (var db = new BerserkMembersDatabase())
            {
                var members = db.BerserkMembers;
                var monthDifference = MonthDifference();

                foreach (var item in members)
                {
                    item.CurrentDebt = item.StartDebt * (monthDifference + 1);
                }
                db.SaveChanges();
            }
        }

        /// <summary>
        /// разница между первой и последней операцией члена клуба (в месяцах)
        /// </summary>
        /// <returns>разница между первой и последней операцией члена клуба (в месяцах)</returns>
        public int MonthDifference()
        {
            int monthDifference = 0;
            using (var db = new BerserkMembersDatabase())
            {
                var members = db.BerserkMembers;
                foreach (var item in members)
                {
                    monthDifference = (DateTime.Now.Day - item.StartDate.Day)
                                  + 12 * (DateTime.Now.Year - item.StartDate.Year);
                }
            }
            return monthDifference;
        }
    }
}
// разделить на методы + комментарии
// поменять дни на месяца в рассчетах данных
