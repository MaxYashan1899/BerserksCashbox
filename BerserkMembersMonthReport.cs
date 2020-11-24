using System;
using System.Collections.Generic;
using System.Linq;

namespace CHRBerserk.BerserksCashbox
{
   public class BerserkMembersMonthReport
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
                Console.WriteLine("№  Член клуба \t   Долг(начало месяца)" +
                               "\t   Взнос(тек.месяц) \t  Баланс(тек. месяц)");

                GetTotalDebt();

                berserkMembers = db.BerserkMembers.ToList();
                // выборка уникальных имен членов клуба
                var uniqueBerserksMember = berserkMembers.GroupBy(n => n.BerserksName)
                                                       .Select(m => m.FirstOrDefault());
                var count = 0;
                foreach (var item in uniqueBerserksMember)
                {
                    var memberMonthPaymentsSum = db.BerserkMembers
                                           .Where(y => y.CurrentDate.Year == DateTime.Now.Year)
                                           .Where(d => d.CurrentDate.Day == DateTime.Now.Day)
                                           .Where(n => n.BerserksName == item.BerserksName)
                                           .Sum(p => p.CurrentPayment);
                   var memberPreviousMonthsPaymentsSum = db.BerserkMembers
                                          .Where(y => y.CurrentDate.Year < DateTime.Now.Year)
                                          .Where(n => n.BerserksName == item.BerserksName)
                                          .Sum(p => p.CurrentPayment)
                                          + db.BerserkMembers
                                          .Where(y => y.CurrentDate.Year == DateTime.Now.Year)
                                          .Where(d => d.CurrentDate.Day < DateTime.Now.Day)
                                          .Where(n => n.BerserksName == item.BerserksName)
                                          .Sum(p => p.CurrentPayment);
                    var currentDebt = item.TotalDebt - memberPreviousMonthsPaymentsSum;
                    item.MoneyBalance = -(currentDebt - memberMonthPaymentsSum);
                  
                    Console.WriteLine($"{++count}. {item.BerserksName}\t\t  {currentDebt} грн." +
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
                    // разница между теперешним месяцем и месяцем добавление члена клуба в казну
                    var monthDifference = (DateTime.Now.Day - item.StartDate.Day)
                                      + 12 * (DateTime.Now.Year - item.StartDate.Year);
                    item.TotalDebt = item.StartDebt * (monthDifference + 1);
                    }
                   db.SaveChanges();
                }
        }
    }
}

// поменять дни на месяца в рассчетах данных
