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
                Console.WriteLine("Член клуба \t - долг(начало месяца)" +
                               " \t - взнос(тек.месяц) \t баланс(тек. месяц)");

                GetTotalDebt();

                berserkMembers = db.BerserkMembers.ToList();
                var uniqueBerserksMember = berserkMembers.GroupBy(n => n.BerserksName)
                                                       .Select(m => m.FirstOrDefault());
                foreach (var item in uniqueBerserksMember)
                {
                    var memberMonthPaymentsSum = db.BerserkMembers
                                           .Where(y => y.CurrentDate.Year == DateTime.Now.Year)
                                           .Where(d => d.CurrentDate.Day == DateTime.Now.Day)
                                           .Where(n => n.BerserksName == item.BerserksName)
                                           .Sum(s => s.CurrentPayment);
                   var memberPreviousMonthsPaymentsSum = db.BerserkMembers
                                          .Where(n => n.CurrentDate.Year < DateTime.Now.Year)
                                          .Where(n => n.BerserksName == item.BerserksName)
                                          .Sum(p => p.CurrentPayment)
                                          + db.BerserkMembers
                                          .Where(n => n.CurrentDate.Year == DateTime.Now.Year)
                                          .Where(d => d.CurrentDate.Day < DateTime.Now.Day)
                                          .Where(n => n.BerserksName == item.BerserksName)
                                          .Sum(p => p.CurrentPayment);
                    var currentDebt = item.CurrentDebt - memberPreviousMonthsPaymentsSum;
                    item.MoneyBalance = -(currentDebt - memberMonthPaymentsSum);
                  
                    Console.WriteLine($"{item.BerserksName}\t\t  {currentDebt} грн." +
                     $" \t\t  {memberMonthPaymentsSum} грн. \t\t  {item.MoneyBalance} грн.");
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
                    foreach (var item in members)
                {
                    var monthDifference = (DateTime.Now.Day - item.StartDate.Day)
                                      + 12 * (DateTime.Now.Year - item.StartDate.Year);
                    item.CurrentDebt = item.StartDebt * (monthDifference + 1);
                    }
                   db.SaveChanges();
                }
        }
    }
}

// разделить на методы + комментарии
// поменять дни на месяца в рассчетах данных
